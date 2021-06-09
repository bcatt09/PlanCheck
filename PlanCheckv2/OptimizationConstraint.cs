using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using VMS.TPS.Common.Model.API;
using VMS.TPS.Common.Model.Types;

namespace PlanCheck.Checks
{
	class OptimizationConstraint : INotifyPropertyChanged
	{
		private Type _constraintType;
		private string _constraintTypeText;
		private string _structureId;
		private double _priority;
		private OptimizationObjectiveOperator _operator;
		private string _operatorText;
		private DoseValue _dose;
		private double _volume;
		private double _parameterA;
		private double _cost;
		private double _costPercentage;

		public Type ConstraintType { get { return _constraintType; } set { _constraintType = value; OnPropertyChanged("ConstraintType"); } }
		public string ConstraintTypeText { get { return _constraintTypeText; } set { _constraintTypeText = value; OnPropertyChanged("ConstraintTypeText"); } }
		public string StructureId { get { return _structureId; } set { _structureId = value; OnPropertyChanged("StructureId"); } }
		public double Priority { get { return _priority; } set { _priority = value; OnPropertyChanged("Priority"); } }
		public OptimizationObjectiveOperator Operator { get { return _operator; } set { _operator = value; OnPropertyChanged("Operator"); } }
		public string OperatorText { get { return _operatorText; } set { _operatorText = value; OnPropertyChanged("OperatorText"); } }
		public DoseValue Dose { get { return _dose; } set { _dose = value; OnPropertyChanged("Dose"); } }
		public double Volume { get { return _volume; } set { _volume = value; OnPropertyChanged("Volume"); } }
		public double ParameterA { get { return _parameterA; } set { _parameterA = value; OnPropertyChanged("ParameterA"); } }
		public double Cost { get { return _cost; } set { _cost = value; OnPropertyChanged("Cost"); } }
		public double CostPercentage { get { return _costPercentage; } set { _costPercentage = value; OnPropertyChanged("CostPercentage"); } }

		public OptimizationConstraint(OptimizationObjective constraint, PlanSetup plan)
		{
			_structureId = constraint.StructureId;

			if (constraint.GetType() == typeof(OptimizationPointObjective))
			{
				OptimizationPointObjective temp = (OptimizationPointObjective)constraint;
				ConstraintType = typeof(OptimizationPointObjective);
				ConstraintTypeText = "Point";
				Priority = temp.Priority;
				Operator = temp.Operator;
				if (Operator == OptimizationObjectiveOperator.Lower)
					OperatorText = ">";
				else if (Operator == OptimizationObjectiveOperator.Upper)
					OperatorText = "<";
				else if (Operator == OptimizationObjectiveOperator.Exact)
					OperatorText = "=";
				Dose = temp.Dose;
				Volume = temp.Volume;

				//calculate cost using weight = Priority^3  (Varian states that the function is proportional to Priority^n where n>2 so this should be close enough for approximating cost % since it is all relative to each other
				Cost = Math.Pow(Priority, 3) * Math.Pow(plan.GetDoseAtVolume(temp.Structure, Volume, VolumePresentation.Relative, DoseValuePresentation.Absolute).Dose - Dose.Dose, 2);
			}
			else if (constraint.GetType() == typeof(OptimizationMeanDoseObjective))
			{
				OptimizationMeanDoseObjective temp = (OptimizationMeanDoseObjective)constraint;
				ConstraintType = typeof(OptimizationMeanDoseObjective);
				ConstraintTypeText = "Mean Dose";
				Priority = temp.Priority;
				Dose = temp.Dose;

				//calculate cost using weight = Priority^3  (Varian states that the function is proportional to Priority^n where n>2 so this should be close enough for approximating cost % since it is all relative to each other
				Cost = Math.Pow(Priority, 3) * Math.Pow(plan.GetDVHCumulativeData(temp.Structure, DoseValuePresentation.Absolute, VolumePresentation.Relative, 0.01).MeanDose.Dose - Dose.Dose, 2);
			}
			else if (constraint.GetType() == typeof(OptimizationEUDObjective))
			{
				OptimizationEUDObjective temp = (OptimizationEUDObjective)constraint;
				ConstraintType = typeof(OptimizationEUDObjective);
				ConstraintTypeText = "gEUD";
				Priority = temp.Priority;
				Operator = temp.Operator;
				if (Operator == OptimizationObjectiveOperator.Lower)
					OperatorText = ">";
				else if (Operator == OptimizationObjectiveOperator.Upper)
					OperatorText = "<";
				else if (Operator == OptimizationObjectiveOperator.Exact)
					OperatorText = "=";
				Dose = temp.Dose;
				ParameterA = temp.ParameterA;

				//calculate gEUD using param a
				double gEUDSum = 0;
				for (double i = 0; i <= 100; i++)
					gEUDSum += Math.Pow(plan.GetDoseAtVolume(temp.Structure, i, VolumePresentation.Relative, DoseValuePresentation.Absolute).Dose, ParameterA);
				double gEUD = Math.Pow(1.0 / 100 * gEUDSum, 1 / ParameterA);

				//calculate cost using weight = Priority^3  (Varian states that the function is proportional to Priority^n where n>2 so this should be close enough for approximating cost % since it is all relative to each other
				Cost = Math.Pow(Priority, 3) * Math.Pow(gEUD - Dose.Dose, 2);
			}
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
