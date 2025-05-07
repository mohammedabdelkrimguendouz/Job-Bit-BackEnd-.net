// src/services/authService.js
import axiosInstance from "./axiosInstance";

export const loginUser = async (email, password) => {
  try {
    const response = await axiosInstance.post("/Auth/LogInUser", {
      Email: email,
      Password: password,
    });

    return response.data;
  } catch (error) {
    throw new Error(error.response?.data?.message || "Login failed");
  }
};

export const updateUserProfile = async (id, Email, Phone, CurrentPassword, Password) => {
  try {
    const response = await axiosInstance.put("/Users/UpdateUser", {
      userID: id,
      email: Email,
      password: Password,
      phone: Phone,
      isActive: true,
      currentPassword: CurrentPassword,
    });

    return response.data;
  } catch (error) {
    throw error;
  }
};
