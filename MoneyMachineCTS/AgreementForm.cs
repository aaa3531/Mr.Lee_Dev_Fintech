using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace MoneyMachineCTS
{
    public partial class AgreementForm : DevExpress.XtraEditors.XtraForm
    {
        public AgreementForm()
        {
            InitializeComponent();

            lbText.Text = "최로 로그인 시 '주식인 다이아' 사용 동의로 간주하며, \r\n추후 자동 로그인을 통해 바로\r\n매매화면으로 넘어갈 수 있습니다.";
        }

        private void btnSignUp_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Agreemnet = true;
            Properties.Settings.Default.Save();

            Agree();
        }

        void Agree()
        {
            Login_eBestForm login_EBestForm = new Login_eBestForm();

            if (login_EBestForm.ShowDialog() == DialogResult.OK)
            {
                this.DialogResult = DialogResult.OK;
            }
        }

        private void AgreementForm_Load(object sender, EventArgs e)
        {
            if(Properties.Settings.Default.Agreemnet)
            {
               Agree();
            }
        }
    }
}