import { useEffect } from "react";
import { useAuth } from "context/AuthContext";
import { useNavigate } from "react-router-dom";

function SignOut() {
  const auth = useAuth(); // Get the entire auth object first
  const navigate = useNavigate();

  useEffect(() => {
    // Check if auth and logout function exist before using them
    if (auth && typeof auth.logout === "function") {
      auth.logout();
    } else {
      // Fallback - clear localStorage manually if logout function isn't available

      localStorage.removeItem("user");
    }

    // Always navigate to sign-in page
    navigate("/authentication/sign-in");
  }, [auth, navigate]);

  return null; // No UI to render
}

export default SignOut;
