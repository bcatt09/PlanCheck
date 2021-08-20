using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.TPS.Common.Model.API;
using VMS.TPS.Common.Model.Types;

namespace PlanCheck.Checks
{
    public class CalcParametersChecks : PlanCheckBasePhoton
	{
        protected override List<string> MachineExemptions => new List<string> { };

        public CalcParametersChecks(PlanSetup plan) : base(plan) { }

		public override void RunTestPhoton(ExternalPlanSetup plan)
		{
			DisplayName = "Calc Parameters";
			Result = "";
			ResultDetails = "";
			DisplayColor = ResultColorChoices.Pass;
			TestExplanation = "";

            bool photon = false;
			bool electron = false;
			bool IMRT = false;
			bool VMAT = false;

			if(plan.Dose == null)
            {
				Result = "Error";
				ResultDetails = "No dose calculated";
				DisplayColor = ResultColorChoices.Fail;
            }

			//get calc options
			Dictionary<string, string> photonOptions = plan.PhotonCalculationOptions;
			Dictionary<string, string> electronOptions = plan.ElectronCalculationOptions;

			//loop through beams to see what needs to be displayed
			foreach (Beam beam in plan.Beams.Where(x => !x.IsSetupField))
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

			// Photon Volume Dose Options:
			// CalculationGridSizeInCM
			// CalculationGridSizeInCMForSRSAndHyperArc
			// FieldNormalizationType
			// HeterogeneityCorrection
			if (photon)
			{
				ResultDetails += "Volume Dose: " + plan.PhotonCalculationModel.ToString();

				TestExplanation += $"Volume Dose: {plan.PhotonCalculationModel}\n";
				TestExplanation += String.Join("\n", photonOptions.Select(x => $"{AddSpaces(x.Key)}: {x.Value}"));

				if (IMRT)
				{
					ResultDetails += "\nIMRT Optimization: " + plan.GetCalculationModel(CalculationType.PhotonIMRTOptimization);
					ResultDetails += "\nLeaf Motion: " + plan.GetCalculationModel(CalculationType.PhotonLeafMotions);

					TestExplanation += $"\n\nIMRT Optimization: {plan.GetCalculationModel(CalculationType.PhotonIMRTOptimization)}\n";
					TestExplanation += String.Join("\n", plan.GetCalculationOptions(plan.GetCalculationModel(CalculationType.PhotonIMRTOptimization)).Select(x => $"{AddSpaces(x.Key)}: {x.Value}"));
					TestExplanation += $"\n\nLeaf Motion: {plan.GetCalculationModel(CalculationType.PhotonLeafMotions)}\n";
					TestExplanation += String.Join("\n", plan.GetCalculationOptions(plan.GetCalculationModel(CalculationType.PhotonLeafMotions)).Select(x => $"{AddSpaces(x.Key)}: {x.Value}"));
				}
				if (VMAT)
				{
					ResultDetails += "\nVMAT Optimization: " + plan.GetCalculationModel(CalculationType.PhotonVMATOptimization);

					TestExplanation += $"\n\nVMAT Optimization: {plan.GetCalculationModel(CalculationType.PhotonVMATOptimization)}\n";
					TestExplanation += String.Join("\n", plan.GetCalculationOptions(plan.GetCalculationModel(CalculationType.PhotonVMATOptimization)).Select(x => $"{AddSpaces(x.Key)}: {x.Value}"));
				}

				ResultDetails += "\nGrid Size: " + photonOptions["CalculationGridSizeInCM"] + "cm";
				ResultDetails += "\nHeterogeneity Corrections: " + photonOptions["HeterogeneityCorrection"];
			}

			// Electron Volume Dose Options:
			// CalculationGridSizeInCM
			// DoseThresholdForUncertainty
			// NumberOfParticleHistories
			// RandomGeneratorSeedNumber
			// SmoothingLevel
			// SmoothingMethod
			// StatisticalUncertainty
			// StatisticalUncertaintyLimit
			if (electron)
			{
				ResultDetails += "Volume Dose: " + plan.ElectronCalculationModel.ToString();
				ResultDetails += "\nGrid Size: " + electronOptions["CalculationGridSizeInCM"];
				ResultDetails += "\nUncertainty: " + electronOptions["StatisticalUncertainty"];
				ResultDetails += "\nSmooting Method: " + electronOptions["SmoothingMethod"];
				ResultDetails += "\nSmoothing Level: " + electronOptions["SmoothingLevel"];

				TestExplanation += $"Volume Dose: {plan.ElectronCalculationModel}\n";
                TestExplanation += String.Join("\n", electronOptions.Select(x => $"{AddSpaces(x.Key)}: {x.Value}"));
            }

			ResultDetails += "\n\nClick to see full model options";
		}
    }
}
