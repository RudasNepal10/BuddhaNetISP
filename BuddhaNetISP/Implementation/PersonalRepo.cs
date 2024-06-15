using BuddhaNetISP.DTO;
using BuddhaNetISP.Helpers;
using BuddhaNetISP.Interface;
using BuddhaNetISP.Models;
using Microsoft.Extensions.Options;
using Npgsql;

namespace BuddhaNetISP.Implementation
{
    public class PersonalRepo :IPersonalRepo
    {
        private readonly IOptions<ConnectionString> _connectionString;
        public PersonalRepo(IOptions<ConnectionString> connectionstring)
        {
            _connectionString = connectionstring;   
        }
        public JsonResponse DeletePersonal(int personnelid)
        {
            JsonResponse response = new JsonResponse();

            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString.Value.DBConnection))
            {
                try
                {
                    connection.Open();

                    using (NpgsqlCommand command = new NpgsqlCommand("DELETE FROM public.personnel WHERE personnelid = @PersonnelID", connection))
                    {
                        command.Parameters.AddWithValue("@PersonnelID", NpgsqlTypes.NpgsqlDbType.Integer, personnelid);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            response.IsSuccess = true;
                            response.Message = "Personal record deleted successfully.";
                        }
                        else
                        {
                            response.IsSuccess = false;
                            response.Message = "No personal record found with the provided personal ID.";
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
        public JsonResponse GetAllPersonals()
        {
            JsonResponse response = new JsonResponse();
            List<Personal> personals = new List<Personal>();

            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString.Value.DBConnection))
            {
                try
                {
                    connection.Open();

                    NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM public.personnel", connection);

                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Personal personal = new Personal
                            {
                                personnelid = Convert.ToInt32(reader["PersonnelID"]),
                                name = Convert.ToString(reader["Name"]),
                                jobtitle = Convert.ToString(reader["JobTitle"]),
                                department = Convert.ToString(reader["Department"])
                            };
                            personals.Add(personal);
                        }
                    }

                    response.IsSuccess = true;
                    response.Message = "Personals retrieved successfully.";
                    response.ResponseData = personals;
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
        public JsonResponse GetPersonalById(int personnelid)
        {
            JsonResponse response = new JsonResponse();
            Personal personal = new Personal();

            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString.Value.DBConnection))
            {
                try
                {
                    connection.Open();

                    NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM public.personnel WHERE Personnelid = @personnelid", connection);
                    command.Parameters.AddWithValue("@personnelid", personnelid);

                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            personal = new Personal
                            {
                                personnelid = Convert.ToInt32(reader["personnelid"]),
                                name = Convert.ToString(reader["Name"]),
                                jobtitle = Convert.ToString(reader["JobTitle"]),
                                department = Convert.ToString(reader["Department"])
                            };
                        }
                    }

                    if (personal.personnelid !=0)
                    {
                        response.IsSuccess = true;
                        response.Message = "Personal found.";
                        response.ResponseData = personal;
                    }
                    else
                    {
                        response.IsSuccess = false;
                        response.Message = "No personal found with the provided ID.";
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
        public JsonResponse SavePersonal(PersonalDTO dto)
        {
            JsonResponse response = new JsonResponse();

            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString.Value.DBConnection))
            {
                connection.Open();
                using (NpgsqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        NpgsqlCommand command = new NpgsqlCommand("INSERT INTO public.personnel( name, jobtitle, department) VALUES( @Name, @JobTitle, @Department);", connection);
                        var parameters = command.Parameters;
                        //parameters.AddWithValue("@personnelid", dto.personnelid);
                        parameters.AddWithValue("@Name", dto.name);
                        parameters.AddWithValue("@JobTitle", dto.jobtitle);
                        parameters.AddWithValue("@Department", dto.department);

                        var rowsAffected = command.ExecuteNonQuery();
                        transaction.Commit();
                        response.IsSuccess = true;
                        response.Message = "Personal saved successfully.";
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        response.IsSuccess = false;
                        response.Message = "An error occurred: " + ex.Message;
                    }
                }
            }

            return response;
        }
        public JsonResponse UpdatePersonal(PersonalDTO dto)
        {
            JsonResponse response = new JsonResponse();

            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString.Value.DBConnection))
            {
                connection.Open();
                using (NpgsqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        NpgsqlCommand command = new NpgsqlCommand("UPDATE public.personnel SET name = @Name, jobtitle = @JobTitle, department = @Department WHERE personnelid = @PersonnelId;", connection);
                        var parameters = command.Parameters;
                        //parameters.AddWithValue("@PersonnelId", dto.personnelid);
                        parameters.AddWithValue("@Name", dto.name);
                        parameters.AddWithValue("@JobTitle", dto.jobtitle);
                        parameters.AddWithValue("@Department", dto.department);

                        var rowsAffected = command.ExecuteNonQuery();
                        transaction.Commit();

                        response.IsSuccess = true;
                        response.Message = "Personal updated successfully.";
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        response.IsSuccess = false;
                        response.Message = "An error occurred: " + ex.Message;
                    }
                }
            }

            return response;
        }


    }
}
