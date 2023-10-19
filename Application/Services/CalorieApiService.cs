using Application.ReturnModels;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace Application.Services
{
    public class CalorieApiService
    {
        private const string url = "https://api.calorieninjas.com/v1/nutrition?query=";
        private string apiKey;
        private HttpClient httpClient;

        public CalorieApiService(IConfiguration config)
        {
            apiKey = config.GetRequiredSection("CalorieNinjasApiKey").Value;
        }

        public async Task<CalorieNinjaApiResultModel> GetCalorieAsync(string itemWithCalories)
        {
            InitClient();
            CalorieNinjaApiResultModel ninjaResult = new CalorieNinjaApiResultModel();
            HttpResponseMessage response = await httpClient.GetAsync(url + itemWithCalories);
            if (response.IsSuccessStatusCode) {
                string resBody = await response.Content.ReadAsStringAsync();
                ninjaResult = JsonConvert.DeserializeObject<CalorieNinjaApiResultModel>(resBody);

                //Log.Information("calories: "+ resBody);
                //Log.Information("calories: " + ninjaResult.Items.FirstOrDefault().CaloriesPer100);
            }
            return ninjaResult;
        }
        private void InitClient()
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(url);

            httpClient.DefaultRequestHeaders.Add("X-Api-Key", apiKey);
            httpClient.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
