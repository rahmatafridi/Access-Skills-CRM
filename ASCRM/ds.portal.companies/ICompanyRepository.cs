using ds.portal.companies.Models;
using ds.portal.leads.Models;
using System;
using System.Collections.Generic;
using CompanyContact = ds.portal.companies.Models.CompanyContact;

namespace ds.portal.companies
{
    public interface ICompanyRepository
    {
        IEnumerable<CompanyModel> GetCompanies(int? security_user_id, DateTime? startDate, DateTime? endDate);
        IEnumerable<CompanyModel> GetCompaniesForOsca( string name);

        bool DeleteCompanyById(int id, int userId, string userName);
        CompanyModel InsertUpdateCompanyRecord(CompanyModel company);
        CompanyModel GetCompanyById(int id);
        IEnumerable<CompanyModel> CompanySearch(string id,string name,int? sale_id);
        IEnumerable<CompanySearch> CompanySearchAtZ(string name,string searchtype);
        IEnumerable<CompanySearch> CompanySearchByWord(string name);

        IEnumerable<CompanyModel> CompanyInSearch(string name);
        IEnumerable<CompanyWorkplaces> CompanyWorkPlaceInSearch(string name,int company_id);

        IEnumerable<CompanyModel> CompanySearchByName(string name);

        IEnumerable<DuplicateCompany> CheckDuplicateCompany(int id,string name, string postCode, int userId,string address, string tel);
        IEnumerable<DuplicateCompany> CheckDuplicateCompanyPostcode(string postCode,string name);

        IEnumerable<CompanyNote> GetAllCompanyNotes(int company_Id);
        IEnumerable<CompanyContact> GetAllCompanyContacts(int company_Id);
        IEnumerable<CompanyContact> GetAllWorkplaceContacts(int company_id ,int wp_id);
        IEnumerable<CompanyAssignedLearnerModel> GetAllAssignedLearner(int company_id);
        IEnumerable<CompanyAssignedLearnerModel> GetAllAssignedLearnerById(int id, string purpuse);

        IEnumerable<CompanyWorkplaces> GetAllCompanyWorkplaceses(int company_Id);

        CompanyNote GetNoteById(int id);
        CompanyContact GetCompanyContactById(int id);
        CompanyWorkplaces GetCompanyWorkplaceById(int id);

        bool InsertUpdateCompanyNoteRecord(CompanyNote note);
        bool InssertUpdateAssignLearnerAssessment(CompanyAssignedLearnerModel model);
        bool InssertUpdateAssignLearnerEmployer(CompanyAssignedLearnerModel model);

        bool InsertUpdateCompanyContactRecord(CompanyContact contact);
        List<CompanyFileUpload> ValidateCompanyForImport(List<CompanyFileUpload> companyFileUploads);

        bool DeleteNoteById(int id, int userId, string userName);
        bool DeleteCompanyContactById(int id, int userId, string userName);
        bool DeleteCompanyWorkPlaceById(int id, int userId, string userName);

        bool InsertUpdateCompanyWorkPlaceRecord(CompanyWorkplaces workplaces);
       
        List<CompanyFileUpload> BulkInsert(List<CompanyFileUpload> companyFileUploads, int userId);
        IEnumerable<LeadViewModel> GetLeadByCompany(int companyId,int userId);

        Employeer GetEmployeerByLearner(int id);
        Employeer GetAssessmentByLearner(int id);

        IEnumerable<Employeer> GetPreviousEmployers(int id);
        IEnumerable<Employeer> GetPreviousAssessments(int id);

        bool UpdateAssignedLearnerOnContactChange(int company_id, int learner_id, int contact_id, int? wp_id);
        IEnumerable<CompanyAssignedLearnerModel> GetLearnerByCompanyId(int id);
        IEnumerable<CompanyModel> CheckCompanyForCrm(string name, string postcode);

    }

}
