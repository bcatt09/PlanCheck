using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.TPS.Common.Model.API;
using VMS.TPS.Common.Model.Types;

namespace PlanCheck.Checks
{
	public class ProtonCalculationModelChecks : PlanCheckBaseProton
	{
		protected override List<string> MachineExemptions => new List<string> { };

		public ProtonCalculationModelChecks(PlanSetup plan) : base(plan) { }

		public override void RunTestProton(IonPlanSetup plan)
		{

			DisplayName = "Calculation Model";
			TestExplanation = "Checks to see what calculation/optimization models have been used in planning";
			IonPlanSetup ionPlan = (IonPlanSetup)plan;

			string VolumeDoseType = plan.GetCalculationModel(CalculationType.ProtonVolumeDose).ToString();

			string OptimizationType = plan.GetCalculationModel(CalculationType.ProtonOptimization).ToString();

			string technique = plan.RTPrescription.Technique.Trim('|');

			if (string.IsNullOrEmpty(VolumeDoseType) || string.IsNullOrEmpty(OptimizationType) || string.IsNullOrEmpty(technique))
			{
				Result = "";
				ResultDetails = "Error, there is no calculation or optimization or technique set";
				DisplayColor = ResultColorChoices.Fail;
			}
			else
			{
				Result = "";
				ResultDetails = $"Volume dose model is {VolumeDoseType} and Optimization Type is {OptimizationType} and {technique}";
				DisplayColor = ResultColorChoices.Pass;
			}

		}

	}

}
