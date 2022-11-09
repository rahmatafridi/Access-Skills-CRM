using Dapper;
using ds.core.uow;
using ds.portal.diary.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ds.portal.diary
{
    public class DiaryRepository : IDiaryRepository
    {
        private readonly string _connString;
        private readonly IUOW _unitOfWork;
        public DiaryRepository(IUOW unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _connString = unitOfWork.GetConnectionString();
        }

        public IEnumerable<DiaryModel> GetAll(int user_Id, bool isAdmin)
        {
            IEnumerable<DiaryModel> diaryModels  = new List<DiaryModel>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Diary_GetAll]";
                    object dynamicParams = new { @userId = user_Id, @isAdmin = isAdmin };
                    diaryModels = conn.Query<DiaryModel>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return diaryModels;
        }
    }
}
