using System;
using System.Collections.Generic;
using System.Text;

namespace ds.portal.applications.invites.Models
{
    public class AppInvites
    {
        public int api_id { get; set; }
        public int api_id_courselevel { get; set; }
        public string api_courseleveltitle { get; set; }
        public string api_email { get; set; }
        public string api_firstname { get; set; }
        public string api_lastname { get; set; }
        public string api_password { get; set; }
        public string api_emailbody { get; set; }
        public int api_createdbyuserid { get; set; }
        public string api_createdbyusername { get; set; }
        public DateTime api_datecreated { get; set; }
        public bool api_isactivated { get; set; }
        public bool api_hasapplications { get; set; }
        public int? api_updatedbyuserid { get; set; }
        public string api_updatedbyusername { get; set; }
        public DateTime? api_dateupdated { get; set; }
        public bool? api_isdeleted { get; set; }
        public int? api_deletedbyuserid { get; set; }
        public string api_deletedbyusername { get; set; }
        public DateTime api_datedeleted { get; set; }
        public int lead_id { get; set; }
    }
}
