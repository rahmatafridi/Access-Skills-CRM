using ds.portal.ui.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ds.portal.applications.Models
{
    public class Application
    {
        public string encoded_app_id
        {
            get
            {
                return Encryption.Encrypt(this.app_id.ToString());
            }
            //set
            //{
            //    encoded_app_id = value;
            //}
        }
        public int app_id { get; set; }
        public string app_title { get; set; }
        public string app_firstname { get; set; }
        public string app_surname { get; set; }
        public string app_gender { get; set; }
        public string app_dob { get; set; }
        //public DateTime? app_dob { get; set; }
        public string app_ni { get; set; }
        public string app_permaddress1 { get; set; }
        public string app_permaddress2 { get; set; }
        public string app_permaddress3 { get; set; }
        public string app_permaddress4 { get; set; }
        public string app_permaddress5 { get; set; }
        public string app_permpostcode { get; set; }
        public string app_email { get; set; }
        public string app_hometel { get; set; }
        public string app_mobile { get; set; }
        public string app_nationality { get; set; }
        public string app_legalresidency { get; set; }
        public string app_islivedeulast3years { get; set; }
        public string app_livedentrydate { get; set; }
        //public DateTime? app_livedentrydate { get; set; }
        public string app_ethnicity { get; set; }
        public string app_religion { get; set; }
        public string app_sexualorientation { get; set; }
        public string app_anydisability { get; set; }
        public string app_needlearningsupport { get; set; }
        public string app_havelearnersupportprogram { get; set; }
        public string app_programappliedfor { get; set; }
        public string app_isaccesstocomputer { get; set; }
        public string app_isaccesstofacetime { get; set; }
        public string app_isaccesstoemail { get; set; }
        public string app_iseportoflioaware { get; set; }
        public string app_manageworkstudy { get; set; }
        public string app_educ_highestqual { get; set; }
        public string app_educ_isgcseenglish { get; set; }
        public string app_educ_isgcsemath { get; set; }
        public string app_educ_isenrolledother { get; set; }
        public string app_educ_previouscollege { get; set; }
        public string app_educ_previousqual { get; set; }
        public string app_educ_previoustraining { get; set; }
        public string app_educ_trainingplannednext12months { get; set; }
        public string app_educ_futureinspirations { get; set; }
        public string app_emp_companyname { get; set; }
        public string app_emp_workplaceaddress { get; set; }
        public string app_emp_workplaceaddress1 { get; set; }
        public string app_emp_workplaceaddress2 { get; set; }
        public string app_emp_workplaceaddress3 { get; set; }
        public string app_emp_workplaceaddress4 { get; set; }
        public string app_emp_workplaceaddress5 { get; set; }
        public string app_emp_workplacepostcode { get; set; }
        public string app_emp_email { get; set; }
        public string app_emp_tel { get; set; }
        public string app_emp_contactname { get; set; }
        public string app_emp_position { get; set; }
        public string app_emp_businesssector { get; set; }
        public string app_emp_companyestablished { get; set; }
        public string app_emp_weeklyhours { get; set; }
        public string app_emp_isselfemployed { get; set; }
        public string app_emp_isemployerpaying { get; set; }
        public string app_job_jobfunction { get; set; }
        public string app_job_howlongworkingjob { get; set; }
        public string app_job_howlongworkingemployer { get; set; }
        public string app_job_anypreviousmanagement { get; set; }
        public string app_job_havecurrentdevplan { get; set; }
        public string app_job_isawarefundamentalstd { get; set; }
        public string app_job_isresponsiblecqcpir { get; set; }
        public string app_job_isresponsiblerecruitment { get; set; }
        public string app_job_isresponsiblestaffinduction { get; set; }
        public string app_job_isresponsiblestaffappraisal { get; set; }
        public string app_job_isresponsiblemonitoringstaff { get; set; }
        public string app_job_isresponsibletaskallocationrotas { get; set; }
        public string app_job_isresponsibleplanningreviewing { get; set; }
        public string app_job_isresponsibleorganisingreferrals { get; set; }
        public string app_job_isresponsibleorganisingpartnerships { get; set; }
        public string app_job_isresponsibleeffectivenesspartnerships { get; set; }
        public string app_job_isreviewauditpolicies { get; set; }
        public string app_job_isrespondingtocomplaints { get; set; }
        public string app_job_isinvestigatingsafeguarding { get; set; }
        public string app_job_isauditfeedback { get; set; }
        public string app_job_isresponsiblewritingdevplan { get; set; }
        public string app_job_isorganisingleadstaffmeetings { get; set; }
        public string app_job_haveregularstaffmeetings { get; set; }
        public string app_job_isattendingstrategicmeetings { get; set; }
        public string app_job_isensuringcompliancehs { get; set; }
        public string app_job_isfurthertrainingneeded { get; set; }
        public string app_job_isspecificsupportneeded { get; set; }
        public string app_job_relevantpathway { get; set; }
        public string app_job_havejobdescription { get; set; }
        public string app_job_dailymainduties { get; set; }
        public string app_job_usualhoursattendancy { get; set; }
        public string app_job_otherpositionresponsabilities { get; set; }
        public string app_job_aboutyourstrenghts { get; set; }
        public string app_job_areasofdevelopment { get; set; }
        public string app_officeuse_uniquelearnerreference { get; set; }
        public DateTime? app_officeuse_startdate { get; set; }
        public string app_officeuse_startdate_str { get; set; }
        public DateTime? app_officeuse_enddate { get; set; }
        public string app_officeuse_enddate_str { get; set; }
        public string app_officeuse_apprenticeshipframework { get; set; }
        public int? app_officeuse_learnerid { get; set; }
        public string app_officeuse_referenceid { get; set; }
        public DateTime? app_officeuse_cqcinspectiondate { get; set; }
        public string app_officeuse_cqcinspectiondate_str { get; set; }
        public string app_officeuse_ukprn { get; set; }
        public string app_officeuse_employerid { get; set; }
        public byte? app_officeuse1_isliteracynumeracydone { get; set; }
        public byte? app_officeuse1_isalldocumentssigned { get; set; }
        public byte? app_officeuse1_isconfirmenrolment { get; set; }
        public int? app_officeuse1_courseid { get; set; }
        public string app_ispartnerofowner { get; set; }
        public string app_educ_equivalentqualification { get; set; }
        public string app_lengthofaddress { get; set; }
        public string app_preplannedabsence { get; set; }
        public string app_emp_empoyementstartdate { get; set; }
        public string app_job_carryoutauditing { get; set; }
        public string app_anydisabilityprimary { get; set; }
        public string app_circumstance { get; set; }
        public string app_anydisabilitysecondaries { get; set; }
        public string app_officeuse1_coursetitle { get; set; }
        public int? app_officeuse1_courselevel { get; set; }
        public string app_job_isassessreviewimplementcare { get; set; }
        public string app_job_isinvolvedriskassessment { get; set; }
        public string app_job_isensureothersfollowpolicy { get; set; }
        public string app_job_issupportserviceinpersonalcare { get; set; }
        public string app_job_isworksupportroleserviceusers { get; set; }
        public string app_job_isplanreviewimplmentcare { get; set; }
        public string app_job_isinvolvedsafeguardinginvestigations { get; set; }
        public string app_job_istakepartinriskassessment { get; set; }
        public string app_job_istakepartinmanagingquality { get; set; }
        public string app_job_relevantpathway_l3 { get; set; }
        public string app_job_havejobdescription_l3 { get; set; }
        public string app_mktg_hearabout { get; set; }
        public string app_mktg_contactconsent { get; set; }
        public string app_mktg_byphone { get; set; }
        public string app_mktg_byemail { get; set; }
        public string app_mktg_bypost { get; set; }
        public string app_job_havejobdescription_l3_doc { get; set; }
        public string app_legalresidency_doc { get; set; }
        public string app_job_havejobdescription_doc { get; set; }
        public string app_noneuukhowlongliveinuk { get; set; }
        public string app_educ_isallfund { get; set; }
        public string app_educ_allfundqualdetails { get; set; }
        public string app_job_allowworkplaceobsvisit { get; set; }
        public string app_job_havestartedglh { get; set; }
        public byte? app_job_confirm16hrs { get; set; }
        public DateTime? app_officeuse_referencedate { get; set; }
        public string app_officeuse_referencedate_str { get; set; }
        public byte? app_officeuse_isevidenceseen { get; set; }
        public int? app_officeuse_fundingid { get; set; }
        public string app_officeuse_fundingtitle { get; set; }
        public string app_officeuse_referenceidtype { get; set; }
        public int? app_job_confirm16hrsid { get; set; }
        public string app_job_confirm16hrstitle { get; set; }
        public string app_printname { get; set; }
        public string app_applicationdate { get; set; }
        public string app_qn43 { get; set; }
        public string app_qn44 { get; set; }
        public string app_qn45 { get; set; }
        public string app_qn46 { get; set; }
        public string app_qn47 { get; set; }
        public string app_qn48 { get; set; }
        public string app_qn49 { get; set; }
        public string app_qn50 { get; set; }
        public string app_qn51 { get; set; }
        public string app_qn52 { get; set; }
        public string app_qn53 { get; set; }
        public string app_qn54 { get; set; }
        public string app_qn55 { get; set; }
        public string app_qn56 { get; set; }
        public string app_qn57 { get; set; }
        public string app_qn58 { get; set; }
        public string app_qn59 { get; set; }
        public string app_qn60 { get; set; }
        public string app_qn61 { get; set; }
        public int? app_advisoruserid { get; set; }
        public bool? app_isamended { get; set; }
        public string app_acs_wdsnumber { get; set; }
        public string dataUri { get; set; }
        public string Username { get; set; }
        public byte[] Signature { get; set; }
    }
}
