import axiosInstance from "./axiosInstance";

export const getAllCompanies = async () => {
  try {
    const response = await axiosInstance.get("/Companies/GetAllCompanies");
    return response.data;
  } catch (error) {
    throw new Error(error.response?.data?.message || "Failed to fetch companies");
  }
};

export const deleteCompany = async (companyID) => {
  try {
    const response = await axiosInstance.delete(`/Companies/DeleteCompany/${companyID}`);
    return response.data;
  } catch (error) {
    throw error;
  }
};

export const updateCompanyActivityStatus = async (companyID, newStatus) => {
  try {
    const response = await axiosInstance.put(
      `/Companies/UpdateCompanyActivityStatus/${companyID},${newStatus}`
    );
    return response.data;
  } catch (error) {
    throw error;
  }
};

export const getCompanyByID = async (companyID) => {
  try {
    const response = await axiosInstance.get(`/Companies/GetCompanyByID/${companyID}`);
    return response.data;
  } catch (error) {
    throw error;
  }
};
