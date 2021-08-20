using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.TPS.Common.Model.API;
using VMS.TPS.Common.Model.Types;

namespace PlanCheck.Checks
{
    class ProtonSpotPositionChecks : PlanCheckBase
    {
        protected override List<string> MachineExemptions => DepartmentInfo.LinearAccelerators;
    
        public ProtonSpotPositionChecks(PlanSetup plan) : base(plan) { }

        protected override void RunTest(PlanSetup plan)
        {
            DisplayName = "Spot Position Limit Checks";
            TestExplanation = "This test will ensure spot positions are within allowed limits";
            IonPlanSetup ionPlan = (IonPlanSetup)plan;
            Result = "Pass"; // for this test set to pass and switch to fail if any spots found outside of limits

            double xLimitSnout30 = 125, yLimitSnout30 = 125;
            double xLimitSnoutS1 = 125, yLimitSnoutS1 = 125;
            double xLimitSnout15 = 75, yLimitSnout15 = 75;

            ResultDetails = ""; // Just putting that there so I remember about it.

            foreach (IonBeam beam in ionPlan.IonBeams)
            {
                if (!beam.IsSetupField)
                {
                    String beamResult = "OK";
                    String beamResultDetails = "";
                    String xResultDetails = "";
                    string yResultDetails = "";

                    double maxX = 0, maxY = 0; // need to make sure I handle negative values as well
                    double limitX, limitY;

                    string snout = beam.SnoutId;

                    switch (snout)
                    {
                        case "S1":
                            limitX = xLimitSnoutS1;
                            limitY = yLimitSnoutS1;
                            break;
                        case "15x15":
                            limitX = xLimitSnout15;
                            limitY = yLimitSnout15;
                            break;
                        case "30x30":
                            limitX = xLimitSnout30;
                            limitY = yLimitSnout30;
                            break;
                        default:
                            limitX = 0;
                            limitY = 0;
                            break;
                            

                    }

                    foreach (IonControlPoint icp in beam.IonControlPoints)
                    {
                        foreach (IonSpot spot in icp.FinalSpotList)
                        {
                            if (Math.Abs(spot.Position.x) > Math.Abs(maxX))
                            {
                                if (Math.Abs(spot.Position.x) > Math.Abs(limitX))
                                {
                                    beamResult = "Fail";
                                    Result = "Fail";
                                    DisplayColor = ResultColorChoices.Fail;
                                    maxX = spot.Position.x;
                                    xResultDetails = $"Max X: {Math.Round(maxX, 2)}, Layer({icp.NominalBeamEnergy.ToString()})";
                                }
                                else
                                {
                                    maxX = spot.Position.x;
                                    xResultDetails = $"Max X: {Math.Round(maxX, 2)}, Layer({icp.NominalBeamEnergy.ToString()})";
                                }

                            }
                            if (Math.Abs(spot.Position.y) > Math.Abs(maxY))
                            {
                                if (Math.Abs(spot.Position.y) > Math.Abs(limitY))
                                {
                                    beamResult = "Fail";
                                    Result = "Fail";
                                    DisplayColor = ResultColorChoices.Fail;
                                    maxY = spot.Position.y;
                                    yResultDetails = $"Max Y: {Math.Round(maxY, 2)}, Layer({icp.NominalBeamEnergy.ToString()})";
                                }
                                else
                                {
                                    maxY = spot.Position.y;
                                    yResultDetails = $"Max Y: {Math.Round(maxY, 2)}, Layer({icp.NominalBeamEnergy.ToString()})";
                                }

                            }

                        }


                    }

                    ResultDetails += $"\n\t{beamResult} - {beam.Id}: {xResultDetails}, {yResultDetails}";
                }


            }

        }
    }
}
