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
using System.Windows.Forms.VisualStyles;
using System.Threading;

namespace MoneyMachineCTS
{
    public partial class Login_eBestForm : DevExpress.XtraEditors.XtraForm
    {
        XASessionClass _XASessionClass = new XASessionClass();
        XAQueryClass _XAQueryClass = new XAQueryClass();

        public Login_eBestForm()
        {
            InitializeComponent();

            txID.Text = UserData.ebestID;
            txPW.Text = UserData.ebestPW;
            txCert.Text = UserData.ebestCert;

            txID.Text = Properties.Settings.Default.ID;
            txPW.Text = Properties.Settings.Default.PW;
            txCert.Text = Properties.Settings.Default.Cert;
            txAccPW.Text = Properties.Settings.Default.AccountPW;

            _XASessionClass._IXASessionEvents_Event_Login += new _IXASessionEvents_LoginEventHandler(_XASessionClass_Login);
            _XAQueryClass.ReceiveData += new _IXAQueryEvents_ReceiveDataEventHandler(myXAQueryClass_ReceiveData);
            _XAQueryClass.ReceiveMessage += new _IXAQueryEvents_ReceiveMessageEventHandler(myXAQueryClass_ReceiveMessage);
        }

        private void Login()
        {
            try
            {
                if (string.IsNullOrEmpty(txID.Text))
                    throw new Exception("아이디를 입력해주세요.");

                if (string.IsNullOrEmpty(txPW.Text))
                    throw new Exception("비밀번호를 입력해주세요.");

                if (string.IsNullOrEmpty(txCert.Text))
                    throw new Exception("공인인증서 비밀번호를 입력해주세요.");

                ///// 데모용
                //if (!_XASessionClass.ConnectServer("demo.etrade.co.kr", 20001)) //hts.etrade.co.kr / demo.etrade.co.kr
                //{
                //    UserData.IsAccountLogin = false;
                //    MessageBox.Show("서버에 접속할 수 없습니다.");
                //    return;
                //}

                /// 실서버용
                if (!_XASessionClass.ConnectServer("hts.etrade.co.kr", 20001)) //hts.etrade.co.kr / demo.etrade.co.kr
                {
                    UserData.IsAccountLogin = false;
                    MessageBox.Show("서버에 접속할 수 없습니다.");
                    return;
                }

                Thread.Sleep(1000);

                if (!_XASessionClass.Login(txID.Text, txPW.Text, txCert.Text, 0, false))
                {
                    UserData.IsAccountLogin = false;
                    MessageBox.Show("연결 실패");
                    return;
                }

                UserData.ebestID = txID.Text;
                UserData.ebestPW = txPW.Text;
                UserData.ebestCert = txCert.Text;

                Properties.Settings.Default.ID = txID.Text;
                Properties.Settings.Default.PW = txPW.Text;
                Properties.Settings.Default.Cert = txCert.Text;
                Properties.Settings.Default.AccountPW = txAccPW.Text;
                Properties.Settings.Default.ebestLogin = true;
                Properties.Settings.Default.Save();

                UserData.IsAccountLogin = true;
                
                if(!Properties.Settings.Default.ebestLogin)
                {
                    MessageBox.Show("연결 성공");
                }
                
                //()_XASessionClass += _XASessionClass_Login;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "오류");
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            //실서버 접속시 공인인증 비밀번호 필요.
            //이베스트 홈페이지에서 계정 API 사용등록을 해줘야함.
            //if (!_XASessionClass.ConnectServer("hts.etrade.co.kr", 20001))
            //{
            //    MessageBox.Show("서버에 접속할 수 없습니다.");
            //    return;
            //}

            Login();
        }

        void _XASessionClass_Login(string szCode, string szMsg)
        {
            if (szCode == "0000")
            {
                int accountCount = _XASessionClass.GetAccountListCount();

                //UserData.listAccount = new List<string>();

                for (int i = 0; i < accountCount; i++)
                {
                    var UserName = _XASessionClass.GetAccountName(_XASessionClass.GetAccountList(i));
                    var AccountName = _XASessionClass.GetAcctDetailName(_XASessionClass.GetAccountList(i));
                    if(AccountName.Contains("종합") || AccountName.Contains("위탁"))
                    {
                        //UserData.userName = UserName; //배포시 주석 해제
                        UserData.listAccount.Add(new AccountInfo(_XASessionClass.GetAccountList(i)));
                    }
                }

                Console.WriteLine("접속완료");
                this.DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show(szCode + " : " + szMsg);
            }
        }

        void myXAQueryClass_ReceiveMessage(bool bIsSystemError, string nMessageCode, string szMessage)
        {
            if (bIsSystemError)
            {
                MessageBox.Show(nMessageCode + " : " + szMessage, "알림");
            }
        }

        void myXAQueryClass_ReceiveData(string szTrCode)
        {
            string 계좌번호 = _XAQueryClass.GetFieldData("CACBQ21900OutBlock1", "AcntNo", 0);
            string 계좌명 = _XAQueryClass.GetFieldData("CACBQ21900OutBlock2", "AcntNm", 0);
            string 상품 = _XAQueryClass.GetFieldData("CACBQ21900OutBlock2", "PrdtDtlNm", 0);

            //foreach (DataGridViewRow row in dataGridView1.Rows)
            //{
            //    if (row.Cells["계좌번호"].Value.ToString() == 계좌번호)
            //    {
            //        row.Cells["계좌명"].Value = 계좌명;
            //        row.Cells["상품"].Value = 상품;
            //        break;
            //    }
            //}
        }

        private void txID_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
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

        private void txCert_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Login();
            }
        }

        private void Login_eBestForm_Load(object sender, EventArgs e)
        {
            if(txID.Text != "" && txPW.Text !="" && txCert.Text !="" && txAccPW.Text != "" && Properties.Settings.Default.ebestLogin)
            {
                Login();
            }
        }

        private void txAccPW_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                Login();
            }
        }
    }
}