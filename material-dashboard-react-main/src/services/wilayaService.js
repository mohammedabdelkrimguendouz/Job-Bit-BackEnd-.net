// src/services/wilayaService.js
import axiosInstance from "./axiosInstance";

export const getAllWilayas = async () => {
  try {
    const response = await axiosInstance.get("/Wilayas/GetAllWilayas");
    return response.data;
  } catch (error) {
    throw new Error(error.response?.data?.message || "Failed to fetch Wilayas");
  }
};

export const deleteWilaya = async (WilayaID) => {
  try {
    const response = await axiosInstance.delete(`/Wilayas/DeleteWilaya/${WilayaID}`);
    return response.data;
  } catch (error) {
    throw error;
  }
};

export const addWilaya = async (wilayaName) => {
  try {
    const response = await axiosInstance.post("/Wilayas/AddWilaya", {
      Name: wilayaName,
      WilayaID: -1,
    });

    if (response.status === 201) {
      return response.data;
    }
  } catch (error) {
    throw error;
  }
};

export const updateWilaya = async (wilayaID, wilayaName) => {
  try {
    const response = await axiosInstance.put("/Wilayas/UpdateWilaya", {
      WilayaID: wilayaID,
      Name: wilayaName,
    });

    if (response.status === 200) {
      return response.data;
    }
  } catch (error) {
    throw error;
  }
};
