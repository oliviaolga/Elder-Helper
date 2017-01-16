using ElderHelperApplication.Interfaces;
using ElderHelperApplication.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElderHelperApplication.DataContext
{
    public static class DataContextHelper
    {
        public static async Task AddRecord<T>(T newRecord) where T : class
        {
            using (var db = new GoalDataContext())
            {
                db.Add<T>(newRecord);
                await db.SaveChangesAsync();

                if (typeof(T) == typeof(Calculation))
                {
                    var calculation = newRecord as Calculation;
                    await AddCalorieToday(calculation);
                }
            }
        }

        public static List<T> GetTable<T>() where T : class
        {
            using (var db = new GoalDataContext())
                return db.Set<T>().ToList();
        }

        public static List<Calculation> GetCalculationByGoalId(int goalId)
        {
            using (var db = new GoalDataContext())
                return db.Set<Calculation>().Where(x => x.GoalId == goalId).ToList();
        }

        public static async Task UpdateGoal(Goal updateGoal)
        {
            using (var db = new GoalDataContext())
            {
                var goals = await db.Goals.ToListAsync();
                var goal = goals.FirstOrDefault(x => x.GoalId == updateGoal.GoalId);
                if (goal != null)
                {
                    goal.nama = updateGoal.nama;
                    goal.berat = updateGoal.berat;
                    goal.tinggi = updateGoal.tinggi;
                    goal.umur = updateGoal.umur;
                    goal.gender = updateGoal.gender;
                    goal.calorieGoal = updateGoal.calorieGoal;
                    goal.calorieToday = 0;
                    goal.date = updateGoal.date;
                    await db.SaveChangesAsync();
                }
            }
        }

        private static async Task AddCalorieToday(Calculation savedCalculation)
        {
            await Task.Factory.StartNew(async () =>
            {
                using (var db = new GoalDataContext())
                {
                    var goals = await db.Goals.ToListAsync();
                    var goal = goals.SingleOrDefault(x => x.GoalId == savedCalculation.GoalId);                                       

                    goal.calorieToday += savedCalculation.amount;                    

                    await db.SaveChangesAsync();
                }
            });
        }
                
        public static void DeleteAllGoals()
        {
            using (var db = new GoalDataContext())
            {
                foreach (var item in db.Goals)
                    db.Goals.Remove(item);

                db.SaveChanges();
            }
        }

        public static T GetItem<T>(int Id) where T : class
        {
            var item = default(T);
            using (var db = new GoalDataContext())
            {
                item = db.Set<T>().ToList().FirstOrDefault(x => ((ITableItem)x).GetId() == Id);
            }

            return item;
        }

        public static async Task DeleteItem<T>(T itemToDelete) where T : class
        {
            using (var db = new GoalDataContext())
            {
                db.Set<T>().Remove(itemToDelete);
                await db.SaveChangesAsync();
            }
        }

        public static void ResetItem()
        {
            using (var db = new GoalDataContext())
            {                
                foreach (var item in db.Goals)
                {
                    //var data = new TotalCalorie();
                    //data.calorieToday = item.calorieToday;
                    //data.date = DateTime.Now;
                    //db.Add(data);
                    item.dataToday = 0;
                    item.dataToday = item.calorieToday;
                    item.calorieToday = 0;
                }                    

                foreach (var item2 in db.Calculations)                
                    db.Calculations.Remove(item2);                

                db.SaveChanges();
            }
        }        
    }
}
