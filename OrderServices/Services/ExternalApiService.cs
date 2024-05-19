using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using OrderServices.Dtos;
using Newtonsoft.Json;
using OrderServices.Dtos;
using OrderServices.Dtos.Ingredients;
using Shared.PaymentModels;

namespace OrderServices.Services
{
    public class ExternalApiService(HttpClient httpClient)
    {
        private readonly HttpClient _httpClient = httpClient;

        public async Task<List<AvailableIngredientResponse>> GetAvailableIngredientResponseAsync(string apiUrl)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            

            List<AvailableIngredientResponse> availableIngredientResponse = JsonConvert.DeserializeObject<List<AvailableIngredientResponse>>(responseBody);
            return availableIngredientResponse;
        }
        public async Task<List<ItemQuantityIngredientResponse>> GetIngredientsOnItemsAndQuantityAsync(string apiUrl, List<ItemQuantityRequest> itemQuantityRequests)
        {
            var requestContent = new StringContent(JsonConvert.SerializeObject(itemQuantityRequests), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(apiUrl, requestContent);
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            List<ItemQuantityIngredientResponse> ingredientResponses = JsonConvert.DeserializeObject<List<ItemQuantityIngredientResponse>>(responseBody, options);

            return ingredientResponses;
        }
        public async Task<Payment> PostCreatePaymentAsync(string apiUrl,CreatePaymentRequest request)
        {
            var jsonContent = JsonConvert.SerializeObject(request);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(apiUrl, content);
            response.EnsureSuccessStatusCode();
            string resBody = await response.Content.ReadAsStringAsync();

            var paymentResponse = JsonConvert.DeserializeObject<Payment>(resBody);

            return paymentResponse;
        }
    }
}