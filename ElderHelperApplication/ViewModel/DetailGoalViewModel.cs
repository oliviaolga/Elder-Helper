using ElderHelperApplication.DataContext;
using ElderHelperApplication.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElderHelperApplication.ViewModel
{
    public class DetailGoalViewModel
    {
        private int goalId;

        public Goal CurrentGoal
        {
            get
            {
                var goal = DataContextHelper.GetItem<Goal>(goalId);
                goal.Calculations = DataContextHelper.GetCalculationByGoalId(goal.GoalId);

                return goal;
            }
        }

        public DetailGoalViewModel(int Id)
        {
            goalId = Id;
        }
    }
}
