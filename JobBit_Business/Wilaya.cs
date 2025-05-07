using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobBit_DataAccess;

namespace JobBit_Business
{
    public class Wilaya
    {
        public enum enMode { AddNew = 0, Update = 1 };
        private enMode Mode = enMode.AddNew;
        public WilayaDTO wilayaDTO
        {
            get => new WilayaDTO(this.WilayaID, this.Name);
        }
        public int WilayaID { get; set; }
        public string Name { get; set; }

        public Wilaya(WilayaDTO wilayaDTO, enMode CreationMode = enMode.AddNew)
        {
            this.WilayaID = wilayaDTO.WilayaID;
            this.Name = wilayaDTO.Name
            ;
            Mode = CreationMode;
        }

        public static Wilaya Find(int WilayaID)
        {

            WilayaDTO wilayaDTO = WilayaData.GetWilayaInfoByID(WilayaID);

            if (wilayaDTO != null)
            {
                return new Wilaya(wilayaDTO, enMode.Update);
            }
            return null;

        }

        public static Wilaya Find(string WilayaName)
        {

            WilayaDTO wilayaDTO = WilayaData.GetWilayaInfoByName(WilayaName);

            if (wilayaDTO != null)
            {
                return new Wilaya(wilayaDTO, enMode.Update);
            }
            return null;

        }

        private bool _AddNewWilaya()
        {
            this.WilayaID = WilayaData.AddNewWilaya(this.wilayaDTO);
            return (this.WilayaID != -1);
        }

        private bool _UpdateWilaya()
        {
            return WilayaData.UpdateWilaya(this.wilayaDTO);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewWilaya())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;
                case enMode.Update:
                    return _UpdateWilaya();
            }
            return false;
        }

        public static bool DeleteWilaya(int WilayaID)
        {
            return WilayaData.DeleteWilaya(WilayaID);
        }

        public static List<WilayaDTO> GetAllWilayas()
        {
            return WilayaData.GetAllWilayas();
        }

        public static bool IsWilayaExist(int WilayaID)
        {
            return WilayaData.IsWilayaExistByID(WilayaID);
        }

        public static bool IsWilayaExist(string WilayaName)
        {
            return WilayaData.IsWilayaExistByName(WilayaName);
        }




    }
}
