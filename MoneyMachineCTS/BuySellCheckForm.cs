using eBestAPI.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XA_DATASETLib;

namespace MoneyMachineCTS
{
    public partial class BuySellCheckForm : DevExpress.XtraEditors.XtraForm
    {
        enum TYPE
        {
            NULL,
            BUY,
            SELL,
        }

        XAQueryClass CSPAT00600 = new XAQueryClass();
        XAQueryClass t1102 = new XAQueryClass();

        TYPE _type = TYPE.NULL;
        int _orderCount = 0;

        public BuySellCheckForm(OrderModel orderModel, string 수량)
        {
            InitializeComponent();

            tbCount.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            tbCount.Properties.Mask.EditMask = "n0";
            tbCount.Properties.Mask.UseMaskAsDisplayFormat = true;

            File.WriteAllBytes(System.IO.Directory.GetCurrentDirectory() + "\\CSPAT00600.res", Properties.Resources.CSPAT00600);
            CSPAT00600.ResFileName = System.IO.Directory.GetCurrentDirectory() + "\\CSPAT00600.res";
            CSPAT00600.ReceiveData += new _IXAQueryEvents_ReceiveDataEventHandler(CSPAT00600_ReceiveData);
            CSPAT00600.ReceiveMessage += new _IXAQueryEvents_ReceiveMessageEventHandler(CSPAT00600_ReceiveMessage);


            File.WriteAllBytes(System.IO.Directory.GetCurrentDirectory() + "\\t1102.res", Properties.Resources.t1102);
            t1102.ResFileName = System.IO.Directory.GetCurrentDirectory() + "\\t1102.res";
            t1102.ReceiveData += new _IXAQueryEvents_ReceiveDataEventHandler(t1102_ReceiveData);
            t1102.ReceiveMessage += new _IXAQueryEvents_ReceiveMessageEventHandler(t1102_ReceiveMessage);

            t1102.SetFieldData("t1102InBlock", "shcode", 0, orderModel.expcode);
            t1102.Request(true);

            if (orderModel.ordertype == "매수")
            {
                _type = TYPE.BUY;
                this.Text = "매수 주문하기";
                lbBuySell.Text = "매수";
                lbBuySell.ForeColor = Color.White;
                lbBuySell.BackColor = Color.Red;
            }
            else if (orderModel.ordertype == "매도")
            {
                _type = TYPE.SELL;
                this.Text = "매도하기";
                lbBuySell.Text = "매도";
                lbBuySell.ForeColor = Color.White;
                lbBuySell.BackColor = Color.Blue;
                
            }

            lbAccount.Text = UserData.SelectedAccount.계좌번호;
            lbHCode.Text = orderModel.expcode;
            tbCount.Text = 수량;
        }

        public BuySellCheckForm(string type, string code, string 잔고)
        {
            InitializeComponent();

            tbCount.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            tbCount.Properties.Mask.EditMask = "n0";
            tbCount.Properties.Mask.UseMaskAsDisplayFormat = true;

            File.WriteAllBytes(System.IO.Directory.GetCurrentDirectory() + "\\CSPAT00600.res", Properties.Resources.CSPAT00600);
            CSPAT00600.ResFileName = System.IO.Directory.GetCurrentDirectory() + "\\CSPAT00600.res";
            CSPAT00600.ReceiveData += new _IXAQueryEvents_ReceiveDataEventHandler(CSPAT00600_ReceiveData);
            CSPAT00600.ReceiveMessage += new _IXAQueryEvents_ReceiveMessageEventHandler(CSPAT00600_ReceiveMessage);


            File.WriteAllBytes(System.IO.Directory.GetCurrentDirectory() + "\\t1102.res", Properties.Resources.t1102);
            t1102.ResFileName = System.IO.Directory.GetCurrentDirectory() + "\\t1102.res";
            t1102.ReceiveData += new _IXAQueryEvents_ReceiveDataEventHandler(t1102_ReceiveData);
            t1102.ReceiveMessage += new _IXAQueryEvents_ReceiveMessageEventHandler(t1102_ReceiveMessage);

            t1102.SetFieldData("t1102InBlock", "shcode", 0, code);
            t1102.Request(true);

            //if (type == "매수")
            //{
            //    _type = TYPE.BUY;
            //    this.Text = "매수 주문하기";
            //    lbBuySell.Text = "매수";
            //    lbBuySell.ForeColor = Color.White;
            //    lbBuySell.BackColor = Color.Red;
            //}
            //else if (type == "매도")
            //{
            //    _type = TYPE.SELL;
            //    this.Text = "매도하기";
            //    lbBuySell.Text = "매도";
            //    lbBuySell.ForeColor = Color.White;
            //    lbBuySell.BackColor = Color.Blue;
            //    tbCount.Text = 잔고;
            //    tbCount.Enabled = false;
            //    _orderCount = int.Parse(잔고);
            //}

            if (type == "매도")
            {
                _type = TYPE.SELL;
                this.Text = "매도하기";
                lbBuySell.Text = "매도";
                lbBuySell.ForeColor = Color.White;
                lbBuySell.BackColor = Color.Blue;
                tbCount.Text = 잔고;
                tbCount.Enabled = false;
                _orderCount = int.Parse(잔고);
            }

            lbAccount.Text = UserData.SelectedAccount.계좌번호;
            lbHCode.Text = code;
        }

        void t1102_ReceiveMessage(bool bIsSystemError, string nMessageCode, string szMessage)
        {
            if (bIsSystemError)
            {
                MessageBox.Show(nMessageCode + " : " + szMessage, "알림");
            }
        }

        string _upLimitPrice;
        void t1102_ReceiveData(string szTrCode)
        {
            int cnt = t1102.GetBlockCount("t1102OutBlock");
            for (int i = 0; i < cnt; i++)
            {
                lbHName.Text = t1102.GetFieldData("t1102OutBlock", "hname", 0);
                _upLimitPrice = t1102.GetFieldData("t1102OutBlock", "uplmtprice", 0);
            }
        }

        void CSPAT00600_ReceiveMessage(bool bIsSystemError, string nMessageCode, string szMessage)
        {
            if (bIsSystemError)
            {
                MessageBox.Show(nMessageCode + " : " + szMessage, "알림");
            }
            else
            {
                if (nMessageCode == "02218")
                {
                    MessageBox.Show(nMessageCode + " : " + szMessage, "알림");
                }
                else
                {
                    MessageBox.Show(nMessageCode + " : " + szMessage, "알림");
                }
            }
        }

        void CSPAT00600_ReceiveData(string szTrCode)//주식잔고2
        {
            int cnt = CSPAT00600.GetBlockCount("CSPAQ22200OutBlock2");
            for (int i = 0; i < cnt; i++)
            {
                long 현금주문가능금액 = long.Parse(CSPAT00600.GetFieldData("CSPAQ22200OutBlock2", "MnyOrdAbleAmt", 0));
            }

            if (CSPAT00600.IsNext)
            {
                this.DialogResult = DialogResult.OK;
            }
            else
            {
            }
        }

        private void btnOrder_Click(object sender, EventArgs e)
        {
            CSPAT00600.SetFieldData("CSPAT00600InBlock1", "AcntNo", 0, UserData.SelectedAccount.계좌번호);
            CSPAT00600.SetFieldData("CSPAT00600InBlock1", "InptPwd", 0, Properties.Settings.Default.AccountPW);
            CSPAT00600.SetFieldData("CSPAT00600InBlock1", "IsuNo", 0, lbHCode.Text);
            CSPAT00600.SetFieldData("CSPAT00600InBlock1", "OrdQty", 0, tbCount.Text);
            CSPAT00600.SetFieldData("CSPAT00600InBlock1", "OrdprcPtnCode", 0, "03"); // 00 지정가 03 시장가

            switch (_type)
            {
                case TYPE.BUY:
                    {
                        //시장가 매수일때 상한가 입력인지 공백입력인지 확인필요
                        CSPAT00600.SetFieldData("CSPAT00600InBlock1", "OrdPrc", 0, " ");
                        //CSPAT00600.SetFieldData("CSPAT00600InBlock1", "OrdPrc", 0, _upLimitPrice);
                        CSPAT00600.SetFieldData("CSPAT00600InBlock1", "BnsTpCode", 0, "2"); // 2 매수
                    }
                    break;
                case TYPE.SELL:
                    {
                        //시장가 매수일때 상한가 입력인지 공백입력인지 확인필요
                        CSPAT00600.SetFieldData("CSPAT00600InBlock1", "OrdPrc", 0, " ");
                        //CSPAT00600.SetFieldData("CSPAT00600InBlock1", "OrdPrc", 0, _upLimitPrice);
                        CSPAT00600.SetFieldData("CSPAT00600InBlock1", "BnsTpCode", 0, "1"); //1 매도
                    }
                    break;
            }

            CSPAT00600.SetFieldData("CSPAT00600InBlock1", "MgntrnCode", 0, "000"); //신용거래코드 : 000보통
            CSPAT00600.SetFieldData("CSPAT00600InBlock1", "OrdCndiTpCode", 0, "0"); // 0 : 없음 , 1 : IOC , 2 : FOK

            CSPAT00600.Request(true);
        }
    }
}
