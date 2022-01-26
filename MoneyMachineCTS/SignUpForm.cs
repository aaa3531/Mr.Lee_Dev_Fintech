using eBestAPI.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
    public partial class SignUpForm : DevExpress.XtraEditors.XtraForm
    {
        private List<CompanyModel> _listCompanyModel = new List<CompanyModel>();
        public SignUpForm()
        {
            InitializeComponent();
            InitializeControl();
        }

        private void InitializeControl()
        {
            try
            {
                string callUrl = "http://Moneymachinects-env-1.eba-49j29inb.ap-northeast-2.elasticbeanstalk.com/api/company?idx=1";
                //string callUrl = "http://localhost:63768/api/company?idx=1";

                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(callUrl); // 인코딩 UTF-8
                httpWebRequest.ContentType = "application/json; charset=UTF-8";
                httpWebRequest.Method = "GET";

                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.GetEncoding("UTF-8")))
                {
                    string responseString = streamReader.ReadToEnd();

                    var jsonArray = JArray.FromObject(JsonConvert.DeserializeObject(responseString));

                    _listCompanyModel = jsonArray.ToObject<List<CompanyModel>>();
                    foreach (var company in _listCompanyModel)
                    {
                        cbSubHeadquarters.Properties.Items.Add(company.name);
                    }
                    streamReader.Close();
                }

                httpWebResponse.Close();

                cbSubHeadquarters.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "오류");
            }
        }

        private void btnSignUp_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txID.Text))
                    throw new Exception("아이디를 입력해주세요.");

                if(string.IsNullOrEmpty(txPW.Text))
                    throw new Exception("비밀번호를 입력해주세요.");

                if (string.IsNullOrEmpty(txName.Text))
                    throw new Exception("이름을 입력해주세요.");

                if (string.IsNullOrEmpty(txEmail.Text))
                    throw new Exception("이메일을 입력해주세요.");

                if (string.IsNullOrEmpty(txPhone.Text))
                    throw new Exception("전화번호를 입력해주세요.");

                if (string.IsNullOrEmpty(txAddress.Text))
                    throw new Exception("주소를 입력해주세요.");

                if(cbSubHeadquarters.SelectedIndex < 0)
                    throw new Exception("부본사를 선택해주세요.");

                int companyidx = _listCompanyModel.Find(c => c.name == cbSubHeadquarters.Text).idx;
                if(companyidx == -1)
                    throw new Exception("부본사를 선택해주세요.");

                string callUrl = "http://Moneymachinects-env-1.eba-49j29inb.ap-northeast-2.elasticbeanstalk.com/api/registration";
                //string callUrl = "http://localhost:63768/api/registration";

                // 전송값
                JObject jobject = new JObject();
                jobject.Add("id", txID.Text);
                jobject.Add("pw", txPW.Text);
                jobject.Add("name", txName.Text);
                jobject.Add("email", txEmail.Text);
                jobject.Add("phone", txPhone.Text);
                jobject.Add("address", txAddress.Text);
                jobject.Add("signupdate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                jobject.Add("companyidx", companyidx);
                jobject.Add("expertidx", -1);
                string json = JsonConvert.SerializeObject(jobject);

                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(callUrl); // 인코딩 UTF-8
                byte[] sendData = UTF8Encoding.UTF8.GetBytes(json);
                httpWebRequest.ContentType = "application/json; charset=UTF-8";
                httpWebRequest.Method = "POST";
                httpWebRequest.ContentLength = sendData.Length;
                Stream requestStream = httpWebRequest.GetRequestStream();
                requestStream.Write(sendData, 0, sendData.Length);
                requestStream.Close();
                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.GetEncoding("UTF-8"));
                string read = streamReader.ReadToEnd();
                streamReader.Close();
                httpWebResponse.Close();
                if(read.IndexOf("false") < 0)
                {
                    MessageBox.Show("회원가입 완료");
                    this.DialogResult = DialogResult.OK;
                }
                else
                    throw new Exception("이미 존재하는 아이디 입니다.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "오류");
            }
        }
    }
}
