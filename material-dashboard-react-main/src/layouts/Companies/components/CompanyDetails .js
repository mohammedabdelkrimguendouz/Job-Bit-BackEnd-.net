/* eslint-disable react/prop-types */
import React from "react";
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
} from "@mui/material";
import CloseIcon from "@mui/icons-material/Close";
import EmailIcon from "@mui/icons-material/Email";
import PhoneIcon from "@mui/icons-material/Phone";
import LanguageIcon from "@mui/icons-material/Language";
import LocationOnIcon from "@mui/icons-material/LocationOn";
import CheckCircleIcon from "@mui/icons-material/CheckCircle";
import CancelIcon from "@mui/icons-material/Cancel";
import InfoIcon from "@mui/icons-material/Info";
import DescriptionIcon from "@mui/icons-material/Description";

// Define styles outside the component for better organization
const styles = {
  paper: {
    padding: 4,
    borderRadius: 4,
    boxShadow: "0 8px 24px rgba(0, 0, 0, 0.12)",
    position: "relative",
    overflow: "hidden",
  },
  bgDecoration: {
    position: "absolute",
    top: 0,
    right: 0,
    width: "150px",
    height: "150px",
    borderBottomLeftRadius: "100%",
    zIndex: 0,
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
  logoContainer: {
    display: "flex",
    justifyContent: "center",
    marginBottom: 3,
    position: "relative",
    zIndex: 1,
  },
  logo: {
    width: 100,
    height: 100,
    objectFit: "contain",
    boxShadow: "0 4px 12px rgba(0, 0, 0, 0.1)",
    border: "3px solid #f5f5f5",
    background: "white",
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
      background: "#36305E",
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
  },
  fieldValue: {
    color: "#202124",
    fontWeight: "bold",
    marginBottom: 0.5,
  },
  link: {
    color: "#36305E",
    textDecoration: "none",
    "&:hover": {
      textDecoration: "underline",
    },
  },
  icon: {
    color: "#36305E",
    fontSize: "small",
  },
  description: {
    padding: 1.5,
    borderRadius: 1,
    backgroundColor: "rgba(0, 0, 0, 0.02)",
    border: "1px solid rgba(0, 0, 0, 0.05)",
    color: "#202124",
  },
  locationChip: {
    backgroundColor: "rgba(54, 48, 94, 0.1)",
    color: "#36305E",
    fontWeight: 500,
  },
  activeStatus: {
    color: "#34a853",
    fontWeight: 500,
  },
  inactiveStatus: {
    color: "#ea4335",
    fontWeight: 500,
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

// Colors object for consistent theming
const colors = {
  primary: "#36305E",
  secondary: "#f5f5f5",
  accent: "#36305E",
  success: "#34a853",
  error: "#ea4335",
  text: "#202124",
  lightText: "#5f6368",
  divider: "rgba(0, 0, 0, 0.12)",
  gradientStart: "#36305E",
  gradientEnd: "#34a853",
};

// Component for company header section
const CompanyHeader = ({ company, onClose }) => {
  return (
    <Box sx={styles.header}>
      <Typography variant="h5" sx={styles.title}>
        <InfoIcon fontSize="small" /> Company Details
      </Typography>
      <IconButton onClick={onClose} sx={styles.closeButton}>
        <CloseIcon />
      </IconButton>
    </Box>
  );
};

// Component for company logo
const CompanyLogo = ({ logoPath }) => {
  if (!logoPath) return null;

  return (
    <Box sx={styles.logoContainer}>
      <Avatar src={logoPath} alt="Company Logo" sx={styles.logo} />
    </Box>
  );
};

// Component for company name
const CompanyName = ({ company }) => {
  return (
    <Box sx={{ marginBottom: 2 }}>
      <Typography variant="body2" sx={styles.fieldLabel} gutterBottom>
        Company Name
      </Typography>
      <Typography variant="h6" sx={styles.fieldValue}>
        {company.name}
      </Typography>
    </Box>
  );
};

// Component for company contact info
const CompanyContactInfo = ({ company }) => {
  return (
    <>
      <Box sx={styles.fieldContainer}>
        <EmailIcon sx={styles.icon} />
        <Typography variant="body1" color={colors.text}>
          {company.email}
        </Typography>
      </Box>

      <Box sx={styles.fieldContainer}>
        <PhoneIcon sx={styles.icon} />
        <Typography variant="body1" color={colors.text}>
          {company.phone}
        </Typography>
      </Box>

      <Box sx={styles.fieldContainer}>
        <LanguageIcon sx={styles.icon} />
        <Link href={company.link} target="_blank" rel="noopener" sx={styles.link}>
          {company.link}
        </Link>
      </Box>
    </>
  );
};

// Component for company description
const CompanyDescription = ({ company }) => {
  return (
    <Box sx={{ marginBottom: 2 }}>
      <Typography variant="body2" sx={styles.fieldLabel} gutterBottom>
        Description
      </Typography>
      <Typography variant="body1" sx={styles.description}>
        {company.description ? company.description : "Not Available"}
      </Typography>
    </Box>
  );
};

// Component for company location
const CompanyLocation = ({ company }) => {
  return (
    <Box sx={styles.fieldContainer}>
      <LocationOnIcon sx={styles.icon} />
      <Chip label={company.wilayaInfo?.name} size="small" sx={styles.locationChip} />
    </Box>
  );
};

// Component for company status
const CompanyStatus = ({ company }) => {
  return (
    <Box sx={styles.fieldContainer}>
      <Tooltip title={company.isActive ? "Company is active" : "Company is inactive"}>
        {company.isActive ? (
          <CheckCircleIcon fontSize="small" sx={{ color: colors.success }} />
        ) : (
          <CancelIcon fontSize="small" sx={{ color: colors.error }} />
        )}
      </Tooltip>
      <Typography variant="body1" color={colors.text}>
        Status:{" "}
        {company.isActive ? (
          <span style={styles.activeStatus}>Active</span>
        ) : (
          <span style={styles.inactiveStatus}>Inactive</span>
        )}
      </Typography>
    </Box>
  );
};

// Main component
const CompanyDetails = ({ company, onClose }) => {
  return (
    <Paper
      elevation={5}
      sx={{
        ...styles.paper,
        background: `linear-gradient(145deg, ${colors.secondary} 0%, white 100%)`,
      }}
    >
      {/* Background decoration */}
      <Box
        sx={{
          ...styles.bgDecoration,
          background: `linear-gradient(135deg, ${colors.gradientStart}10, ${colors.gradientEnd}20)`,
        }}
      />

      {/* Company header with title and close button */}
      <CompanyHeader company={company} onClose={onClose} />

      {/* Company logo */}
      <CompanyLogo logoPath={company.logoPath} />

      <Divider sx={styles.divider} />

      {/* Company information in single column */}
      <Box sx={styles.infoContainer}>
        {/* Company name */}
        <CompanyName company={company} />

        {/* Contact section */}
        <Typography sx={styles.sectionTitle}>
          <InfoIcon fontSize="small" /> Contact Information
        </Typography>
        <CompanyContactInfo company={company} />

        {/* Description section */}
        <Typography sx={styles.sectionTitle}>
          <DescriptionIcon fontSize="small" /> About
        </Typography>
        <CompanyDescription company={company} />

        {/* Location and status */}
        <CompanyLocation company={company} />
        <CompanyStatus company={company} />
      </Box>
    </Paper>
  );
};

export default CompanyDetails;
