using System;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
using System.Data;
using static JobBit_DataAccess.CompanyData;


namespace JobBit_DataAccess
{
    public class CompanyDTO
    {
        public int CompanyID { get; set; }
        public int UserID { get; set; }
        public int WilayaID { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? LogoPath { get; set; }
        public string Link { get; set; }

        public CompanyDTO(int CompanyID, int UserID, int WilayaID, string Name, string? Description, string? LogoPath, string Link)
        {
            this.CompanyID = CompanyID;
            this.UserID = UserID;
            this.WilayaID = WilayaID;
            this.Name = Name;
            this.Description = Description;
            this.LogoPath = LogoPath;
            this.Link = Link;
        }

    }

    public class CompanyListDTO
    {
        public int CompanyID { get; set; }

        public string CompanyName { get; set; }
     
        public string WilayaName { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }

        public CompanyListDTO(int companyID, string companyName, string wilayaName, string email, bool isActive)
        {
            CompanyID = companyID;
            CompanyName = companyName;
            WilayaName = wilayaName;
            Email = email;
            IsActive = isActive;
        }
    }
    public class CompanyData
    {
        public static int AddNewCompany(CompanyDTO companyDTO)
        {
            int CompanyID = -1;
            try
            {
                using (SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    Connection.Open();
                    using (SqlCommand Command = new SqlCommand("SP_AddNewCompany", Connection))
                    {
                        Command.CommandType = CommandType.StoredProcedure;
                        Command.Parameters.AddWithValue("@UserID", companyDTO.UserID);
                        Command.Parameters.AddWithValue("@WilayaID", companyDTO.WilayaID);
                        Command.Parameters.AddWithValue("@Name", companyDTO.Name);
                        if (companyDTO.Description != null && companyDTO.Description != "")
                            Command.Parameters.AddWithValue("@Description", companyDTO.Description);
                        else
                            Command.Parameters.AddWithValue("@Description", DBNull.Value);
                        if (companyDTO.LogoPath != null && companyDTO.LogoPath != "")
                            Command.Parameters.AddWithValue("@LogoPath", companyDTO.LogoPath);
                        else
                            Command.Parameters.AddWithValue("@LogoPath", DBNull.Value);
                        Command.Parameters.AddWithValue("@Link", companyDTO.Link);


                        SqlParameter outputIdParam = new SqlParameter("@CompanyID", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        Command.Parameters.Add(outputIdParam);


                        Command.ExecuteNonQuery();

                        CompanyID = (int)outputIdParam.Value;

                    }
                }

            }
            catch (Exception Ex)
            {
                clsEventLogData.WriteEvent($" Message : {Ex.Message} \n\n Source : {Ex.Source} \n\n Target Site :  {Ex.TargetSite} \n\n Stack Trace :  {Ex.StackTrace}", EventLogEntryType.Error);
                CompanyID = -1;
            }
            return CompanyID;
        }

        public static bool UpdateCompany(CompanyDTO companyDTO)
        {
            int RowsEffected = 0;
            try
            {
                using (SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    Connection.Open();
                    using (SqlCommand Command = new SqlCommand("SP_UpdateCompany", Connection))
                    {
                        Command.CommandType = CommandType.StoredProcedure;
                        Command.Parameters.AddWithValue("@CompanyID", companyDTO.CompanyID);
                        Command.Parameters.AddWithValue("@UserID", companyDTO.UserID);
                        Command.Parameters.AddWithValue("@WilayaID", companyDTO.WilayaID);
                        Command.Parameters.AddWithValue("@Name", companyDTO.Name);
                        if (companyDTO.Description != null && companyDTO.Description != "")
                            Command.Parameters.AddWithValue("@Description", companyDTO.Description);
                        else
                            Command.Parameters.AddWithValue("@Description", DBNull.Value);
                        if (companyDTO.LogoPath != null && companyDTO.LogoPath != "")
                            Command.Parameters.AddWithValue("@LogoPath", companyDTO.LogoPath);
                        else
                            Command.Parameters.AddWithValue("@LogoPath", DBNull.Value);
                        Command.Parameters.AddWithValue("@Link", companyDTO.Link);


                        SqlParameter RowsEffectedParam = new SqlParameter("@RowsEffected", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.ReturnValue
                        };
                        Command.Parameters.Add(RowsEffectedParam);

                        Command.ExecuteNonQuery();
                        RowsEffected = ((int)RowsEffectedParam.Value);


                    }
                }
            }
            catch (Exception Ex)
            {
                clsEventLogData.WriteEvent($" Message : {Ex.Message} \n\n Source : {Ex.Source} \n\n Target Site :  {Ex.TargetSite} \n\n Stack Trace :  {Ex.StackTrace}", EventLogEntryType.Error);
                RowsEffected = 0;
            }

            return (RowsEffected == 1);
        }

        public static bool DeleteCompany(int CompanyID)
        {
            int RowsEffected = 0;
            try
            {
                using (SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    Connection.Open();
                    using (SqlCommand Command = new SqlCommand("SP_DeleteCompany", Connection))
                    {
                        Command.CommandType = CommandType.StoredProcedure;
                        Command.Parameters.AddWithValue("@CompanyID", CompanyID);


                        SqlParameter RowsEffectedParam = new SqlParameter("@RowsEffected", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.ReturnValue
                        };
                        Command.Parameters.Add(RowsEffectedParam);

                        Command.ExecuteNonQuery();
                        RowsEffected = ((int)RowsEffectedParam.Value);

                    }
                }
            }
            catch (Exception Ex)
            {
                clsEventLogData.WriteEvent($" Message : {Ex.Message} \n\n Source : {Ex.Source} \n\n Target Site :  {Ex.TargetSite} \n\n Stack Trace :  {Ex.StackTrace}", EventLogEntryType.Error);
                RowsEffected = 0;
            }

            return RowsEffected == 1;
        }

        public static List<CompanyListDTO> GetAllCompanies()
        {
            List<CompanyListDTO> ListCompanies = new List<CompanyListDTO>();
            try
            {
                using (SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    Connection.Open();
                    using (SqlCommand Command = new SqlCommand("SP_GetAllCompanies", Connection))
                    {
                        Command.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader Reader = Command.ExecuteReader())
                        {
                            while (Reader.Read())
                            {
                                ListCompanies.Add
                                   (
                                      new CompanyListDTO
                                      (
                                         Reader.GetInt32(Reader.GetOrdinal("CompanyID")),
Reader.GetString(Reader.GetOrdinal("CompanyName")),
Reader.GetString(Reader.GetOrdinal("WilayaName")),
Reader.GetString(Reader.GetOrdinal("Email")),
Reader.GetBoolean(Reader.GetOrdinal("IsActive"))
                                      )
                                   );
                            }
                        }


                    }
                }
            }
            catch (Exception Ex)
            {
                clsEventLogData.WriteEvent($" Message : {Ex.Message} \n\n Source : {Ex.Source} \n\n Target Site :  {Ex.TargetSite} \n\n Stack Trace :  {Ex.StackTrace}", EventLogEntryType.Error);

            }

            return ListCompanies;
        }

        public static bool IsCompanyExistByID(int CompanyID)
        {
            bool IsFound = false;
            try
            {
                using (SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    Connection.Open();
                    using (SqlCommand Command = new SqlCommand("SP_IsCompanyExistByID", Connection))
                    {
                        Command.CommandType = CommandType.StoredProcedure;
                        Command.Parameters.AddWithValue("@CompanyID", CompanyID);
                        SqlParameter IsFoundParam = new SqlParameter("@IsFound", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.ReturnValue
                        };
                        Command.Parameters.Add(IsFoundParam);

                        Command.ExecuteNonQuery();
                        IsFound = ((int)IsFoundParam.Value == 1);

                    }
                }
            }
            catch (Exception Ex)
            {
                clsEventLogData.WriteEvent($" Message : {Ex.Message} \n\n Source : {Ex.Source} \n\n Target Site :  {Ex.TargetSite} \n\n Stack Trace :  {Ex.StackTrace}", EventLogEntryType.Error);
                IsFound = false;
            }
            return IsFound;
        }
        public static bool IsCompanyExistByUserID(int UserID)
        {
            bool IsFound = false;
            try
            {
                using (SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    Connection.Open();
                    using (SqlCommand Command = new SqlCommand("SP_IsCompanyExistByID", Connection))
                    {
                        Command.CommandType = CommandType.StoredProcedure;
                        Command.Parameters.AddWithValue("@UserID", UserID);
                        SqlParameter IsFoundParam = new SqlParameter("@IsFound", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.ReturnValue
                        };
                        Command.Parameters.Add(IsFoundParam);

                        Command.ExecuteNonQuery();
                        IsFound = ((int)IsFoundParam.Value == 1);

                    }
                }
            }
            catch (Exception Ex)
            {
                clsEventLogData.WriteEvent($" Message : {Ex.Message} \n\n Source : {Ex.Source} \n\n Target Site :  {Ex.TargetSite} \n\n Stack Trace :  {Ex.StackTrace}", EventLogEntryType.Error);
                IsFound = false;
            }
            return IsFound;
        }


        public static bool IsCompanyExistByEmail(string Email)
        {
            bool IsFound = false;
            try
            {
                using (SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    Connection.Open();
                    using (SqlCommand Command = new SqlCommand("SP_IsCompanyExistByEmail", Connection))
                    {
                        Command.CommandType = CommandType.StoredProcedure;
                        Command.Parameters.AddWithValue("@Email", Email);
                        SqlParameter IsFoundParam = new SqlParameter("@IsFound", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.ReturnValue
                        };
                        Command.Parameters.Add(IsFoundParam);

                        Command.ExecuteNonQuery();
                        IsFound = ((int)IsFoundParam.Value == 1);

                    }
                }
            }
            catch (Exception Ex)
            {
                clsEventLogData.WriteEvent($" Message : {Ex.Message} \n\n Source : {Ex.Source} \n\n Target Site :  {Ex.TargetSite} \n\n Stack Trace :  {Ex.StackTrace}", EventLogEntryType.Error);
                IsFound = false;
            }
            return IsFound;
        }

        public static bool IsCompanyExistByPhone(string Phone)
        {
            bool IsFound = false;
            try
            {
                using (SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    Connection.Open();
                    using (SqlCommand Command = new SqlCommand("SP_IsCompanyExistByPhone", Connection))
                    {
                        Command.CommandType = CommandType.StoredProcedure;
                        Command.Parameters.AddWithValue("@Phone", Phone);
                        SqlParameter IsFoundParam = new SqlParameter("@IsFound", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.ReturnValue
                        };
                        Command.Parameters.Add(IsFoundParam);

                        Command.ExecuteNonQuery();
                        IsFound = ((int)IsFoundParam.Value == 1);

                    }
                }
            }
            catch (Exception Ex)
            {
                clsEventLogData.WriteEvent($" Message : {Ex.Message} \n\n Source : {Ex.Source} \n\n Target Site :  {Ex.TargetSite} \n\n Stack Trace :  {Ex.StackTrace}", EventLogEntryType.Error);
                IsFound = false;
            }
            return IsFound;
        }

        public static bool IsCompanyExistByEmailAndPassword(string Email, string Password)
        {
            bool IsFound = false;
            try
            {
                using (SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    Connection.Open();
                    using (SqlCommand Command = new SqlCommand("SP_IsCompanyExistByEmailAndPassword", Connection))
                    {
                        Command.CommandType = CommandType.StoredProcedure;
                        Command.Parameters.AddWithValue("@Email", Email);
                        Command.Parameters.AddWithValue("@Password", Password);
                        SqlParameter IsFoundParam = new SqlParameter("@IsFound", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.ReturnValue
                        };
                        Command.Parameters.Add(IsFoundParam);

                        Command.ExecuteNonQuery();
                        IsFound = ((int)IsFoundParam.Value == 1);

                    }
                }
            }
            catch (Exception Ex)
            {
                clsEventLogData.WriteEvent($" Message : {Ex.Message} \n\n Source : {Ex.Source} \n\n Target Site :  {Ex.TargetSite} \n\n Stack Trace :  {Ex.StackTrace}", EventLogEntryType.Error);
                IsFound = false;
            }
            return IsFound;
        }


        public static CompanyDTO GetCompanyInfoByID(int CompanyID)
        {
            CompanyDTO companyDTO = null;
            try
            {
                using (SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    Connection.Open();
                    using (SqlCommand Command = new SqlCommand("SP_GetCompanyInfoByID", Connection))
                    {
                        Command.CommandType = CommandType.StoredProcedure;
                        Command.Parameters.AddWithValue("@CompanyID", CompanyID);
                        using (SqlDataReader Reader = Command.ExecuteReader())
                        {
                            if (Reader.Read())
                            {

                                companyDTO = new CompanyDTO(Reader.GetInt32(Reader.GetOrdinal("CompanyID")),
Reader.GetInt32(Reader.GetOrdinal("UserID")),
Reader.GetInt32(Reader.GetOrdinal("WilayaID")),
Reader.GetString(Reader.GetOrdinal("Name")),
Reader.IsDBNull(Reader.GetOrdinal("Description")) ? null : Reader.GetString(Reader.GetOrdinal("Description")),
Reader.IsDBNull(Reader.GetOrdinal("LogoPath")) ? null : Reader.GetString(Reader.GetOrdinal("LogoPath")),
Reader.GetString(Reader.GetOrdinal("Link"))
                                 );

                            }
                            else
                                companyDTO = null;
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                clsEventLogData.WriteEvent($" Message : {Ex.Message} \n\n Source : {Ex.Source} \n\n Target Site :  {Ex.TargetSite} \n\n Stack Trace :  {Ex.StackTrace}", EventLogEntryType.Error);
                companyDTO = null;
            }
            return companyDTO;

        }


        public static CompanyDTO GetCompanyInfoByUserID(int UserID)
        {
            CompanyDTO CompanyDTO = null;
            try
            {
                using (SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    Connection.Open();
                    using (SqlCommand Command = new SqlCommand("SP_GetCompanyInfoByUserID", Connection))
                    {
                        Command.CommandType = CommandType.StoredProcedure;
                        Command.Parameters.AddWithValue("@UserID", UserID);
                        using (SqlDataReader Reader = Command.ExecuteReader())
                        {
                            if (Reader.Read())
                            {

                                CompanyDTO = new CompanyDTO(Reader.GetInt32(Reader.GetOrdinal("CompanyID")),
Reader.GetInt32(Reader.GetOrdinal("UserID")),
Reader.GetInt32(Reader.GetOrdinal("WilayaID")),
Reader.GetString(Reader.GetOrdinal("Name")),
Reader.IsDBNull(Reader.GetOrdinal("Description")) ? null : Reader.GetString(Reader.GetOrdinal("Description")),
Reader.IsDBNull(Reader.GetOrdinal("LogoPath")) ? null : Reader.GetString(Reader.GetOrdinal("LogoPath")),
Reader.GetString(Reader.GetOrdinal("Link"))
                                 );

                            }
                            else
                                CompanyDTO = null;
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                clsEventLogData.WriteEvent($" Message : {Ex.Message} \n\n Source : {Ex.Source} \n\n Target Site :  {Ex.TargetSite} \n\n Stack Trace :  {Ex.StackTrace}", EventLogEntryType.Error);
                CompanyDTO = null;
            }
            return CompanyDTO;

        }

        public static CompanyDTO GetCompanyInfoByEmail(string Email)
        {
            CompanyDTO CompanyDTO = null;
            try
            {
                using (SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    Connection.Open();
                    using (SqlCommand Command = new SqlCommand("SP_GetCompanyInfoByEmail", Connection))
                    {
                        Command.CommandType = CommandType.StoredProcedure;
                        Command.Parameters.AddWithValue("@Email", Email);
                        using (SqlDataReader Reader = Command.ExecuteReader())
                        {
                            if (Reader.Read())
                            {

                                CompanyDTO = new CompanyDTO(Reader.GetInt32(Reader.GetOrdinal("CompanyID")),
Reader.GetInt32(Reader.GetOrdinal("UserID")),
Reader.GetInt32(Reader.GetOrdinal("WilayaID")),
Reader.GetString(Reader.GetOrdinal("Name")),
Reader.IsDBNull(Reader.GetOrdinal("Description")) ? null : Reader.GetString(Reader.GetOrdinal("Description")),
Reader.IsDBNull(Reader.GetOrdinal("LogoPath")) ? null : Reader.GetString(Reader.GetOrdinal("LogoPath")),
Reader.GetString(Reader.GetOrdinal("Link"))
                                 );

                            }
                            else
                                CompanyDTO = null;
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                clsEventLogData.WriteEvent($" Message : {Ex.Message} \n\n Source : {Ex.Source} \n\n Target Site :  {Ex.TargetSite} \n\n Stack Trace :  {Ex.StackTrace}", EventLogEntryType.Error);
                CompanyDTO = null;
            }
            return CompanyDTO;

        }

        public static CompanyDTO GetCompanyInfoByEmailAndPassword(string Email, string Password)
        {
            CompanyDTO CompanyDTO = null;
            try
            {
                using (SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    Connection.Open();
                    using (SqlCommand Command = new SqlCommand("SP_GetCompanyInfoByEmailAndPassword", Connection))
                    {
                        Command.CommandType = CommandType.StoredProcedure;
                        Command.Parameters.AddWithValue("@Email", Email);
                        Command.Parameters.AddWithValue("@Password", Password);
                        using (SqlDataReader Reader = Command.ExecuteReader())
                        {
                            if (Reader.Read())
                            {

                                CompanyDTO = new CompanyDTO(Reader.GetInt32(Reader.GetOrdinal("CompanyID")),
Reader.GetInt32(Reader.GetOrdinal("UserID")),
Reader.GetInt32(Reader.GetOrdinal("WilayaID")),
Reader.GetString(Reader.GetOrdinal("Name")),
Reader.IsDBNull(Reader.GetOrdinal("Description")) ? null : Reader.GetString(Reader.GetOrdinal("Description")),
Reader.IsDBNull(Reader.GetOrdinal("LogoPath")) ? null : Reader.GetString(Reader.GetOrdinal("LogoPath")),
Reader.GetString(Reader.GetOrdinal("Link"))
                                 );

                            }
                            else
                                CompanyDTO = null;
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                clsEventLogData.WriteEvent($" Message : {Ex.Message} \n\n Source : {Ex.Source} \n\n Target Site :  {Ex.TargetSite} \n\n Stack Trace :  {Ex.StackTrace}", EventLogEntryType.Error);
                CompanyDTO = null;
            }
            return CompanyDTO;

        }


    }
}
