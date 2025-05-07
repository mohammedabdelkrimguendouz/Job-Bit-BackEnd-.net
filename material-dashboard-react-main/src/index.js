import React from "react";
import { createRoot } from "react-dom/client";
import { BrowserRouter } from "react-router-dom";
import App from "App";
import { AuthProvider } from "context/AuthContext";
import { MaterialUIControllerProvider } from "context";

const container = document.getElementById("app");
const root = createRoot(container);

root.render(
  <React.StrictMode>
    <BrowserRouter>
      <MaterialUIControllerProvider>
        <AuthProvider>
          <App />
        </AuthProvider>
      </MaterialUIControllerProvider>
    </BrowserRouter>
  </React.StrictMode>
);
