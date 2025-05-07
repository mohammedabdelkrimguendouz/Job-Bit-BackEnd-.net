import { Box } from "@mui/material";
import PropTypes from "prop-types";

function CleanLayout({ image, children }) {
  return (
    <Box
      sx={{
        height: "100vh",
        display: "flex",
        alignItems: "center",
        justifyContent: "center",
        backgroundImage: `url(${image})`,
        backgroundSize: "cover",
        backgroundPosition: "center",
      }}
    >
      <Box sx={{ width: "100%", maxWidth: "400px", px: 2 }}>{children}</Box>
    </Box>
  );
}

CleanLayout.propTypes = {
  image: PropTypes.string.isRequired,
  children: PropTypes.node.isRequired,
};

export default CleanLayout;
