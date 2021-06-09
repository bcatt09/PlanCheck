using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VMS.TPS.Common.Model.API;

namespace PlanCheck
{
    public class MROQCStructureCheck : INotifyPropertyChanged
    {
        private string _structure;
        private string _result;
        private string _resultColor;		//color of the result

        public string Structure { get { return _structure; } set { _structure = value; OnPropertyChanged("Structure"); } }
        public string Result { get { return _result; } set { _result = value; OnPropertyChanged("Result"); } }
        public string ResultColor { get { return _resultColor; } set { _resultColor = value; OnPropertyChanged("ResultColor"); } }

        public MROQCStructureCheck(string structureName, ScriptContext context)
        {
            Structure = structureName;

            StructureSet ss = context.StructureSet;

            if (ss.Structures.Where(x => x.Id == Structure).Count() > 0)
            {
                Structure mroqcStructure = ss.Structures.Where(x => x.Id == Structure).First();

                if (!mroqcStructure.IsEmpty && mroqcStructure.HasSegment)
                {
                    Result = "Pass";
                    ResultColor = "LightGreen";
                }
                else
                {
                    Result = $"{Structure} is not contoured";
                    ResultColor = "Salmon";
                }
            }
            else
            {
                Result = $"{Structure} not found in structure set";
                ResultColor = "Salmon";
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
