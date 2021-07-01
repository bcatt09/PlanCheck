using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VMS.TPS.Common.Model.API;

namespace PlanCheck.Checks
{
    public class MLCChecks : PlanCheckBasePhoton
    {
        protected override List<string> MachineExemptions => new List<string> { };

        public MLCChecks(PlanSetup plan) : base(plan) { }

        public override void RunTestPhoton(ExternalPlanSetup plan)
        {
            DisplayName = "MLC Checks";
            Result = "";
            ResultDetails = "";
            DisplayColor = ResultColorChoices.Pass;
            TestExplanation = "Checks that there are no closed leaf pairs parked inside the jaws and calculates MU factor (MUs / cGy per fx)";

            var totalMUs = 0.0;

            // Coordinates are in IEC Beam Limiting Device Coordinates
            // + = Open (pulled back away from CAX)
            // - = Closed (pushed forward beyond CAX)

            foreach (var field in plan.Beams.Where(x => !x.IsSetupField && x.MLC != null))
            {
                // Is it an HD MLC or not (or unknown)
                // "Millennium 120" or "Varian High Definition 120"
                bool HD;

                if (field.MLC.Model == "Varian High Definition 120")
                    HD = true;
                else if (field.MLC.Model == "Millennium 120")
                    HD = false;
                else
                {
                    Result = "Unknown MLC";
                    DisplayColor = ResultColorChoices.Fail;
                    return;
                }
                //double[,] closedPairsInField = new double[60,3];
                List<ClosedLeafPairInfo> closedPairTracking = new List<ClosedLeafPairInfo>(60);

                // Initialize the leaf pair if it's empty
                for (var lp = 0; lp < 60; lp++)
                    closedPairTracking.Add(new ClosedLeafPairInfo(lp, HD));

                for (var cp = 0; cp < field.ControlPoints.Count; cp++)
                {
                    // Jaw positions
                    var x1 = field.ControlPoints[cp].JawPositions.X1;
                    var x2 = field.ControlPoints[cp].JawPositions.X2;
                    var y1 = field.ControlPoints[cp].JawPositions.Y1;
                    var y2 = field.ControlPoints[cp].JawPositions.Y2;

                    var leafPairs = field.ControlPoints[cp].LeafPositions;

                    var cpMeterset = field.ControlPoints[cp].MetersetWeight - field.ControlPoints[Math.Max(cp - 1, 0)].MetersetWeight;

                    for (var lp = 0; lp < 60; lp++)
                    {
                        var leafA = leafPairs[0, lp];
                        var leafB = leafPairs[1, lp];

                        var openingWidth = Math.Abs(leafA - leafB);

                        // Small enough gap
                        if (Math.Round(openingWidth, 1) <= field.MLC.MinDoseDynamicLeafGap && 
                        // Within the X jaws
                            (leafA > x1 && leafB < x2) &&
                        // Within the Y jaws
                            (closedPairTracking[lp].YOffset > y1 && closedPairTracking[lp].YOffset < y2))
                        {
                            // If leaf hasn't moved, update how long it's been there
                            if (Math.Round(Math.Abs(closedPairTracking[lp].CurrentPosition - leafA), 1) <= 0.1 || closedPairTracking[lp].CurrentPosition == -10000.0f)
                            {
                                closedPairTracking[lp].CurrentPosition = leafA;
                                closedPairTracking[lp].MetersetInCurrentPosition += cpMeterset;

                                // If greater than max time, save as new max
                                if (closedPairTracking[lp].MetersetInCurrentPosition > closedPairTracking[lp].MaxMetersetInOnePosition)
                                    closedPairTracking[lp].MaxMetersetInOnePosition = closedPairTracking[lp].MetersetInCurrentPosition;
                            }
                            // If it's moving, reset counters
                            else
                            {
                                closedPairTracking[lp].CurrentPosition = leafA;
                                closedPairTracking[lp].MetersetInCurrentPosition = cpMeterset;
                            }
                        }
                    }
                }

                // Display if closed leaf pair was parked for > 15% of field MUs
                if (closedPairTracking.Select(x => x.MaxMetersetInOnePosition).Max() > 0.15)
                {
                    Result = "Warning";
                    DisplayColor = ResultColorChoices.Warn;
                    ResultDetails += $"{field.Id} - Closed leaf pair in field for {Math.Round(closedPairTracking.Select(x => x.MaxMetersetInOnePosition).Max() * 100)}% of MUs\n";
                }

                totalMUs += field.Meterset.Value;
            }

            ResultDetails += $"MU Factor: {Math.Round(totalMUs / plan.DosePerFraction.Dose, 1)} MU / cGy";
        }

        private static float GetLeafOffset(float leafOffset, bool HD)
        {
            return GetBigLeafOffset(leafOffset, HD) + GetSmallLeafOffset(leafOffset, HD);
        }

        private static float GetBigLeafOffset(float leafOffset, bool HD)
        {
            // It is a small leaf
            if (Math.Abs(leafOffset) < 20)
                return 0.0f;

            var leafWidth = HD ? 5.0f : 10.0f;

            return (leafOffset - 20 * Math.Sign(leafOffset)) * leafWidth;
        }

        private static float GetSmallLeafOffset(float leafOffset, bool HD)
        {
            // If it's a big leaf, only count the small ones
            if (Math.Abs(leafOffset) > 20)
                leafOffset = 20 * Math.Sign(leafOffset);

            if (HD)
                return leafOffset * 2.5f;
            else
                return leafOffset * 5.0f;
        }

        private class ClosedLeafPairInfo
        {
            public int LeafIndex { get; set; }
            public double MaxMetersetInOnePosition { get; set; }
            public double MetersetInCurrentPosition { get; set; }
            public float CurrentPosition { get; set; }
            public double YOffset { get; set; }

            public ClosedLeafPairInfo(int index, bool HD)
            {
                LeafIndex = index;
                MaxMetersetInOnePosition = 0;
                MetersetInCurrentPosition = 0;
                CurrentPosition = -10000.0f;

                // Find the center
                float yOffset = index + 1 - 30;
                // Offset for scaling
                if (yOffset > 0) yOffset -= 1;
                // Scale based on leaf width
                YOffset = GetLeafOffset(yOffset, HD);
            }
        }
    }
}
