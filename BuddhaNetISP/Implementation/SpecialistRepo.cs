using BuddhaNetISP.DTO;
using BuddhaNetISP.Helpers;
using BuddhaNetISP.Interface;
using BuddhaNetISP.Models;
using Microsoft.Extensions.Options;
using Npgsql;

namespace BuddhaNetISP.Implementation
{
    public class SpecialistRepo: IspecialistRepo
    {
        private readonly IOptions<ConnectionString> _connectionstring;
        public SpecialistRepo(IOptions<ConnectionString> connectionstring)
        {
            _connectionstring = connectionstring;   
        }
        public JsonResponse DeleteSpecialist(int specialistId)
        {
            JsonResponse response = new JsonResponse();

            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionstring.Value.DBConnection))
            {
                try
                {
                    connection.Open();

                    using (NpgsqlCommand command = new NpgsqlCommand("DELETE FROM public.specialists WHERE specialistid = @specialistId", connection))
                    {
                        command.Parameters.AddWithValue("@specialistId", NpgsqlTypes.NpgsqlDbType.Integer, specialistId);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            response.IsSuccess = true;
                            response.Message = "Specialist deleted successfully.";
                        }
                        else
                        {
                            response.IsSuccess = false;
                            response.Message = "No specialist found with the provided ID.";
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
        public JsonResponse GetAllSpecialists()
        {
            JsonResponse response = new JsonResponse();
            List<Specialist> specialistList = new List<Specialist>();

            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionstring.Value.DBConnection))
            {
                try
                {
                    connection.Open();

                    NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM public.specialists", connection);

                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Specialist specialist = new Specialist()
                            {
                                specialistid = Convert.ToInt32(reader["specialistid"]),
                                specialistname = Convert.ToString(reader["name"]),
                                expertisearea = Convert.ToInt32(reader["expertisearea"])
                            };
                            specialistList.Add(specialist);
                        }
                    }

                    response.IsSuccess = true;
                    response.Message = "Specialists retrieved successfully.";
                    response.ResponseData = specialistList;
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
        public JsonResponse GetSpecialistById(int specialistId)
        {
            JsonResponse response = new JsonResponse();
            Specialist specialist = new Specialist();

            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionstring.Value.DBConnection))
            {
                try
                {
                    connection.Open();

                    NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM public.specialists WHERE specialistid = @specialistId", connection);
                    command.Parameters.AddWithValue("@specialistId", specialistId);

                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            specialist = new Specialist
                            {
                                specialistid = Convert.ToInt32(reader["specialistid"]),
                                specialistname = Convert.ToString(reader["name"]),
                                expertisearea = Convert.ToInt32(reader["expertisearea"])
                            };
                        }
                    }

                    if (specialist.specialistid != 0)
                    {
                        response.IsSuccess = true;
                        response.Message = "Specialist found.";
                        response.ResponseData = specialist;
                    }
                    else
                    {
                        response.IsSuccess = false;
                        response.Message = "No specialist found with the provided ID.";
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
        public JsonResponse SaveSpecialist(SpecialistDTO dto)
        {
            JsonResponse response = new JsonResponse();

            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionstring.Value.DBConnection))
            {
                connection.Open();
                using (NpgsqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        NpgsqlCommand command = new NpgsqlCommand("INSERT INTO public.specialists(name, expertisearea) VALUES(@name, @expertisearea);", connection);
                        var parameters = command.Parameters;
                        parameters.AddWithValue("@name", dto.specialistname);
                        parameters.AddWithValue("@expertisearea", dto.expertisearea);

                        var rowsAffected = command.ExecuteNonQuery();
                        transaction.Commit();
                        response.IsSuccess = true;
                        response.Message = "Specialist saved successfully.";
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
        public JsonResponse UpdateSpecialist(SpecialistDTO dto)
        {
            JsonResponse response = new JsonResponse();

            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionstring.Value.DBConnection))
            {
                connection.Open();
                using (NpgsqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        NpgsqlCommand command = new NpgsqlCommand("UPDATE public.specialists SET \"name\" = @name, expertisearea = @expertisearea WHERE specialistid = @specialistid;", connection);
                        var parameters = command.Parameters;
                        parameters.AddWithValue("@name", dto.specialistname);
                        parameters.AddWithValue("@expertisearea", dto.expertisearea);
                        parameters.AddWithValue("@specialistid", dto.specialistid);

                        var rowsAffected = command.ExecuteNonQuery();
                        transaction.Commit();
                        response.IsSuccess = true;
                        response.Message = "Specialist updated successfully.";
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
