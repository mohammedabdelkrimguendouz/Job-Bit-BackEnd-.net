/* eslint-disable react/prop-types */
import React, { useState, useEffect } from "react";
import { getProfilePicture, getCV } from "../../../services/JobSeekerService";
import {
  Typography,
  Divider,
  Grid,
  IconButton,
  Box,
  Paper,
  Chip,
  Avatar,
  Link,
  Tooltip,
  Button,
  CircularProgress,
  Stack,
} from "@mui/material";
import {
  Close as CloseIcon,
  Email as EmailIcon,
  Phone as PhoneIcon,
  Person as PersonIcon,
  Cake as CakeIcon,
  LocationOn as LocationOnIcon,
  CheckCircle as CheckCircleIcon,
  Cancel as CancelIcon,
  Info as InfoIcon,
  Description as DescriptionIcon,
  LinkedIn as LinkedInIcon,
  GitHub as GitHubIcon,
  Code as CodeIcon,
  Work as WorkIcon,
} from "@mui/icons-material";

const styles = {
  modalContainer: {
    position: "fixed",
    inset: 0,
    backgroundColor: "rgba(0, 0, 0, 0.5)",
    display: "flex",
    alignItems: "center",
    justifyContent: "center",
    zIndex: 1300,
    overflow: "auto", // تمكين التمرير للمحتوى الكبير
    py: 4, // إضافة padding عمودي
  },
  paper: {
    padding: 4,
    borderRadius: 4,
    boxShadow: "0 8px 24px rgba(0, 0, 0, 0.12)",
    position: "relative",
    overflow: "hidden",
    maxWidth: 800,
    width: "90%", // عرض نسبي ليتناسب مع الشاشات الصغيرة
    maxHeight: "90vh", // ارتفاع أقصى
    display: "flex",
    flexDirection: "column",
  },
  scrollableContent: {
    overflowY: "auto", // تمكين التمرير العمودي
    flex: 1, // يأخذ المساحة المتبقية
    pr: 2, // إضافة padding لتجنب اختفاء المحتوى
    "&::-webkit-scrollbar": {
      width: "6px",
    },
    "&::-webkit-scrollbar-track": {
      background: "#f1f1f1",
    },
    "&::-webkit-scrollbar-thumb": {
      background: "#888",
      borderRadius: "3px",
    },
    "&::-webkit-scrollbar-thumb:hover": {
      background: "#555",
    },
  },
  // بقية الأنماط تبقى كما هي
  bgDecoration: {
    position: "absolute",
    top: 0,
    right: 0,
    width: "150px",
    height: "150px",
    borderBottomLeftRadius: "100%",
    zIndex: 0,
    background: "linear-gradient(135deg, #4285f410, #34a85320)",
  },
  header: {
    display: "flex",
    justifyContent: "space-between",
    alignItems: "center",
    marginBottom: 3,
    position: "relative",
    zIndex: 1,
  },
  title: {
    color: "#36305E",
    display: "flex",
    alignItems: "center",
    gap: 1,
    fontWeight: 700,
  },
  closeButton: {
    color: "#5f6368",
    "&:hover": {
      color: "#ea4335",
      background: "rgba(234, 67, 53, 0.08)",
    },
  },
  avatarContainer: {
    display: "flex",
    justifyContent: "center",
    marginBottom: 3,
    position: "relative",
    zIndex: 1,
  },
  avatar: {
    width: 120,
    height: 120,
    fontSize: 48,
    boxShadow: "0 4px 12px rgba(0, 0, 0, 0.1)",
    border: "3px solid #f5f5f5",
  },
  divider: {
    marginBottom: 3,
    borderColor: "rgba(0, 0, 0, 0.12)",
    position: "relative",
    zIndex: 1,
    "&::before": {
      content: '""',
      position: "absolute",
      left: 0,
      width: "60px",
      height: "3px",
      background: "#36305E", // كان #1a73e8
      bottom: "-1px",
    },
  },
  infoContainer: {
    position: "relative",
    zIndex: 1,
  },
  fieldContainer: {
    marginBottom: 2,
    display: "flex",
    alignItems: "center",
    gap: 1,
  },
  fieldLabel: {
    color: "#5f6368",
    fontWeight: 500,
    minWidth: 100,
  },
  fieldValue: {
    color: "#202124",
    fontWeight: "bold",
  },
  sectionTitle: {
    marginTop: 3,
    marginBottom: 2,
    color: "#36305E",
    fontWeight: 600,
    fontSize: "1rem",
    display: "flex",
    alignItems: "center",
    gap: 1,
  },
};

const JobSeekerDetails = ({ jobSeeker, isOpen, onClose }) => {
  const [profileImageUrl, setProfileImageUrl] = useState(null);
  const [loadingImage, setLoadingImage] = useState(true);

  useEffect(() => {
    if (jobSeeker?.jobSeekerID) {
      setLoadingImage(true);
      getProfilePicture(jobSeeker.jobSeekerID)
        .then((res) => {
          const imageUrl = URL.createObjectURL(res.data);
          setProfileImageUrl(imageUrl);
        })
        .catch(() => {
          setProfileImageUrl(null);
        })
        .finally(() => setLoadingImage(false));
    }
  }, [jobSeeker]);

  if (!jobSeeker || !isOpen) return null;

  const formatDate = (dateString) => {
    if (!dateString) return "N/A";
    return new Date(dateString).toLocaleDateString(undefined, {
      year: "numeric",
      month: "long",
      day: "numeric",
    });
  };

  const handleDownloadCV = async () => {
    try {
      const cvBlob = await getCV(jobSeeker.jobSeekerID);

      const url = window.URL.createObjectURL(new Blob([cvBlob]));
      const link = document.createElement("a");
      link.href = url;
      link.setAttribute("download", `CV_${jobSeeker.firstName}_${jobSeeker.lastName}.pdf`);
      document.body.appendChild(link);
      link.click();
      link.remove();

      window.URL.revokeObjectURL(url);
    } catch (error) {
      console.error("Error downloading CV:", error);
    }
  };

  const handlePreviewCV = async () => {
    try {
      const cvBlob = await getCV(jobSeeker.jobSeekerID);
      const cvUrl = URL.createObjectURL(cvBlob);

      window.open(cvUrl, "_blank", "noopener,noreferrer");

      setTimeout(() => {
        URL.revokeObjectURL(cvUrl);
      }, 10000); // بعد 10 ثواني
    } catch (error) {
      console.error("Error previewing CV:", error);
    }
  };

  return (
    <Box sx={styles.modalContainer}>
      <Paper sx={styles.paper}>
        <Box sx={styles.bgDecoration} />

        {/* الجزء العلوي الثابت (غير قابل للتمرير) */}
        <Box>
          <Box sx={styles.header}>
            <Typography variant="h5" sx={styles.title}>
              <InfoIcon fontSize="small" />
              Candidate Profile
            </Typography>
            <IconButton onClick={onClose} sx={styles.closeButton}>
              <CloseIcon />
            </IconButton>
          </Box>

          <Box sx={styles.avatarContainer}>
            {loadingImage ? (
              <Avatar sx={{ ...styles.avatar, display: "flex", justifyContent: "center" }}>
                <CircularProgress size={24} />
              </Avatar>
            ) : profileImageUrl ? (
              <Avatar src={profileImageUrl} sx={styles.avatar} />
            ) : (
              <Avatar sx={styles.avatar}>
                {jobSeeker.firstName?.charAt(0)}
                {jobSeeker.lastName?.charAt(0)}
              </Avatar>
            )}
          </Box>

          <Box display="flex" justifyContent="center" mb={3}>
            <Chip
              label={jobSeeker.isActive ? "Active" : "Inactive"}
              color={jobSeeker.isActive ? "success" : "error"}
              size="small"
              icon={
                jobSeeker.isActive ? (
                  <CheckCircleIcon fontSize="small" />
                ) : (
                  <CancelIcon fontSize="small" />
                )
              }
            />
          </Box>

          <Divider sx={styles.divider} />
        </Box>

        <Box sx={styles.scrollableContent}>
          <Box sx={styles.infoContainer}>
            {/* Basic Info */}
            <Typography sx={styles.sectionTitle}>
              <PersonIcon fontSize="small" />
              Basic Information
            </Typography>
            <Grid container spacing={2}>
              <Grid item xs={12} sm={6}>
                <Box sx={styles.fieldContainer}>
                  <Typography sx={styles.fieldLabel}>Full Name:</Typography>
                  <Typography sx={styles.fieldValue}>
                    {jobSeeker.firstName} {jobSeeker.lastName}
                  </Typography>
                </Box>
              </Grid>
              <Grid item xs={12} sm={6}>
                <Box sx={styles.fieldContainer}>
                  <Typography sx={styles.fieldLabel}>Birthdate:</Typography>
                  <Typography sx={styles.fieldValue}>
                    {formatDate(jobSeeker.dateOfBirth)}
                  </Typography>
                </Box>
              </Grid>
              <Grid item xs={12} sm={6}>
                <Box sx={styles.fieldContainer}>
                  <Typography sx={styles.fieldLabel}>Location:</Typography>
                  <Typography sx={styles.fieldValue}>
                    {jobSeeker.wilayaInfo?.name || "N/A"}
                  </Typography>
                </Box>
              </Grid>
            </Grid>

            {/* Contact Info */}
            <Typography sx={{ ...styles.sectionTitle, mt: 4 }}>
              <EmailIcon fontSize="small" />
              Contact Information
            </Typography>
            <Grid container spacing={2}>
              <Grid item xs={12} sm={6}>
                <Box sx={styles.fieldContainer}>
                  <EmailIcon fontSize="small" sx={{ color: "#36305E" }} />
                  <Typography sx={styles.fieldValue}>{jobSeeker.email || "N/A"}</Typography>
                </Box>
              </Grid>
              <Grid item xs={12} sm={6}>
                <Box sx={styles.fieldContainer}>
                  <PhoneIcon fontSize="small" sx={{ color: "#36305E" }} />
                  <Typography sx={styles.fieldValue}>{jobSeeker.phone || "N/A"}</Typography>
                </Box>
              </Grid>
            </Grid>

            {/* External Links Section - Improved */}
            {(jobSeeker.linkProfileLinkden ||
              jobSeeker.linkProfileGithub ||
              jobSeeker.cvFilePath) && (
              <>
                <Typography
                  sx={{
                    ...styles.sectionTitle,
                    mt: 4,
                    fontSize: "1.1rem",
                    color: "#2c387e",
                    "& svg": {
                      color: "#2c387e",
                    },
                  }}
                >
                  <WorkIcon fontSize="small" />
                  External Links
                </Typography>

                <Box
                  sx={{
                    backgroundColor: "#f8f9fa",
                    p: 2,
                    borderRadius: 2,
                    borderLeft: "4px solid #2c387e",
                  }}
                >
                  <Grid container spacing={2}>
                    {/* LinkedIn Link */}
                    {jobSeeker.linkProfileLinkden && (
                      <Grid item xs={12} sm={6}>
                        <Button
                          fullWidth
                          variant="outlined"
                          startIcon={<LinkedInIcon sx={{ color: "#0077b5" }} />}
                          href={jobSeeker.linkProfileLinkden}
                          target="_blank"
                          sx={{
                            ...styles.linkButton,
                            justifyContent: "flex-start",
                            borderColor: "#0077b5",
                            color: "#0077b5",
                            "&:hover": {
                              backgroundColor: "#0077b510",
                              borderColor: "#006097",
                            },
                          }}
                        >
                          LinkedIn Profile
                        </Button>
                      </Grid>
                    )}

                    {/* GitHub Link */}
                    {jobSeeker.linkProfileGithub && (
                      <Grid item xs={12} sm={6}>
                        <Button
                          fullWidth
                          variant="outlined"
                          startIcon={<GitHubIcon sx={{ color: "#333" }} />}
                          href={jobSeeker.linkProfileGithub}
                          target="_blank"
                          sx={{
                            ...styles.linkButton,
                            justifyContent: "flex-start",
                            borderColor: "#333",
                            color: "#333",
                            "&:hover": {
                              backgroundColor: "#33310",
                              borderColor: "#222",
                            },
                          }}
                        >
                          GitHub Profile
                        </Button>
                      </Grid>
                    )}

                    {/* CV Options */}
                    {jobSeeker.cvFilePath && (
                      <>
                        <Grid item xs={12}>
                          <Typography
                            variant="subtitle2"
                            sx={{
                              color: "#555",
                              mb: 1,
                              display: "flex",
                              alignItems: "center",
                              gap: 1,
                            }}
                          >
                            <DescriptionIcon fontSize="small" />
                            CV options:
                          </Typography>
                        </Grid>

                        <Grid item xs={12} sm={6}>
                          <Button
                            fullWidth
                            variant="contained"
                            startIcon={<DescriptionIcon />}
                            onClick={handleDownloadCV}
                            sx={{
                              ...styles.linkButton,
                              backgroundColor: "#2c387e",
                              "&:hover": {
                                backgroundColor: "#1a237e",
                              },
                            }}
                          >
                            Download CV
                          </Button>
                        </Grid>

                        <Grid item xs={12} sm={6}>
                          <Button
                            fullWidth
                            variant="outlined"
                            startIcon={<DescriptionIcon />}
                            onClick={handlePreviewCV}
                            sx={{
                              ...styles.linkButton,
                              borderColor: "#2c387e",
                              color: "#2c387e",
                              "&:hover": {
                                backgroundColor: "#2c387e10",
                              },
                            }}
                          >
                            Preview CV
                          </Button>
                        </Grid>
                      </>
                    )}
                  </Grid>
                </Box>
              </>
            )}

            {/* Skills Section */}
            {jobSeeker.skills?.length > 0 && (
              <>
                <Typography sx={{ ...styles.sectionTitle, mt: 4 }}>
                  <CodeIcon fontSize="small" />
                  Technical Skills
                </Typography>
                <Box display="flex" flexWrap="wrap" gap={1}>
                  {jobSeeker.skills.map((skill) => (
                    <Tooltip key={skill.skillID} title={skill.name}>
                      <Avatar src={skill.iconUrl} alt={skill.name} sx={{ width: 40, height: 40 }} />
                    </Tooltip>
                  ))}
                </Box>
              </>
            )}
          </Box>
        </Box>
      </Paper>
    </Box>
  );
};

export default JobSeekerDetails;
