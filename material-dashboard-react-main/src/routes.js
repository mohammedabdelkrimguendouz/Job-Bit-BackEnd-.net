import Dashboard from "layouts/dashboard";
import Profile from "layouts/profile";
import SignIn from "layouts/authentication/sign-in";
import SignOut from "layouts/authentication/sign-out";
import Companies from "layouts/Companies";
import JobSeekers from "layouts/JobSeekers";
import Wilayas from "layouts/Wilayas";
import Skills from "layouts/skills";

// @mui icons
import Icon from "@mui/material/Icon";

const routes = [
  {
    type: "collapse",
    name: "Dashboard",
    key: "dashboard",
    icon: <Icon fontSize="small">dashboard</Icon>,
    route: "/dashboard",
    component: <Dashboard />,
    protected: true,
  },
  {
    type: "collapse",
    name: "Companies",
    key: "companies",
    icon: <Icon fontSize="small">business</Icon>,
    route: "/companies",
    component: <Companies />,
    protected: true,
  },
  {
    type: "collapse",
    name: "JobSeekers",
    key: "jobSeekers",
    icon: <Icon fontSize="small">people</Icon>,
    route: "/jobSeekers",
    component: <JobSeekers />,
    protected: true,
  },
  {
    type: "collapse",
    name: "Wilayas",
    key: "wilayas",
    icon: <Icon fontSize="small">map</Icon>,
    route: "/wilayas",
    component: <Wilayas />,
    protected: true,
  },
  {
    type: "collapse",
    name: "Skills",
    key: "skills",
    icon: <Icon fontSize="small">code</Icon>,
    route: "/skills",
    component: <Skills />,
    protected: true,
  },
  {
    type: "collapse",
    name: "Profile",
    key: "profile",
    icon: <Icon fontSize="small">account_circle</Icon>,
    route: "/profile",
    component: <Profile />,
    protected: true,
  },
  {
    type: "collapse",
    name: "Sign In",
    key: "sign-in",
    icon: <Icon fontSize="small">login</Icon>,
    route: "/authentication/sign-in",
    component: <SignIn />,
    protected: false,
    hideWhenAuth: true,
  },
  {
    type: "collapse",
    name: "Sign Out",
    key: "sign-out",
    icon: <Icon fontSize="small">logout</Icon>,
    route: "/sign-out",
    component: <SignOut />,
    protected: true,
  },
];

export default routes;
