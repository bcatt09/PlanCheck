using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.TPS.Common.Model.API;

namespace PlanCheck.Checks
{
    public class ToleranceTableChecks : PlanCheckBase
	{
		protected override List<string> MachineExemptions => new List<string> { };

		public ToleranceTableChecks(PlanSetup plan) : base(plan) { }

        protected override void RunTest(PlanSetup plan)
        {
			DisplayName = "Tolerance Table";
			Result = "";
			ResultDetails = "";
			TestExplanation = "Checks that all fields use the correct tolerance table based on department standards";

			#region Port Huron
			// SRS/SRT for plans with "_5"
			// SBRT for plans with "_4"
			// Electron for electron plans
			// TB Photon for all other plans
			if (Department == Department.MPH)
			{
				string tolTable;
				string badFields = "";

				if (plan.Id.Contains("_5") || plan.StructureSet.Image.ZRes == 1) // Plan is single fraction (SRS) or has 1 mm slices (likely a brain SRS or SRT)
					tolTable = "SRS/SRT";
				else if (plan.Id.Contains("_4"))
					tolTable = "SBRT";
				else if (plan.Beams.Where(x => !x.IsSetupField).Where(x => x.EnergyModeDisplayName.Contains("E", StringComparison.CurrentCultureIgnoreCase)).Count() > 0)
					tolTable = "Electron";
				else
					tolTable = "TB Photon";

				//Check each field to make sure they're the same
				foreach (Beam field in plan.Beams)
				{
					if (ResultDetails == "")
						ResultDetails = field.ToleranceTableLabel;

					//wrong tolerance table
					if (field.ToleranceTableLabel != tolTable)
					{
						Result = "Warning";
						ResultDetails = $"Not all fields use the {tolTable} tolerance table: ";
						badFields += field.Id + ", ";
						DisplayColor = ResultColorChoices.Warn;
					}
				}

				ResultDetails += badFields;
				ResultDetails = ResultDetails.TrimEnd(' ');
				ResultDetails = ResultDetails.TrimEnd(',');

				//no issues found
				if (Result == "")
				{
					Result = "";
					DisplayColor = ResultColorChoices.Pass;
				}
			}
			#endregion

			#region Macomb / Clarkston / Central / Northern
			// Same tolerance table selected for all fields
			else if (Department == Department.CLA ||
					 Department == Department.MAC ||
					 Department == Department.CEN ||
					 Department == Department.NOR)
			{
				//Check each field to make sure they're the same
				foreach (Beam field in plan.Beams)
				{
					if (ResultDetails == "")
						ResultDetails = field.ToleranceTableLabel;
					else if (ResultDetails != field.ToleranceTableLabel)
					{
						Result = "Warning";
						ResultDetails = "Not all fields have the same tolerance table";
						DisplayColor = ResultColorChoices.Warn;
					}
				}

				//no issues found
				if (Result == "")
				{
					Result = "";
					DisplayColor = ResultColorChoices.Pass;
				}
			}
            #endregion

            #region Flint

			// TrueBeam for all plans

            else if (Department == Department.FLT)
			{
				string tolTable;
				string txFieldsResult = "";
				string badTxFields = "";
				string setupFieldsResult = "";
				string badSetupFields = "";

				if (plan.Id.Contains("_5")) // SRS Plan
					tolTable = "TrueBeam";
				else if (plan.Id.Contains("_4")) // SBRT Plan
					tolTable = "TrueBeam";
				else
					tolTable = "TrueBeam";

				//Check each field to make sure they're the same
				foreach (Beam field in plan.Beams)
				{
					if (field.IsSetupField)
					{
						if (!field.ToleranceTableLabel.Contains("TrueBeam"))
						{
							Result = "Warning";
							setupFieldsResult = "OBI tolerance table not chosen for setup field: ";
							badSetupFields += $"{field.Id}, ";
							DisplayColor = ResultColorChoices.Warn;
						}
					}
					else  // not a setup field
					{
						if (ResultDetails == "")
							ResultDetails = field.ToleranceTableLabel;

						//wrong tolerance table
						if (field.ToleranceTableLabel != tolTable)
						{
							Result = "Warning";
							txFieldsResult = $"Not all fields use the {tolTable} tolerance table: ";
							badTxFields += $"{field.Id}, ";
							DisplayColor = ResultColorChoices.Warn;
						}
					}
				}

				if (setupFieldsResult != "" || txFieldsResult != "") ResultDetails += "\n";
				ResultDetails += $"{setupFieldsResult}{badSetupFields}";
				ResultDetails = ResultDetails.TrimEnd(' ');
				ResultDetails = ResultDetails.TrimEnd(',');
				if (setupFieldsResult != "") ResultDetails += "\n";
				ResultDetails += $"{txFieldsResult}{badTxFields}";
				ResultDetails = ResultDetails.TrimEnd(' ');
				ResultDetails = ResultDetails.TrimEnd(',');

				//no issues found
				if (Result == "")
				{
					Result = "";
					DisplayColor = ResultColorChoices.Pass;
				}
			}
            #endregion

            #region Lapeer/Owosso
			// OBI for setup fields
			// Same tolerance table selected for all treatment fields
            else if (Department == Department.LAP || 
					 Department == Department.OWO)
			{
				string tolTable = "";
				string badFields = "";

				//Check each field to make sure they're the same
				foreach (Beam field in plan.Beams)
				{
					//if (field.IsSetupField)
					//{
					//	if (!field.ToleranceTableLabel.Contains("OBI"))
					//	{
					//		Result = "Warning";
					//		ResultDetails = "OBI tolerance table chosen for setup field of 21iX machine\n";
					//		DisplayColor = ResultColorChoices.Warn;
					//		break;
					//	}
					//}
					//else
					{
						if (tolTable == "")
							tolTable = field.ToleranceTableLabel;

						//wrong tolerance table
						if (field.ToleranceTableLabel != tolTable)
						{
							Result = "Warning";
							ResultDetails = $"Not all fields use the {tolTable} tolerance table: ";
							badFields += field.Id + ", ";
							DisplayColor = ResultColorChoices.Warn;
						}
					}
				}

				ResultDetails += badFields;
				ResultDetails = ResultDetails.TrimEnd(' ');
				ResultDetails = ResultDetails.TrimEnd(',');
				if (ResultDetails != "") ResultDetails += '\n';
				ResultDetails += tolTable;

				//no issues found
				if (Result == "")
				{
					Result = "";
					DisplayColor = ResultColorChoices.Pass;
				}
			}
			#endregion

			#region Detroit Group/Lansing
			// 01 SRS for plans with "_5"
			// 02 SBRT for plans with "_4"
			// Same tolerance table selected for all treatment fields otherwise
			else if (Department == Department.DET ||
					 Department == Department.FAR ||
					 Department == Department.LAN)
			{
				string tolTable = "";
				string badFields = "";

				if (plan.Id.Contains("_5"))
					tolTable = "01 SRS";
				else if (plan.Id.Contains("_4"))
					tolTable = "02 SBRT";

				//Check each field to make sure they're the same
				foreach (Beam field in plan.Beams)
				{
					if (tolTable == "")
						tolTable = field.ToleranceTableLabel;

					//wrong tolerance table
					if (field.ToleranceTableLabel != tolTable)
					{
						Result = "Warning";
						ResultDetails = $"Not all fields use the {tolTable} tolerance table: ";
						badFields += field.Id + ", ";
						DisplayColor = ResultColorChoices.Warn;
					}
				}

				ResultDetails += badFields;
				ResultDetails = ResultDetails.TrimEnd(' ');
				ResultDetails = ResultDetails.TrimEnd(',');
				if (ResultDetails == "")
					ResultDetails = tolTable;

				//no issues found
				if (Result == "")
				{
					Result = "";
					DisplayColor = ResultColorChoices.Pass;
				}
			}
            #endregion

			else
				TestNotImplemented();
		}
    }
}
