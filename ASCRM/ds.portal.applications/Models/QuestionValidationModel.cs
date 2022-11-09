using System;
using System.Collections.Generic;
using System.Text;

namespace ds.portal.applications.Models
{
   public class QuestionValidationModel
    {
        public int v_id { get; set; }

        private string _encodedId { get; set; }
        public string encodedId
        {
            get
            {
                return ui.Models.Encryption.Encrypt(this.v_id.ToString());
            }
            set
            {
                _encodedId = value;
            }
        }

        public string q_type_regex { get; set; }
        public string q_type_name { get; set; }
        public int q_max { get; set; }
        public int q_min { get; set; }
        public string q_validation_massage { get; set; }
    }
}
