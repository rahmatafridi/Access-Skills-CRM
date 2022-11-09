using crm.portal.OscaModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace crm.portal.Interfaces.Matrix
{
   public interface IMatrixRepository
    {
        IEnumerable<MatrixLabel> LoadMatrixLables(int id);
        CourseLevel LoadLearnerCourse(int id);
        CourseLevel LoadLearnerCourseForOsca(int id);

        IEnumerable<Matrixes> LoadMatrix(int userId,int matrixLabelId);
        IEnumerable<MatrixSummary> LoadMatrixSummary(int course_id, int matrixLabelId);
        IEnumerable<MatrixSummary> LoadMatrixFullSummary(int topic_Id,int course_Id, int matrixLabelId);
        List<MatrixLabel> GetMatrixLabels(int LearnerCourseId);
        List<MatrixAssignedLearners> MatrixDetail(int learnercours, int topicId, int lableId);

    }
}
