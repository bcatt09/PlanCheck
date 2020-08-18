using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VMS.TPS.Common.Model.API;

namespace VMS.TPS.PlanChecks
{
    class TargetChecks : PlanCheck
	{
		protected override List<string> MachineExemptions => new List<string> { };

		public TargetChecks(PlanSetup plan) : base(plan) { }

        protected override void RunTest(PlanSetup plan)
		{
			DisplayName = "Target";
			TestExplanation = "Checks that there is only one piece to the target structure";

			if (plan.TargetVolumeID != "")
			{
				//Select the target structure for the plan and get how many separate pieces it has
				Structure target = (from s in plan.StructureSet.Structures where s.Id == plan.TargetVolumeID select s).FirstOrDefault();
				int targetPieces = target.GetNumberOfSeparateParts();

				if (targetPieces > 1)
				{
					Result = "Warning";
					ResultDetails = $"Plan target ({target.Id}) has " + targetPieces + " separates pieces.  Is this correct?";
					ResultColor = "Gold";
				}
				else
				{
					Result = "";
					ResultDetails = target.Id;
					ResultColor = "LimeGreen";
				}
			}
			else
			{
				Result = "";
				ResultDetails = "No target structure selected";
				ResultColor = "Gold";
			}
		}
    }
}
