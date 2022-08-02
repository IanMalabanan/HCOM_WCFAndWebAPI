
using Hcom.App.Entities;
using Hcom.Web.Api.Interface;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hcom.Web.Api.Services
{
    public class UserInfoProvider
    {
        private string _cacheKey = "__users";
        private int _cacheExpSec = 60 * 60 * 5; //3hrs
        private int _maxRefOjb = 100;

        private readonly IUser _userService;
        private readonly ICacheProvider _cacheProvider;

        public UserInfoProvider(IUser userService,
            ICacheProvider cacheProvider)
        {
            _userService = userService;
            _cacheProvider = cacheProvider;
        }

        public async Task<User> GetUserAsync(string username)
        {
            if (string.IsNullOrEmpty(username))
                return null;

            var _results = await GetUsersAsync(new string[] { username });
            return _results.FirstOrDefault();
        }

        public async Task<IEnumerable<User>> GetUsersAsync(string[] usernames)
        {
            if (usernames == null)
                return new List<User>();

            usernames = usernames.Where(x => !string.IsNullOrEmpty(x)).Select(x => x.ToUpper()).Distinct().ToArray();

            if (!usernames.Any())
                return new List<User>();

            var _cachedUsers = await _cacheProvider.GetAsync<IEnumerable<User>>(_cacheKey);

            if (_cachedUsers == null)
                _cachedUsers = new List<User>();

            var _usersExists = _cachedUsers.Where(x => usernames.Contains(x.Id));

            //Not existing reference objects
            var _existingUsernames = _usersExists.Select(x => x.Id).ToArray();
            var _getUserNames = usernames.Where(x => !_existingUsernames.Contains(x)).ToArray();

            if (!_getUserNames.Any())
                return _usersExists;

            var _usersFromSvc = await GetUsersFromServiceAsync(_getUserNames);

            //combine existing and put in cache
            _cachedUsers = _cachedUsers.Concat(_usersFromSvc);
            await _cacheProvider.SetWithSlidingExpirationAsync(_cacheKey, _cachedUsers, _cacheExpSec);

            var _return = _usersExists.Concat(_usersFromSvc);
            return _return;
        }

        private async Task<IEnumerable<User>> GetUsersFromServiceAsync(string[] usernames)
        {
            var _getUsersTask = new List<Task<User>>();
            var _usernames = new List<string>();

            foreach (var _username in usernames)
            {
                _getUsersTask.Add(_userService.GetUserAsync(_username));
            }

            var _return = new List<User>();
            while (_getUsersTask.Any())
            {
                var _getTask = await Task.WhenAny(_getUsersTask);
                var _output = await _getTask;
                if (_output != null)
                {
                    _output.Id = _output.Id.ToUpper();
                    _return.Add(_output);
                }
                    
                _getUsersTask.Remove(_getTask);
            }

            return _return;

        }
    }
}
