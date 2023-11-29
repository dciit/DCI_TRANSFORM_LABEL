using System.Windows.Forms;

namespace DCI_TRANSFER_SERIAR
{
    internal class MPanelShowDetail
    {
        private TextBox sErial = new TextBox();
        private TextBox mOdelCode = new TextBox();
        private TextBox mOdelName = new TextBox();
        private TextBox lIne = new TextBox();

        public TextBox SErial { get => sErial; set => sErial = value; }
        public TextBox MOdelCode { get => mOdelCode; set => mOdelCode = value; }
        public TextBox MOdelName { get => mOdelName; set => mOdelName = value; }
        public TextBox LIne { get => lIne; set => lIne = value; }
    }
}