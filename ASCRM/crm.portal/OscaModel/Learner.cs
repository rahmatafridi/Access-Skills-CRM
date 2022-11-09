using ds.portal.ui.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace crm.portal.OscaModel
{
    public class Learner
    {
        public int Users_Id_LearnerContacts { get; set; }
        public string LearnerName { get; set; }
        public int Id { get; set; }
        public int Learner_ID { get; set; }
        private string _encodedId { get; set; }
        public string encodedId
        {
            get
            {
                return Encryption.Encrypt(this.Learner_ID.ToString());
            }
            set
            {
                _encodedId = value;
            }
        }
        public string Titles_Title { get; set; }
        public string Learner_FirstName { get; set; }
        public string Learner_Surname { get; set; }
        public string CourseName { get; set; }
        public string CurrentLevel { get; set; }
        public string Status { get; set; }
        public string CandidateStatus { get; set; }
        public string CandidateStatus_BackColor { get; set; }
        public string Learner_Class { get; set; }
        public string Percentage { get; set; }
        public string IsAssigned { get; set; }
        public int LearnerCourseId { get; set; }
        public string Employer_PostCode1 { get; set; }
        public string IsHidden { get; set; }

    }
    public class LinkLearnerABTPUser
    {
        public int Link_Learner_AB_TP_Id { get; set; }
        public int AB_TP_Id_User { get; set; }
        public int AB_TP_Id_Learner { get; set; }
    }
    public class EmployerLearner
    {
        public int Learner_Id { get; set; }
        public int User_Id { get; set; }
    }
    public class CourseSummaryLearnerNotes
    {

        public int NoteId { get; set; }
        public string LearnerNotes_Note { get; set; }
        public string NoteDate { get; set; }
        public string NoteDateTime { get; set; }

    }
    public class LearnerList
    {
       


    }
}