using ElderHelperApplication.Model;
using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Metadata;
using Windows.System.Profile;
using Windows.UI.Notifications;

namespace ElderHelperApplication
{
    public class ReminderHelper
    {
        private const int DAYS_IN_ADVANCE_TO_SCHEDULE = 5;
        public static string describe { get; set; }

        public static void ScheduleReminder(MyReminder reminder)
        {
            EnsureScheduled(reminder, checkForExisting: false);
        }

        public static void RemoveReminder(MyReminder reminder)
        {
            var notifier = ToastNotificationManager.CreateToastNotifier();
            var tag = GetTag(reminder);

            // Find all of the scheduled toasts for the reminder
            var scheduledNotifs = notifier.GetScheduledToastNotifications()
                .Where(i => i.Tag.Equals(tag));

            // Remove all of those from the schedule
            foreach (var n in scheduledNotifs)
            {
                notifier.RemoveFromSchedule(n);
            }
        }

        public static void EnsureScheduled(MyReminder reminder)
        {
            EnsureScheduled(reminder, checkForExisting: true);
        }

        private static void EnsureScheduled(MyReminder reminder, bool checkForExisting)
        {
            var notifier = ToastNotificationManager.CreateToastNotifier();

            IReadOnlyList<ScheduledToastNotification> existing = null;
            if (checkForExisting)
            {
                var tag = GetTag(reminder);
                existing = notifier.GetScheduledToastNotifications()
                    .Where(i => i.Tag.Equals(tag))
                    .ToList();
            }

            DateTime now = DateTime.Now;

            DateTime[] reminderTimes = GetReminderTimesForReminder(reminder);

            foreach (var time in reminderTimes)
            {
                if (time.AddSeconds(5) > now)
                {
                    // If the reminder isn't scheduled already
                    if (!checkForExisting || !existing.Any(i => i.DeliveryTime == time))
                    {
                        var scheduledNotif = GenerateReminderNotification(reminder, time);
                        notifier.AddToSchedule(scheduledNotif);
                    }
                }
            }
        }

        private static string GetTag(MyReminder reminder)
        {
            // Tag needs to be 16 chars or less, so hash the Id
            return reminder.Id.GetHashCode().ToString();
        }

        private static ScheduledToastNotification GenerateReminderNotification(MyReminder reminder, DateTime reminderTime)
        {
            // Using NuGet package Microsoft.Toolkit.Uwp.Notifications

            ToastContent content = new ToastContent()
            {
                Launch = "action=viewEvent&eventId=1983",
                Scenario = ToastScenario.Reminder,

                Visual = new ToastVisual()
                {
                    BindingGeneric = new ToastBindingGeneric()
                    {
                        Children =
                        {
                            new AdaptiveText()
                            {
                                Text = "Pengingat!"
                            },

                            new AdaptiveText()
                            {
                                Text = $"{reminder.Name}"
                            },

                            new AdaptiveText()
                            {
                                Text = $"{reminder.Describe}"
                            },

                            new AdaptiveText()
                            {
                                Text = reminderTime.ToString()
                            }
                        }
                    }
                },

                Actions = new ToastActionsSnoozeAndDismiss(),
            };

            #region custom_audio

            bool supportsCustomAudio = true;

            // If we're running on Desktop before Version 1511, do NOT include custom audio
            // since it was not supported until Version 1511, and would result in a silent toast.
            if (AnalyticsInfo.VersionInfo.DeviceFamily.Equals("Windows.Desktop")
                && !ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 2))
            {
                supportsCustomAudio = false;
            }

            if (supportsCustomAudio)
            {
                content.Audio = new ToastAudio()
                {
                    Src = new Uri(reminder.audio),
                    Loop = true,
                    Silent = false
                };
            }

            #endregion

            // We can easily enable Universal Dismiss by generating a RemoteId for the reminder that will be
            // the same on both devices. We'll just use the reminder delivery time. If an reminder on one device
            // has the same delivery time as an reminder on another device, it'll be dismissed when one of the
            // reminders is dismissed.
            string remoteId = (reminderTime.Ticks / 10000000 / 60).ToString(); // Minutes

            return new ScheduledToastNotification(content.GetXml(), reminderTime)
            {
                Tag = GetTag(reminder),

                // RemoteId is a 1607 feature, if you support older systems, use ApiInformation to check if property is present
                RemoteId = remoteId
            };
        }

        private static DateTime[] GetReminderTimesForReminder(MyReminder reminder)
        {
            if (reminder.IsOneTime())
            {
                return new DateTime[] { reminder.SingleFireTime };
            }
            //else if (reminder.IsOneMonth())
            //{
            //    int thisyear = DateTime.Now.Year;
            //    int thismonth = DateTime.Now.Month;
            //    int OneMonthDays = DateTime.DaysInMonth(thisyear, thismonth);
            //    DateTime today = DateTime.Today;
            //    List<DateTime> answer = new List<DateTime>();
            //    for (int i = 0; i < OneMonthDays; i++)
            //    {
            //        answer.Add(today.Add(reminder.TimeOfDay));
            //        today = today.AddDays(1);
            //    }

            //    return answer.ToArray();
            //}
            else
            {
                DateTime today = DateTime.Today;
                List<DateTime> answer = new List<DateTime>();
                for (int i = 0; i < DAYS_IN_ADVANCE_TO_SCHEDULE; i++)
                {
                    if (reminder.DaysOfWeek.Contains(today.DayOfWeek))
                    {
                        answer.Add(today.Add(reminder.TimeOfDay));
                    }

                    today = today.AddDays(1);
                }

                return answer.ToArray();
            }
        }
    }
}
