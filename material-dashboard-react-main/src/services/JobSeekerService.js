import axiosInstance from "./axiosInstance";

export const getAllJobSeekers = async () => {
  try {
    const response = await axiosInstance.get("/JobSeekers/GetAllJobSeekers");
    return response.data;
  } catch (error) {
    throw new Error(error.response?.data?.message || "Failed to fetch job seekers");
  }
};

export const deleteJobSeeker = async (jobSeekerID) => {
  try {
    const response = await axiosInstance.delete(`/JobSeekers/DeleteJobSeeker/${jobSeekerID}`);
    return response.data;
  } catch (error) {
    throw error;
  }
};

export const updateJobSeekerActivityStatus = async (jobSeekerID, newStatus) => {
  try {
    const response = await axiosInstance.put(
      `/JobSeekers/UpdateJobSeekerActivityStatus/${jobSeekerID},${newStatus}`
    );
    return response.data;
  } catch (error) {
    throw error;
  }
};

export const getJobSeekerById = async (JobSeekerID) => {
  try {
    const response = await axiosInstance.get(`/JobSeekers/GetJobSeekerByID/${JobSeekerID}`);
    return response.data;
  } catch (error) {
    throw error;
  }
};

export const getProfilePicture = (jobSeekerId) => {
  try {
    return axiosInstance.get(`/JobSeekers/GetProfilePicture/${jobSeekerId}`, {
      responseType: "blob",
    });
  } catch (error) {
    throw error;
  }
};

export const getCV = async (jobSeekerId) => {
  try {
    const response = await axiosInstance.get(`/JobSeekers/GetCV/${jobSeekerId}`, {
      responseType: "blob",
    });
    return response.data;
  } catch (error) {
    throw new Error(error.response?.data?.message || "Failed to download CV");
  }
};
