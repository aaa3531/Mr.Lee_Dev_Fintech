using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MoneyMachineCTS
{
    public partial class ConfigForm : DevExpress.XtraEditors.XtraForm
    {
        public ConfigForm()
        {
            InitializeComponent();
            lbPaidDate.Text = UserData.user.paiddate.ToString("yyyy.MM.dd");
            labelControl3.Text = "";
            labelControl7.Text = "*회원매도 시 일괄매도와 분할매도가 모두 가능하나, \r\n현재는 금융법 방침에 따라 일괄매도만 진행하고 있습니다.";
            labelControl5.Text = "*최초가입후 처음 로그인을 하신 상태라면, \r\n반드시 안전하게 비밀번호 변경을 진행해 주시기 바랍니다.";

            Properties.Settings.Default.AutoBuy = false;
            Properties.Settings.Default.BulkSell = false;

            BuyState(Properties.Settings.Default.AutoBuy);
            SellState(Properties.Settings.Default.BulkSell);

            ceAutoLogin.Checked = Properties.Settings.Default.AutoLogin;
            ceEbestAutoLogin.Checked = Properties.Settings.Default.ebestLogin;
        }

        private void btnShowEbestLoginForm_Click(object sender, EventArgs e)
        {
            Login_eBestForm login_EBestForm = new Login_eBestForm();
            Properties.Settings.Default.ebestLogin = false;
            Properties.Settings.Default.Save();
            login_EBestForm.ShowDialog();
        }

        private void btnAuto_Click(object sender, EventArgs e)
        {
            BuyState(true);
            MainForm.GetInstance.ChangeControlMode();
        }

        private void btnManual_Click(object sender, EventArgs e)
        {
            BuyState(false);
            MainForm.GetInstance.ChangeControlMode();
        }

        void BuyState(bool auto)
        {
            if(auto)
            {
                //btnManual.Enabled = true;
                //btnAuto.Enabled = false;
                Properties.Settings.Default.AutoBuy = true;
                Properties.Settings.Default.Save();
            }
            else
            {
                //btnAuto.Enabled = true;
                //btnManual.Enabled = false;
                Properties.Settings.Default.AutoBuy = false;
                Properties.Settings.Default.Save();
            }
        }

        void SellState(bool bulk)
        {
            //btnBulk.Enabled = !bulk;
            //btnInstallment.Enabled = bulk;
            Properties.Settings.Default.BulkSell = bulk;
            Properties.Settings.Default.Save();
        }

        private void ceAutoLogin_CheckedChanged(object sender, EventArgs e)
        {
            if(ceAutoLogin.Checked)
            {
                Properties.Settings.Default.AutoLogin = true;
            }
            else
            {
                Properties.Settings.Default.AutoLogin = false;
            }
            Properties.Settings.Default.Save();
        }

        private void ceEbestAutoLogin_CheckedChanged(object sender, EventArgs e)
        {
            if (ceEbestAutoLogin.Checked)
            {
                Properties.Settings.Default.ebestLogin = true;
            }
            else
            {
                Properties.Settings.Default.ebestLogin = false;
            }
            Properties.Settings.Default.Save();
        }

        private void btnChangePW_Click(object sender, EventArgs e)
        {
            ConfigPasswordForm configPasswordForm = new ConfigPasswordForm();
            configPasswordForm.ShowDialog();
        }

        private void btnBulk_Click(object sender, EventArgs e)
        {
            //SellState(true);
        }

        private void btnInstallment_Click(object sender, EventArgs e)
        {
            //SellState(false);
        }
    }
}
