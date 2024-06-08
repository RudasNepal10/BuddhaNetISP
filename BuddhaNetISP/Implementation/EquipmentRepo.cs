
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using BuddhaNetISP.DTO;
using BuddhaNetISP.Interface;
using BuddhaNetISP.Models;
using BuddhaNetISP.Helpers;
using Microsoft.Extensions.Options;

namespace BuddhaNetISP.Implementation
{
    public class EquipmentRepo : IEquipmenrRepo
    {
        private readonly IOptions<ConnectionString> _connectionString;

        public EquipmentRepo(IOptions<ConnectionString> connectionString)
        {
            _connectionString = connectionString;
        }

        public JsonResponse DeleteEquipment(string serialnumber)
        {
            JsonResponse response = new JsonResponse();

            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString.Value.DBConnection))
            {
                try
                {
                    connection.Open();

                    using (NpgsqlCommand command = new NpgsqlCommand("DELETE FROM public.equipment WHERE serialnumber = @Serialnumber", connection))
                    {
                        command.Parameters.AddWithValue("@Serialnumber", NpgsqlTypes.NpgsqlDbType.Varchar, serialnumber);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            response.IsSuccess = true;
                            response.Message = "Equipment deleted successfully.";
                        }
                        else
                        {
                            response.IsSuccess = false;
                            response.Message = "No equipment found with the provided serial number.";
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

        public JsonResponse GetAllEquipments()
        {
            JsonResponse response = new JsonResponse();
            List<EquipementModel> equipments = new List<EquipementModel>();

            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString.Value.DBConnection))
            {
                try
                {
                    connection.Open();

                    NpgsqlCommand command = new NpgsqlCommand("select * from public.equipment e ", connection);

                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            EquipementModel equipment = new EquipementModel
                            {
                                equipmentid = Convert.ToInt32(reader["EquipmentId"]),
                                serialnumber = Convert.ToString(reader["SerialNumber"]),
                                equipmenttype = Convert.ToString(reader["Equipmenttype"]),
                                make = Convert.ToString(reader["make"]),
                                isunderlicense = Convert.ToBoolean(reader["isunderlicense"])

                            };
                            equipments.Add(equipment);
                        }
                    }

                    response.IsSuccess = true;
                    response.Message = "Equipments retrieved successfully.";
                    response.ResponseData = equipments;
                }
                catch (Exception ex)
                {
                    response.IsSuccess = false;
                    response.Message = "An error occurred: " + ex.Message;
                }
                finally { connection.Close(); }
            }

            return response;
        }

        public JsonResponse GetEquipmentById(int equipmentId)
        {
            JsonResponse response = new JsonResponse();
            EquipementModel equipments = new EquipementModel();

            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString.Value.DBConnection))
            {
                try
                {
                    connection.Open();

                    NpgsqlCommand command = new NpgsqlCommand("select * from public.equipment  WHERE EquipmentId = @EquipmentId", connection);
                    command.Parameters.AddWithValue("@EquipmentId", equipmentId);

                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                             equipments = new EquipementModel
                            {
                                equipmentid = Convert.ToInt32(reader["EquipmentId"]),
                                serialnumber = Convert.ToString(reader["SerialNumber"]),
                                equipmenttype = Convert.ToString(reader["Equipmenttype"]),
                                make = Convert.ToString(reader["make"]),
                                isunderlicense = Convert.ToBoolean(reader["isunderlicense"])
                            };
                        }
                    }

                    if (equipments.equipmentid != 0)
                    {
                        response.IsSuccess = true;
                        response.Message = "Equipment found.";
                        response.ResponseData = equipments;
                    }
                    else
                    {
                        response.IsSuccess = false;
                        response.Message = "No equipment found with the provided ID.";
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

        public JsonResponse SaveEquipment(EquipmentDTO dto)
        {
            JsonResponse response = new JsonResponse();

            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString.Value.DBConnection))
            {
                connection.Open();
                using (NpgsqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        NpgsqlCommand command = new NpgsqlCommand("INSERT INTO public.equipment( serialnumber, equipmenttype, make, isunderlicense)   VALUES(@SerialNumber, @EquipmentType, @Make, @IsUnderLicense);", connection);
                        var parameters = command.Parameters;
                        parameters.AddWithValue("@SerialNumber", dto.SerialNumber);
                        parameters.AddWithValue("@EquipmentType", dto.EquipmentType);
                        parameters.AddWithValue("@Make", dto.Make);
                        parameters.AddWithValue("@IsUnderLicense", dto.IsUnderLicense);

                        var rowAffected = command.ExecuteNonQuery();
                        transaction.Commit();
                        response.IsSuccess = true;
                        response.Message = "Equipment saved successfully.";
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


        public JsonResponse UpdateEquipment(EquipmentDTO dto)
        {
            JsonResponse response = new JsonResponse();

            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString.Value.DBConnection))
            {
                connection.Open();
                using (NpgsqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        NpgsqlCommand command = new NpgsqlCommand("UPDATE public.equipment SET equipmenttype= @EquipmentType , make=@Make, isunderlicense=@IsUnderLicense WHERE equipmentid = @EquipmentID;", connection);
                        var parameters = command.Parameters;
                       // parameters.AddWithValue("@SerialNumber", dto.SerialNumber);
                        parameters.AddWithValue("@EquipmentType", dto.EquipmentType);
                        parameters.AddWithValue("@Make", dto.Make);
                        parameters.AddWithValue("@IsUnderLicense", dto.IsUnderLicense);
                        parameters.AddWithValue("@EquipmentID", dto.EquipmentID);

                        var rowAffected = command.ExecuteNonQuery();
                        transaction.Commit();
                        response.IsSuccess = true;
                        response.Message = "Equipment updated successfully.";
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

