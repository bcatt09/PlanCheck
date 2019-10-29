using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using VMS.TPS.Common.Model.API;
using VMS.TPS.Common.Model.Types;

namespace VMS.TPS
{
	class SetupNote : INotifyPropertyChanged
	{
		private ScriptContext _context;
		private Beam _beam;
		private string _setupNoteText;

		public string SetupNoteText { get { return _setupNoteText; } set { _setupNoteText = value; OnPropertyChanged("SetupNoteText"); } }

		public SetupNote(ScriptContext context, Beam beam, FieldReferencePoint point)
		{
			_context = context;
			_beam = beam;
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
