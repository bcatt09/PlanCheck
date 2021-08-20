using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.TPS.Common.Model.API;

namespace PlanCheck.Checks
{
    class ProtonIsocenterMarkerChecks : PlanCheckBaseProton
    {
        protected override List<string> MachineExemptions => new List<string> { };

        public ProtonIsocenterMarkerChecks(PlanSetup plan) : base(plan) { }

        public override void RunTestProton(IonPlanSetup plan)
        {
            DisplayName = "Proton Isocenter Marker Present";
            ResultDetails = "";
            TestExplanation = "Checks to make sure an isocenter marker point is present in the structure set and is located at the beam isocenter";

            Structure isoMarker;

            // General test flow:
            //   - Check for marker structure
            int isoMarkerCount = plan.StructureSet.Structures.Where(s => s.DicomType.ToUpper() == "ISOCENTER").Count();

            //var isoStruct = plan.StructureSet.Structures.FirstOrDefault(s => s.Id.ToLower() == "isocenter marker"); (THIS WAS FOR DEBUGGING)

            // System.Windows.MessageBox.Show($"Isocount = {isoMarkerCount}");

            if (isoMarkerCount==1)
            {
                isoMarker = plan.StructureSet.Structures.FirstOrDefault(s => s.DicomType.ToUpper() == "ISOCENTER");

                double isoDeltaX = isoMarker.CenterPoint.x - plan.IonBeams.FirstOrDefault().IsocenterPosition.x;
                double isoDeltaY = isoMarker.CenterPoint.y - plan.IonBeams.FirstOrDefault().IsocenterPosition.y;
                double isoDeltaZ = isoMarker.CenterPoint.z - plan.IonBeams.FirstOrDefault().IsocenterPosition.z;

                
                if (isoDeltaX<=0.01 && isoDeltaY <= 0.01 && isoDeltaZ <= 0.01)
                {
                    Result = "Pass";
                    ResultDetails = "isocenter marker is present and within 0.01 somethings of beam center in all directions";
                    DisplayColor = ResultColorChoices.Pass;
                }
                else
                {
                    Result = "Fail";
                    ResultDetails = "Isocenter found but does not match beam center\n\n";
                    ResultDetails += $"Isocenter Marker X:{isoMarker.CenterPoint.x} Y:{isoMarker.CenterPoint.y} Z:{isoMarker.CenterPoint.z}\n";
                    ResultDetails += $"Beam Center X: {plan.Beams.FirstOrDefault().IsocenterPosition.x} Y:{plan.Beams.FirstOrDefault().IsocenterPosition.y} Z:{plan.Beams.FirstOrDefault().IsocenterPosition.z}";
                    DisplayColor = ResultColorChoices.Fail;
                }
            }
            else if (isoMarkerCount==0)
            {
                ResultDetails = "No Isocenter Marker found in structure set";
                DisplayColor = ResultColorChoices.Fail;
                Result = "Fail";
            }
            else if (isoMarkerCount > 1)
            {
                ResultDetails = "Potentially multiple isocenter markers in plan";
                DisplayColor = ResultColorChoices.Fail;
                Result = "Fail";
            }
        }
    }
}
