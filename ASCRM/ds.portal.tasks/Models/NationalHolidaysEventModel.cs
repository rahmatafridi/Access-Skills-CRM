using Newtonsoft.Json;
using System;

namespace ds.portal.tasks.Models
{
    public class NationalHolidaysEventModel
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("notes")]
        public string Notes { get; set; }

        [JsonProperty("bunting")]
        public bool Bunting { get; set; }
    }
}