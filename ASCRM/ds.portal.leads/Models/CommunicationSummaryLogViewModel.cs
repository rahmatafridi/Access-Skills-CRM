using System;

namespace ds.portal.leads.Models
{
    public class CommunicationSummaryLogViewModel
    {
        public int CSL_Id { get; set; }

        public int CSL_Id_Lead { get; set; }

        public int CSL_Id_OptHeader { get; set; }

        public int CSL_Id_Outcome { get; set; }

        public string CSL_Outcome { get; set; }

        public string CSL_Date { get; set; }
      
        public string CSL_Date_str { get; set; }

        public string CSL_Note { get; set; }

        public int? CSL_CreatedByUserId { get; set; }

        public string CSL_CreatedByUserName { get; set; }

        public DateTime CSL_DateCreated { get; set; }

        public int? CSL_UpdatedByUserId { get; set; }

        public string CSL_UpdatedByUserName { get; set; }

        public DateTime? CSL_DateUpdated { get; set; }

        public bool? CSL_IsDeleted { get; set; }

        public int? CSL_DeletedByUserId { get; set; }

        public string CSL_DeletedByUserName { get; set; }

        public DateTime? CSL_DateDeleted { get; set; }

    }

}
