using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobBit_DataAccess;
using static JobBit_DataAccess.UserData;

namespace JobBit_Business
{
    public class User
    {
        public enum enUserType { JobSeeker,Company};
        public enum enUserPolicy { JobSeekerPolicy, CompanyPolicy };
        public enum enMode { AddNew = 0, Update = 1 };
        protected enMode Mode = enMode.AddNew;
        public UserDTO userDTO
        {
            get => new UserDTO(this.UserID, this.Email, this.Password, this.Phone, this.IsActive);
        }
        public int UserID { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; }

        public User(UserDTO userDTO, enMode CreationMode = enMode.AddNew)
        {
            this.UserID = userDTO.UserID;
            this.Email = userDTO.Email;
            this.Password = userDTO.Password;
            this.Phone = userDTO.Phone;
            this.IsActive = userDTO.IsActive
            ;
            Mode = CreationMode;
        }

        public static User FindBaseUser(int UserID)
        {

            UserDTO userDTO = UserData.GetUserInfoByID(UserID);

            if (userDTO != null)
            {
                return new User(userDTO, enMode.Update);
            }
            return null;

        }
        public static User FindBaseUser(string Email,string Password)
        {

            UserDTO userDTO = UserData.GetUserInfoByEmailAndPassword(Email, Password);

            if (userDTO != null)
            {
                return new User(userDTO, enMode.Update);
            }
            return null;

        }

        private bool _AddNewUser()
        {
            this.UserID = UserData.AddNewUser(this.userDTO);
            return (this.UserID != -1);
        }

        private bool _UpdateUser()
        {
            return UserData.UpdateUser(this.userDTO);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewUser())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;
                case enMode.Update:
                    return _UpdateUser();
            }
            return false;
        }

        protected bool DeleteUser()
        {
            return UserData.DeleteUser(this.UserID);
        }

        public static List<UserDTOWithoutPassword> GetAllUsers()
        {
            return UserData.GetAllUsers();
        }

        public static bool IsUserExistByID(int UserID)
        {
            return UserData.IsUserExistByID(UserID);
        }
        public static bool IsUserExistByEmail(string Email)
        {
            return UserData.IsUserExistByEmail(Email);
        }

        public static bool IsUserExistByPhone(string Phone)
        {
            return UserData.IsUserExistByPhone(Phone);
        }

        public static bool IsUserExistByEmailAndPassword(string Email,string Password)
        {
            return UserData.IsUserExistByEmailAndPassword(Email,Password);
        }


    }
}
