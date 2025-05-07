import Grid from "@mui/material/Grid";
import LinearProgress from "@mui/material/LinearProgress";
import Typography from "@mui/material/Typography";
import Box from "@mui/material/Box";
import MDBox from "components/MDBox";
import DashboardLayout from "examples/LayoutContainers/DashboardLayout";
import DefaultInfoCard from "examples/Cards/InfoCards/DefaultInfoCard";
import { motion } from "framer-motion";
import { useState, useEffect } from "react";
import { getStatistics } from "../../services/statisticsService";

function Dashboard() {
  const [stats, setStats] = useState({
    jobSeekersCount: 0,
    companiesCount: 0,
    availableJobsCount: 0,
    unavailableJobsCount: 0,
    acceptedRequests: 0,
    rejectedRequests: 0,
    pendingRequests: 0,
  });

  useEffect(() => {
    const fetchStatistics = async () => {
      try {
        const data = await getStatistics();
        setStats(data);
      } catch (error) {
        console.error("Error fetching statistics:", error);
      }
    };
    fetchStatistics();
  }, []);

  const totalRequests =
    stats.acceptedRequests + stats.rejectedRequests + stats.pendingRequests || 1;

  const requestData = [
    { title: "Accepted Requests", value: stats.acceptedRequests, color: "#4CAF50" },
    { title: "Rejected Requests", value: stats.rejectedRequests, color: "#F44336" },
    { title: "Pending Requests", value: stats.pendingRequests, color: "#2196F3" },
  ];

  return (
    <DashboardLayout>
      <MDBox py={3}>
        {/* عرض الإحصائيات بشكل أفقي مع أيقونات معبرة وألوان جديدة */}
        <Grid container spacing={3}>
          {[
            { title: "Job Seekers", value: stats.jobSeekersCount, icon: "groups", color: "info" },
            { title: "Companies", value: stats.companiesCount, icon: "business", color: "primary" },
            {
              title: "Available Jobs",
              value: stats.availableJobsCount,
              icon: "work",
              color: "success",
            },
            {
              title: "Unavailable Jobs",
              value: stats.unavailableJobsCount,
              icon: "work_off",
              color: "error",
            },
          ].map((item, index) => (
            <Grid item xs={12} sm={6} md={3} key={index}>
              <motion.div
                initial={{ opacity: 0, y: -20 }}
                animate={{ opacity: 1, y: 0 }}
                transition={{ duration: 0.8, delay: index * 0.2 }}
              >
                <DefaultInfoCard
                  color={item.color} // الألوان الجديدة
                  icon={item.icon}
                  title={item.title}
                  value={item.value}
                />
              </motion.div>
            </Grid>
          ))}
        </Grid>

        {/* عرض progress بشكل عمودي مع حركة بطيئة */}
        <MDBox mt={5}>
          <Grid container direction="column" spacing={3} alignItems="center">
            {requestData.map((item, index) => {
              const percentage = (item.value / totalRequests) * 100;
              return (
                <Grid item xs={12} key={index} style={{ width: "100%" }}>
                  <motion.div
                    initial={{ opacity: 0, y: 50 }}
                    animate={{ opacity: 1, y: 0 }}
                    transition={{ duration: 1, delay: 0.5 + index * 0.3 }}
                  >
                    <Typography variant="h6" align="center" gutterBottom>
                      {item.title}: {item.value}
                    </Typography>
                    <Box position="relative" display="flex" alignItems="center">
                      <LinearProgress
                        variant="determinate"
                        value={percentage}
                        sx={{
                          height: "40px",
                          width: "100%",
                          borderRadius: "10px",
                          backgroundColor: "#ddd",
                          "& .MuiLinearProgress-bar": {
                            backgroundColor: item.color,
                            borderRadius: "10px",
                          },
                        }}
                      />
                      <Typography
                        variant="body1"
                        component="div"
                        sx={{
                          position: "absolute",
                          left: "50%",
                          transform: "translateX(-50%)",
                          color: "#fff",
                          fontWeight: "bold",
                        }}
                      >
                        {Math.round(percentage)}%
                      </Typography>
                    </Box>
                  </motion.div>
                </Grid>
              );
            })}
          </Grid>
        </MDBox>
      </MDBox>
    </DashboardLayout>
  );
}

export default Dashboard;
