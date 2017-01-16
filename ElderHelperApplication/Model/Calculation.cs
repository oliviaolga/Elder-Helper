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
    public class Calculation : ITableItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime date { get; set; }
        public double amount { get; set; }

        public int GoalId { get; set; }        

        [ForeignKey("GoalId")]
        public Goal Goal { get; set; }

        int ITableItem.GetId()
        {
            return Id;
        }
    }
}
