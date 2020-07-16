using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VMS.TPS.Common.Model.API;

namespace VMS.TPS
{
    public abstract class PlanCheck : INotifyPropertyChanged
    {
        /// <summary>
        /// Displayed name of the test that will show in the PlanCheck window
        /// </summary>
        public string DisplayName { get; protected set; }
        /// <summary>
        /// Simple result text (Pass/Warning/Fail) of the test that will show on the first line
        /// Leave as "" to skip
        /// </summary>
        public string Result { get; protected set; }
        /// <summary>
        /// Detailed results
        /// </summary>
        public string ResultDetails { get; protected set; }
        /// <summary>
        /// Color of the result background (must be a color from System.Drawing.Color)
        /// Pass = LimeGreen
        /// Warning = Gold
        /// Fail = Tomato
        /// </summary>
        public string ResultColor { get; protected set; }
        /// <summary>
        /// Explanation of the test that will show in the table when clicked
        /// </summary>
        public string TestExplanation { get; protected set; }

        /// <summary>
        /// List of machines that this test will not run on
        /// Use the Globals list ...... to add machines into this list
        /// </summary>
        protected abstract List<string> MachineExemptions { get; }

        public void RunTestNew(PlanSetup plan, List<string> exemptions)
        {
            if (plan.Beams.Count() < 1)
            { 
                MessageBox.Show("Plan must contain at least one beam", "No Beams", MessageBoxButton.OK, MessageBoxImage.Error);
                throw new Exception("Plan must contain at least one beam");
            }

            else if (!exemptions.Contains(plan.Beams.First().TreatmentUnit.Id))   // Planned machine isn't in the list of test exceptions
                RunTest(plan);
        }

        /// <summary>
        /// Executes test and stores all results
        /// </summary>
        /// <param name="plan"></param>
        protected abstract void RunTest(PlanSetup plan);

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
