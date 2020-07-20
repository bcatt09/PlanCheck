using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.TPS.Common.Model.API;
using VMS.TPS.Common.Model.Types;

namespace VMS.TPS.PlanChecks
{
    public class CalcParametersChecks : PlanCheck
    {
        protected override List<string> MachineExemptions => new List<string> { };

        public CalcParametersChecks(PlanSetup plan) : base(plan) { }

		protected override void RunTest(PlanSetup plan)
		{
			DisplayName = "Calc Parameters";
			Result = "";
			ResultDetails = "";
			ResultColor = "LimeGreen";
			TestExplanation = "Displays calculation parameters";

			Boolean photon = false;
			Boolean electron = false;
			Boolean IMRT = false;
			Boolean VMAT = false;

			//get calc options
			Dictionary<string, string> photonOptions = plan.PhotonCalculationOptions;
			Dictionary<string, string> electronOptions = plan.ElectronCalculationOptions;

			//loop through beams to see what needs to be displayed
			foreach (Beam beam in plan.Beams)
			{
				if (beam.EnergyModeDisplayName.Contains('E'))
					electron = true;
				else if (beam.EnergyModeDisplayName.Contains('X'))
					photon = true;

				if (beam.MLCPlanType == MLCPlanType.DoseDynamic && beam.ControlPoints.Count > 18)
					IMRT = true;
				else if (beam.MLCPlanType == MLCPlanType.VMAT)
					VMAT = true;
			}

			if (electron)
			{
				ResultDetails += "Volume Dose: " + plan.ElectronCalculationModel.ToString();
				ResultDetails += "\nGrid Size: " + electronOptions["CalculationGridSizeInCM"];
				ResultDetails += "\nUncertainty: " + electronOptions["StatisticalUncertainty"];
				ResultDetails += "\nSmooting Method: " + electronOptions["SmoothingMethod"];
				ResultDetails += "\nSmoothing Level: " + electronOptions["SmoothingLevel"];
			}
			if (photon)
			{
				ResultDetails += "Volume Dose: " + plan.PhotonCalculationModel.ToString();

				if (IMRT)
				{
					ResultDetails += "\nIMRT Optimization: " + plan.GetCalculationModel(CalculationType.PhotonIMRTOptimization);
					ResultDetails += "\nLeaf Motion: " + plan.GetCalculationModel(CalculationType.PhotonLeafMotions);
				}
				if (VMAT)
					ResultDetails += "\nVMAT Optimization: " + plan.GetCalculationModel(CalculationType.PhotonVMATOptimization);

				ResultDetails += "\nGrid Size: " + photonOptions["CalculationGridSizeInCM"] + "cm";
				ResultDetails += "\nHeterogeneity Corrections: " + photonOptions["HeterogeneityCorrection"];
			}
		}
    }
}
