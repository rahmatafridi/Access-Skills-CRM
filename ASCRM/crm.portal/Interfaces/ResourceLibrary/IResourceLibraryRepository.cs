using crm.portal.CrmModel;
using crm.portal.OscaModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace crm.portal.Interfaces.ResourceLibrary
{
   public interface IResourceLibraryRepository
    {
        IEnumerable<DocCategory> LoadCategories(string code);
        IEnumerable<DocCategory> LoadCategoriesForAdmin();

        IEnumerable<PortalDocCategory> LoadDocCategory();
        PrimeReviewStats LoadQuickStat(int id);

        IEnumerable<PortalDoc> LoadDocFiles();
        IEnumerable<PortalDoc> LoadDocFilesById(int id);

        bool AddDocCategory(PortalDocCategory docCategory);
        bool AddDocResource(PortalDoc doc);
        bool DeleteCate(int  id);
        bool DeleteCateFile(int id);

        PortalDocCategory GetDocCategoryById(int id);
        PortalDoc GetRecourseFileById(int id);

    }
}
