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
    public class TotalCalorie : ITableItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GuId { get; set; }

        public DateTime date { get; set; }
        public double calorieToday { get; set; }             
                
        public int GoalId { get; set; }

        [ForeignKey("GoalId")]
        public Goal Goal { get; set; }

        public int Id { get; set; }

        [ForeignKey("Id")]
        public Calculation Calculation { get; set; }

        int ITableItem.GetId()
        {
            return GuId;
        }
    }
}
