using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;
using Microsoft.Data.SqlClient;

namespace JobBit_DataAccess
{
    
    public class RequestDTO
    {
        public int RequestID { get; set; }
        public int JobSeekerID { get; set; }
        public int JobID { get; set; }
        public DateTime Date { get; set; }
        public bool? Status { get; set; }

        public RequestDTO(int RequestID, int JobSeekerID, int JobID, DateTime Date, bool? Status)
        {
            this.RequestID = RequestID;
            this.JobSeekerID = JobSeekerID;
            this.JobID = JobID;
            this.Date = Date;
            this.Status = Status;
        }

    }

    public class ApplicantForCompanyJobDTO
    {
        public int RequestID { get; set; }
        public DateTime PostedDate{ get; set; }
        public string JobTitle { get; set; }

        public ApplicantForCompanyJobDTO(int requestID, DateTime postedDate, string jobTitle)
        {
            RequestID = requestID;
            PostedDate = postedDate;
            JobTitle = jobTitle;
           
        }
    }

    public class ListApplicantForCompanyJobCaterizedByStatus
    {
        public bool? Status { get; set; }
        public string StatusAsText { get; set; }

        public List<ApplicantForCompanyJobDTO> applicantForCompanyJob { get; set; }

        public ListApplicantForCompanyJobCaterizedByStatus(bool? status, string statusAsText,
            List<ApplicantForCompanyJobDTO> applicantForCompanyJobDTOs)
        {
            Status = status;
            StatusAsText = statusAsText;
            this.applicantForCompanyJob = applicantForCompanyJobDTOs;
        }
    }

    public class JobSeekerApplications
    {
        public int JobSeekerID { get; set; }
        public int JobID { get; set; }
        public string JobOffer {  get; set; }
        public DateTime AppliedOn { get; set; }

        public string Company { get; set; }
        public bool? Status { get; set; }

        public JobSeekerApplications(int jobSeekerID, int jobID, string jobOffer,
            DateTime appliedOn, string company, bool? status)
        {
            JobSeekerID = jobSeekerID;
            JobID = jobID;
            JobOffer = jobOffer;
            AppliedOn = appliedOn;
            Company = company;
            Status = status;
        }
    }

    public class RequestData
    {
        public static List<ApplicantForCompanyJobDTO> GetAllJobsForCompanyByStatus(int CompanyID,bool? Status)
        {
            List<ApplicantForCompanyJobDTO> applicantForCompanyJobDTOs = new List<ApplicantForCompanyJobDTO>();
            try
            {
                using (SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    Connection.Open();
                    using (SqlCommand Command = new SqlCommand("SP_GetAllJobsForCompanyByStatus", Connection))
                    {
                        Command.CommandType = CommandType.StoredProcedure;
                        Command.Parameters.AddWithValue("@CompanyID", CompanyID);
                        Command.Parameters.AddWithValue("@Status", (Status==null)?DBNull.Value:Status);
                        using (SqlDataReader Reader = Command.ExecuteReader())
                        {
                            while (Reader.Read())
                            {
                                applicantForCompanyJobDTOs.Add(
                                    new ApplicantForCompanyJobDTO(
                                        Reader.GetInt32(Reader.GetOrdinal("RequestID")),
                                        Reader.GetDateTime(Reader.GetOrdinal("PostedDate")),
Reader.GetString(Reader.GetOrdinal("Title"))
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
            return applicantForCompanyJobDTOs;

        }
       

        public static List<ListApplicantForCompanyJobCaterizedByStatus> GetAllApplicantsForCompanyJob(int CompanyID)
        {
            List<ListApplicantForCompanyJobCaterizedByStatus> applicantForCompanyJobCaterizedByStatus =
                new List<ListApplicantForCompanyJobCaterizedByStatus>();


            applicantForCompanyJobCaterizedByStatus.Add(new ListApplicantForCompanyJobCaterizedByStatus(
                null, "Pending Job Offers", GetAllJobsForCompanyByStatus(CompanyID, null)));

            applicantForCompanyJobCaterizedByStatus.Add(new ListApplicantForCompanyJobCaterizedByStatus(
                true, "Accepted Job Offers", GetAllJobsForCompanyByStatus(CompanyID, true)));

            applicantForCompanyJobCaterizedByStatus.Add(new ListApplicantForCompanyJobCaterizedByStatus(
                false, "Rejected Job Offers", GetAllJobsForCompanyByStatus(CompanyID, false)));

            return applicantForCompanyJobCaterizedByStatus;

        }



        public static List<JobSeekerApplications> GetAllJobSeekerApplications(int JobSeekerID)
        {
            List<JobSeekerApplications> jobSeekerApplications = new List<JobSeekerApplications>();
            try
            {
                using (SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    Connection.Open();
                    using (SqlCommand Command = new SqlCommand("SP_GetAllJobSeekerApplications", Connection))
                    {
                        Command.CommandType = CommandType.StoredProcedure;
                        Command.Parameters.AddWithValue("@JobSeekerID", JobSeekerID);
                        using (SqlDataReader Reader = Command.ExecuteReader())
                        {
                            while (Reader.Read())
                            {
                                jobSeekerApplications.Add(
                                    new JobSeekerApplications(
                                        Reader.GetInt32(Reader.GetOrdinal("JobSeekerID")),
Reader.GetInt32(Reader.GetOrdinal("JobID")),
Reader.GetString(Reader.GetOrdinal("JobOffer")),
Reader.GetDateTime(Reader.GetOrdinal("AppliedOn")),
Reader.GetString(Reader.GetOrdinal("Company")),
Reader.IsDBNull(Reader.GetOrdinal("Status")) ? null : Reader.GetBoolean(Reader.GetOrdinal("Status"))
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
            return jobSeekerApplications;

        }

        public static int AddNewRequest(RequestDTO requestDTO)
        {
            int RequestID = -1;
            try
            {
                using (SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    Connection.Open();
                    using (SqlCommand Command = new SqlCommand("SP_AddNewRequest", Connection))
                    {
                        Command.CommandType = CommandType.StoredProcedure;
                        Command.Parameters.AddWithValue("@JobSeekerID", requestDTO.JobSeekerID);
                        Command.Parameters.AddWithValue("@JobID", requestDTO.JobID);
                        Command.Parameters.AddWithValue("@Date", DateTime.Now);
                        Command.Parameters.AddWithValue("@Status", DBNull.Value);


                        SqlParameter outputIdParam = new SqlParameter("@RequestID", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        Command.Parameters.Add(outputIdParam);


                        Command.ExecuteNonQuery();

                        RequestID = (int)outputIdParam.Value;

                    }
                }

            }
            catch (Exception Ex)
            {
                clsEventLogData.WriteEvent($" Message : {Ex.Message} \n\n Source : {Ex.Source} \n\n Target Site :  {Ex.TargetSite} \n\n Stack Trace :  {Ex.StackTrace}", EventLogEntryType.Error);
                RequestID = -1;
            }
            return RequestID;
        }

        public static bool UpdateRequest(RequestDTO requestDTO)
        {
            int RowsEffected = 0;
            try
            {
                using (SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    Connection.Open();
                    using (SqlCommand Command = new SqlCommand("SP_UpdateRequest", Connection))
                    {
                        Command.CommandType = CommandType.StoredProcedure;
                        Command.Parameters.AddWithValue("@RequestID", requestDTO.RequestID);
                        Command.Parameters.AddWithValue("@JobSeekerID", requestDTO.JobSeekerID);
                        Command.Parameters.AddWithValue("@JobID", requestDTO.JobID);

                        if(requestDTO.Status==null)
                            Command.Parameters.AddWithValue("@Status", DBNull.Value);
                        else
                            Command.Parameters.AddWithValue("@Status", requestDTO.Status);


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

        public static bool DeleteRequest(int RequestID)
        {
            int RowsEffected = 0;
            try
            {
                using (SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    Connection.Open();
                    using (SqlCommand Command = new SqlCommand("SP_DeleteRequest", Connection))
                    {
                        Command.CommandType = CommandType.StoredProcedure;
                        Command.Parameters.AddWithValue("@RequestID", RequestID);


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

        public static List<RequestDTO> GetAllRequests()
        {
            List<RequestDTO> ListRequests = new List<RequestDTO>();
            try
            {
                using (SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    Connection.Open();
                    using (SqlCommand Command = new SqlCommand("SP_GetAllRequests", Connection))
                    {
                        Command.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader Reader = Command.ExecuteReader())
                        {
                            while (Reader.Read())
                            {
                                ListRequests.Add
                                   (
                                      new RequestDTO
                                      (
                                         Reader.GetInt32(Reader.GetOrdinal("RequestID")),
Reader.GetInt32(Reader.GetOrdinal("JobSeekerID")),
Reader.GetInt32(Reader.GetOrdinal("JobID")),
Reader.GetDateTime(Reader.GetOrdinal("Date")),
Reader.IsDBNull(Reader.GetOrdinal("Status")) ?  null  : Reader.GetBoolean(Reader.GetOrdinal("Status"))
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

            return ListRequests;
        }

        public static bool IsRequestExistByID(int RequestID)
        {
            bool IsFound = false;
            try
            {
                using (SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    Connection.Open();
                    using (SqlCommand Command = new SqlCommand("SP_IsRequestExistByID", Connection))
                    {
                        Command.CommandType = CommandType.StoredProcedure;
                        Command.Parameters.AddWithValue("@RequestID", RequestID);
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

        public static bool IsJobSeekerApplyedForJob(int JobSeekerID,int JobID)
        {
            bool IsFound = false;
            try
            {
                using (SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    Connection.Open();
                    using (SqlCommand Command = new SqlCommand("SP_IsJobSeekerApplyedForJob", Connection))
                    {
                        Command.CommandType = CommandType.StoredProcedure;
                        Command.Parameters.AddWithValue("@JobSeekerID", JobSeekerID);
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

        public static RequestDTO GetRequestInfoByID(int RequestID)
        {
            RequestDTO requestDTO = null;
            try
            {
                using (SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    Connection.Open();
                    using (SqlCommand Command = new SqlCommand("SP_GetRequestInfoByID", Connection))
                    {
                        Command.CommandType = CommandType.StoredProcedure;
                        Command.Parameters.AddWithValue("@RequestID", RequestID);
                        using (SqlDataReader Reader = Command.ExecuteReader())
                        {
                            if (Reader.Read())
                            {

                                requestDTO = new RequestDTO
                                      (
                                         Reader.GetInt32(Reader.GetOrdinal("RequestID")),
Reader.GetInt32(Reader.GetOrdinal("JobSeekerID")),
Reader.GetInt32(Reader.GetOrdinal("JobID")),
Reader.GetDateTime(Reader.GetOrdinal("Date")),
Reader.IsDBNull(Reader.GetOrdinal("Status")) ? null : Reader.GetBoolean(Reader.GetOrdinal("Status"))
                                      );

                            }
                            else
                                requestDTO = null;
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                clsEventLogData.WriteEvent($" Message : {Ex.Message} \n\n Source : {Ex.Source} \n\n Target Site :  {Ex.TargetSite} \n\n Stack Trace :  {Ex.StackTrace}", EventLogEntryType.Error);
                requestDTO = null;
            }
            return requestDTO;

        }



    }
}
