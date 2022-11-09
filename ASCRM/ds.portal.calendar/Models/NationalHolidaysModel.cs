using Newtonsoft.Json;

namespace ds.portal.calendar.Models
{
    public class NationalHolidaysModel
    {
        [JsonProperty("england-and-wales")]
        public NationalHolidaysCountryModel EnglandAndWales { get; set; }

        [JsonProperty("scotland")]
        public NationalHolidaysCountryModel Scotland { get; set; }

        [JsonProperty("northern-ireland")]
        public NationalHolidaysCountryModel NorthenIreland { get; set; }
    }
}
