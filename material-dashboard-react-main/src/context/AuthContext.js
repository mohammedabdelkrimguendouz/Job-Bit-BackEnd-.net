/* eslint-disable react/prop-types */
import React, { createContext, useContext, useState, useEffect, useCallback } from "react";
import { useNavigate } from "react-router-dom";

const AuthContext = createContext({
  user: null,
  login: () => {},
  logout: () => {},
  loading: true,
  isAuthenticated: false,
});

export function AuthProvider({ children }) {
  const [user, setUser] = useState(null);
  const [loading, setLoading] = useState(true);
  const navigate = useNavigate();

  useEffect(() => {
    try {
      const storedUser = localStorage.getItem("user");
      if (storedUser) {
        setUser(JSON.parse(storedUser));
      }
    } catch (error) {
      localStorage.removeItem("user");
    } finally {
      setLoading(false);
    }
  }, []);

  const login = useCallback((userData) => {
    if (!userData) {
      return;
    }

    try {
      const userString = JSON.stringify(userData);
      localStorage.setItem("user", userString);
      setUser(userData);
    } catch (error) {}
  }, []);

  const logout = useCallback(() => {
    localStorage.removeItem("user");
    setUser(null); // Update state after removing localStorage
    navigate("/authentication/sign-in");
  }, [navigate]);

  const contextValue = {
    user,
    login,
    logout,
    loading,
    isAuthenticated: !!user,
  };

  return <AuthContext.Provider value={contextValue}>{children}</AuthContext.Provider>;
}

export function useAuth() {
  const context = useContext(AuthContext);

  if (!context) {
    return {
      user: null,
      login: () => console.error("Auth context not available"),
      logout: () => console.error("Auth context not available"),
      loading: false,
      isAuthenticated: false,
    };
  }

  return context;
}
