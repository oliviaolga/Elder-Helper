using ElderHelperApplication.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElderHelperApplication.DataContext
{
    public class GoalDataContext : DbContext
    {
        public DbSet<Goal> Goals { get; set; }
        public DbSet<Calculation> Calculations { get; set; }
        public DbSet<TotalCalorie> TotalCalories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename=CalorieTracker.db");
        }
    }
}
