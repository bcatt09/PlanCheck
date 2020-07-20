using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.TPS.Common.Model.API;

namespace VMS.TPS.PlanChecks
{
    public class OrientationChecks : PlanCheck
    {
        protected override List<string> MachineExemptions => new List<string> { };

        public OrientationChecks(PlanSetup plan) : base(plan) { }

        protected override void RunTest(PlanSetup plan)
		{
			DisplayName = "Orientation";
			TestExplanation = "Checks the planned patient orientation against the orientation selected from CT sim";
			Result = "";
			ResultDetails = AddSpacesToSentence(plan.TreatmentOrientation.ToString());
			ResultColor = "LimeGreen";

			if (plan.StructureSet.Image.ImagingOrientation != plan.TreatmentOrientation)
			{
				Result = "Warning";
				ResultDetails = $"Scanning orientation of \"{AddSpacesToSentence(plan.StructureSet.Image.ImagingOrientation.ToString())}\" does not match treatment orientation of \"{AddSpacesToSentence(plan.TreatmentOrientation.ToString())}\"\nAny calculated directions may be backwards";
				ResultColor = "Gold";
			}
		}
    }
}
