using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VMS.TPS.Common.Model.API;
using VMS.TPS.Common.Model.Types;

namespace VMS.TPS.PlanChecks
{
    class FieldNameChecks : PlanCheckBase
    {
		protected override List<string> MachineExemptions => new List<string> { };

		public FieldNameChecks(PlanSetup plan) : base(plan) { }

        protected override void RunTest(PlanSetup plan)
		{
			DisplayName = "Field Names";
			ResultDetails = "";
			TestExplanation = "Checks that field names follow OneAria naming conventions";

			foreach (Beam field in plan.Beams)
			{
				//ignore setup fields
				if (!field.IsSetupField)
				{
                    #region Static Gantry Angles
                    //for static fields, check that the gantry angle is contained in the field name
                    if (field.Technique.ToString() == "STATIC" || field.Technique.ToString() == "SRS STATIC")
					{
						//field name matching pattern: g125 with or without a space or "_" between
						string fieldNameGantry = "(?i)g_? ?" + Math.Round(field.GantryAngleToUser(field.ControlPoints.First().GantryAngle), 0).ToString();

						if (!Regex.IsMatch(field.Id, fieldNameGantry))
						{
							Result = "Warning";
							ResultDetails += "Field name mismatch  —  Field: " + field.Id + "  Gantry Angle: " + field.GantryAngleToUser(field.ControlPoints.FirstOrDefault().GantryAngle).ToString() + "\n";
							DisplayColor = ResultColorChoices.Warn;
						}
					}
                    #endregion

                    #region Arcs
                    //for arc fields, check that cw vs ccw matches rotation direction
                    else if (field.Technique.ToString() == "ARC" || field.Technique.ToString() == "SRS ARC")
					{
						if (field.GantryDirection == GantryDirection.Clockwise)
						{
							if (field.Id.Contains("ccw", StringComparison.CurrentCultureIgnoreCase) || !field.Id.Contains("cw", StringComparison.CurrentCultureIgnoreCase))
							{
								Result = "Warning";
								ResultDetails += "Field name mismatch  —  Field: " + field.Id + "  Gantry Direction: " + field.GantryDirection + "\n";
								DisplayColor = ResultColorChoices.Warn;
							}

						}
						else if (field.GantryDirection == GantryDirection.CounterClockwise)
						{
							if (!field.Id.Contains("ccw", StringComparison.CurrentCultureIgnoreCase))
							{
								Result = "Warning";
								ResultDetails += "Field name mismatch  —  Field: " + field.Id + "  Gantry Direction: " + field.GantryDirection + "\n";
								DisplayColor = ResultColorChoices.Warn;
							}
						}
					}
                    #endregion

                    #region Couch Kicks
                    //check for pedestal kicks
                    if (field.ControlPoints.First().PatientSupportAngle != 0)
					{
						//field name matching pattern: g125 with or without a space or "_" between
						string fieldNamePedestal = "(?i)(p|t)_? ?" + Math.Round(field.PatientSupportAngleToUser(field.ControlPoints.First().PatientSupportAngle), 0).ToString();

						if (!Regex.IsMatch(field.Id, fieldNamePedestal))
						{
							Result = "Warning";
							ResultDetails += "Field name mismatch  —  Field: " + field.Id + "  Pedestal Angle: " + field.PatientSupportAngleToUser(field.ControlPoints.First().PatientSupportAngle).ToString() + "\n";
							DisplayColor = ResultColorChoices.Warn;
						}
					}
                    #endregion
                }
			}

			ResultDetails = ResultDetails.TrimEnd('\n');

			//no problems found
			if (ResultDetails == "")
			{
				Result = "Pass";
				DisplayColor = ResultColorChoices.Pass;
			}
		}
    }
}
