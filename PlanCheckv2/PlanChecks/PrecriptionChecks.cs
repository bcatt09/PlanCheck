using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.TPS.Common.Model.API;
using VMS.TPS.PlanChecks;

namespace VMS.TPS
{
    public class PrecriptionChecks : PlanCheck
    {
        protected override List<string> MachineExemptions => new List<string> { };

        public PrecriptionChecks(PlanSetup plan) : base(plan) { }

        protected override void RunTest(PlanSetup plan)
        {
            DisplayName = "Prescription";
            TestExplanation = "Displays plan dose information from Eclipse and checks it versus the prescription in Aria";

            if (plan.RTPrescription == null)
            {
                Result = "";
                ResultDetails = "No prescription attached to the plan";
                ResultColor = "Tomato";

                return;
            }

            RTPrescriptionTarget rx = plan.RTPrescription.Targets.OrderByDescending(x => x.DosePerFraction * x.NumberOfFractions).First();

            if ((plan.NumberOfFractions != rx.NumberOfFractions || plan.DosePerFraction != rx.DosePerFraction))
            {
                Result = "Warning";
                ResultDetails = $"Plan dose does not match prescription\n\nPrescription:\n{rx.DosePerFraction} x {rx.NumberOfFractions} Fx = {rx.DosePerFraction * rx.NumberOfFractions}\n\nPlan:\n{plan.DosePerFraction} x {plan.NumberOfFractions} Fx = {plan.TotalDose.ToString()}\n\nPrescribed Percentage: {(plan.TreatmentPercentage * 100.0).ToString("0.0")}%\nPlan Normalization: {plan.PlanNormalizationValue.ToString("0.0")}%";
                ResultColor = "Gold";
            }
            else
            {
                Result = "";
                ResultDetails = $"{plan.DosePerFraction} x {plan.NumberOfFractions} Fx = {plan.TotalDose}\nPrescribed Percentage: {(plan.TreatmentPercentage * 100.0).ToString("0.0")}%\nPlan Normalization: {plan.PlanNormalizationValue.ToString("0.0")}%";
                ResultColor = "LimeGreen";
            }
        }
    }
}
