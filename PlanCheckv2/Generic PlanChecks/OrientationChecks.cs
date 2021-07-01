using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.TPS.Common.Model.API;

namespace PlanCheck.Checks
{
    public class OrientationChecks : PlanCheckBase
	{
        protected override List<string> MachineExemptions => new List<string> { };

        public OrientationChecks(PlanSetup plan) : base(plan) { }

        public override void RunTest(PlanSetup plan)
		{
			DisplayName = "Orientation";
			TestExplanation = "Checks the planned patient orientation against the orientation selected from CT sim";
			Result = "";
			ResultDetails = AddSpaces(plan.TreatmentOrientation.ToString());
			DisplayColor = ResultColorChoices.Pass;

			if (plan.StructureSet.Image.ImagingOrientation != plan.TreatmentOrientation)
			{
				Result = "Warning";
				ResultDetails = $"Scanning orientation of \"{AddSpaces(plan.StructureSet.Image.ImagingOrientation.ToString())}\" does not match treatment orientation of \"{AddSpaces(plan.TreatmentOrientation.ToString())}\"\nAny calculated directions may be backwards";
				DisplayColor = ResultColorChoices.Warn;
			}
		}
    }
}
