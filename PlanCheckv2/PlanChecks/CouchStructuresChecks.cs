using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VMS.TPS.Common.Model.API;
using VMS.TPS.Common.Model.Types;

namespace VMS.TPS.PlanChecks
{
    public class CouchStructuresChecks : PlanCheck
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
						ResultColor = "Gold";

						AddCouchStructureInfo(couchName, couchStructures);
					}
					else
					{
						Result = "";
						ResultDetails = "No couch structures";
						ResultColor = "LimeGreen";
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
								ResultColor = "LimeGreen";

								AddCouchStructureInfo(couchName, couchStructures);
							}
							else
							{
								Result = "Warning";
								ResultDetails = "IGRT couch structures not inserted, please check that correct couch is inserted";
								ResultColor = "Gold";

								AddCouchStructureInfo(couchName, couchStructures);
							}
						}
						else
						{
							Result = "Warning";
							ResultDetails = "No couch structures included";
							ResultColor = "Gold";
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
						ResultColor = "LimeGreen";
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
						ResultColor = "LimeGreen";

						AddCouchStructureInfo(couchName, couchStructures);
					}
					else
					{
						Result = "Warning";
						ResultDetails = "Flat panel couch structures not inserted, please check that correct couch is inserted";
						ResultColor = "Gold";

						AddCouchStructureInfo(couchName, couchStructures);
					}
				}
				else
				{
					Result = "Warning";
					ResultDetails = "No couch structures included";
					ResultColor = "Gold";
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
					ResultColor = "Gold";

					AddCouchStructureInfo(couchName, couchStructures);
				}
				else
				{
					Result = "Warning";
					ResultDetails = "No couch structures included";
					ResultColor = "Gold";
				}
			}
            #endregion

			#region Northern
            else if (Department == Department.NOR)
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
				ResultColor = "Gold";
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
