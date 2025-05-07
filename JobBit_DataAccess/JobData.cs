using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;
using Dapper;
using System.Text.Json;
using Microsoft.Data.SqlClient;

namespace JobBit_DataAccess
{
    public class JobDTO
    {
        public int JobID { get; set; }
        public int CompanyID { get; set; }
        public byte JobType { get; set; }
        public DateTime PostedDate { get; set; }
        public byte Experience { get; set; }
        public bool Available { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }

        public JobDTO(int JobID, int CompanyID, byte JobType, DateTime PostedDate, byte Experience, bool Available, string? Description, string title)
        {
            this.JobID = JobID;
            this.CompanyID = CompanyID;
            this.JobType = JobType;
            this.PostedDate = PostedDate;
            this.Experience = Experience;
            this.Available = Available;
            this.Description = Description;
            this.Title = title;
        }

    }

    public class JobListWithOnlyIconUrlSkillsDTO
    {
        public int JobID { get; set; }
        public string Title { get; set; }
        public DateTime PostedDate { get; set; }

        public string WilayaName { get; set; }

        public string CompanyName { get; set; }

        public string? LogoPath { get; set; }

        public List<string> skillsIconUrl { get; set; }


        public JobListWithOnlyIconUrlSkillsDTO(int jobID, string title, DateTime postedDate, string wilayaName, string companyName, string? logoPath, List<string> skills)
        {
            JobID = jobID;
            Title = title;
            PostedDate = postedDate;
            WilayaName = wilayaName;
            CompanyName = companyName;
            LogoPath = logoPath;
            this.skillsIconUrl = skills;
        }
    }

    public class JobListForCompanyDTO
    {
        public int JobID { get; set; }
        public string Title { get; set; }
        public DateTime PostedDate { get; set; }
        public bool Available { get; set; }

        public JobListForCompanyDTO(int jobID, string title, DateTime postedDate, bool available)
        {
            JobID = jobID;
            Title = title;
            PostedDate = postedDate;
            Available = available;
        }
    }

   

    public class JobData
    {
        public static List<string> GetAllJobsTitles()
        {
            List<string> JobsTitles = new List<string>();

            try
            {
                using (SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    Connection.Open();
                    using (SqlCommand Command = new SqlCommand("SP_GetAllJobsTitles", Connection))
                    {
                        Command.CommandType = CommandType.StoredProcedure;


                        using (SqlDataReader Reader = Command.ExecuteReader())
                        {

                            while (Reader.Read())
                            {


                                JobsTitles.Add(

                                    Reader.GetString(Reader.GetOrdinal("Title"))

                                );


                            }


                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                clsEventLogData.WriteEvent(
                    $" Message : {Ex.Message} \n\n Source : {Ex.Source} \n\n Target Site :  {Ex.TargetSite} \n\n Stack Trace :  {Ex.StackTrace}",
                    EventLogEntryType.Error);
            }

            return JobsTitles;
        }




        public static List<JobListWithOnlyIconUrlSkillsDTO> FilterJobs(List<int>? WilayaIDs, List<int>? SkillIDs,
List<int>? JobTypeIDs, List<int>? JobExperienceIDs)
        {
            List<JobListWithOnlyIconUrlSkillsDTO> JobLists = new List<JobListWithOnlyIconUrlSkillsDTO>();

            try
            {
                using (SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    Connection.Open();
                    using (SqlCommand Command = new SqlCommand("SP_FilterJobs", Connection))
                    {
                        Command.CommandType = CommandType.StoredProcedure;

                        Command.Parameters.Add("@SkillIDs", SqlDbType.NVarChar, -1).Value =
                            SkillIDs != null && SkillIDs.Count > 0 ? (object)string.Join(",", SkillIDs) : DBNull.Value;
                        Command.Parameters.Add("@WilayaIDs", SqlDbType.NVarChar, -1).Value =
                            WilayaIDs != null && WilayaIDs.Count > 0 ? (object)string.Join(",", WilayaIDs) : DBNull.Value;
                        Command.Parameters.Add("@JobTypeIDs", SqlDbType.NVarChar, -1).Value =
                            JobTypeIDs != null && JobTypeIDs.Count > 0 ? (object)string.Join(",", JobTypeIDs) : DBNull.Value;
                        Command.Parameters.Add("@JobExperienceIDs", SqlDbType.NVarChar, -1).Value =
                            JobExperienceIDs != null && JobExperienceIDs.Count > 0 ? (object)string.Join(",", JobExperienceIDs) : DBNull.Value;

                        using (SqlDataReader Reader = Command.ExecuteReader())
                        {
                            
                            while (Reader.Read())
                            {
                                
                                string iconUrlRaw = Reader.IsDBNull(Reader.GetOrdinal("IconUrls")) ? "" : Reader.GetString(Reader.GetOrdinal("IconUrls"));
                                List<string> iconUrls = iconUrlRaw.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();


                                JobLists.Add(
                                    new JobListWithOnlyIconUrlSkillsDTO(
                                    Reader.GetInt32(Reader.GetOrdinal("JobID")),
                                    Reader.GetString(Reader.GetOrdinal("Title")),
                                    Reader.GetDateTime(Reader.GetOrdinal("PostedDate")),
                                    Reader.GetString(Reader.GetOrdinal("WilayaName")),
                                    Reader.GetString(Reader.GetOrdinal("CompanyName")),
                                    Reader.IsDBNull(Reader.GetOrdinal("LogoPath")) ? null : Reader.GetString(Reader.GetOrdinal("LogoPath")),
                                    iconUrls
                                )


                                    );


                            }

                            
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                clsEventLogData.WriteEvent(
                    $" Message : {Ex.Message} \n\n Source : {Ex.Source} \n\n Target Site :  {Ex.TargetSite} \n\n Stack Trace :  {Ex.StackTrace}",
                    EventLogEntryType.Error);
            }

            return JobLists;
        }

        public static List<JobListWithOnlyIconUrlSkillsDTO> FilterJobsByJobTitle(string SearchTerm)
        {
            List<JobListWithOnlyIconUrlSkillsDTO> JobLists = new List<JobListWithOnlyIconUrlSkillsDTO>();

            try
            {
                using (SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    Connection.Open();
                    using (SqlCommand Command = new SqlCommand("SP_FilterJobsByJobTitle", Connection))
                    {
                        Command.CommandType = CommandType.StoredProcedure;

                        Command.Parameters.AddWithValue("@SearchTerm", SearchTerm);


                        using (SqlDataReader Reader = Command.ExecuteReader())
                        {

                            while (Reader.Read())
                            {

                                string iconUrlRaw = Reader.IsDBNull(Reader.GetOrdinal("IconUrls")) ? "" : Reader.GetString(Reader.GetOrdinal("IconUrls"));
                                List<string> iconUrls = iconUrlRaw.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();


                                JobLists.Add(
                                    new JobListWithOnlyIconUrlSkillsDTO(
                                    Reader.GetInt32(Reader.GetOrdinal("JobID")),
                                    Reader.GetString(Reader.GetOrdinal("Title")),
                                    Reader.GetDateTime(Reader.GetOrdinal("PostedDate")),
                                    Reader.GetString(Reader.GetOrdinal("WilayaName")),
                                    Reader.GetString(Reader.GetOrdinal("CompanyName")),
                                    Reader.IsDBNull(Reader.GetOrdinal("LogoPath")) ? null : Reader.GetString(Reader.GetOrdinal("LogoPath")),
                                    iconUrls
                                )


                                    );


                            }


                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                clsEventLogData.WriteEvent(
                    $" Message : {Ex.Message} \n\n Source : {Ex.Source} \n\n Target Site :  {Ex.TargetSite} \n\n Stack Trace :  {Ex.StackTrace}",
                    EventLogEntryType.Error);
            }

            return JobLists;
        }



        public static List<JobListWithOnlyIconUrlSkillsDTO> GetAllJobs()
        {
            List<JobListWithOnlyIconUrlSkillsDTO> JobLists = new List<JobListWithOnlyIconUrlSkillsDTO>();

            try
            {
                using (SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    Connection.Open();
                    using (SqlCommand Command = new SqlCommand("SP_GetAllJobs", Connection))
                    {
                        Command.CommandType = CommandType.StoredProcedure;

                       
                        using (SqlDataReader Reader = Command.ExecuteReader())
                        {

                            while (Reader.Read())
                            {

                                string iconUrlRaw = Reader.IsDBNull(Reader.GetOrdinal("IconUrls")) ? "" : Reader.GetString(Reader.GetOrdinal("IconUrls"));
                                List<string> iconUrls = iconUrlRaw.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();


                                JobLists.Add(
                                    new JobListWithOnlyIconUrlSkillsDTO(
                                    Reader.GetInt32(Reader.GetOrdinal("JobID")),
                                    Reader.GetString(Reader.GetOrdinal("Title")),
                                    Reader.GetDateTime(Reader.GetOrdinal("PostedDate")),
                                    Reader.GetString(Reader.GetOrdinal("WilayaName")),
                                    Reader.GetString(Reader.GetOrdinal("CompanyName")),
                                    Reader.IsDBNull(Reader.GetOrdinal("LogoPath")) ? null : Reader.GetString(Reader.GetOrdinal("LogoPath")),
                                    iconUrls
                                )


                                    );


                            }


                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                clsEventLogData.WriteEvent(
                    $" Message : {Ex.Message} \n\n Source : {Ex.Source} \n\n Target Site :  {Ex.TargetSite} \n\n Stack Trace :  {Ex.StackTrace}",
                    EventLogEntryType.Error);
            }

            return JobLists;
        }

        public static int AddNewJob(JobDTO jobDTO)
        {
            int JobID = -1;
            try
            {
                using (SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    Connection.Open();
                    using (SqlCommand Command = new SqlCommand("SP_AddNewJob", Connection))
                    {
                        Command.CommandType = CommandType.StoredProcedure;
                        Command.Parameters.AddWithValue("@CompanyID", jobDTO.CompanyID);
                        Command.Parameters.AddWithValue("@JobType", jobDTO.JobType);
                        Command.Parameters.AddWithValue("@PostedDate", jobDTO.PostedDate);
                        Command.Parameters.AddWithValue("@Experience", jobDTO.Experience);
                        Command.Parameters.AddWithValue("@Available", jobDTO.Available);
                        Command.Parameters.AddWithValue("@Title", jobDTO.Title);
                        if (jobDTO.Description != null && jobDTO.Description != "")
                            Command.Parameters.AddWithValue("@Description", jobDTO.Description);
                        else
                            Command.Parameters.AddWithValue("@Description", DBNull.Value);


                        SqlParameter outputIdParam = new SqlParameter("@JobID", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        Command.Parameters.Add(outputIdParam);


                        Command.ExecuteNonQuery();

                        JobID = (int)outputIdParam.Value;

                    }
                }

            }
            catch (Exception Ex)
            {
                clsEventLogData.WriteEvent($" Message : {Ex.Message} \n\n Source : {Ex.Source} \n\n Target Site :  {Ex.TargetSite} \n\n Stack Trace :  {Ex.StackTrace}", EventLogEntryType.Error);
                JobID = -1;
            }
            return JobID;
        }

        public static bool UpdateJob(JobDTO jobDTO)
        {
            int RowsEffected = 0;
            try
            {
                using (SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    Connection.Open();
                    using (SqlCommand Command = new SqlCommand("SP_UpdateJob", Connection))
                    {
                        Command.CommandType = CommandType.StoredProcedure;
                        Command.Parameters.AddWithValue("@JobID", jobDTO.JobID);
                        Command.Parameters.AddWithValue("@CompanyID", jobDTO.CompanyID);
                        Command.Parameters.AddWithValue("@JobType", jobDTO.JobType);
                        Command.Parameters.AddWithValue("@Experience", jobDTO.Experience);
                        Command.Parameters.AddWithValue("@Available", jobDTO.Available);
                        Command.Parameters.AddWithValue("@Title", jobDTO.Title);
                        if (jobDTO.Description != null && jobDTO.Description != "")
                            Command.Parameters.AddWithValue("@Description", jobDTO.Description);
                        else
                            Command.Parameters.AddWithValue("@Description", DBNull.Value);


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

       

        public static bool DeleteJob(int JobID)
        {
            int RowsEffected = 0;
            try
            {
                using (SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    Connection.Open();
                    using (SqlCommand Command = new SqlCommand("SP_DeleteJob", Connection))
                    {
                        Command.CommandType = CommandType.StoredProcedure;
                        Command.Parameters.AddWithValue("@JobID", JobID);


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

        public static List<JobListForCompanyDTO> GetAllJobsForCompany(int CompanyID)
        {
            List<JobListForCompanyDTO> ListJobs = new List<JobListForCompanyDTO>();
            try
            {
                using (SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    Connection.Open();
                    using (SqlCommand Command = new SqlCommand("SP_GetAllJobsForCompany", Connection))
                    {
                        Command.CommandType = CommandType.StoredProcedure;
                        Command.Parameters.AddWithValue("@CompanyID", CompanyID);
                        using (SqlDataReader Reader = Command.ExecuteReader())
                        {
                            while (Reader.Read())
                            {
                                ListJobs.Add
                                   (
                                      new JobListForCompanyDTO
                                      (
                                         Reader.GetInt32(Reader.GetOrdinal("JobID")),
                                         Reader.GetString(Reader.GetOrdinal("Title")),
                                         Reader.GetDateTime(Reader.GetOrdinal("PostedDate")),
Reader.GetBoolean(Reader.GetOrdinal("Available"))

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

            return ListJobs;
        }

     


        public static bool IsJobExistByID(int JobID)
        {
            bool IsFound = false;
            try
            {
                using (SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    Connection.Open();
                    using (SqlCommand Command = new SqlCommand("SP_IsJobExistByID", Connection))
                    {
                        Command.CommandType = CommandType.StoredProcedure;
                        Command.Parameters.AddWithValue("@JobID", JobID);
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

        public static JobDTO GetJobInfoByID(int JobID)
        {
            JobDTO jobDTO = null;
            try
            {
                using (SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    Connection.Open();
                    using (SqlCommand Command = new SqlCommand("SP_GetJobInfoByID", Connection))
                    {
                        Command.CommandType = CommandType.StoredProcedure;
                        Command.Parameters.AddWithValue("@JobID", JobID);
                        using (SqlDataReader Reader = Command.ExecuteReader())
                        {
                            if (Reader.Read())
                            {

                                jobDTO = new JobDTO
                                      (
                                         Reader.GetInt32(Reader.GetOrdinal("JobID")),
Reader.GetInt32(Reader.GetOrdinal("CompanyID")),
Reader.GetByte(Reader.GetOrdinal("JobType")),
Reader.GetDateTime(Reader.GetOrdinal("PostedDate")),
Reader.GetByte(Reader.GetOrdinal("Experience")),
Reader.GetBoolean(Reader.GetOrdinal("Available")),
Reader.IsDBNull(Reader.GetOrdinal("Description")) ? null : Reader.GetString(Reader.GetOrdinal("Description")),
Reader.GetString(Reader.GetOrdinal("Title"))
                                      
                                   );

                            }
                            else
                                jobDTO = null;
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                clsEventLogData.WriteEvent($" Message : {Ex.Message} \n\n Source : {Ex.Source} \n\n Target Site :  {Ex.TargetSite} \n\n Stack Trace :  {Ex.StackTrace}", EventLogEntryType.Error);
                jobDTO = null;
            }
            return jobDTO;

        }

        public static bool IsCompanyPostedJob(int CompanyID,  int JobID)
        {
            bool IsFound = false;
            try
            {
                using (SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    Connection.Open();
                    using (SqlCommand Command = new SqlCommand("SP_IsCompanyPostedJob", Connection))
                    {
                        Command.CommandType = CommandType.StoredProcedure;
                        Command.Parameters.AddWithValue("@JobID", JobID);
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
    }
}
