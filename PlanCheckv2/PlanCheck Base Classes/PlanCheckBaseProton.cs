using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.TPS.Common.Model.API;

namespace PlanCheck.Checks
{
    public abstract class PlanCheckBaseProton : PlanCheckBase
    {
        public PlanCheckBaseProton(PlanSetup plan) : base(plan) { }

        public override void RunTest(PlanSetup plan)
        {
            MachineExemptions.Concat(DepartmentInfo.LinearAccelerators);
            RunTestProton(plan as IonPlanSetup);
        }

        public abstract void RunTestProton(IonPlanSetup plan);
    }
}
