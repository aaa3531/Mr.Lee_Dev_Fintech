using eBestAPI.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using XA_DATASETLib;

namespace MoneyMachineCTS
{
    public partial class BuySellForm : DevExpress.XtraEditors.XtraForm
    {
        XAQueryClass CSPAT00600 = new XAQueryClass();

        public BuySellForm()
        {
            InitializeComponent();
            
            File.WriteAllBytes(System.IO.Directory.GetCurrentDirectory() + "\\CSPAT00600.res", Properties.Resources.CSPAT00600);
            CSPAT00600.ResFileName = System.IO.Directory.GetCurrentDirectory() + "\\CSPAT00600.res";
            CSPAT00600.ReceiveData += new _IXAQueryEvents_ReceiveDataEventHandler(CSPAT00600_ReceiveData);
            CSPAT00600.ReceiveMessage += new _IXAQueryEvents_ReceiveMessageEventHandler(CSPAT00600_ReceiveMessage);

            System.Windows.Forms.Timer t = new System.Windows.Forms.Timer();
            t.Tick += new EventHandler(timeTimer_Tick);
            t.Start();

            AddRepositoryButton();
        }

        public void AddRepositoryButton()
        {
            repositoryItemButtonEdit7.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            repositoryItemButtonEdit7.ButtonPressed += edit_SellButtonClick;
            repositoryItemButtonEdit7.Buttons[0].Caption = "매도";
            repositoryItemButtonEdit7.Buttons[0].Kind = DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph;
            //edit.Buttons[0].Appearance.BackColor = Color.Black;
            gridView7.Columns["Status"].ColumnEdit = repositoryItemButtonEdit7;
            gridView7.Columns["Status"].AppearanceCell.BackColor = Color.FromArgb(65, 108, 183);
            gridView7.Appearance.FocusedRow.BackColor = Color.Transparent;
            gridView7.Appearance.FocusedCell.BackColor = Color.FromArgb(65, 108, 183);
            gridView7.Columns["Status"].AppearanceCell.ForeColor = Color.White;

            repositoryItemButtonEdit1.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            repositoryItemButtonEdit1.ButtonPressed += edit_BuyButtonClick;
            repositoryItemButtonEdit1.Buttons[0].Caption = "매수";
            repositoryItemButtonEdit1.Buttons[0].Kind = DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph;
            //edit.Buttons[0].Appearance.BackColor = Color.Black;
            gridView2.Columns["Status"].ColumnEdit = repositoryItemButtonEdit1;
            gridView2.Columns["Status"].AppearanceCell.BackColor = Color.FromArgb(170, 57, 57);
            gridView2.Appearance.FocusedRow.BackColor = Color.Transparent;
            gridView2.Appearance.FocusedCell.BackColor = Color.FromArgb(170, 57, 57);
            gridView2.Columns["Status"].AppearanceCell.ForeColor = Color.White;
        }

        void edit_SellButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                var code = gridView7.GetFocusedRowCellValue("보유종목코드");
                var 잔고수량 = gridView7.GetRowCellValue(gridView7.GetFocusedDataSourceRowIndex(), "잔고수량");
                BuySellCheckForm buySellCheckForm = new BuySellCheckForm("매도", code.ToString(), 잔고수량.ToString());
                buySellCheckForm.ShowDialog();
            }
            catch
            {
                MessageBox.Show("매도 실패! 고객센터에 문의바랍니다.");
            }
        }

        void edit_BuyButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                var code = gridView2.GetRowCellValue(gridView2.GetFocusedDataSourceRowIndex(), "expcode");
                OrderModel orderModel = new OrderModel();
                orderModel.expcode = code.ToString();
                orderModel.ordertype = "매수";
                BuySellCheckForm buySellCheckForm = new BuySellCheckForm(orderModel, "0");
                buySellCheckForm.ShowDialog();
            }
            catch
            {
                MessageBox.Show("매수 실패! 고객센터에 문의바랍니다.");
            }
        }

        private void RefreshData()
        {
            if(this.InvokeRequired)
            {
                RefreshData();
            }
            else
            {
                int rowIndex = gridView2.TopRowIndex;
                int focusRow = gridView2.FocusedRowHandle;
                gridControl2.BeginUpdate();

                gridControl2.DataSource = MainForm._listBuyOrderModel.ToList();
                gridControl2.RefreshDataSource();

                gridView2.FocusedRowHandle = focusRow;
                gridView2.TopRowIndex = rowIndex;
                gridControl2.EndUpdate();

                ///
                rowIndex = gridView7.TopRowIndex;
                focusRow = gridView7.FocusedRowHandle;
                gridControl7.BeginUpdate();

                gridControl7.DataSource = MainForm._listHoldings.ToList();
                gridControl7.RefreshDataSource();

                gridView7.FocusedRowHandle = focusRow;
                gridView7.TopRowIndex = rowIndex;
                gridControl7.EndUpdate();
                ///

                ///
                rowIndex = gridView8.TopRowIndex;
                focusRow = gridView8.FocusedRowHandle;
                gridControl8.BeginUpdate();

                gridControl8.DataSource = MainForm._listSignedModel.ToList();
                gridControl8.RefreshDataSource();

                gridView8.FocusedRowHandle = focusRow;
                gridView8.TopRowIndex = rowIndex;
                gridControl8.EndUpdate();
            }
        }

        private void timeTimer_Tick(object sender, EventArgs e)
        {
            lbCurrentTime.Text = System.DateTime.Now.ToString("yy.MM.dd hh:mm:ss");

            RefreshData();

            //gridControl2.DataSource = MainForm._listBuyOrderModel.ToList();
            //gridControl7.DataSource = MainForm._listHoldings.ToList();
            //gridControl8.DataSource = MainForm._listSignedModel.ToList();
            //gridControl8.RefreshDataSource();
            //gridControl2.RefreshDataSource();
            //gridControl7.RefreshDataSource();
        }

        private void btnAllSell_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("전체 일괄 매도합니다.", "매도", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    for (int i = 0; i < gridView7.RowCount; i++)
                    {
                        string 보유종목코드 = (string)gridView7.GetRowCellValue(i, "보유종목코드");
                        string 잔고수량 = gridView7.GetRowCellValue(i, "잔고수량").ToString();
                        AllSell(보유종목코드, 잔고수량);
                    }
                }
            }
            catch
            {
                MessageBox.Show("전체일괄매도 실패! 고객센터에 문의바랍니다.");
            }
        }

        void CSPAT00600_ReceiveMessage(bool bIsSystemError, string nMessageCode, string szMessage)
        {
            if (bIsSystemError)
            {
                MessageBox.Show(nMessageCode + " : " + szMessage, "알림");
            }
            if (nMessageCode == "02218")
            {
                MessageBox.Show(nMessageCode + " : " + szMessage, "알림");
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
            }
            else
            {
            }
        }

        void AllSell(string code, string count)
        {
            CSPAT00600.SetFieldData("CSPAT00600InBlock1", "AcntNo", 0, UserData.SelectedAccount.계좌번호);
            CSPAT00600.SetFieldData("CSPAT00600InBlock1", "InptPwd", 0, Properties.Settings.Default.AccountPW);
            CSPAT00600.SetFieldData("CSPAT00600InBlock1", "IsuNo", 0, code);
            CSPAT00600.SetFieldData("CSPAT00600InBlock1", "OrdQty", 0, count);
            CSPAT00600.SetFieldData("CSPAT00600InBlock1", "OrdprcPtnCode", 0, "03"); // 00 지정가 03 시장가

            //시장가일때 상한가 입력인지 공백입력인지 확인필요
            CSPAT00600.SetFieldData("CSPAT00600InBlock1", "OrdPrc", 0, "");
            //CSPAT00600.SetFieldData("CSPAT00600InBlock1", "OrdPrc", 0, _upLimitPrice);
            CSPAT00600.SetFieldData("CSPAT00600InBlock1", "BnsTpCode", 0, "1"); //1 매도

            CSPAT00600.SetFieldData("CSPAT00600InBlock1", "MgntrnCode", 0, "000"); //신용거래코드 : 000보통
            CSPAT00600.SetFieldData("CSPAT00600InBlock1", "OrdCndiTpCode", 0, "0"); // 0 : 없음 , 1 : IOC , 2 : FOK

            CSPAT00600.Request(true);
        }
    }
}

