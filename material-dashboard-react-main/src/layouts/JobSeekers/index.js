import { useState, useEffect, useMemo } from "react";
import {
  Grid,
  Card,
  TextField,
  Tabs,
  Tab,
  Menu,
  MenuItem,
  IconButton,
  ListItemIcon,
  ListItemText,
  Dialog,
} from "@mui/material";
import { motion } from "framer-motion";
import MoreVertIcon from "@mui/icons-material/MoreVert";
import JobSeekerDetails from "../JobSeekers/components/JobSeekerDetails";
import PowerSettingsNewIcon from "@mui/icons-material/PowerSettingsNew";
import DeleteIcon from "@mui/icons-material/Delete";
import VisibilityIcon from "@mui/icons-material/Visibility";
import Swal from "sweetalert2";

import MDBox from "components/MDBox";
import MDTypography from "components/MDTypography";
import DashboardLayout from "examples/LayoutContainers/DashboardLayout";
import DataTable from "examples/Tables/DataTable";
import MDBadge from "components/MDBadge";

import {
  getAllJobSeekers,
  deleteJobSeeker,
  updateJobSeekerActivityStatus,
  getJobSeekerById,
} from "../../services/JobSeekerService";

function JobSeekers() {
  const [jobSeekers, setJobSeekers] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [anchorEl, setAnchorEl] = useState({});
  const [searchTerm, setSearchTerm] = useState("");
  const [statusFilter, setStatusFilter] = useState("all");
  const [selectedJobSeeker, setSelectedJobSeeker] = useState(null);
  const [isDetailsModalOpen, setDetailsModalOpen] = useState(false);

  const handleShowDetails = async (jobSeekerID) => {
    try {
      const data = await getJobSeekerById(jobSeekerID);
      setSelectedJobSeeker(data);
      setDetailsModalOpen(true);
    } catch (error) {
      console.error("Failed to load job seeker details", error);
      Swal.fire({
        title: "Error!",
        text: "Failed to load job seeker details",
        icon: "error",
      });
    }
  };

  const fetchJobSeekers = async () => {
    try {
      const data = await getAllJobSeekers();
      setJobSeekers(data);
    } catch (error) {
      setError(error.message);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchJobSeekers();
  }, []);

  const toggleActiveStatus = async (id, currentStatus) => {
    try {
      await updateJobSeekerActivityStatus(id, !currentStatus);
      setLoading(true);
      await fetchJobSeekers();
    } catch (error) {
      console.error("Error updating status:", error);
    }
  };

  const handleDelete = async (id) => {
    const result = await Swal.fire({
      title: "Are you sure?",
      text: "You won't be able to revert this!",
      icon: "warning",
      showCancelButton: true,
      confirmButtonColor: "#d33",
      cancelButtonColor: "#3085d6",
      confirmButtonText: "Yes, delete it!",
    });

    if (result.isConfirmed) {
      try {
        await deleteJobSeeker(id);
        setLoading(true);
        await fetchJobSeekers();

        Swal.fire({
          title: "Deleted!",
          text: "Job Seeker has been deleted.",
          icon: "success",
          timer: 1500,
        });
      } catch (error) {
        Swal.fire({
          title: "Error!",
          text: "Failed to delete the Job Seeker.",
          icon: "error",
        });
      }
    }
  };

  const handleOpen = (event, id) => setAnchorEl((prev) => ({ ...prev, [id]: event.currentTarget }));
  const handleClose = (id) => setAnchorEl((prev) => ({ ...prev, [id]: null }));

  const columns = [
    { Header: "Name", accessor: "name", width: "30%", align: "left" },
    { Header: "Email", accessor: "email", align: "left" },
    { Header: "Active", accessor: "active", align: "center" },
    { Header: "Action", accessor: "action", align: "center" },
  ];

  const rows = jobSeekers.map((jobSeeker) => ({
    name: `${jobSeeker.firstName} ${jobSeeker.lastName}`,
    email: jobSeeker.email,
    isActiveBoolean: Boolean(jobSeeker.isActive),
    active: (
      <MDBox ml={-1}>
        <MDBadge
          badgeContent={jobSeeker.isActive ? "YES" : "NO"}
          color={jobSeeker.isActive ? "success" : "error"}
          variant="gradient"
          size="sm"
        />
      </MDBox>
    ),
    action: (
      <div>
        <IconButton onClick={(event) => handleOpen(event, jobSeeker.jobSeekerID)}>
          <MoreVertIcon />
        </IconButton>
        <Menu
          anchorEl={anchorEl[jobSeeker.jobSeekerID]}
          open={Boolean(anchorEl[jobSeeker.jobSeekerID])}
          onClose={() => handleClose(jobSeeker.jobSeekerID)}
        >
          <MenuItem
            onClick={() => {
              toggleActiveStatus(jobSeeker.jobSeekerID, jobSeeker.isActive);
              handleClose(jobSeeker.jobSeekerID);
            }}
          >
            <ListItemIcon>
              <PowerSettingsNewIcon fontSize="small" />
            </ListItemIcon>
            <ListItemText primary={jobSeeker.isActive ? "Deactivate" : "Activate"} />
          </MenuItem>

          <MenuItem
            onClick={() => {
              handleDelete(jobSeeker.jobSeekerID);
              handleClose(jobSeeker.jobSeekerID);
            }}
          >
            <ListItemIcon>
              <DeleteIcon fontSize="small" />
            </ListItemIcon>
            <ListItemText primary="Delete" />
          </MenuItem>

          <MenuItem
            onClick={() => {
              handleShowDetails(jobSeeker.jobSeekerID);
              handleClose(jobSeeker.jobSeekerID);
            }}
          >
            <ListItemIcon>
              <VisibilityIcon fontSize="small" />
            </ListItemIcon>
            <ListItemText primary="Details" />
          </MenuItem>
        </Menu>
      </div>
    ),
  }));

  const filteredRows = useMemo(() => {
    return rows.filter((row) => {
      const matchesName = row.name.toLowerCase().includes(searchTerm.toLowerCase());
      const matchesStatus =
        statusFilter === "all"
          ? true
          : statusFilter === "active"
          ? row.isActiveBoolean === true
          : row.isActiveBoolean === false;

      return matchesName && matchesStatus;
    });
  }, [searchTerm, statusFilter, rows]);

  const fadeInUp = {
    hidden: { opacity: 0, y: 20 },
    visible: { opacity: 1, y: 0, transition: { duration: 0.8 } },
  };

  return (
    <DashboardLayout>
      <MDBox pt={6} pb={3}>
        <Grid container spacing={3} justifyContent="center">
          <Grid item xs={12} md={6}>
            <motion.div initial="hidden" animate="visible" variants={fadeInUp}>
              <TextField
                fullWidth
                label="🔍 Search by Name"
                variant="outlined"
                value={searchTerm}
                onChange={(e) => setSearchTerm(e.target.value)}
                sx={{
                  height: "56px",
                  backgroundColor: "#f5f5f5",
                  borderRadius: "8px",
                  "& .MuiOutlinedInput-root": { borderRadius: "8px" },
                }}
              />
            </motion.div>
          </Grid>

          <Grid item xs={12} md={6}>
            <motion.div
              initial="hidden"
              animate="visible"
              variants={fadeInUp}
              transition={{ delay: 0.2 }}
            >
              <Tabs
                value={statusFilter}
                onChange={(event, newValue) => setStatusFilter(newValue)}
                centered
                textColor="primary"
                indicatorColor="primary"
                sx={{ backgroundColor: "#f5f5f5", borderRadius: "8px" }}
              >
                <Tab label="🔄 All" value="all" />
                <Tab label="✅ Active" value="active" />
                <Tab label="🚫 Inactive" value="inactive" />
              </Tabs>
            </motion.div>
          </Grid>
        </Grid>

        <Grid container spacing={3} mt={3}>
          <Grid item xs={12}>
            <motion.div
              initial="hidden"
              animate="visible"
              variants={fadeInUp}
              transition={{ delay: 0.4 }}
            >
              <Card>
                <MDBox
                  mx={2}
                  mt={-3}
                  py={3}
                  px={2}
                  variant="gradient"
                  sx={{
                    background: "linear-gradient(135deg, #36305E, #5A4E8C)",
                    borderRadius: "lg",
                    coloredShadow: "info",
                  }}
                >
                  <MDTypography variant="h6" color="white">
                    Job Seekers ({filteredRows.length})
                  </MDTypography>
                </MDBox>
                <MDBox pt={3}>
                  <DataTable
                    table={{ columns, rows: filteredRows }}
                    isSorted={false}
                    entriesPerPage={false}
                    showTotalEntries={false}
                  />
                </MDBox>
              </Card>
            </motion.div>
          </Grid>
        </Grid>
      </MDBox>

      <JobSeekerDetails
        isOpen={isDetailsModalOpen}
        jobSeeker={selectedJobSeeker}
        onClose={() => setDetailsModalOpen(false)}
      />
    </DashboardLayout>
  );
}

export default JobSeekers;
