using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Web;
using climate_MVC.Models.DTO;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;

namespace climate_MVC.Classes
{
    public class ApiControl
    {
        string baseUrl = ConfigurationManager.AppSettings["baseUrl"];
        public async System.Threading.Tasks.Task<HttpResponseMessage> ApiLogin(tblUsersInsert objUsers)
        {
            try
            {
                using (var cliente = new HttpClient())
                {
                    cliente.BaseAddress = new Uri(baseUrl);
                    cliente.DefaultRequestHeaders.Clear();
                    cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    StringContent content = new StringContent(JsonConvert.SerializeObject(objUsers), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await cliente.PostAsync(serviceUrl.loginUrl,content);
                    return response;
                }
            }
            catch (Exception ex) 
            {
                throw ex;
            }
        }

        public async System.Threading.Tasks.Task<HttpResponseMessage> PostRequest(StringContent data, string token, string urlApi)
        {
            try
            {
                using (var client = new HttpClient())
                {

                    client.BaseAddress = new Uri(baseUrl);

                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = await client.PostAsync(urlApi, data);

                    return response;
                }
            }
            catch (Exception e) { throw e; }
        }

        public async System.Threading.Tasks.Task<HttpResponseMessage> GetRequest(string token, string urlApi)
        {

            try
            {
                using (var client = new HttpClient())
                {

                    client.BaseAddress = new Uri(baseUrl);

                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = await client.GetAsync(urlApi);

                    return response;
                }
            }
            catch (Exception e) { throw e; }
        }
    }
}