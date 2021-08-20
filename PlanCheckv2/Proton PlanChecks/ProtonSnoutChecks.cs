using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.TPS.Common.Model.API;

namespace PlanCheck.Checks
{
    public class ProtonSnoutChecks : PlanCheckBaseProton
    {
        protected override List<string> MachineExemptions => new List<string> { };

        public ProtonSnoutChecks(PlanSetup plan) : base(plan) { }

        private List<string> openFieldSnouts = new List<string> { "S1" };
        private List<string> r40Snouts = new List<string> { "15x15","30x30" };

        public override void RunTestProton(IonPlanSetup plan)
        {
            DisplayName = "Snout Check";
            Result = "Pass"; // Setting initially to pass then switching it if failure detected
            DisplayColor = ResultColorChoices.Pass;

            foreach (IonBeam beam in plan.IonBeams)
            {
                if (!beam.IsSetupField)
                {
                    string snout = beam.SnoutId;
                    string rangeShifter;
                    
                    if (beam.RangeShifters.Count() > 0)
                    {
                        rangeShifter = beam.RangeShifters.FirstOrDefault().Id;  // This is here in case there are multiple range shifters in the future.
                        if (r40Snouts.Contains(snout))
                        {
                            ResultDetails += $"Beam: {beam.Id}  Snout: {snout}  RS: {rangeShifter} - OK\n";
                        }
                        else
                        {
                            Result = "Fail";
                            DisplayColor = ResultColorChoices.Fail;
                            ResultDetails += $"Beam: {beam.Id}  Snout: {snout}  RS: {rangeShifter} - Fail\n";
                        }

                    }
                    else // No range shifter
                    {
                        rangeShifter = "none";
                        if (openFieldSnouts.Contains(snout))
                        {
                            ResultDetails += $"Beam: {beam.Id}  Snout: {snout}  RS: {rangeShifter} - OK\n";
                        }
                        else
                        {
                            Result = "Fail";
                            DisplayColor = ResultColorChoices.Fail;
                            ResultDetails += $"Beam: {beam.Id}  Snout: {snout}  RS: {rangeShifter} - Fail\n";
                        }
                    }
                }
            }
        }
    }
}
