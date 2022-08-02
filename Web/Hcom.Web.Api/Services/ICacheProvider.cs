using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hcom.Web.Api.Services
{
    public interface ICacheProvider
    {
        Task SetAsync<T>(string key, T value);
        Task SetWithSlidingExpirationAsync<T>(string key, T value, int expiresInSecs);
        Task SetWithAbsoluteExpirationAsync<T>(string key, T value, int expiresInSecs);
        Task SetWithAbsoluteExpirationRelativeToNowAsync<T>(string key, T value, int expiresInSecs);
        Task<T> GetAsync<T>(string key);
        Task Remove(string key);
    }
}
