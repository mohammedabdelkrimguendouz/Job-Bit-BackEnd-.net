import { useState, useEffect, useMemo } from "react";
import { Routes, Route, Navigate, useLocation } from "react-router-dom";
import { ThemeProvider } from "@mui/material/styles";
import CssBaseline from "@mui/material/CssBaseline";
import Icon from "@mui/material/Icon";
import MDBox from "components/MDBox";
import Sidenav from "examples/Sidenav";
import Configurator from "examples/Configurator";
import theme from "assets/theme";
import themeRTL from "assets/theme/theme-rtl";
import themeDark from "assets/theme-dark";
import themeDarkRTL from "assets/theme-dark/theme-rtl";
import rtlPlugin from "stylis-plugin-rtl";
import { CacheProvider } from "@emotion/react";
import createCache from "@emotion/cache";
import routes from "routes";
import { useMaterialUIController, setMiniSidenav, setOpenConfigurator } from "context";
import { useAuth } from "context/AuthContext";
import brandWhite from "assets/images/logo-ct.png";
import brandDark from "assets/images/logo-ct-dark.png";
import ProtectedRoute from "components/ProtectedRoute";

export default function App() {
  const [controller, dispatch] = useMaterialUIController();
  const { user, loading } = useAuth();
  const {
    miniSidenav,
    direction,
    layout,
    openConfigurator,
    sidenavColor,
    transparentSidenav,
    whiteSidenav,
    darkMode,
  } = controller;

  const [onMouseEnter, setOnMouseEnter] = useState(false);
  const [rtlCache, setRtlCache] = useState(null);
  const { pathname } = useLocation();

  // Create RTL cache
  useMemo(() => {
    const cacheRtl = createCache({
      key: "rtl",
      stylisPlugins: [rtlPlugin],
    });
    setRtlCache(cacheRtl);
  }, []);

  // Handle sidenav hover
  const handleOnMouseEnter = () => {
    if (miniSidenav && !onMouseEnter) {
      setMiniSidenav(dispatch, false);
      setOnMouseEnter(true);
    }
  };

  const handleOnMouseLeave = () => {
    if (onMouseEnter) {
      setMiniSidenav(dispatch, true);
      setOnMouseEnter(false);
    }
  };

  // Toggle configurator
  const handleConfiguratorOpen = () => setOpenConfigurator(dispatch, !openConfigurator);

  // Update direction
  useEffect(() => {
    document.body.setAttribute("dir", direction);
  }, [direction]);

  // Scroll to top on route change
  useEffect(() => {
    document.documentElement.scrollTop = 0;
    document.scrollingElement.scrollTop = 0;
  }, [pathname]);

  // Filter routes based on auth status
  const filteredRoutes = useMemo(() => {
    return routes.filter((route) => {
      if (route.hideWhenAuth && user) return false;
      if (route.protected && !user) return false;
      return true;
    });
  }, [user]);

  // Handle route rendering with protection
  const getRoutes = (allRoutes) =>
    allRoutes.flatMap((route) => {
      if (route.collapse) {
        return getRoutes(route.collapse);
      }

      if (route.route) {
        // Special handling for sign-out route
        if (route.key === "sign-out") {
          return <Route key={route.key} path={route.route} element={route.component} />;
        }

        return route.protected ? (
          <Route
            key={route.key}
            path={route.route}
            element={<ProtectedRoute>{route.component}</ProtectedRoute>}
          />
        ) : (
          <Route key={route.key} path={route.route} element={route.component} />
        );
      }

      return [];
    });

  // Config + Logout buttons
  const configButton = (
    <MDBox
      display="flex"
      justifyContent="center"
      alignItems="center"
      width="3.25rem"
      height="3.25rem"
      bgColor="white"
      shadow="sm"
      borderRadius="50%"
      position="fixed"
      right="2rem"
      bottom="2rem"
      zIndex={99}
      color="dark"
      sx={{ cursor: "pointer" }}
      onClick={handleConfiguratorOpen}
    >
      <Icon fontSize="small" color="inherit">
        settings
      </Icon>
    </MDBox>
  );

  const appliedTheme =
    direction === "rtl" ? (darkMode ? themeDarkRTL : themeRTL) : darkMode ? themeDark : theme;

  // If still loading auth state, show a simple loading screen
  if (loading) {
    return (
      <ThemeProvider theme={appliedTheme}>
        <CssBaseline />
        <MDBox display="flex" justifyContent="center" alignItems="center" height="100vh">
          <MDBox sx={{ textAlign: "center" }}>
            <MDBox
              component="img"
              src={darkMode ? brandDark : brandWhite}
              alt="Brand Logo"
              width="80px"
              mb={2}
            />
            <MDBox sx={{ display: "flex", justifyContent: "center" }}>
              <div className="loading-spinner"></div>
            </MDBox>
          </MDBox>
        </MDBox>
      </ThemeProvider>
    );
  }

  const content = (
    <ThemeProvider theme={appliedTheme}>
      <CssBaseline />
      {layout === "dashboard" && user && (
        <>
          <Sidenav
            color={sidenavColor}
            brand={(transparentSidenav && !darkMode) || whiteSidenav ? brandDark : brandWhite}
            brandName="Admin Dashboard"
            routes={filteredRoutes}
            onMouseEnter={handleOnMouseEnter}
            onMouseLeave={handleOnMouseLeave}
          />
          <Configurator />
        </>
      )}
      {layout === "vr" && <Configurator />}
      <Routes>
        {getRoutes(routes)}
        <Route
          path="*"
          element={<Navigate to={user ? "/dashboard" : "/authentication/sign-in"} />}
        />
      </Routes>
    </ThemeProvider>
  );

  return direction === "rtl" ? <CacheProvider value={rtlCache}>{content}</CacheProvider> : content;
}
