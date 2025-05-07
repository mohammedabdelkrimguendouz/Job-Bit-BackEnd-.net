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
  Switch,
  FormControlLabel,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  Button,
} from "@mui/material";
import MoreVertIcon from "@mui/icons-material/MoreVert";
import DeleteIcon from "@mui/icons-material/Delete";
import VisibilityIcon from "@mui/icons-material/Visibility";
import Swal from "sweetalert2";
import { motion } from "framer-motion";

import MDBox from "components/MDBox";
import MDTypography from "components/MDTypography";
import DashboardLayout from "examples/LayoutContainers/DashboardLayout";
import DataTable from "examples/Tables/DataTable";

import { getAllSkills, deleteSkill, addSkill, updateSkill } from "../../services/skillService";

function Skills() {
  const [skills, setSkills] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [anchorEl, setAnchorEl] = useState({});
  const [searchTerm, setSearchTerm] = useState("");
  const [openModal, setOpenModal] = useState(false);
  const [editMode, setEditMode] = useState(false);
  const [editId, setEditId] = useState(null);
  const [formData, setFormData] = useState({
    skillName: "",
    iconUrl: "",
  });

  const fetchSkills = async () => {
    try {
      const data = await getAllSkills();
      setSkills(data);
    } catch (error) {
      setError(error.message);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchSkills();
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
        await deleteSkill(id);
        setLoading(true);
        await fetchSkills();
        Swal.fire("Deleted!", "Skill has been deleted.", "success");
      } catch (error) {
        Swal.fire("Error!", "Failed to delete the skill.", "error");
      }
    }
  };

  const handleOpen = (event, id) => setAnchorEl((prev) => ({ ...prev, [id]: event.currentTarget }));
  const handleClose = (id) => setAnchorEl((prev) => ({ ...prev, [id]: null }));

  const openAddModal = () => {
    setEditMode(false);
    setFormData({ skillName: "", iconUrl: "" });
    setOpenModal(true);
  };

  const openEditModal = (skill) => {
    setEditMode(true);
    setEditId(skill.skillID);
    setFormData({
      skillName: skill.name,
      iconUrl: skill.iconUrl,
    });
    setOpenModal(true);
  };

  const handleSave = async () => {
    try {
      if (editMode) {
        await updateSkill(editId, formData.skillName, formData.iconUrl);
      } else {
        await addSkill(formData.skillName, formData.iconUrl);
      }

      setOpenModal(false);
      setLoading(true);
      await fetchSkills();
    } catch (err) {}
  };

  const columns = [
    { Header: "ID", accessor: "id", align: "left" },
    { Header: "Icon", accessor: "icon", align: "center" },
    { Header: "Name", accessor: "name", align: "left" },
    { Header: "Action", accessor: "action", align: "center" },
  ];

  const rows = skills
    ? skills.map((skill) => ({
        id: skill.skillID,
        icon: (
          <img
            src={skill.iconUrl}
            alt={skill.name}
            style={{
              width: 40,
              height: 40,
              borderRadius: "50%",
              objectFit: "cover",
            }}
          />
        ),
        name: skill.name,
        action: (
          <div>
            <IconButton onClick={(event) => handleOpen(event, skill.skillID)}>
              <MoreVertIcon />
            </IconButton>
            <Menu
              anchorEl={anchorEl[skill.skillID]}
              open={Boolean(anchorEl[skill.skillID])}
              onClose={() => handleClose(skill.skillID)}
            >
              <MenuItem
                onClick={() => {
                  handleDelete(skill.skillID);
                  handleClose(skill.skillID);
                }}
              >
                <ListItemIcon>
                  <DeleteIcon fontSize="small" />
                </ListItemIcon>
                <ListItemText primary="Delete" />
              </MenuItem>
              <MenuItem
                onClick={() => {
                  openEditModal(skill);
                  handleClose(skill.skillID);
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
      }))
    : [];

  const filteredRows = useMemo(() => {
    const lowerSearchTerm = searchTerm.toLowerCase();
    return rows.filter((row) => row.name.toLowerCase().includes(lowerSearchTerm));
  }, [searchTerm, rows]);

  const fadeInUp = {
    hidden: { opacity: 0, y: 20 },
    visible: { opacity: 1, y: 0, transition: { duration: 0.8 } },
  };

  if (loading) return <p>Loading...</p>;
  if (error) return <p>Error: {error}</p>;

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
            <Button variant="contained" color="success" onClick={() => openAddModal()}>
              + Add Skill
            </Button>
          </Grid>
        </Grid>

        <Grid container spacing={3} mt={1}>
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
                    Skills ({filteredRows.length})
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

      {/* Modal */}
      <Dialog open={openModal} onClose={() => setOpenModal(false)} fullWidth>
        <DialogTitle>{editMode ? "Edit Skill" : "Add Skill"}</DialogTitle>
        <DialogContent>
          <TextField
            fullWidth
            margin="normal"
            label="Skill Name"
            value={formData.skillName}
            onChange={(e) => setFormData({ ...formData, skillName: e.target.value })}
          />
          <TextField
            fullWidth
            margin="normal"
            label="Icon URL"
            value={formData.iconUrl}
            onChange={(e) => setFormData({ ...formData, iconUrl: e.target.value })}
          />
        </DialogContent>
        <DialogActions>
          <Button
            onClick={() => setOpenModal(false)}
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
    </DashboardLayout>
  );
}

export default Skills;
