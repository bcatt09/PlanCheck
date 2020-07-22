using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.TPS.Common.Model.API;
using VMS.TPS.Common.Model.Types;

namespace VMS.TPS.PlanChecks
{
    public class Shifts : PlanCheck
    {
        protected override List<string> MachineExemptions => new List<string> { };

        public Shifts(PlanSetup plan) : base(plan) { }

        protected override void RunTest(PlanSetup plan)
		{
			DisplayName = "Patient Shifts";
			TestExplanation = "Displays shifts from Marker Structure or User Origin";
			Result = "";
			ResultDetails = "";
			ResultColor = "LimeGreen";

			PatientOrientation orientation = plan.TreatmentOrientation;

			// get location of user origin and plan isocenter
			VVector tattoos = plan.StructureSet.Image.UserOrigin;
			VVector isocenter = plan.Beams.First().IsocenterPosition;

			// calculated shift distance from user origin
			VVector shift = isocenter - tattoos;
			string shiftFrom = "User Origin";

			// these sites set iso at sim and import in a "MARKER" structure that shifts will be based off (also they don't use gold markers, so there's no need to worry about those "MARKER" structures)
			if (Department == Department.MPH ||
				Department == Department.FLT ||
				Department == Department.LAP ||
				Department == Department.OWO ||
				Department == Department.DET ||
				Department == Department.FAR)
			{
				// loop through each patient marker and see if it's closer to the iso than the user origin and if it is use that for the calculated shift
				foreach (Structure point in plan.StructureSet.Structures.Where(x => x.DicomType == "MARKER"))
				{
					if (Math.Round((isocenter - point.CenterPoint).Length, 2) <= Math.Round(shift.Length, 2))
					{
						shift = isocenter - point.CenterPoint;
						shiftFrom = point.Id;
					}
				}
			}

			//round it off to prevent very small numbers from appearing and convert to cm for shifts
			shift.x = Math.Round(shift.x / 10, 1);
			shift.y = Math.Round(shift.y / 10, 1);
			shift.z = Math.Round(shift.z / 10, 1);

			if (shift.Length == 0)
				ResultDetails = $"No shifts from {shiftFrom}";
			else
			{
				//x-axis
				if (shift.x > 0)
					ResultDetails += $"Patient left: {shift.x.ToString("0.0")} cm\n";
				else if (shift.x < 0)
					ResultDetails += $"Patient right: {(-shift.x).ToString("0.0")} cm\n";

				//z-axis
				if (shift.z > 0)
					ResultDetails += $"Patient superior: {shift.z.ToString("0.0")} cm\n";
				else if (shift.z < 0)
					ResultDetails += $"Patient inferior: {(-shift.z).ToString("0.0")} cm\n";

				//y-axis
				if (shift.y > 0)
					ResultDetails += $"Patient posterior: {shift.y.ToString("0.0")} cm\n";
				else if (shift.y < 0)
					ResultDetails += $"Patient anterior: {(-shift.y).ToString("0.0")} cm\n";


				//remove negatives
				ResultDetails.Replace("-", string.Empty);

				ResultDetails = $"Shifts from {shiftFrom}\n" + ResultDetails;
			}

			ResultDetails = ResultDetails.TrimEnd('\n');
		}
    }
}
