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
    public class StatisticDTO
    {
        public int JobSeekersCount { get; set; }
        public int CompaniesCount { get; set; }
        public int AvailableJobsCount { get; set; }

        public int UnavailableJobsCount
        { get; set; }

        public int AcceptedRequests {  get; set; }
        public int RejectedRequests
        { get; set; }

        public int PendingRequests {  get; set; }

        public StatisticDTO(int jobSeekersCount, int companiesCount, int availableJobsCount,
            int unavailableJobsCount, int acceptedRequests, int rejectedRequests, int pendingRequests)
        {
            JobSeekersCount = jobSeekersCount;
            CompaniesCount = companiesCount;
            AvailableJobsCount = availableJobsCount;
            UnavailableJobsCount = unavailableJobsCount;
            AcceptedRequests = acceptedRequests;
            RejectedRequests = rejectedRequests;
            PendingRequests = pendingRequests;
        }
    }
    public class StatisticsData
    {
        public static StatisticDTO GetStatistics()
        {
            StatisticDTO Statistic = null;
            try
            {
                using (SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    Connection.Open();
                    using (SqlCommand Command = new SqlCommand("SP_GetStatistics", Connection))
                    {
                        Command.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader Reader = Command.ExecuteReader())
                        {
                            if (Reader.Read())
                            {
                                Statistic = 
                                   (
                                      new StatisticDTO
                                      (
                                         Reader.GetInt32(Reader.GetOrdinal("JobSeekersCount")),
                                         Reader.GetInt32(Reader.GetOrdinal("CompaniesCount")),
                                         Reader.GetInt32(Reader.GetOrdinal("AvailableJobsCount")),
                                         Reader.GetInt32(Reader.GetOrdinal("UnavailableJobsCount")),
                                         Reader.GetInt32(Reader.GetOrdinal("AcceptedRequests")),
                                         Reader.GetInt32(Reader.GetOrdinal("RejectedRequests")),
                                         Reader.GetInt32(Reader.GetOrdinal("PendingRequests"))
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

            return Statistic;
        }
    }
}
