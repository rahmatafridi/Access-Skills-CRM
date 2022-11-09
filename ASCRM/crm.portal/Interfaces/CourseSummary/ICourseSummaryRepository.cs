using crm.portal.OscaModel;
using ds.core.document.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
using crm.portal.CrmModel;

namespace crm.portal.Interfaces.CourseSummary
{
    public interface ICourseSummaryRepository
    {
        List<CourseTopic> getCourseTopics(int iLearnerId);
        CourseTopic getCourseTopicsById(int courseId);
        List<CourseSummaryLearnerNotes> GetSummaryNote(int iLearnerId);

        List<CourseTopicDocuments> getCourseTopicsDocsForLearner(int iLearnerId);
        List<documentModel> UploadCourseSummaryContent(int id,dynamic document, int Id, HttpRequest Request, out long uploaded_size, out int iCounter, out string sFiles_uploaded);
        void AssessorFile(int learnerId,HttpRequest Request, int id);
        Dashboard LoadDashboard(int id);
        Employer LoadEmployeeAccount(int id);
        List<Learner> GetMyLearners(int id);
        List<LearnerDoc> GetLearnerDocs(int id);
    }
}
