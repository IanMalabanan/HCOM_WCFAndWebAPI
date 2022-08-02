using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using CTI.HI.Business.Entities.Notification;
using Newtonsoft.Json;
using RestSharp;

namespace CTI.HI.Business.Managers.External
{
    [Export(typeof(IExternalMessagingService))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class InfoBipManager : IExternalMessagingService2
    {
        //public bool SendEmailbyInfobip(EmailModel email)
        //{

        //    string username = "FILINVEST04";
        //    string password = "!P@ssw0rd123";

        //    byte[] concatenated = System.Text.ASCIIEncoding.ASCII.GetBytes(username + ":" + password);
        //    string header = System.Convert.ToBase64String(concatenated);

        //    HttpClient client = new HttpClient();
        //    client.BaseAddress = new Uri("https://api.infobip.com/");
        //    client.DefaultRequestHeaders.Accept.Clear();
        //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", header);
        //    var request = new MultipartFormDataContent();
        //    request.Add(new StringContent(email.From), "from");
        //    request.Add(new StringContent(email.To), "to");
        //    request.Add(new StringContent(email.Subject), "subject");
        //    request.Add(new StringContent(email.Body), "html");


        //    var response = client.PostAsync("email/1/send", request).Result;
        //    return true;
        //}



        //public bool SendSmsByInfobip(string msg, string recipient)
        //{
        //    string username = "FILINVEST04";
        //    string password = "!P@ssw0rd123";

        //    byte[] concatenated = System.Text.ASCIIEncoding.ASCII.GetBytes(username + ":" + password);
        //    string header = System.Convert.ToBase64String(concatenated);

        //    var client = new RestClient("https://vnz2m.api.infobip.com/sms/2/text/single");

        //    var request = new RestRequest(Method.POST);
        //    request.AddHeader("accept", "application/json");
        //    request.AddHeader("content-type", "application/json");
        //    request.AddHeader("authorization", "Basic " + header);
        //    request.AddParameter("application/json", "{\"from\":\"HCOM\", \"to\":\"" + recipient + "\",\"text\":\"" + msg + ".\"}", ParameterType.RequestBody);

        //    IRestResponse response = client.Execute(request);

        //    return true;
        //}

        private string _bearerToken;

        public async Task<string> GetTokenAsync(string username, string password)
        {
            var _url = "http://messaging.filinvest.com.ph/token";
            string apiResponse = "";

            using (var httpClient = new HttpClient())
            {
                var creds = new List<KeyValuePair<string, string>> {
                        new KeyValuePair<string, string>("username", username),
                        new KeyValuePair<string, string>("password", password)
                };

                var content = new FormUrlEncodedContent(creds);
                using (var response = await httpClient.PostAsync(_url, content))
                {
                    apiResponse = await response.Content.ReadAsStringAsync();
                }
            }

            return apiResponse;
        }
        public async Task UseBearerToken()
        {
            string username = "FILINVEST04";
            string password = "!P@ssw0rd123";
            _bearerToken = await GetTokenAsync(username, password);
        }

        public async Task<Tuple<bool, string[]>> SendSMSAsync(SMSModel sms)
        {
            try
            {
                sms.From = "FILINVEST";

                var _watch = System.Diagnostics.Stopwatch.StartNew();

                var _url = "http://messaging.filinvest.com.ph/api/InfoBip/sms";
                await UseBearerToken();

                if (string.IsNullOrEmpty(_bearerToken))
                    throw new TypeInitializationException("MessagingApiClient._bearerToken", new Exception("Bearer Token not initialized. Use .UseBearerToken(token)"));

                var myContent = JsonConvert.SerializeObject(sms);

                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization =
                         new AuthenticationHeaderValue("Bearer", _bearerToken);

                    using (var response = await httpClient.PostAsync(_url, byteContent))
                    {
                        var apiResponseX = await response.Content.ReadAsStringAsync();
                        var _response = JsonConvert.DeserializeObject<MessagingInfobipSmsResponse>(apiResponseX);
                         
                        return new Tuple<bool, string[]>(true, new string[] { _response.StatusCode.ToString() });
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public async Task<Tuple<bool, string[]>> SendSMSAsync2(SMSModel sms)
        //{

        //    string username = "FILINVEST04";
        //    string password = "!P@ssw0rd123";

        //    byte[] concatenated = System.Text.ASCIIEncoding.ASCII.GetBytes(username + ":" + password);
        //    string header = System.Convert.ToBase64String(concatenated);

        //    //var client = new RestClient("https://vnz2m.api.infobip.com/sms/2/text/advanced");
        //    var client = new RestClient("http://messaging.filinvest.com.ph/api/InfoBip/sms");

        //    var request = new RestRequest(Method.POST);
        //    request.AddHeader("accept", "application/json");
        //    request.AddHeader("content-type", "application/json");
        //    request.AddHeader("authorization", "Basic " + header);
        //    request.AddParameter("application/json", "{\"from\":\"HCOM\", \"to\":\"" + sms.To + "\",\"text\":\"" + sms.Message + ".\"}", ParameterType.RequestBody);

        //    IRestResponse response = client.Execute(request);



        //    return Tuple.Create(true, new string[] { response.Content });
        //}
        //public async Task<Tuple<bool, string[]>> SendSMSAsync(SMSModel sms)
        //{
        //    try
        //    {
        //        sms.From = "CTI";

        //        var _watch = System.Diagnostics.Stopwatch.StartNew();

        //        var _url = "http://messaging.filinvest.com.ph/api/InfoBip/sms";
        //        await UseBearerToken();
        //        string apiResponse = "";

        //        if (string.IsNullOrEmpty(_bearerToken))
        //            throw new TypeInitializationException("MessagingApiClient._bearerToken", new Exception("Bearer Token not initialized. Use .UseBearerToken(token)"));

        //        var myContent = JsonConvert.SerializeObject(sms);

        //        var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
        //        var byteContent = new ByteArrayContent(buffer);
        //        byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        //        using (var httpClient = new HttpClient())
        //        {
        //            httpClient.DefaultRequestHeaders.Authorization =
        //                 new AuthenticationHeaderValue("Bearer", _bearerToken);

        //            using (var response = await httpClient.PostAsync(_url, byteContent))
        //            {
        //                var apiResponseX = await response.Content.ReadAsStringAsync();
        //                var _response = JsonConvert.DeserializeObject<MessagingInfobipSmsResponse>(apiResponseX);

        //                //return new SmsSendResponse()
        //                //{
        //                //    StatusCode = response.StatusCode,
        //                //    IsSuccessStatusCode = response.IsSuccessStatusCode,
        //                //    IsSuccessSending = (_response != null ? _response.Item1 : false),
        //                //    Messages = (_response != null ? _response.Item2 : null),
        //                //};
        //                return new Tuple<bool, string[]>(true, new string[] { _response.StatusCode.ToString() });
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public async Task<Tuple<bool, string[]>> SendEmailAsync(EmailModel email)
        {
            string username = "FILINVEST04";
            string password = "!P@ssw0rd123";

            byte[] concatenated = System.Text.ASCIIEncoding.ASCII.GetBytes(username + ":" + password);
            string header = System.Convert.ToBase64String(concatenated);

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://api.infobip.com/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", header);
            var request = new MultipartFormDataContent();
            request.Add(new StringContent(email.From), "from");
            request.Add(new StringContent(email.To), "to");
            request.Add(new StringContent(email.Subject), "subject");
            request.Add(new StringContent(email.Body), "html");


            var response = client.PostAsync("email/1/send", request).Result;
            string contents = await response.Content.ReadAsStringAsync();


            return Tuple.Create(true, new string[] { contents.ToString() });
        }
    }
}


// //  string apiURL = "https://api.zerobounce.net/v1/validate?apikey=" + apiKey + "&email=" + HttpUtility.UrlEncode(email);
// //string apiURL = "http://messaging.filinvest.com.ph/api/InfoBip/email"

// var _emailmessage = new EmailModel();

// _emailmessage.To = "";
// _emailmessage.From = "";
// _emailmessage.Body = "";
// _emailmessage.Subject = "";
// _emailmessage.Body = "";


// //Sending   
//// var _emailApiClient = new EmailApiClient();
//// var _sendReponse = await _emailApiClient.SendEmailAsync(_emailmessage).ConfigureAwait(false);

//// var _sendReponseStr = JsonConvert.SerializeObject(_sendReponse);


// throw new NotImplementedException();
