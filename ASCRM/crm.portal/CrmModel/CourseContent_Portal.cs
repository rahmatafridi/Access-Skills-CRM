using System;
using System.Collections.Generic;
using System.Text;

namespace crm.portal.CrmModel
{
   public class CourseContent_Portal
    {
        public int CC_Id { get; set; }

        public int CC_Id_Course_Level { get; set; }

        public string CC_Name { get; set; }

        public string CC_Description { get; set; }

        public string CC_Type { get; set; }

        public string CC_FilePath { get; set; }

        public int? CC_CreatedByUserId { get; set; }

        public string CC_CreatedByUserName { get; set; }

        public DateTime? CC_CreatedDate { get; set; }

        public int? CC_UpdatedByUserId { get; set; }

        public string CC_UpdatedByUserName { get; set; }

        public DateTime? CC_UpdatedDate { get; set; }

        public bool? CC_IsDeleted { get; set; }

        public int? CC_DeletedByUserId { get; set; }

        public string CC_DeletedByUserName { get; set; }

        public DateTime? CC_DeletedDate { get; set; }

        public string CC_File_Size { get; set; }

        public string CC_SortOrder { get; set; }
    }
}
