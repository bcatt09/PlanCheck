using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.TPS.Common.Model.API;
using VMS.TPS.Common.Model.Types;

namespace PlanCheck.Checks
{
    class DoseRateChecks : PlanCheckBasePhoton
	{
        protected override List<string> MachineExemptions => new List<string> { };

		public DoseRateChecks(PlanSetup plan) : base(plan) { }

        public override void RunTestPhoton(ExternalPlanSetup plan)
        {
			DisplayName = "Dose Rate";
			ResultDetails = "";
			TestExplanation = "Checks that all dose rates are set to maximum allowed per department standards";

			#region Macomb Group and Northern TrueBeam
			// Flattened - 600
			// 6FFF      - 1400
			// 10FFF     - 2400
			// Electron  - 1000
			if (Department == Department.CLA ||
				Department == Department.MAC ||
				Department == Department.MPH ||
				(Department == Department.NOR && MachineID == DepartmentInfo.MachineNames.NOR_TB))
			{
				foreach (Beam field in plan.Beams)
				{
					//ignore setup fields
					if (!field.IsSetupField)
					{
						string energy = field.EnergyModeDisplayName;

						if (energy == "6X" || energy == "10X" || energy == "15X" || energy == "16X" || energy == "18X" || energy == "23X")
						{
							if (field.DoseRate < 600)
							{
								Result = "Warning";
								ResultDetails += field.Id + " dose rate set at " + field.DoseRate + "\n";
								DisplayColor = ResultColorChoices.Warn;
							}
						}
						else if (energy == "6X-FFF")
						{
							if (field.DoseRate < 1400)
							{
								Result = "Warning";
								ResultDetails += field.Id + " dose rate set at " + field.DoseRate + "\n";
								DisplayColor = ResultColorChoices.Warn;
							}
						}
						else if (energy == "10X-FFF")
						{
							if (field.DoseRate < 2400)
							{
								Result = "Warning";
								ResultDetails += field.Id + " dose rate set at " + field.DoseRate + "\n";
								DisplayColor = ResultColorChoices.Warn;
							}
						}
						else if (energy.Contains("E", StringComparison.CurrentCultureIgnoreCase))
						{
							if (field.DoseRate < 1000)
							{
								Result = "Warning";
								ResultDetails += field.Id + " dose rate set at " + field.DoseRate + "\n";
								DisplayColor = ResultColorChoices.Warn;
							}
						}
					}
				}

				//no problems found
				if (ResultDetails == "")
				{
					Result = "";
					ResultDetails = plan.Beams.Where(x => !x.IsSetupField).First().DoseRate.ToString();
					DisplayColor = ResultColorChoices.Pass;
				}

				ResultDetails = ResultDetails.TrimEnd('\n');
			}
			#endregion

			#region Flint TrueBeams
			// Flattened - 600
			// 6FFF      - 1400
			// 10FFF     - 2400
			// Electron  - 600
			else if (Department == Department.FLT)
			{
				foreach (Beam field in plan.Beams)
				{
					//ignore setup fields
					if (!field.IsSetupField)
					{
						string energy = field.EnergyModeDisplayName;

						if (energy == "6X" || energy == "10X" || energy == "15X" || energy == "16X" || energy == "18X" || energy == "23X")
						{
							if (field.DoseRate < 600)
							{
								Result = "Warning";
								ResultDetails += field.Id + " dose rate set at " + field.DoseRate + "\n";
								DisplayColor = ResultColorChoices.Warn;
							}
						}
						else if (energy == "6X-FFF")
						{
							if (field.DoseRate < 1400)
							{
								Result = "Warning";
								ResultDetails += field.Id + " dose rate set at " + field.DoseRate + "\n";
								DisplayColor = ResultColorChoices.Warn;
							}
						}
						else if (energy == "10X-FFF")
						{
							if (field.DoseRate < 2400)
							{
								Result = "Warning";
								ResultDetails += field.Id + " dose rate set at " + field.DoseRate + "\n";
								DisplayColor = ResultColorChoices.Warn;
							}
						}
						else if (energy.Contains("E", StringComparison.CurrentCultureIgnoreCase))
						{
							if (field.DoseRate < 600)
							{
								Result = "Warning";
								ResultDetails += field.Id + " dose rate set at " + field.DoseRate + "\n";
								DisplayColor = ResultColorChoices.Warn;
							}
						}
					}
				}

				//no problems found
				if (ResultDetails == "")
				{
					Result = "";
					ResultDetails = plan.Beams.Where(x => !x.IsSetupField).First().DoseRate.ToString();
					DisplayColor = ResultColorChoices.Pass;
				}

				ResultDetails = ResultDetails.TrimEnd('\n');
			}
            #endregion

            #region Lapeer / Owosso
            // Photon   - 600
            // Electron - 400
            else if (Department == Department.LAP ||
					 Department == Department.OWO)
			{
				foreach (Beam field in plan.Beams)
				{
					//ignore setup fields
					if (!field.IsSetupField)
					{
						string energy = field.EnergyModeDisplayName;

						if (energy == "6X" || energy == "10X" || energy == "15X" || energy == "16X" || energy == "18X" || energy == "23X")
						{
							if (field.DoseRate < 600)
							{
								Result = "Warning";
								ResultDetails += field.Id + " dose rate set at " + field.DoseRate + "\n";
								DisplayColor = ResultColorChoices.Warn;
							}
						}
						else if (energy.Contains("E", StringComparison.CurrentCultureIgnoreCase))
						{
							if (field.DoseRate < 400)
							{
								Result = "Warning";
								ResultDetails += field.Id + " dose rate set at " + field.DoseRate + "\n";
								DisplayColor = ResultColorChoices.Warn;
							}
						}
					}
				}

				//no problems found
				if (ResultDetails == "")
				{
					Result = "";
					ResultDetails = plan.Beams.Where(x => !x.IsSetupField).First().DoseRate.ToString();
					DisplayColor = ResultColorChoices.Pass;
				}

				ResultDetails = ResultDetails.TrimEnd('\n');
			}
            #endregion

			#region Lansing
            // IMRT     - 600
            // 3D       - 600
            // Electron - 400
            else if (Department == Department.LAN)
			{
				foreach (Beam field in plan.Beams)
				{
					//ignore setup fields
					if (!field.IsSetupField)
					{
						string energy = field.EnergyModeDisplayName;

						if (energy == "6X" || energy == "10X" || energy == "15X" || energy == "16X" || energy == "18X" || energy == "23X")
						{

							//for IMRT fields that have more control points than step and shoot, dose rate should be 500
							if (field.MLCPlanType == MLCPlanType.DoseDynamic && field.ControlPoints.Count > 18)
							{
								if (field.DoseRate != 600)
								{
									Result = "Warning";
									ResultDetails += field.Id + " (IMRT) dose rate set at " + field.DoseRate + "\n";
									DisplayColor = ResultColorChoices.Warn;
								}
							}
							//3D dose rate should be 600
							else
							{
								if (field.DoseRate < 600)
								{
									Result = "Warning";
									ResultDetails += field.Id + " (3D) dose rate set at " + field.DoseRate + "\n";
									DisplayColor = ResultColorChoices.Warn;
								}
							}
						}
						else if (energy.Contains("E", StringComparison.CurrentCultureIgnoreCase))
						{
							if (field.DoseRate != 400)
							{
								Result = "Warning";
								ResultDetails += field.Id + " (electron) dose rate set at " + field.DoseRate + "\n";
								DisplayColor = ResultColorChoices.Warn;
							}
						}
					}
				}

				//no problems found
				if (ResultDetails == "")
				{
					Result = "";
					ResultDetails = plan.Beams.Where(x => !x.IsSetupField).First().DoseRate.ToString();
					DisplayColor = ResultColorChoices.Pass;
				}

				ResultDetails = ResultDetails.TrimEnd('\n');
			}
            #endregion

			#region Detroit group
            // IMRT & 3D - 400
            // VMAT      - 600
            // 10FFF     - 1600
            // Electron  - 1000
            else if (Department == Department.DET ||
				Department == Department.FAR)
			{
				foreach (Beam field in plan.Beams)
				{
					//ignore setup fields
					if (!field.IsSetupField)
					{
						string energy = field.EnergyModeDisplayName;
						string field_type = field.Technique.ToString();

						if (energy == "6X" || energy == "10X" || energy == "15X")
						{
							if ((field.DoseRate < 600) && (field_type.Contains("ARC", StringComparison.CurrentCultureIgnoreCase)))
							{
								Result = "Warning";
								ResultDetails += field.Id + " dose rate set at " + field.DoseRate + "\n";
								DisplayColor = ResultColorChoices.Warn;
							}
							else if (field.DoseRate != 400 && field_type.Contains("STATIC", StringComparison.CurrentCultureIgnoreCase))
							{
								Result = "Warning";
								ResultDetails += field.Id + " dose rate set at " + field.DoseRate + "\n";
								DisplayColor = ResultColorChoices.Warn;
							}
						}
						else if (energy == "10X-FFF")
						{
							if (field.DoseRate < 1600)
							{
								Result = "Warning";
								ResultDetails += field.Id + " dose rate set at " + field.DoseRate + "\n";
								DisplayColor = ResultColorChoices.Warn;
							}
						}
						else if (energy.Contains("E", StringComparison.CurrentCultureIgnoreCase))
						{
							if (field.DoseRate < 1000)
							{
								Result = "Warning";
								ResultDetails += field.Id + " dose rate set at " + field.DoseRate + "\n";
								DisplayColor = ResultColorChoices.Warn;
							}
						}
					}
				}

				//no problems found
				if (ResultDetails == "")
				{
					Result = "";
					ResultDetails = plan.Beams.Where(x => !x.IsSetupField).First().DoseRate.ToString();
					DisplayColor = ResultColorChoices.Pass;
				}

				ResultDetails = ResultDetails.TrimEnd('\n');
			}
            #endregion

            #region Northern Trilogy
            // 600 for everything
            else if (Department == Department.NOR)
			{
				foreach (Beam field in plan.Beams)
				{
					//ignore setup fields
					if (!field.IsSetupField)
					{

						if (field.DoseRate < 600)
						{
							Result = "Warning";
							ResultDetails += field.Id + " dose rate set at " + field.DoseRate + "\n";
							DisplayColor = ResultColorChoices.Warn;
						}
					}
				}

				//no problems found
				if (ResultDetails == "")
				{
					Result = "";
					ResultDetails = plan.Beams.Where(x => !x.IsSetupField).First().DoseRate.ToString();
					DisplayColor = ResultColorChoices.Pass;
				}

				ResultDetails = ResultDetails.TrimEnd('\n');
			}
			#endregion

			#region Central
			// 400 or 600 for everything
			else if (Department == Department.CEN)
			{
				foreach (Beam field in plan.Beams)
				{
					//ignore setup fields
					if (!field.IsSetupField)
					{

						if (field.DoseRate != 600 && field.DoseRate != 400)
						{
							Result = "Warning";
							ResultDetails += field.Id + " dose rate set at " + field.DoseRate + "\n";
							DisplayColor = ResultColorChoices.Warn;
						}
					}
				}

				//no problems found
				if (ResultDetails == "")
				{
					Result = "";
					ResultDetails = plan.Beams.Where(x => !x.IsSetupField).First().DoseRate.ToString();
					DisplayColor = ResultColorChoices.Pass;
				}

				ResultDetails = ResultDetails.TrimEnd('\n');
			}
			#endregion

			else
				TestNotImplemented();
		}
    }
}
