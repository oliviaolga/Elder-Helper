using ElderHelperApplication.DataContext;
using ElderHelperApplication.Enum;
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
    public sealed partial class AddGoal : UserControl
    {
        public event EventHandler OnGoalSaved;

        private static readonly DependencyProperty GoalIdProperty = DependencyProperty.Register("GoalId", typeof(int), typeof(AddGoal), null);
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

        private static readonly DependencyProperty ActionProperty = DependencyProperty.Register("Action", typeof(GoalAction), typeof(AddGoal), new PropertyMetadata(GoalAction.Create, SetFields));

        public GoalAction Action
        {
            get
            {
                return (GoalAction)GetValue(ActionProperty);
            }
            set
            {
                SetValue(ActionProperty, value);
            }
        }

        private static void SetFields(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            //if(Action == GoalAction.Update)
            //{
            //    var goal = DataContextHelper.GetItem<Goal>(GoalId);
            //}

            if (((GoalAction)e.NewValue) == GoalAction.Update)
            {
                var addGoal = o as AddGoal;
                var Goal = DataContextHelper.GetItem<Goal>(addGoal.GoalId);
              
                addGoal.Nama.Text = Goal.nama;
                addGoal.Berat.Text = Goal.berat.ToString();
                addGoal.Tinggi.Text = Goal.tinggi.ToString();
                addGoal.Usia.Text = Goal.umur.ToString();
            }
        }

        public AddGoal()
        {
            this.InitializeComponent();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Collapsed;
            ClearText();
        }

        private void Gender_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var newList = new Goal();
            newList.gender = ((ComboBoxItem)Gender.SelectedItem).Content.ToString();
        }

        private async void Hitung_Click(object sender, RoutedEventArgs e)
        {
            if ((System.Text.RegularExpressions.Regex.IsMatch(Nama.Text, "^[a-zA-Z]")) && (Nama.Text != "") &&
                (!System.Text.RegularExpressions.Regex.IsMatch(Berat.Text, "^[a-zA-Z]")) && (Berat.Text != "") &&
                (!System.Text.RegularExpressions.Regex.IsMatch(Tinggi.Text, "^[a-zA-Z]")) && (Tinggi.Text != "") &&
                (!System.Text.RegularExpressions.Regex.IsMatch(Usia.Text, "^[a-zA-Z]")) && (Usia.Text != "") &&
                (Gender.SelectedItem != null))
            {
                if (Action == GoalAction.Create)
                {
                    var newList = new Goal();
                    newList.date = DateTime.Now;
                    newList.nama = Nama.Text;
                    newList.berat = Convert.ToDouble(Berat.Text);
                    newList.tinggi = Convert.ToDouble(Tinggi.Text);
                    newList.umur = Convert.ToInt32(Usia.Text);
                    newList.calorieToday = 0;
                    newList.gender = ((ComboBoxItem)Gender.SelectedItem).Content.ToString();

                    if (newList.gender == "Laki-laki")
                    {
                        newList.calorieGoal = (10 * newList.berat) + (6.25 * newList.tinggi) - (5 * newList.umur) + 5;
                    }
                    else if (newList.gender == "Perempuan")
                    {
                        newList.calorieGoal = (10 * newList.berat) + (6.25 * newList.tinggi) - (5 * newList.umur) - 161;
                    }

                    await DataContextHelper.AddRecord<Goal>(newList);
                }
                else if (Action == GoalAction.Update)
                {
                    var goal = DataContextHelper.GetItem<Goal>(GoalId);
                    goal.date = DateTime.Now;
                    goal.nama = Nama.Text;
                    goal.berat = Convert.ToDouble(Berat.Text);
                    goal.tinggi = Convert.ToInt32(Usia.Text);
                    goal.gender = ((ComboBoxItem)Gender.SelectedItem).Content.ToString();
                    if (goal.gender == "Laki-laki")
                    {
                        goal.calorieGoal = (10 * goal.berat) + (6.25 * goal.tinggi) - (5 * goal.umur) + 5;
                    }
                    else if (goal.gender == "Perempuan")
                    {
                        goal.calorieGoal = (10 * goal.berat) + (6.25 * goal.tinggi) - (5 * goal.umur) - 161;
                    }
                    await DataContextHelper.UpdateGoal(goal);
                }

                FireOnGoalSave();

                ClearText();
                Visibility = Visibility.Collapsed;
            }
            else
            {
                var message = new Windows.UI.Popups.MessageDialog("Cek kembali input anda. Pastikan tidak ada yang kosong. ");
                await message.ShowAsync();
            }
        }

        private void ClearText()
        {
            Nama.Text = string.Empty;
            Berat.Text = string.Empty;
            Tinggi.Text = string.Empty;
            Usia.Text = string.Empty;
            Gender.SelectedIndex = 0;
        }

        private void FireOnGoalSave()
        {
            OnGoalSaved?.Invoke(null, null);
        }
    }
}
