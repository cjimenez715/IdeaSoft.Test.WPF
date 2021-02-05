using IdeaSoft.Test.Desktop.UI.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace IdeaSoft.Test.Desktop.UI.Services
{
    public class PersonService : ServiceHandler, IPersonService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;

        public PersonService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _httpClient = _httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(configuration["ServerURL"]);
        }

        public async Task<ObservableCollection<SearchPersonDto>> GetPersonByFilterAsync(string filter)
        {
            var response = await _httpClient.GetAsync($"person/get-by-filter?filter={filter}");
            var res = CustomDeserializeObjectResponseAsync<List<SearchPersonDto>>(response);
            return await Task.Run(() => new ObservableCollection<SearchPersonDto>(res.Result));
        }

        public async Task<UpdatePersonDto> GetPersonByIdAsync(string id)
        {
            var response = await _httpClient.GetAsync($"person/get-by-id/{id}");
            var res = CustomDeserializeObjectResponseAsync<UpdatePersonDto>(response);
            return await Task.Run(() => (UpdatePersonDto)res.Result);
        }

        public async Task<bool> RemovePersonAsync(string id)
        {
            var response = await _httpClient.DeleteAsync($"person/{id}");
            return await Task.Run(() => true);
        }

        public async Task<string> SavePersonAsync(SavePersonDto person)
        {
            var personJson = JsonConvert.SerializeObject(person);
            var buffer = System.Text.Encoding.UTF8.GetBytes(personJson);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await _httpClient.PostAsync($"person/", byteContent);
            return await Task.Run(() => string.Empty);
        }

        public async Task<string> UpdatePersonAsync(string id, UpdatePersonDto person)
        {
            var personJson = JsonConvert.SerializeObject(person);
            var buffer = System.Text.Encoding.UTF8.GetBytes(personJson);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await _httpClient.PutAsync($"person/{id}", byteContent);
            var res = CustomDeserializeObjectResponseAsync<string>(response);
            return await Task.Run(() => string.Empty);
        }
    }
}
