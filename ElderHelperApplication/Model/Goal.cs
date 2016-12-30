using ElderHelperApplication.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElderHelperApplication.Model
{
    [Table("Goals")]
    public class Goal : ITableItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GoalId { get; set; }        

        public DateTime date { get; set; }
        public string nama { get; set; }
        public double berat { get; set; }
        public double tinggi { get; set; }
        public int umur { get; set; }
        public string gender { get; set; }
        public double calorieGoal { get; set; }
        public double calorieToday { get; set; }        

        public List<Calculation> Calculations { get; set; }
        public List<TotalCalorie> TotalCalories { get; set; }

        int ITableItem.GetId()
        {
            return GoalId;
        }
    }
}
