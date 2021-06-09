using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.TPS.Common.Model.API;
using VMS.TPS.Common.Model.Types;

namespace PlanCheck.Checks
{
    public class JawTrackingChecks : PlanCheckBase
    {
		protected override List<string> MachineExemptions => new List<string> {
			DepartmentInfo.MachineNames.CEN_EX,
			DepartmentInfo.MachineNames.CLA_EX,
			DepartmentInfo.MachineNames.DET_IX,
			DepartmentInfo.MachineNames.FAR_IX,
			DepartmentInfo.MachineNames.LAN_IX,
			DepartmentInfo.MachineNames.LAP_IX,
			DepartmentInfo.MachineNames.MAC_IX,
			DepartmentInfo.MachineNames.NOR_EX,
			DepartmentInfo.MachineNames.NOR_IX,
			DepartmentInfo.MachineNames.OWO_IX,
			DepartmentInfo.MachineNames.PRO_G1,
			DepartmentInfo.MachineNames.PRO_G2
		};

		public JawTrackingChecks(PlanSetup plan) : base(plan) { }

        protected override void RunTest(PlanSetup plan)
        {
			bool IMRT = false;

			DisplayName = "Jaw Tracking";
			Result = "";
			TestExplanation = "Checks to see if jaw tracking is enabled for IMRT/VMAT plans";

            #region Macomb Group
            // use jaw tracking
            if (Department == Department.MAC ||
				Department == Department.MPH)
			{
				//VMAT plan
				if ((from s in plan.Beams where s.MLCPlanType == MLCPlanType.VMAT select s).Count() > 0)
				{
					IMRT = true;

					foreach (Beam field in plan.Beams)
					{
						//ignore setup fields
						if (!field.IsSetupField)
						{
							//for VMAT fields, check that there are different jaw positions in the control points
							if (field.MLCPlanType == MLCPlanType.VMAT)
							{
								//get initial jaw positions
								double x1 = field.ControlPoints.First().JawPositions.X1;
								double x2 = field.ControlPoints.First().JawPositions.X2;
								double y1 = field.ControlPoints.First().JawPositions.Y1;
								double y2 = field.ControlPoints.First().JawPositions.Y2;

								//if they change at any of the control points, then jaw tracking must be on
								foreach (ControlPoint point in field.ControlPoints)
								{
									if (x1 != point.JawPositions.X1 || x2 != point.JawPositions.X2 || y1 != point.JawPositions.Y1 || y2 != point.JawPositions.Y2)
									{
										Result = "Pass";
										DisplayColor = ResultColorChoices.Pass;
										break;
									}
								}
							}
						}
					}
				}
				//IMRT plan
				else if ((from s in plan.Beams where s.MLCPlanType == MLCPlanType.DoseDynamic select s).Count() > 0)
				{
					foreach (Beam field in plan.Beams)
					{
						//ignore setup fields
						if (!field.IsSetupField)
						{
							//for IMRT fields that have more control points than step and shoot, check that there are different jaw positions in the control points
							if (field.MLCPlanType == MLCPlanType.DoseDynamic && field.ControlPoints.Count > 18)
							{
								IMRT = true;

								//get initial jaw positions
								double x1 = field.ControlPoints.First().JawPositions.X1;
								double x2 = field.ControlPoints.First().JawPositions.X2;
								double y1 = field.ControlPoints.First().JawPositions.Y1;
								double y2 = field.ControlPoints.First().JawPositions.Y2;

								//if they change at any of the control points, then jaw tracking must be on
								foreach (ControlPoint point in field.ControlPoints)
								{
									if (x1 != point.JawPositions.X1 || x2 != point.JawPositions.X2 || y1 != point.JawPositions.Y1 || y2 != point.JawPositions.Y2)
									{
										Result = "Pass";
										DisplayColor = ResultColorChoices.Pass;
										break;
									}
								}
							}
						}
					}
				}

				//Static fields
				if (!IMRT)
				{
					Result = "Pass";
					DisplayColor = ResultColorChoices.Pass;
				}

				//No jaw tracking detected
				else if (Result == "")
				{
					Result = "Warning";
					ResultDetails = "Please check that jaw tracking is enabled in the optimization window or leaf motion calculator";
					DisplayColor = ResultColorChoices.Warn;
				}
			}
			#endregion

			#region Flint Group and Northern TrueBeam (for now)
			// do not use jaw tracking
			else if (Department == Department.FLT || Department == Department.NOR)
			{
				//VMAT plan
				if ((from s in plan.Beams where s.MLCPlanType == MLCPlanType.VMAT select s).Count() > 0)
				{
					IMRT = true;

					foreach (Beam field in plan.Beams)
					{
						//ignore setup fields
						if (!field.IsSetupField)
						{
							//for VMAT fields, check that there are different jaw positions in the control points
							if (field.MLCPlanType == MLCPlanType.VMAT)
							{
								//get initial jaw positions
								double x1 = field.ControlPoints.First().JawPositions.X1;
								double x2 = field.ControlPoints.First().JawPositions.X2;
								double y1 = field.ControlPoints.First().JawPositions.Y1;
								double y2 = field.ControlPoints.First().JawPositions.Y2;

								//if they change at any of the control points, then jaw tracking must be on
								foreach (ControlPoint point in field.ControlPoints)
								{
									if (x1 != point.JawPositions.X1 || x2 != point.JawPositions.X2 || y1 != point.JawPositions.Y1 || y2 != point.JawPositions.Y2)
									{
										Result = "Warning";
										ResultDetails = "Please check that jaw tracking is disabled in the optimization window";
										DisplayColor = ResultColorChoices.Warn;
										break;
									}
								}
							}
						}
					}
				}
				//IMRT plan
				else if ((from s in plan.Beams where s.MLCPlanType == MLCPlanType.DoseDynamic select s).Count() > 0)
				{
					foreach (Beam field in plan.Beams)
					{
						//ignore setup fields
						if (!field.IsSetupField)
						{
							//for IMRT fields that have more control points than step and shoot, check that there are different jaw positions in the control points
							if (field.MLCPlanType == MLCPlanType.DoseDynamic && field.ControlPoints.Count > 18)
							{
								IMRT = true;

								//get initial jaw positions
								double x1 = field.ControlPoints.First().JawPositions.X1;
								double x2 = field.ControlPoints.First().JawPositions.X2;
								double y1 = field.ControlPoints.First().JawPositions.Y1;
								double y2 = field.ControlPoints.First().JawPositions.Y2;

								//if they change at any of the control points, then jaw tracking must be on
								foreach (ControlPoint point in field.ControlPoints)
								{
									if (x1 != point.JawPositions.X1 || x2 != point.JawPositions.X2 || y1 != point.JawPositions.Y1 || y2 != point.JawPositions.Y2)
									{
										Result = "Warning";
										ResultDetails = "Please check that jaw tracking is disabled in the leaf motion calculator";
										DisplayColor = ResultColorChoices.Warn;
										break;
									}
								}
							}
						}
					}
				}

                //Static fields
                if (!IMRT)
				{
					Result = "Pass";
					DisplayColor = ResultColorChoices.Pass;
				}

				//No jaw tracking detected
				else if (Result == "")
				{
					Result = "Pass";
					DisplayColor = ResultColorChoices.Pass;
				}
			}
			#endregion

			else
				TestNotImplemented();
		}
    }
}
