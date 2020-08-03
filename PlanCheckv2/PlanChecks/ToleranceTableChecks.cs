using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.TPS.Common.Model.API;

namespace VMS.TPS.PlanChecks
{
    public class ToleranceTableChecks : PlanCheck
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

				if (plan.Id.Contains("_5"))
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
						ResultColor = "Gold";
					}
				}

				ResultDetails += badFields;
				ResultDetails = ResultDetails.TrimEnd(' ');
				ResultDetails = ResultDetails.TrimEnd(',');

				//no issues found
				if (Result == "")
				{
					Result = "";
					ResultColor = "LimeGreen";
				}
			}
			#endregion

			#region Macomb / Clarkston
			// Same tolerance table selected for all fields
			else if (Department == Department.CLA ||
					 Department == Department.MAC)
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
						ResultColor = "Gold";
					}
				}

				//no issues found
				if (Result == "")
				{
					Result = "";
					ResultColor = "LimeGreen";
				}
			}
            #endregion

            #region Flint
			// OBI selected for setup fields
			// SRS/SRT for plans with "_5"
			// SBRT for plans with "_4"
			// TrueBeam for all other plans
            else if (Department == Department.FLT)
			{
				string tolTable;
				string badFields = "";

				if (plan.Id.Contains("_5"))
					tolTable = "SRS/SRT";
				else if (plan.Id.Contains("_4"))
					tolTable = "SBRT";
				else
					tolTable = "TrueBeam";

				//Check each field to make sure they're the same
				foreach (Beam field in plan.Beams)
				{
					if (field.IsSetupField)
					{
						if (!field.ToleranceTableLabel.Contains("OBI"))
						{
							Result = "Warning";
							ResultDetails = "OBI tolerance table not chosen for setup field";
							ResultColor = "Gold";
							break;
						}
					}
					else
					{
						if (ResultDetails == "")
							ResultDetails = field.ToleranceTableLabel;

						//wrong tolerance table
						if (field.ToleranceTableLabel != tolTable)
						{
							Result = "Warning";
							ResultDetails = $"Not all fields use the {tolTable} tolerance table: ";
							badFields += field.Id + ", ";
							ResultColor = "Gold";
						}
					}
				}

				ResultDetails += badFields;
				ResultDetails = ResultDetails.TrimEnd(' ');
				ResultDetails = ResultDetails.TrimEnd(',');

				//no issues found
				if (Result == "")
				{
					Result = "";
					ResultColor = "LimeGreen";
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
					if (field.IsSetupField)
					{
						if (field.ToleranceTableLabel.Contains("OBI"))
						{
							Result = "Warning";
							ResultDetails = "OBI tolerance table chosen for setup field of 21iX machine\n";
							ResultColor = "Gold";
							break;
						}
					}
					else
					{
						if (tolTable == "")
							tolTable = field.ToleranceTableLabel;

						//wrong tolerance table
						if (field.ToleranceTableLabel != tolTable)
						{
							Result = "Warning";
							ResultDetails = $"Not all fields use the {tolTable} tolerance table: ";
							badFields += field.Id + ", ";
							ResultColor = "Gold";
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
					ResultColor = "LimeGreen";
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
						ResultColor = "Gold";
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
					ResultColor = "LimeGreen";
				}
			}
            #endregion

            #region Northern
            else if (Department == Department.NOR)
			{
				//Check each field to make sure they're the same
				foreach (Beam field in plan.Beams)
				{
					if (field.IsSetupField)
					{
						if (field.ToleranceTableLabel != "IGRT")
						{
							Result = "Warning";
							ResultDetails = "IGRT tolerance table not chosen for setup field";
							ResultColor = "Gold";
							break;
						}
					}
					else
					{
						if (ResultDetails == "")
							ResultDetails = field.ToleranceTableLabel;
						else if (ResultDetails != field.ToleranceTableLabel)
						{
							Result = "Warning";
							ResultDetails = "Not all fields have the same tolerance table";
							ResultColor = "Gold";
						}
					}
				}

                //no issues found
                if (Result == "")
				{
					Result = "";
					ResultColor = "LimeGreen";
				}
			}
			#endregion

			else
				TestNotImplemented();
		}
    }
}
