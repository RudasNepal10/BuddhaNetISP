using BuddhaNetISP.DTO;
using BuddhaNetISP.Helpers;
using BuddhaNetISP.Interface;
using BuddhaNetISP.Models;
using Microsoft.Extensions.Options;
using Npgsql;

namespace BuddhaNetISP.Implementation
{
    public class ProblemtypeRepo : IProblemtypeRepo
    {
        private readonly IOptions<ConnectionString> _connectionstring;
        public ProblemtypeRepo(IOptions<ConnectionString> connectionstring)
        {
            _connectionstring = connectionstring;   
        }
        public JsonResponse DeleteProblemType(int problemTypeId)
        {
            JsonResponse response = new JsonResponse();

            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionstring.Value.DBConnection))
            {
                try
                {
                    connection.Open();

                    using (NpgsqlCommand command = new NpgsqlCommand("DELETE FROM public.problemtypes WHERE problemtypeid = @ProblemtypeId", connection))
                    {
                        command.Parameters.AddWithValue("@ProblemtypeId", NpgsqlTypes.NpgsqlDbType.Integer, problemTypeId);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            response.IsSuccess = true;
                            response.Message = "Problem Type deleted successfully.";
                        }
                        else
                        {
                            response.IsSuccess = false;
                            response.Message = "No problem type found with the provided ID.";
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
        public JsonResponse GetAllProblemTypes()
        {
            JsonResponse response = new JsonResponse();
            List<Problemtype> problemtypes = new List<Problemtype>();

            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionstring.Value.DBConnection))
            {
                try
                {
                    connection.Open();

                    NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM public.problemtypes", connection);

                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Problemtype problemtype = new Problemtype()
                            {
                                problemtypeid = Convert.ToInt32(reader["problemtypeid"]),
                                typename = Convert.ToString(reader["typename"]),
                                parentproblemtypeid = reader.IsDBNull(reader.GetOrdinal("parentproblemtypeid")) ? 0 : Convert.ToInt32(reader["parentproblemtypeid"])
                            };
                            problemtypes.Add(problemtype);
                        }
                    }

                    response.IsSuccess = true;
                    response.Message = "Problem Types retrieved successfully.";
                    response.ResponseData = problemtypes;
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
        public JsonResponse GetProblemTypeById(int problemtypeid)
        {
            JsonResponse response = new JsonResponse();
            Problemtype problemtype= new Problemtype();

            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionstring.Value.DBConnection))
            {
                try
                {
                    connection.Open();

                    NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM public.problemtypes WHERE problemtypeid = @problemtypeid", connection);
                    command.Parameters.AddWithValue("@problemtypeid", problemtypeid);

                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            problemtype = new Problemtype
                            {
                                problemtypeid = Convert.ToInt32(reader["Problemtypeid"]),
                                typename = Convert.ToString(reader["Typename"]),
                                //parentproblemtypeid = reader.IsDBNull(reader.GetOrdinal("parentproblemtypeid")) ? 0 : Convert.ToInt32(reader["parentproblemtypeid"])
                            };
                        }
                    }

                    if (problemtype.problemtypeid != 0)
                    {
                        response.IsSuccess = true;
                        response.Message = "Problem type found.";
                        response.ResponseData = problemtype;
                    }
                    else
                    {
                        response.IsSuccess = false;
                        response.Message = "No problem type found with the provided ID.";
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
        public JsonResponse SaveProblemType(ProblemtypeDTO dto)
        {
            JsonResponse response = new JsonResponse();

            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionstring.Value.DBConnection))
            {
                connection.Open();
                using (NpgsqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        NpgsqlCommand command = new NpgsqlCommand("INSERT INTO public.problemtypes( typename ) VALUES( @typename );", connection);
                        var parameters = command.Parameters;
                        parameters.AddWithValue("@typename", dto.typename);
                        //parameters.AddWithValue("@parentproblemtypeid", dto.parentproblemtypeid);

                        var rowAffected = command.ExecuteNonQuery();
                        transaction.Commit();
                        response.IsSuccess = true;
                        response.Message = "Problem type saved successfully.";
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
        public JsonResponse UpdateProblemType(ProblemtypeDTO dto)
        {
            JsonResponse response = new JsonResponse();

            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionstring.Value.DBConnection))
            {
                connection.Open();
                using (NpgsqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        NpgsqlCommand command = new NpgsqlCommand("UPDATE public.problemtypes SET typename = @Typename WHERE problemtypeid = @problemtypeid;", connection);
                        var parameters = command.Parameters;
                        parameters.AddWithValue("@Typename", dto.typename);
                        //parameters.AddWithValue("@ParentproblemTypeId", dto.parentproblemtypeid);

                        var rowAffected = command.ExecuteNonQuery();
                        transaction.Commit();
                        response.IsSuccess = true;
                        response.Message = "Problem type updated successfully.";
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
