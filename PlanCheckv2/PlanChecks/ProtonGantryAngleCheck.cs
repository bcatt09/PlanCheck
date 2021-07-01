using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.TPS.Common.Model.API;
using VMS.TPS.Common.Model.Types;

namespace PlanCheck.Checks
{
    class ProtonGantryAngleCheck : PlanCheckBase
    {
        protected override List<string> MachineExemptions => DepartmentInfo.LinearAccelerators;
    
        public ProtonGantryAngleCheck(PlanSetup plan) : base(plan) { }

        protected override void RunTest(PlanSetup plan)
        {
            DisplayName = "Gantry Angle Checks";
            TestExplanation = "This test will ensure Gantry Angles used in plan are allowable";
            IonPlanSetup ionPlan = (IonPlanSetup)plan;
            ResultDetails = $"Gantry angle verification for: {MachineID}\n";
            Result = "Testing";

            List<int> G1CommissionedGantryAngles = new List<int> { 0,15,30,45,60,75,90,120,135,150,180 };
            List<int> G1CommissionedCouchAngles = new List<int> { 0,180,270 };

            List<int> G2CommissionedGantryAngles = new List<int> { 0,15,30,45,60,75,90,120,135,150,180 };
            List<int> G2CommissionedCouchAngles = new List<int> { 0, 180, 270 };


            foreach (IonBeam beam in ionPlan.IonBeams)
            {
                if (!beam.IsSetupField)
                {
                    if(beam.TreatmentUnit.Id == DepartmentInfo.MachineNames.PRO_G1)
                    {
                        if (G1CommissionedGantryAngles.Contains((int)beam.ControlPoints.FirstOrDefault().GantryAngle) && G1CommissionedCouchAngles.Contains((int)beam.ControlPoints.FirstOrDefault().PatientSupportAngle))
                        {
                            ResultDetails += $"  Field: {beam.Id}: Gantry: {beam.ControlPoints.FirstOrDefault().GantryAngle} Couch: {beam.ControlPoints.FirstOrDefault().PatientSupportAngle}- OK\n";
                        }
                        else
                        {
                            Result = "Fail";
                            DisplayColor = ResultColorChoices.Fail;
                            ResultDetails += $"FAILED - Field: {beam.Id}: Gantry: {beam.ControlPoints.FirstOrDefault().GantryAngle} Couch: {beam.ControlPoints.FirstOrDefault().PatientSupportAngle} not in allowed list.\n";
                        }
                    }

                    if (beam.TreatmentUnit.Id == DepartmentInfo.MachineNames.PRO_G2)
                    {
                        if (G2CommissionedGantryAngles.Contains((int)beam.ControlPoints.FirstOrDefault().GantryAngle) && G2CommissionedCouchAngles.Contains((int)beam.ControlPoints.FirstOrDefault().PatientSupportAngle))
                        {
                            ResultDetails += $"  Field: {beam.Id}: Gantry: {beam.ControlPoints.FirstOrDefault().GantryAngle} Couch: {beam.ControlPoints.FirstOrDefault().PatientSupportAngle}- OK\n";
                        }
                        else
                        {
                            Result = "Fail";
                            DisplayColor = ResultColorChoices.Fail;
                            ResultDetails += $"FAILED - Field: {beam.Id}: Gantry: {beam.ControlPoints.FirstOrDefault().GantryAngle} Couch: {beam.ControlPoints.FirstOrDefault().PatientSupportAngle} not in allowed list.\n";
                        }
                    }

                }
            }

            if (Result == "Testing")
            {
                Result = "Pass";
                DisplayColor = ResultColorChoices.Pass;
            }

        }
    }
}
