using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VMS.TPS.Common.Model.API;

namespace PlanCheck.Checks
{
    class NamingConventionChecks : PlanCheckBase
    {
        protected override List<string> MachineExemptions => new List<string> { };

        public NamingConventionChecks(PlanSetup plan) : base(plan) { }
        
        protected override void RunTest(PlanSetup plan)
        {
            DisplayName = "Naming Conventions";
            TestExplanation = "Checks Course, Plan, and Reference Point naming against OneAria conventions";
            Result = "";
            ResultDetails = "";
            DisplayColor = ResultColorChoices.Pass;

            var courseIdRegexStrings = Properties.Resources.CourseSiteNames.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList().Select(x => $@"^\d{{1,2}} (?:[RL] )?{x}$");
            var planIdRegexStrings = Properties.Resources.PlanSiteNames.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList().Select(x => $@"^(?:[RL] )?{x}(?: (?:LN|Bst))?_\d[a-z]?\.?$");
            var refPointRegexStrings = Properties.Resources.PlanSiteNames.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList().Select(x => $@"^(?:[RL] )?{x}(?: (?:LN|Bst))?$");

            // Plan ID suffix vs Treatment Type
            if(plan.RTPrescription != null)
            {
                var planSuffixTxTypeDict = new Dictionary<string, string>
                {
                    { "[Clinical Setup]", "0" },
                    { "(2D)", "2" },
                    { "(3D)", "3" },
                    { "(IMRT)", "1" },
                    { "(SBRT)", "4" },
                    { "(SRS)", "5" },
                    { "(TBI)", "2" },
                    { "(VMAT)", "1" },
                    { "{HDR-interstitial", "6" },
                    { "{HDR-intracavitary", "6" },
                    { "{HDR-intraluminal", "6" },
                    { "|MFO|", "7" },
                    { "|SFO|", "7" }
                };
                var fullSuffix = plan.Id.Split('_').Last();
                var suffix = fullSuffix[0];
                var txType = plan.RTPrescription.Technique;

                if(planSuffixTxTypeDict[txType] != suffix.ToString())
                {
                    ResultDetails += $"Prescribed Technique is {txType} but the plan ID suffix is _{suffix}";
                    DisplayColor = ResultColorChoices.Warn;
                }
            }

            // Course ID
            bool match = false;
            foreach (var allowedCourseIdString in courseIdRegexStrings)
            {
                Regex courseRegex = new Regex(allowedCourseIdString);
                if (courseRegex.IsMatch(plan.Course.Id))
                {
                    match = true;
                    continue;
                }
            }

            if (!match)
            {
                ResultDetails += $"Course ID: {plan.Course.Id} doesn't match OneAria naming conventions\n";
                DisplayColor = ResultColorChoices.Warn;
            }

            // Plan ID
            match = false;
            foreach (var allowedPlanIdString in planIdRegexStrings)
            {
                Regex planRegex = new Regex(allowedPlanIdString);
                if (planRegex.IsMatch(plan.Id))
                {
                    match = true;
                    continue;
                }
            }

            if (!match)
            {
                ResultDetails += $"Plan ID: {plan.Id} doesn't match OneAria naming conventions\n";
                DisplayColor = ResultColorChoices.Warn;
            }

            // Reference Point ID
            match = false;
            foreach (var allowedRefPointString in refPointRegexStrings)
            {
                Regex refPointRegex = new Regex(allowedRefPointString);
                if (refPointRegex.IsMatch(plan.PrimaryReferencePoint.Id))
                {
                    match = true;
                    continue;
                }
            }

            if (!match)
            {
                ResultDetails += $"Reference Point: {plan.PrimaryReferencePoint.Id} doesn't match OneAria naming conventions\n";
                DisplayColor = ResultColorChoices.Warn;
            }

            // Plan Name
            match = false;
            foreach (var planNameString in refPointRegexStrings)
            {
                Regex planNameRegex = new Regex(planNameString);
                if (planNameRegex.IsMatch(plan.Name))
                {
                    match = true;
                    continue;
                }
            }

            if (!match)
            {
                ResultDetails += $"Plan Name: {(plan.Name == "" ? "Blank name" : plan.Name)} doesn't match OneAria naming conventions\n";
                DisplayColor = ResultColorChoices.Warn;
            }

            ResultDetails = ResultDetails.TrimEnd('\n');

            if (ResultDetails == "")
                Result = "Pass";
        }
    }
}
