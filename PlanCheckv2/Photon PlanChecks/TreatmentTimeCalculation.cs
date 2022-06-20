using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.TPS.Common.Model.API;
using VMS.TPS.Common.Model.Types;

namespace PlanCheck.Checks
{
    class TreatmentTimeCalculation : PlanCheckBasePhoton
    {
        protected override List<string> MachineExemptions => new List<string> {
            DepartmentInfo.MachineNames.BAY_TB,
            DepartmentInfo.MachineNames.CEN_EX,
            DepartmentInfo.MachineNames.CEN_TB,
            DepartmentInfo.MachineNames.CLA_EX,
            DepartmentInfo.MachineNames.CLA_TB,
            DepartmentInfo.MachineNames.FLT_BackTB,
            DepartmentInfo.MachineNames.FLT_FrontTB,
            DepartmentInfo.MachineNames.LAP_IX,
            DepartmentInfo.MachineNames.MAC_IX,
            DepartmentInfo.MachineNames.MAC_TB,
            //DepartmentInfo.MachineNames.MPH_TB,
            DepartmentInfo.MachineNames.NOR_EX,
            DepartmentInfo.MachineNames.NOR_IX,
            DepartmentInfo.MachineNames.NOR_TB,
            DepartmentInfo.MachineNames.OWO_IX,
            DepartmentInfo.MachineNames.PRO_G1,
            DepartmentInfo.MachineNames.PRO_G2

        };

        public TreatmentTimeCalculation(PlanSetup plan) : base(plan) { }

        public override void RunTestPhoton(ExternalPlanSetup plan)
        {
            DisplayName = "Delivery Time";
            Result = "";
            DisplayColor = ResultColorChoices.Pass;
            TestExplanation = "Calculates the delivery time of the beam for 3D/FiF plans";

            var beams = plan.Beams.Where(x => !x.IsSetupField);

            foreach(var beam in beams)
            {
                // Electron
                if (beam.EnergyModeDisplayName.ToUpper().Contains("E"))
                {
                    Result += $"{beam.Id} = {Math.Ceiling(beam.Meterset.Value / beam.DoseRate * 100.0) / 100.0} min\n";
                }
                // Can't calculate for EDW fields right now
                else if (beam.Wedges.Where(x => x is EnhancedDynamicWedge).Count() > 0)
                {
                    Result += $"{beam.Id}: Can't calculate times for EDW fields\n";
                }
                // 3D
                else if(beam.MLCPlanType == MLCPlanType.Static)
                {
                    Result += $"{beam.Id} = {Math.Ceiling(beam.Meterset.Value / beam.DoseRate * 100.0) / 100.0} min\n";
                }
                // VMAT
                else if(beam.MLCPlanType == MLCPlanType.VMAT || beam.MLCPlanType == MLCPlanType.DoseDynamic)
                {
                    double totalTime60 = 0;
                    double totalTime48 = 0;
                    float[,] prevMlcPos = null;
                    ControlPoint prevCP = null;
                    foreach (var cp in beam.ControlPoints)
                    {
                        if (prevCP == null)
                        {
                            prevCP = cp;
                            prevMlcPos = cp.LeafPositions;
                            continue;
                        }

                        var cpGantryRotation = Math.Abs(convertGantryAngle(cp.GantryAngle) - convertGantryAngle(prevCP.GantryAngle));     // Gantry movement in degrees of the previous control point
                        var cpMUs = (cp.MetersetWeight - prevCP.MetersetWeight) * beam.Meterset.Value;  // MUs delivered by the previous control point

                        var cpMinTimeGanty60 = cpGantryRotation / 6.0;    // How long it would take the gantry to move through the control point assuming max speed of 6 degrees / second
                        var cpMinTimeGanty48 = cpGantryRotation / 4.8;    // How long it would take the gantry to move through the control point assuming max speed of 4.8 degrees / second
                        var cpMinTimeDoseRate = cpMUs / beam.DoseRate * 60; // How long it would take to deliver the control point MUs assuming max dose rate of beam

                        float[,] mlcMovements = new float[2, 60];
                        // Get movements of each leaf from previous position into new position
                        for (int i = 0; i < 2; i++)
                            for (int k = 0; k < 60; k++)
                                mlcMovements[i, k] += Math.Abs(cp.LeafPositions[i, k] - prevMlcPos[i, k]);

                        var cpMinMlc = mlcMovements.Cast<float>().Max() / 25;

                        totalTime60 += Math.Max(cpMinTimeGanty60, Math.Max(cpMinTimeDoseRate, cpMinMlc));
                        totalTime48 += Math.Max(cpMinTimeGanty48, Math.Max(cpMinTimeDoseRate, cpMinMlc));

                        prevCP = cp;
                        prevMlcPos = cp.LeafPositions;
                    }

                    Result += $"{beam.Id} (4.8 deg/sec) = {Math.Ceiling((totalTime48 / 1) * 100.0) / 100.0} sec\n";
                    Result += $"{beam.Id} (6.0 deg/sec) = {Math.Ceiling((totalTime60 / 1) * 100.0) / 100.0} sec\n";
                }
                // FiF / IMRT
                else
                {
                    float[,] prevPos = null;
                    float totalMaxMovement = 0;
                    foreach (var point in beam.ControlPoints)
                    {
                        if (prevPos != null)
                        {
                            float[,] movements = new float[2, 60];
                            // Get movements of each leaf from previous position into new position
                            for (int i = 0; i < 2; i++)
                                for (int k = 0; k < 60; k++)
                                    movements[i, k] += Math.Abs(point.LeafPositions[i, k] - prevPos[i, k]);

                            // Find max distance moved
                            totalMaxMovement += movements.Cast<float>().Max();
                        }

                        prevPos = point.LeafPositions;
                    }

                    Result += $"{beam.Id} = {Math.Ceiling((beam.Meterset.Value / beam.DoseRate + (totalMaxMovement / 25) / 60) * 100.0) / 100.0} min\n";    // Max leaf speed of 25 mm / second
                }
            }

            Result = Result.TrimEnd('\n');
        }

        private double convertGantryAngle(double gantry)
        {
            if (gantry > 180)
                return gantry - 360;
            else
                return gantry;
        }
    }
}
