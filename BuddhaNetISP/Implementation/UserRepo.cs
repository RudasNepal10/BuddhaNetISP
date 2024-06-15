using BuddhaNetISP.DTO;
using BuddhaNetISP.Helpers;
using BuddhaNetISP.Interface;
using Humanizer;
using Microsoft.Extensions.Options;
using Npgsql;

namespace BuddhaNetISP.Implementation
{
    public class UserRepo : IuserRepo
    {

        private IOptions<ConnectionString> _connectionstring;
        public UserRepo(IOptions<ConnectionString> connectionstring)
        {
            _connectionstring = connectionstring;   
        }
        public JsonResponse GetUser(UserDTO dto)
        {
            JsonResponse response = new JsonResponse();

            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionstring.Value.DBConnection))
            {
                connection.Open();

                try
                {
                    string query = "SELECT EXISTS(SELECT 1 FROM public.users WHERE username = @username AND Password = @Password);";
                    NpgsqlCommand command = new NpgsqlCommand(query, connection);
                    command.Parameters.AddWithValue("@username", dto.Username);
                    command.Parameters.AddWithValue("@Password", dto.Password); // Make sure to hash the password before querying

                    bool exists = (bool)command.ExecuteScalar();
                    if (exists)
                    {
                        response.IsSuccess = true;
                        response.Message = "Login successful.";
                    }
                    else
                    {
                        response.IsSuccess = false;
                        response.Message = "Invalid username or password.";
                    }
                }
                catch (Exception ex)
                {
                    response.IsSuccess = false;
                    response.Message = ex.Message;
                }
            }

            return response;
        }

    }

}

