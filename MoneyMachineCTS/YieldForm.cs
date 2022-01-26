using eBestAPI.Common;
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
using XA_DATASETLib;

namespace MoneyMachineCTS
{
    public partial class YieldForm : DevExpress.XtraEditors.XtraForm
    {
        XAQueryClass CDPCQ04700 = new XAQueryClass();

        public YieldForm()
        {
            InitializeComponent();

            CDPCQ04700.ReceiveData += new _IXAQueryEvents_ReceiveDataEventHandler(CDPCQ04700_ReceiveData);
            CDPCQ04700.ReceiveMessage += new _IXAQueryEvents_ReceiveMessageEventHandler(CDPCQ04700_ReceiveMessage);

            File.WriteAllBytes(System.IO.Directory.GetCurrentDirectory() + "\\CDPCQ04700.res", Properties.Resources.CDPCQ04700);
            CDPCQ04700.ResFileName = System.IO.Directory.GetCurrentDirectory() + "\\CDPCQ04700.res";

            for (int i = 0; i < UserData.listAccount.Count; i++)
            {
                cbAccount.Properties.Items.Add(UserData.listAccount[i].계좌번호);
                cbAccount.SelectedItem = UserData.SelectedAccount.계좌번호;
            }

            deEndDate.DateTime = DateTime.Today;
            deStartDate.DateTime = DateTime.Today;

            InitializeProfit();
        }

        public void InitializeProfit()
        {
            try
            {
                lbEstimatedAssets.Text = string.Format("{0:n0}", MainForm._serverData.totalestimatedassets);
                lbAccumulatedProfit.Text = string.Format("{0:n0}", MainForm._serverData.profit);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "오류");
            }
        }

        void CDPCQ04700_ReceiveMessage(bool bIsSystemError, string nMessageCode, string szMessage)
        {
            if (bIsSystemError)
            {
                //MessageBox.Show(nMessageCode + " : " + szMessage, "알림");
            }
        }

        void CDPCQ04700_ReceiveData(string szTrCode)//주식잔고2
        {
            //종목명
            //추정실현손익
            //수익률
            //매도단가 
            //매도금액
            //매도수수료 
            //제세금
            //매입단가 
            //매수금액 
            //매수수수료

            List<거래내역> dataList = new List<거래내역>();
            int cnt = CDPCQ04700.GetBlockCount("CDPCQ04700OutBlock3");
            for (int i = 0; i < cnt; i++)
            {
                거래내역 data = new 거래내역();

                data.거래일자 = CDPCQ04700.GetFieldData("CDPCQ04700OutBlock3", "TrdDt", i);
                data.거래유형 = CDPCQ04700.GetFieldData("CDPCQ04700OutBlock3", "TpCodeNm", i);
                data.종목명 = CDPCQ04700.GetFieldData("CDPCQ04700OutBlock3", "IsuNm", i);
                data.거래수량 = Convert.ToInt32(CDPCQ04700.GetFieldData("CDPCQ04700OutBlock3", "TrdQty", i));
                data.거래단가 = Convert.ToDouble(CDPCQ04700.GetFieldData("CDPCQ04700OutBlock3", "TrdUprc", i));
                data.거래금액 = Convert.ToDouble(CDPCQ04700.GetFieldData("CDPCQ04700OutBlock3", "FcurrTrdAmt", i));
                data.외화수수료 = Convert.ToDouble(CDPCQ04700.GetFieldData("CDPCQ04700OutBlock3", "FcurrCmsnAmt", i));
                data.세금합계금액 = Convert.ToDouble(CDPCQ04700.GetFieldData("CDPCQ04700OutBlock3", "FcurrTaxSumAmt", i));
                dataList.Add(data);
            }

            List<거래내역> 매수리스트 = new List<거래내역>();
            List<거래내역> 매도리스트 = new List<거래내역>();

            for (int i = 0; i < dataList.Count; i++)
            {
                if (dataList[i].거래유형 == "매수")
                    매수리스트.Add(dataList[i]);
                else if (dataList[i].거래유형 == "매도")
                    매도리스트.Add(dataList[i]);
            }

            List<거래내역> 매수리스트2 = 매수리스트.GroupBy(l => l.종목명).Select(cl => new 거래내역
            {
                거래일자 = cl.First().거래일자,
                거래유형 = cl.First().거래유형,
                종목명 = cl.First().종목명,
                외화수수료 = cl.Sum(c => c.외화수수료),
                거래단가 = cl.Aggregate(0.0, (total, next) => (total + next.거래단가)),
                거래수량 = cl.Sum(c => c.거래수량),
                거래금액 = cl.Sum(c => c.거래금액),
                세금합계금액 = cl.Sum(c => c.세금합계금액),
            }).ToList();

            List<거래내역> 매도리스트2 = 매도리스트.GroupBy(l => l.종목명).Select(cl => new 거래내역
            {
                거래일자 = cl.First().거래일자,
                거래유형 = cl.First().거래유형,
                종목명 = cl.First().종목명,
                외화수수료 = cl.Sum(c => c.외화수수료),
                거래수량 = cl.Sum(c => c.거래수량),
                거래단가 = cl.Sum(c => c.거래단가),
                거래금액 = cl.Sum(c => c.거래금액),
                세금합계금액 = cl.Sum(c => c.세금합계금액),
            }).ToList();


            List<거래내역> result = new List<거래내역>();
            result.AddRange(매수리스트2);
            result.AddRange(매도리스트2);

            List<당일매도실현손익> 당일매도실현 = new List<당일매도실현손익>();
            for (int i = 0; i < 매수리스트2.Count; i++)
            {
                var 매수 = 매수리스트2[i];
                var 매도 = 매도리스트2.Find(I => I.종목명 == 매수.종목명);
                if(매도 != null)
                {
                    당일매도실현손익 손익 = new 당일매도실현손익();
                    손익.종목명 = 매도.종목명;
                    var 총매도금액 = 매도.거래금액 - 매도.외화수수료 - 매도.세금합계금액;
                    var 총매수금액 = 매수.거래금액 + 매수.외화수수료 + 매수.세금합계금액;
                    손익.추정실현손익 = 총매도금액 - 총매수금액;
                    손익.수익률 = Math.Round(손익.추정실현손익 / 매수.거래금액 * 100, 2);
                    손익.매도수량 = 매도.거래수량;
                    손익.매도단가 = (int)(매도.거래금액 / 매도.거래수량);
                    손익.매도금액 = (int)(매도.거래금액);
                    손익.매수금액 = (int)(매수.거래금액);
                    손익.매입단가 = (int)(매수.거래금액 / 매수.거래수량);
                    손익.매도수수료 = 매도.외화수수료;
                    손익.매수수수료 = 매수.외화수수료;
                    손익.제세금 = 매도.세금합계금액;
                    당일매도실현.Add(손익);
                }
            }

            gridControl2.DataSource = 당일매도실현.ToList();
            gridControl2.RefreshDataSource();
            //List<거래내역> 수익리스트 = result.GroupBy(l => l.종목명).Select(cl => new 거래내역
            //{
            //    거래일자 = cl.First().거래일자,
            //    거래유형 = "매수/매도",
            //    종목명 = cl.First().종목명,
            //    외화수수료 = cl.First(c => c.거래유형 == "매도").외화수수료 cl.Sum(c => c.외화수수료),
            //    거래단가 = cl.Sum(c => c.거래단가),
            //    거래수량 = cl.Sum(c => c.거래수량),
            //    거래금액 = cl.Sum(c => c.거래금액),
            //    세금합계금액 = cl.Sum(c => c.세금합계금액),
            //}).ToList();

            if (CDPCQ04700.IsNext)
            {

            }
            //gridControl1.DataSource = dataList.ToList();
            //gridControl1.RefreshDataSource();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (deStartDate.DateTime > deEndDate.DateTime)
            {
                MessageBox.Show("날짜를 확인해주세요.", "오류");
                return;
            }
            string startDate = deStartDate.DateTime.ToString("yyyyMMdd");
            string endDate = deEndDate.DateTime.ToString("yyyyMMdd");

            CDPCQ04700.SetFieldData("CDPCQ04700InBlock1", "RecCnt", 0, "1");
            CDPCQ04700.SetFieldData("CDPCQ04700InBlock1", "QryTp", 0, "3");
            CDPCQ04700.SetFieldData("CDPCQ04700InBlock1", "AcntNo", 0, cbAccount.SelectedItem.ToString());
            CDPCQ04700.SetFieldData("CDPCQ04700InBlock1", "Pwd", 0, Properties.Settings.Default.AccountPW);
            CDPCQ04700.SetFieldData("CDPCQ04700InBlock1", "QrySrtDt", 0, startDate);
            CDPCQ04700.SetFieldData("CDPCQ04700InBlock1", "QryEndDt", 0, endDate);
            CDPCQ04700.SetFieldData("CDPCQ04700InBlock1", "SrtNo", 0, " ");
            CDPCQ04700.SetFieldData("CDPCQ04700InBlock1", "PdptnCode", 0, " ");
            CDPCQ04700.SetFieldData("CDPCQ04700InBlock1", "IsuLgclssCode", 0, " ");
            CDPCQ04700.SetFieldData("CDPCQ04700InBlock1", "IsuNo", 0, " ");
            CDPCQ04700.Request(false);
        }
    }
}
