using DevExpress.Utils.Behaviors;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using XA_DATASETLib;

namespace MoneyMachineCTS
{
    public partial class AccountBalanceForm : DevExpress.XtraEditors.XtraForm
    {

        public AccountBalanceForm()
        {
            InitializeComponent();

            //File.WriteAllBytes(System.IO.Directory.GetCurrentDirectory() + "\\t0424.res", Properties.Resources.t0424);
            //File.WriteAllBytes(System.IO.Directory.GetCurrentDirectory() + "\\CSPAQ22200.res", Properties.Resources.CSPAQ22200);

            //t0424.ResFileName = System.IO.Directory.GetCurrentDirectory() + "\\t0424.res";
            //t0424.ReceiveData += new _IXAQueryEvents_ReceiveDataEventHandler(t0424_ReceiveData);
            
            //CSPAQ22200.ResFileName = System.IO.Directory.GetCurrentDirectory() + "\\CSPAQ22200.res";
            //CSPAQ22200.ReceiveData += new _IXAQueryEvents_ReceiveDataEventHandler(CSPAQ22200_ReceiveData);

            for (int i = 0; i < UserData.listAccount.Count; i++)
            {
                cbAccountList.Properties.Items.Add(UserData.listAccount[i].계좌번호);
                cbAccountList.SelectedItem = UserData.SelectedAccount.계좌번호;
            }

            lbExpertName.Text = UserData.user.expertname;
            lbExpertName.Text = UserData.user.name;
            AddRepositoryButton();
            refreshTimer.Start();

        }

        private void cbAccountList_SelectedIndexChanged(object sender, EventArgs e)
        {
            //t0424.SetFieldData("t0424InBlock", "accno", 0, cbAccountList.SelectedItem.ToString());
            //t0424.SetFieldData("t0424InBlock", "passwd", 0, "");
            //t0424.Request(true);

            //CSPAQ22200.SetFieldData("CSPAQ22200InBlock1", "AcntNo", 0, cbAccountList.SelectedItem.ToString());
            //CSPAQ22200.SetFieldData("CSPAQ22200InBlock1", "Pwd", 0, "");
            //CSPAQ22200.Request(true);
        }

        //void t0424_ReceiveData(string szTrCode)//주식잔고2
        //{
        //    int cnt = t0424.GetBlockCount("t0424OutBlock");
        //    for (int i = 0; i < cnt; i++)
        //    {
        //        AccountInfo user = new AccountInfo();

        //        user.추정순자산 = int.Parse(t0424.GetFieldData("t0424OutBlock", "sunamt", i));
        //        user.매입금액 = int.Parse(t0424.GetFieldData("t0424OutBlock", "mamt", i));
        //        user.평가금액 = int.Parse(t0424.GetFieldData("t0424OutBlock", "tappamt", i));
        //        user.평가손익 = int.Parse(t0424.GetFieldData("t0424OutBlock", "tdtsunik", i));
        //        user.확정손익 = int.Parse(t0424.GetFieldData("t0424OutBlock", "dtsunik", i));

        //        lbTotalEstimatedAssets.Text = string.Format("{0:n0}", user.추정순자산);
        //        lbPurchaseAmount.Text = string.Format("{0:n0}", user.매입금액);
        //        lbEvaluationAmount.Text = string.Format("{0:n0}", user.평가금액);
        //        lbValuationGainOrLoss.Text = string.Format("{0:n0}", user.평가손익);
        //        if (user.평가손익 < 0)
        //        {
        //            lbValuationGainOrLoss.ForeColor = Color.Blue;
        //        }
        //        else
        //        {
        //            lbValuationGainOrLoss.ForeColor = Color.Red;
        //        }

        //        lbExtendedIncome.Text = string.Format("{0:n0}", user.확정손익);
        //        if (user.확정손익 < 0)
        //        {
        //            lbExtendedIncome.ForeColor = Color.Blue;
        //        }
        //        else
        //        {
        //            lbExtendedIncome.ForeColor = Color.Red;
        //        }

        //        UserData.listAccount.Add(user);
        //    }

        //    List<보유종목> dataList = new List<보유종목>();
        //    int cnt2 = t0424.GetBlockCount("t0424OutBlock1");
        //    for (int i = 0; i < cnt2; i++)
        //    {
        //        보유종목 data = new 보유종목();
        //        data.보유종목코드 = t0424.GetFieldData("t0424OutBlock1", "expcode", i);
        //        data.보유종목명 = t0424.GetFieldData("t0424OutBlock1", "hname", i);
        //        data.매수날짜 = "";
        //        data.잔고수량 = long.Parse(t0424.GetFieldData("t0424OutBlock1", "janqty", i));
        //        data.매입금액 = long.Parse(t0424.GetFieldData("t0424OutBlock1", "mamt", i));
        //        data.평가금액 = long.Parse(t0424.GetFieldData("t0424OutBlock1", "appamt", i));
        //        data.평가손익 = long.Parse(t0424.GetFieldData("t0424OutBlock1", "dtsunik", i));
        //        data.수익률 = float.Parse(t0424.GetFieldData("t0424OutBlock1", "sunikrt", i));
        //        data.매도신호시간 = "";
        //        data.Status = false;

        //        dataList.Add(data);
        //    }
        //    gridControl7.DataSource = dataList.ToList();
        //    gridControl7.RefreshDataSource();
            

        //    if (t0424.IsNext)
        //    {
        //    }
        //    else
        //    {
        //    }
        //}

        //void CSPAQ22200_ReceiveData(string szTrCode)//주식잔고2
        //{
        //    int cnt = CSPAQ22200.GetBlockCount("CSPAQ22200OutBlock2");
        //    for (int i = 0; i < cnt; i++)
        //    {
        //        long 현금주문가능금액 = long.Parse(CSPAQ22200.GetFieldData("CSPAQ22200OutBlock2", "MnyOrdAbleAmt", 0));
        //        lbMnyOrdAbleAmt.Text = string.Format("{0:n0}", 현금주문가능금액);
        //    }
             
        //    if (CSPAQ22200.IsNext)
        //    {
        //    }
        //    else
        //    {
        //    }
        //}

        public void AddRepositoryButton()
        {
            repositoryItemButtonEdit7.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            repositoryItemButtonEdit7.ButtonClick += edit_ButtonClick;
            repositoryItemButtonEdit7.Buttons[0].Caption = "매도";
            repositoryItemButtonEdit7.Buttons[0].Kind = DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph;
            //edit.Buttons[0].Appearance.BackColor = Color.Black;
            gridView7.Columns["Status"].ColumnEdit = repositoryItemButtonEdit7;
            gridView7.Columns["Status"].AppearanceCell.BackColor = Color.FromArgb(65, 108, 183);
            gridView7.Appearance.FocusedRow.BackColor = Color.Transparent;
            gridView7.Appearance.FocusedCell.BackColor = Color.FromArgb(65, 108, 183);
            gridView7.Columns["Status"].AppearanceCell.ForeColor = Color.White;
        }

        void edit_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                var code = gridView7.GetRowCellValue(gridView7.GetFocusedDataSourceRowIndex(), "보유종목코드");
                var 잔고수량 = gridView7.GetRowCellValue(gridView7.GetFocusedDataSourceRowIndex(), "잔고수량");
                BuySellCheckForm buySellCheckForm = new BuySellCheckForm("매도", code.ToString(), 잔고수량.ToString());
                buySellCheckForm.ShowDialog();
            }
            catch
            {
                MessageBox.Show("매도 실패! 고객센터에 문의바랍니다.");
            }
        }

        private void RefreshTimer_Tick(object sender, EventArgs e)
        {
            lbTotalEstimatedAssets.Text = string.Format("{0:n0}", UserData.SelectedAccount.추정순자산);
            lbPurchaseAmount.Text = string.Format("{0:n0}", UserData.SelectedAccount.매입금액);
            lbEvaluationAmount.Text = string.Format("{0:n0}", UserData.SelectedAccount.평가금액);
            lbValuationGainOrLoss.Text = string.Format("{0:n0}", UserData.SelectedAccount.평가손익);
            lbExtendedIncome.Text = string.Format("{0:n0}", UserData.SelectedAccount.확정손익);
            gridControl7.DataSource = MainForm._listHoldings.ToList();
            gridControl7.RefreshDataSource();
        }

        private void AccountBalanceForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            refreshTimer.Stop();
        }
    }
}
