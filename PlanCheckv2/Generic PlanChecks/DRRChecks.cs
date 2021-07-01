using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.TPS.Common.Model.API;

namespace PlanCheck.Checks
{
    public class DRRChecks : PlanCheckBase
	{
        protected override List<string> MachineExemptions => new List<string> { };

        public DRRChecks(PlanSetup plan) : base(plan) { }

        public override void RunTest(PlanSetup plan)
		{
			DisplayName = "DRRs";
			Result = "Pass";
			ResultDetails = "";
			DisplayColor = ResultColorChoices.Pass;
			TestExplanation = "Checks that DRRs are created and attached as a reference for all fields";

			foreach (Beam field in plan.Beams)
			{
				if (field.ReferenceImage == null)
				{
					Result = "Warning";
					ResultDetails += field.Id + " has no reference image\n";
					DisplayColor = ResultColorChoices.Warn;
				}
			}

			ResultDetails = ResultDetails.TrimEnd('\n');
		}
    }
}
