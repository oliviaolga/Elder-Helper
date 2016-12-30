using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace ElderHelperApplication.Model
{
    public static class DataModel
    {
        public static ObservableCollection<MyReminder> Reminders { get; private set; }

        /// <summary>
        /// Adds an reminder to the storage, and schedules the reminder in the notifications platform
        /// </summary>
        /// <param name="reminder"></param>
        /// <returns></returns>
        public static async Task AddReminder(MyReminder reminder)
        {
            try
            {
                // Generate Id for it
                reminder.Id = Guid.NewGuid();

                Reminders.Add(reminder);

                await Task.Run(async delegate
                {
                    await SaveRemindersAsync();

                    ReminderHelper.ScheduleReminder(reminder);
                });
            }

            catch { }
        }

        public static async Task DeleteReminder(MyReminder reminder)
        {
            try
            {
                Reminders.Remove(reminder);

                await Task.Run(async delegate
                {
                    await SaveRemindersAsync();

                    ReminderHelper.RemoveReminder(reminder);
                });
            }
            catch { }
        }

        private static Task _loadTask;

        /// <summary>
        /// Initial app start should call this, which loads the reminders
        /// </summary>
        /// <returns></returns>
        public static Task LoadAsync()
        {
            if (_loadTask == null)
            {
                _loadTask = GenerateLoadTask();
            }

            return _loadTask;
        }

        public static void EnsureAllScheduled()
        {
            foreach (var reminder in Reminders)
            {
                ReminderHelper.EnsureScheduled(reminder);
            }
        }

        private static async Task GenerateLoadTask()
        {
            try
            {
                await Task.Run(async delegate
                {
                    var file = await GetRemindersFileAsync();

                    try
                    {
                        using (Stream s = await file.OpenStreamForReadAsync())
                        {
                            MyReminder[] reminders = (MyReminder[])RemindersSerializer.ReadObject(s);

                            Reminders = new ObservableCollection<MyReminder>(reminders);
                        }
                    }
                    catch
                    {
                        await file.DeleteAsync();
                        Reminders = new ObservableCollection<MyReminder>();
                    }
                });
            }

            catch (Exception)
            {
                Reminders = new ObservableCollection<MyReminder>();
            }
        }

        private static async Task SaveRemindersAsync()
        {
            try
            {
                var reminders = Reminders.ToArray();

                var file = await GetRemindersFileAsync();

                using (Stream s = await file.OpenStreamForWriteAsync())
                {
                    RemindersSerializer.WriteObject(s, reminders);
                }
            }
            catch { }
        }

        private static DataContractSerializer RemindersSerializer = new DataContractSerializer(typeof(MyReminder[]));

        private static Task<StorageFile> GetRemindersFileAsync()
        {
            return ApplicationData.Current.LocalFolder.CreateFileAsync("Reminders.dat", CreationCollisionOption.OpenIfExists).AsTask();
        }
    }
}
