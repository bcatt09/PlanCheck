using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.TPS.Common.Model.API;

namespace PlanCheck.Checks
{
    public abstract class PlanCheckBasePhoton : PlanCheckBase
    {
        public PlanCheckBasePhoton(PlanSetup plan) : base(plan) { }

        public override void RunTest(PlanSetup plan)
        {
            MachineExemptions.Concat(DepartmentInfo.ProtonGantries);
            RunTestPhoton(plan as ExternalPlanSetup);
        }

        public abstract void RunTestPhoton(ExternalPlanSetup plan);
    }
}
