using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VMS.TPS.Common.Model.API;
using VMS.TPS.Common.Model.Types;

namespace PlanCheck.Checks
{
    public class CouchStructuresChecks : PlanCheckBase
    {
        protected override List<string> MachineExemptions => new List<string>
		{
			DepartmentInfo.MachineNames.CEN_EX,
			DepartmentInfo.MachineNames.CLA_EX,
			DepartmentInfo.MachineNames.LAN_IX,
			DepartmentInfo.MachineNames.MAC_IX,
			DepartmentInfo.MachineNames.MAC_TB
		};

        public CouchStructuresChecks(PlanSetup plan) : base(plan) { }

        protected override void RunTest(PlanSetup plan)
		{
			DisplayName = "Couch Structures";
			TestExplanation = "Checks that the correct couch structure based on department standards";

			IEnumerable<Structure> couchStructures = null;
			string couchName = "";
			bool couchStructure;
			bool IMRT = false;

			//find couch structure if it exists
			if ((from s in plan.StructureSet.Structures where s.DicomType == "SUPPORT" select s).Count() > 0)
			{
				couchStructures = (from s in plan.StructureSet.Structures where s.DicomType == "SUPPORT" select s);
				couchStructure = true;
				Structure firstCouch = couchStructures.FirstOrDefault();
				if (firstCouch.Name != "")
					couchName = firstCouch.Name;
				else
					couchName = firstCouch.Comment;
			}
			else
				couchStructure = false;

            #region Port Huron
			// No couch for SRS (1 mm slices) plans
			// Varian IGRT couch for any other IMRT/VMAT plan
			// Ignore for 3D plans
            if (Department == Department.MPH)
			{
				//if plan likely an SRS (with 1 mm slices) it should not include a couch
				if (plan.StructureSet.Image.ZRes == 1)
				{
					if (couchStructure)
					{
						Result = "Warning";
						ResultDetails = "Couch structures should not be included for SRS plans";
						DisplayColor = ResultColorChoices.Warn;

						AddCouchStructureInfo(couchName, couchStructures);
					}
					else
					{
						Result = "";
						ResultDetails = "No couch structures";
						DisplayColor = ResultColorChoices.Pass;
					}
				}
				//Non SRS plan
				else
				{
					//check to see if VMAT/IMRT
					foreach (Beam field in plan.Beams)
					{
						//ignore setup fields
						if (!field.IsSetupField)
						{
							//for non 3D/FiF plans on a TrueBeam a couch should exist
							if (field.TreatmentUnit.MachineModel == "TDS" && !(field.MLCPlanType == MLCPlanType.Static || (field.MLCPlanType == MLCPlanType.DoseDynamic && field.ControlPoints.Count() > 25)))  //This should mean it's anything but a 3D of FiF plan
								IMRT = true;
						}
					}

					//for non 3D/FiF plans a couch should exist
					if (IMRT)
					{
						if (couchStructure)
						{
							if (couchName.Contains("IGRT"))
							{
								Result = "";
								DisplayColor = ResultColorChoices.Pass;

								AddCouchStructureInfo(couchName, couchStructures);
							}
							else
							{
								Result = "Warning";
								ResultDetails = "IGRT couch structures not inserted, please check that correct couch is inserted";
								DisplayColor = ResultColorChoices.Warn;

								AddCouchStructureInfo(couchName, couchStructures);
							}
						}
						else
						{
							Result = "Warning";
							ResultDetails = "No couch structures included";
							DisplayColor = ResultColorChoices.Warn;
						}
					}
					else
					{
						Result = "";
						if (couchStructure)
						{
							AddCouchStructureInfo(couchName, couchStructures);
						}
						else
							ResultDetails = "No couch structures included";
						DisplayColor = ResultColorChoices.Pass;
					}
				}
			}
            #endregion

			# region Lapeer/Owosso
			// Flat Panel couch for all plans
            else if (Department == Department.LAP ||
					 Department == Department.OWO)
			{
				//check to see if VMAT/IMRT
				foreach (Beam field in plan.Beams)
				{
					//ignore setup fields
					if (!field.IsSetupField)
					{
						//for non 3D/FiF plans on a TrueBeam a couch should exist
						if (!(field.MLCPlanType == MLCPlanType.Static || (field.MLCPlanType == MLCPlanType.DoseDynamic && field.ControlPoints.Count() > 25)))  //This should mean it's anything but a 3D of FiF plan
							IMRT = true;
					}
				}

				//couch should always exist
				if (couchStructure)
				{
					if (couchName.Contains("Flat panel"))
					{
						Result = "";
						DisplayColor = ResultColorChoices.Pass;

						AddCouchStructureInfo(couchName, couchStructures);
					}
					else
					{
						Result = "Warning";
						ResultDetails = "Flat panel couch structures not inserted, please check that correct couch is inserted";
						DisplayColor = ResultColorChoices.Warn;

						AddCouchStructureInfo(couchName, couchStructures);
					}
				}
				else
				{
					Result = "Warning";
					ResultDetails = "No couch structures included";
					DisplayColor = ResultColorChoices.Warn;
				}
			}
			#endregion

			#region Flint/Detroit/Farmington
			// Just display the couch structures used and make it yellow
			else if (Department == Department.FLT ||
					 Department == Department.DET ||
					 Department == Department.FAR)
			{
				if (couchStructure)
				{
					Result = "";
					DisplayColor = ResultColorChoices.Warn;

					AddCouchStructureInfo(couchName, couchStructures);
				}
				else
				{
					Result = "Warning";
					ResultDetails = "No couch structures included";
					DisplayColor = ResultColorChoices.Warn;
				}
			}
            #endregion

			#region Northern
            else if (Department == Department.NOR)
			{
				if (MachineID == DepartmentInfo.MachineNames.NOR_TB)
                {
					if (couchStructure)
					{
						Result = "";
						DisplayColor = ResultColorChoices.Warn;

						AddCouchStructureInfo(couchName, couchStructures);
					}
					else
					{
						ResultDetails = "No couch structures included";
						DisplayColor = ResultColorChoices.Warn;
					}
				}
				else
				{
					if (couchStructure)
					{
						if (couchName == "BrainLab - iBeam Couch")
						{
							Result = "";
							AddCouchStructureInfo(couchName, couchStructures);
						}
						else
						{
							Result = "Warning";
							ResultDetails = "BrainLab couch structures not inserted, please check that correct couch is inserted";

							AddCouchStructureInfo(couchName, couchStructures);
						}
					}
					else
					{
						Result = "";
						ResultDetails = "No couch structures included";
					}
					Result = "";
					DisplayColor = ResultColorChoices.Warn;
				}
			}
			#endregion

			#region Proton
			else if (Department == Department.PRO)
			{
				if (couchStructure)
				{
					// needed
					//  Structure called couch 
					//  HUs need to be correct (-930)
					//  Body contains couch

					if (plan.StructureSet.Structures.Where(s => s.Id.ToUpper() == "COUCH").Count()==1)
                    {
						var protonCouchStruct = plan.StructureSet.Structures.FirstOrDefault(s => s.Id.ToUpper() == "COUCH");
						ResultDetails = "";

						if (protonCouchStruct.GetAssignedHU(out double protonCouchHU))
                        {
                            if (protonCouchHU != -930)
                            {
								Result = "Warning";
								ResultDetails += $"Couch found but HU set to {protonCouchHU} not -930\n";
								DisplayColor = ResultColorChoices.Warn;
							}

							// Check if Couch is inside body
							var protonBodyStruct = plan.StructureSet.Structures.FirstOrDefault(s => s.DicomType.ToUpper() == "BODY");
							if (!protonCouchStruct.Equals(protonCouchStruct.And(protonBodyStruct)))
							{
								Result = "Warning";
								ResultDetails += $"Portions of Couch may not be in Body structure\n";
								DisplayColor = ResultColorChoices.Warn;
							}
							
							if (protonCouchHU != -930 && protonCouchStruct.Equals(protonCouchStruct.And(protonBodyStruct)))
                            {
								Result = "Pass";
								ResultDetails += $"Couch structure found inside body with HU of -930";
								DisplayColor = ResultColorChoices.Pass;
							}

                        }
                        else // No HU assigned to structure called couch
                        {
							var protonBodyStruct = plan.StructureSet.Structures.FirstOrDefault(s => s.DicomType.ToUpper() == "BODY");
							if (protonCouchStruct.Equals(protonCouchStruct.And(protonBodyStruct)))
							{
								Result = "Warning";
								ResultDetails += $"Couch structure found inside Body, but no HU assigned\n";
								DisplayColor = ResultColorChoices.Warn;
							}
							else
                            {
								Result = "Warning";
								ResultDetails += $"Couch structure has no Assigned HU and is not inside the Body structure\n";
								DisplayColor = ResultColorChoices.Warn;
							}


						}
					}

					else
                    {
						Result = "Warning";
						ResultDetails = "No Structure named \"Couch\" included in structure set";
						DisplayColor = ResultColorChoices.Warn; 

					}

					Result = "";
					DisplayColor = ResultColorChoices.Warn;

					AddCouchStructureInfo(couchName, couchStructures);
				}
				else
				{
					Result = "Warning";
					ResultDetails = "No couch structures included";
					DisplayColor = ResultColorChoices.Warn;
				}
			}


			#endregion
			else
				TestNotImplemented();
		}

		/// <summary>
		/// Add couch info to ResultDetails
		/// </summary>
		private void AddCouchStructureInfo(string couchName, IEnumerable<Structure> couchStructures)
		{
			if (ResultDetails == null)
				ResultDetails = couchName;
			else
				ResultDetails += $"\n{couchName}";

			//dislay HU values for all support structures
			foreach (Structure couch in couchStructures)
			{
				if (!couch.GetAssignedHU(out double HU))
					ResultDetails += $"\n{couch.Id}: HU = N/A";

				ResultDetails += $"\n{couch.Id}: HU = {HU}";
			}
		}
	}
}
