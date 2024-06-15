using BuddhaNetISP.DTO;
using BuddhaNetISP.Helpers;
using BuddhaNetISP.Interface;
using BuddhaNetISP.Models;
using Microsoft.Extensions.Options;
using Npgsql;

namespace BuddhaNetISP.Implementation
{
    public class HelpdeskqueriesRepo:IHelpdeskqueriesRepo
    {
     private readonly IOptions<ConnectionString> _Connectionstring;
        public HelpdeskqueriesRepo(IOptions<ConnectionString> connectionstring)
        {
            _Connectionstring = connectionstring;
        }
        public JsonResponse DeleteHelpdeskQuery(int queryId)
        {
            JsonResponse response = new JsonResponse();

            using (NpgsqlConnection connection = new NpgsqlConnection(_Connectionstring.Value.DBConnection))
            {
                try
                {
                    connection.Open();

                    using (NpgsqlCommand command = new NpgsqlCommand("DELETE FROM public.helpdeskqueries WHERE queryid = @QueryID", connection))
                    {
                        command.Parameters.AddWithValue("@QueryID", NpgsqlTypes.NpgsqlDbType.Integer, queryId);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            response.IsSuccess = true;
                            response.Message = "Helpdesk query deleted successfully.";
                        }
                        else
                        {
                            response.IsSuccess = false;
                            response.Message = "No helpdesk query found with the provided query ID.";
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
        public JsonResponse GetAllHelpdeskQueries()
        {
            JsonResponse response = new JsonResponse();
            List<Helpdeskqueries> helpdeskQueries = new List<Helpdeskqueries>();

            using (NpgsqlConnection connection = new NpgsqlConnection(_Connectionstring.Value.DBConnection))
            {
                try
                {
                    connection.Open();

                    NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM public.helpdeskqueries", connection);

                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Helpdeskqueries helpdeskQuery = new Helpdeskqueries
                            {
                                QueryId = Convert.ToInt32(reader["queryid"]),
                                CallerId = Convert.ToInt32(reader["callerid"]),
                                OperatorId = Convert.ToInt32(reader["operatorid"]),
                                CallTime = Convert.ToDateTime(reader["calltime"]),
                                EquipmentId = Convert.ToInt32(reader["equipmentid"]),
                                SoftwareId = Convert.ToInt32(reader["softwareid"]),
                                ProblemTypeId = Convert.ToInt32(reader["problemtypeid"]),
                                Description = Convert.ToString(reader["description"]),
                                IsResolved = Convert.ToBoolean(reader["isresolved"]),
                                ResolutionTime = reader["resolutiontime"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["resolutiontime"]),
                                ResolutionDetails = Convert.ToString(reader["resolutiondetails"]),
                                SpecialistId = Convert.ToInt32(reader["specialistid"]),
                                CreateDate = Convert.ToDateTime(reader["createdat"]),
                                UpdateDate = reader["updatedat"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["updatedat"])
                            };
                            helpdeskQueries.Add(helpdeskQuery);
                        }
                    }

                    response.IsSuccess = true;
                    response.Message = "Helpdesk queries retrieved successfully.";
                    response.ResponseData = helpdeskQueries;
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
        public JsonResponse SaveHelpdeskQuery(HelpdeskqueriesDTO dto)
        {
            JsonResponse response = new JsonResponse();

            using (NpgsqlConnection connection = new NpgsqlConnection(_Connectionstring.Value.DBConnection))
            {
                connection.Open();
                using (NpgsqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        string query = @"
                    INSERT INTO public.helpdeskqueries (
                        callerid, operatorid, calltime, equipmentid, softwareid, 
                        problemtypeid, description, isresolved, resolutiontime, 
                        resolutiondetails, specialistid, createdat, updatedat
                    ) VALUES (
                        @CallerId, @OperatorId, @CallTime, @EquipmentId, @SoftwareId, 
                        @problemtypeid, @Description, @IsResolved, @ResolutionTime, 
                        @ResolutionDetails, @SpecialistId, @CreateDat, @UpdateDat
                    );";

                        using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                        {
                            var parameters = command.Parameters;
                            parameters.AddWithValue("@CallerId", dto.CallerId);
                            parameters.AddWithValue("@OperatorId", dto.OperatorId);
                            parameters.AddWithValue("@CallTime", dto.CallTime);
                            parameters.AddWithValue("@EquipmentId", dto.EquipmentId);
                            parameters.AddWithValue("@SoftwareId", dto.SoftwareId);
                            parameters.AddWithValue("@problemtypeid", dto.ProblemTypeId);
                            parameters.AddWithValue("@Description", dto.Description ?? (object)DBNull.Value);
                            parameters.AddWithValue("@IsResolved", dto.IsResolved);
                            parameters.AddWithValue("@ResolutionTime", dto.ResolutionTime ?? (object)DBNull.Value);
                            parameters.AddWithValue("@ResolutionDetails", dto.ResolutionDetails ?? (object)DBNull.Value);
                            parameters.AddWithValue("@SpecialistId", dto.SpecialistId);
                            parameters.AddWithValue("@CreateDat", dto.CreateDate);
                            parameters.AddWithValue("@UpdateDat", dto.UpdateDate ?? (object)DBNull.Value);

                            var rowsAffected = command.ExecuteNonQuery();
                            transaction.Commit();
                            response.IsSuccess = true;
                            response.Message = "Helpdesk query saved successfully.";
                        }
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
