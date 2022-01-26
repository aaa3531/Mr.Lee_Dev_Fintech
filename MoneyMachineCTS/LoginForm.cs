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
using MoneyMachineCTS;
using XA_SESSIONLib;
using XA_DATASETLib;
using DevExpress.Utils.Extensions;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MoneyMachineCTS
{
    public partial class LoginForm : DevExpress.XtraEditors.XtraForm
    {

        public LoginForm()
        {
            InitializeComponent();

            txID.Text = Properties.Settings.Default.LoginID;
            txPW.Text = Properties.Settings.Default.LoginPW;
            ceAutoLogin.Checked = Properties.Settings.Default.AutoLogin;
        }

        void Login()
        {
            try
            {
                if (string.IsNullOrEmpty(txID.Text))
                    throw new Exception("아이디를 입력해주세요.");

                if (string.IsNullOrEmpty(txPW.Text))
                    throw new Exception("비밀번호를 입력해주세요.");

                Properties.Settings.Default.LoginID = txID.Text;
                Properties.Settings.Default.LoginPW = txPW.Text;
                Properties.Settings.Default.AutoLogin = ceAutoLogin.Checked;
                Properties.Settings.Default.Save();

                string callUrl = "http://Moneymachinects-env-1.eba-49j29inb.ap-northeast-2.elasticbeanstalk.com/api/user?idx=0&" + "id=" + txID.Text + "&" + "pw=" + txPW.Text;
                //string callUrl = "http://localhost:63768/api/user?idx=0&" + "id=" + txID.Text + "&" + "pw=" + txPW.Text;

                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(callUrl); // 인코딩 UTF-8
                httpWebRequest.ContentType = "application/json; charset=UTF-8";
                httpWebRequest.Method = "GET";

                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.GetEncoding("UTF-8"));
                string responseString = streamReader.ReadToEnd();

                var jsonArray = JArray.FromObject(JsonConvert.DeserializeObject(responseString));
                List<UserModel> listUserModel = jsonArray.ToObject<List<UserModel>>();
                streamReader.Close();
                httpWebResponse.Close();

                foreach (var userModel in listUserModel)
                {
                    UserData.user = userModel;
                }

                if (listUserModel.Count > 0)
                {
                    if(!UserData.user.access)
                        throw new Exception("가입승인이 나지 않았습니다.");

                    DateTime dt = Convert.ToDateTime(UserData.user.paiddate);

                    TimeSpan dateDiff = dt - DateTime.Now;

                    if(dateDiff.TotalSeconds < 0)
                    {
                        if (MessageBox.Show("사용기간이 만료된 계정 입니다.\r\n예를 누르시면 고객센터에 연결됩니다.", "오류", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            Properties.Settings.Default.AutoLogin = false;
                            Properties.Settings.Default.Save();
                            System.Diagnostics.Process.Start("https://helpsite.kr/");
                            Application.Restart();
                        }
                        else
                        {
                            Application.Exit();
                        }
                    }
                    else
                        this.DialogResult = DialogResult.OK;
                }
                else
                    throw new Exception("로그인 실패");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "오류");
            }
        }

        private void txID_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Login();
            }
        }

        private void txPW_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Login();
            }
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            if (ceAutoLogin.Checked && txID.Text != "" && txPW.Text != "")
            {
                Login();
            }
        }

        private void btnSignup_Click(object sender, EventArgs e)
        {
            SignUpForm signUpForm = new SignUpForm();

            signUpForm.ShowDialog();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            Login();
        }
    }
}