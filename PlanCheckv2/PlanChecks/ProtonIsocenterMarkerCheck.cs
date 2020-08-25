using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.TPS.Common.Model.API;
using VMS.TPS.Common.Model.Types;

namespace VMS.TPS.PlanChecks
{
    class ProtonIsocenterMarkerCheck : PlanCheck
    {
        protected override List<string> MachineExemptions => new List<string> { };

        public ProtonIsocenterMarkerCheck(PlanSetup plan) : base(plan) { }

        protected override void RunTest(PlanSetup plan)
        {
            DisplayName = "Proton Isocenter Marker Present";
            ResultDetails = "";
            TestExplanation = "Checks to make sure an isocenter marker point is present in the structure set and is located at the beam isocenter";

            Structure isoMarker;

            // General test flow:
            //   - Check for marker structure
            int isoMarkerCount = plan.StructureSet.Structures.Where(s => s.DicomType == "MARKER" && s.Id.ToLower().Contains("isocenter")).Count();

            if (isoMarkerCount==1)
            {
                isoMarker = plan.StructureSet.Structures.FirstOrDefault(s => s.DicomType == "MARKER" && s.Id.ToLower().Contains("isocenter"));

                double isoDeltaX = isoMarker.CenterPoint.x - plan.Beams.FirstOrDefault().IsocenterPosition.x;
                double isoDeltaY = isoMarker.CenterPoint.y - plan.Beams.FirstOrDefault().IsocenterPosition.y;
                double isoDeltaZ = isoMarker.CenterPoint.z - plan.Beams.FirstOrDefault().IsocenterPosition.z;

                if (isoDeltaX<=0.01 && isoDeltaY <= 0.01 && isoDeltaZ <= 0.01)
                {
                    Result = "Pass";
                    ResultDetails = "isocenter marker is present and within 0.01 somethings of beam center in all directions";
                    ResultColor = "LimeGreen";
                }
                else
                {
                    Result = "Fail";
                    ResultDetails = "Isocenter found but does not match beam center\n\n";
                    ResultDetails += $"Isocenter Marker X:{isoMarker.CenterPoint.x} Y:{isoMarker.CenterPoint.y} Z:{isoMarker.CenterPoint.z}\n";
                    ResultDetails += $"Beam Center X: {plan.Beams.FirstOrDefault().IsocenterPosition.x} Y:{plan.Beams.FirstOrDefault().IsocenterPosition.y} Z:{plan.Beams.FirstOrDefault().IsocenterPosition.z}";
                }
            }
            else if (isoMarkerCount==0)
            {
                ResultDetails = "No Isocenter Marker found in structure set";
                ResultColor = "Red";
                Result = "Fail";
            }
            else if (isoMarkerCount > 1)
            {
                ResultDetails = "Potentially multiple isocenter markers in plan";
                ResultColor = "Red";
                Result = "Fail";
            }
        }
    }
}
