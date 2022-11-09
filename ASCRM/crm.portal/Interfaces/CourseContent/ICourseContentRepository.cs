using crm.portal.CrmModel;
using crm.portal.OscaModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace crm.portal.Interfaces.CourseContent
{
   public interface ICourseContentRepository
    {
        IEnumerable<CourseContent_Portal> GetCourseContents(int userId,int id, bool isAdmin);
        IEnumerable<Courses> LoadCourses();
        bool InsertUpdateDocumentRecord(CourseContent_Portal document);
        bool DeleteCourseContentById(int id);
        CourseContent_Portal GetCourseContentById(int Id);

    }
}
