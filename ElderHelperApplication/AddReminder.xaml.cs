using ElderHelperApplication.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.ApplicationModel.Contacts;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.SpeechSynthesis;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ElderHelperApplication
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddReminder : Page
    {
        public AddReminder()
        {
            this.InitializeComponent();
        }

        public string filepicker { get; set; }

        private async void AddTone_Click(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation =
                Windows.Storage.Pickers.PickerLocationId.MusicLibrary;
            picker.FileTypeFilter.Add(".mp3");
            picker.FileTypeFilter.Add(".m4a");
            picker.FileTypeFilter.Add(".wma");
            picker.FileTypeFilter.Add(".wav");

            Windows.Storage.StorageFile file = await picker.PickSingleFileAsync();
            filepicker = file.Path;

            if (filepicker == null)
            {
                var messageDialog = new Windows.UI.Popups.MessageDialog("File lagu sudah dipilih.");
                await messageDialog.ShowAsync();
            }
        }

        private async void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            if ((TextBoxName.Text != null) && (DescriptionName.Text != null) && (filepicker != null))
            {
                string name = TextBoxName.Text;
                TimeSpan time = TimePickerTime.Time;
                bool onlyOnce = ComboBoxRepeats.SelectedIndex == 0;
                bool onlyOneMonth = ComboBoxRepeats.SelectedIndex == 2;
                string describe = DescriptionName.Text;

                var reminder = new MyReminder()
                {
                    Name = name,
                    TimeOfDay = time,
                    Describe = describe,
                    audio = filepicker
                };

                if (onlyOnce)
                {
                    if (time < DateTime.Now.TimeOfDay)
                    {
                        // If time for today has already passed, set it for tomorrow
                        reminder.SingleFireTime = DateTime.Today.AddDays(1).Add(time);
                    }
                    else
                    {
                        // Otherwise, set it for today at the time
                        reminder.SingleFireTime = DateTime.Today.Add(time);
                    }
                }
                //else if (onlyOneMonth)
                //{
                //    reminder.MonthFireTime = DateTime.Today.Add(time);
                //}
                else
                {
                    reminder.DaysOfWeek = new DayOfWeek[]
                    {
                    DayOfWeek.Sunday,
                    DayOfWeek.Monday,
                    DayOfWeek.Tuesday,
                    DayOfWeek.Wednesday,
                    DayOfWeek.Thursday,
                    DayOfWeek.Friday,
                    DayOfWeek.Saturday
                    };
                }

                await DataModel.AddReminder(reminder);

                Frame.GoBack();
            }
            else
            {
                var message = new Windows.UI.Popups.MessageDialog("Cek kembali input anda, pastikan tidak ada yang kosong.");
                await message.ShowAsync();
            }
        }

        private void ClearOutputs()
        {
            // Reset the output fields.
            DescriptionName.Text = "";
        }

        #region recording
        public RecordLibrary Library = new RecordLibrary();

        private void Record_Click(object sender, RoutedEventArgs e)
        {
            if (RecordLibrary.Recording)
            {
                Library.Stop();
                Record.Icon = new SymbolIcon(Symbol.Memo);
            }
            else
            {
                Library.Record();
                Record.Icon = new SymbolIcon(Symbol.Microphone);
            }
        }

        private async void Play_Click(object sender, RoutedEventArgs e)
        {
            await Library.Play(Dispatcher);
        }
        #endregion              

        #region find_contact

        private async void ReportContactResult(Contact contact)
        {
            if (contact != null)
            {
                DescriptionName.Text = AddReminder.GetContactResult(contact);
            }
            else
            {
                var messageDialog = new Windows.UI.Popups.MessageDialog("Tidak ada kontak yang dipilih. Silahkan pilih kontak kembali.");
                await messageDialog.ShowAsync();
            }
        }

        private static string GetContactResult(Contact contact)
        {
            var result = new StringBuilder();
            result.AppendFormat("Nama: {0}", contact.DisplayName);
            result.AppendLine();

            foreach (ContactPhone phone in contact.Phones)
            {
                result.AppendFormat("Nomor Telepon ({0}): {1}", phone.Kind, phone.Number);
                result.AppendLine();
            }

            return result.ToString();
        }

        private async void PickContactPhone()
        {
            ClearOutputs();

            // Ask the user to pick a contact phone number.
            var contactPicker = new ContactPicker();
            contactPicker.DesiredFieldsWithContactFieldType.Add(ContactFieldType.PhoneNumber);

            Contact contact = await contactPicker.PickContactAsync();

            ReportContactResult(contact);
        }
        #endregion

        #region text-to-speech
        private void Speak_Click(object sender, RoutedEventArgs e)
        {
            AudioforToast(TextBoxName.Text);
        }

        private static async void AudioforToast(string text)
        {
            var voices = SpeechSynthesizer.AllVoices;
            MediaElement media = new MediaElement();

            if (!String.IsNullOrEmpty(text))
            {
                //try
                //{                                      

                var synthesizer = new SpeechSynthesizer();
                synthesizer.Voice = voices.First(gender => gender.Gender == VoiceGender.Female);
                SpeechSynthesisStream stream = await synthesizer.SynthesizeTextToStreamAsync(text);

                media.AutoPlay = true;
                media.SetSource(stream, stream.ContentType);
                media.Play();

                var savePicker = new FileSavePicker();
                savePicker.SuggestedStartLocation = PickerLocationId.Desktop;
                savePicker.FileTypeChoices.Add("Sound", new List<string>() { ".mp3" });
                savePicker.SuggestedFileName = "speech";

                var file = await savePicker.PickSaveFileAsync();

                using (var reader = new DataReader(stream))
                {
                    await reader.LoadAsync((uint)stream.Size);
                    IBuffer buffer = reader.ReadBuffer((uint)stream.Size);
                    await FileIO.WriteBufferAsync(file, buffer);
                }
                //}

                //catch (Exception)
                //{
                //    // If the text is unable to be synthesized, throw an error message to the user.            
                //    var messageDialog = new Windows.UI.Popups.MessageDialog("Komputer tidak dapat menyimpan data atau membaca teks.");
                //    await messageDialog.ShowAsync();
                //}                    
            }
        }
        #endregion
    }
}
