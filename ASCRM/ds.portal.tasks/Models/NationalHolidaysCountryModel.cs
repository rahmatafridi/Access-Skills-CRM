using Newtonsoft.Json;
using System.Collections.Generic;

namespace ds.portal.tasks.Models
{
    public class NationalHolidaysCountryModel
    {
        [JsonProperty("division")]
        public string Division { get; set; }

        [JsonProperty("events")]
        public List<NationalHolidaysEventModel> Events { get; set; } 
    }
}
