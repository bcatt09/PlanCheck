using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.TPS.Common.Model.API;

namespace VMS.TPS.PlanChecks
{
    class TreatmentTimeCalculation : PlanCheck
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
            ResultColor = "LimeGreen";
            TestExplanation = "Calculates the delivery time of the beam for 3D/FiF plans";

            var beams = plan.Beams.Where(x => !x.IsSetupField);

            foreach(var beam in beams)
            {
                // 3D
                if(beam.ControlPoints.Count == 1)
                {
                    Result += $"{beam.Id} = {Math.Round(beam.Meterset.Value / beam.DoseRate, 2)} min";
                }

                // FiF/IMRT
                else //if(beam.ControlPoints.Count < 25)
                {
                    float[,] prevPos = null;
                    float[,] totalMovement = new float[2, 60];
                    foreach (var point in beam.ControlPoints)
                    {
                        if (prevPos != null)
                        {
                            for (int i = 0; i < 2; i++)
                                for (int k = 0; k < 60; k++)
                                    totalMovement[i, k] += Math.Abs(point.LeafPositions[i, k] - prevPos[i, k]);
                        }

                        prevPos = point.LeafPositions;
                    }

                    Result += $"{beam.Id} = {Math.Round(beam.Meterset.Value / beam.DoseRate + (totalMovement.Cast<float>().Max() / 25) / 60, 2)} min\n";
                }

            }

            Result = Result.TrimEnd('\n');
        }
    }
}
