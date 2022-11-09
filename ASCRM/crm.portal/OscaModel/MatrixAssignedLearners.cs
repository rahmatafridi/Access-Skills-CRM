using System;
using System.Collections.Generic;
using System.Text;

namespace crm.portal.OscaModel
{
  public  class MatrixAssignedLearners
    {
        public int SSML_Id { get; set; }

        public int? SSML_Id_Matrix { get; set; }

        public int? SSML_Id_Learner { get; set; }

        public int? SSML_Id_LearnerCourse { get; set; }

        public int? SSML_Id_Course { get; set; }

        public int? SSML_Id_MatrixLabel { get; set; }

        public int? SSML_Id_Level { get; set; }

        public int? SSML_Id_LevelKey { get; set; }

        public int? SSML_Id_LevelValue { get; set; }

        public byte? SSML_IsCompleted { get; set; }

        public int? SSML_CompletedByUser { get; set; }

        public string SSML_CompletedByUsername { get; set; }

        public DateTime? SSML_CompletionDate { get; set; }
    }
}
