using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCI_TRANSFER_SERIAR.Models
{
    internal class MSummaryTransfer
    {

        private string modelCodeOld = "";
        private string modelNameOld = "";
        
        private string modelCodeNew = "";
        private string modelNameNew = "";
        private int transferQty = 0;

        public string ModelCodeOld { get => modelCodeOld; set => modelCodeOld = value; }
        public string ModelNameOld { get => modelNameOld; set => modelNameOld = value; }
        public string ModelCodeNew { get => modelCodeNew; set => modelCodeNew = value; }
        public string ModelNameNew { get => modelNameNew; set => modelNameNew = value; }
        public int TransferQty { get => transferQty; set => transferQty = value; }
    }
}
