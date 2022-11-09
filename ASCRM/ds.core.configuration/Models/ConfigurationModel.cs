using System;

namespace ds.core.configuration.Models
{
    public sealed class ConfigurationModel
    {
        public int config_id { get; set; }
        public string config_key { get; set; }
        public string config_value { get; set; }
        public string config_description { get; set; }
    }

    public class AuditModel
    {
        public int audit_Id { get; set; }
        public int user_Id { get; set; }
        public string action_url { get; set; }
        public string action_method { get; set; }
        public string ip_address { get; set; }
        public DateTime date_time { get; set; }
    }

    public class HistoryModel
    {
        public int history_Id { get; set; }
        public int user_Id { get; set; }
        public int role_Id { get; set; }
        public string ip_address { get; set; }
        public DateTime date_time { get; set; }
        public string Audit { get; set; }
        public string url { get; set; }
    }
}
