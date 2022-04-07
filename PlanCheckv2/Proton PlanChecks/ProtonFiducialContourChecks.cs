using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.TPS.Common.Model.API;

namespace PlanCheck.Checks
{
	public class ProtonFiducialContourChecks : PlanCheckBaseProton
	{
		protected override List<string> MachineExemptions => new List<string> { };

		public ProtonFiducialContourChecks(PlanSetup plan) : base(plan) { }

		public override void RunTestProton(IonPlanSetup plan)
		{

			DisplayName = "Fiducial Contour checks";
			TestExplanation = "Check if fudicials are contoured or not";
			string resultTemp = "";
			int numFiducials = 0;
			double assignedHU = 0;
			bool isContoured = true;
			bool isAssigned = true;

			foreach (Structure fiducial in plan.StructureSet.Structures.Where(x => x.Id.Contains("iducial")))
			{

				numFiducials++;

				if (fiducial.IsEmpty)
				{
					resultTemp += fiducial.Id + " doesn't have a contour" + "\n";
					isContoured = false;
				}
				else
				{
					if (fiducial.GetAssignedHU(out assignedHU))
					{
						resultTemp += fiducial.Id + " is contoured and is assigned a value of " + assignedHU + "\n";
					}
					else
					{
						resultTemp += fiducial.Id + " is contoured and is not assigned" + "\n";
						isAssigned = false;
					}
				}
			}

			if (!isContoured)
			{
				Result = "";
				ResultDetails = $"There are {numFiducials} contoured fiducials \n {resultTemp.TrimEnd('\n')}";
				DisplayColor = ResultColorChoices.Pass;
			}
			else
			{
				if (numFiducials == 0)
                {
					Result = "";
					ResultDetails = $"There are {numFiducials} fiducials \n {resultTemp.TrimEnd('\n')}";
					DisplayColor = ResultColorChoices.TestNotRun;
				}
				else
                {
					Result = "";
					ResultDetails = $"There are {numFiducials} fiducials \n {resultTemp.TrimEnd('\n')}";
					DisplayColor = ResultColorChoices.Fail;
				}
				
			}

		}

	}

}
