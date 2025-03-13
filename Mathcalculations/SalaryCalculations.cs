using System;

namespace Budgetinator_2000.Mathcalculations
{
    public static class SalaryCalculations
    {
        public static decimal AnnualSalary(decimal monthlySalary)
        {
            return monthlySalary * 12;
        }

        public static decimal MonthlySalary(decimal hourlyWage, decimal monthlyHours = 150)
        {
            return hourlyWage * monthlyHours;
        }

        public static decimal TaxDeduction(decimal grossIncome, decimal taxPercentage)
        {
            return grossIncome * taxPercentage / 100;
        }

        public static decimal NetSalary(decimal grossIncome, decimal taxDeduction)
        {
            return grossIncome - taxDeduction;
        }

        public static decimal HourlyWage(decimal monthlySalary, decimal monthlyHours)
        {
            if (monthlyHours == 0)
                throw new DivideByZeroException("Monthly hours cannot be zero.");

            return monthlySalary / monthlyHours;
        }

        public static decimal OvertimeCompensation(decimal hourlyWage, decimal overtimePercentage, decimal overtimeHours)
        {
            return hourlyWage * (1 + overtimePercentage / 100) * overtimeHours;
        }
    }
}