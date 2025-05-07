using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace JobBit_DataAccess
{
    public class JobSkillDTO
    {
        public int JobSkillID { get; set; }
        public int JobID { get; set; }
        public int SkillID { get; set; }

        public JobSkillDTO(int JobSkillID, int JobID, int SkillID)
        {
            this.JobSkillID = JobSkillID;
            this.JobID = JobID;
            this.SkillID = SkillID;
        }

    }
    public class JobSkillData
    {


       

        public static int AddNewJobSkill(JobSkillDTO jobskillDTO)
        {
            int JobSkillID = -1;
            try
            {
                using (SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    Connection.Open();
                    using (SqlCommand Command = new SqlCommand("SP_AddNewJobSkill", Connection))
                    {
                        Command.CommandType = CommandType.StoredProcedure;
                        Command.Parameters.AddWithValue("@JobID", jobskillDTO.JobID);
                        Command.Parameters.AddWithValue("@SkillID", jobskillDTO.SkillID);


                        SqlParameter outputIdParam = new SqlParameter("@JobSkillID", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        Command.Parameters.Add(outputIdParam);


                        Command.ExecuteNonQuery();

                        JobSkillID = (int)outputIdParam.Value;

                    }
                }

            }
            catch (Exception Ex)
            {
                clsEventLogData.WriteEvent($" Message : {Ex.Message} \n\n Source : {Ex.Source} \n\n Target Site :  {Ex.TargetSite} \n\n Stack Trace :  {Ex.StackTrace}", EventLogEntryType.Error);
                JobSkillID = -1;
            }
            return JobSkillID;
        }

        public static bool UpdateJobSkill(JobSkillDTO jobskillDTO)
        {
            int RowsEffected = 0;
            try
            {
                using (SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    Connection.Open();
                    using (SqlCommand Command = new SqlCommand("SP_UpdateJobSkill", Connection))
                    {
                        Command.CommandType = CommandType.StoredProcedure;
                        Command.Parameters.AddWithValue("@JobSkillID", jobskillDTO.JobSkillID);
                        Command.Parameters.AddWithValue("@JobID", jobskillDTO.JobID);
                        Command.Parameters.AddWithValue("@SkillID", jobskillDTO.SkillID);


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

        public static bool DeleteJobSkill(int JobSkillID)
        {
            int RowsEffected = 0;
            try
            {
                using (SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    Connection.Open();
                    using (SqlCommand Command = new SqlCommand("SP_DeleteJobSkill", Connection))
                    {
                        Command.CommandType = CommandType.StoredProcedure;
                        Command.Parameters.AddWithValue("@JobSkillID", JobSkillID);


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

        public static List<JobSkillDTO> GetAllJobSkills()
        {
            List<JobSkillDTO> ListJobSkills = new List<JobSkillDTO>();
            try
            {
                using (SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    Connection.Open();
                    using (SqlCommand Command = new SqlCommand("SP_GetAllJobSkills", Connection))
                    {
                        Command.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader Reader = Command.ExecuteReader())
                        {
                            while (Reader.Read())
                            {
                                ListJobSkills.Add
                                   (
                                      new JobSkillDTO
                                      (
                                         Reader.GetInt32(Reader.GetOrdinal("JobSkillID")),
Reader.GetInt32(Reader.GetOrdinal("JobID")),
Reader.GetInt32(Reader.GetOrdinal("SkillID"))
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

            return ListJobSkills;
        }

        public static bool IsJobSkillExistByID(int JobSkillID)
        {
            bool IsFound = false;
            try
            {
                using (SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    Connection.Open();
                    using (SqlCommand Command = new SqlCommand("SP_IsJobSkillExistByID", Connection))
                    {
                        Command.CommandType = CommandType.StoredProcedure;
                        Command.Parameters.AddWithValue("@JobSkillID", JobSkillID);
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

        public static JobSkillDTO GetJobSkillInfoByID(int JobSkillID)
        {
            JobSkillDTO jobskillDTO = null;
            try
            {
                using (SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    Connection.Open();
                    using (SqlCommand Command = new SqlCommand("SP_GetJobSkillInfoByID", Connection))
                    {
                        Command.CommandType = CommandType.StoredProcedure;
                        Command.Parameters.AddWithValue("@JobSkillID", JobSkillID);
                        using (SqlDataReader Reader = Command.ExecuteReader())
                        {
                            if (Reader.Read())
                            {

                                jobskillDTO = new JobSkillDTO(Reader.GetInt32(Reader.GetOrdinal("JobSkillID")),
Reader.GetInt32(Reader.GetOrdinal("JobID")),
Reader.GetInt32(Reader.GetOrdinal("SkillID"))
                                 );

                            }
                            else
                                jobskillDTO = null;
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                clsEventLogData.WriteEvent($" Message : {Ex.Message} \n\n Source : {Ex.Source} \n\n Target Site :  {Ex.TargetSite} \n\n Stack Trace :  {Ex.StackTrace}", EventLogEntryType.Error);
                jobskillDTO = null;
            }
            return jobskillDTO;

        }


        public static bool IsJobHaveThisSkill(int JobID, int SkillID)
        {
            bool IsFound = false;
            try
            {
                using (SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    Connection.Open();
                    using (SqlCommand Command = new SqlCommand("SP_IsJobHaveThisSkill", Connection))
                    {
                        Command.CommandType = CommandType.StoredProcedure;
                        Command.Parameters.AddWithValue("@JobID", JobID);
                        Command.Parameters.AddWithValue("@SkillID", SkillID);
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

        public static List<SkillDTO> GetAllSkillsForJob(int JobID)
        {
            List<SkillDTO> ListJobSkills = new List<SkillDTO>();
            try
            {
                using (SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    Connection.Open();
                    using (SqlCommand Command = new SqlCommand("SP_GetAllSkillsForJob", Connection))
                    {
                        Command.CommandType = CommandType.StoredProcedure;
                        Command.Parameters.AddWithValue("@JobID", JobID);
                        using (SqlDataReader Reader = Command.ExecuteReader())
                        {
                            while (Reader.Read())
                            {
                                ListJobSkills.Add
                                   (
                                      new SkillDTO(
                                         Reader.GetInt32(Reader.GetOrdinal("SkillID")),
Reader.GetString(Reader.GetOrdinal("Name")),
Reader.IsDBNull(Reader.GetOrdinal("IconUrl")) ? null : Reader.GetString(Reader.GetOrdinal("IconUrl"))
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

            return ListJobSkills;
        }

        public static bool DeleteAllSkillsForJob(int JobID)
        {
            int RowsEffected = -1;
            try
            {
                using (SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    Connection.Open();
                    using (SqlCommand Command = new SqlCommand("SP_DeleteAllSkillsForJob", Connection))
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
                RowsEffected = -1;
            }

            return RowsEffected >= 0;
        }


        public static bool DeleteSkillForJob(int JobID, int SkillID)
        {
            int RowsEffected = -1;
            try
            {
                using (SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    Connection.Open();
                    using (SqlCommand Command = new SqlCommand("SP_DeleteSkillForJob", Connection))
                    {
                        Command.CommandType = CommandType.StoredProcedure;
                        Command.Parameters.AddWithValue("@JobID", JobID);
                        Command.Parameters.AddWithValue("@SkillID", SkillID);


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
                RowsEffected = -1;
            }

            return RowsEffected >= 0;
        }

    }
}
