using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobBit_DataAccess;
namespace JobBit_Business
{
    public class Statistics
    {
        public static StatisticDTO GetStatistics()
        {
            return StatisticsData.GetStatistics();
        }
    }
}
