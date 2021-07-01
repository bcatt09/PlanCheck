using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.TPS.Common.Model.API;

namespace PlanCheck.Checks
{
    public class BolusChecks : PlanCheckBasePhoton
    {
        protected override List<string> MachineExemptions => new List<string> {
			DepartmentInfo.MachineNames.PRO_G1,
			DepartmentInfo.MachineNames.PRO_G2
		};

        public BolusChecks(PlanSetup plan) : base(plan) { }

        public override void RunTestPhoton(ExternalPlanSetup plan)
		{
			DisplayName = "Bolus";
			Result = "";
			ResultDetails = "";
			TestExplanation = "Checks that each field has a linked bolus if a bolus exists";

			string bolus = "";
			bool containsBolus = false;
			bool containsMultiple = false;
			string resultDetailsMultiPerFieldLine = "";
			string resultDetailsMultiPerPlanLine = "";

			//Check to see if plan contains a bolus
			foreach (Structure struc in plan.StructureSet.Structures)
			{
				if (struc.DicomType == "BOLUS")
				{
					//if it's already found one bolus, then there are multiple
					if (containsBolus)
						containsMultiple = true;

					containsBolus = true;
				}
			}

			//Check each field to make sure it has a bolus attached
			foreach (Beam field in plan.Beams)
			{
				if (!containsBolus)
					break;
				if (!field.IsSetupField)
				{
					//no bolus
					if (field.Boluses.Count() == 0)
					{
						//Set up "no bolus linked" string
						if (ResultDetails == "")
						{
							Result = "Warning";
							ResultDetails = "Some fields do not have a linked bolus: ";
							DisplayColor = ResultColorChoices.Warn;
						}
						ResultDetails += field.Id + ", ";
					}
					//more than 1 bolus
					else if (field.Boluses.Count() > 1)
					{
						//set up "multiple boluses" string
						if (resultDetailsMultiPerFieldLine == "")
						{
							Result = "Warning";
							resultDetailsMultiPerFieldLine = "Some fields have more than one bolus linked: ";
							DisplayColor = ResultColorChoices.Warn;
						}
						resultDetailsMultiPerFieldLine += field.Id + ", ";
					}
					//just one bolus
					else
					{
						//if this is the first bolus found, save it
						if (bolus == "")
						{
							bolus = field.Boluses.First().Id;
						}
						//if not make sure it's the same bolus used on other fields
						else if (field.Boluses.First().Id != bolus)
						{
							//set up "multiple boluses" string
							if (resultDetailsMultiPerPlanLine == "")
							{
								Result = "Warning";
								resultDetailsMultiPerPlanLine = $"Multiple bolus structures linked in plan: {bolus}, ";
								DisplayColor = ResultColorChoices.Warn;
							}
							resultDetailsMultiPerPlanLine += field.Boluses.First().Id + ", ";
						}
					}
				}
			}

			//no bolus in plan so it's good
			if (!containsBolus)
			{
				Result = "";
				ResultDetails = "No bolus in structure set";
				DisplayColor = ResultColorChoices.Pass;
			}
			//no issues found
			else if (Result == "")
			{
				Result = "";
				ResultDetails = $"{bolus} attached to all fields";
				DisplayColor = ResultColorChoices.Pass;
			}

			//clean up strings
			ResultDetails = ResultDetails.TrimEnd(' ');
			ResultDetails = ResultDetails.TrimEnd(',');
			resultDetailsMultiPerFieldLine = resultDetailsMultiPerFieldLine.TrimEnd(' ');
			resultDetailsMultiPerFieldLine = resultDetailsMultiPerFieldLine.TrimEnd(',');
			resultDetailsMultiPerPlanLine = resultDetailsMultiPerPlanLine.TrimEnd(' ');
			resultDetailsMultiPerPlanLine = resultDetailsMultiPerPlanLine.TrimEnd(',');
			if (resultDetailsMultiPerFieldLine != "")
				ResultDetails += '\n' + resultDetailsMultiPerFieldLine;
			if (resultDetailsMultiPerPlanLine != "")
				ResultDetails += '\n' + resultDetailsMultiPerPlanLine;
			ResultDetails = ResultDetails.TrimStart('\n');
			ResultDetails = ResultDetails.TrimStart('\n');

			//multiple boluses in structure set, so put a warning at the end
			if (containsMultiple)
			{
				Result = "Warning";
				ResultDetails += "\nMultiple bolus structures in the structure set, please ensure that the correct one is used";
				DisplayColor = ResultColorChoices.Warn;
			}
		}
    }
}
