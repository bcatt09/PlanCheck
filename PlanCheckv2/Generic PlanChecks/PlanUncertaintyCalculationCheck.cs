using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VMS.TPS.Common.Model.API;
using VMS.TPS.Common.Model.Types;

namespace PlanCheck.Checks
{
    public class PlanUncertaintyCalculationCheck : PlanCheckBase
    {
		protected override List<string> MachineExemptions => DepartmentInfo.LinearAccelerators;

        public PlanUncertaintyCalculationCheck(PlanSetup plan) : base(plan) { }

        public override void RunTest(PlanSetup plan)
        {
            DisplayName = "Plan Uncertainty Calculation Verification";
            TestExplanation = "Validates that plan uncertainty calculations have been performed";

            var pus = plan.PlanUncertainties;
            int puCount = pus.Count();
            double puShift;
            double puCalibrationCurveError;
            int countCalculatedUncertainties = 0;
            //List<int> testResultsList = new List<int>();

            Result = "TEST NOT RUN";
            DisplayColor = ResultColorChoices.TestNotRun;
            
            if (puCount > 0)
            {
                foreach (PlanUncertainty pu in pus)
                {
                    string puString = pu.ToString();
                    string pubus = pu.BeamUncertainties.Count().ToString();
                    double pucce = pu.CalibrationCurveError;
                    string puname = pu.DisplayName.ToString();
                    string putype = pu.UncertaintyType.ToString();
                    string pumax = pu.Dose?.DoseMax3D.ToString();
                    //string pusomething = pu.Dose?.

                    //double maxX, maxY, maxZ = 0;
                    //double minX, minY, minZ = 0;


                    if (pumax !=null)  // it found dose in there
                    {
                        countCalculatedUncertainties++;
                        //ResultDetails += $"{puname}: dMax: {pumax}\n";
                        //testResultsList.Add(EvaluateUncertainty(pu, plan.Id));
                    }
                }

                // Check how many uncertainties are calculated just make sure it's over 5
                
            }
            if (countCalculatedUncertainties < 5)
            {
                //testResultsList.Add(0);
                Result = "Warning";
                DisplayColor= ResultColorChoices.Warn;
                ResultDetails += $"Count of calculated plan uncertainties was {countCalculatedUncertainties}\n";
            }
            else
            {
                Result = "Pass";
                DisplayColor = ResultColorChoices.Pass;
                ResultDetails += $"Count of calculated plan uncertainties was {countCalculatedUncertainties}\n";
            }

        }
        private int EvaluateUncertainty (PlanUncertainty pu, String planId)
        {

            return 1;
        }

    }
}
