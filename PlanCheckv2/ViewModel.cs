using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.TPS.Common.Model.Types;
using VMS.TPS.Common.Model.API;
using System.Windows;
using System.Collections.ObjectModel;

namespace VMS.TPS
{
	class ViewModel : INotifyPropertyChanged
	{
		private ScriptContext _context;												//ScriptContext from Aria
		private Dictionary<string, string> _treatmentUnits;							//list of treatment units for the dropdown
		private ObservableCollection<PlanCheck> _planChecks;						//list of plan checks and results to be displayed
		private List<Tuple<ReferencePoint, PlanSetup, string>> _referencePoints;	//list of reference points for the dropdown
		private ObservableCollection<ReferencePointTableEntry> _referencePointTable;//list of reference point info to be displayed
		private string _patientName;												//patient name
		private string _planID;														//plan id
		private string _courseID;													//course id
		private string _selectedTreatmentUnit;										//treatment unit selected from dropdown (will select which tests are run and settings in each test)
		private string _selectionWarning;                                           //warning if treatment unit is changed in the dropdown
		private Tuple<ReferencePoint, PlanSetup, string> _selecetedReferencePoint;	//reference point selected in dropdown
		private Visibility _referencePointVisibility;                               //should the reference point info be displayed
		private List<OptimizationConstraint> _optiConstraints;

		public Dictionary<string, string> TreatmentUnits { get { return _treatmentUnits; } set { _treatmentUnits = value; OnPropertyChanged("TreatmentUnits"); } }  //just for binding in the xaml
		public ObservableCollection<PlanCheck> PlanChecks { get { return _planChecks; } set { _planChecks = value; OnPropertyChanged("PlanChecks"); } }
		public List<Tuple<ReferencePoint, PlanSetup, string>> ReferencePoints { get { return _referencePoints; } set { _referencePoints = value; OnPropertyChanged("ReferencePoints"); } }
		public ObservableCollection<ReferencePointTableEntry> ReferencePointTable { get { return _referencePointTable; } set { _referencePointTable = value; OnPropertyChanged("ReferencePointTable"); } }
		public string PatientName { get { return _patientName; } set { _patientName = value; OnPropertyChanged("PatientName"); } }
		public string PlanID { get { return _planID; } set { _planID = value; OnPropertyChanged("PlanID"); } }
		public string CourseID { get { return _courseID; } set { _courseID = value; OnPropertyChanged("CourseID"); } }
		public string SelectedTreatmentUnit { get { return _selectedTreatmentUnit; } set { _selectedTreatmentUnit = value; SelectedTreatmentUnitChanged(); OnPropertyChanged("SelectedTreatmentUnit"); } }
		public string SelectionWarning { get { return _selectionWarning; } set { _selectionWarning = value; OnPropertyChanged("SelectionWarning"); } }
		public Tuple<ReferencePoint, PlanSetup, string> SelectedReferencePoint { get { return _selecetedReferencePoint; } set { _selecetedReferencePoint = value; SelectedReferencePointChanged(); OnPropertyChanged("SelecetedReferencePoint"); } }
		public Visibility ReferencePointVisibility { get { return _referencePointVisibility; } set { _referencePointVisibility = value; OnPropertyChanged("ReferencePointVisibility"); } }
		public List<OptimizationConstraint> OptiConstraints { get { return _optiConstraints; } set { _optiConstraints = value; OnPropertyChanged("OptiConstraints"); } }

		public ViewModel(ScriptContext context)
		{
			_context = context;

			//create the treatment unit dropdown list
			TreatmentUnits = Globals.TreatmentUnits;

			PlanChecks = new ObservableCollection<PlanCheck>();

			//plan information
			PatientName = context.Patient.Name;
			CourseID = context.Course.Id;
			PlanID = context.PlanSetup.Id;

			//select treatment unit based on first field in plan
			SelectedTreatmentUnit = context.PlanSetup.Beams.First().TreatmentUnit.Id;
			
			//populate reference points dropdown
			PopulateReferencePoints();

			//setup optimization constraints dataset
			//PopulateOptimizationConstraints();
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

		// SelectedTreatmentUnit is changed from the dropdown
		private void SelectedTreatmentUnitChanged()
		{
			//clear current list of checks and rerun
			PlanChecks.Clear();
			RunPlanChecks();

			//log that it has completed running
			ESAPILog.Entry(_context, "PlanCheck", "");
		}
		
		// Run all tests
		private void RunPlanChecks()
		{
			//check that each test should be run for this machine
			foreach (string test in Globals.TestNames.Tests)
			{
				if (!Globals.Exemptions[test].Contains(Globals.TreatmentUnits.Where(x => x.Value == SelectedTreatmentUnit).Select(x => x.Key).First()))
					PlanChecks.Add(new PlanCheck(test, SelectedTreatmentUnit, _context));
			}

			//display a warning if the incorrect machine is chosen from the dropdown
			if (SelectedTreatmentUnit != _context.PlanSetup.Beams.First().TreatmentUnit.Id)
				SelectionWarning = $"Warning: Selected machine ({SelectedTreatmentUnit}) does not match planned machine ({_context.PlanSetup.Beams.First().TreatmentUnit.Id}), some checks may be invalid";
			else
				SelectionWarning = "";
		}

		// Populate reference points in the dropdown list
		private void PopulateReferencePoints()
		{
			ReferencePoints = new List<Tuple<ReferencePoint, PlanSetup, string>>();

			//check each plan
			foreach (PlanSetup plan in _context.PlansInScope)
			{
				if (plan.Beams.Where(x => !x.IsSetupField).Count() > 0)
				{
					//find any reference points which have a location and don't check setup fields if there are any
					if (plan.Beams.Where(x => !x.IsSetupField).First().FieldReferencePoints.Where(x => !Double.IsNaN(x.RefPointLocation.x)).Count() > 0)
					{
						foreach (FieldReferencePoint point in plan.Beams.Where(x => !x.IsSetupField).First().FieldReferencePoints.Where(x => !Double.IsNaN(x.RefPointLocation.x)).ToList())
						{
							//add them to the list
							ReferencePoints.Add(new Tuple<ReferencePoint, PlanSetup, string>(point.ReferencePoint, plan, $"{point.ReferencePoint} (Plan: {plan.Id})"));
						}
					}
				}
			}

			//setup table and pick the default point
			ReferencePointTable = new ObservableCollection<ReferencePointTableEntry>();
			if (ReferencePoints.Count() > 0)
				SelectedReferencePoint = ReferencePoints.First();

			//set visibility
			if (ReferencePoints.Count() > 0)
				ReferencePointVisibility = Visibility.Visible;
			else
				ReferencePointVisibility = Visibility.Collapsed;
		}

		// SelectedReferencePoint changed from dropdown list
		private void SelectedReferencePointChanged()
		{
			//reset the table
			ReferencePointTable.Clear();

			foreach (Beam beam in SelectedReferencePoint.Item2.Beams.Where(x => !x.IsSetupField))
			{
				//select the FieldReferencePoint that is linked to the desired Reference point and create a new ReferencePointTableEntry with it
				ReferencePointTable.Add(new ReferencePointTableEntry(beam, beam.FieldReferencePoints.Where(x => x.ReferencePoint.Id == SelectedReferencePoint.Item1.Id).First(), _context));
			}
		}

		private void PopulateOptimizationConstraints()
		{
			//setup optimization constraints
			OptiConstraints = new List<OptimizationConstraint>();

			foreach (OptimizationObjective objective in _context.PlanSetup.OptimizationSetup.Objectives)
			{
				OptiConstraints.Add(new OptimizationConstraint(objective, _context.PlanSetup));
			}

			//calculate estimated cost percentages excluding any target structures (structures that have a lower constraint)
			//find targets
			var optiTargets = new HashSet<string> (OptiConstraints.Where(x => x.Operator == OptimizationObjectiveOperator.Lower).Select(x => x.StructureId));

			//loop through structures that aren't a target and calc total cost of these structures
			double totalCost = 0;
			foreach (OptimizationConstraint constraint in OptiConstraints.Where(x => !optiTargets.Contains(x.StructureId)))
				totalCost += constraint.Cost;

			//loop through again and calc the percentage
			foreach (OptimizationConstraint constraint in OptiConstraints.Where(x => !optiTargets.Contains(x.StructureId)))
				constraint.CostPercentage = Math.Round(constraint.Cost / totalCost * 100, 1);
		}
	}
}
