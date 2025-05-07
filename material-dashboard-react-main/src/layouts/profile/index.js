import { useState } from "react";
import Grid from "@mui/material/Grid";
import EditIcon from "@mui/icons-material/Edit";

// Material Dashboard 2 React components
import MDBox from "components/MDBox";
import MDTypography from "components/MDTypography";
import MDSnackbar from "components/MDSnackbar";

// Material Dashboard 2 React example components
import DashboardLayout from "examples/LayoutContainers/DashboardLayout";
import Header from "layouts/profile/components/Header";

// Auth Context
import { useAuth } from "../../context/AuthContext";
import { updateUserProfile } from "../../services/userService";

function Overview() {
  const { user, login } = useAuth();
  const [isEditing, setIsEditing] = useState(false);
  const [formData, setFormData] = useState({
    email: user?.email || "",
    phone: user?.phone || "",
    currentPassword: "",
    password: "",
    userID: user?.userID || "",
  });
  const [notification, setNotification] = useState({
    open: false,
    color: "info",
    message: "",
  });

  const handleEdit = () => {
    setIsEditing(true);
  };

  const handleCancel = () => {
    setIsEditing(false);
    setFormData({
      email: user?.email || "",
      phone: user?.phone || "",
      currentPassword: "",
      password: "",
      userID: user?.userID || "",
    });
  };

  const handleChange = (e) => {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value,
    });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (!user?.userID) {
      setNotification({
        open: true,
        color: "error",
        message: "User ID not found",
      });
      return;
    }

    try {
      const result = await updateUserProfile(
        user.userID,
        formData.email,
        formData.phone,
        formData.currentPassword,
        formData.password
      );

      const updatedUser = {
        ...user,
        email: formData.email,
        phone: formData.phone,
      };

      login(updatedUser);

      setNotification({
        open: true,
        color: "success",
        message: result.message || "Profile updated successfully!",
      });

      setIsEditing(false);
    } catch (error) {
      const errorMessage =
        error.response?.data?.message ||
        error.response?.data?.errors?.join(", ") ||
        "Failed to update profile";

      setNotification({
        open: true,
        color: "error",
        message: errorMessage,
      });
    }
  };

  const handleCloseNotification = () => {
    setNotification({
      ...notification,
      open: false,
    });
  };

  return (
    <DashboardLayout>
      <MDBox mb={2} />
      <Header>
        {/* Professional Title Section */}
        <MDBox
          display="flex"
          justifyContent="center"
          mt={5}
          mb={4}
          sx={{
            background: "linear-gradient(195deg, #36305E, #36305E)",
            borderRadius: "12px",
            padding: "2rem 1rem",
            boxShadow: "0 4px 20px 0 rgba(0,0,0,0.14)",
          }}
        >
          <MDBox textAlign="center">
            <MDTypography variant="h3" fontWeight="bold" color="white">
              {user?.name || "User Profile"}
            </MDTypography>
            <MDTypography variant="body2" fontWeight="regular" color="white" opacity={0.8}>
              Manage your account information
            </MDTypography>
          </MDBox>
        </MDBox>

        {/* Profile Information Card */}
        <MDBox mt={3} mb={5}>
          <Grid container justifyContent="center">
            <Grid item xs={12} md={8} lg={6}>
              <MDBox
                p={4}
                bgcolor="white"
                borderRadius="lg"
                shadow="lg"
                sx={{
                  border: "1px solid #e0e0e0",
                  transition: "box-shadow 0.3s ease-in-out",
                  "&:hover": {
                    boxShadow: "0 8px 24px 0 rgba(0,0,0,0.12)",
                  },
                }}
              >
                {isEditing ? (
                  <MDBox component="form" onSubmit={handleSubmit}>
                    <MDTypography
                      variant="h5"
                      fontWeight="bold"
                      mb={4}
                      textAlign="center"
                      color="#36305E"
                    >
                      Edit Profile Information
                    </MDTypography>

                    <Grid container spacing={3}>
                      <Grid item xs={12}>
                        <MDBox mb={3}>
                          <MDTypography variant="body2" fontWeight="medium" mb={1}>
                            User ID
                          </MDTypography>
                          <MDBox
                            p={1.5}
                            borderRadius="md"
                            bgcolor="#f8f9fa"
                            border="1px solid #e0e0e0"
                          >
                            <MDTypography variant="button" fontWeight="regular">
                              {formData.userID}
                            </MDTypography>
                          </MDBox>
                        </MDBox>
                      </Grid>

                      <Grid item xs={12}>
                        <MDBox mb={3}>
                          <MDTypography variant="body2" fontWeight="medium" mb={1}>
                            Status
                          </MDTypography>
                          <MDBox
                            p={1.5}
                            borderRadius="md"
                            bgcolor="#f8f9fa"
                            border="1px solid #e0e0e0"
                          >
                            <MDTypography variant="button" fontWeight="regular">
                              {user?.isActive ? "Active" : "Inactive"}
                            </MDTypography>
                          </MDBox>
                        </MDBox>
                      </Grid>

                      <Grid item xs={12}>
                        <MDBox mb={3}>
                          <MDTypography variant="body2" fontWeight="medium" mb={1}>
                            Email Address
                          </MDTypography>
                          <input
                            type="email"
                            name="email"
                            value={formData.email}
                            onChange={handleChange}
                            style={{
                              width: "100%",
                              padding: "12px",
                              borderRadius: "8px",
                              border: "1px solid #e0e0e0",
                              fontSize: "0.875rem",
                              transition: "border-color 0.3s",
                              outline: "none",
                            }}
                            required
                          />
                        </MDBox>
                      </Grid>

                      <Grid item xs={12}>
                        <MDBox mb={3}>
                          <MDTypography variant="body2" fontWeight="medium" mb={1}>
                            Phone Number
                          </MDTypography>
                          <input
                            type="text"
                            name="phone"
                            value={formData.phone}
                            onChange={handleChange}
                            style={{
                              width: "100%",
                              padding: "12px",
                              borderRadius: "8px",
                              border: "1px solid #e0e0e0",
                              fontSize: "0.875rem",
                              outline: "none",
                            }}
                          />
                        </MDBox>
                      </Grid>

                      <Grid item xs={12}>
                        <MDBox mb={3}>
                          <MDTypography variant="body2" fontWeight="medium" mb={1}>
                            Current Password
                          </MDTypography>
                          <input
                            type="password"
                            name="currentPassword"
                            value={formData.currentPassword}
                            onChange={handleChange}
                            style={{
                              width: "100%",
                              padding: "12px",
                              borderRadius: "8px",
                              border: "1px solid #e0e0e0",
                              fontSize: "0.875rem",
                              outline: "none",
                            }}
                            required
                          />
                        </MDBox>
                      </Grid>

                      <Grid item xs={12}>
                        <MDBox mb={4}>
                          <MDTypography variant="body2" fontWeight="medium" mb={1}>
                            New Password (optional)
                          </MDTypography>
                          <input
                            type="password"
                            name="password"
                            value={formData.password}
                            onChange={handleChange}
                            style={{
                              width: "100%",
                              padding: "12px",
                              borderRadius: "8px",
                              border: "1px solid #e0e0e0",
                              fontSize: "0.875rem",
                              outline: "none",
                            }}
                          />
                        </MDBox>
                      </Grid>
                    </Grid>

                    <MDBox display="flex" justifyContent="flex-end" mt={2}>
                      <button
                        type="button"
                        onClick={handleCancel}
                        style={{
                          padding: "10px 24px",
                          borderRadius: "8px",
                          border: "1px solid #e0e0e0",
                          backgroundColor: "white",
                          color: "#555",
                          cursor: "pointer",
                          fontWeight: "500",
                          fontSize: "0.875rem",
                          marginRight: "16px",
                          transition: "all 0.2s ease",
                        }}
                      >
                        Cancel
                      </button>
                      <button
                        type="submit"
                        style={{
                          padding: "10px 24px",
                          borderRadius: "8px",
                          border: "none",
                          background: "linear-gradient(195deg, #36305E, #36305E)",
                          color: "white",
                          cursor: "pointer",
                          fontWeight: "500",
                          fontSize: "0.875rem",
                          transition: "all 0.2s ease",
                          boxShadow: "0 4px 10px rgba(54, 48, 94, 0.3)",
                        }}
                      >
                        Save Changes
                      </button>
                    </MDBox>
                  </MDBox>
                ) : (
                  <MDBox>
                    <MDTypography
                      variant="h5"
                      fontWeight="bold"
                      mb={4}
                      textAlign="center"
                      color="#36305E"
                    >
                      Profile Information
                    </MDTypography>

                    <Grid container spacing={3}>
                      <Grid item xs={12}>
                        <MDBox mb={3}>
                          <MDTypography variant="body2" fontWeight="medium" mb={1}>
                            User ID
                          </MDTypography>
                          <MDBox
                            p={1.5}
                            borderRadius="md"
                            bgcolor="#f8f9fa"
                            border="1px solid #e0e0e0"
                          >
                            <MDTypography variant="button" fontWeight="regular">
                              {user?.userID || "Not available"}
                            </MDTypography>
                          </MDBox>
                        </MDBox>
                      </Grid>

                      <Grid item xs={12}>
                        <MDBox mb={3}>
                          <MDTypography variant="body2" fontWeight="medium" mb={1}>
                            Status
                          </MDTypography>
                          <MDBox
                            p={1.5}
                            borderRadius="md"
                            bgcolor="#f8f9fa"
                            border="1px solid #e0e0e0"
                          >
                            <MDTypography variant="button" fontWeight="regular">
                              {user?.isActive ? "Active" : "Inactive"}
                            </MDTypography>
                          </MDBox>
                        </MDBox>
                      </Grid>

                      <Grid item xs={12}>
                        <MDBox mb={3}>
                          <MDTypography variant="body2" fontWeight="medium" mb={1}>
                            Email Address
                          </MDTypography>
                          <MDBox
                            p={1.5}
                            borderRadius="md"
                            bgcolor="#f8f9fa"
                            border="1px solid #e0e0e0"
                          >
                            <MDTypography variant="button" fontWeight="regular">
                              {user?.email || "Not available"}
                            </MDTypography>
                          </MDBox>
                        </MDBox>
                      </Grid>

                      <Grid item xs={12}>
                        <MDBox mb={4}>
                          <MDTypography variant="body2" fontWeight="medium" mb={1}>
                            Phone Number
                          </MDTypography>
                          <MDBox
                            p={1.5}
                            borderRadius="md"
                            bgcolor="#f8f9fa"
                            border="1px solid #e0e0e0"
                          >
                            <MDTypography variant="button" fontWeight="regular">
                              {user?.phone || "Not available"}
                            </MDTypography>
                          </MDBox>
                        </MDBox>
                      </Grid>
                    </Grid>

                    <MDBox display="flex" justifyContent="center" mt={4}>
                      <button
                        type="button"
                        onClick={handleEdit}
                        style={{
                          padding: "12px 32px",
                          borderRadius: "8px",
                          border: "none",
                          background: "linear-gradient(195deg, #36305E, #36305E)",
                          color: "white",
                          cursor: "pointer",
                          fontWeight: "500",
                          fontSize: "0.875rem",
                          transition: "all 0.2s ease",
                          boxShadow: "0 4px 10px rgba(54, 48, 94, 0.3)",
                          "&:hover": {
                            transform: "translateY(-2px)",
                            boxShadow: "0 6px 18px rgba(54, 48, 94, 0.4)",
                          },
                        }}
                      >
                        <MDBox display="flex" alignItems="center">
                          <EditIcon sx={{ mr: 1, fontSize: 18 }} />
                          Edit Profile
                        </MDBox>
                      </button>
                    </MDBox>
                  </MDBox>
                )}
              </MDBox>
            </Grid>
          </Grid>
        </MDBox>
      </Header>
      {/* Notification */}
      <MDSnackbar
        color={notification.color}
        icon="notifications"
        title="Notification"
        content={notification.message}
        open={notification.open}
        onClose={handleCloseNotification}
        close={handleCloseNotification}
        bgWhite
      />
    </DashboardLayout>
  );
}

export default Overview;
