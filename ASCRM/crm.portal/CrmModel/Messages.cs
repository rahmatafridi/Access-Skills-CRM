using ds.portal.companies.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace crm.portal.CrmModel
{
  public class Messages
    {
        public int id { get; set; }
        public int topic_id { get; set; }
        public string topic { get; set; }
        public string subject { get; set; }
        public string message { get; set; }
        public DateTime created_date { get; set; }
        public int user_id { get; set; }
        public string user_name { get; set; }
        public int learner_id { get; set; }
        public int is_question { get; set; }
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
  
  public class MessageDetail
    {
        public int main_id { get; set; }
        public string message { get; set; }
        public DateTime created_date { get; set; }
        public string user_name { get; set; }
        public int user_id { get; set; }
        public int is_question { get; set; }

    }
}
