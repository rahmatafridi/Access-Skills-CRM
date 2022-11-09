namespace ds.portal.leads.Models
{
    public class HistoryModel
    {
        public int history_action_id { get; set; }
        public string module { get; set; }
        public string action_type { get; set; }
        public int action_opt_id { get; set; }
        public int lead_id { get; set; }
        public int user_id { get; set; }
        public string date_time { get; set; }
        public bool is_deleted { get; set; }
        public string action_title { get; set; }

        public string username { get; set; }

    }
}
