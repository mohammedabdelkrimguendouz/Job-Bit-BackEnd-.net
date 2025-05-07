using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobBit_DataAccess;

namespace JobBit_Business
{
    public class Request
    {

        public class AllRequestInfo
        {
            public int RequestID { get; set; }
            public JobSeeker.AllJobSeekerInfo JobSeekerInfo { get; set; }
            public Job.AllJobInfoDTO JobInfo { get; set; }
            public DateTime Date { get; set; }
            public bool? Status { get; set; }

            public AllRequestInfo(int requestID, JobSeeker.AllJobSeekerInfo jobSeekerInfo,
                Job.AllJobInfoDTO jobInfo, DateTime date, bool? status)
            {
                RequestID = requestID;
                JobSeekerInfo = jobSeekerInfo;
                JobInfo = jobInfo;
                Date = date;
                Status = status;
            }
        }

        public enum enMode { AddNew = 0, Update = 1 };
        private enMode Mode = enMode.AddNew;
        public RequestDTO requestDTO
        {
            get => new RequestDTO(this.RequestID, this.JobSeekerID, this.JobID, this.Date, this.Status);
        }

        public AllRequestInfo allRequestInfo
        {
            get => new AllRequestInfo(
                this.RequestID,
                this.JobSeekerInfo.alljobseekerInfo,
                this.JobInfo.allJobInfoDTO,
                this.Date,
                this.Status
                );
        }

        public int RequestID { get; set; }
        public int JobSeekerID { get; set; }
        public JobSeeker JobSeekerInfo;
        public int JobID { get; set; }
        public Job JobInfo;
        public DateTime Date { get; set; }
        public bool? Status { get; set; }

        public Request(RequestDTO requestDTO, enMode CreationMode = enMode.AddNew)
        {
            this.RequestID = requestDTO.RequestID;
            this.JobSeekerID = requestDTO.JobSeekerID;
            this.JobSeekerInfo = JobSeeker.FindByJobSeeker(this.JobSeekerID);
            this.JobID = requestDTO.JobID;
            this.JobInfo = Job.Find(this.JobID);
            this.Date = requestDTO.Date;
            this.Status = requestDTO.Status
            ;
            Mode = CreationMode;
        }

        public static Request Find(int RequestID)
        {

            RequestDTO requestDTO = RequestData.GetRequestInfoByID(RequestID);

            if (requestDTO != null)
            {
                return new Request(requestDTO, enMode.Update);
            }
            return null;

        }

        private bool _AddNewRequest()
        {
            this.RequestID = RequestData.AddNewRequest(this.requestDTO);
            return (this.RequestID != -1);
        }

        private bool _UpdateRequest()
        {
            return RequestData.UpdateRequest(this.requestDTO);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewRequest())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;
                case enMode.Update:
                    return _UpdateRequest();
            }
            return false;
        }

        public static bool DeleteRequest(int RequestID)
        {
            return RequestData.DeleteRequest(RequestID);
        }

        public static List<RequestDTO> GetAllRequests()
        {
            return RequestData.GetAllRequests();
        }
        public static List<ListApplicantForCompanyJobCaterizedByStatus> GetAllApplicantsForCompanyJob(int CompanyID)
        {
            return RequestData.GetAllApplicantsForCompanyJob(CompanyID);
        }

        public static bool IsRequestExist(int RequestID)
        {
            return RequestData.IsRequestExistByID(RequestID);
        }
        public static bool IsJobSeekerApplyedForJob(int JobSeekerID, int JobID)
        {
            return RequestData.IsJobSeekerApplyedForJob(JobSeekerID, JobID);
        }
        public static List<JobSeekerApplications> GetAllJobSeekerApplications(int JobSeekerID)
        {
            return RequestData.GetAllJobSeekerApplications(JobSeekerID);
        }

    }
}
