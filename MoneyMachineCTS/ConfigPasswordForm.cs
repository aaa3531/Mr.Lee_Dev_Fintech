using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MoneyMachineCTS
{
    public partial class ConfigPasswordForm : DevExpress.XtraEditors.XtraForm
    {
        public ConfigPasswordForm()
        {
            InitializeComponent();
        }

        private void btnPWSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(tbCurrentPW.Text))
                    throw new Exception("현재 비밀번호를 입력해주세요.");

                if (string.IsNullOrEmpty(tbNewPW.Text))
                    throw new Exception("새 비밀번호를 입력해주세요.");

                if (string.IsNullOrEmpty(tbNewPWCheck.Text))
                    throw new Exception("비밀번호 확인을 입력해주세요.");

                if(tbCurrentPW.Text != UserData.user.pw || tbNewPW.Text != tbNewPWCheck.Text)
                    throw new Exception("비밀번호를 확인해주세요.");


                UserData.user.pw = tbNewPW.Text;

                string callUrl = "http://Moneymachinects-env-1.eba-49j29inb.ap-northeast-2.elasticbeanstalk.com/api/user";
                //string callUrl = "http://localhost:63768/api/user";

                string json = JsonConvert.SerializeObject(UserData.user);

                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(callUrl); // 인코딩 UTF-8
                byte[] sendData = UTF8Encoding.UTF8.GetBytes(json);
                httpWebRequest.ContentType = "application/json; charset=UTF-8";
                httpWebRequest.Method = "PUT";
                httpWebRequest.ContentLength = sendData.Length;
                Stream requestStream = httpWebRequest.GetRequestStream();
                requestStream.Write(sendData, 0, sendData.Length);
                requestStream.Close();
                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.GetEncoding("UTF-8"));
                string read = streamReader.ReadToEnd();
                streamReader.Close();
                httpWebResponse.Close();

                MessageBox.Show("비밀번호가 변경되었습니다.");

                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "오류");
            }
        }
    }
}
