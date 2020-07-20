using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.TPS.Common.Model.API;

namespace VMS.TPS.PlanChecks
{
    public class HotspotChecks : PlanCheck
    {
        protected override List<string> MachineExemptions => new List<string> { };

        public HotspotChecks(PlanSetup plan) : base(plan) { }

        protected override void RunTest(PlanSetup plan)
		{
			DisplayName = "Hotspot";
			TestExplanation = "Checks to see if the hotspot is inside of the plan target";

			Structure target = plan.StructureSet.Structures.First(s => s.Id == plan.TargetVolumeID);

			if (plan.IsDoseValid)
			{
				bool inTarget = target.IsPointInsideSegment(plan.Dose.DoseMax3DLocation);

				Result = "";
				ResultDetails = inTarget ? $"Hotspot is in {target.Id}" : $"Hotspot is not in {target.Id}";
				ResultColor = inTarget ? "LimeGreen" : "Gold";
			}
			else
			{
				Result = "";
				ResultDetails = "Dose has not been calculated";
				ResultColor = "Gold";
			}
		}
    }
}
