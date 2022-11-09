using System;

namespace ds.portal.leads.Models
{
    public sealed class LeadViewModel : Lead
    {
        public string Lead_Status { get; set; }
        public string Lead_Status_Class { get; set; }
        public string Lead_LastResult { get; set; }
        public DateTime? Lead_DateOfEnquiry_D { get; set; }
        public string Lead_Last_Action { get; set; }

        public string AgreedPaymentSchemeId { get; set; }
    }

    public sealed class DuplicateLeadViewModel : Lead
    {
        public string Lead_Status { get; set; }
        public string Lead_Status_Class { get; set; }
        public string Lead_LastResult { get; set; }
        public DateTime? Lead_DateOfEnquiry_D { get; set; }
        public string Lead_Last_Action { get; set; }

        public string AgreedPaymentSchemeId { get; set; }
    }
}
