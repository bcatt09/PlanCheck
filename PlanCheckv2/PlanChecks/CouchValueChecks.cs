using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.TPS.Common.Model.API;

namespace PlanCheck.Checks
{
    public class CouchValueChecks : PlanCheckBase
	{
		protected override List<string> MachineExemptions => new List<string> 
		{
			DepartmentInfo.MachineNames.NOR_EX,
			DepartmentInfo.MachineNames.NOR_IX
		};

		public CouchValueChecks(PlanSetup plan) : base(plan) { }

        protected override void RunTest(PlanSetup plan)
        {
			DisplayName = "Couch Values";
			ResultDetails = "";
			TestExplanation = "Checks that couch values are entered for each field based on department standards";

            #region Macomb/Detroit Groups and Central
            // Any couch values entered
            if (Department == Department.CLA ||
				Department == Department.MAC ||
				Department == Department.MPH ||
				Department == Department.DET ||
				Department == Department.FAR ||
				Department == Department.CEN)
			{
				//Check each field to see if couch values are NaN
				foreach (Beam field in plan.Beams)
				{
					if (field.ControlPoints.First().TableTopLateralPosition.ToString() == "NaN" || field.ControlPoints.First().TableTopLongitudinalPosition.ToString() == "NaN" || field.ControlPoints.First().TableTopVerticalPosition.ToString() == "NaN")
					{
						Result = "Warning";
						ResultDetails += "No couch values for " + field.Id.ToString() + ": ";
						DisplayColor = ResultColorChoices.Warn;

						if (field.ControlPoints.First().TableTopLateralPosition.ToString() == "NaN")
							ResultDetails += "lat, ";
						if (field.ControlPoints.First().TableTopLongitudinalPosition.ToString() == "NaN")
							ResultDetails += "long, ";
						if (field.ControlPoints.First().TableTopVerticalPosition.ToString() == "NaN")
							ResultDetails += "vert, ";

						ResultDetails = ResultDetails.TrimEnd(' ');
						ResultDetails = ResultDetails.TrimEnd(',');
						ResultDetails += '\n';
					}
				}

				ResultDetails = ResultDetails.TrimEnd('\n');

				//no issues found
				if (ResultDetails == "")
				{
					Result = "";
					ResultDetails = $"Lat: {(ConvertCouchLatToVarianIECScale(plan.Beams.First().ControlPoints.First().TableTopLateralPosition) / 10.0).ToString("0.0")} cm\n" +
									$"Long: {(plan.Beams.First().ControlPoints.First().TableTopLongitudinalPosition / 10.0).ToString("0.0")} cm\n" +
									$"Vert: {(ConvertCouchVertToVarianIECScale(plan.Beams.First().ControlPoints.First().TableTopVerticalPosition) / 10.0).ToString("0.0")} cm";
					DisplayColor = ResultColorChoices.Pass;
				}
			}
            #endregion

            #region Flint Group
            // Vert = 0
            // Long = 100
            // Lat = 0
            else if (Department == Department.LAP ||
					 Department == Department.FLT ||
					 Department == Department.OWO)
			{
				//Check each field to see if couch values are NaN
				foreach (Beam field in plan.Beams)
				{
					if (field.ControlPoints.FirstOrDefault().TableTopLateralPosition != 0 || field.ControlPoints.FirstOrDefault().TableTopLongitudinalPosition != 1000 || field.ControlPoints.FirstOrDefault().TableTopVerticalPosition != 0)
					{
						Result = "Warning";
						ResultDetails += "Couch value incorrect for " + field.Id.ToString() + ": ";
						DisplayColor = ResultColorChoices.Warn;

						if (field.ControlPoints.First().TableTopLateralPosition != 0)
							ResultDetails += "lat, ";
						if (field.ControlPoints.First().TableTopLongitudinalPosition != 1000)
							ResultDetails += "long, ";
						if (field.ControlPoints.First().TableTopVerticalPosition != 0)
							ResultDetails += "vert, ";

						ResultDetails = ResultDetails.TrimEnd(' ');
						ResultDetails = ResultDetails.TrimEnd(',');
						ResultDetails += '\n';
					}
				}

				ResultDetails = ResultDetails.TrimEnd('\n');

				//no issues found
				if (ResultDetails == "")
				{
					Result = "";
					if (Department == Department.LAP ||
						Department == Department.OWO)
						ResultDetails = $"Lat: {(ConvertCouchLatToVarianStandardScale(plan.Beams.First().ControlPoints.First().TableTopLateralPosition) / 10.0).ToString("0.0")} cm\nLong: {(plan.Beams.First().ControlPoints.First().TableTopLongitudinalPosition / 10.0).ToString("0.0")} cm\nVert: {(ConvertCouchVertToVarianStandardScale(plan.Beams.First().ControlPoints.First().TableTopVerticalPosition) / 10.0).ToString("0.0")} cm";
					else
						ResultDetails = $"Lat: {(ConvertCouchLatToVarianIECScale(plan.Beams.First().ControlPoints.First().TableTopLateralPosition) / 10.0).ToString("0.0")} cm\n" +
										$"Long: {(plan.Beams.First().ControlPoints.First().TableTopLongitudinalPosition / 10.0).ToString("0.0")} cm\n" +
										$"Vert: {(ConvertCouchVertToVarianIECScale(plan.Beams.First().ControlPoints.First().TableTopVerticalPosition) / 10.0).ToString("0.0")} cm";
					DisplayColor = ResultColorChoices.Pass;
				}
			}
            #endregion

            #region Lansing
            // Vert <= 50
            // Long = 50
            // Lat = 0
            else if (Department == Department.LAN)
			{
				//Check each field to see if couch values are NaN
				foreach (Beam field in plan.Beams)
				{
					if (field.ControlPoints.First().TableTopLateralPosition != 0 || field.ControlPoints.First().TableTopLongitudinalPosition != 500 || field.ControlPoints.First().TableTopVerticalPosition < -500)
					{
						Result = "Warning";
						ResultDetails += "Couch value incorrect for " + field.Id.ToString() + ": ";
						DisplayColor = ResultColorChoices.Warn;

						if (field.ControlPoints.First().TableTopLateralPosition != 0)
							ResultDetails += "lat, ";
						if (field.ControlPoints.First().TableTopLongitudinalPosition != 500)
							ResultDetails += "long, ";
						if (field.ControlPoints.First().TableTopVerticalPosition < -500)
							ResultDetails += "vert, ";

						ResultDetails = ResultDetails.TrimEnd(' ');
						ResultDetails = ResultDetails.TrimEnd(',');
						ResultDetails += '\n';
					}
				}

				ResultDetails = ResultDetails.TrimEnd('\n');

				//no issues found
				if (ResultDetails == "")
				{
					ResultDetails = $"Lat: {(ConvertCouchLatToVarianIECScale(plan.Beams.First().ControlPoints.First().TableTopLateralPosition) / 10.0).ToString("0.0")} cm\n" +
									$"Long: {(plan.Beams.First().ControlPoints.First().TableTopLongitudinalPosition / 10.0).ToString("0.0")} cm\n" +
									$"Vert: {(ConvertCouchVertToVarianIECScale(plan.Beams.First().ControlPoints.First().TableTopVerticalPosition) / 10.0).ToString("0.0")} cm";
					DisplayColor = ResultColorChoices.Pass;
				}
			}
            #endregion

            else
                TestNotImplemented();
		}

		/// <summary>
		/// Converts a given couch lateral to Varian IEC Scale
		/// </summary>
		/// <param name="couch">Couch lateral in mm</param>
		/// <returns>Couch lateral in Varian IEC Scale in mm</returns>
		private double ConvertCouchLatToVarianIECScale(double couch)
		{
			return (couch + 10000) % 10000;
		}

		/// <summary>
		/// Converts a given couch vertical to Varian IEC Scale
		/// </summary>
		/// <param name="couch">Couch vertical in mm</param>
		/// <returns>Couch vertical in Varian IEC Scale in mm</returns>
		private double ConvertCouchVertToVarianIECScale(double couch)
		{
			return (10000 - couch) % 10000;
		}

		/// <summary>
		/// Converts a given couch lateral to Varian Standard Scale
		/// </summary>
		/// <param name="couch">Couch lateral in mm</param>
		/// <returns>Couch lateral in Varian Standard Scale in mm</returns>
		private double ConvertCouchLatToVarianStandardScale(double couch)
		{
			return couch + 1000;
		}

		/// <summary>
		/// Converts a given couch vertical to Varian Standard Scale
		/// </summary>
		/// <param name="couch">Couch vertical in mm</param>
		/// <returns>Couch vertical in Varian Standard Scale in mm</returns>
		private double ConvertCouchVertToVarianStandardScale(double couch)
		{
			return 1000 - couch;
		}
	}
}
