using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using VMS.TPS.Common.Model.API;
using VMS.TPS.Common.Model.Types;

namespace PlanCheck
{
	class ReferencePointTableEntry : INotifyPropertyChanged
	{
		private Beam _beam;
		private FieldReferencePoint _point;
		private string _field;
		private double _depth;
		private double _effectiveDepth;

		public string Field { get { return _field; } set { _field = value; OnPropertyChanged("Field"); } }
		public double Depth { get { return _depth; } set { _depth = value; OnPropertyChanged("Depth"); } }
		public double EffectiveDepth { get { return _effectiveDepth; } set { _effectiveDepth = value; OnPropertyChanged("EffectiveDepth"); } }

		private static readonly Logger logger = NLog.LogManager.GetCurrentClassLogger();

		public ReferencePointTableEntry(Beam beam, FieldReferencePoint point, ScriptContext context)
		{
			_beam = beam;
			_point = point;

			try
			{
				CalcDepths();
			}
			catch(Exception e)
			{
				logger.Error($"Could not get reference points - {e.Message}\n\t\t\t{e.StackTrace}");


			}
		}

		private void CalcDepths()
		{
			Field = _beam.Id;

			if (_beam.MLCPlanType == MLCPlanType.VMAT || _beam.MLCPlanType == MLCPlanType.ArcDynamic)
			{
				double totalDistFromTarget = 0;
				foreach(ControlPoint controlPoint in _beam.ControlPoints)
				{
					//vector from source to reference point for calculating physical depth of point
					VVector ToRefPoint = _point.RefPointLocation - _beam.GetSourceLocation(controlPoint.GantryAngle);

					totalDistFromTarget += ToRefPoint.Length;
				}

				Depth = Math.Round((totalDistFromTarget / _beam.ControlPoints.Count() - _point.SSD) / 10.0, 1);
			}
			else
			{
				//vector from source to reference point for calculating physical depth of point
				VVector ToRefPoint = _point.RefPointLocation - _beam.GetSourceLocation(_beam.ControlPoints.First().GantryAngle);

				Depth = Math.Round((ToRefPoint.Length - _point.SSD) / 10.0, 1);
			}

			EffectiveDepth = Math.Round(_point.EffectiveDepth / 10.0, 1);
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
