using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VMS.TPS.Common.Model.API;

namespace PlanCheck.Checks
{
    public class UseGatedChecks : PlanCheckBasePhoton
    {
        protected override List<string> MachineExemptions => new List<string> { 
            DepartmentInfo.MachineNames.CEN_EX, 
            DepartmentInfo.MachineNames.CLA_EX, 
            DepartmentInfo.MachineNames.DET_IX, 
            DepartmentInfo.MachineNames.FAR_IX,
            DepartmentInfo.MachineNames.FLT_BackTB,
            DepartmentInfo.MachineNames.FLT_FrontTB,
            DepartmentInfo.MachineNames.LAN_IX,
            DepartmentInfo.MachineNames.MAC_IX,
            DepartmentInfo.MachineNames.NOR_EX,
            DepartmentInfo.MachineNames.NOR_IX,
            DepartmentInfo.MachineNames.NOR_TB,
            DepartmentInfo.MachineNames.PRO_G1,
            DepartmentInfo.MachineNames.PRO_G2
        };

        public UseGatedChecks(PlanSetup plan) : base(plan) { }
        
        public override void RunTestPhoton(ExternalPlanSetup plan)
        {
            DisplayName = "\"Use Gated\"?";
            TestExplanation = "Checks to see if \"Use Gated\" should be checked off in plan properties based on department standards";

            if (MachineID == DepartmentInfo.MachineNames.CLA_TB ||
                MachineID == DepartmentInfo.MachineNames.MPH_TB ||
                MachineID == DepartmentInfo.MachineNames.MAC_TB)
            {
                string planningImageComment = plan.StructureSet.Image.Comment;
                string planningImageId = plan.StructureSet.Image.Id;
                int? numberOfFractions = plan.NumberOfFractions;

                if (numberOfFractions < 10 && (planningImageComment.Contains("AIP", StringComparison.CurrentCultureIgnoreCase) || planningImageComment.Contains("avg", StringComparison.CurrentCultureIgnoreCase) || planningImageId.Contains("AIP", StringComparison.CurrentCultureIgnoreCase) || planningImageId.Contains("avg", StringComparison.CurrentCultureIgnoreCase) || planningImageId.Contains("ave", StringComparison.CurrentCultureIgnoreCase) || planningImageComment.Contains("%")))
                {
                    if (plan.UseGating)
                    {
                        Result = "";
                        ResultDetails = "\"Use Gating\" is checked";
                        DisplayColor = ResultColorChoices.Pass;
                    }
                    else
                    {
                        Result = "Warning";
                        ResultDetails = "Plan has a low number of fractions and looks to contain a 4D image.  Should \"Use Gated\" be checked?";
                        DisplayColor = ResultColorChoices.Warn;
                    }
                }
                else
                {
                    Result = "Pass";
                    DisplayColor = ResultColorChoices.Pass;
                }
            }
            else if (MachineID == DepartmentInfo.MachineNames.LAP_IX ||
                     MachineID == DepartmentInfo.MachineNames.OWO_IX)
            {
                bool DIBH = plan.StructureSet.Image.Series.Comment.ToLower().Contains("dibh");

                if (DIBH)
                {
                    if (plan.UseGating)
                    {
                        Result = "";
                        ResultDetails = "\"Use Gated\" is checked";
                        DisplayColor = ResultColorChoices.Pass;
                    }
                    else
                    {
                        Result = "Warning";
                        ResultDetails = "\"Use Gated\" is not checked and the plan appears to be DIBH";
                        DisplayColor = ResultColorChoices.Warn;
                    }
                }
                else
                {
                    Result = "Pass";
                    DisplayColor = ResultColorChoices.Pass;
                }
            }
            else
                TestNotImplemented();
        }
    }
}
