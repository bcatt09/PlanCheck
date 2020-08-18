using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.TPS.Common.Model.API;
using VMS.TPS.Common.Model.Types;

namespace VMS.TPS.PlanChecks
{
    class IsocenterChecks : PlanCheck
    {
        protected override List<string> MachineExemptions => new List<string> { };

        public IsocenterChecks(PlanSetup plan) : base(plan) { }

        protected override void RunTest(PlanSetup plan)
        {
			DisplayName = "Isocenter";
			ResultDetails = "";
			TestExplanation = "Checks that only a single isocenter exists in the plan\nAlso suggests using G180E if the isocenter is shifted >2cm to patient's right";

			int isocenters = 1;


			VVector firstIso = plan.Beams.FirstOrDefault().IsocenterPosition;

			foreach (Beam field in plan.Beams)
			{
				//check for multiple isocenters
				if (field != plan.Beams.First())
				{
					//if there's no distance between the two, they are equal
					if (VVector.Distance(field.IsocenterPosition, firstIso) != 0)
					{
						isocenters++;
					}
				}

				//check if 180E might be necessary
				if (!field.IsSetupField && field.ControlPoints.First().GantryAngle == 180)
				{
					try
					{
						Structure body = (from s in plan.StructureSet.Structures where (s.DicomType == "BODY" || s.DicomType == "EXTERNAL") select s).First();

						//Looks for the isocenter position to be 20 mm to the left when facing the linac. Positions are in mm
						if (field.IsocenterPosition.x - body.CenterPoint.x < -20 && (plan.TreatmentOrientation == PatientOrientation.HeadFirstSupine || plan.TreatmentOrientation == PatientOrientation.FeetFirstProne))
						{
							Result = "Warning";
							ResultDetails += $"Isocenter is shifted to patients right, do you want to use {field.GantryAngleToUser(180)}E?\n";
							ResultColor = "Gold";
						}
						if (field.IsocenterPosition.x - body.CenterPoint.x > 20 && (plan.TreatmentOrientation == PatientOrientation.HeadFirstProne || plan.TreatmentOrientation == PatientOrientation.FeetFirstSupine))
						{
							Result = "Warning";
							ResultDetails += $"Isocenter is shifted to patients left, do you want to use {field.GantryAngleToUser(180)}E?\n";
							ResultColor = "Gold";
						}
					}
					catch
					{
						TestCouldNotComplete("CheckIsocenterPosition - No structure with type \"BODY\" or \"EXTERNAL\" found");
					}
				}
			}

			if (isocenters > 1)
			{
				Result = "Warning";
				ResultDetails += $"{isocenters} isocenters detected, please check plan\n";
				ResultColor = "Gold";
			}

			ResultDetails = ResultDetails.TrimEnd('\n');

			//no problems found
			if (ResultDetails == "")
			{
				Result = "Pass";
				ResultColor = "LimeGreen";
			}
		}
    }
}
