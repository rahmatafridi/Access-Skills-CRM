using ds.portal.ui.Models;
using System;

namespace ds.portal.dashboard.Models
{
    public class MyLeadsModel
    {
        public int Lead_Id { get; set; }
        private string _encodeLeadId { get; set; }
        public string encodeLeadId
        {
            get
            {
                return Encryption.Encrypt(this.Lead_Id.ToString());
            }
            set
            {
                _encodeLeadId = value;
            }
        }
        public int SalesAdvisorId { get; set; }
        public string ContactName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string AssignedTo { get; set; }
        public string CourseTitle { get; set; }
        public string DateOfEnquiry { get; set; }
        public string EnquiryDate { get; set; }
        public string Status { get; set; }
        public int Status_Id { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
        public string Lead_Status_Class { get; set; }
 
    }
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
        public string company_number { get; set; }
        public string name_desision_maker { get; set; }
        public string sales_advisor { get; set; }

    }

    public class CompanyWorkplaces
    {
        public int wp_id { get; set; }

        public int? company_id { get; set; }
        private string _encodedId { get; set; }
        public string encodedId
        {
            get
            {
                return Encryption.Encrypt(this.company_id.ToString());
            }
            set
            {
                _encodedId = value;
            }
        }
        public string wp_name { get; set; }

        public string address1 { get; set; }

        public string address2 { get; set; }

        public string post_code { get; set; }

        public int? created_by { get; set; }

        public DateTime? created_date { get; set; }

        public int? updated_by { get; set; }

        public DateTime? updated_date { get; set; }

        public int? deleted_by { get; set; }

        public DateTime? deleted_date { get; set; }
        public bool? is_deleted { get; set; }
        public string TotalContact { get; set; }

        public string town { get; set; }
        public string county { get; set; }
        public string employee_email { get; set; }
        public string phone_number { get; set; }
        public int? job_title { get; set; }
        public string job_Title { get; set; }
        public string business_type { get; set; }
        public string company_name { get; set; }
    }
    public class CompanyContact
    {
        public int contact_id { get; set; }

        public int company_id { get; set; }
        private string _encodedId { get; set; }
        public string encodedId
        {
            get
            {
                return Encryption.Encrypt(this.company_id.ToString());
            }
            set
            {
                _encodedId = value;
            }
        }

        public int? wp_id { get; set; }

        public string contact_name { get; set; }

        public string title { get; set; }

        public string job_title { get; set; }

        public string email1 { get; set; }

        public string email2 { get; set; }

        public string mobile1 { get; set; }

        public string address1 { get; set; }

        public string address2 { get; set; }

        public string address3 { get; set; }

        public string town { get; set; }

        public string county { get; set; }

        public string postcode { get; set; }

        public int? country_id { get; set; }

        public string website { get; set; }

        public bool is_deleted { get; set; }

        public int? created_by { get; set; }

        public DateTime created_date { get; set; }

        public DateTime? modified_date { get; set; }

        public int? modified_by { get; set; }

        public DateTime? deleted_date { get; set; }

        public int? deleted_by { get; set; }
    }

}
