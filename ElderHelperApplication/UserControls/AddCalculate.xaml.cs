using ElderHelperApplication.DataContext;
using ElderHelperApplication.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace ElderHelperApplication.UserControls
{
    public sealed partial class AddCalculate : UserControl
    {
        public int GoalId
        {
            get
            {
                return (int)GetValue(GoalIdProperty);
            }
            set
            {
                SetValue(GoalIdProperty, value);
            }
        }

        private readonly DependencyProperty GoalIdProperty = DependencyProperty.Register("GoalId", typeof(int), typeof(AddCalculate), null);
        public event EventHandler CalculationSaveFinished;
        private void FireCalculationSavedFinished()
        {
            if (CalculationSaveFinished != null)
                CalculationSaveFinished(null, null);
        }

        public AddCalculate()
        {
            this.InitializeComponent();
        }


        private async void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if ((!System.Text.RegularExpressions.Regex.IsMatch(calorieToday.Text, "^[a-zA-Z]")) && (calorieToday.Text != ""))
            {
                var newCalculation = new Calculation();
                newCalculation.date = DateTime.Now;
                newCalculation.amount = Convert.ToDouble(calorieToday.Text);
                newCalculation.GoalId = GoalId;
                await DataContextHelper.AddRecord<Calculation>(newCalculation);

                FireCalculationSavedFinished();
                ClearFields();
                HideControl();
            }
            else
            {
                var message = new Windows.UI.Popups.MessageDialog("Cek kembali input anda. Pastikan yang terisi adalah angka dan tidak kosong. ");
                await message.ShowAsync();
            }

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            ClearFields();
            HideControl();
        }

        private void ClearFields()
        {
            calorieToday.Text = string.Empty;
        }

        private void HideControl()
        {
            Visibility = Visibility.Collapsed;
        }
    }
}
