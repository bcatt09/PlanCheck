using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.TPS.Common.Model.API;
using VMS.TPS.Common.Model.Types;

namespace VMS.TPS.PlanChecks
{
    public class PlanApprovalChecks : PlanCheck
    {
        protected override List<string> MachineExemptions => new List<string> { };

        public PlanApprovalChecks(PlanSetup plan) : base(plan) { }

        protected override void RunTest(PlanSetup plan)
        {
            PlanSetupApprovalStatus approvalStatus = plan.ApprovalStatus;

            DisplayName = "Plan Approval";
            Result = "";
            ResultDetails = $"Status: {AddSpaces(approvalStatus.ToString())}";
            ResultColor = "Gold";
            TestExplanation = "Displays plan approval\nAlso checks that plan has been reviewed by a physician\nReviewed timestamp is estimated based on target structure or CT image approval";

            // Not Approved yet
            if (approvalStatus != PlanSetupApprovalStatus.ExternallyApproved && approvalStatus != PlanSetupApprovalStatus.PlanningApproved && approvalStatus != PlanSetupApprovalStatus.Reviewed && approvalStatus != PlanSetupApprovalStatus.TreatmentApproved)
                return;

            // Has been Approved or Reviewed
            else
            {
                // Hasn't been "Reviewed"
                if (plan.ApprovalHistory.Where(x => x.ApprovalStatus == PlanSetupApprovalStatus.Reviewed).Count() < 1)
                {
                    Result = "Warning";
                    ResultDetails += "Plan has not been \"Reviewed\"\nVerify that a physician has reviewed the plan";
                }
                // Has been Reviewed
                if (plan.ApprovalHistory.Where(x => x.ApprovalStatus == PlanSetupApprovalStatus.Reviewed).Count() > 0)
                {
                    // Get user who marked plan as "Reviewed" last
                    ApprovalHistoryEntry reviewedHistoryEntry = plan.ApprovalHistory.Where(x => x.ApprovalStatus == PlanSetupApprovalStatus.Reviewed).Last();
                    string reviewedUserDisplayName = reviewedHistoryEntry.UserDisplayName;
                    string reviewedUserName = reviewedHistoryEntry.UserId;
                    string reviewedDateTime = reviewedHistoryEntry.ApprovalDateTime.ToString("dddd, MMMM d, yyyy H:mm:ss tt");
                    string reviewedUserNameMinusDomain = reviewedUserName.Substring(reviewedUserName.IndexOf('\\') + 1);

                    if (DepartmentInfo.GetRadOncUserNames(Department).Count > 0)
                    {
                        // Check approval user name against physician list
                        if (!DepartmentInfo.GetRadOncUserNames(Department).Contains(reviewedUserNameMinusDomain))
                        {
                            Result = "Warning";
                            ResultDetails += $"\n\"Reviewed\" by {reviewedUserDisplayName} at {reviewedDateTime}\nVerify that a physician reviewed the plan";
                        }
                        else
                        {
                            ResultColor = "LimeGreen";
                            ResultDetails += $"\nReviewed by: {reviewedUserName} at {reviewedDateTime}";
                        }
                    }
                    // No physician user names have been defined for this site
                    else
                        TestNotImplemented();
                }
                // Has been Planning Approved
                if (plan.ApprovalHistory.Where(x => x.ApprovalStatus == PlanSetupApprovalStatus.PlanningApproved).Count() > 0)
                {
                    // Get user who marked plan as "Planning Approved" last
                    ApprovalHistoryEntry planningApprovedHistoryEntry = plan.ApprovalHistory.Where(x => x.ApprovalStatus == PlanSetupApprovalStatus.PlanningApproved).Last();
                    string planningApprovedUserDisplayName = planningApprovedHistoryEntry.UserDisplayName;
                    string planningApprovedDateTime = planningApprovedHistoryEntry.ApprovalDateTime.ToString("dddd, MMMM d, yyyy H:mm:ss tt");

                    ResultColor = "LimeGreen";
                    ResultDetails += $"\nPlanning Approved by: {planningApprovedUserDisplayName} at {planningApprovedDateTime}";
                }
                // Has been Treatment Approved
                if (plan.ApprovalHistory.Where(x => x.ApprovalStatus == PlanSetupApprovalStatus.TreatmentApproved).Count() > 0)
                {
                    // Get user who marked plan as "Treatment Approved" last
                    ApprovalHistoryEntry treatApprovedHistoryEntry = plan.ApprovalHistory.Where(x => x.ApprovalStatus == PlanSetupApprovalStatus.TreatmentApproved).Last();
                    string treatApprovedUserDisplayName = treatApprovedHistoryEntry.UserDisplayName;
                    string treatApprovedDateTime = treatApprovedHistoryEntry.ApprovalDateTime.ToString("dddd, MMMM d, yyyy H:mm:ss tt");

                    ResultColor = "LimeGreen";
                    ResultDetails += $"\nPlanning Approved by: {treatApprovedUserDisplayName} at {treatApprovedDateTime}";
                }
            }
        }
    }
}
