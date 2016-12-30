using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using ElderHelperApplication.DataContext;

namespace ElderHelperApplication.Migrations
{
    [DbContext(typeof(GoalDataContext))]
    [Migration("20161229143556_CalorieTable")]
    partial class CalorieTable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752");

            modelBuilder.Entity("ElderHelperApplication.Model.Calculation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("GoalId");

                    b.Property<double>("amount");

                    b.Property<DateTime>("date");

                    b.HasKey("Id");

                    b.HasIndex("GoalId");

                    b.ToTable("Calculations");
                });

            modelBuilder.Entity("ElderHelperApplication.Model.Goal", b =>
                {
                    b.Property<int>("GoalId")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("berat");

                    b.Property<double>("calorieGoal");

                    b.Property<double>("calorieToday");

                    b.Property<DateTime>("date");

                    b.Property<string>("gender");

                    b.Property<string>("nama");

                    b.Property<double>("tinggi");

                    b.Property<int>("umur");

                    b.HasKey("GoalId");

                    b.ToTable("Goals");
                });

            modelBuilder.Entity("ElderHelperApplication.Model.TotalCalorie", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("CalculationId");

                    b.Property<int>("GoalId");

                    b.Property<int>("GuId");

                    b.Property<double>("calorieToday");

                    b.Property<DateTime>("date");

                    b.HasKey("Id");

                    b.HasIndex("CalculationId");

                    b.HasIndex("GoalId");

                    b.ToTable("TotalCalories");
                });

            modelBuilder.Entity("ElderHelperApplication.Model.Calculation", b =>
                {
                    b.HasOne("ElderHelperApplication.Model.Goal", "Goal")
                        .WithMany("Calculations")
                        .HasForeignKey("GoalId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ElderHelperApplication.Model.TotalCalorie", b =>
                {
                    b.HasOne("ElderHelperApplication.Model.Calculation", "Calculation")
                        .WithMany("TotalCalories")
                        .HasForeignKey("CalculationId");

                    b.HasOne("ElderHelperApplication.Model.Goal", "Goal")
                        .WithMany("TotalCalories")
                        .HasForeignKey("GoalId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
