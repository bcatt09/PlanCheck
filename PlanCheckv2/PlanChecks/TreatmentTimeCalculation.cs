using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.TPS.Common.Model.API;
using VMS.TPS.Common.Model.Types;

namespace PlanCheck.Checks
{
    class TreatmentTimeCalculation : PlanCheckBase
    {
        protected override List<string> MachineExemptions => new List<string> {
            DepartmentInfo.MachineNames.BAY_TB,
            DepartmentInfo.MachineNames.CEN_EX,
            DepartmentInfo.MachineNames.CLA_EX,
            DepartmentInfo.MachineNames.DET_IX,
            DepartmentInfo.MachineNames.DET_TB,
            DepartmentInfo.MachineNames.FAR_IX,
            DepartmentInfo.MachineNames.FLT_BackTB,
            DepartmentInfo.MachineNames.FLT_FrontTB,
            DepartmentInfo.MachineNames.LAP_IX,
            DepartmentInfo.MachineNames.MAC_IX,
            DepartmentInfo.MachineNames.MAC_TB,
            DepartmentInfo.MachineNames.MPH_TB,
            DepartmentInfo.MachineNames.NOR_EX,
            DepartmentInfo.MachineNames.NOR_IX,
            DepartmentInfo.MachineNames.OWO_IX
        };

        public TreatmentTimeCalculation(PlanSetup plan) : base(plan) { }

        protected override void RunTest(PlanSetup plan)
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
                    Result += $"{beam.Id} = {Math.Ceiling(beam.Meterset.Value / beam.DoseRate * 100.0) / 100.0} min\n";          // Math.Ceiling( x * 100 ) / 100.0
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

                // FiF / IMRT
                else //if(beam.ControlPoints.Count < 25)
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

                    Result += $"{beam.Id} = {Math.Ceiling((beam.Meterset.Value / beam.DoseRate + (totalMaxMovement / 25) / 60) * 100.0) / 100.0} min\n";
                }

            }

            Result = Result.TrimEnd('\n');
        }
    }
}
