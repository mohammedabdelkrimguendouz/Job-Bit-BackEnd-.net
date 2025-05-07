// src/services/skillService.js
import axiosInstance from "./axiosInstance";

export const getAllSkills = async () => {
  try {
    const response = await axiosInstance.get("/Skills/GetAllSkills");
    return response.data;
  } catch (error) {
    throw new Error(error.response?.data?.message || "Failed to fetch Skills");
  }
};

export const deleteSkill = async (SkillID) => {
  try {
    const response = await axiosInstance.delete(`/Skills/DeleteSkill/${SkillID}`);
    return response.data;
  } catch (error) {
    throw error;
  }
};

export const addSkill = async (Name, IconUrl) => {
  try {
    const response = await axiosInstance.post("/Skills/AddSkill", {
      skillID: -1,
      name: Name,
      iconUrl: IconUrl,
    });
    if (response.status === 201) {
      return response.data;
    }
  } catch (error) {
    throw error;
  }
};

export const updateSkill = async (SkillID, Name, IconUrl) => {
  try {
    const response = await axiosInstance.put("/Skills/UpdateSkill", {
      skillID: SkillID,
      name: Name,
      iconUrl: IconUrl,
    });
    if (response.status === 200) {
      return response.data;
    }
  } catch (error) {
    throw error;
  }
};
