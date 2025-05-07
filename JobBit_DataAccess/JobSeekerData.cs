using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using static JobBit_DataAccess.JobSeekerData;

namespace JobBit_DataAccess
{
    public class JobSeekerDTO
    {
        public int JobSeekerID { get; set; }
        public int UserID { get; set; }
        public int? WilayaID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? ProfilePicturePath { get; set; }
        public string? CvFilePath { get; set; }
        public string? LinkProfileLinkden { get; set; }
        public string? LinkProfileGithub { get; set; }

        public JobSeekerDTO(int JobSeekerID, int UserID, int? WilayaID, string FirstName, string LastName, DateTime? DateOfBirth, string? ProfilePicturePath, string? CvFilePath, string? LinkProfileLinkden, string? LinkProfileGithub)
        {
            this.JobSeekerID = JobSeekerID;
            this.UserID = UserID;
            this.WilayaID = WilayaID;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.DateOfBirth = DateOfBirth;
            this.ProfilePicturePath = ProfilePicturePath;
            this.CvFilePath = CvFilePath;
            this.LinkProfileLinkden = LinkProfileLinkden;
            this.LinkProfileGithub = LinkProfileGithub;
        }

    }
    public class JobSeekerListDTO
    {
        public int JobSeekerID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Email { get; set; }

        public bool IsActive { get; set; }

        public JobSeekerListDTO(int jobSeekerID, string firstName, string lastName, string email, bool isActive)
        {
            JobSeekerID = jobSeekerID;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            IsActive = isActive;
        }
    }
    public class JobSeekerData
    {
       

        

        public static int AddNewJobSeeker(JobSeekerDTO jobseekerDTO)
        {
            int JobSeekerID = -1;
            try
            {
                using (SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    Connection.Open();
                    using (SqlCommand Command = new SqlCommand("SP_AddNewJobSeeker", Connection))
                    {
                        Command.CommandType = CommandType.StoredProcedure;
                        Command.Parameters.AddWithValue("@UserID", jobseekerDTO.UserID);
                        if (jobseekerDTO.WilayaID != null && jobseekerDTO.WilayaID != -1)
                            Command.Parameters.AddWithValue("@WilayaID", jobseekerDTO.WilayaID);
                        else
                            Command.Parameters.AddWithValue("@WilayaID", DBNull.Value);

                        Command.Parameters.AddWithValue("@FirstName", jobseekerDTO.FirstName);
                        Command.Parameters.AddWithValue("@LastName", jobseekerDTO.LastName);

                        if (jobseekerDTO.DateOfBirth != null)
                            Command.Parameters.AddWithValue("@DateOfBirth", jobseekerDTO.DateOfBirth);
                        else
                            Command.Parameters.AddWithValue("@DateOfBirth", DBNull.Value);

              
                        if (jobseekerDTO.ProfilePicturePath != null && jobseekerDTO.ProfilePicturePath != "")
                            Command.Parameters.AddWithValue("@ProfilePicturePath", jobseekerDTO.ProfilePicturePath);
                        else
                            Command.Parameters.AddWithValue("@ProfilePicturePath", DBNull.Value);

                        if (jobseekerDTO.CvFilePath != null && jobseekerDTO.CvFilePath != "")
                            Command.Parameters.AddWithValue("@CvFilePath", jobseekerDTO.CvFilePath);
                        else
                            Command.Parameters.AddWithValue("@CvFilePath", DBNull.Value);


                        


                        if (jobseekerDTO.LinkProfileLinkden != null && jobseekerDTO.LinkProfileLinkden != "")
                            Command.Parameters.AddWithValue("@LinkProfileLinkden", jobseekerDTO.LinkProfileLinkden);
                        else
                            Command.Parameters.AddWithValue("@LinkProfileLinkden", DBNull.Value);

                        if (jobseekerDTO.LinkProfileGithub != null && jobseekerDTO.LinkProfileGithub != "")
                            Command.Parameters.AddWithValue("@LinkProfileGithub", jobseekerDTO.LinkProfileGithub);
                        else
                            Command.Parameters.AddWithValue("@LinkProfileGithub", DBNull.Value);


                        SqlParameter outputIdParam = new SqlParameter("@JobSeekerID", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        Command.Parameters.Add(outputIdParam);


                        Command.ExecuteNonQuery();

                        JobSeekerID = (int)outputIdParam.Value;

                    }
                }

            }
            catch (Exception Ex)
            {
                clsEventLogData.WriteEvent($" Message : {Ex.Message} \n\n Source : {Ex.Source} \n\n Target Site :  {Ex.TargetSite} \n\n Stack Trace :  {Ex.StackTrace}", EventLogEntryType.Error);
                JobSeekerID = -1;
            }
            return JobSeekerID;
        }

        public static bool UpdateJobSeeker(JobSeekerDTO jobseekerDTO)
        {
            int RowsEffected = 0;
            try
            {
                using (SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    Connection.Open();
                    using (SqlCommand Command = new SqlCommand("SP_UpdateJobSeeker", Connection))
                    {
                        Command.CommandType = CommandType.StoredProcedure;
                        Command.Parameters.AddWithValue("@JobSeekerID", jobseekerDTO.JobSeekerID);
                        Command.Parameters.AddWithValue("@UserID", jobseekerDTO.UserID);
                        if (jobseekerDTO.WilayaID != null && jobseekerDTO.WilayaID != -1)
                            Command.Parameters.AddWithValue("@WilayaID", jobseekerDTO.WilayaID);
                        else
                            Command.Parameters.AddWithValue("@WilayaID", DBNull.Value);

                        Command.Parameters.AddWithValue("@FirstName", jobseekerDTO.FirstName);
                        Command.Parameters.AddWithValue("@LastName", jobseekerDTO.LastName);

                        if (jobseekerDTO.DateOfBirth != null)
                            Command.Parameters.AddWithValue("@DateOfBirth", jobseekerDTO.DateOfBirth);
                        else
                            Command.Parameters.AddWithValue("@DateOfBirth", DBNull.Value);


                        if (jobseekerDTO.ProfilePicturePath != null && jobseekerDTO.ProfilePicturePath != "")
                            Command.Parameters.AddWithValue("@ProfilePicturePath", jobseekerDTO.ProfilePicturePath);
                        else
                            Command.Parameters.AddWithValue("@ProfilePicturePath", DBNull.Value);

                        if (jobseekerDTO.CvFilePath != null && jobseekerDTO.CvFilePath != "")
                            Command.Parameters.AddWithValue("@CvFilePath", jobseekerDTO.CvFilePath);
                        else
                            Command.Parameters.AddWithValue("@CvFilePath", DBNull.Value);


                        if (jobseekerDTO.LinkProfileLinkden != null && jobseekerDTO.LinkProfileLinkden != "")
                            Command.Parameters.AddWithValue("@LinkProfileLinkden", jobseekerDTO.LinkProfileLinkden);
                        else
                            Command.Parameters.AddWithValue("@LinkProfileLinkden", DBNull.Value);

                        if (jobseekerDTO.LinkProfileGithub != null && jobseekerDTO.LinkProfileGithub != "")
                            Command.Parameters.AddWithValue("@LinkProfileGithub", jobseekerDTO.LinkProfileGithub);
                        else
                            Command.Parameters.AddWithValue("@LinkProfileGithub", DBNull.Value);

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

        public static bool DeleteJobSeeker(int JobSeekerID)
        {
            int RowsEffected = 0;
            try
            {
                using (SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    Connection.Open();
                    using (SqlCommand Command = new SqlCommand("SP_DeleteJobSeeker", Connection))
                    {
                        Command.CommandType = CommandType.StoredProcedure;
                        Command.Parameters.AddWithValue("@JobSeekerID", JobSeekerID);


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

        public static List<JobSeekerListDTO> GetAllJobSeekers()
        {
            List<JobSeekerListDTO> ListJobSeekers = new List<JobSeekerListDTO>();
            try
            {
                using (SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    Connection.Open();
                    using (SqlCommand Command = new SqlCommand("SP_GetAllJobSeekers", Connection))
                    {
                        Command.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader Reader = Command.ExecuteReader())
                        {
                            while (Reader.Read())
                            {
                                ListJobSeekers.Add
                                (
                                       new JobSeekerListDTO(Reader.GetInt32(Reader.GetOrdinal("JobSeekerID")),
Reader.GetString(Reader.GetOrdinal("FirstName")),
Reader.GetString(Reader.GetOrdinal("LastName")),
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

            return ListJobSeekers;
        }

        public static bool IsJobSeekerExistByID(int JobSeekerID)
        {
            bool IsFound = false;
            try
            {
                using (SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    Connection.Open();
                    using (SqlCommand Command = new SqlCommand("SP_IsJobSeekerExistByID", Connection))
                    {
                        Command.CommandType = CommandType.StoredProcedure;
                        Command.Parameters.AddWithValue("@JobSeekerID", JobSeekerID);
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

        public static bool IsJobSeekerExistByUserID(int UserID)
        {
            bool IsFound = false;
            try
            {
                using (SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    Connection.Open();
                    using (SqlCommand Command = new SqlCommand("SP_IsJobSeekerExistByUserID", Connection))
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

        public static bool IsJobSeekerExistByEmail(string Email)
        {
            bool IsFound = false;
            try
            {
                using (SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    Connection.Open();
                    using (SqlCommand Command = new SqlCommand("SP_IsJobSeekerExistByEmail", Connection))
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

        public static bool IsJobSeekerExistByPhone(string Phone)
        {
            bool IsFound = false;
            try
            {
                using (SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    Connection.Open();
                    using (SqlCommand Command = new SqlCommand("SP_IsJobSeekerExistByPhone", Connection))
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

        public static bool IsJobSeekerExistByEmailAndPassword(string Email,string Password)
        {
            bool IsFound = false;
            try
            {
                using (SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    Connection.Open();
                    using (SqlCommand Command = new SqlCommand("SP_IsJobSeekerExistByEmailAndPassword", Connection))
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


        public static JobSeekerDTO GetJobSeekerInfoByID(int JobSeekerID)
        {
            JobSeekerDTO jobseekerDTO = null;
            try
            {
                using (SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    Connection.Open();
                    using (SqlCommand Command = new SqlCommand("SP_GetJobSeekerInfoByID", Connection))
                    {
                        Command.CommandType = CommandType.StoredProcedure;
                        Command.Parameters.AddWithValue("@JobSeekerID", JobSeekerID);
                        using (SqlDataReader Reader = Command.ExecuteReader())
                        {
                            if (Reader.Read())
                            {

                                jobseekerDTO = new JobSeekerDTO(Reader.GetInt32(Reader.GetOrdinal("JobSeekerID")),
Reader.GetInt32(Reader.GetOrdinal("UserID")),
Reader.IsDBNull(Reader.GetOrdinal("WilayaID")) ? null : Reader.GetInt32(Reader.GetOrdinal("WilayaID")),
Reader.GetString(Reader.GetOrdinal("FirstName")),
Reader.GetString(Reader.GetOrdinal("LastName")),
Reader.IsDBNull(Reader.GetOrdinal("DateOfBirth")) ? null : Reader.GetDateTime(Reader.GetOrdinal("DateOfBirth")),
Reader.IsDBNull(Reader.GetOrdinal("ProfilePicturePath")) ? null : Reader.GetString(Reader.GetOrdinal("ProfilePicturePath")),
Reader.IsDBNull(Reader.GetOrdinal("CvFilePath")) ? null : Reader.GetString(Reader.GetOrdinal("CvFilePath")),
Reader.IsDBNull(Reader.GetOrdinal("LinkProfileLinkden")) ? null : Reader.GetString(Reader.GetOrdinal("LinkProfileLinkden")),
Reader.IsDBNull(Reader.GetOrdinal("LinkProfileGithub")) ? null : Reader.GetString(Reader.GetOrdinal("LinkProfileGithub"))
                                 );

                            }
                            else
                                jobseekerDTO = null;
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                clsEventLogData.WriteEvent($" Message : {Ex.Message} \n\n Source : {Ex.Source} \n\n Target Site :  {Ex.TargetSite} \n\n Stack Trace :  {Ex.StackTrace}", EventLogEntryType.Error);
                jobseekerDTO = null;
            }
            return jobseekerDTO;

        }

        public static JobSeekerDTO GetJobSeekerInfoByUserID(int UserID)
        {
            JobSeekerDTO jobseekerDTO = null;
            try
            {
                using (SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    Connection.Open();
                    using (SqlCommand Command = new SqlCommand("SP_GetJobSeekerInfoByUserID", Connection))
                    {
                        Command.CommandType = CommandType.StoredProcedure;
                        Command.Parameters.AddWithValue("@UserID", UserID);
                        using (SqlDataReader Reader = Command.ExecuteReader())
                        {
                            if (Reader.Read())
                            {

                                jobseekerDTO = new JobSeekerDTO(Reader.GetInt32(Reader.GetOrdinal("JobSeekerID")),
Reader.GetInt32(Reader.GetOrdinal("UserID")),
Reader.IsDBNull(Reader.GetOrdinal("WilayaID")) ? null : Reader.GetInt32(Reader.GetOrdinal("WilayaID")),
Reader.GetString(Reader.GetOrdinal("FirstName")),
Reader.GetString(Reader.GetOrdinal("LastName")),
Reader.IsDBNull(Reader.GetOrdinal("DateOfBirth")) ? null : Reader.GetDateTime(Reader.GetOrdinal("DateOfBirth")),
Reader.IsDBNull(Reader.GetOrdinal("ProfilePicturePath")) ? null : Reader.GetString(Reader.GetOrdinal("ProfilePicturePath")),
Reader.IsDBNull(Reader.GetOrdinal("CvFilePath")) ? null : Reader.GetString(Reader.GetOrdinal("CvFilePath")),
Reader.IsDBNull(Reader.GetOrdinal("LinkProfileLinkden")) ? null : Reader.GetString(Reader.GetOrdinal("LinkProfileLinkden")),
Reader.IsDBNull(Reader.GetOrdinal("LinkProfileGithub")) ? null : Reader.GetString(Reader.GetOrdinal("LinkProfileGithub"))
                                 );

                            }
                            else
                                jobseekerDTO = null;
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                clsEventLogData.WriteEvent($" Message : {Ex.Message} \n\n Source : {Ex.Source} \n\n Target Site :  {Ex.TargetSite} \n\n Stack Trace :  {Ex.StackTrace}", EventLogEntryType.Error);
                jobseekerDTO = null;
            }
            return jobseekerDTO;

        }

        public static JobSeekerDTO GetJobSeekerInfoByEmail(string Email)
        {
            JobSeekerDTO jobseekerDTO = null;
            try
            {
                using (SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    Connection.Open();
                    using (SqlCommand Command = new SqlCommand("SP_GetJobSeekerInfoByEmail", Connection))
                    {
                        Command.CommandType = CommandType.StoredProcedure;
                        Command.Parameters.AddWithValue("@Email", Email);
                        using (SqlDataReader Reader = Command.ExecuteReader())
                        {
                            if (Reader.Read())
                            {

                                jobseekerDTO = new JobSeekerDTO(Reader.GetInt32(Reader.GetOrdinal("JobSeekerID")),
Reader.GetInt32(Reader.GetOrdinal("UserID")),
Reader.IsDBNull(Reader.GetOrdinal("WilayaID")) ? null : Reader.GetInt32(Reader.GetOrdinal("WilayaID")),
Reader.GetString(Reader.GetOrdinal("FirstName")),
Reader.GetString(Reader.GetOrdinal("LastName")),
Reader.IsDBNull(Reader.GetOrdinal("DateOfBirth")) ? null : Reader.GetDateTime(Reader.GetOrdinal("DateOfBirth")),
Reader.IsDBNull(Reader.GetOrdinal("ProfilePicturePath")) ? null : Reader.GetString(Reader.GetOrdinal("ProfilePicturePath")),
Reader.IsDBNull(Reader.GetOrdinal("CvFilePath")) ? null : Reader.GetString(Reader.GetOrdinal("CvFilePath")),
Reader.IsDBNull(Reader.GetOrdinal("LinkProfileLinkden")) ? null : Reader.GetString(Reader.GetOrdinal("LinkProfileLinkden")),
Reader.IsDBNull(Reader.GetOrdinal("LinkProfileGithub")) ? null : Reader.GetString(Reader.GetOrdinal("LinkProfileGithub"))
                                 );

                            }
                            else
                                jobseekerDTO = null;
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                clsEventLogData.WriteEvent($" Message : {Ex.Message} \n\n Source : {Ex.Source} \n\n Target Site :  {Ex.TargetSite} \n\n Stack Trace :  {Ex.StackTrace}", EventLogEntryType.Error);
                jobseekerDTO = null;
            }
            return jobseekerDTO;

        }

        public static JobSeekerDTO GetJobSeekerInfoByEmailAndPassword(string Email,string Password)
        {
            JobSeekerDTO jobseekerDTO = null;
            try
            {
                using (SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    Connection.Open();
                    using (SqlCommand Command = new SqlCommand("SP_GetJobSeekerInfoByEmailAndPassword", Connection))
                    {
                        Command.CommandType = CommandType.StoredProcedure;
                        Command.Parameters.AddWithValue("@Email", Email);
                        Command.Parameters.AddWithValue("@Password", Password);
                        using (SqlDataReader Reader = Command.ExecuteReader())
                        {
                            if (Reader.Read())
                            {

                                jobseekerDTO = new JobSeekerDTO(Reader.GetInt32(Reader.GetOrdinal("JobSeekerID")),
Reader.GetInt32(Reader.GetOrdinal("UserID")),
Reader.IsDBNull(Reader.GetOrdinal("WilayaID")) ? null : Reader.GetInt32(Reader.GetOrdinal("WilayaID")),
Reader.GetString(Reader.GetOrdinal("FirstName")),
Reader.GetString(Reader.GetOrdinal("LastName")),
Reader.IsDBNull(Reader.GetOrdinal("DateOfBirth")) ? null : Reader.GetDateTime(Reader.GetOrdinal("DateOfBirth")),
Reader.IsDBNull(Reader.GetOrdinal("ProfilePicturePath")) ? null : Reader.GetString(Reader.GetOrdinal("ProfilePicturePath")),
Reader.IsDBNull(Reader.GetOrdinal("CvFilePath")) ? null : Reader.GetString(Reader.GetOrdinal("CvFilePath")),
Reader.IsDBNull(Reader.GetOrdinal("LinkProfileLinkden")) ? null : Reader.GetString(Reader.GetOrdinal("LinkProfileLinkden")),
Reader.IsDBNull(Reader.GetOrdinal("LinkProfileGithub")) ? null : Reader.GetString(Reader.GetOrdinal("LinkProfileGithub"))
                                 );

                            }
                            else
                                jobseekerDTO = null;
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                clsEventLogData.WriteEvent($" Message : {Ex.Message} \n\n Source : {Ex.Source} \n\n Target Site :  {Ex.TargetSite} \n\n Stack Trace :  {Ex.StackTrace}", EventLogEntryType.Error);
                jobseekerDTO = null;
            }
            return jobseekerDTO;

        }


    }
}
