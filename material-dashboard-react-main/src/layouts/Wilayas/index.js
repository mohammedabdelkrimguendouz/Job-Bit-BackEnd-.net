import { useState, useEffect, useMemo } from "react";
import Grid from "@mui/material/Grid";
import Card from "@mui/material/Card";
import TextField from "@mui/material/TextField";
import Button from "@mui/material/Button";
import Dialog from "@mui/material/Dialog";
import DialogTitle from "@mui/material/DialogTitle";
import DialogContent from "@mui/material/DialogContent";
import DialogActions from "@mui/material/DialogActions";
import Tabs from "@mui/material/Tabs";
import Tab from "@mui/material/Tab";
import { motion } from "framer-motion";
import { Menu, MenuItem, IconButton, ListItemIcon, ListItemText } from "@mui/material";
import MoreVertIcon from "@mui/icons-material/MoreVert";
import DeleteIcon from "@mui/icons-material/Delete";
import VisibilityIcon from "@mui/icons-material/Visibility";
import Swal from "sweetalert2";

import { getAllWilayas, deleteWilaya, addWilaya, updateWilaya } from "services/wilayaService";

import MDBox from "components/MDBox";
import MDTypography from "components/MDTypography";
import DashboardLayout from "examples/LayoutContainers/DashboardLayout";
import DataTable from "examples/Tables/DataTable";

function Wilayas() {
  const [wilayas, setWilayas] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [anchorEl, setAnchorEl] = useState({});
  const [searchTerm, setSearchTerm] = useState("");

  const [openForm, setOpenForm] = useState(false);
  const [formData, setFormData] = useState({ name: "" });
  const [editId, setEditId] = useState(null);

  const fetchWilayas = async () => {
    try {
      const data = await getAllWilayas();
      setWilayas(data);
    } catch (error) {
      setError(error.message);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchWilayas();
  }, []);

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
        await deleteWilaya(id);
        setLoading(true);
        await fetchWilayas();
        Swal.fire("Deleted!", "Wilaya has been deleted.", "success");
      } catch (error) {
        Swal.fire("Error!", "Failed to delete the wilaya.", "error");
      }
    }
  };

  const handleOpen = (event, id) => {
    setAnchorEl((prev) => ({ ...prev, [id]: event.currentTarget }));
  };

  const handleClose = (id) => {
    setAnchorEl((prev) => ({ ...prev, [id]: null }));
  };

  const handleOpenForm = (wilaya = null) => {
    if (wilaya) {
      setFormData({ name: wilaya.name });
      setEditId(wilaya.wilayaID);
    } else {
      setFormData({ name: "" });
      setEditId(null);
    }
    setOpenForm(true);
  };

  const handleCloseForm = () => {
    setOpenForm(false);
    setFormData({ name: "" });
    setEditId(null);
  };

  const handleSave = async () => {
    try {
      if (editId) {
        await updateWilaya(editId, formData.name);
        Swal.fire("Updated!", "Wilaya has been updated.", "success");
      } else {
        await addWilaya(formData.name);
        Swal.fire("Added!", "Wilaya has been added.", "success");
      }
      handleCloseForm();
      setLoading(true);
      await fetchWilayas();
    } catch (error) {
      Swal.fire("Error", "Something went wrong!", "error");
    }
  };

  const columns = [
    { Header: "ID", accessor: "id", align: "left" },
    { Header: "Name", accessor: "name", align: "left" },
    { Header: "Action", accessor: "action", align: "center" },
  ];

  const rows = wilayas.map((wilaya) => ({
    id: wilaya.wilayaID,
    name: wilaya.name,
    action: (
      <div>
        <IconButton onClick={(event) => handleOpen(event, wilaya.wilayaID)}>
          <MoreVertIcon />
        </IconButton>
        <Menu
          anchorEl={anchorEl[wilaya.wilayaID]}
          open={Boolean(anchorEl[wilaya.wilayaID])}
          onClose={() => handleClose(wilaya.wilayaID)}
        >
          <MenuItem
            onClick={() => {
              handleDelete(wilaya.wilayaID);
              handleClose(wilaya.wilayaID);
            }}
          >
            <ListItemIcon>
              <DeleteIcon fontSize="small" />
            </ListItemIcon>
            <ListItemText primary="Delete" />
          </MenuItem>
          <MenuItem
            onClick={() => {
              handleOpenForm(wilaya);
              handleClose(wilaya.wilayaID);
            }}
          >
            <ListItemIcon>
              <VisibilityIcon fontSize="small" />
            </ListItemIcon>
            <ListItemText primary="Edit" />
          </MenuItem>
        </Menu>
      </div>
    ),
  }));

  const filteredRows = useMemo(() => {
    return rows.filter((row) => row.name.toLowerCase().includes(searchTerm.toLowerCase()));
  }, [searchTerm, rows]);

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
          <Grid item xs={12} md={6} display="flex" justifyContent="flex-end">
            <Button variant="contained" color="success" onClick={() => handleOpenForm()}>
              + Add Wilaya
            </Button>
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
                    Wilayas ({filteredRows.length})
                  </MDTypography>
                </MDBox>
                <MDBox pt={3}>
                  {loading ? (
                    <MDTypography p={2}>Loading...</MDTypography>
                  ) : error ? (
                    <MDTypography p={2} color="error">
                      {error}
                    </MDTypography>
                  ) : (
                    <DataTable
                      table={{ columns, rows: filteredRows }}
                      isSorted={false}
                      entriesPerPage={false}
                      showTotalEntries={false}
                      noEndBorder
                    />
                  )}
                </MDBox>
              </Card>
            </motion.div>
          </Grid>
        </Grid>

        <Dialog open={openForm} onClose={handleCloseForm}>
          <DialogTitle>{editId ? "Edit Wilaya" : "Add Wilaya"}</DialogTitle>
          <DialogContent>
            <TextField
              autoFocus
              margin="dense"
              label="Wilaya Name"
              type="text"
              fullWidth
              value={formData.name}
              onChange={(e) => setFormData({ ...formData, name: e.target.value })}
            />
          </DialogContent>
          <DialogActions>
            <Button
              onClick={handleCloseForm}
              sx={{
                color: "#36305E",
                "&:hover": {
                  color: "#36305E",
                  backgroundColor: "transparent",
                },
              }}
            >
              Cancel
            </Button>
            <Button
              onClick={handleSave}
              variant="contained"
              sx={{
                backgroundColor: "#36305E",
                "&:hover": {
                  backgroundColor: "#2a254d",
                },
              }}
            >
              Save
            </Button>
          </DialogActions>
        </Dialog>
      </MDBox>
    </DashboardLayout>
  );
}

export default Wilayas;
