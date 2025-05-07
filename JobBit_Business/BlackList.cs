using JobBit_DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBit_Business
{
    public class BlackList
    {
        public enum enMode { AddNew = 0, Update = 1 };
        private enMode Mode = enMode.AddNew;
        public BlackListDTO blacklistDTO
        {
            get => new BlackListDTO(this.ID, this.Token, this.ExpiryDate);
        }
        public long ID { get; set; }
        public string Token { get; set; }
        public DateTime ExpiryDate { get; set; }

        public BlackList(BlackListDTO blacklistDTO, enMode CreationMode = enMode.AddNew)
        {
            this.ID = blacklistDTO.ID;
            this.Token = blacklistDTO.Token;
            this.ExpiryDate = blacklistDTO.ExpiryDate
            ;
            Mode = CreationMode;
        }

        public static BlackList Find(long ID)
        {

            BlackListDTO blacklistDTO = BlackListData.GetBlackListInfoByID(ID);

            if (blacklistDTO != null)
            {
                return new BlackList(blacklistDTO, enMode.Update);
            }
            return null;

        }

        private bool _AddNewBlackList()
        {
            this.ID = BlackListData.AddNewBlackList(this.blacklistDTO);
            return (this.ID != -1);
        }

        private bool _UpdateBlackList()
        {
            return BlackListData.UpdateBlackList(this.blacklistDTO);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewBlackList())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;
                case enMode.Update:
                    return _UpdateBlackList();
            }
            return false;
        }

        public static bool DeleteBlackList(long ID)
        {
            return BlackListData.DeleteBlackList(ID);
        }

        public static List<BlackListDTO> GetAllBlackList()
        {
            return BlackListData.GetAllBlackList();
        }

        public static bool IsBlackListExist(long ID)
        {
            return BlackListData.IsBlackListExistByID(ID);
        }

        public static async Task<bool> IsTokenExistAsync(string Token)
        {
            return await BlackListData.IsTokenExistAsync(Token);
        }

    }
}
