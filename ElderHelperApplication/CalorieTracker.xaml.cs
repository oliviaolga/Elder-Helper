using ElderHelperApplication.DataContext;
using ElderHelperApplication.Model;
using ElderHelperApplication.ViewModel;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ElderHelperApplication
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CalorieTracker : Page
    {
        private CalorieTrackerViewModel _calorieTrackerViewModel;

        public CalorieTracker()
        {
            this.InitializeComponent();
            Loaded += CalorieTracker_Loaded;
        }

        private void CalorieTracker_Loaded(object sender, RoutedEventArgs e)
        {
            //reset data everyday
            DateTime currentTimeStamp = new DateTime();
            using (var db = new GoalDataContext())
            {
                foreach (var datenow in db.Calculations)
                    currentTimeStamp = datenow.date;
            }
            DateTime currentNow = DateTime.Now;
            int changeDay = currentNow.Day;
            if (currentTimeStamp.Day != changeDay)
            {
                DataContextHelper.ResetItem();
            }

            AddGoalShow.OnGoalSaved += AddGoalShow_OnGoalSaved;
            CalculateControl.CalculationSaveFinished += CalculateControl_CalculationSaveFinished;

            if (_calorieTrackerViewModel == null)
            {
                _calorieTrackerViewModel = new CalorieTrackerViewModel();

                _calorieTrackerViewModel.OnDeleteFinished += _calorieTrackerViewModel_OnDeleteFinished;
                DataContext = _calorieTrackerViewModel;
            }
        }

        private void _calorieTrackerViewModel_OnDeleteFinished(object sender, EventArgs e)
        {
            GoalListView.ItemsSource = _calorieTrackerViewModel.GoalList;
        }

        private void CalculateControl_CalculationSaveFinished(object sender, EventArgs e)
        {
            GoalListView.ItemsSource = _calorieTrackerViewModel.GoalList;
        }

        private void AddGoalShow_OnGoalSaved(object sender, EventArgs e)
        {
            //add goal to list      
            GoalListView.ItemsSource = _calorieTrackerViewModel.GoalList;
        }

        private void GoalListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var goal = e.ClickedItem as Goal;
            Frame.Navigate(typeof(DetailGoalPage), goal.GoalId);
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddGoalShow.Visibility = Visibility.Visible;
        }
    }
}
