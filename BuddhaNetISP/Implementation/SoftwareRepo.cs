using BuddhaNetISP.DTO;
using BuddhaNetISP.Helpers;
using BuddhaNetISP.Interface;
using BuddhaNetISP.Models;
using Microsoft.Extensions.Options;
using Npgsql;

namespace BuddhaNetISP.Implementation
{
    public class SoftwareRepo:ISoftwareRepo
    {
        private readonly IOptions<ConnectionString> _connectionstring;
        public SoftwareRepo(IOptions<ConnectionString> connectionstring)
        {
            _connectionstring = connectionstring;   
        }
        public JsonResponse DeleteSoftware(int softwareid)
        {
            JsonResponse response = new JsonResponse();

            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionstring.Value.DBConnection))
            {
                try
                {
                    connection.Open();

                    using (NpgsqlCommand command = new NpgsqlCommand("DELETE FROM public.software WHERE softwareid = @softwareId", connection))
                    {
                        command.Parameters.AddWithValue("@softwareId", NpgsqlTypes.NpgsqlDbType.Integer, softwareid);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            response.IsSuccess = true;
                            response.Message = "Software deleted successfully.";
                        }
                        else
                        {
                            response.IsSuccess = false;
                            response.Message = "No software found with the provided ID.";
                        }
                    }
                }
                catch (Exception ex)
                {
                    response.IsSuccess = false;
                    response.Message = "An error occurred: " + ex.Message;
                }
            }

            return response;
        }
        public JsonResponse GetAllSoftware()
        {
            JsonResponse response = new JsonResponse();
            List<Software> softwareList = new List<Software>();

            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionstring.Value.DBConnection))
            {
                try
                {
                    connection.Open();

                    NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM public.software", connection);

                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Software software = new Software()
                            {
                                softwareid = Convert.ToInt32(reader["softwareid"]),
                                softwarename = Convert.ToString(reader["name"]),
                                softwareversion = Convert.ToString(reader["version"]),
                                isunderlicense = Convert.ToBoolean(reader["isunderlicense"])
                            };
                            softwareList.Add(software);
                        }
                    }

                    response.IsSuccess = true;
                    response.Message = "Software retrieved successfully.";
                    response.ResponseData = softwareList;
                }
                catch (Exception ex)
                {
                    response.IsSuccess = false;
                    response.Message = "An error occurred: " + ex.Message;
                }
                finally
                {
                    connection.Close();
                }
            }

            return response;
        }
        public JsonResponse GetSoftwareById(int softwareId)
        {
            JsonResponse response = new JsonResponse();
            Software software = new Software();

            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionstring.Value.DBConnection))
            {
                try
                {
                    connection.Open();

                    NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM public.software WHERE softwareid = @softwareId", connection);
                    command.Parameters.AddWithValue("@softwareId", softwareId);

                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            software = new Software
                            {
                                softwareid = Convert.ToInt32(reader["softwareid"]),
                                softwarename = Convert.ToString(reader["name"]),
                                softwareversion = Convert.ToString(reader["version"]),
                                isunderlicense = Convert.ToBoolean(reader["isunderlicense"])
                            };
                        }
                    }

                    if (software.softwareid != 0)
                    {
                        response.IsSuccess = true;
                        response.Message = "Software found.";
                        response.ResponseData = software;
                    }
                    else
                    {
                        response.IsSuccess = false;
                        response.Message = "No software found with the provided ID.";
                    }
                }
                catch (Exception ex)
                {
                    response.IsSuccess = false;
                    response.Message = "An error occurred: " + ex.Message;
                }
            }

            return response;
        }
        public JsonResponse SaveSoftware(SoftwareDTO dto)
        {
            JsonResponse response = new JsonResponse();

            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionstring.Value.DBConnection))
            {
                connection.Open();
                using (NpgsqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        NpgsqlCommand command = new NpgsqlCommand("INSERT INTO public.software(name, version, isunderlicense) VALUES(@name, @version, @isunderlicense);", connection);
                        var parameters = command.Parameters;
                        parameters.AddWithValue("@name", dto.softwarename);
                        parameters.AddWithValue("@version", dto.softwareversion);
                        parameters.AddWithValue("@isunderlicense", dto.isunderlicense);

                        var rowsAffected = command.ExecuteNonQuery();
                        transaction.Commit();
                        response.IsSuccess = true;
                        response.Message = "Software saved successfully.";
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        response.IsSuccess = false;
                        response.Message = ex.Message;
                    }
                }
            }

            return response;
        }
        public JsonResponse UpdateSoftware(SoftwareDTO dto)
        {
            JsonResponse response = new JsonResponse();

            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionstring.Value.DBConnection))
            {
                connection.Open();
                using (NpgsqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        NpgsqlCommand command = new NpgsqlCommand("UPDATE public.software SET name = @name, version = @version, isunderlicense = @isunderlicense WHERE softwareid = @softwareid;", connection);
                        var parameters = command.Parameters;
                        parameters.AddWithValue("@name", dto.softwarename);
                        parameters.AddWithValue("@version", dto.softwareversion);
                        parameters.AddWithValue("@isunderlicense", dto.isunderlicense);
                        parameters.AddWithValue("@softwareid", dto.softwareid);

                        var rowsAffected = command.ExecuteNonQuery();
                        transaction.Commit();
                        response.IsSuccess = true;
                        response.Message = "Software updated successfully.";
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        response.IsSuccess = false;
                        response.Message = ex.Message;
                    }
                }
            }

            return response;
        }
       
    }
}
