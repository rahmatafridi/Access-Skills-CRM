using System;
using System.Collections.Generic;
using System.Text;
using CsvHelper.Configuration;

namespace ds.portal.companies.Models
{   
    public class CompanyModel
    {
        public int id { get; set; }

        
             private string _encodedId { get; set; }
        public string encodedId
        {
            get
            {
                return Encryption.Encrypt(this.id.ToString());
            }
            set
            {
                _encodedId = value;
            }
        }
        public string reference { get; set; }

        public string name { get; set; }

        public string contact_name1 { get; set; }

        public string contact_name2 { get; set; }

        public string address1 { get; set; }

        public string address2 { get; set; }

        public string address3 { get; set; }

        public string town { get; set; }

        public string county { get; set; }

        public string postcode { get; set; }

        public int? country_id { get; set; }

        public string tel1 { get; set; }

        public string tel2 { get; set; }

        public string mobile1 { get; set; }

        public string mobile2 { get; set; }

        public string fax1 { get; set; }

        public string fax2 { get; set; }

        public string email1 { get; set; }

        public string email2 { get; set; }

        public string website { get; set; }

        public int? rating { get; set; }

        public int? number_of_beds { get; set; }

        public int? number_of_employees { get; set; }

        public string edrs_number { get; set; }

        public bool is_active { get; set; }

        public bool is_deleted { get; set; }

        public bool? is_sponsor { get; set; }

        public bool is_levy_paying { get; set; }

        public int? company_group_id { get; set; }

        public int? ratings_id { get; set; }

        public int? sales_advisor_id { get; set; }
        public string sales_advisor { get; set; }

        public string cqc_rating { get; set; }

        public string cqc_capacity { get; set; }

        public string cqc_inspection_date { get; set; }

        public string cqc_last_update_date { get; set; }

        public bool? cqc_standard_1 { get; set; }

        public bool? cqc_standard_2 { get; set; }

        public bool? cqc_standard_3 { get; set; }

        public bool? cqc_standard_4 { get; set; }

        public bool? cqc_standard_5 { get; set; }

        public int? created_by { get; set; }

        public DateTime created_date { get; set; }

        public DateTime? modified_date { get; set; }

        public int? modified_by { get; set; }

        public DateTime? deleted_date { get; set; }

        public int? deleted_by { get; set; }
        public string company_number { get;set; }
        public string name_desision_maker { get; set; }

        public string cqc_registration_number { get; set; }
        public string business_type { get; set; }
        public string levy_employer { get; set; }
        public string no_of_employees { get; set; }
        public string company_house_registration_number { get; set; }
        public string employer_email_address { get; set; }
        public int? job_title { get; set; }
        public string insurance_no { get; set; }
        public string expiry_date { get; set; }
        public string LeadID { get; set; }

    }

    public class CompanyFileUpload
    {
        public string reference { get; set; }

        public string name { get; set; }
        public string contact_name1 { get; set; }

        public string address1 { get; set; }

        public string address2 { get; set; }

        public string address3 { get; set; }

        public string town { get; set; }

        public string county { get; set; }

        public string postcode { get; set; }
        public string tel1 { get; set; }

        public string mobile1 { get; set; }

        public string fax1 { get; set; }

        public string email1 { get; set; }

        public string website { get; set; }

        public int? rating { get; set; }

        public int? number_of_beds { get; set; }

        public int? number_of_employees { get; set; }

        public string edrs_number { get; set; }

        public int? company_group_id { get; set; }
        public string company_group { get; set; }
        public string sale_advisor { get; set; }
        public int? sales_advisor_id { get; set; }
        public bool contact_is_error { get; set; }
        public string contact_issues { get; set; }
        public string company_number { get; set; }
        public string name_desision_maker { get; set; }

        public string cqc_registration_number { get; set; }
        public string business_type { get; set; }
        public string levy_employer { get; set; }
        public string no_of_employees { get; set; }
        public string company_house_registration_number { get; set; }
        public string employer_email_address { get; set; }
        public int job_title { get; set; }
    }
    public class CompanyUploadRequiredOptions
    {
        public string OptHeader_Title { get; set; }
        public int Opt_Id { get; set; }
        public int Opt_Value { get; set; }
        public string Opt_Title { get; set; }
        public int Opt_Id_OptHeader { get; set; }


    }

    public sealed class ImportCompanyMap : ClassMap<CompanyFileUpload>
    {
        public ImportCompanyMap()
        {
            Map(l => l.name).Name("Company Name");
            Map(l => l.address1).Name("Address1");
            Map(l => l.address2).Name("Address2");
            Map(l => l.address3).Name("Address3");
            Map(l => l.town).Name("Town");
            Map(l => l.county).Name("County");
            Map(l => l.postcode).Name("Postcode");
            Map(l => l.business_type).Name("Business Type");
            Map(l => l.cqc_registration_number).Name("CQC Registration Number");
            Map(l => l.edrs_number).Name("EDRS Number");
            Map(l => l.levy_employer).Name("Levy Employer");
            Map(l => l.sale_advisor).Name("Sale Advisor");
            Map(l => l.rating).Name("CQC Rating");
            Map(l => l.number_of_employees).Name("Number of Employee");
            Map(l => l.company_group).Name("Company Group");
            Map(l => l.company_number).Name("Company ID");
            Map(l => l.tel1).Name("Telephone Number");
            Map(l => l.email1).Name("Company Email Address");
            Map(l => l.website).Name("Website link");
            Map(l => l.company_house_registration_number).Name("Company House Registration Number");
            Map(l => l.employer_email_address).Name("Employer Email Address");
            Map(l => l.mobile1).Name("Phone Number");
            //Map(l => l.job_title).Name("Job Title");
        

        }
    }
    public static class Encryption
    {
        public static string Encrypt(string clearText)
        {
            try
            {
                return Convert.ToBase64String(Encoding.Unicode.GetBytes(clearText));
            }
            catch
            {
                return "";
            }
        }

        public static string Decrypt(string cipherText)
        {
            try
            {
                return Encoding.Unicode.GetString(Convert.FromBase64String(cipherText));
            }
            catch
            {
                return "";
            }
        }
    }
    public class PageParameters
    {
        const int maxPageSize = 50;
        public int PageNumber { get; set; } = 1;
        private int _pageSize = 10;
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }
    }
    public class CompanySearch
    {
        public string name { get; set; }
        public int id { get; set; }
        public string sa { get; set; }
        public string let { get; set; }
        public string RowNo { get; set; }
        public List<CompanyModel> models { get; set; }
        private string _encodedId { get; set; }
        public string encodedId
        {
            get
            {
                return Encryption.Encrypt(this.id.ToString());
            }
            set
            {
                _encodedId = value;
            }
        }
    }

    public class DuplicateCompany
    {
        public int id { get; set; }
        public string name { get; set; }
        public string address1  { get; set; }
        public string postcode { get; set; }
        public string telephone { get; set; }
        public string type { get; set; }
    }
}
