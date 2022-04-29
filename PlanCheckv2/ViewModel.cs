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
using PlanCheck.Checks;
using NLog;
using NLog.Fluent;
using System.Reflection;
using System.IO;

namespace PlanCheck
{
    class ViewModel : INotifyPropertyChanged
    {
        private ScriptContext _context;                                             //ScriptContext from Aria
        private ObservableCollection<PlanCheckBase> _planChecks;                        //list of plan checks and results to be displayed
        private List<Tuple<ReferencePoint, PlanSetup, string>> _referencePoints;    //list of reference points for the dropdown
        private ObservableCollection<ReferencePointTableEntry> _referencePointTable;//list of reference point info to be displayed
        private string _patientName;                                                //patient name
        private string _planID;                                                     //plan id
        private string _courseID;                                                   //course id
        private string _selectionWarning;                                           //warning if treatment unit is changed in the dropdown
        private Tuple<ReferencePoint, PlanSetup, string> _selecetedReferencePoint;  //reference point selected in dropdown
        private Visibility _referencePointVisibility;                               //should the reference point info be displayed
        private List<OptimizationConstraint> _optiConstraints;
        private ObservableCollection<MROQCStructureCheck> _mroqcChecks;                        //list of MROQC structure checks and results to be displayed
        private List<Tuple<string, List<string>>> _mroqcTemplates;                  //list of MROQC structure check templates to be displayed in the dropdown
        private Tuple<string, List<string>> _selectedMROQCTemplate;

        public ObservableCollection<PlanCheckBase> PlanChecks { get { return _planChecks; } set { _planChecks = value; OnPropertyChanged("PlanChecks"); } }
        public List<Tuple<ReferencePoint, PlanSetup, string>> ReferencePoints { get { return _referencePoints; } set { _referencePoints = value; OnPropertyChanged("ReferencePoints"); } }
        public ObservableCollection<ReferencePointTableEntry> ReferencePointTable { get { return _referencePointTable; } set { _referencePointTable = value; OnPropertyChanged("ReferencePointTable"); } }
        public string PatientName { get { return _patientName; } set { _patientName = value; OnPropertyChanged("PatientName"); } }
        public string PlanID { get { return _planID; } set { _planID = value; OnPropertyChanged("PlanID"); } }
        public string CourseID { get { return _courseID; } set { _courseID = value; OnPropertyChanged("CourseID"); } }
        public string SelectionWarning { get { return _selectionWarning; } set { _selectionWarning = value; OnPropertyChanged("SelectionWarning"); } }
        public Tuple<ReferencePoint, PlanSetup, string> SelectedReferencePoint { get { return _selecetedReferencePoint; } set { _selecetedReferencePoint = value; SelectedReferencePointChanged(); OnPropertyChanged("SelecetedReferencePoint"); } }
        public Visibility ReferencePointVisibility { get { return _referencePointVisibility; } set { _referencePointVisibility = value; OnPropertyChanged("ReferencePointVisibility"); } }
        public List<OptimizationConstraint> OptiConstraints { get { return _optiConstraints; } set { _optiConstraints = value; OnPropertyChanged("OptiConstraints"); } }
        public ObservableCollection<MROQCStructureCheck> MROQCChecks { get { return _mroqcChecks; } set { _mroqcChecks = value; OnPropertyChanged("MROQCChecks"); } }
        public List<Tuple<string, List<string>>> MROQCTemplates { get { return _mroqcTemplates; } set { _mroqcTemplates = value; OnPropertyChanged("MROQCTemplates"); } }
        public Tuple<string, List<string>> SelectedMROQCTemplate { get { return _selectedMROQCTemplate; } set { _selectedMROQCTemplate = value; SelectedMROQCTemplateChanged(); OnPropertyChanged("SelectedMROQCTemplate"); } }

        private static readonly Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public ViewModel(ScriptContext context)
        {
            _context = context;

            PlanChecks = new ObservableCollection<PlanCheckBase>();
            MROQCChecks = new ObservableCollection<MROQCStructureCheck>();

            //plan information
            PatientName = context.Patient.Name;
            CourseID = context.Course.Id;
            PlanID = context.PlanSetup.Id;

            Log.Initialize(context);

            RunPlanChecks();

            //populate reference points dropdown
            PopulateReferencePoints();

            //setup optimization constraints dataset
            //PopulateOptimizationConstraints();

            logger.Info("");

            LogManager.Shutdown();
        }

        // Run all tests
        private void RunPlanChecks()
        {
            PlanChecks = new ObservableCollection<PlanCheckBase>();

            // Run all plan checks
            PlanChecks.Add(new MachineChecks(_context.PlanSetup));
            PlanChecks.Add(new DoseRateChecks(_context.PlanSetup));
            PlanChecks.Add(new CTSimChecks(_context.PlanSetup));
            PlanChecks.Add(new OrientationChecks(_context.PlanSetup));
            PlanChecks.Add(new PrecriptionChecks(_context.PlanSetup));
            PlanChecks.Add(new TargetChecks(_context.PlanSetup));
            PlanChecks.Add(new ProtonGantryAngleChecks(_context.PlanSetup));
            PlanChecks.Add(new ProtonSpotPositionChecks(_context.PlanSetup));
            PlanChecks.Add(new ProtonIsocenterMarkerChecks(_context.PlanSetup));
            PlanChecks.Add(new ProtonDRRNameEndChecks(_context.PlanSetup));
            PlanChecks.Add(new ProtonFiducialContourChecks(_context.PlanSetup));
            PlanChecks.Add(new ProtonCalculationModelChecks(_context.PlanSetup));
            PlanChecks.Add(new ProtonSnoutChecks(_context.PlanSetup));
            PlanChecks.Add(new PlanUncertaintyCalculationCheck(_context.PlanSetup));
            PlanChecks.Add(new HotspotChecks(_context.PlanSetup));
            PlanChecks.Add(new PlanApprovalChecks(_context.PlanSetup));
            PlanChecks.Add(new IsocenterChecks(_context.PlanSetup));
            PlanChecks.Add(new FieldNameChecks(_context.PlanSetup));
            PlanChecks.Add(new JawTrackingChecks(_context.PlanSetup));
            PlanChecks.Add(new CouchStructuresChecks(_context.PlanSetup));
            PlanChecks.Add(new CouchValueChecks(_context.PlanSetup));
            PlanChecks.Add(new Shifts(_context.PlanSetup));
            PlanChecks.Add(new ToleranceTableChecks(_context.PlanSetup));
            PlanChecks.Add(new BolusChecks(_context.PlanSetup));
            PlanChecks.Add(new DRRChecks(_context.PlanSetup));
            PlanChecks.Add(new UseGatedChecks(_context.PlanSetup));
            PlanChecks.Add(new MLCChecks(_context.PlanSetup));
            PlanChecks.Add(new TreatmentTimeCalculation(_context.PlanSetup));
            PlanChecks.Add(new NamingConventionChecks(_context.PlanSetup));
            PlanChecks.Add(new CalcParametersChecks(_context.PlanSetup));
            PlanChecks.Add(new ProtonFieldNameChecks(_context.PlanSetup));


            // Remove any plan checks that were not run
            foreach (var p in PlanChecks.Where(x => x.MachineExempt).ToList())
                PlanChecks.Remove(p);
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
            var optiTargets = new HashSet<string>(OptiConstraints.Where(x => x.Operator == OptimizationObjectiveOperator.Lower).Select(x => x.StructureId));

            //loop through structures that aren't a target and calc total cost of these structures
            double totalCost = 0;
            foreach (OptimizationConstraint constraint in OptiConstraints.Where(x => !optiTargets.Contains(x.StructureId)))
                totalCost += constraint.Cost;

            //loop through again and calc the percentage
            foreach (OptimizationConstraint constraint in OptiConstraints.Where(x => !optiTargets.Contains(x.StructureId)))
                constraint.CostPercentage = Math.Round(constraint.Cost / totalCost * 100, 1);

        }

        public void CreateMROQCWindow()
        {
            List<Tuple<string, List<string>>> tempTemplates = new List<Tuple<string, List<string>>>();

            tempTemplates.Add(new Tuple<string, List<string>>("Lung", new List<string> { "PTV", "Lungs-GTV", "Esophagus", "Heart", "SpinalCord" }));
            tempTemplates.Add(new Tuple<string, List<string>>("Left Breast", new List<string> { "CTVsb", "PTVsb", "PTV_Breast_L", "Breast_R", "Heart", "Lung_L" }));
            tempTemplates.Add(new Tuple<string, List<string>>("Right Breast", new List<string> { "CTVsb", "PTVsb", "PTV_Breast_R", "Breast_L", "Heart", "Lung_R" }));

            MROQCTemplates = tempTemplates;
            
            MROQCWindow mroqcWindow = new MROQCWindow();
            mroqcWindow.Title = "MROQC Structure Checks";
            mroqcWindow.DataContext = this;
            mroqcWindow.Show();

            mroqcWindow.KeyDown += (o, e) => { if (e.Key == System.Windows.Input.Key.Escape) (o as MROQCWindow).Close(); };
        }

        public void SelectedMROQCTemplateChanged()
        {
            MROQCChecks.Clear();

            foreach (string struc in SelectedMROQCTemplate.Item2)
                MROQCChecks.Add(new MROQCStructureCheck(struc, _context));
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
