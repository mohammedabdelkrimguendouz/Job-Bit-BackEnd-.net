import { useState, useEffect, useMemo } from "react";
import {
  Menu,
  MenuItem,
  IconButton,
  ListItemIcon,
  ListItemText,
  Grid,
  Card,
  TextField,
  Tabs,
  Tab,
} from "@mui/material";
import { Modal, Box } from "@mui/material";
import CompanyDetails from "../Companies/components/CompanyDetails ";
import MoreVertIcon from "@mui/icons-material/MoreVert";
import PowerSettingsNewIcon from "@mui/icons-material/PowerSettingsNew";
import DeleteIcon from "@mui/icons-material/Delete";
import VisibilityIcon from "@mui/icons-material/Visibility";

import { motion } from "framer-motion";
import MDBox from "components/MDBox";
import MDBadge from "components/MDBadge";
import MDTypography from "components/MDTypography";
import DashboardLayout from "examples/LayoutContainers/DashboardLayout";
import DataTable from "examples/Tables/DataTable";
import Swal from "sweetalert2";

import {
  getAllCompanies,
  deleteCompany,
  updateCompanyActivityStatus,
  getCompanyByID,
} from "../../services/companyService";

function Companies() {
  const [companies, setCompanies] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [anchorEl, setAnchorEl] = useState({});
  const [searchTerm, setSearchTerm] = useState("");
  const [statusFilter, setStatusFilter] = useState("all");
  const [selectedCompany, setSelectedCompany] = useState(null);
  const [openModal, setOpenModal] = useState(false);

  const fetchCompanies = async () => {
    try {
      const data = await getAllCompanies();
      setCompanies(data);
    } catch (error) {
      setError(error.message);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchCompanies();
  }, []);

  const toggleActiveStatus = async (id, currentStatus) => {
    try {
      await updateCompanyActivityStatus(id, !currentStatus);
      setLoading(true);
      await fetchCompanies();
    } catch (error) {}
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
        await deleteCompany(id);
        setLoading(true);
        await fetchCompanies();

        Swal.fire({
          title: "Deleted!",
          text: "Company has been deleted.",
          icon: "success",
          timer: 1500,
        });
      } catch (error) {
        Swal.fire({
          title: "Error!",
          text: "Failed to delete the company.",
          icon: "error",
        });
      }
    }
  };

  const handleOpen = (event, id) => setAnchorEl((prev) => ({ ...prev, [id]: event.currentTarget }));
  const handleClose = (id) => setAnchorEl((prev) => ({ ...prev, [id]: null }));

  const showCompanyDetails = async (id) => {
    try {
      const data = await getCompanyByID(id);
      setSelectedCompany(data);
      setOpenModal(true);
    } catch (error) {
      console.error("Failed to load company details", error);
    }
  };

  const handleCloseModal = () => {
    setOpenModal(false);
    setSelectedCompany(null);
  };

  const columns = [
    { Header: "Name", accessor: "name", width: "30%", align: "left" },
    { Header: "Email", accessor: "email", align: "left" },
    { Header: "Wilaya", accessor: "wilaya", align: "center" },
    { Header: "Active", accessor: "active", align: "center" },
    { Header: "Action", accessor: "action", align: "center" },
  ];

  const rows =
    companies?.map((company) => ({
      name: company.companyName,
      email: company.email,
      wilaya: company.wilayaName,
      isActiveBoolean: Boolean(company.isActive),
      active: (
        <MDBox ml={-1}>
          <MDBadge
            badgeContent={company.isActive ? "YES" : "NO"}
            color={company.isActive ? "success" : "error"}
            variant="gradient"
            size="sm"
          />
        </MDBox>
      ),
      action: (
        <div>
          <IconButton onClick={(event) => handleOpen(event, company.companyID)}>
            <MoreVertIcon />
          </IconButton>
          <Menu
            anchorEl={anchorEl[company.companyID]}
            open={Boolean(anchorEl[company.companyID])}
            onClose={() => handleClose(company.companyID)}
          >
            <MenuItem
              onClick={() => {
                toggleActiveStatus(company.companyID, company.isActive);
                handleClose(company.companyID);
              }}
            >
              <ListItemIcon>
                <PowerSettingsNewIcon fontSize="small" />
              </ListItemIcon>
              <ListItemText primary={company.isActive ? "Deactivate" : "Activate"} />
            </MenuItem>

            <MenuItem
              onClick={() => {
                handleDelete(company.companyID);
                handleClose(company.companyID);
              }}
            >
              <ListItemIcon>
                <DeleteIcon fontSize="small" />
              </ListItemIcon>
              <ListItemText primary="Delete" />
            </MenuItem>

            <MenuItem
              onClick={() => {
                showCompanyDetails(company.companyID);
                handleClose(company.companyID);
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
    })) ?? [];

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

  if (loading)
    return (
      <DashboardLayout>
        <MDBox pt={6} pb={3}>
          Loading...
        </MDBox>
      </DashboardLayout>
    );

  if (error)
    return (
      <DashboardLayout>
        <MDBox pt={6} pb={3}>
          Error: {error}
        </MDBox>
      </DashboardLayout>
    );

  return (
    <DashboardLayout>
      <MDBox pt={6} pb={3}>
        {/* Filters Section */}
        <Grid container spacing={3} justifyContent="center">
          {/* Search Filter */}
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

          {/* Status Filter using Tabs */}
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

        {/* Table Section */}
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
                    Companies ({filteredRows.length})
                  </MDTypography>
                </MDBox>
                <MDBox pt={3}>
                  <DataTable
                    table={{ columns, rows: filteredRows }}
                    isSorted={false}
                    entriesPerPage={false}
                    showTotalEntries={false}
                    noEndBorder
                  />
                </MDBox>
              </Card>
            </motion.div>
          </Grid>
        </Grid>
      </MDBox>
      <Modal open={openModal} onClose={handleCloseModal}>
        <Box
          sx={{
            width: "90%",
            maxWidth: 600,
            bgcolor: "background.paper",
            p: 4,
            m: "auto",
            mt: "5%",
            borderRadius: 2,
            boxShadow: 24,
            outline: "none",
            maxHeight: "90vh",
            overflowY: "auto",
          }}
        >
          {selectedCompany && (
            <CompanyDetails company={selectedCompany} onClose={handleCloseModal} />
          )}
        </Box>
      </Modal>
    </DashboardLayout>
  );
}

export default Companies;
