using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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
                email = email,
                password = password
            }), Encoding.UTF8, "application/json");
            HttpClient httpClient = new HttpClient();
            //WebClient wc = new WebClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            //wc.Headers["Content-Type"] = "application/json";
            var response = httpClient.PostAsync(endpoint, stringContent).Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<User>(data);
            }
            return null;
        }
        public User GetUserDetails(User user)
        {
            string endpoint = this.baseUrl + "/users/" + user.Email;
            string access_token = user.access_token;

            HttpClient httpClient = new HttpClient();
            //WebClient wc = new WebClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", access_token.ToString());
            //wc.Headers["Authorization"] = access_token;           


            var response = httpClient.GetAsync(endpoint).Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<User>(data);
            }
            return null;
          
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
