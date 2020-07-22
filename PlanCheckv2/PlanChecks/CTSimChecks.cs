using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.TPS.Common.Model.API;

namespace VMS.TPS.PlanChecks
{
    public class CTSimChecks : PlanCheck
	{
		protected override List<string> MachineExemptions => new List<string> { };

		public CTSimChecks(PlanSetup plan) : base(plan) { }

        protected override void RunTest(PlanSetup plan)
        {
			DisplayName = "CT Sim";
			ResultDetails = "";
			TestExplanation = "Checks that the correct CT is chosen for the image series based on what treatment unit is used for planning\nWarning: This will not correctly account for patients simmed at one facility and treated at another";

			string ct = plan.StructureSet.Image.Series.ImagingDeviceId;

			if (!DepartmentInfo.GetCTIDs(Department).Contains(ct))
			{
				Result = "Warning";
				ResultDetails = $"Patient is being treated on {MachineID}, but the {String.Join(" or ", DepartmentInfo.GetCTIDs(Department))} was not chosen as the imaging device for the series, please check";
				ResultDetails += $"\nSelected CT: {ct}\n";
				ResultColor = "Gold";
			}

			if (ResultDetails == "")
			{
				Result = "";
				ResultDetails = ct;
				ResultColor = "LimeGreen";
			}

			ResultDetails = ResultDetails.TrimEnd('\n');
		}
    }
}
