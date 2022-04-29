using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.TPS.Common.Model.API;

namespace PlanCheck.Checks
{
	public class ProtonFieldNameChecks : PlanCheckBaseProton
	{
		protected override List<string> MachineExemptions => new List<string> { };

		public ProtonFieldNameChecks(PlanSetup plan) : base(plan) { }

		public override void RunTestProton(IonPlanSetup plan)
		{

			DisplayName = "Field Names";
			
			TestExplanation = "Checks that Field Names are correct and consistent with gantry and couch angle ";
			
			IonPlanSetup ionPlan = (IonPlanSetup)plan;

			bool WrongName = false;

			string WrongFields = "";

			int WrongFieldsNumber = 0;

			foreach (IonBeam field in ionPlan.IonBeams)
			{
				if (!field.IsSetupField)
                {
					var Gantry = (int)field.ControlPoints.FirstOrDefault().GantryAngle;

					var Couch = (int)field.ControlPoints.FirstOrDefault().PatientSupportAngle;

					string FieldName = "";

					if (Couch == 0)
					{
						if (Gantry == 0)
						{
							FieldName += "AP";
						}
						else if (Gantry < 90 && Gantry != 0)
						{
							FieldName += "LAO";
						}
						else if (Gantry == 90)
						{
							FieldName += "LtLat";
						}
						else if (Gantry > 90 && Gantry != 180)
						{
							FieldName += "LPO";
						}
						else if (Gantry == 180)
						{
							FieldName += "PA";
						}

					}
					else if (Couch == 180)
					{
						if (Gantry == 0)
						{
							FieldName += "AP";
						}
						else if (Gantry < 90 && Gantry != 0)
						{
							FieldName += "RAO";
						}
						else if (Gantry == 90)
						{
							FieldName += "RtLat";
						}
						else if (Gantry > 90 && Gantry != 180)
						{
							FieldName += "RPO";
						}
						else if (Gantry == 180)
						{
							FieldName += "PA";
						}
					}
					else if (Couch == 270)
					{
						FieldName += "Vert";
					}

					FieldName += "G" + Gantry + "C" + Couch;

					if (!FieldName.ToLower().Equals(field.Id.ToLower()))
					{
						WrongName = true;
						WrongFieldsNumber++;
						WrongFields += "Field " + field.Id + " is not correct.  Script suggestion: " + FieldName +"\n";
					}
				}
			}

			if (!WrongName)
			{
				Result = "Pass";
				ResultDetails += $"All Field names are correct";
				DisplayColor = ResultColorChoices.Pass;
			}
			else
			{
				Result = "FAIL";
				ResultDetails += $"There are {WrongFieldsNumber} fields named improperly and they are:\n {WrongFields}";
				DisplayColor = ResultColorChoices.Fail;
			}

		}

	}

}







