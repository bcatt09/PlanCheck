using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using VMS.TPS.Common.Model.API;

namespace VMS.TPS.PlanChecks
{
    /// <summary>
    /// Base class for all new plan checks<br/>
    /// Must add constructor: <br/> <code>public MyNewCheck(PlanSetup plan) : base(plan) { }</code>
    /// </summary>
    public abstract class PlanCheck : INotifyPropertyChanged
    {
        /// <summary>
        /// Displayed name of the test that will show in the PlanCheck window
        /// </summary>
        public string DisplayName { get; protected set; }
        /// <summary>
        /// Simple result text (Pass/Warning/Fail) of the test that will show on the first line<br/>
        /// Leave as "" to skip
        /// </summary>
        public string Result { get; protected set; }
        /// <summary>
        /// Detailed results
        /// </summary>
        public string ResultDetails { get; protected set; }
        /// <summary>
        /// Color of the result background (must be a color from System.Drawing.Color)<br/>
        /// Pass = LimeGreen<br/>
        /// Warning = Gold<br/>
        /// Fail = Tomato
        /// </summary>
        public string ResultColor { get; protected set; }
        /// <summary>
        /// Explanation of the test that will show in the table when clicked
        /// </summary>
        public string TestExplanation { get; protected set; }
        /// <summary>
        /// List of machine IDs that this test will not run for<br/>
        /// Example:
        /// <code>protected override List&lt;string&gt; MachineExemptions => new List&lt;string&gt; { Globals.MachineNames.MPH_TB };</code><br/>
        /// To use on all machines, leave list blank:
        /// <code>protected override List&lt;string&gt; MachineExemptions => new List&lt;string&gt; { };</code>
        /// </summary>
        protected abstract List<string> MachineExemptions { get; }
        /// <summary>
        /// ID of the planned treatment machine
        /// </summary>
        protected string MachineID { get; }
        /// <summary>
        /// Department of the planned treatment machine
        /// </summary>
        protected Department Department { get; }

        protected static readonly Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public PlanCheck(PlanSetup plan)
        {
            if (plan.Beams.Count() < 1)
            {
                MessageBox.Show("Plan must contain at least one beam", "No Beams", MessageBoxButton.OK, MessageBoxImage.Error);
                throw new Exception("Plan must contain at least one beam");
            }

            MachineID = plan.Beams.First().TreatmentUnit.Id;
            Department = DepartmentInfo.GetDepartment(MachineID);

            if (!MachineExemptions.Contains(plan.Beams.First().TreatmentUnit.Id))   // Planned machine isn't in the list of test exceptions
            {
                try
                {
                    RunTest(plan);
                }
                catch (Exception e)
                {
                    logger.Error($"{DisplayName} - {e.Message}");
                }
            }
        }

        /// <summary>
        /// Executes test and stores all results
        /// </summary>
        /// <param name="plan">PlanSetup that the test will be run on</param>
        protected abstract void RunTest(PlanSetup plan);

        /// <summary>
        /// Log that check failed to run
        /// </summary>
        /// <param name="message"></param>
        protected void TestCouldNotComplete(string message)
        {
            logger.Error(message);

            Result = "Failure - Test could not be run";
            ResultDetails = message;
            ResultColor = "Tomato";
        }

        /// <summary>
        /// Test criteria have not been specified by a site for implementation
        /// </summary>
        protected void TestNotImplemented()
        {
            Result = $"Test \"{DisplayName}\" has not been implemented yet for {MachineID}";
            ResultColor = "Tomato";
            TestExplanation += "\n\nCriteria for test need to be specified";
        }

        protected static string AddSpacesToSentence(string text, bool preserveAcronyms = true)
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
