using ElderHelperApplication.ButtonCommands;
using ElderHelperApplication.DataContext;
using ElderHelperApplication.Enum;
using ElderHelperApplication.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElderHelperApplication.ViewModel
{
    public class CalorieTrackerViewModel : BaseViewModel
    {       
        public List<Goal> GoalList
        {
            get { return DataContextHelper.GetTable<Goal>(); }
        }

        private bool _showCalculateControl = false;
        public bool ShowCalculateControl
        {
            get { return _showCalculateControl; }
            set
            {
                _showCalculateControl = value;
                NotifyPropertyChanged("ShowCalculateControl");
            }
        }

        private bool _showGoalControl = false;
        public bool ShowGoalControl
        {
            get { return _showGoalControl; }
            set
            {
                _showGoalControl = value;
                NotifyPropertyChanged("ShowGoalControl");
            }
        }

        private int _goalId;
        public int GoalId
        {
            get { return _goalId; }
            set
            {
                _goalId = value;
                NotifyPropertyChanged("GoalId");
            }
        }

        private GoalAction _goalAction = GoalAction.Create;
        public GoalAction GoalAction
        {
            get { return _goalAction; }
            set
            {
                _goalAction = value;
                NotifyPropertyChanged("GoalAction");
            }
        }

        public ButtonCommand AddCalculateButtonCommand { get; set; }
        public ButtonCommand GoalButtonCommand { get; set; }
        public ButtonCommand EditButtonCommand { get; set; }
        public ButtonCommand DeleteButtonCommand { get; set; }

        public event EventHandler OnDeleteFinished;
        private void FireOnDeleteFinished()
        {
            if (OnDeleteFinished != null)
                OnDeleteFinished(null, null);
        }

        public CalorieTrackerViewModel()
        {
            //_goalList = new ObservableCollection<Goal>();
            AddCalculateButtonCommand = new ButtonCommand(ChangeAddCalculateVisibility);
            GoalButtonCommand = new ButtonCommand(ChangeGoalVisibility);
            EditButtonCommand = new ButtonCommand(EditGoal);
            DeleteButtonCommand = new ButtonCommand(DeleteGoal);
        }

        public async void AddNewGoal(Goal newGoal)
        {
            //add item to our goal list
            await DataContextHelper.AddRecord<Goal>(newGoal);
        }

        private void ChangeAddCalculateVisibility(object parameter)
        {
            var goal = parameter as Goal;
            GoalId = goal.GoalId;
            ShowCalculateControl = true;
        }

        private void ChangeGoalVisibility(object parameter)
        {
            this.GoalAction = GoalAction;
            ShowGoalControl = true;
        }

        private void EditGoal(object parameter)
        {
            var goal = parameter as Goal;
            GoalId = goal.GoalId;
            this.GoalAction = GoalAction.Update;

            ShowGoalControl = true;
        }

        private async void DeleteGoal(object parameter)
        {
            var goal = parameter as Goal;
            await DataContextHelper.DeleteItem<Goal>(goal);

            FireOnDeleteFinished();
        }
    }
}
