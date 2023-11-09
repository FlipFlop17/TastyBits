using Newtonsoft.Json;

namespace Application.Common.ReturnModels;

public class CalorieNinjaApiResultModel
{
    [JsonProperty("items")]
    public List<CalorieNinjaApiResult> Items { get; set; }

    public class CalorieNinjaApiResult
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("calories")]
        public double CaloriesPer100 { get; set; }

        [JsonProperty("serving_size_g")]
        public double ServingSize_b { get; set; }

        [JsonProperty("fat_total_g")]
        public double FatTotal_g { get; set; }

        [JsonProperty("fat_saturated_g")]
        public double FatSaturated_g { get; set; }

        [JsonProperty("protein_g")]
        public double Protein_g { get; set; }

        [JsonProperty("sodium_mg")]
        public double Sodium_mg { get; set; }

        [JsonProperty("potassium_mg")]
        public double Potasium_mg { get; set; }

        [JsonProperty("cholesterol_mg")]
        public double Cholesterol_mg { get; set; }

        [JsonProperty("carbohydrates_total_g")]
        public double Carbs_g { get; set; }

        [JsonProperty("fiber_g")]
        public double Fiber_g { get; set; }

        [JsonProperty("sugar_g")]
        public double Sugar_g { get; set; }
    }
}
