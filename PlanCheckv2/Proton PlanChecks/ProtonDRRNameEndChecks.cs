using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.TPS.Common.Model.API;

namespace PlanCheck.Checks
{
    public class ProtonDRRNameEndChecks : PlanCheckBaseProton
    {
        protected override List<string> MachineExemptions => new List<string> { };

        public ProtonDRRNameEndChecks(PlanSetup plan) : base(plan) { }

        public override void RunTestProton(IonPlanSetup plan)
        {

			DisplayName = "DRRs";
			TestExplanation = "Checks that DRR images have 'DRR' at the end of their names ";
			IonPlanSetup ionPlan = (IonPlanSetup)plan;

			int numErrorImages = 0;

			foreach (Beam field in plan.Beams)
			{
				string fieldName = field.ReferenceImage.Id;
				int fieldNameLength = fieldName.Length;
				if (fieldName.Substring(fieldNameLength - 3) != "DRR")
				{
					numErrorImages++;
				}
			}

			if (numErrorImages == 0)
			{
				Result = "";
				ResultDetails = $"All DRRs have 'DRR' at the end";
				DisplayColor = ResultColorChoices.Pass;
			}
			else
			{
				Result = "";
				ResultDetails = $"There are {numErrorImages} named improperly";
				DisplayColor = ResultColorChoices.Fail;
			}

		}

    }

}
