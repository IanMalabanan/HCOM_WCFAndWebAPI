using Hcom.Web.Api.Interface;
using Hcom.Web.Api.Models;
using Hcom.Web.Api.Utilities;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Hcom.Web.Api.Services
{
    public class FrebasChangeForgotPasswordAPIService : IFrebasChangeForgotPasswordAPIService
    {
        private readonly ServiceEndpoints _config;
        private const string ChangePasswordEndpoint = "/api/ChangeOrForgotPassword/ChangePassword";
        private const string ForgotPasswordEndpoint = "/api/ChangeOrForgotPassword/ForgotPassword";
        private const string GetOTPEndpoint = "/api/ChangeOrForgotPassword/GetOTP";
        private const string ValidateOTPEndpoint = "/api/ChangeOrForgotPassword/ValidateOTP";

        public FrebasChangeForgotPasswordAPIService(IOptionsMonitor<ServiceEndpoints> config)
        {
            _config = config.CurrentValue;
        }

        public async Task<Tuple<bool,string>> ChangeUserPassword(string userName, string oldPassword, string newPassword)
        {
            try
            {
                UriBuilder url = new UriBuilder(_config.FrebasBaseUrl)
                {
                    Path = ChangePasswordEndpoint,
                };

                ChangeorForgotPassword model = new ChangeorForgotPassword()
                {
                    confirmPassword = newPassword,
                    deviceID = "",
                    deviceUsed = "",
                    environment = _config.Environment,
                    generatedOTP = "",
                    isResend = true,
                    location = "",
                    newPassword = newPassword,
                    oldPassword = oldPassword,
                    sessionID = "",
                    userName = userName
                };

                var httpClient = new HttpClient();
                string jsonResult = string.Empty;

                var content = new StringContent(JsonConvert.SerializeObject(model));
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var responseMessage =
                   await httpClient.PostAsync(url.ToString(), content)
                   ;

                jsonResult = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (responseMessage.IsSuccessStatusCode)
                {
                    return Tuple.Create(true, "Success"); ;
                }
                else
                {
                    return Tuple.Create(false, jsonResult); ;
                }
            }
            catch (Exception ex)
            {
                //Debug.WriteLine($"{ e.GetType().Name + " : " + e.Message}");
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public async Task<Tuple<bool,string>> ForgotPassword(string userName, string newPassword)
        {
            try
            {
                UriBuilder url = new UriBuilder(_config.FrebasBaseUrl)
                {
                    Path = ForgotPasswordEndpoint,
                };

                ChangeorForgotPassword model = new ChangeorForgotPassword()
                {
                    confirmPassword = newPassword,
                    deviceID = "",
                    deviceUsed = "",
                    environment = _config.Environment,
                    generatedOTP = "",
                    isResend = true,
                    location = "",
                    newPassword = newPassword,
                    oldPassword = "",
                    sessionID = "",
                    userName = userName
                };

                var httpClient = new HttpClient();
                string jsonResult = string.Empty;

                var content = new StringContent(JsonConvert.SerializeObject(model));
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var responseMessage = await httpClient.PostAsync(url.ToString(), content).ConfigureAwait(false);

                jsonResult = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (responseMessage.IsSuccessStatusCode)
                {
                    return Tuple.Create(true,"Success");
                }
                else
                {
                    return Tuple.Create(false, jsonResult);
                    //throw new ApplicationException(jsonResult.ToString().Replace("\"", ""));
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public async Task<string> GetOTP(string userName)
        {
            try
            {
                UriBuilder url = new UriBuilder(_config.FrebasBaseUrl)
                {
                    Path = GetOTPEndpoint,
                };

                ChangeorForgotPassword model = new ChangeorForgotPassword()
                {
                    confirmPassword = "",
                    deviceID = "",
                    deviceUsed = "",
                    environment = _config.Environment,
                    generatedOTP = "",
                    isResend = true,
                    location = "",
                    newPassword = "",
                    oldPassword = "",
                    sessionID = "",
                    userName = userName
                };

                var httpClient = new HttpClient();
                
                string jsonResult = string.Empty;

                var content = new StringContent(JsonConvert.SerializeObject(model));
                
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var responseMessage = await httpClient.PostAsync(url.ToString(), content).ConfigureAwait(false);

                jsonResult = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (responseMessage.IsSuccessStatusCode)
                {
                    return "Success";
                }
                else
                {
                    throw new ApplicationException(jsonResult);
                }
            }
            catch(ApplicationException ex)
            {
                throw new ApplicationException(ex.Message, ex.InnerException);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public async Task<Tuple<bool,string>> ValidateOTP(string userName, string otp)
        {
            try
            {
                UriBuilder url = new UriBuilder(_config.FrebasBaseUrl)
                {
                    Path = ValidateOTPEndpoint,
                };

                ChangeorForgotPassword model = new ChangeorForgotPassword()
                {
                    confirmPassword = "",
                    deviceID = "",
                    deviceUsed = "",
                    environment = _config.Environment,
                    generatedOTP = otp,
                    isResend = true,
                    location = "",
                    newPassword = "",
                    oldPassword = "",
                    sessionID = "",
                    userName = userName
                };

                var httpClient = new HttpClient();
                
                string jsonResult = string.Empty;

                var content = new StringContent(JsonConvert.SerializeObject(model));
                
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var responseMessage = await httpClient.PostAsync(url.ToString(), content).ConfigureAwait(false);

                jsonResult = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (responseMessage.IsSuccessStatusCode)
                {
                    if (jsonResult == "false")
                        return Tuple.Create(false, jsonResult);

                    return Tuple.Create(true, jsonResult);
                }
                else
                {
                    return Tuple.Create(false, jsonResult);
                }
            }
            catch(ApplicationException ex)
            {
                throw new ApplicationException(ex.Message, ex.InnerException);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }
    }
}
