using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DebtorsProcessing.Api.Dtos.Requests;
using DebtorsProcessing.Api.Dtos.Responses;

using RestSharp;

namespace DebtorsProcessing.Desktop.Model
{
    public static class ServiceTalker
    {
        private static string loginToken;
        private static string loginRefreshToken;
        private static string sessionToken;
        private static string sessionRefreshToken;
        private static readonly RestClient client = new("https://localhost:5001");
        public static async Task<bool> Login(string login, string pass)
        {
            RestRequest request = new("Login/Login", Method.Post);
            LoginRequest requestModel = new()
            {
                Login = login,
                Password = pass
            };
            request.AddBody(requestModel);
            LoginResponse result = await client.PostAsync<LoginResponse>(request);
            if (result != null)
            {
                loginToken = result.AuthToken;
                loginRefreshToken = result.RefreshToken;
            }
            return result is { Success: true };
        }


        public static async Task<IEnumerable<UserRoleDto>> GetRoles()
        {
            try
            {
                RestRequest request = new("SessionsManagement/MyRoles");
                request.AddHeader("LoginAuth", loginToken);
                var response = await client.GetAsync<RolesArrayDto>(request);
                return response?.Roles;
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                return null;
            }
        }

        public static async Task<IEnumerable<ChooseUserSessionDto>> GetSessions()
        {
            RestRequest request = new("SessionsManagement/MySessions");
            request.AddHeader("LoginAuth", loginToken);
            var response = await client.GetAsync<UserActiveSessionsDto>(request);
            return response?.Sessions;
        }

        public static async Task<Guid> CreateSesion(Guid[] roles)
        {
            try
            {
                RestRequest request = new("SessionsManagement/CreateSession", Method.Post);
                request.AddHeader("LoginAuth", loginToken);
                var requestModel = new Api.Dtos.Requests.CreateSessionDto();
                requestModel.Roles = roles;
                request.AddBody(requestModel);
                var response = await client.PostAsync<Guid>(request);
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine();
            }

            return Guid.Empty;
        }

        public static async Task DropCurrentSession()
        {
            try
            {
                RestRequest request = new("SessionsManagement/DropSession", Method.Post);
                request.AddHeader("LoginAuth", loginToken);
                var response = await client.PostAsync(request);
                sessionRefreshToken = sessionToken = null;
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static async Task<bool> SelectSession(Guid sessionId)
        {
            try
            {
                RestRequest request = new("SessionsManagement/SelectSession", Method.Post);
                request.AddHeader("LoginAuth", loginToken);
                request.AddQueryParameter("sessionId", sessionId);
                var response = await client.PostAsync<SelectedSessionResult>(request);
                if (response == null) return false;
                if (response.Success)
                {
                    sessionToken = response.Token;
                    sessionRefreshToken = response.RefreshToken;
                }

                return response.Success;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return false;
        }
    }
}
