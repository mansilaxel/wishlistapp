using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace wishListClient.Services
{
    class UserService
    {
        public string baseUrl { get; set; }

        public UserService()
        {
            this.baseUrl = "http://localhost:51656/api";
        }

        public User AuthenticateUser(string email, string password)
        {
            string endpoint = this.baseUrl + "/users/login";
            //string method = "POST";
            var stringContent = new StringContent(JsonConvert.SerializeObject(new
            {
                username = email,
                password = password
            }));
            HttpClient httpClient = new HttpClient();
            //WebClient wc = new WebClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            //wc.Headers["Content-Type"] = "application/json";
            try
            {
                var response = httpClient.PostAsync(endpoint, stringContent);
                //string response = wc.UploadString(endpoint, method, json);
                return JsonConvert.DeserializeObject<User>(response.ToString());
            }
            catch (Exception)
            {
                return null;
            }
        }
        public User GetUserDetails(User user)
        {
            string endpoint = this.baseUrl + "/users/" + user.Id;
            string access_token = user.access_token;

            HttpClient httpClient = new HttpClient();
            //WebClient wc = new WebClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(access_token);
            //wc.Headers["Authorization"] = access_token;
            try
            {
                //string response = wc.DownloadString(endpoint);
                var response = httpClient.GetAsync(endpoint);
                user = JsonConvert.DeserializeObject<User>(response.ToString());
                user.access_token = access_token;
                return user;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public User RegisterUser(string firstname, string secondname, string email, string password)
        {

            using(HttpClient httpClient = new HttpClient())
            {
                string endpoint = this.baseUrl + "/users";
                var stringContent = new StringContent(JsonConvert.SerializeObject(new
                {
                    email = email,
                    password = password,
                    firstname = firstname,
                    secondname = secondname
                }), Encoding.UTF8, "application/json");

                var response = httpClient.PostAsync(endpoint, stringContent).Result;
                //SendRequestAsync(endpoint, stringContent);
                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    User user = JsonConvert.DeserializeObject<User>(data);
                    return user;
                }
                return null;
            }            
        }
    }
}
