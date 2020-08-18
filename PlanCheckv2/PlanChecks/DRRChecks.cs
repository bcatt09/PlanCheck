using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.TPS.Common.Model.API;

namespace VMS.TPS.PlanChecks
{
    public class DRRChecks : PlanCheck
    {
        protected override List<string> MachineExemptions => new List<string> { };

        public DRRChecks(PlanSetup plan) : base(plan) { }

        protected override void RunTest(PlanSetup plan)
		{
			DisplayName = "DRRs";
			Result = "Pass";
			ResultDetails = "";
			ResultColor = "LimeGreen";
			TestExplanation = "Checks that DRRs are created and attached as a reference for all fields";

			foreach (Beam field in plan.Beams)
			{
				if (field.ReferenceImage == null)
				{
					Result = "Warning";
					ResultDetails += field.Id + " has no reference image\n";
					ResultColor = "Gold";
				}
			}

			ResultDetails = ResultDetails.TrimEnd('\n');
		}
    }
}
