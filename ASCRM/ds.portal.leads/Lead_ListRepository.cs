using Dapper;
using ds.core.uow;
using ds.portal.leads.Models;
using ds.portal.leads.Models.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace ds.portal.leads
{
    public class Lead_ListRepository : ILead_ListRepository
    {
        private readonly string _connString;
        private readonly IUOW _unitOfWork;
        public Lead_ListRepository(IUOW unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _connString = unitOfWork.GetConnectionString();

        }
        public IEnumerable<DropdownOption> GetDropdownOptionsByHeaderName(string headerName)
        {
            IEnumerable<DropdownOption> options = new List<DropdownOption>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_DDLOptions_GetByByHeaderName]";
                    object dynamicParams = new { Header = headerName };

                    options = conn.Query<DropdownOption>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return options;
        }
        public IEnumerable<DropdownOption> GetDropdownOptionsByHeaderId(int headerId)
        {
            IEnumerable<DropdownOption> options = new List<DropdownOption>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_DDLOptions_GetByByHeaderId]";
                    object dynamicParams = new { Opt_Id_OptHeader = headerId };

                    options = conn.Query<DropdownOption>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return options;
        }
        public IEnumerable<DropdownOptionsHeader> GetDropdownOptionsHeaders()
        {
            IEnumerable<DropdownOptionsHeader> optionsHeaders = new List<DropdownOptionsHeader>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_DDLOptions_GetAllHeaders]";

                    optionsHeaders = conn.Query<DropdownOptionsHeader>(storedProc, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return optionsHeaders;
        }

        public bool InsertUpdateOptionRecord(DropdownOption dropdownOption)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    if (dropdownOption.Opt_Id > 0)
                    {
                        string storedProc = "[dbo].[SP_DDLOptions_Update]";
                        object dynamicParams = dropdownOption;

                        conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    }
                    else
                    {
                        string storedProc = "[dbo].[SP_DDLOptions_Insert]";
                        object dynamicParams = dropdownOption;

                        conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return true;
        }

        public bool DeleteDropdownOptionByOptionId(int id, int userId, string userName)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_DDLOptions_DeleteById]";
                    object dynamicParams = new { Opt_Id = id };

                    conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return true;
        }
        public Tuple<bool, bool> CheckTitleAndValueExists(DropdownOption dropdownOption)
        {                        
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_DDLOptions_CheckTitleAndValueExists]";
                    object dynamicParams = new { Opt_Id = dropdownOption.Opt_Id, Opt_Value = dropdownOption.Opt_Value, Opt_Title = dropdownOption.Opt_Title, Opt_Id_OptHeader = dropdownOption.Opt_Id_OptHeader };

                    var dynamic = conn.QuerySingleOrDefault<dynamic>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                    var fields = dynamic as IDictionary<string, object>;

                    return Tuple.Create<bool, bool>(Convert.ToBoolean(fields["TitleExists"]), Convert.ToBoolean(fields["ValueExists"]));
                }
                catch (Exception)
                {
                    throw;
                }
            }            
        }
        
        public DropdownOption GetDropdownOptionsByHeaderNameAndValue(string headerName, string opt_value)
        {
            DropdownOption option = new DropdownOption();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_List_GetTitleByHeaderNameAndValue]";
                    object dynamicParams = new { Header = headerName, Value = opt_value };

                    option = conn.QuerySingle<DropdownOption>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return option;
        }

        public IEnumerable<CompanyGroup> GetAllCompanyGroup()
        {
            IEnumerable<CompanyGroup> companyGroups = new List<CompanyGroup>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Company_Group_GetAll]";

                    companyGroups = conn.Query<CompanyGroup>(storedProc, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return companyGroups;
        }

        public IEnumerable<Company> GetAllCompany()
        {
            IEnumerable<Company> company = new List<Company>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Company_For_Lead]";

                    company = conn.Query<Company>(storedProc, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return company;
        }

        public DropdownOption GetLastResultBySa(int value, string title)
        {
            DropdownOption option = new DropdownOption();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[Sp_Get_LastResult_For_SA]";
                    object dynamicParams = new {  title = title , value = value };

                    option = conn.QuerySingle<DropdownOption>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return option;
        }
    }
}
