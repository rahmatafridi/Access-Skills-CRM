using ds.portal.diary.Models;
using System.Collections.Generic;

namespace ds.portal.diary
{
    public interface IDiaryRepository
    {
        IEnumerable<DiaryModel> GetAll(int user_Id,bool isAdmin);
    }
}
