using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace RealworldOneBackendTest.Services
{
    public class CatService : ICatService
    {
        private readonly HttpClient _httpClient;

        public CatService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<byte[]> GetCat()
        {
            var resposne = await _httpClient.GetAsync("/cat");

            byte[] byteArray = Array.Empty<byte>();

            try
            {
                if (resposne.IsSuccessStatusCode)
                {
                    var responseBody = await resposne.Content.ReadAsByteArrayAsync();

                    return responseBody;
                }

                return byteArray;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
