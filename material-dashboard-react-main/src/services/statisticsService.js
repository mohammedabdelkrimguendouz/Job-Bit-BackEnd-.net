import axiosInstance from "./axiosInstance";

export const getStatistics = async () => {
  try {
    const response = await axiosInstance.get("/Statistics/GetStatistics");
    return response.data;
  } catch (error) {
    throw new Error(error.response?.data?.message || "Failed to fetch Statistics");
  }
};
