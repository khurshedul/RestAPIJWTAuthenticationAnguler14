using Core.EMS.Entities;
using Core.Utils.Entities;
using Core.Utils.Interfaces;
using Core.Utils.Services;
using Core.Utils.Utils;
using Interfaces;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Core.EMS.Services
{
    public class UserService : BaseService<User>, IUserService
    {
        private readonly IAsyncRepository<User> _userRepository;

        public UserService(IAsyncRepository<User> userRepository) : base(userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<BaseResponse> Save(User entity)
        {
            var response = new BaseResponse();

            entity.Password = Authenticator.GetHashPassword(entity.Password);

            await _userRepository.AddAsync(entity);
            response.IsSuccessful = true;
            response.Message = "OK";
            response.Data = entity.ID;
            return response;
        }
        public async Task<BaseResponse> Login(string username, string password, string requestIP, string requestBrowser)
        {
            var response = new BaseResponse();

            if (string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(password))
            {
                response.IsSuccessful = false;
                response.Message = "Username or password is incorrect.";
                return response;
            }

            var user = await GetUser(username);

            var result = ValidateLoginInformation(user, password);
            if (result.Key)
            {


                response.IsSuccessful = true;
                response.Message = "OK";
                response.Data = new LoginInformation()
                {
                    Username = user.Username,
                };
                return response;
            }

            response.IsSuccessful = false;
            response.Message = result.Value;
            return response;
        }
        public async Task<User> GetUser(string username)
        {
            var result = await _userRepository.ListAsync(s => s.Username == username);
            return result.FirstOrDefault();
        }
        private KeyValuePair<bool, string> ValidateLoginInformation(User userInformation, string password)
        {
            if (userInformation == null)
            {
                return new KeyValuePair<bool, string>(false, "User information is not found.");
            }
            if (!Authenticator.ValidatePassword(password, userInformation.Password.Trim()))
            {
                return new KeyValuePair<bool, string>(false, "Username or password is incorrect.");
            }
            return new KeyValuePair<bool, string>(true, "");
        }
    }
}
