using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.TPS.Common.Model.API;
using VMS.TPS.Common.Model.Types;

namespace PlanCheck.Checks
{
    public class PrecriptionChecks : PlanCheckBase
    {
        protected override List<string> MachineExemptions => new List<string> { };

        public PrecriptionChecks(PlanSetup plan) : base(plan) { }

        public override void RunTest(PlanSetup plan)
        {
            DisplayName = "Prescription";
            TestExplanation = "Displays plan dose information from Eclipse and checks it versus the prescription in Aria";

            // Abort test quickly if there is no attached prescription

            if (plan.RTPrescription == null)
            {
                Result = "";
                ResultDetails = "No prescription attached to the plan";
                DisplayColor = ResultColorChoices.Fail;

                return;
            }



            // Some work on an Alternate Display
            string headerText = $"{cellFormatter("Parameter","left")}|{cellFormatter("Rx Value","middle")}|{cellFormatter("Plan Value","right")}\n" +
                                "----------------------------------------------------------------------------------\n";

            ////////////////////////////
            // Getting Rx Information //
            ////////////////////////////

            string rxName = plan.RTPrescription.Name,
                   rxId = plan.RTPrescription.Id,
                   rxEnergies = string.Join(",", plan.RTPrescription.Energies),
                   rxModalities = String.Join(",", plan.RTPrescription.EnergyModes), 
                   rxTechnique = plan.RTPrescription.Technique, // Plan technique string has potential to be modified during the evaluation phase (proton will shorten and ammend mfo/sfo
                   rxGating = plan.RTPrescription.Gating,
                   rxBolus = plan.RTPrescription.BolusThickness,
                   rxNotes = plan.RTPrescription.Notes;

            int rxFractions = (int)plan.RTPrescription.NumberOfFractions;

            string rxPrimaryTargetVolume = "", rxSecondaryTargetVolumes = "";
            double rxPrimaryTotalDose = 0;
            double rxPrimaryDosePerFraction = 0; 
            string rxSecondaryTotalDoses = "";
            string rxSecondaryDosePerFractions = "";

            foreach (var target in plan.RTPrescription.Targets.OrderByDescending(x => x.DosePerFraction * x.NumberOfFractions))
            {
                if (rxPrimaryTargetVolume == "")
                {
                    rxPrimaryTargetVolume = target.TargetId;
                    rxPrimaryDosePerFraction = target.DosePerFraction.Dose;
                    rxPrimaryTotalDose = rxPrimaryDosePerFraction * rxFractions;
                }
                else
                {
                    rxSecondaryTargetVolumes += target.TargetId + " ";
                    rxSecondaryDosePerFractions += target.DosePerFraction.Dose.ToString() + " ";
                    rxSecondaryTotalDoses += (target.NumberOfFractions * target.DosePerFraction.Dose).ToString() + " ";
                }
            }


            ////////////////////
            /// Getting the Plan information
            /// ///////////////////////
            /// 


            string planPrimaryTarget = plan.TargetVolumeID;
            int planNumberOfFractions = (int)plan.NumberOfFractions;
            double planDosePerFraction = plan.DosePerFraction.Dose;
            double planDose = planNumberOfFractions * planDosePerFraction;
            string planID = plan.Id;
            string planTechnique="";
            string planGating = plan.UseGating.ToString();
            string planModalities = "";
            string planEnergies = "";

            // Techniques, Modalities, and Energies
            bool electron = false,
                 photon = false,
                 IMRT = false,
                 VMAT = false,
                 Stereo = false,
                 proton = false;

            foreach (Beam b in plan.Beams.Where(x => !x.IsSetupField))
            {
                if (!planTechnique.Contains(b.Technique.ToString()))
                {
                    planTechnique += b.Technique.ToString();
                }
                if (!planEnergies.Contains(b.EnergyModeDisplayName.ToString()))
                {
                    planEnergies += b.EnergyModeDisplayName .ToString();
                }
                if (b.EnergyModeDisplayName.Contains('E'))
                    if (!planModalities.Contains("Electron")) planModalities += "Electron ";
                
                else if (b.EnergyModeDisplayName.Contains('X'))
                    if (!planModalities.Contains("Photon")) planModalities += "Photon ";

                if (b.MLCPlanType == MLCPlanType.DoseDynamic && b.ControlPoints.Count > 18)
                    IMRT = true;
                else if (b.MLCPlanType == MLCPlanType.VMAT)
                    VMAT = true;

                if (b.Technique.Id.Contains("Stereo") || b.Technique.Id.Contains("SRS"))
                    Stereo = true;
            }

            if (planTechnique.Equals("MODULAT_SCANNING"))
            {
                planTechnique = $"Scanning";
            }



            // 




            //////////////////////////////////////
            /// evaluation Section
            /////////////////////////////////////

            // Variables (Prefixes to be put before)
            // + = good, * = issue, no prefix = unchecked

            string prefixId = "",
                   prefixTechnique = "",
                   prefixModes = "",
                   prefixEnergies = "",
                   prefixPrimTarget = "",
                   prefixFractoins = "",
                   prefixDperF = "",
                   prefixTotalDose = "",
                   prefixGating = "",
                   prefixBolus = "";
                   
            // Fractions
            if (rxFractions == planNumberOfFractions)
            {
                prefixFractoins = "+";
            }
            else
            {
                prefixFractoins = "-";
                Result = "Review for Potential Mismatch";
                DisplayColor = ResultColorChoices.Warn;
            }

            // Dose/Fx
            if (rxPrimaryDosePerFraction == planDosePerFraction)
            {
                prefixDperF="+";
            }
            else
            {
                prefixDperF = "-";
                Result = "Review for Potential Mismatch";
                DisplayColor = ResultColorChoices.Warn;
            }
            // Total Dose
            if (rxPrimaryTotalDose==planDose)
            {
                prefixTotalDose="+";
            }
            else
            {
                prefixTotalDose="-";
                Result = "Review for Potential Mismatch";
                DisplayColor = ResultColorChoices.Warn;
            }



            ////////////////////////////////////////////
            /// Start Building Out the Display /////////
            ////////////////////////////////////////////

            ResultDetails += $"{headerText}";

            ResultDetails += $"{cellFormatter("ID/Name","left")}|{cellFormatter(rxId,"middle")}|{cellFormatter(planID,"right")}\n";
            ResultDetails += $"{cellFormatter("Technique", "left")}|{cellFormatter(rxTechnique, "middle")}|{cellFormatter(planTechnique, "right")}\n";
            ResultDetails += $"{cellFormatter("Modes", "left")}|{cellFormatter(rxModalities, "middle")}|{cellFormatter("coming soon", "right")}\n";
            ResultDetails += $"{cellFormatter("Energies", "left")}|{cellFormatter(rxEnergies, "middle")}|{cellFormatter(planEnergies, "right")}\n";
            ResultDetails += $"{cellFormatter("PrimTarget", "left")}|{cellFormatter(rxPrimaryTargetVolume, "middle")}|{cellFormatter(planPrimaryTarget, "right")}\n";
            ResultDetails += $"{cellFormatter(String.Concat(prefixFractoins,"Fractions"), "left")}|{cellFormatter(rxFractions.ToString(), "middle")}|{cellFormatter(planNumberOfFractions.ToString(), "right")}\n";
            ResultDetails += $"{cellFormatter(String.Concat(prefixDperF, "Dose/Fx"), "left")}|{cellFormatter(rxPrimaryDosePerFraction.ToString(), "middle")}|{cellFormatter(planDosePerFraction.ToString(), "right")}\n";
            ResultDetails += $"{cellFormatter(String.Concat(prefixTotalDose, "Total Dose"), "left")}|{cellFormatter(rxPrimaryTotalDose.ToString(), "middle")}|{cellFormatter(planDose.ToString(), "right")}\n";
            ResultDetails += $"{cellFormatter("Gating", "left")}|{cellFormatter(rxGating, "middle")}|{cellFormatter(planGating, "right")}\n";
            ResultDetails += $"{cellFormatter("Bolus", "left")}|{cellFormatter(rxBolus, "middle")}|{cellFormatter("coming soon", "right")}\n";



            //if (plan.NumberOfFractions != rx.NumberOfFractions || plan.DosePerFraction != rx.DosePerFraction)
            //{
            //    Result = "Warning";
            //    ResultDetails = $"Plan dose does not match prescription\n\nPrescription:\n{rx.DosePerFraction} x {rx.NumberOfFractions} Fx = {rx.DosePerFraction * rx.NumberOfFractions}\n\nPlan:\n{plan.DosePerFraction} x {plan.NumberOfFractions} Fx = {plan.TotalDose.ToString()}\n\nPrescribed Percentage: {(plan.TreatmentPercentage * 100.0).ToString("0.0")}%\nPlan Normalization: {plan.PlanNormalizationValue.ToString("0.0")}%";
            //    DisplayColor = ResultColorChoices.Warn;
            //}
            //else if (plan.TreatmentPercentage < 0.9 || plan.TreatmentPercentage > 1.1)
            //{
            //    Result = "Warning";
            //    ResultDetails = $"Treatment percentage is outside of ±10%\nTreatment Percentage: {plan.TreatmentPercentage:P1}%";
            //    DisplayColor = ResultColorChoices.Warn;
            //}
            //else
            //{
            //    Result = "";
            //    ResultDetails = $"{plan.DosePerFraction} x {plan.NumberOfFractions} Fx = {plan.TotalDose}\nPrescribed Percentage: {(plan.TreatmentPercentage * 100.0).ToString("0.0")}%\nPlan Normalization: {plan.PlanNormalizationValue.ToString("0.0")}%";
            //    DisplayColor = ResultColorChoices.Pass;
            //}
        }

        private string cellFormatter(string cellText, string colLoc)
        {
            string formattedCellText = "";
            Font resultDisplayFont = new Font("Segoe UI", 12, FontStyle.Regular);
            
            Bitmap b = new Bitmap(200,300);
            Graphics g = Graphics.FromImage(b);
            SizeF textSize = new SizeF();
            textSize = g.MeasureString(cellText, resultDisplayFont);
            double textWidth = textSize.Width;

            // Keep line in case formatting messes up to find how wide the culprit text is
            //if (cellText == "Left Breast") { System.Windows.MessageBox.Show($"{cellText} has {textWidth} width"); }

            switch (colLoc)
            {
                case "left":
                    
                    if (textWidth <= 75)
                    {
                        formattedCellText = cellText.PadRight(cellText.Length + 2, '\t');
                    }
                    else
                    {
                        formattedCellText = cellText.PadRight(cellText.Length + 1, '\t');
                    }
                    break;
                case "middle":
                case "right":
                    formattedCellText = cellText.PadLeft(cellText.Length + 1, '\t');
                    if (textWidth <= 75)
                    {
                        formattedCellText = cellText.PadLeft(cellText.Length + 1, '\t').PadRight(cellText.Length + 3, '\t');
                    }
                    else
                    {
                        formattedCellText = cellText.PadLeft(cellText.Length + 1, '\t').PadRight(cellText.Length + 2, '\t');
                    }
                    break;
                default:
                    break;
            }

            return formattedCellText;


        }
    }
}
