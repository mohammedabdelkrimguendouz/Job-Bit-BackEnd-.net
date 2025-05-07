/* eslint-disable react/prop-types */
// src/components/ProtectedRoute.js
import { Navigate, useLocation } from "react-router-dom";
import { useAuth } from "context/AuthContext";
import MDBox from "components/MDBox";
import CircularProgress from "@mui/material/CircularProgress";

export default function ProtectedRoute({ children }) {
  const { user, loading } = useAuth();
  const location = useLocation();

  if (loading) {
    return (
      <MDBox display="flex" justifyContent="center" alignItems="center" height="100vh">
        <CircularProgress color="info" size={60} />
      </MDBox>
    );
  }

  if (!user) {
    // Save the current location they were trying to go to
    return <Navigate to="/authentication/sign-in" state={{ from: location }} replace />;
  }

  return children;
}
