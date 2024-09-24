using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.TPS.Common.Model.API;
using VMS.TPS.Common.Model.Types;

namespace PlanCheck.Checks
{
    public class PlanningRxTargetChecks : PlanCheckBase
    {
        protected override List<string> MachineExemptions => new List<string> { };

        public PlanningRxTargetChecks(PlanSetup plan) : base(plan) { }
        public override void RunTest(PlanSetup plan)
        {
            DisplayName = "Target Naming Checks";
            TestExplanation = "Displays and verifies plan target against Rx Target and Name of structure in target constraints";

            List<string> rxTargetVolumesWithConstraints = new List<string> { };

            string planTargetName = plan.TargetVolumeID;
            string rxPrimaryTargetVolume = "", rxSecondaryTargetVolumes = "";


            // Abort test quickly if there is no attached prescription
            if (plan.RTPrescription == null)
            {
                Result = "";
                ResultDetails = "No prescription attached to the plan";
                DisplayColor = ResultColorChoices.Fail;

                return;
            }


            // Getting from main volume area
            foreach (var target in plan.RTPrescription.Targets.OrderByDescending(x => x.DosePerFraction * x.NumberOfFractions))
            {
                if (rxPrimaryTargetVolume == "")
                {
                    rxPrimaryTargetVolume = target.TargetId;
                }
                else
                {
                    rxSecondaryTargetVolumes += target.TargetId + "\n";
                }
            }

            // Checking on target constraint area



        }
    }
}
