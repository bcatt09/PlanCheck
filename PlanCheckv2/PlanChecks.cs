using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Windows;
using VMS.TPS.Common.Model.API;
using VMS.TPS.Common.Model.Types;

namespace VMS.TPS
{
	public static class Globals
	{
		//treatment unit display names
		public static class MachineDisplayNames
		{
			public static string BAYTB = "BAY - TrueBeam";
			public static string CENEX = "CEN - EX";
			public static string CLAEX = "CLA - EX";
			public static string DETIX = "DET - iX";
			public static string DETTB = "DET - TrueBeam";
			public static string FARIX = "FAR - iX";
			public static string FLTFTB = "FLT - Front TrueBeam";
			public static string FLTBTB = "FLT - Back TrueBeam";
			public static string LANIX = "LAN - iX";
			public static string LAPIX = "LAP - iX";
			public static string MACTB = "MAC - TrueBeam";
			public static string MACIX = "MAC - Trilogy";
			public static string MPHTB = "MPH - TrueBeam";
			public static string NOREX = "NOR - EX";
			public static string NORIX = "NOR - Trilogy";
			public static string OWOIX = "OWO - iX";

		}

		//test names
		public static class TestNames
		{
			public static string FieldNames = "Field Names";
			public static string DoseRates = "Dose Rates";
			public static string Isocenter = "Isocenter";
			public static string SelectedCT = "Selected CT";
			public static string Target = "Target";
			public static string UseGated = "\"Use Gated\"";
			public static string JawTracking = "Jaw Tracking";
			public static string CouchValues = "Couch Values";
			public static string Machine = "Machine";
			public static string ToleranceTables = "Tolerance Tables";
			public static string Bolus = "Bolus";
			public static string CouchStructures = "Couch Structures";
			public static string CalculationParameters = "Calculation Parameters";
			public static string DRRs = "DRRs";
			public static string PlanApproval = "Plan Approval";
			public static string CalcShifts = "Patient Shifts";
			public static string Orientation = "Patient Orientation";
			public static string Prescription = "Plan Prescription";
			public static string Hotspot = "Hotspot";

			//This is the display order
			public static List<string> Tests = new List<string>
			{
				Machine,
				DoseRates,
				SelectedCT,
				Orientation,
				Target,
				Hotspot,
				PlanApproval,
				Prescription,
				Isocenter,
				FieldNames,
				JawTracking,
				CouchStructures,
				CouchValues,
				CalcShifts,
				ToleranceTables,
				Bolus,
				DRRs,
				UseGated,
				CalculationParameters
			};
		}

		//dictionary of treatment units
		public static Dictionary<string, string> TreatmentUnits = new Dictionary<string, string>
		{
			{MachineDisplayNames.BAYTB, "BAY_TB3384"},
			{MachineDisplayNames.CENEX, "CMCH-21EX"},
			{MachineDisplayNames.CLAEX, "21EX"},
			{MachineDisplayNames.DETIX, "IX_GROC"},
			{MachineDisplayNames.DETTB, "GROC_TB1601"},
			{MachineDisplayNames.FARIX, "IX_Farmington"},
			{MachineDisplayNames.FLTFTB, "TrueBeamSN2873"},
			{MachineDisplayNames.FLTBTB, "TrueBeam1030"},
			{MachineDisplayNames.LANIX, "ING21IX1"},
			{MachineDisplayNames.LAPIX, "21IX-SN3743"},
			{MachineDisplayNames.MACTB, "MAC_TB3568"},
			{MachineDisplayNames.MACIX, "TRILOGY3789"},
			{MachineDisplayNames.MPHTB, "TB2681"},
			{MachineDisplayNames.NOREX, "2100ex"},
			{MachineDisplayNames.NORIX, "TRILOGY"},
			{MachineDisplayNames.OWOIX, "21IX-SN3856"}
		};

		//dictionary of CT sims
		public static Dictionary<string, string> CTs = new Dictionary<string, string>
		{
			{"BAY", "CT99"},
			{"CEN", "cmchctcon"},
			{"CLA", "LightSpeed RT16"},
			{"DET", "Gershenson CT"},
			{"FAR", "Farmington CT"},
			{"FLT", "Philips Big Bore"},
			{"LAN", "BBCT" },
			{"LAP", "Philips Big Bore"},
			{"MAC", "LightSpeed RT16"},
			{"MPH", "MPH CT Sim"},
			{"NOR", "Oncology"},
			{"OWO", "Philips Big Bore"}
		};

		//dictionary of exemptions from tests (tests will not be performed/displayed for these linacs)
		public static Dictionary<string, List<string>> Exemptions = new Dictionary<string, List<string>>
		{
			{TestNames.FieldNames, new List<string>{ } },
			{TestNames.DoseRates, new List<string>{ } },
			{TestNames.Isocenter, new List<string>{ } },
			{TestNames.SelectedCT, new List<string>{ } },
			{TestNames.Target, new List<string>{ } },
			{TestNames.UseGated, new List<string>{ MachineDisplayNames.CENEX, MachineDisplayNames.CLAEX, MachineDisplayNames.DETIX, MachineDisplayNames.FARIX, MachineDisplayNames.LANIX, MachineDisplayNames.LAPIX, MachineDisplayNames.MACIX, MachineDisplayNames.NOREX, MachineDisplayNames.NORIX, MachineDisplayNames.OWOIX } },
			{TestNames.CouchValues, new List<string>{ } },
			{TestNames.Machine, new List<string>{ } },
			{TestNames.ToleranceTables, new List<string>{ } },
			{TestNames.Bolus, new List<string>{ } },
			{TestNames.CouchStructures, new List<string>{ MachineDisplayNames.CLAEX, MachineDisplayNames.LANIX, MachineDisplayNames.MACIX, MachineDisplayNames.MACTB } },
			{TestNames.CalculationParameters, new List<string>{ } },
			{TestNames.DRRs, new List<string>{ } },
			{TestNames.PlanApproval, new List<string>{ } },
			{TestNames.CalcShifts, new List<string>{ } },
			{TestNames.Orientation, new List<string>{ } },
			{TestNames.Prescription, new List<string>{ } },
			{TestNames.JawTracking, new List<string>{ MachineDisplayNames.CENEX, MachineDisplayNames.CLAEX, MachineDisplayNames.DETIX, MachineDisplayNames.FARIX, MachineDisplayNames.LANIX, MachineDisplayNames.LAPIX, MachineDisplayNames.MACIX, MachineDisplayNames.NOREX, MachineDisplayNames.NORIX, MachineDisplayNames.OWOIX } },
			{TestNames.Hotspot, new List<string>{ } }
		};
	}

	public static class StringExtensions
	{
		public static bool Contains(this string source, string toCheck, StringComparison comp)
		{
			return source?.IndexOf(toCheck, comp) >= 0;
		}
	}

	public class PlanCheck : INotifyPropertyChanged
	{
		private ScriptContext _context;
		private string _machine;			//machine used in planning
		private string _selectedMachineUI;	//selected machine from UI which will be used for selecting rules for tests (this should just always match the plan machine though)
		private string _test;				//test name
		private string _result;				//pass/fail result to be displayed
		private string _resultDetails;		//more info to be displayed in row details in datagrid
		private string _resultColor;		//color of the result
		private string _testExplanation;	//explanation of the test
		
		public string Test { get { return _test; } set { _test = value; OnPropertyChanged("Test"); } }
		public string Result { get { return _result; } set { _result = value; OnPropertyChanged("Result"); } }
		public string ResultDetails { get { return _resultDetails; } set { _resultDetails = value; OnPropertyChanged("ResultDetails"); } }
		public string ResultColor { get { return _resultColor; } set { _resultColor = value; OnPropertyChanged("ResultColor"); } }
		public string TestExplanation { get { return _testExplanation; } set { _testExplanation = value; OnPropertyChanged("TestExplanation"); } }

		public PlanCheck(string test, string selectedTreatmentUnit, ScriptContext context)
		{
			_context = context;
			_machine = context.PlanSetup.Beams.First().TreatmentUnit.Id;
			_selectedMachineUI = selectedTreatmentUnit;

			Test = test;

			try
			{
				RunTest(test);
			}
			catch(Exception e)
			{
				TestCouldNotComplete(test + " - " + e.Message);
			}
		}


		private void RunTest(string test)
		{
			if (test == Globals.TestNames.FieldNames)
				CheckFieldNames();
			else if (test == Globals.TestNames.DoseRates)
				CheckDoseRates();
			else if (test == Globals.TestNames.Isocenter)
				CheckIsocenterPosition();
			else if (test == Globals.TestNames.SelectedCT)
				CheckCTSim();
			else if (test == Globals.TestNames.Target)
				CheckTargetPieces();
			else if (test == Globals.TestNames.UseGated)
				CheckForUseGated();
			else if (test == Globals.TestNames.CouchValues)
				CheckForCouchValues();
			else if (test == Globals.TestNames.Machine)
				CheckMachine();
			else if (test == Globals.TestNames.ToleranceTables)
				CheckToleranceTables();
			else if (test == Globals.TestNames.Bolus)
				CheckLinkedBolus();
			else if (test == Globals.TestNames.CouchStructures)
				CheckCouchStructures();
			else if (test == Globals.TestNames.CalculationParameters)
				CheckCalcParameters();
			else if (test == Globals.TestNames.DRRs)
				CheckDRRs();
			else if (test == Globals.TestNames.PlanApproval)
				PlanApproval();
			else if (test == Globals.TestNames.CalcShifts)
				CalcShifts();
			else if (test == Globals.TestNames.Orientation)
				CheckOrientation();
			else if (test == Globals.TestNames.Prescription)
				CheckPrescription();
			else if (test == Globals.TestNames.JawTracking)
				CheckForJawTracking();
			else if (test == Globals.TestNames.Hotspot)
				CheckHotspot();
			else
			{
				ThrowNotImplemented();
				throw new NotImplementedException($"No \"{test}\" test defined");
			}
		}


		/// <summary>
		/// Checks fields names against gantry angles and directions and stores results
		/// 
		/// Flint offsites use Varian scale
		/// </summary>
		public void CheckFieldNames()
		{
			ResultDetails = "";
			TestExplanation = "Checks that field names follow OneAria naming conventions";

			if (_selectedMachineUI != Globals.TreatmentUnits[Globals.MachineDisplayNames.LAPIX] &&
				_selectedMachineUI != Globals.TreatmentUnits[Globals.MachineDisplayNames.NOREX] &&
				_selectedMachineUI != Globals.TreatmentUnits[Globals.MachineDisplayNames.NORIX] &&
				_selectedMachineUI != Globals.TreatmentUnits[Globals.MachineDisplayNames.OWOIX])
			{
				foreach (Beam field in _context.PlanSetup.Beams)
				{
					//ignore setup fields
					if (!field.IsSetupField)
					{
						//for static fields, check that the gantry angle is contained in the field name
						if (field.Technique.ToString() == "STATIC" || field.Technique.ToString() == "SRS STATIC")
						{
							//field name matching pattern: g125 with or without a space or "_" between
							string fieldNameGantry = "(?i)g_? ?" + Math.Round(field.ControlPoints.First().GantryAngle, 0).ToString();

							if (!Regex.IsMatch(field.Id, fieldNameGantry))
							{
								Result = "Warning";
								ResultDetails += "Field name mismatch  —  Field: " + field.Id + "  Gantry Angle: " + field.ControlPoints.FirstOrDefault().GantryAngle.ToString() + "\n";
								ResultColor = "Gold";
							}
						}
						//for arc fields, check that cw vs ccw matches rotation direction
						else if (field.Technique.ToString() == "ARC" || field.Technique.ToString() == "SRS ARC")
						{
							if (field.GantryDirection == GantryDirection.Clockwise)
							{
								if (field.Id.Contains("ccw", StringComparison.CurrentCultureIgnoreCase) || !field.Id.Contains("cw", StringComparison.CurrentCultureIgnoreCase))
								{
									Result = "Warning";
									ResultDetails += "Field name mismatch  —  Field: " + field.Id + "  Gantry Direction: " + field.GantryDirection + "\n";
									ResultColor = "Gold";
								}

							}
							else if (field.GantryDirection == GantryDirection.CounterClockwise)
							{
								if (!field.Id.Contains("ccw", StringComparison.CurrentCultureIgnoreCase))
								{
									Result = "Warning";
									ResultDetails += "Field name mismatch  —  Field: " + field.Id + "  Gantry Direction: " + field.GantryDirection + "\n";
									ResultColor = "Gold";
								}
							}
						}
						//check for pedestal kicks
						if (field.ControlPoints.First().PatientSupportAngle != 0)
						{
							//field name matching pattern: g125 with or without a space or "_" between
							string fieldNamePedestal = "(?i)p_? ?" + Math.Round(ConvertCouchAngleToVarianIECScale(field.ControlPoints.First().PatientSupportAngle), 0).ToString();

							if (!Regex.IsMatch(field.Id, fieldNamePedestal))
							{
								Result = "Warning";
								ResultDetails += "Field name mismatch  —  Field: " + field.Id + "  Pedestal Angle: " + ConvertCouchAngleToVarianIECScale(field.ControlPoints.First().PatientSupportAngle).ToString() + "\n";
								ResultColor = "Gold";
							}
						}
					}
				}

				ResultDetails = ResultDetails.TrimEnd('\n');

				//no problems found
				if (ResultDetails == "")
				{
					Result = "Pass";
					ResultColor = "LimeGreen";
				}
			}
			else if (_selectedMachineUI == Globals.TreatmentUnits[Globals.MachineDisplayNames.LAPIX] ||
					 _selectedMachineUI == Globals.TreatmentUnits[Globals.MachineDisplayNames.OWOIX])
			{
				foreach (Beam field in _context.PlanSetup.Beams)
				{
					//ignore setup fields
					if (!field.IsSetupField)
					{
						//for static fields, check that the gantry angle is contained in the field name
						if (field.Technique.ToString() == "STATIC" || field.Technique.ToString() == "SRS STATIC")
						{
							//field name matching pattern: g125 with or without a space or "_" between
							string fieldNameGantry = "(?i)g_? ?" + Math.Round(ConvertToVarianStandardScale(field.ControlPoints.First().GantryAngle), 0).ToString();

							if (!Regex.IsMatch(field.Id, fieldNameGantry))
							{
								Result = "Warning";
								ResultDetails += "Field name mismatch  —  Field: " + field.Id + "  Gantry Angle: " + ConvertToVarianStandardScale(field.ControlPoints.FirstOrDefault().GantryAngle).ToString() + "\n";
								ResultColor = "Gold";
							}
						}
						//check for pedestal kicks
						if (field.ControlPoints.First().PatientSupportAngle != 0)
						{
							//field name matching pattern: g125 with or without a space or "_" between
							string fieldNamePedestal = "(?i)p_? ?" + Math.Round(ConvertToVarianStandardScale(field.ControlPoints.First().GantryAngle), 0).ToString();

							if (!Regex.IsMatch(field.Id, fieldNamePedestal))
							{
								Result = "Warning";
								ResultDetails += "Field name mismatch  —  Field: " + field.Id + "  Pedestal Angle: " + ConvertToVarianStandardScale(field.ControlPoints.First().PatientSupportAngle).ToString() + "\n";
								ResultColor = "Gold";
							}
						}
					}
				}

				ResultDetails = ResultDetails.TrimEnd('\n');

				//no problems found
				if (ResultDetails == "")
				{
					Result = "Pass";
					ResultColor = "LimeGreen";
				}
			}
			else
				ThrowNotImplemented();
		}

		/// <summary>
		/// Checks dose rates to make sure they are all set at max
		/// </summary>
		public void CheckDoseRates()
		{
			ResultDetails = "";
			TestExplanation = "Checks that all dose rates are set to maximum allowed per department standards";

			// Macomb group
			// Flattened - 600
			// 6FFF      - 1400
			// 10FFF     - 2400
			// Electron  - 1000
			if (_selectedMachineUI == Globals.TreatmentUnits[Globals.MachineDisplayNames.CLAEX] ||
				_selectedMachineUI == Globals.TreatmentUnits[Globals.MachineDisplayNames.MACIX] ||
				_selectedMachineUI == Globals.TreatmentUnits[Globals.MachineDisplayNames.MACTB] ||
				_selectedMachineUI == Globals.TreatmentUnits[Globals.MachineDisplayNames.MPHTB])
			{
				foreach (Beam field in _context.PlanSetup.Beams)
				{
					//ignore setup fields
					if (!field.IsSetupField)
					{
						string energy = field.EnergyModeDisplayName;

						if (energy == "6X" || energy == "10X" || energy == "15X" || energy == "16X" || energy == "18X" || energy == "23X")
						{
							if (field.DoseRate < 600)
							{
								Result = "Warning";
								ResultDetails += field.Id + " dose rate set at " + field.DoseRate + "\n";
								ResultColor = "Gold";
							}
						}
						else if (energy == "6X-FFF")
						{
							if (field.DoseRate < 1400)
							{
								Result = "Warning";
								ResultDetails += field.Id + " dose rate set at " + field.DoseRate + "\n";
								ResultColor = "Gold";
							}
						}
						else if (energy == "10X-FFF")
						{
							if (field.DoseRate < 2400)
							{
								Result = "Warning";
								ResultDetails += field.Id + " dose rate set at " + field.DoseRate + "\n";
								ResultColor = "Gold";
							}
						}
						else if (energy.Contains("E", StringComparison.CurrentCultureIgnoreCase))
						{
							if (field.DoseRate < 1000)
							{
								Result = "Warning";
								ResultDetails += field.Id + " dose rate set at " + field.DoseRate + "\n";
								ResultColor = "Gold";
							}
						}
					}
				}

				//no problems found
				if (ResultDetails == "")
				{
					Result = "";
					ResultDetails = _context.PlanSetup.Beams.Where(x => !x.IsSetupField).First().DoseRate.ToString();
					ResultColor = "LimeGreen";
				}

				ResultDetails = ResultDetails.TrimEnd('\n');
			}

			// Flint TrueBeams
			// Flattened - 600
			// 6FFF      - 1400
			// 10FFF     - 2400
			// Electron  - 600
			else if (_selectedMachineUI == Globals.TreatmentUnits[Globals.MachineDisplayNames.FLTFTB] ||
					 _selectedMachineUI == Globals.TreatmentUnits[Globals.MachineDisplayNames.FLTBTB])
			{
				foreach (Beam field in _context.PlanSetup.Beams)
				{
					//ignore setup fields
					if (!field.IsSetupField)
					{
						string energy = field.EnergyModeDisplayName;

						if (energy == "6X" || energy == "10X" || energy == "15X" || energy == "16X" || energy == "18X" || energy == "23X")
						{
							if (field.DoseRate < 600)
							{
								Result = "Warning";
								ResultDetails += field.Id + " dose rate set at " + field.DoseRate + "\n";
								ResultColor = "Gold";
							}
						}
						else if (energy == "6X-FFF")
						{
							if (field.DoseRate < 1400)
							{
								Result = "Warning";
								ResultDetails += field.Id + " dose rate set at " + field.DoseRate + "\n";
								ResultColor = "Gold";
							}
						}
						else if (energy == "10X-FFF")
						{
							if (field.DoseRate < 2400)
							{
								Result = "Warning";
								ResultDetails += field.Id + " dose rate set at " + field.DoseRate + "\n";
								ResultColor = "Gold";
							}
						}
						else if (energy.Contains("E", StringComparison.CurrentCultureIgnoreCase))
						{
							if (field.DoseRate < 600)
							{
								Result = "Warning";
								ResultDetails += field.Id + " dose rate set at " + field.DoseRate + "\n";
								ResultColor = "Gold";
							}
						}
					}
				}

				//no problems found
				if (ResultDetails == "")
				{
					Result = "";
					ResultDetails = _context.PlanSetup.Beams.Where(x => !x.IsSetupField).First().DoseRate.ToString();
					ResultColor = "LimeGreen";
				}

				ResultDetails = ResultDetails.TrimEnd('\n');
			}

			// Lapeer/Owosso
			// Photon   - 600
			// Electron - 400
			else if (_selectedMachineUI == Globals.TreatmentUnits[Globals.MachineDisplayNames.LAPIX] ||
					 _selectedMachineUI == Globals.TreatmentUnits[Globals.MachineDisplayNames.OWOIX])
			{
				foreach (Beam field in _context.PlanSetup.Beams)
				{
					//ignore setup fields
					if (!field.IsSetupField)
					{
						string energy = field.EnergyModeDisplayName;

						if (energy == "6X" || energy == "10X" || energy == "15X" || energy == "16X" || energy == "18X" || energy == "23X")
						{
							if (field.DoseRate < 600)
							{
								Result = "Warning";
								ResultDetails += field.Id + " dose rate set at " + field.DoseRate + "\n";
								ResultColor = "Gold";
							}
						}
						else if (energy.Contains("E", StringComparison.CurrentCultureIgnoreCase))
						{
							if (field.DoseRate < 400)
							{
								Result = "Warning";
								ResultDetails += field.Id + " dose rate set at " + field.DoseRate + "\n";
								ResultColor = "Gold";
							}
						}
					}
				}

				//no problems found
				if (ResultDetails == "")
				{
					Result = "";
					ResultDetails = _context.PlanSetup.Beams.Where(x => !x.IsSetupField).First().DoseRate.ToString();
					ResultColor = "LimeGreen";
				}

				ResultDetails = ResultDetails.TrimEnd('\n');
			}

			// Lansing
			// IMRT     - 500
			// 3D       - 600
			// Electron - 400
			else if (_selectedMachineUI == Globals.TreatmentUnits[Globals.MachineDisplayNames.LANIX])
			{
				foreach (Beam field in _context.PlanSetup.Beams)
				{
					//ignore setup fields
					if (!field.IsSetupField)
					{
						string energy = field.EnergyModeDisplayName;

						if (energy == "6X" || energy == "10X" || energy == "15X" || energy == "16X" || energy == "18X" || energy == "23X")
						{

							//for IMRT fields that have more control points than step and shoot, dose rate should be 500
							if (field.MLCPlanType == MLCPlanType.DoseDynamic && field.ControlPoints.Count > 18)
							{
								if (field.DoseRate != 500)
								{
									Result = "Warning";
									ResultDetails += field.Id + " (IMRT) dose rate set at " + field.DoseRate + "\n";
									ResultColor = "Gold";
								}
							}
							//3D dose rate should be 600
							else
							{
								if (field.DoseRate < 600)
								{
									Result = "Warning";
									ResultDetails += field.Id + " (3D) dose rate set at " + field.DoseRate + "\n";
									ResultColor = "Gold";
								}
							}
						}
						else if (energy.Contains("E", StringComparison.CurrentCultureIgnoreCase))
						{
							if (field.DoseRate != 400)
							{
								Result = "Warning";
								ResultDetails += field.Id + " (electron) dose rate set at " + field.DoseRate + "\n";
								ResultColor = "Gold";
							}
						}
					}
				}

				//no problems found
				if (ResultDetails == "")
				{
					Result = "";
					ResultDetails = _context.PlanSetup.Beams.Where(x => !x.IsSetupField).First().DoseRate.ToString();
					ResultColor = "LimeGreen";
				}

				ResultDetails = ResultDetails.TrimEnd('\n');
			}

			else
				ThrowNotImplemented();
		}

		/// <summary>
		/// Checks for more than one isocenter position in the plan, and based on the position will check the necessity of using gantry 180E
		/// This only works for head first supine probably
		/// </summary>
		public void CheckIsocenterPosition()
		{
			ResultDetails = "";
			TestExplanation = "Checks that only a single isocenter exists in the plan\nAlso suggests using G180E if the isocenter is shifted >2cm to patient's right";

			int isocenters = 1;


			VVector firstIso = _context.PlanSetup.Beams.FirstOrDefault().IsocenterPosition;

			foreach (Beam field in _context.PlanSetup.Beams)
			{
				//check for multiple isocenters
				if (field != _context.PlanSetup.Beams.First())
				{
					//if there's no distance between the two, they are equal
					if (VVector.Distance(field.IsocenterPosition, firstIso) != 0)
					{
						isocenters++;
					}
				}

				//check if 180E might be necessary
				if ((_selectedMachineUI != Globals.TreatmentUnits[Globals.MachineDisplayNames.LAPIX] &&																				//For non Lapeer/Owosso check for g180
					_selectedMachineUI != Globals.TreatmentUnits[Globals.MachineDisplayNames.OWOIX] && !field.IsSetupField && field.ControlPoints.First().GantryAngle == 180) ||
					(_selectedMachineUI == Globals.TreatmentUnits[Globals.MachineDisplayNames.LAPIX] &&																				//For Lapeer/Owosso check for g0
					_selectedMachineUI == Globals.TreatmentUnits[Globals.MachineDisplayNames.OWOIX] && !field.IsSetupField && field.ControlPoints.First().GantryAngle == 0))
				{
					try
					{
						Structure body = (from s in _context.PlanSetup.StructureSet.Structures where (s.DicomType == "BODY" || s.DicomType == "EXTERNAL") select s).First();

						//Looks for the isocenter position to be 20 mm to the left when facing the linac. Positions are in mm
						if (field.IsocenterPosition.x - body.CenterPoint.x < -20 && (_context.PlanSetup.TreatmentOrientation == PatientOrientation.HeadFirstSupine || _context.PlanSetup.TreatmentOrientation == PatientOrientation.FeetFirstProne))
						{
							Result = "Warning";
							ResultDetails += "Isocenter is shifted to patients right, do you want to use 180E?\n";
							ResultColor = "Gold";
						}
						if (field.IsocenterPosition.x - body.CenterPoint.x > 20 && (_context.PlanSetup.TreatmentOrientation == PatientOrientation.HeadFirstProne || _context.PlanSetup.TreatmentOrientation == PatientOrientation.FeetFirstSupine))
						{
							Result = "Warning";
							ResultDetails += "Isocenter is shifted to patients left, do you want to use 180E?\n";
							ResultColor = "Gold";
						}
					}
					catch
					{
						TestCouldNotComplete("CheckIsocenterPosition - No structure with type \"BODY\" or \"EXTERNAL\" found");
					}
				}
			}

			if(isocenters > 1)
			{
				Result = "Warning";
				ResultDetails += $"{isocenters} isocenters detected, please check plan\n";
				ResultColor = "Gold";
			}

			ResultDetails = ResultDetails.TrimEnd('\n');

			//no problems found
			if (ResultDetails == "")
			{
				Result = "Pass";
				ResultColor = "LimeGreen";
			}

			//if it's Lapeer/Owosso replace "180E" with "0E"
			else
			{
				if (_selectedMachineUI == Globals.TreatmentUnits[Globals.MachineDisplayNames.LAPIX] ||
				_selectedMachineUI == Globals.TreatmentUnits[Globals.MachineDisplayNames.OWOIX])
				{
					ResultDetails = ResultDetails.Replace("180E", "0E");
				}
			}
		}

		/// <summary>
		/// Checks that the correct CT sim was added for the image series
		/// </summary>
		public void CheckCTSim()
		{
			ResultDetails = "";
			TestExplanation = "Checks that the correct CT is chosen for the image series based on what treatment unit is used for planning\nWarning: This will not correctly account for patients simmed at one facility and treated at another";

			string department = Globals.TreatmentUnits.Where(x => x.Value == _selectedMachineUI).Select(x => x.Key).First().Substring(0, 3);
			string ct = _context.PlanSetup.StructureSet.Image.Series.ImagingDeviceId;
			
			if (ct != Globals.CTs[department])
			{
				Result = "Warning";
				ResultDetails = $"Patient is being treated on {_selectedMachineUI}, but the {Globals.CTs[department]} was not chosen as the imaging device for the series, please check";
				ResultDetails += $"\nSelected CT: {ct}\n";
				ResultColor = "Gold";
			}

			if (ResultDetails == "")
			{
				Result = "";
				ResultDetails = ct;
				ResultColor = "LimeGreen";
			}

			ResultDetails = ResultDetails.TrimEnd('\n');
		}
		
		/// <summary>
		/// Checks to see if the PTV has more than one piece
		/// </summary>
		public void CheckTargetPieces()
		{
			TestExplanation = "Checks that there is only one piece to the target structure";

			if (_context.PlanSetup.TargetVolumeID != "")
			{
				//Select the target structure for the plan and get how many separate pieces it has
				Structure target = (from s in _context.PlanSetup.StructureSet.Structures where s.Id == _context.PlanSetup.TargetVolumeID select s).FirstOrDefault();
				int targetPieces = target.GetNumberOfSeparateParts();

				if (targetPieces > 1)
				{
					Result = "Warning";
					ResultDetails = $"Plan target ({target.Id}) has " + targetPieces + " separates pieces.  Is this correct?";
					ResultColor = "Gold";
				}
				else
				{
					Result = "";
					ResultDetails = target.Id;
					ResultColor = "LimeGreen";
				}
			}
			else
			{
				Result = "Warning";
				ResultDetails = "No target defined";
				ResultColor = "Gold";
			}
		}

		/// <summary>
		/// Checks to see if the plan is a low number of fractions and has a 4D image, meaning it should probably have "Use Gated" checked
		/// </summary>
		public void CheckForUseGated()
		{
			TestExplanation = "Checks to see if \"Use Gated\" should be checked off in plan properties based on department standards";

			if (_selectedMachineUI == Globals.TreatmentUnits[Globals.MachineDisplayNames.MPHTB] ||
				_selectedMachineUI == Globals.TreatmentUnits[Globals.MachineDisplayNames.MACTB])
			{
				string planningImageComment = _context.PlanSetup.StructureSet.Image.Comment;
				string planningImageId = _context.PlanSetup.StructureSet.Image.Id;
				int? numberOfFractions = _context.PlanSetup.NumberOfFractions;

				if (numberOfFractions < 10 && (planningImageComment.Contains("AIP", StringComparison.CurrentCultureIgnoreCase) || planningImageComment.Contains("avg", StringComparison.CurrentCultureIgnoreCase) || planningImageId.Contains("AIP", StringComparison.CurrentCultureIgnoreCase) || planningImageId.Contains("avg", StringComparison.CurrentCultureIgnoreCase) || planningImageId.Contains("ave", StringComparison.CurrentCultureIgnoreCase) || planningImageComment.Contains("%")))
				{
					Result = "Warning";
					ResultDetails = "Plan has a low number of fractions and looks to contain a 4D image.  Should the plan \"Use Gated\"?";
					ResultColor = "Gold";
				}
				else
				{
					Result = "Pass";
					ResultColor = "LimeGreen";
				}
			}
			else
				ThrowNotImplemented();
		}

		/// <summary>
		/// Checks to see if dynamic plans have different jaw positions, meaning that jaw tracking was enabled during optimization
		/// </summary>
		public void CheckForJawTracking()
		{
			bool IMRT = false;

			Result = "";
			TestExplanation = "Checks to see if jaw tracking is enabled for IMRT/VMAT plans";

			// use jaw tracking
			if (_selectedMachineUI == Globals.TreatmentUnits[Globals.MachineDisplayNames.MACTB] ||
				_selectedMachineUI == Globals.TreatmentUnits[Globals.MachineDisplayNames.MPHTB])
			{
				//VMAT plan
				if ((from s in _context.PlanSetup.Beams where s.MLCPlanType == MLCPlanType.VMAT select s).Count() > 0)
				{
					IMRT = true;

					foreach (Beam field in _context.PlanSetup.Beams)
					{
						//ignore setup fields
						if (!field.IsSetupField)
						{
							//for VMAT fields, check that there are different jaw positions in the control points
							if (field.MLCPlanType == MLCPlanType.VMAT)
							{
								//get initial jaw positions
								double x1 = field.ControlPoints.First().JawPositions.X1;
								double x2 = field.ControlPoints.First().JawPositions.X2;
								double y1 = field.ControlPoints.First().JawPositions.Y1;
								double y2 = field.ControlPoints.First().JawPositions.Y2;

								//if they change at any of the control points, then jaw tracking must be on
								foreach (ControlPoint point in field.ControlPoints)
								{
									if (x1 != point.JawPositions.X1 || x2 != point.JawPositions.X2 || y1 != point.JawPositions.Y1 || y2 != point.JawPositions.Y2)
									{
										Result = "Pass";
										ResultColor = "LimeGreen";
										break;
									}
								}
							}
						}
					}
				}
				//IMRT plan
				else if ((from s in _context.PlanSetup.Beams where s.MLCPlanType == MLCPlanType.DoseDynamic select s).Count() > 0)
				{
					foreach (Beam field in _context.PlanSetup.Beams)
					{
						//ignore setup fields
						if (!field.IsSetupField)
						{
							//for IMRT fields that have more control points than step and shoot, check that there are different jaw positions in the control points
							if (field.MLCPlanType == MLCPlanType.DoseDynamic && field.ControlPoints.Count > 18)
							{
								IMRT = true;

								//get initial jaw positions
								double x1 = field.ControlPoints.First().JawPositions.X1;
								double x2 = field.ControlPoints.First().JawPositions.X2;
								double y1 = field.ControlPoints.First().JawPositions.Y1;
								double y2 = field.ControlPoints.First().JawPositions.Y2;

								//if they change at any of the control points, then jaw tracking must be on
								foreach (ControlPoint point in field.ControlPoints)
								{
									if (x1 != point.JawPositions.X1 || x2 != point.JawPositions.X2 || y1 != point.JawPositions.Y1 || y2 != point.JawPositions.Y2)
									{
										Result = "Pass";
										ResultColor = "LimeGreen";
										break;
									}
								}
							}
						}
					}
				}

				//Static fields
				if(!IMRT)
				{
					Result = "Pass";
					ResultColor = "LimeGreen";
				}

				//No jaw tracking detected
				else if (Result == "")
				{
					Result = "Warning";
					ResultDetails = "Please check that jaw tracking is enabled in the optimization window or leaf motion calculator";
					ResultColor = "Gold";
				}
			}

			// do not use jaw tracking
			else if (_selectedMachineUI == Globals.TreatmentUnits[Globals.MachineDisplayNames.FLTBTB] ||
					 _selectedMachineUI == Globals.TreatmentUnits[Globals.MachineDisplayNames.FLTFTB])
			{
				//VMAT plan
				if ((from s in _context.PlanSetup.Beams where s.MLCPlanType == MLCPlanType.VMAT select s).Count() > 0)
				{
					IMRT = true;

					foreach (Beam field in _context.PlanSetup.Beams)
					{
						//ignore setup fields
						if (!field.IsSetupField)
						{
							//for VMAT fields, check that there are different jaw positions in the control points
							if (field.MLCPlanType == MLCPlanType.VMAT)
							{
								//get initial jaw positions
								double x1 = field.ControlPoints.First().JawPositions.X1;
								double x2 = field.ControlPoints.First().JawPositions.X2;
								double y1 = field.ControlPoints.First().JawPositions.Y1;
								double y2 = field.ControlPoints.First().JawPositions.Y2;

								//if they change at any of the control points, then jaw tracking must be on
								foreach (ControlPoint point in field.ControlPoints)
								{
									if (x1 != point.JawPositions.X1 || x2 != point.JawPositions.X2 || y1 != point.JawPositions.Y1 || y2 != point.JawPositions.Y2)
									{
										Result = "Warning";
										ResultDetails = "Please check that jaw tracking is disabled in the optimization window";
										ResultColor = "Gold";
										break;
									}
								}
							}
						}
					}
				}
				//IMRT plan
				else if ((from s in _context.PlanSetup.Beams where s.MLCPlanType == MLCPlanType.DoseDynamic select s).Count() > 0)
				{
					foreach (Beam field in _context.PlanSetup.Beams)
					{
						//ignore setup fields
						if (!field.IsSetupField)
						{
							//for IMRT fields that have more control points than step and shoot, check that there are different jaw positions in the control points
							if (field.MLCPlanType == MLCPlanType.DoseDynamic && field.ControlPoints.Count > 18)
							{
								IMRT = true; 

								//get initial jaw positions
								double x1 = field.ControlPoints.First().JawPositions.X1;
								double x2 = field.ControlPoints.First().JawPositions.X2;
								double y1 = field.ControlPoints.First().JawPositions.Y1;
								double y2 = field.ControlPoints.First().JawPositions.Y2;

								//if they change at any of the control points, then jaw tracking must be on
								foreach (ControlPoint point in field.ControlPoints)
								{
									if (x1 != point.JawPositions.X1 || x2 != point.JawPositions.X2 || y1 != point.JawPositions.Y1 || y2 != point.JawPositions.Y2)
									{
										Result = "Warning";
										ResultDetails = "Please check that jaw tracking is disabled in the leaf motion calculator";
										ResultColor = "Gold";
										break;
									}
								}
							}
						}
					}
				}

				//Static fields
				if (!IMRT)
				{
					Result = "Pass";
					ResultColor = "LimeGreen";
				}

				//No jaw tracking detected
				else if (Result == "")
				{
					Result = "Pass";
					ResultColor = "LimeGreen";
				}
			}
			else
				ThrowNotImplemented();
		}

		/// <summary>
		/// Checks to see if each field has table coordinates for TrueBeams
		/// </summary>
		public void CheckForCouchValues()
		{
			ResultDetails = "";
			TestExplanation = "Checks that couch values are entered for each field based on department standards";

			//any couch value
			if (_selectedMachineUI == Globals.TreatmentUnits[Globals.MachineDisplayNames.CLAEX] ||
				_selectedMachineUI == Globals.TreatmentUnits[Globals.MachineDisplayNames.MACIX] ||
				_selectedMachineUI == Globals.TreatmentUnits[Globals.MachineDisplayNames.MACTB] ||
				_selectedMachineUI == Globals.TreatmentUnits[Globals.MachineDisplayNames.MPHTB])
			{
				//Check each field to see if couch values are NaN
				foreach (Beam field in _context.PlanSetup.Beams)
				{
					if (field.ControlPoints.First().TableTopLateralPosition.ToString() == "NaN" || field.ControlPoints.First().TableTopLongitudinalPosition.ToString() == "NaN" || field.ControlPoints.First().TableTopVerticalPosition.ToString() == "NaN")
					{
						Result = "Warning";
						ResultDetails += "No couch values for " + field.Id.ToString() + ": ";
						ResultColor = "Gold";

						if (field.ControlPoints.First().TableTopLateralPosition.ToString() == "NaN")
							ResultDetails += "lat, ";
						if (field.ControlPoints.First().TableTopLongitudinalPosition.ToString() == "NaN")
							ResultDetails += "long, ";
						if (field.ControlPoints.First().TableTopVerticalPosition.ToString() == "NaN")
							ResultDetails += "vert, ";

						ResultDetails = ResultDetails.TrimEnd(' ');
						ResultDetails = ResultDetails.TrimEnd(',');
						ResultDetails += '\n';
					}
				}

				ResultDetails = ResultDetails.TrimEnd('\n');

				//no issues found
				if (ResultDetails == "")
				{
					Result = "";
					ResultDetails = $"Lat: {(ConvertCouchLatToVarianIECScale(_context.PlanSetup.Beams.First().ControlPoints.First().TableTopLateralPosition) / 10.0).ToString("0.0")} cm\nLong: {(_context.PlanSetup.Beams.First().ControlPoints.First().TableTopLongitudinalPosition / 10.0).ToString("0.0")} cm\nVert: {(ConvertCouchVertToVarianIECScale(_context.PlanSetup.Beams.First().ControlPoints.First().TableTopVerticalPosition) / 10.0).ToString("0.0")} cm";
					ResultColor = "LimeGreen";
				}
			}
			// vert = 0
			// long = 100
			// lat = 0
			else if (_selectedMachineUI == Globals.TreatmentUnits[Globals.MachineDisplayNames.LAPIX] || 
					 _selectedMachineUI == Globals.TreatmentUnits[Globals.MachineDisplayNames.FLTBTB] ||
				     _selectedMachineUI == Globals.TreatmentUnits[Globals.MachineDisplayNames.FLTFTB] ||
					 _selectedMachineUI == Globals.TreatmentUnits[Globals.MachineDisplayNames.OWOIX])
			{
				//Check each field to see if couch values are NaN
				foreach (Beam field in _context.PlanSetup.Beams)
				{
					if (field.ControlPoints.FirstOrDefault().TableTopLateralPosition != 0 || field.ControlPoints.FirstOrDefault().TableTopLongitudinalPosition != 1000 || field.ControlPoints.FirstOrDefault().TableTopVerticalPosition != 0)
					{
						Result = "Warning";
						ResultDetails += "Couch value incorrect for " + field.Id.ToString() + ": ";
						ResultColor = "Gold";

						if (field.ControlPoints.First().TableTopLateralPosition != 0)
							ResultDetails += "lat, ";
						if (field.ControlPoints.First().TableTopLongitudinalPosition != 1000)
							ResultDetails += "long, ";
						if (field.ControlPoints.First().TableTopVerticalPosition != 0)
							ResultDetails += "vert, ";

						ResultDetails = ResultDetails.TrimEnd(' ');
						ResultDetails = ResultDetails.TrimEnd(',');
						ResultDetails += '\n';
					}
				}

				ResultDetails = ResultDetails.TrimEnd('\n');

				//no issues found
				if (ResultDetails == "")
				{
					Result = "";
					if (_selectedMachineUI == Globals.TreatmentUnits[Globals.MachineDisplayNames.LAPIX] ||
						_selectedMachineUI == Globals.TreatmentUnits[Globals.MachineDisplayNames.OWOIX])
						ResultDetails = $"Lat: {(ConvertCouchLatToVarianStandardScale(_context.PlanSetup.Beams.First().ControlPoints.First().TableTopLateralPosition) / 10.0).ToString("0.0")} cm\nLong: {(_context.PlanSetup.Beams.First().ControlPoints.First().TableTopLongitudinalPosition / 10.0).ToString("0.0")} cm\nVert: {(ConvertCouchVertToVarianStandardScale(_context.PlanSetup.Beams.First().ControlPoints.First().TableTopVerticalPosition) / 10.0).ToString("0.0")} cm";
					else
						ResultDetails = $"Lat: {(ConvertCouchLatToVarianIECScale(_context.PlanSetup.Beams.First().ControlPoints.First().TableTopLateralPosition) / 10.0).ToString("0.0")} cm\nLong: {(_context.PlanSetup.Beams.First().ControlPoints.First().TableTopLongitudinalPosition / 10.0).ToString("0.0")} cm\nVert: {(ConvertCouchVertToVarianIECScale(_context.PlanSetup.Beams.First().ControlPoints.First().TableTopVerticalPosition) / 10.0).ToString("0.0")} cm";
					ResultColor = "LimeGreen";
				}
			}
			// vert <= 50
			// long = 50
			// lat = 0
			else if (_selectedMachineUI == Globals.TreatmentUnits[Globals.MachineDisplayNames.LANIX])
			{
				//Check each field to see if couch values are NaN
				foreach (Beam field in _context.PlanSetup.Beams)
				{
					if (field.ControlPoints.First().TableTopLateralPosition != 0 || field.ControlPoints.First().TableTopLongitudinalPosition != 500 || field.ControlPoints.First().TableTopVerticalPosition < -500)
					{
						Result = "Warning";
						ResultDetails += "Couch value incorrect for " + field.Id.ToString() + ": ";
						ResultColor = "Gold";

						if (field.ControlPoints.First().TableTopLateralPosition != 0)
							ResultDetails += "lat, ";
						if (field.ControlPoints.First().TableTopLongitudinalPosition != 500)
							ResultDetails += "long, ";
						if (field.ControlPoints.First().TableTopVerticalPosition < -500)
							ResultDetails += "vert, ";

						ResultDetails = ResultDetails.TrimEnd(' ');
						ResultDetails = ResultDetails.TrimEnd(',');
						ResultDetails += '\n';
					}
				}

				ResultDetails = ResultDetails.TrimEnd('\n');

				//no issues found
				if (ResultDetails == "")
				{
					ResultDetails = $"Lat: {(ConvertCouchLatToVarianIECScale(_context.PlanSetup.Beams.First().ControlPoints.First().TableTopLateralPosition) / 10.0).ToString("0.0")} cm\nLong: {(_context.PlanSetup.Beams.First().ControlPoints.First().TableTopLongitudinalPosition / 10.0).ToString("0.0")} cm\nVert: {(ConvertCouchVertToVarianIECScale(_context.PlanSetup.Beams.First().ControlPoints.First().TableTopVerticalPosition) / 10.0).ToString("0.0")} cm";
					ResultColor = "LimeGreen";
				}
			}
			else
				ThrowNotImplemented();
		}

		/// <summary>
		/// Checks to see if each field has the same machine
		/// </summary>
		public void CheckMachine()
		{
			Result = "";
			ResultDetails = "";
			TestExplanation = "Checks that all fields are planned using the same machine";

			//Check each field to make sure they're the same
			foreach (Beam field in _context.PlanSetup.Beams)
			{
				if (field.TreatmentUnit.Id != _selectedMachineUI)
				{
					Result = "Fail";
					ResultDetails = $"Patient is being treated on {_selectedMachineUI} but not all fields use that machine";
					ResultColor = "Tomato";
				}
			}

			if (Result == "")
			{
				Result = "";
				ResultDetails = _selectedMachineUI;
				ResultColor = "LimeGreen";
			}
		}

		/// <summary>
		/// Checks to see if each field has the same tolerance table (besides imaging fields)
		/// </summary>
		public void CheckToleranceTables()
		{
			Result = "";
			ResultDetails = "";
			TestExplanation = "Checks that all fields use the correct tolerance table based on department standards";

			if (_selectedMachineUI == Globals.TreatmentUnits[Globals.MachineDisplayNames.CLAEX] ||
				_selectedMachineUI == Globals.TreatmentUnits[Globals.MachineDisplayNames.MACIX] ||
				_selectedMachineUI == Globals.TreatmentUnits[Globals.MachineDisplayNames.MACTB] ||
				_selectedMachineUI == Globals.TreatmentUnits[Globals.MachineDisplayNames.MPHTB])
			{
				//Check each field to make sure they're the same
				foreach (Beam field in _context.PlanSetup.Beams)
				{
					if (field.IsSetupField)
					{
						if (field.ToleranceTableLabel != "OBI TX")
						{
							Result = "Warning";
							ResultDetails = "OBI TX tolerance table not chosen for setup field";
							ResultColor = "Gold";
							break;
						}
					}
					else
					{
						if (ResultDetails == "")
							ResultDetails = field.ToleranceTableLabel;
						else if (ResultDetails != field.ToleranceTableLabel)
						{
							Result = "Warning";
							ResultDetails = "Not all fields have the same tolerance table";
							ResultColor = "Gold";
						}
					}
				}

				//no issues found
				if (Result == "")
				{
					Result = "";
					ResultColor = "LimeGreen";
				}
			}
			else if (_selectedMachineUI == Globals.TreatmentUnits[Globals.MachineDisplayNames.FLTFTB] ||
					 _selectedMachineUI == Globals.TreatmentUnits[Globals.MachineDisplayNames.FLTBTB])
			{
				string tolTable;
				string badFields = "";

				if (_context.PlanSetup.Id.Contains("_5"))
					tolTable = "SRS/SRT";
				else if (_context.PlanSetup.Id.Contains("_4"))
					tolTable = "SBRT";
				else
					tolTable = "TrueBeam";

				//Check each field to make sure they're the same
				foreach (Beam field in _context.PlanSetup.Beams)
				{
					if (field.IsSetupField)
					{
						if (!field.ToleranceTableLabel.Contains("OBI"))
						{
							Result = "Warning";
							ResultDetails = "OBI tolerance table not chosen for setup field";
							ResultColor = "Gold";
							break;
						}
					}
					else
					{
						if (ResultDetails == "")
							ResultDetails = field.ToleranceTableLabel;

						//wrong tolerance table
						if (field.ToleranceTableLabel != tolTable)
						{
							Result = "Warning";
							ResultDetails = $"Not all fields use the {tolTable} tolerance table: ";
							badFields += field.Id + ", ";
							ResultColor = "Gold";
						}
					}
				}

				ResultDetails += badFields;
				ResultDetails = ResultDetails.TrimEnd(' ');
				ResultDetails = ResultDetails.TrimEnd(',');

				//no issues found
				if (Result == "")
				{
					Result = "";
					ResultColor = "LimeGreen";
				}
			}
			else if (_selectedMachineUI == Globals.TreatmentUnits[Globals.MachineDisplayNames.LAPIX] ||
					 _selectedMachineUI == Globals.TreatmentUnits[Globals.MachineDisplayNames.OWOIX])
			{
				string tolTable = "";
				string badFields = "";

				//Check each field to make sure they're the same
				foreach (Beam field in _context.PlanSetup.Beams)
				{
					if (field.IsSetupField)
					{
						if (field.ToleranceTableLabel.Contains("OBI"))
						{
							Result = "Warning";
							ResultDetails = "OBI tolerance table chosen for setup field of 21iX machine\n";
							ResultColor = "Gold";
							break;
						}
					}
					else
					{
						if (tolTable == "")
							tolTable = field.ToleranceTableLabel;

						//wrong tolerance table
						if (field.ToleranceTableLabel != tolTable)
						{
							Result = "Warning";
							ResultDetails = $"Not all fields use the {tolTable} tolerance table: ";
							badFields += field.Id + ", ";
							ResultColor = "Gold";
						}
					}
				}

				ResultDetails += badFields;
				ResultDetails = ResultDetails.TrimEnd(' ');
				ResultDetails = ResultDetails.TrimEnd(',');
				if (ResultDetails != "") ResultDetails += '\n';
				ResultDetails += tolTable;

				//no issues found
				if (Result == "")
				{
					Result = "";
					ResultColor = "LimeGreen";
				}
			}
			else if (_selectedMachineUI == Globals.TreatmentUnits[Globals.MachineDisplayNames.LANIX])
			{
				string tolTable = "";
				string badFields = "";

				if (_context.PlanSetup.Id.Contains("_5"))
					tolTable = "01 SRS";
				else if (_context.PlanSetup.Id.Contains("_4"))
					tolTable = "02 SBRT";

				//Check each field to make sure they're the same
				foreach (Beam field in _context.PlanSetup.Beams)
				{
					if (tolTable == "")
						tolTable = field.ToleranceTableLabel;

					//wrong tolerance table
					if (field.ToleranceTableLabel != tolTable)
					{
						Result = "Warning";
						ResultDetails = $"Not all fields use the {tolTable} tolerance table: ";
						badFields += field.Id + ", ";
						ResultColor = "Gold";
					}
				}

				ResultDetails += badFields;
				ResultDetails = ResultDetails.TrimEnd(' ');
				ResultDetails = ResultDetails.TrimEnd(',');
				if (ResultDetails == "")
					ResultDetails = tolTable;

				//no issues found
				if (Result == "")
				{
					Result = "";
					ResultColor = "LimeGreen";
				}
			}
			else
				ThrowNotImplemented();
		}

		/// <summary>
		/// Checks to see if each field has a linked bolus if one exists
		/// </summary>
		public void CheckLinkedBolus()
		{
			Result = "";
			ResultDetails = "";
			TestExplanation = "Checks that each field has a linked bolus if a bolus exists";

			string bolus = "";
			bool containsBolus = false;
			bool containsMultiple = false;
			string resultDetailsMultiPerFieldLine = "";
			string resultDetailsMultiPerPlanLine = "";

			//Check to see if plan contains a bolus
			foreach (Structure struc in _context.PlanSetup.StructureSet.Structures)
			{
				if (struc.DicomType == "BOLUS")
				{
					//if it's already found one bolus, then there are multiple
					if (containsBolus)
						containsMultiple = true;

					containsBolus = true;
				}
			}

			//Check each field to make sure it has a bolus attached
			foreach (Beam field in _context.PlanSetup.Beams)
			{
				if (!containsBolus)
					break;
				if (!field.IsSetupField)
				{
					//no bolus
					if (field.Boluses.Count() == 0)
					{
						//Set up "no bolus linked" string
						if (ResultDetails == "")
						{
							Result = "Warning";
							ResultDetails = "Some fields do not have a linked bolus: ";
							ResultColor = "Gold";
						}
						ResultDetails += field.Id + ", ";
					}
					//more than 1 bolus
					else if (field.Boluses.Count() > 1)
					{
						//set up "multiple boluses" string
						if (resultDetailsMultiPerFieldLine == "")
						{
							Result = "Warning";
							resultDetailsMultiPerFieldLine = "Some fields have more than one bolus linked: ";
							ResultColor = "Gold";
						}
						resultDetailsMultiPerFieldLine += field.Id + ", ";
					}
					//just one bolus
					else
					{
						//if this is the first bolus found, save it
						if (bolus == "")
						{
							bolus = field.Boluses.First().Id;
						}
						//if not make sure it's the same bolus used on other fields
						else if (field.Boluses.First().Id != bolus)
						{
							//set up "multiple boluses" string
							if (resultDetailsMultiPerPlanLine == "")
							{
								Result = "Warning";
								resultDetailsMultiPerPlanLine = $"Multiple bolus structures linked in plan: {bolus}, ";
								ResultColor = "Gold";
							}
							resultDetailsMultiPerPlanLine += field.Boluses.First().Id + ", ";
						}
					}
				}
			}

			//no bolus in plan so it's good
			if (!containsBolus)
			{
				Result = "";
				ResultDetails = "No bolus in structure set";
				ResultColor = "LimeGreen";
			}
			//no issues found
			else if (Result == "")
			{
				Result = "";
				ResultDetails = $"{bolus} attached to all fields";
				ResultColor = "LimeGreen";
			}

			//clean up strings
			ResultDetails = ResultDetails.TrimEnd(' ');
			ResultDetails = ResultDetails.TrimEnd(',');
			resultDetailsMultiPerFieldLine = resultDetailsMultiPerFieldLine.TrimEnd(' ');
			resultDetailsMultiPerFieldLine = resultDetailsMultiPerFieldLine.TrimEnd(',');
			resultDetailsMultiPerPlanLine = resultDetailsMultiPerPlanLine.TrimEnd(' ');
			resultDetailsMultiPerPlanLine = resultDetailsMultiPerPlanLine.TrimEnd(',');
			if (resultDetailsMultiPerFieldLine != "")
				ResultDetails += '\n' + resultDetailsMultiPerFieldLine;
			if (resultDetailsMultiPerPlanLine != "")
				ResultDetails += '\n' + resultDetailsMultiPerPlanLine;
			ResultDetails = ResultDetails.TrimStart('\n');
			ResultDetails = ResultDetails.TrimStart('\n');

			//multiple boluses in structure set, so put a warning at the end
			if (containsMultiple)
			{
				Result = "Warning";
				ResultDetails += "\nMultiple bolus structures in the structure set, please ensure that the correct one is used";
				ResultColor = "Gold";
			}
		}

		/// <summary>
		/// Checks to see if couch structures are appropriate (will have '!' as the first character if it should be flagged as a warning)
		/// </summary>
		public void CheckCouchStructures()
		{
			TestExplanation = "Checks that the correct couch structure based on department standards";

			IEnumerable<Structure> couchStructures = null;
			string couchName = "";
			bool couchStructure;
			bool IMRT = false;

			//find couch structure if it exists
			if ((from s in _context.PlanSetup.StructureSet.Structures where s.DicomType == "SUPPORT" select s).Count() > 0)
			{
				couchStructures = (from s in _context.PlanSetup.StructureSet.Structures where s.DicomType == "SUPPORT" select s);
				couchStructure = true;
				couchName = couchStructures.FirstOrDefault().Name;
			}
			else
				couchStructure = false;

			//Port Huron
			if (_selectedMachineUI == Globals.TreatmentUnits[Globals.MachineDisplayNames.MPHTB])
			{
				//if plan likely an SRS (with 1 mm slices) it should not include a couch
				if (_context.PlanSetup.StructureSet.Image.ZRes == 1)
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
					foreach (Beam field in _context.PlanSetup.Beams)
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

			//Lapeer/Owosso
			else if (_selectedMachineUI == Globals.TreatmentUnits[Globals.MachineDisplayNames.LAPIX] ||
					 _selectedMachineUI == Globals.TreatmentUnits[Globals.MachineDisplayNames.OWOIX])
			{
				//check to see if VMAT/IMRT
				foreach (Beam field in _context.PlanSetup.Beams)
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

			//Flint
			else if (_selectedMachineUI == Globals.TreatmentUnits[Globals.MachineDisplayNames.FLTFTB] ||
				     _selectedMachineUI == Globals.TreatmentUnits[Globals.MachineDisplayNames.FLTBTB])
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

			else
				ThrowNotImplemented();
		}

		/// <summary>
		/// Displays calculation parameters
		/// </summary>
		public void CheckCalcParameters()
		{
			Result = "";
			ResultDetails = "";
			ResultColor = "LimeGreen";
			TestExplanation = "Displays calculation parameters";

			Boolean photon = false;
			Boolean electron = false;
			Boolean IMRT = false;
			Boolean VMAT = false;

			//get calc options
			Dictionary<string, string> photonOptions = _context.PlanSetup.PhotonCalculationOptions;
			Dictionary<string, string> electronOptions = _context.PlanSetup.ElectronCalculationOptions;

			//loop through beams to see what needs to be displayed
			foreach (Beam beam in _context.PlanSetup.Beams)
			{
				if (beam.EnergyModeDisplayName.Contains('E'))
					electron = true;
				else if (beam.EnergyModeDisplayName.Contains('X'))
					photon = true;

				if (beam.MLCPlanType == MLCPlanType.DoseDynamic && beam.ControlPoints.Count > 18)
					IMRT = true;
				else if (beam.MLCPlanType == MLCPlanType.VMAT)
					VMAT = true;
			}

			if (electron)
			{
				ResultDetails += "Volume Dose: " + _context.PlanSetup.ElectronCalculationModel.ToString();
				ResultDetails += "\nGrid Size: " + electronOptions["CalculationGridSizeInCM"];
				ResultDetails += "\nUncertainty: " + electronOptions["StatisticalUncertainty"];
				ResultDetails += "\nSmooting Method: " + electronOptions["SmoothingMethod"];
				ResultDetails += "\nSmoothing Level: " + electronOptions["SmoothingLevel"];
			}
			if (photon)
			{
				ResultDetails += "Volume Dose: " + _context.PlanSetup.PhotonCalculationModel.ToString();

				if (IMRT)
				{
					ResultDetails += "\nIMRT Optimization: " + _context.PlanSetup.GetCalculationModel(CalculationType.PhotonIMRTOptimization);
					ResultDetails += "\nLeaf Motion: " + _context.PlanSetup.GetCalculationModel(CalculationType.PhotonLeafMotions);
				}
				if (VMAT)
					ResultDetails += "\nVMAT Optimization: " + _context.PlanSetup.GetCalculationModel(CalculationType.PhotonVMATOptimization);

				ResultDetails += "\nGrid Size: " + photonOptions["CalculationGridSizeInCM"] + "cm";
				ResultDetails += "\nHeterogeneity Corrections: " + photonOptions["HeterogeneityCorrection"];
			}
		}

		/// <summary>
		/// Displays plan approval information
		/// </summary>
		public void PlanApproval()
		{
			PlanSetupApprovalStatus approval = _context.PlanSetup.ApprovalStatus;

			Result = "";
			ResultDetails = $"Status: {AddSpacesToSentence(approval.ToString())}";
			ResultColor = "LimeGreen";
			TestExplanation = "Displays plan approval\nReviewed timestamp is estimated based on CT image approval";

            // Build out list of physicians who can approve plans in Flint/Lapeer/Owosso
            List<string> FLORadOncs = new List<string>();
            FLORadOncs.InsertRange(FLORadOncs.Count, new string[] { "heshamg", "jackn", "kirand", "ogayar" });

            // Switch colour to gold for F/L/O Machines, will turn green if good
            if (_selectedMachineUI==Globals.TreatmentUnits[Globals.MachineDisplayNames.FLTBTB] ||
                _selectedMachineUI == Globals.TreatmentUnits[Globals.MachineDisplayNames.FLTFTB] ||
                _selectedMachineUI == Globals.TreatmentUnits[Globals.MachineDisplayNames.LAPIX] ||
                _selectedMachineUI == Globals.TreatmentUnits[Globals.MachineDisplayNames.OWOIX] 
                )
            {
                ResultColor = "Gold";
            }

			if (approval == PlanSetupApprovalStatus.TreatmentApproved)
			{
				ResultDetails += $"\nReviewed by: {_context.PlanSetup.StructureSet.Image.HistoryUserName} at {_context.PlanSetup.StructureSet.Image.HistoryDateTime.ToString("dddd, MMMM d, yyyy H:mm:ss tt")}";
				ResultDetails += $"\nPlanning Approved by: {_context.PlanSetup.PlanningApprover} at {_context.PlanSetup.PlanningApprovalDate}";
				ResultDetails += $"\nTreatment Approved by {_context.PlanSetup.TreatmentApprover} at {_context.PlanSetup.TreatmentApprovalDate}";
                // If it's a F/L/O Machine verify the physician signature is good
                if (_selectedMachineUI == Globals.TreatmentUnits[Globals.MachineDisplayNames.FLTBTB] ||
                _selectedMachineUI == Globals.TreatmentUnits[Globals.MachineDisplayNames.FLTFTB] ||
                _selectedMachineUI == Globals.TreatmentUnits[Globals.MachineDisplayNames.LAPIX] ||
                _selectedMachineUI == Globals.TreatmentUnits[Globals.MachineDisplayNames.OWOIX]
                )
                {
                    if (FLORadOncs.Contains(_context.PlanSetup.StructureSet.Image.HistoryUserName))
                    {
                        ResultColor = "LimeGreen";
                        Result = "Good";
                    }
                    else
                    {
                        Result = "Verify that there is a physician approval on the plan";
                    }
                    
                }
            }
			else if (approval == PlanSetupApprovalStatus.PlanningApproved)
			{
				ResultDetails += $"\nReviewed by: {_context.PlanSetup.StructureSet.Image.HistoryUserName} at {_context.PlanSetup.StructureSet.Image.HistoryDateTime.ToString("dddd, MMMM d, yyyy H:mm:ss tt")}";
				ResultDetails += $"\nPlanning Approved by: {_context.PlanSetup.PlanningApprover} at {_context.PlanSetup.PlanningApprovalDate}";
                // If it's a F/L/O Machine verify the physician signature is good
                if (_selectedMachineUI == Globals.TreatmentUnits[Globals.MachineDisplayNames.FLTBTB] ||
                _selectedMachineUI == Globals.TreatmentUnits[Globals.MachineDisplayNames.FLTFTB] ||
                _selectedMachineUI == Globals.TreatmentUnits[Globals.MachineDisplayNames.LAPIX] ||
                _selectedMachineUI == Globals.TreatmentUnits[Globals.MachineDisplayNames.OWOIX]
                )
                {
                    if (FLORadOncs.Contains(_context.PlanSetup.StructureSet.Image.HistoryUserName))
                    {
                        ResultColor = "LimeGreen";
                        Result = "Good";
                    }
                    else
                    {
                        Result = "Verify that there is a physician approval on the plan";
                    }

                }

            }
            else if (approval == PlanSetupApprovalStatus.Reviewed)
			{
				ResultDetails += $"\nReviewed by: {_context.PlanSetup.StructureSet.Image.HistoryUserName} at {_context.PlanSetup.StructureSet.Image.HistoryDateTime.ToString("dddd, MMMM d, yyyy H:mm:ss tt")}";
			}
		}

		/// <summary>
		/// Checks that DRRs are created and attached to fields as a reference image
		/// </summary>
		public void CheckDRRs()
		{
			Result = "Pass";
			ResultDetails = "";
			ResultColor = "LimeGreen";
			TestExplanation = "Checks that DRRs are created and attached as a reference for all fields";

			foreach (Beam field in _context.PlanSetup.Beams)
			{
				if (field.ReferenceImage == null)
				{
					Result = "Warning";
					ResultDetails += field.Id + " has no reference image\n";
					ResultColor = "Gold";
				}
			}

			ResultDetails = ResultDetails.TrimEnd('\n');
		}

		/// <summary>
		/// Displays shifts
		/// </summary>
		public void CalcShifts()
		{
			Result = "";
			ResultDetails = "";
			ResultColor = "LimeGreen";
			TestExplanation = "Displays shifts from Marker Structure or User Origin";

			PatientOrientation orientation = _context.PlanSetup.TreatmentOrientation;

			// get location of user origin and plan isocenter
			VVector tattoos = _context.PlanSetup.StructureSet.Image.UserOrigin;
			VVector isocenter = _context.PlanSetup.Beams.First().IsocenterPosition;

			// calculated shift distance from user origin
			VVector shift = isocenter - tattoos;
			string shiftFrom = "User Origin";

			// these sites set iso at sim and import in a "MARKER" structure that shifts will be based off (also they don't use gold markers, so there's no need to worry about those "MARKER" structures)
			if (_selectedMachineUI == Globals.TreatmentUnits[Globals.MachineDisplayNames.FLTFTB] ||
				_selectedMachineUI == Globals.TreatmentUnits[Globals.MachineDisplayNames.FLTBTB] ||
				_selectedMachineUI == Globals.TreatmentUnits[Globals.MachineDisplayNames.LAPIX] ||
				_selectedMachineUI == Globals.TreatmentUnits[Globals.MachineDisplayNames.MPHTB] ||
				_selectedMachineUI == Globals.TreatmentUnits[Globals.MachineDisplayNames.OWOIX])
			{
				// loop through each patient marker and see if it's closer to the iso than the user origin and if it is use that for the calculated shift
				foreach (Structure point in _context.StructureSet.Structures.Where(x => x.DicomType == "MARKER"))
				{
					if (Math.Round((isocenter - point.CenterPoint).Length,2) <= Math.Round(shift.Length,2))
					{
						shift = isocenter - point.CenterPoint;
						shiftFrom = point.Id;
					}
				}
			}

			//round it off to prevent very small numbers from appearing and convert to cm for shifts
			shift.x = Math.Round(shift.x / 10, 1);
			shift.y = Math.Round(shift.y / 10, 1);
			shift.z = Math.Round(shift.z / 10, 1);

			if (shift.Length == 0)
				ResultDetails = $"No shifts from {shiftFrom}";
			else
			{
				//x-axis
				if (shift.x > 0)
					ResultDetails += $"Patient left: {shift.x.ToString("0.0")} cm\n";
				else if (shift.x < 0)
					ResultDetails += $"Patient right: {(-shift.x).ToString("0.0")} cm\n";
				
				//z-axis
				if (shift.z > 0)
					ResultDetails += $"Patient superior: {shift.z.ToString("0.0")} cm\n";
				else if (shift.z < 0)
					ResultDetails += $"Patient inferior: {(-shift.z).ToString("0.0")} cm\n";
				
				//y-axis
				if (shift.y > 0)
					ResultDetails += $"Patient posterior: {shift.y.ToString("0.0")} cm\n";
				else if (shift.y < 0)
					ResultDetails += $"Patient anterior: {(-shift.y).ToString("0.0")} cm\n";
				

				//remove negatives
				ResultDetails.Replace("-", string.Empty);

				ResultDetails = $"Shifts from {shiftFrom}\n" + ResultDetails;
			}

			ResultDetails = ResultDetails.TrimEnd('\n');
		}

		/// <summary>
		/// Checks treatment orientation against scanned orientation
		/// </summary>
		public void CheckOrientation()
		{
			Result = "";
			ResultDetails = AddSpacesToSentence(_context.PlanSetup.TreatmentOrientation.ToString());
			ResultColor = "LimeGreen";
			TestExplanation = "Checks the planned patient orientation against the orientation selected from CT sim";

			if (_context.PlanSetup.StructureSet.Image.ImagingOrientation != _context.PlanSetup.TreatmentOrientation)
			{
				Result = "Warning";
				ResultDetails = $"Scanning orientation of \"{AddSpacesToSentence(_context.PlanSetup.StructureSet.Image.ImagingOrientation.ToString())}\" does not match treatment orientation of \"{AddSpacesToSentence(_context.PlanSetup.TreatmentOrientation.ToString())}\"\nAny calculated directions may be backwards";
				ResultColor = "Gold";
			}
		}

		/// <summary>
		/// Checks plan prescription against Aria prescription (eventually)
		/// </summary>
		public void CheckPrescription()
		{
			//v15 Upgrade
			//will turn into:
			//planSetup.PlannedDosePerFraction;
			//planSetup.NumberOfFractions;
			//planSetup.DosePerFraction;


			PlanSetup plan = _context.PlanSetup;

			Result = "";
			ResultDetails = $"{plan.DosePerFraction.ToString()} x {plan.NumberOfFractions} Fx = {plan.TotalDose.ToString()}\nPrescribed Percentage: {(plan.TreatmentPercentage*100.0).ToString("0.0")}%\nPlan Normalization: {plan.PlanNormalizationValue.ToString("0.0")}%";
			ResultColor = "LimeGreen";
			TestExplanation = "Displays prescription information from plan in Eclipse";
		}

		public void CheckHotspot()
		{
			PlanSetup plan = _context.PlanSetup;
			Structure target = plan.StructureSet.Structures.First(s => s.Id == plan.TargetVolumeID);

			if (plan.IsDoseValid)
			{
				bool inTarget = target.IsPointInsideSegment(plan.Dose.DoseMax3DLocation);

				Result = "";
				ResultDetails = inTarget ? $"Hotspot is in {target.Id}" : $"Hotspot is not in {target.Id}";
				ResultColor = inTarget ? "LimeGreen" : "Gold";
			}
			else
			{
				Result = "";
				ResultDetails = "Dose has not been calculated";
				ResultColor = "Gold";
			}

			TestExplanation = "Checks to see if the hotspot is inside of the plan target";
		}

		public void CheckCollisions()
		{

		}

		/// <summary>
		/// Converts a given gantry or couch angle to Varian Standard scale
		/// </summary>
		private double ConvertToVarianStandardScale(double angle)
		{
			if (angle <= 180)
				return 180 - angle;
			else
				return 540 - angle;
		}

		/// <summary>
		/// Converts a given couch angle to Varian IEC scale
		/// </summary>
		private double ConvertCouchAngleToVarianIECScale(double couch)
		{
			return 360 - couch;
		}

		/// <summary>
		/// Converts a given couch lateral to Varian IEC scale
		/// <paramref name="couch"/>Couch lateral in mm
		/// </summary>
		private double ConvertCouchLatToVarianIECScale(double couch)
		{
			return (couch + 10000) % 10000;
		}

		/// <summary>
		/// Converts a given couch vertical to Varian IEC scale
		/// <paramref name="couch"/>Couch vertical in mm
		/// </summary>
		private double ConvertCouchVertToVarianIECScale(double couch)
		{
			return (10000 - couch) % 10000;
		}

		/// <summary>
		/// Converts a given couch lateral to Varian Standard scale
		/// <paramref name="couch"/>Couch lateral in mm
		/// </summary>
		private double ConvertCouchLatToVarianStandardScale(double couch)
		{
			return couch + 1000;
		}

		/// <summary>
		/// Converts a given couch vertical to Varian Standard scale
		/// <paramref name="couch"/>Couch vertical in mm
		/// </summary>
		private double ConvertCouchVertToVarianStandardScale(double couch)
		{
			return 1000 - couch;
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

		/// <summary>
		/// Test criteria have not been specified by a site for implementation
		/// </summary>
		private void ThrowNotImplemented()
		{
			Result = $"Test \"{Test}\" has not been implemented yet for {Globals.TreatmentUnits.Where(x => x.Value == _selectedMachineUI).Select(x => x.Key).First()}";
			ResultColor = "Tomato";
			TestExplanation += "\n\nCriteria for test need to be specified";
			//MessageBox.Show($"Test \"{Test}\" has not been implemented yet for {Globals.TreatmentUnits.Where(x => x.Value == _selectedMachine).Select(x => x.Key).First()}", "Not Implemented", MessageBoxButton.OK, MessageBoxImage.Error);
			//throw new NotImplementedException($"Test \"{Test}\" has not been implemented yet for {Globals.TreatmentUnits.Where(x => x.Value == _selectedMachine).Select(x => x.Key).First()}");
		}

		private void TestCouldNotComplete(string message)
		{
			ESAPILog.Entry(_context, "PlanCheck", message);

			Result = "Failure - Test could not be run";
			ResultDetails = message;
			ResultColor = "Tomato";
		}

		private static string AddSpacesToSentence(string text, bool preserveAcronyms = true)
		{
			if (string.IsNullOrWhiteSpace(text))
				return string.Empty;
			StringBuilder newText = new StringBuilder(text.Length * 2);
			newText.Append(text[0]);
			for (int i = 1; i < text.Length; i++)
			{
				if (char.IsUpper(text[i]))
					if ((text[i - 1] != ' ' && !char.IsUpper(text[i - 1])) ||
						(preserveAcronyms && char.IsUpper(text[i - 1]) &&
						 i < text.Length - 1 && !char.IsUpper(text[i + 1])))
						newText.Append(' ');
				newText.Append(text[i]);
			}
			return newText.ToString();
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged(string name)
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null)
			{
				handler(this, new PropertyChangedEventArgs(name));
			}
		}
	}
}
