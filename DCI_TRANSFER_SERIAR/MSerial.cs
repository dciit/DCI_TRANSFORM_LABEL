using System;
using System.Collections.Generic;

namespace DCI_TRANSFER_SERIAR
{
    internal class MSerial
    {
        private string rEmark = "";
        private string eMCode = "";
        private string mOdelCodeOld = "";
        private string mOdelNameOld = "";
        private string lIneOld = "";

        private string mOdelCodeNew = "";
        private string mOdelNameNew = "";
        private string lIneNew = "";

        private string sErial = "";
        private string mOdelCode = "";
        private string mOdelName = "";
        private string lIne = "";

        private int aDj_id = 0;
        private string sErial_old = "";
        private string sErial_new = "";
        private string aDj_status = "";
        private List<string> eXcelRemark = new List<string>();
        private DateTime uPdate_dt = new DateTime();

     
      
        public int ADj_id { get => aDj_id; set => aDj_id = value; }
        public string SErial_old { get => sErial_old; set => sErial_old = value; }
        public string SErial_new { get => sErial_new; set => sErial_new = value; }
        public DateTime UPdate_dt { get => uPdate_dt; set => uPdate_dt = value; }
        public string MOdelCodeOld { get => mOdelCodeOld; set => mOdelCodeOld = value; }
        public string MOdelNameOld { get => mOdelNameOld; set => mOdelNameOld = value; }
        public string LIneOld { get => lIneOld; set => lIneOld = value; }
        public string MOdelCodeNew { get => mOdelCodeNew; set => mOdelCodeNew = value; }
        public string MOdelNameNew { get => mOdelNameNew; set => mOdelNameNew = value; }
        public string LIneNew { get => lIneNew; set => lIneNew = value; }
        public string SErial { get => sErial; set => sErial = value; }
        public string MOdelCode { get => mOdelCode; set => mOdelCode = value; }
        public string MOdelName { get => mOdelName; set => mOdelName = value; }
        public string LIne { get => lIne; set => lIne = value; }
        public string EMCode { get => eMCode; set => eMCode = value; }
        public string ADj_status { get => aDj_status; set => aDj_status = value; }
        public List<string> EXcelRemark { get => eXcelRemark; set => eXcelRemark = value; }
        public string REmark { get => rEmark; set => rEmark = value; }
    }
}