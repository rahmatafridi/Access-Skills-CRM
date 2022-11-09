using ds.portal.ui.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ds.portal.applications.Models
{
    public class ApplicationQuestions
    {
        public int q_id { get; set; }
        public int a_id { get; set; }

        public int app_id { get; set; }
        public int q_number { get; set; }
        public string q_title { get; set; }
        public int q_sortorder { get; set; }
        public int? q_id_appstep { get; set; }
        public int? q_id_appsection { get; set; }
        public int q_type { get; set; }
        public string q_optheader_title { get; set; }
        public int q_maxlength { get; set; }
        public int q_minlength { get; set; }
        public int q_id_dependency { get; set; }
        public bool q_is_mandatory { get; set; }
        public string q_mapname { get; set; }
        public string q_class { get; set; }
        public int q_courselevelId { get; set; }
        public string answer { get; set; }
        public string q_opt_header { get; set; }
        public IEnumerable<dynamic> q_ddldata { get; set; }
        public IEnumerable<dynamic> q_ddlMultiCheck { get; set; }

        public int q_is_admin { get; set; }
        public byte[] bin_answer { get; set; }
        public string q_file_extension { get; set; }
        public int q_min { get; set; }
        public int q_mix { get; set; }
        public int q_validation_id { get; set; }
        public bool disabled { get; set; }
        public string q_depend_yesno { get; set; }
        public int q_is_enrolled { get; set; }
        public int q_is_confirm { get; set; }
        public string q_html_lable { get; set; }

    }

    public class MultiCheck
    {
        public  string Opt_Value { get; set; }
        public  string Opt_Title { get; set; }
        public  int Opt_Id_OptHeader { get; set; }
        public int  Check_Value { get; set; }
      

    }

    public class ApplicationSignature
    {
        public int appsig_id { get; set; }
        public int appsig_id_app { get; set; }
        public DateTime appsig_createdon { get; set; }
        public string appsig_username { get; set; }
        public string app_sig_text { get; set; }
    }
    public class QuestionModel {
        public int id { get; set; }
        public int q_id { get; set; }

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
        public int number { get; set; }
        public int sortOrder { get; set; }
        public string title { get; set; }
        public string q_title { get; set; }
        public int? q_id_appstep { get; set; }
        public int? q_id_appsection { get; set; }
        public int sortorder { get; set; }
        public string step { get; set; }
        public string section { get; set; }
        public string type { get; set; }
        public string optheader_title { get; set; }
        public string q_optheader_title { get; set; }
        public string q_type { get; set; }

        public int q_maxlength { get; set; }
        public int q_minlength { get; set; }
        public int q_id_dependency { get; set; }
        public string q_depend_yesno { get; set; }
        public bool q_is_mandatory { get; set; }
        public string q_mapname { get; set; }
        public string q_class { get; set; }
        public string courseLevel { get; set; }
        public bool q_isfuture { get; set; }
        public string answer { get; set; }
        public int q_is_admin { get; set; }
        public int q_min { get; set; }
        public int q_mix { get; set; }
        public int q_validation_id { get; set; }
        public int q_is_enrolled { get; set; }
        public int q_is_confirm { get; set; }
        public string q_html_lable { get; set; }
        public byte[] bin_answer { get; set; }

    }
    public class CourseQuestion
    {
        public int qc_id { get; set; }
        public int qc_q_number { get; set; }
        public int qc_id_courselevel { get; set; }
    }

    public class CourseLevels
    {
        public int CL_Id { get; set; }
        private string _encodedId { get; set; }
        public string encodedId
        {
            get
            {
                return Encryption.Encrypt(this.CL_Id.ToString());
            }
            set
            {
                _encodedId = value;
            }
        }
        public string CL_Title { get; set; }
        public string CL_Description { get; set; }
        public int CL_SortOrder { get; set; }
        public string CL_Code { get; set; }
        public string CL_Level { get; set; }
    }

    public class QuestionType
    {
        public int Id { get; set; }
        public string Name { get; set; }

    }

    public class SaveAnswer
    {
        public string q_mapname { get; set; }
        public string q_answer { get; set; }
        public string q_file { get; set; }
        public string q_file_extention { get; set; }
        public string text { get; set; }
    }
    public class SubmitAnswerModel
    {
        public List<SaveAnswer> SaveAnswers { get; set; }
        public string Sign { get; set; }
        public int AppId { get; set; }
    }
    public class ApplicationUser
    {
        public int appuser_id { get; set; }
        public int appuser_courselevel_id { get; set; }
        public int appuser_istrackchange { get; set; }
        public int appuser_issubmitted { get; set; }
    }
    public class ApplicationVM
    {
        public int app_id { get; set; }
        public bool isApp { get; set; }
        public string encoded_app_id
        {
            get
            {
                return Encryption.Encrypt(this.app_id.ToString());
            }
            set
            {
                encoded_app_id = value;
            }
        }
    }
}
