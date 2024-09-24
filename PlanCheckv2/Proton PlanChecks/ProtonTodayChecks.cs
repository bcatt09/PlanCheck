using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.TPS.Common.Model.API;


namespace PlanCheck.Checks
{
    public class ProtonTodayChecks : PlanCheckBaseProton
    {

        protected override List<string> MachineExemptions => new List<string> { };

        public ProtonTodayChecks(PlanSetup plan) : base(plan) { }

        public override void RunTestProton(IonPlanSetup plan)
        {
            //
            //

            //plan.


        }



    }
}
