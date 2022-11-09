using System;
using System.Collections.Generic;
using System.Text;

namespace portal.models.Comman
{
   public class QueueEmails
    {
        public int qe_id { get; set; }
        public string qe_from { get; set; }
        public string qe_to { get; set; }
        public string qe_subject { get; set; }
        public string qe_body { get; set; }
        public bool qe_is_html { get; set; }
        public string qe_cc { get; set; }
        public string qe_attachments { get; set; }
        public int qe_priority { get; set; }
        public bool qe_sent { get; set; }
        public string qe_status { get; set; }
        public string qe_created_by { get; set; }
        public DateTime qe_created_date { get; set; }
        public DateTime qe_mod_date { get; set; }
        public string qe_bcc { get; set; }
        public int qe_system_id { get; set; }
    }
}
