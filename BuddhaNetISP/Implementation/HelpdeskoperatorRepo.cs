using BuddhaNetISP.DTO;
using BuddhaNetISP.Helpers;
using BuddhaNetISP.Interface;
using BuddhaNetISP.Models;
using Microsoft.Extensions.Options;
using Npgsql;

namespace BuddhaNetISP.Implementation
{
    public class HelpdeskoperatorRepo:IHelpdeskoperatorRepo
    {
        private readonly IOptions<ConnectionString> _connectionstring;
        public HelpdeskoperatorRepo(IOptions<ConnectionString> connectionstring)
        {
            _connectionstring = connectionstring;   
        }
        public JsonResponse DeleteHelpDeskOperator(int operatorid)
        {
            JsonResponse response = new JsonResponse();

            using (NpgsqlConnection connection= new NpgsqlConnection(_connectionstring.Value.DBConnection))
            {
                try
                {
                    connection.Open();

                    using (NpgsqlCommand command = new NpgsqlCommand("DELETE FROM public.helpdeskoperators WHERE operatorid = @Operatorid", connection))
                    {
                        command.Parameters.AddWithValue("@Operatorid", NpgsqlTypes.NpgsqlDbType.Integer, operatorid);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            response.IsSuccess = true;
                            response.Message = "Help Desk Operator deleted successfully.";
                        }
                        else
                        {
                            response.IsSuccess = false;
                            response.Message = "No help desk operator found with the provided ID.";
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
        public JsonResponse GetAllHelpDeskOperators()
        {
            JsonResponse response = new JsonResponse();
            List<Helpdeskoperator> operators = new List<Helpdeskoperator>();

            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionstring.Value.DBConnection))
            {
                try
                {
                    connection.Open();

                    NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM public.helpdeskoperators", connection);

                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Helpdeskoperator helpdeskoperator = new Helpdeskoperator
                            {
                             operatorid = Convert.ToInt32(reader["OperatorID"]),
                              name = Convert.ToString(reader["name"]),
                            };
                            operators.Add(helpdeskoperator);

                        }
                    }

                    response.IsSuccess = true;
                    response.Message = "Help Desk Operators retrieved successfully.";
                    response.ResponseData = operators;
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
        public JsonResponse GetHelpDeskOperatorById(int operatorId)
        {
            JsonResponse response = new JsonResponse();
            Helpdeskoperator helpdeskoperator = new Helpdeskoperator();

            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionstring.Value.DBConnection))
            {
                try
                {
                    connection.Open();

                    NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM public.helpdeskoperators WHERE OperatorId = @OperatorId", connection);
                    command.Parameters.AddWithValue("@OperatorId", operatorId);

                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            helpdeskoperator = new Helpdeskoperator
                            {
                                operatorid = Convert.ToInt32(reader["OperatorId"]),
                                name = Convert.ToString(reader["Name"]),
                            };
                        }
                    }

                    if (helpdeskoperator.operatorid != 0)
                    {
                        response.IsSuccess = true;
                        response.Message = "Help desk operator found.";
                        response.ResponseData = helpdeskoperator;
                    }
                    else
                    {
                        response.IsSuccess = false;
                        response.Message = "No help desk operator found with the provided ID.";
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
        public JsonResponse SaveHelpDeskOperator(HelpdeskoperatorDTO dto)
        {
            JsonResponse response = new JsonResponse();

            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionstring.Value.DBConnection))
            {
                connection.Open();
                using (NpgsqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        NpgsqlCommand command = new NpgsqlCommand("INSERT INTO public.helpdeskoperators( name) VALUES( @name);", connection);
                        var parameters = command.Parameters;
                        //parameters.AddWithValue("@OperatorId", dto.operatorid);
                        parameters.AddWithValue("@name", dto.name);

                        var rowAffected = command.ExecuteNonQuery();
                        transaction.Commit();
                        response.IsSuccess = true;
                        response.Message = "Help desk operator saved successfully.";
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
        public JsonResponse UpdateHelpDeskOperator(HelpdeskoperatorDTO dto)
        {
            JsonResponse response = new JsonResponse();

            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionstring.Value.DBConnection))
            {
                connection.Open();
                using (NpgsqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        NpgsqlCommand command = new NpgsqlCommand("UPDATE public.helpdeskoperators SET name = @Name WHERE operatorid = @OperatorID;", connection);
                        var parameters = command.Parameters;
                        parameters.AddWithValue("@Name", dto.name);
                      
                        var rowAffected = command.ExecuteNonQuery();
                        transaction.Commit();
                        response.IsSuccess = true;
                        response.Message = "Help desk operator updated successfully.";
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
