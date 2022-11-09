using crm.portal.CrmModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace crm.portal.Interfaces.WorkUpload
{
  public  interface IWorkUploadRepository
    {
        IEnumerable<UploadWork> GetLearnerWorkForAdmin(string type);
        IEnumerable<UploadWork> GetLearnerWorkForSearch(string type,string name);
        IEnumerable<UploadWork> GetLearnerWorkForSearchSubmitted(string name);

        IEnumerable<UploadWork> GetLearnerWorkForAssessor(int id);
        IEnumerable<WorkAudit> GetAssessorAuditWork(int id);

        IEnumerable<UploadWork> GetLearnerWorkForCheck(int leanerId);
        IEnumerable<UploadedWorkFiles> WorkFiles(int id);
        IEnumerable<UploadWork> Notifications(int id);
        IEnumerable<UploadWork> AssessorSummary(int id);
        //public IEnumerable<UploadWork> AssessorSummary(int id)

        UploadWork GetUploadWorkById(int id);
        bool DeleteWorkFile(int id);

        IEnumerable<Portal_User> LoadAssessor();
        int AssignAssesser(UploadWork uploadWork);
        int RejectWork(UploadWork uploadWork);
        int ApproveWork(UploadWork uploadWork);
        IEnumerable<UploadWork> CheckAssessorTask();
        TraningAssessment LoadTraningData(int learner_id, int topic_id);
        List<TraningAssessment> LoadTraningHistoryData(int learner_id, int topic_id);
        int UpdateTraningData(TraningAssessment traningAssessment);
    }
}
