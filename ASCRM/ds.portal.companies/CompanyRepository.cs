using System;
using System.Collections.Generic;
using System.Text;
using ds.portal.companies.Models;
using Dapper;
using ds.core.uow; 
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using ds.portal.leads.Models;
using ds.portal.leads.Models.Shared;
using ds.portal.users.Models;
using CompanyContact = ds.portal.companies.Models.CompanyContact;
using System.Linq;

namespace ds.portal.companies
{ 
    public class CompanyRepository : ICompanyRepository
    {
        private readonly string _connString;
        private readonly IUOW _unitOfWork;
        public CompanyRepository(IUOW unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _connString = unitOfWork.GetConnectionString();
        }
        public bool DeleteNoteById(int id, int userId, string userName)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Company_Notes_DeleteById]";
                    object dynamicParams = new { Note_Id = id, Note_DeletedByUserId = userId, Note_DeletedByUserName = userName };
                    conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return true;
        }
        public bool DeleteCompanyContactById(int id, int userId, string userName)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Company_Contact_DeleteById]";
                    object dynamicParams = new { contact_id = id, deletedby = userId };

                    conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return true;
        }

        public CompanyModel GetCompanyById(int id)
        {
            CompanyModel company = new CompanyModel();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Company_GetById]";
                    object dynamicParams = new { id = id };

                    company = conn.QueryFirstOrDefault<CompanyModel>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return company;
        }
        public IEnumerable<CompanyModel> CompanySearchForOsca(string id)
        {
            IEnumerable<CompanyModel> company = new List<CompanyModel>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                  
                   

                    string storedProc = "[dbo].[SP_Company_Search_ForOsca]";
                    object dynamicParams = new { id = id };
                    company = conn.Query<CompanyModel>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    var listSerch = new List<CompanySearch>();

                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return company;
        }

        public IEnumerable<CompanyModel> CompanySearch(string id, string name, int? sale_id)
        {
            IEnumerable<CompanyModel> company = new List<CompanyModel>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                   var param = "";
                    if(id != null)
                    {
                        param = id;
                    }
                    if(name != null)
                    {
                        param = name;
                    }
                    if(sale_id != null)
                    {
                        param = sale_id.ToString();
                    }

                    string storedProc = "[dbo].[SP_Company_Search]";
                    object dynamicParams = new { id = param };
                    company = conn.Query<CompanyModel>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    var listSerch = new List<CompanySearch>();
                
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return company;
        }
        public IEnumerable<CompanySearch> CompanySearchAtZ(string name,string searchtype)
        {
            IEnumerable<CompanySearch> company = new List<CompanySearch>();
            var listSerch = new List<CompanySearch>();

            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
           
                  if(name == null)
                    {
                        name = "";
                    }
                    if (searchtype == null)
                    {
                        searchtype = "";
                    }

                    string storedProc = "[dbo].[SP_Company_SearchATZ]";
                    object dynamicParams = new { @name = name, @searchtype = searchtype };
                    company = conn.Query<CompanySearch>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    //foreach (var item in company)
                    //{
                    //    //if (item.name.Trim().StartsWith("A"))
                    //    //{
                    //    //    if (!listSerch.Any(a => a.Name == "A"))
                    //    //    {
                    //    //        listSerch.Add(new Models.CompanySearch()
                    //    //        {
                    //    //            Name = "A",
                    //    //            models = company.Where(x => x.name.Trim().StartsWith("A")).ToList()

                    //    //        });
                    //    //    }
                    //    //}
                    //    //if (item.name.Trim().StartsWith("B"))
                    //    //{
                    //    //    if (!listSerch.Any(a => a.Name == "B"))
                    //    //    {
                    //    //        listSerch.Add(new Models.CompanySearch()
                    //    //        {
                    //    //            Name = "B",
                    //    //            models = company.Where(x => x.name.Trim().StartsWith("B")).ToList()

                    //    //        });
                    //    //    }
                    //    //}

                    //    //if (item.name.Trim().StartsWith("C"))
                    //    //{
                    //    //    if (!listSerch.Any(a => a.Name == "C"))
                    //    //    {
                    //    //        listSerch.Add(new Models.CompanySearch()
                    //    //        {
                    //    //            Name = "C",
                    //    //            models = company.Where(x => x.name.Trim().StartsWith("C")).ToList()

                    //    //        });
                    //    //    }
                    //    //}
                    //    //if (item.name.Trim().StartsWith("D"))
                    //    //{
                    //    //    if (!listSerch.Any(a => a.Name == "D"))
                    //    //    {
                    //    //        listSerch.Add(new Models.CompanySearch()
                    //    //        {
                    //    //            Name = "D",
                    //    //            models = company.Where(x => x.name.Trim().StartsWith("D")).ToList()

                    //    //        });
                    //    //    }
                    //    //}
                    //}

                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return company;
        }
        public IEnumerable<CompanySearch> CompanySearchByWord(string name)
        {
            IEnumerable<CompanySearch> company = new List<CompanySearch>();
            var listSerch = new List<CompanySearch>();

            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {

                   

                    string storedProc = "[dbo].[SP_Company_SearchBy_Name]";
                    object dynamicParams = new { @name = name};
                    company = conn.Query<CompanySearch>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                   

                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return company;
        }

        public IEnumerable<CompanyModel> GetCompanies(int? currentUserId, DateTime? startDate, DateTime? endDate)
        {
            IEnumerable<CompanyModel> companies = new List<CompanyModel>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Company_GetAll]";
                    object dynamicParams = new { @security_user_id = currentUserId, @start_date = startDate, @end_date = endDate };
                    companies = conn.Query<CompanyModel>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return companies;
        }
        public bool DeleteCompanyById(int id, int userId, string userName)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Company_DeleteById]";
                    object dynamicParams = new { id = id, deleted_by = userId };

                    conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return true;
        }

        public CompanyModel InsertUpdateCompanyRecord(CompanyModel company)
        {
            CultureInfo ukCulture = new CultureInfo("en-GB");//CultureInfo.CreateSpecificCulture("en-GB");
            DateTime? cqc_inspection_date = string.IsNullOrEmpty(company.cqc_inspection_date) ? null : (DateTime?)DateTime.Parse(company.cqc_inspection_date, ukCulture.DateTimeFormat);
            DateTime? cqc_last_update_date = string.IsNullOrEmpty(company.cqc_last_update_date) ? null : (DateTime?)DateTime.Parse(company.cqc_last_update_date, ukCulture.DateTimeFormat);

            var dynamicParams = new DynamicParameters();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    if (company.id > 0)
                    {
                        dynamicParams.Add("@id", company.id);

                    }
                    dynamicParams.Add("@name", company.name);
                    dynamicParams.Add("@reference", company.reference);
                    dynamicParams.Add("@contact_name1", company.contact_name1);
                    dynamicParams.Add("@contact_name2", company.contact_name2);
                    dynamicParams.Add("@address1", company.address1);
                    dynamicParams.Add("@address2", company.address2);
                    dynamicParams.Add("@address3", company.address3);
                    dynamicParams.Add("@town", company.town);
                    dynamicParams.Add("@county", company.county);
                    dynamicParams.Add("@postcode", company.postcode);
                    dynamicParams.Add("@country_id", company.country_id);
                    dynamicParams.Add("@tel1", company.tel1);
                    dynamicParams.Add("@tel2", company.tel2);
                    dynamicParams.Add("@mobile1", company.mobile1);
                    dynamicParams.Add("@mobile2", company.mobile2);
                    dynamicParams.Add("@fax1", company.fax1);
                    dynamicParams.Add("@fax2", company.fax2);
                    dynamicParams.Add("@email1", company.email1);
                    dynamicParams.Add("@email2", company.email2);
                    dynamicParams.Add("@website", company.website);
                    dynamicParams.Add("@rating", company.rating);
                    dynamicParams.Add("@number_of_beds", company.number_of_beds);
                    dynamicParams.Add("@number_of_employees", company.number_of_employees);
                    dynamicParams.Add("@edrs_number", company.edrs_number);
                    dynamicParams.Add("@is_active", company.is_active);
                    dynamicParams.Add("@is_deleted", company.is_deleted);
                    dynamicParams.Add("@is_sponsor", company.is_sponsor);
                    dynamicParams.Add("@is_levy_paying", company.is_levy_paying);
                    dynamicParams.Add("@company_group_id", company.company_group_id);
                    dynamicParams.Add("@ratings_id", company.ratings_id);
                    dynamicParams.Add("@sales_advisor_id", company.sales_advisor_id);
                    dynamicParams.Add("@cqc_rating", company.cqc_rating);
                    dynamicParams.Add("@cqc_capacity", company.cqc_capacity);
                    dynamicParams.Add("@cqc_inspection_date", cqc_inspection_date);
                    dynamicParams.Add("@cqc_last_update_date", cqc_last_update_date);
                    dynamicParams.Add("@cqc_standard_1", company.cqc_standard_1);
                    dynamicParams.Add("@cqc_standard_2", company.cqc_standard_2);
                    dynamicParams.Add("@cqc_standard_3", company.cqc_standard_3);
                    dynamicParams.Add("@cqc_standard_4", company.cqc_standard_4);
                    dynamicParams.Add("@cqc_standard_5", company.cqc_standard_5);
                    dynamicParams.Add("@created_by", company.created_by);
                    dynamicParams.Add("@created_date", DateTime.Now);
                    dynamicParams.Add("@modified_date", DateTime.Now);
                    dynamicParams.Add("@modified_by", company.modified_by);
                    dynamicParams.Add("@deleted_date", company.deleted_date);
                    dynamicParams.Add("@deleted_by", company.deleted_by);
                    dynamicParams.Add("@company_number", company.company_number);
                    dynamicParams.Add("@name_desision_maker", company.name_desision_maker);
                    dynamicParams.Add("@cqc_registration_number", company.cqc_registration_number);
                    dynamicParams.Add("@business_type", company.business_type);
                    dynamicParams.Add("@levy_employer", company.levy_employer);
                    dynamicParams.Add("@no_of_employees", company.no_of_employees);
                    dynamicParams.Add("@company_house_registration_number", company.company_house_registration_number);
                    dynamicParams.Add("@employer_email_address", company.employer_email_address);
                    dynamicParams.Add("@job_title", company.job_title);
                    dynamicParams.Add("@insurance_no", company.insurance_no);
                    if (company.expiry_date == null || company.expiry_date == "")
                    {
                        dynamicParams.Add("@expiry_date", default);
                    }
                    else
                    {
                        dynamicParams.Add("@expiry_date", Convert.ToDateTime(company.expiry_date));
                    }


                    if (company.id > 0)
                    {
                        string storedProc = "[dbo].[SP_Company_Update]";
                        conn.Query<CompanyModel>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                    }
                    else
                    {
                        string storedProc = "[dbo].[SP_Company_Add]";
                        var id = conn.QueryFirstOrDefault<int>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                        if (id > 0)
                        {
                            company.id = id;
                        }
                    }
                }
                catch (Exception e)
                {
                    throw;
                }
            }
            return company;
        }

        public IEnumerable<DuplicateCompany> CheckDuplicateCompany(int id,string name, string postCode, int userId,string address, string tel)
        {
            IEnumerable<CompanyModel> company = new  List<CompanyModel>();
            IEnumerable<CompanyWorkplaces> workplace = new List<CompanyWorkplaces>();

            List<DuplicateCompany> duplicates = new List<DuplicateCompany>();
             using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Company_CheckDuplicate]";
                    string stordProcwp = "[dbo].[SP_Company_Workplace_CheckDuplicate]";

                    object dynamicParams = new
                    {
                        id= id,
                        name = string.IsNullOrEmpty(name) ? null : name,
                        postcode = string.IsNullOrEmpty(postCode) ? null : postCode,
                        address= string.IsNullOrEmpty(address)? null :address,
                        tel = string.IsNullOrEmpty(tel)? null : tel,
                        userId = userId,

                    };
                    company = conn.Query<CompanyModel>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    
                    if(company.Any())
                    {
                        foreach (var item in company)
                        {
                            duplicates.Add(new DuplicateCompany()
                            {
                                address1 = item.address1,
                                id = item.id,
                                name = item.name,
                                postcode = item.postcode,
                                telephone = item.tel1,
                                type="C"
                            });
                        }
                        
                    }
                    conn.Close();
                    conn.Open();
                    object dynamicParamswp = new
                    {
                        id = id,
                        name = string.IsNullOrEmpty(name) ? null : name,
                        postcode = string.IsNullOrEmpty(postCode) ? null : postCode,
                        address = string.IsNullOrEmpty(address) ? null : address,
                        tel = string.IsNullOrEmpty(tel) ? null : tel,
                        userId = userId,

                    };
                    workplace = conn.Query<CompanyWorkplaces>(stordProcwp, param: dynamicParamswp, commandType: CommandType.StoredProcedure);
                    if(workplace.Any())
                    {
                        foreach (var item in workplace)
                        {
                            duplicates.Add(new DuplicateCompany()
                            {
                                address1 = item.address1,
                                telephone = item.phone_number,
                                name = item.wp_name ,
                                postcode = item.post_code,
                                id = item.wp_id,
                                type="WP"
                            });
                        }
                       
                    }

                }
                catch (Exception)
                {
                    throw;
                }
            }
            return duplicates;
        }
        public IEnumerable<DuplicateCompany> CheckDuplicateCompanyPostcode(string postCode,string name)
        {
            IEnumerable<CompanyModel> company = new List<CompanyModel>();
            IEnumerable<CompanyWorkplaces> workplace = new List<CompanyWorkplaces>();

            List<DuplicateCompany> duplicates = new List<DuplicateCompany>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Company_CheckDuplicate_Postcode]";
                    //string stordProcwp = "[dbo].[SP_Company_Workplace_CheckDuplicate]";

                    object dynamicParams = new
                    {
                       
                        postcode = string.IsNullOrEmpty(postCode) ? null : postCode,
                        nameadd = name

                    };
                    company = conn.Query<CompanyModel>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                    if (company.Any())
                    {
                        foreach (var item in company)
                        {
                            duplicates.Add(new DuplicateCompany()
                            {
                                address1 = item.address1,
                                id = item.id,
                                name = item.name,
                                postcode = item.postcode,
                               // telephone = item.tel1,
                            });
                        }

                    }
                   
                    
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return duplicates;
        }

        public bool InsertUpdateCompanyNoteRecord(CompanyNote note)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {


                try
                {
                    if (note.Note_Id > 0)
                    {
                        string storedProc = "[dbo].[SP_Company_Notes_Update]";
                        object dynamicParams = note;
                        conn.Query<CompanyNote>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    }
                    else
                    {
                        note.Note_DateCreated = DateTime.Now;

                        string storedProc = "[dbo].[SP_Company_Notes_Insert]";
                        object dynamicParams = note;
                        conn.Query<CompanyNote>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return true;
        }
        public bool InsertUpdateCompanyContactRecord(CompanyContact companyContact)
        {
            var dynamicParams = new DynamicParameters();

            using (SqlConnection conn = new SqlConnection(_connString))
            {

                try
                {
                    if (companyContact.contact_id > 0)
                    {
                        dynamicParams.Add("@contact_id", companyContact.contact_id);

                    }
                    dynamicParams.Add("@contact_name", companyContact.contact_name);
                    dynamicParams.Add("@title", companyContact.title);
                    dynamicParams.Add("@email1", companyContact.email1);
                    dynamicParams.Add("@email2", companyContact.email2);
                    dynamicParams.Add("@mobile1", companyContact.mobile1);
                    dynamicParams.Add("@job_title", companyContact.job_title);
                    dynamicParams.Add("@website", companyContact.website);
                    dynamicParams.Add("@company_id", companyContact.company_id);
                    dynamicParams.Add("@wp_id", companyContact.wp_id);
                    if (companyContact.contact_id > 0)
                    {
                        dynamicParams.Add("@modified_date", DateTime.Now);
                        dynamicParams.Add("@modified_by", companyContact.modified_by);
                        string storedProc = "[dbo].[SP_Company_Contact_Update]";
                        conn.Query<DropdownOption>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                    }
                    else
                    {

                        dynamicParams.Add("@created_date", DateTime.Now);
                        dynamicParams.Add("@created_by", companyContact.created_by);
                        string storedProc = "[dbo].[SP_Company_Contact_Insert]";
                        var id = conn.QueryFirstOrDefault<int>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                        if (id > 0)
                        {
                            companyContact.contact_id = id;
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }

    
            }
            return true;
        }

        public IEnumerable<CompanyContact> GetAllCompanyContacts(int company_Id)
        {
            IEnumerable<CompanyContact> contacts = new List<CompanyContact>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Company_Contact_GetAllByCompanyId]";
                    object dynamicParams = new { company_Id = company_Id };

                    contacts = conn.Query<CompanyContact>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return contacts;
        }

        public IEnumerable<CompanyContact> GetAllWorkplaceContacts(int company_id ,int wp_id)
        {
            IEnumerable<CompanyContact> contacts = new List<CompanyContact>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Company_Workplace_Contact_GetAllByWPId]";
                    object dynamicParams = new {company_id= company_id , wp_id = wp_id };

                    contacts = conn.Query<CompanyContact>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return contacts;
        }

        public IEnumerable<CompanyNote> GetAllCompanyNotes(int company_Id)
        {
            IEnumerable<CompanyNote> notes = new List<CompanyNote>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Company_Notes_GetAllByCompanyId]";
                    object dynamicParams = new { Note_Id_Company = company_Id };

                    notes = conn.Query<CompanyNote>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return notes;
        }

        public CompanyNote GetNoteById(int id)
        {
            CompanyNote note = new CompanyNote();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Company_Notes_GetById]";
                    object dynamicParams = new { Note_Id = id };

                    note = conn.QuerySingleOrDefault<CompanyNote>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return note;
        }
        public CompanyContact GetCompanyContactById(int id)
        {
            CompanyContact contact = new CompanyContact();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Company_Contact_GetById]";
                    object dynamicParams = new { contact_Id = id };

                    contact = conn.QuerySingleOrDefault<CompanyContact>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return contact;
        }

        public IEnumerable<CompanyWorkplaces> GetAllCompanyWorkplaceses(int company_Id)
        {
            IEnumerable<CompanyWorkplaces> workplaceses = new List<CompanyWorkplaces>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Company_WorkPlace_GetAllByCompanyId]";
                    object dynamicParams = new { company_Id = company_Id };

                    workplaceses = conn.Query<CompanyWorkplaces>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return workplaceses;
        }

        public bool InsertUpdateCompanyWorkPlaceRecord(CompanyWorkplaces workplaces)
        {
            var dynamicParams = new DynamicParameters();

            using (SqlConnection conn = new SqlConnection(_connString))
            {


                try
                {
                    if (workplaces.wp_id > 0)
                    {
                        dynamicParams.Add("@wp_id", workplaces.wp_id);

                    }
                    dynamicParams.Add("@wp_name", workplaces.wp_name);
                    dynamicParams.Add("@address1", workplaces.address1);
                    dynamicParams.Add("@address2", workplaces.address2);
                    dynamicParams.Add("@post_code", workplaces.post_code);
                   
                    dynamicParams.Add("@company_id", workplaces.company_id);
                    dynamicParams.Add("@job_title", workplaces.job_title);
                    dynamicParams.Add("@county", workplaces.county);
                    dynamicParams.Add("@town", workplaces.town);
                    dynamicParams.Add("@employee_email", workplaces.employee_email);
                    dynamicParams.Add("@phone_number", workplaces.phone_number);
                    dynamicParams.Add("@business_type", workplaces.business_type);
                    dynamicParams.Add("@company_name",workplaces.company_name);

                    if (workplaces.wp_id > 0)
                    {
                        dynamicParams.Add("@updated_by", workplaces.updated_by);
                        dynamicParams.Add("@updated_date", workplaces.updated_date);
                        string storedProc = "[dbo].[SP_WorkPlace_Update]";
                        conn.Query<DropdownOption>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                    }
                    else
                    {
                        dynamicParams.Add("@created_by", workplaces.created_by);
                        dynamicParams.Add("@created_date",null);
                        string storedProc = "[dbo].[SP_Company_WorkPlace_Insert]";
                        var id = conn.QueryFirstOrDefault<int>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                        if (id > 0)
                        {
                            workplaces.wp_id = id;
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return true;
        }

        public CompanyWorkplaces GetCompanyWorkplaceById(int id)
        {
            CompanyWorkplaces workplaces = new CompanyWorkplaces();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_WorkPlace_GetById]";
                    object dynamicParams = new { wp_id = id };

                    workplaces = conn.QuerySingleOrDefault<CompanyWorkplaces>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return workplaces;
        }

        public bool DeleteCompanyWorkPlaceById(int id, int userId, string userName)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_WorkPlace_DeleteById]";
                    object dynamicParams = new { wp_id = id, deletedby = userId };

                    conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return true;
        }

        public IEnumerable<CompanyAssignedLearnerModel> GetAllAssignedLearner(int company_id)
        {
            IEnumerable<CompanyAssignedLearnerModel> assigned = new List<CompanyAssignedLearnerModel>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Assigned_Learner_GetAll]";
                    object dynamicParams = new { company_id = company_id };

                    assigned = conn.Query<CompanyAssignedLearnerModel>(storedProc, dynamicParams,  commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return assigned;
        }
        private IEnumerable<CompanyUploadRequiredOptions> getAllOptions()
        {
            IEnumerable<CompanyUploadRequiredOptions> _options = new List<CompanyUploadRequiredOptions>();

            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Lead_GetRequiredOptions]";
                    //object dynamicParams = new { @lead_id = lead_id, @sales_advisor_id = sales_advisor_id };
                    _options = conn.Query<CompanyUploadRequiredOptions>(storedProc, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return _options;
        }
        private bool IsOptionContains(IEnumerable<CompanyUploadRequiredOptions> options, int opt_header, string find_val)
        {
            if (find_val == "") return true;

            foreach (CompanyUploadRequiredOptions opt in options)
            {
                if (opt.Opt_Id_OptHeader == opt_header)
                {
                    if (opt.Opt_Title.ToLower().Trim() == find_val.ToLower().Trim())
                    {
                        return true;

                    }

                }



            }
            return false;

        }

        public List<CompanyFileUpload> ValidateCompanyForImport(List<CompanyFileUpload> companyFileUploads)
        {
            IEnumerable<CompanyUploadRequiredOptions> _options = getAllOptions();
            string current_option = "";
            if (_options != null)
            {
                CompanyGroup contact = new CompanyGroup();
                Users user = new Users();

                IEnumerable<CompanyFileUpload> contacts_to_check = new List<CompanyFileUpload>();
                using (SqlConnection conn = new SqlConnection(_connString))
                {
                    foreach (var item in companyFileUploads)
                    {

                        string storedProc = "[dbo].[SP_Group_GetByName]";
                        object dynamicParams = new { GroupName = item.company_group};

                        string storedProcUser = "[dbo].[SP_User_GetByName]";
                        object dynamicParamsUser = new { UserName = item.sale_advisor };

                        user = conn.QuerySingleOrDefault<Users>(storedProcUser, param: dynamicParamsUser,
                            commandType: CommandType.StoredProcedure);
                        contact = conn.QuerySingleOrDefault<CompanyGroup>(storedProc, param: dynamicParams,
                            commandType: CommandType.StoredProcedure);
                        if (contact != null)
                        {
                            item.company_group_id = contact.id;
                        }
                        if (user != null)
                        {
                            item.sales_advisor_id = user.Users_ID;
                        }
                        if (item.name == "")
                        {
                            item.contact_is_error = true;
                            item.contact_issues += "Name Empty" + ",";
                        }
                        else
                        {
                            var dd = CheckDuplicateCompany(0, item.name, item.postcode, 1,item.address1,item.tel1);
                            if (dd != null)
                            {
                                item.contact_is_error = true;
                                item.contact_issues += "Duplicate Company" + ",";
                            }


                        }

                        
                    }


                }

            }



            return companyFileUploads;

        }

        public List<CompanyFileUpload> BulkInsert(List<CompanyFileUpload> companyFileUploads, int userId)
        {
            List<CompanyFileUpload> leadFileUploadsChecked = new List<CompanyFileUpload>();
         //   leadFileUploadsChecked = ValidateLeadsForImport(leadFileUploads);

            List<CompanyFileUpload> failedLeads = new List<CompanyFileUpload>();

            using (SqlConnection conn = new SqlConnection(_connString))
            {
                foreach (var item in companyFileUploads)
                {
                    if (!item.contact_is_error)
                    {
                        //if (item.Contact != "" && item.contact_is_error == false)
                        //{
                        string storedProc = "[dbo].[SP_Company_Import]";
                        var p = new DynamicParameters();

                        p.Add("@name", item.name);
                        p.Add("@reference", item.reference);
                        p.Add("@contact_name1", item.contact_name1);
                        p.Add("@address1", item.address1);
                        p.Add("@address2", item.address2);
                        p.Add("@address3", item.address3);
                        p.Add("@town", item.town);
                        p.Add("@county", item.county);
                        p.Add("@postcode", item.postcode);
                        p.Add("@tel1", item.tel1);
                        p.Add("@mobile1", item.mobile1);
                        p.Add("@fax1", item.fax1);
                        p.Add("@mobile1", item.mobile1);
                        p.Add("@email1", item.email1);
                        p.Add("@website", item.website);
                        p.Add("@rating", item.rating);
                        p.Add("@number_of_beds", item.number_of_beds);
                        p.Add("@number_of_employees", item.number_of_employees);
                        p.Add("@edrs_number", item.edrs_number);
                        p.Add("@is_active", true);
                        p.Add("@company_group_id", item.company_group_id);
                        p.Add("@sales_advisor_id", item.sales_advisor_id);
                        p.Add("@created_by", userId);
                        p.Add("@created_date", DateTime.Now);
                        p.Add("@company_number", item.company_number);
                        p.Add("@name_desision_maker ", item.name_desision_maker);




                        int a = conn.Execute(storedProc, p, commandType: CommandType.StoredProcedure);

                        //    }
                        //    else
                        //    {

                        //        failedLeads.Add(item);
                        //    }
                    }
                }
            }

            if (failedLeads.Count > 0)
            {

            }
            return failedLeads;
        }

        public IEnumerable<LeadViewModel> GetLeadByCompany(int company_id,int userId)
        {
            IEnumerable<LeadViewModel> leads = new List<LeadViewModel>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Company_Lead_GetAll]";
                    object dynamicParams = new {@company_id = company_id, @UserId = userId };
                    leads = conn.Query<LeadViewModel>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return leads;
        }

        public bool InssertUpdateAssignLearnerAssessment(CompanyAssignedLearnerModel model)
        {
            var dynamicParamsA = new DynamicParameters();
            var dynamicParamsU = new DynamicParameters();

            using (SqlConnection conn = new SqlConnection(_connString))
            {


                try
                {
                    //if (model.id > 0)
                    //{

                    //}
                  

                    //if (model.id > 0)
                    //{
                    //    dynamicParamsU.Add("@id", model.id);

                    //    dynamicParamsU.Add("@wp_id", model.wp_id);
                    //    dynamicParamsU.Add("@learner_id", model.learner_id);
                    //    dynamicParamsU.Add("@date_assigned", DateTime.Now);

                    //    dynamicParamsU.Add("@purposes", model.purposes);
                    //    dynamicParamsU.Add("@company_contact_id", model.company_contact_id);
                    //    dynamicParamsU.Add("@company_wp_contact_id", model.company_wp_contact_id);
                    //    dynamicParamsU.Add("@company_id", model.company_id);
                    //    dynamicParamsU.Add("@Is_current", model.is_current);

                    //    dynamicParamsU.Add("@is_active", model.is_active);
                    //    string storedProc = "[dbo].[SP_Company_Assigned_Learner_Update]";
                    //    conn.Query<CompanyAssignedLearnerModel>(storedProc, param: dynamicParamsU, commandType: CommandType.StoredProcedure);

                    //}
                    //else
                    //{
                        //dynamicParams.Add("@created_by", model.created_by);
                        //dynamicParams.Add("@created_date", null);

                        var existingLearner = GetAllAssignedLearnerById((int)model.learner_id,model.purposes);
                        if (existingLearner.Any())
                        {
                            foreach (var item in existingLearner)
                            {
                                dynamicParamsU.Add("@id", item.id);
                                dynamicParamsU.Add("@wp_id", item.wp_id);
                                dynamicParamsU.Add("@learner_id", item.learner_id);
                                dynamicParamsU.Add("@date_assigned", DateTime.Now);

                                dynamicParamsU.Add("@purposes", item.purposes);
                                dynamicParamsU.Add("@company_contact_id", item.company_contact_id);
                                dynamicParamsU.Add("@company_wp_contact_id", item.company_wp_contact_id);
                                dynamicParamsU.Add("@company_id", item.company_id);

                                dynamicParamsU.Add("@Is_current", false);

                                dynamicParamsU.Add("@is_active", 0);
                                string storedProc1 = "[dbo].[SP_Company_Assigned_Learner_Update]";
                                conn.Query<CompanyAssignedLearnerModel>(storedProc1, param: dynamicParamsU, commandType: CommandType.StoredProcedure);

                            }
                        }
                        dynamicParamsA.Add("@wp_id", model.wp_id);
                        dynamicParamsA.Add("@learner_id", model.learner_id);
                        dynamicParamsA.Add("@date_assigned", DateTime.Now);

                        dynamicParamsA.Add("@purposes", model.purposes);
                        dynamicParamsA.Add("@company_contact_id", model.company_contact_id);
                        dynamicParamsA.Add("@company_wp_contact_id", model.company_wp_contact_id);
                        dynamicParamsA.Add("@company_id", model.company_id);
                        dynamicParamsA.Add("@Is_current", model.is_current);

                        dynamicParamsA.Add("@is_active", model.is_active);
                        string storedProc = "[dbo].[SP_Company_Assigned_Learner_Insert]";
                        var id = conn.QueryFirstOrDefault<int>(storedProc, param: dynamicParamsA, commandType: CommandType.StoredProcedure);
                        if (id > 0)
                        {
                            model.id = id;
                        }
                   // }
                    
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return true;
        }
        public bool InssertUpdateAssignLearnerEmployer(CompanyAssignedLearnerModel model)
        {
            var dynamicParamsA = new DynamicParameters();
            var dynamicParamsU = new DynamicParameters();

            using (SqlConnection conn = new SqlConnection(_connString))
            {


                try
                {
                  

                    //if (model.id > 0)
                    //{
                    //    dynamicParamsU.Add("@id", model.id);

                    //    dynamicParamsU.Add("@wp_id", model.wp_id);
                    //    dynamicParamsU.Add("@learner_id", model.learner_id);
                    //    dynamicParamsU.Add("@date_assigned", DateTime.Now);

                    //    dynamicParamsU.Add("@purposes", model.purposes);
                    //    dynamicParamsU.Add("@company_contact_id", model.company_contact_id);
                    //    dynamicParamsU.Add("@company_wp_contact_id", model.company_wp_contact_id);
                    //    dynamicParamsU.Add("@company_id", model.company_id);
                    //    dynamicParamsU.Add("@Is_current", model.is_current);

                    //    dynamicParamsU.Add("@is_active", model.is_active);
                    //    string storedProc = "[dbo].[SP_Company_Assigned_Learner_Update]";
                    //    conn.Query<CompanyAssignedLearnerModel>(storedProc, param: dynamicParamsU, commandType: CommandType.StoredProcedure);

                    //}
                    //else
                    //{
                        //dynamicParams.Add("@created_by", model.created_by);
                        //dynamicParams.Add("@created_date", null);

                        var existingLearner = GetAllAssignedLearnerById((int)model.learner_id,model.purposes);
                        if (existingLearner.Any())
                        {
                            foreach (var item in existingLearner)
                            {
                                dynamicParamsU.Add("@id", item.id);
                                dynamicParamsU.Add("@wp_id", item.wp_id);
                                dynamicParamsU.Add("@learner_id", item.learner_id);
                                dynamicParamsU.Add("@date_assigned", DateTime.Now);

                                dynamicParamsU.Add("@purposes", item.purposes);
                                dynamicParamsU.Add("@company_contact_id", item.company_contact_id);
                                dynamicParamsU.Add("@company_wp_contact_id", item.company_wp_contact_id);
                                dynamicParamsU.Add("@company_id", item.company_id);

                                dynamicParamsU.Add("@Is_current", false);

                                dynamicParamsU.Add("@is_active", 0);
                                string storedProc1 = "[dbo].[SP_Company_Assigned_Learner_Update]";
                                conn.Query<CompanyAssignedLearnerModel>(storedProc1, param: dynamicParamsU, commandType: CommandType.StoredProcedure);

                            }
                        }
                        dynamicParamsA.Add("@wp_id", model.wp_id);
                        dynamicParamsA.Add("@learner_id", model.learner_id);
                        dynamicParamsA.Add("@date_assigned", DateTime.Now);

                        dynamicParamsA.Add("@purposes", model.purposes);
                        dynamicParamsA.Add("@company_contact_id", model.company_contact_id);
                        dynamicParamsA.Add("@company_wp_contact_id", model.company_wp_contact_id);
                        dynamicParamsA.Add("@company_id", model.company_id);
                        dynamicParamsA.Add("@Is_current", model.is_current);

                        dynamicParamsA.Add("@is_active", model.is_active);
                        string storedProc = "[dbo].[SP_Company_Assigned_Learner_Insert]";
                        var id = conn.QueryFirstOrDefault<int>(storedProc, param: dynamicParamsA, commandType: CommandType.StoredProcedure);
                        if (id > 0)
                        {
                            model.id = id;
                        }
                   // }

                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return true;
        }

        public Employeer GetEmployeerByLearner(int id)
        {
            Employeer contact = new Employeer();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Track_GetCurrentEmployerForLearner2]";
                    object dynamicParams = new { @LearnerId = id };

                    contact = conn.QueryFirstOrDefault<Employeer>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return contact;
        }
        public Employeer GetAssessmentByLearner(int id)
        {
            Employeer contact = new Employeer();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Track_GetCurrentAssessmentForLearner2]";
                    object dynamicParams = new { @LearnerId = id };

                    contact = conn.QueryFirstOrDefault<Employeer>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return contact;
        }
        public IEnumerable<CompanyModel> CompanyInSearch(string name)
        {
            IEnumerable<CompanyModel> company = new List<CompanyModel>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    var param = name;
                
                    string storedProc = "[dbo].[SP_Company_In_Search]";
                    object dynamicParams = new { id = param };
                    company = conn.Query<CompanyModel>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return company;
        }
        public IEnumerable<CompanyWorkplaces> CompanyWorkPlaceInSearch(string name,int company_id)
        {
            IEnumerable<CompanyWorkplaces> company = new List<CompanyWorkplaces>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    var param = name;

                    string storedProc = "[dbo].[Company_WorkPlace_Search]";
                    object dynamicParams = new { id = param,company_id = company_id };
                    company = conn.Query<CompanyWorkplaces>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return company;
        }

        public IEnumerable<CompanyModel> CompanySearchByName(string name)
        {
            IEnumerable<CompanyModel> company = new List<CompanyModel>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    var param = name;

                    string storedProc = "[dbo].[SP_Company_Search_By_Name]";
                    object dynamicParams = new { id = param };
                    company = conn.Query<CompanyModel>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return company;
        }

        public IEnumerable<CompanyAssignedLearnerModel> GetAllAssignedLearnerById(int id,string purpuse)
        {
            IEnumerable<CompanyAssignedLearnerModel> assigned = new List<CompanyAssignedLearnerModel>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Assigned_Learner_By_LearnerId]";
                    object dynamicParams = new { id = id, purpuse = purpuse };

                    assigned = conn.Query<CompanyAssignedLearnerModel>(storedProc, dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return assigned;
        }

        public IEnumerable<Employeer> GetPreviousEmployers(int id)
        {
            IEnumerable<Employeer> company = new List<Employeer>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    var param = id;

                    string storedProc = "[dbo].[SP_GetPreviousEmployersForLearner]";
                    object dynamicParams = new { id = param };
                    company = conn.Query<Employeer>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return company;
        }
        public IEnumerable<Employeer> GetPreviousAssessments(int id)
        {
            IEnumerable<Employeer> company = new List<Employeer>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    var param = id;

                    string storedProc = "[dbo].[SP_GetPreviousAssessmentForLearner]";
                    object dynamicParams = new { id = param };
                    company = conn.Query<Employeer>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return company;
        }

        public IEnumerable<CompanyModel> GetCompaniesForOsca(string name)
        {
            IEnumerable<CompanyModel> companies = new List<CompanyModel>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Company_Get_For_Osca]";
                    object dynamicParams = new { name=name };

                    companies = conn.Query<CompanyModel>(storedProc, dynamicParams,  commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return companies;
        }

        public bool UpdateAssignedLearnerOnContactChange(int company_id, int learner_id, int contact_id, int? wp_id)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_ChangeEmployerContactForLearner]";
                    object dynamicParams = new { @company_id  = company_id, @learner_id= learner_id, contact_id= contact_id, wp_id =wp_id };

                    conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return true;
        }

        public IEnumerable<CompanyAssignedLearnerModel> GetLearnerByCompanyId(int id)
        {
            IEnumerable<CompanyAssignedLearnerModel> assigned = new List<CompanyAssignedLearnerModel>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Assigned_Learner_By_Company_Id]";
                    object dynamicParams = new { id = id };

                    assigned = conn.Query<CompanyAssignedLearnerModel>(storedProc, dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return assigned;
        }

        public IEnumerable<CompanyModel> CheckCompanyForCrm(string name, string postcode)
        {
            IEnumerable<CompanyModel> assigned = new List<CompanyModel>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Company_CheckDuplicate_For_Crm]";
                    object dynamicParams = new { name = name,postcode =postcode};

                    assigned = conn.Query<CompanyModel>(storedProc, dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return assigned;
        }


        //public IEnumerable<leads.Models.CompanyGroup> GetAllCompanyGroup()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
