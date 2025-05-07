import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import Card from "@mui/material/Card";

// Material Dashboard components
import MDBox from "components/MDBox";
import MDTypography from "components/MDTypography";
import MDInput from "components/MDInput";
import MDButton from "components/MDButton";
import { loginUser } from "../../../services/userService";

// Authentication layout component
import CleanLayout from "layouts/authentication/components/CleanLayout";

// Auth context
import { useAuth } from "context/AuthContext";

// Images
import bgImage from "assets/images/bg-sign-in-basic.jpeg";

function Basic() {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();
  const auth = useAuth(); // Get the full auth object instead of destructuring

  useEffect(() => {
    // Check if user is already logged in
    if (auth.user) {
      navigate("/dashboard");
    }
  }, [auth.user, navigate]);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError("");
    setLoading(true);

    try {
      // Use mock login for now
      const userData = await loginUser(email, password);

      // Check if auth.login exists before calling it
      if (typeof auth.login === "function") {
        auth.login(userData);
        console.log(localStorage.getItem("user"));
        console.log(userData);
        navigate("/dashboard");
      } else {
        setError("Authentication system is not available");
      }
    } catch (err) {
      setError("Incorrect Email Or Password");
    } finally {
      setLoading(false);
    }
  };

  return (
    <CleanLayout image={bgImage}>
      <Card>
        <MDBox
          mx={2}
          mt={-3}
          p={2}
          mb={1}
          textAlign="center"
          variant="gradient"
          sx={{
            background: "linear-gradient(135deg, #36305E, #5A4E8C)",
            borderRadius: "lg",
            coloredShadow: "info",
          }}
        >
          <MDTypography variant="h4" fontWeight="medium" color="white" mt={1}>
            Sign in
          </MDTypography>
        </MDBox>
        <MDBox pt={4} pb={3} px={3}>
          {error && (
            <MDTypography variant="body2" color="error" textAlign="center" mb={2}>
              {error}
            </MDTypography>
          )}
          <MDBox component="form" role="form" onSubmit={handleSubmit}>
            <MDBox mb={2}>
              <MDInput
                type="email"
                label="Email"
                fullWidth
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                required
              />
            </MDBox>
            <MDBox mb={2}>
              <MDInput
                type="password"
                label="Password"
                fullWidth
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                required
              />
            </MDBox>
            <MDBox mt={4} mb={1}>
              <MDButton
                variant="gradient"
                fullWidth
                type="submit"
                disabled={loading}
                sx={{
                  background: "linear-gradient(195deg, #36305E, #36305E)",
                  "&:hover": {
                    background: "linear-gradient(195deg, #2a254a, #2a254a)",
                  },
                }}
              >
                {loading ? "Signing in..." : "Sign in"}
              </MDButton>
            </MDBox>
          </MDBox>
        </MDBox>
      </Card>
    </CleanLayout>
  );
}

export default Basic;
