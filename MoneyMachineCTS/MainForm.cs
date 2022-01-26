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
using System.Reflection;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using MoneyMachineCTS.Properties;
using XA_SESSIONLib;
using XA_DATASETLib;
using Newtonsoft.Json.Linq;
using System.Threading;
using DevExpress.Utils;
using eBestAPI.Common;

namespace MoneyMachineCTS
{
    public partial class MainForm : DevExpress.XtraEditors.XtraForm
    {
        public static List<Stock> _listStocks = new List<Stock>();

        public static List<OrderModel> _listBuyOrderModel = new List<OrderModel>();
        public static List<OrderModel> _listSellOrderModel = new List<OrderModel>();
        public static List<OrderModel> _listOrderModel = new List<OrderModel>();

        public static List<체결> _listSignedModel = new List<체결>();
        public List<Order> _listAutoOrder = new List<Order>();
        public List<Order> _listManualOrder = new List<Order>();
        public static RatioModel _ratioModel = new RatioModel();

        XASessionClass _XASessionClass = new XASessionClass();
        XAQueryClass _XAQueryClass = new XAQueryClass();
        XAQueryClass t0425 = new XAQueryClass();
        XAQueryClass t0424 = new XAQueryClass();
        XAQueryClass t1102 = new XAQueryClass();
        XAQueryClass t0167 = new XAQueryClass();
        XAQueryClass t8430 = new XAQueryClass();
        XAQueryClass CSPAT00800 = new XAQueryClass();
        XAQueryClass CSPAT00600 = new XAQueryClass();
        XARealClass s3 = new XARealClass();

        public static ServerData _serverData = new ServerData();
        public static List<Holding> _listHoldings = new List<Holding>();

        string 연속key_t0425 = "";
        string 연속key_t0424 = "";

        Image statusImage;
        static MainForm mainForm;

        public static MainForm GetInstance
        {
            get
            {
                return mainForm;
            }
        }        
        public bool _IsAutoControl { get; set; }
        public bool _IsRefresh { get; set; }
        public bool _IsRun { get; set; }
        public MainForm()
        {
            InitializeComponent();

            mainForm = this;
            gridControl2.DataSource = _listOrderModel.ToList();
            gridControl2.RefreshDataSource();

            //gridControl3.DataSource = _listManualOrder.ToList();
            //gridControl2.RefreshDataSource();

            File.WriteAllBytes(System.IO.Directory.GetCurrentDirectory() + "\\t0424.res", Properties.Resources.t0424);
            t0424.ResFileName = System.IO.Directory.GetCurrentDirectory() + "\\t0424.res";
            t0424.ReceiveData += new _IXAQueryEvents_ReceiveDataEventHandler(t0424_ReceiveData);
            t0424.ReceiveMessage += new _IXAQueryEvents_ReceiveMessageEventHandler(t0424_ReceiveMessage);

            File.WriteAllBytes(System.IO.Directory.GetCurrentDirectory() + "\\t0425.res", Properties.Resources.t0425);
            t0425.ResFileName = System.IO.Directory.GetCurrentDirectory() + "\\t0425.res";
            t0425.ReceiveData += new _IXAQueryEvents_ReceiveDataEventHandler(t0425_ReceiveData);
            t0425.ReceiveMessage += new _IXAQueryEvents_ReceiveMessageEventHandler(t0425_ReceiveMessage);

            File.WriteAllBytes(System.IO.Directory.GetCurrentDirectory() + "\\CSPAT00800.res", Properties.Resources.CSPAT00800);
            CSPAT00800.ResFileName = System.IO.Directory.GetCurrentDirectory() + "\\CSPAT00800.res";
            CSPAT00800.ReceiveData += new _IXAQueryEvents_ReceiveDataEventHandler(CSPAT00800_ReceiveData);
            CSPAT00800.ReceiveMessage += new _IXAQueryEvents_ReceiveMessageEventHandler(CSPAT00800_ReceiveMessage);

            File.WriteAllBytes(System.IO.Directory.GetCurrentDirectory() + "\\t1102.res", Properties.Resources.t1102);
            t1102.ResFileName = System.IO.Directory.GetCurrentDirectory() + "\\t1102.res";
            t1102.ReceiveData += new _IXAQueryEvents_ReceiveDataEventHandler(t1102_ReceiveData);

            File.WriteAllBytes(System.IO.Directory.GetCurrentDirectory() + "\\t8430.res", Properties.Resources.t8430);
            t8430.ResFileName = System.IO.Directory.GetCurrentDirectory() + "\\t8430.res";
            t8430.ReceiveData += new _IXAQueryEvents_ReceiveDataEventHandler(t8430_ReceiveData);
            t8430.ReceiveMessage += new _IXAQueryEvents_ReceiveMessageEventHandler(t8430_ReceiveMessage);

            File.WriteAllBytes(System.IO.Directory.GetCurrentDirectory() + "\\CSPAT00600.res", Properties.Resources.CSPAT00600);
            CSPAT00600.ResFileName = System.IO.Directory.GetCurrentDirectory() + "\\CSPAT00600.res";
            CSPAT00600.ReceiveData += new _IXAQueryEvents_ReceiveDataEventHandler(CSPAT00600_ReceiveData);
            CSPAT00600.ReceiveMessage += new _IXAQueryEvents_ReceiveMessageEventHandler(CSPAT00600_ReceiveMessage);

            File.WriteAllBytes(System.IO.Directory.GetCurrentDirectory() + "\\S3_.res", Properties.Resources.S3_);
            s3.ResFileName = System.IO.Directory.GetCurrentDirectory() + "\\S3_.res";
            s3.ReceiveRealData += new _IXARealEvents_ReceiveRealDataEventHandler(s3_ReceiveData);


            File.WriteAllBytes(System.IO.Directory.GetCurrentDirectory() + "\\t0167.res", Properties.Resources.t0167);
            t0167.ResFileName = System.IO.Directory.GetCurrentDirectory() + "\\t0167.res";
            t0167.ReceiveData += new _IXAQueryEvents_ReceiveDataEventHandler(t0167_ReceiveData);
            t0167.ReceiveMessage += new _IXAQueryEvents_ReceiveMessageEventHandler(t0167_ReceiveMessage);

            if (UserData.IsAccountLogin)
            {
                for (int i = 0; i < UserData.listAccount.Count; i++)
                {
                    cbAccountList.Properties.Items.Add(UserData.listAccount[i].계좌번호);
                    cbAccountList.SelectedIndex = 0;
                }
            }
            lbUserName.Text = UserData.userName;

            AddRepositoryButton();

            _IsAutoControl = false;

            statusImage = lbExpert.ImageOptions.Image;
            lbExpert.ImageOptions.Image = null;

            Thread t3 = new Thread(new ThreadStart(TimerThread));
            t3.Start();

            gridControl4.DataSource = _listHoldings.ToList();
            gridControl7.DataSource = _listHoldings.ToList();

            InitializeProfit();
            InitializeControl();
            InitializeYield();
            InitializeRatio();
            InitializeStocks();
            LoadOrderModel();

        }

        private void InitializeProfit()
        {
            try
            {
                _serverData = ServerData.GetServerData();
                
                if (_serverData.memberCount > 0)
                {
                    lbUserName.Text = UserData.user.name;
                    lbEstimatedAssets.Text = string.Format("{0:n0}", _serverData.totalestimatedassets);
                    lbAccumulatedProfit.Text = string.Format("{0:n0}", _serverData.profit);
                    lbMemberCount.Text = string.Format("{0:n0}", _serverData.memberCount);
                }
                else
                    throw new Exception("누적 수익금 조회 실패");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "오류");
            }
        }

        private void InitializeControl()
        {
            try
            {
                if (string.IsNullOrEmpty(UserData.user.expertname))
                    UserData.user.expertname = "미연결";
                
                lbExpert.Text = UserData.user.expertname;

                if (Properties.Settings.Default.AutoBuy)
                {
                    btnControlMode.Text = "자동";
                    StartAutoControlThread();
                }
                else
                {
                    btnControlMode.Text = "수동";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "오류");
            }

        }

        private void InitializeYield()
        {
            try
            {
                string callUrl = "http://Moneymachinects-env-1.eba-49j29inb.ap-northeast-2.elasticbeanstalk.com/api/expertyield?" + "expertidx=" + UserData.user.expertidx;
                //string callUrl = "http://localhost:63768/api/order?" + "expertidx=" + UserData.user.expertidx;

                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(callUrl); // 인코딩 UTF-8
                httpWebRequest.ContentType = "application/json; charset=UTF-8";
                httpWebRequest.Method = "GET";

                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.GetEncoding("UTF-8")))
                {
                    string responseString = streamReader.ReadToEnd();

                    var jsonArray = JArray.FromObject(JsonConvert.DeserializeObject(responseString));

                    List<YieldModel> listYieldModel = jsonArray.ToObject<List<YieldModel>>();

                    int rowIndex = gridView4.TopRowIndex;
                    int focusRow = gridView4.FocusedRowHandle;
                    gridControl4.BeginUpdate();

                    gridControl4.DataSource = listYieldModel.ToList();
                    gridControl4.RefreshDataSource();

                    gridView4.FocusedRowHandle = focusRow;
                    gridView4.TopRowIndex = rowIndex;
                    gridControl4.EndUpdate();

                    streamReader.Close();
                }

                httpWebResponse.Close();

                if (_listBuyOrderModel.Count > 0 && _listBuyOrderModel.Count > 3)
                {
                    _listBuyOrderModel.RemoveRange(3, _listBuyOrderModel.Count - 3);
                }

                gridControl1.DataSource = _listBuyOrderModel.ToList();
                gridControl1.RefreshDataSource();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "오류");
            }
        }
        private void InitializeRatio()
        {
            string callUrl = "http://Moneymachinects-env-1.eba-49j29inb.ap-northeast-2.elasticbeanstalk.com/api/ratio";
            //string callUrl = "http://localhost:63768/api/ratio;

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(callUrl); // 인코딩 UTF-8
            httpWebRequest.ContentType = "application/json; charset=UTF-8";
            httpWebRequest.Method = "GET";

            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.GetEncoding("UTF-8")))
            {
                string responseString = streamReader.ReadToEnd();

                var jobject = JObject.FromObject(JsonConvert.DeserializeObject(responseString));
                _ratioModel = jobject.ToObject<RatioModel>();
            }
        }

        private void InitializeStocks()
        {
            t8430.SetFieldData("t8430InBlock", "gubun", 0, "0"); // 0:전체 / 1:코스피 / 2:코스닥
            t8430.Request(false);
        }

        private void StartAutoControlThread()
        {
            _IsAutoControl = true;
            // 새로운 쓰레드에서 Run() 실행
            Thread t1 = new Thread(new ThreadStart(AutoControlThread));
            t1.Start();
            timerAuto.Start();
        }

        private void StopAutoControlThread()
        {
            _IsAutoControl = false;
            timerAuto.Stop();
        }

        private void AutoControlThread()
        {
            while (_IsAutoControl)
            {
                AutoOrder();
                Thread.Sleep(100);
            }
        }

        private int GetSellRatioCount(int price)
        {
            var 구매가능금액 =  (UserData.SelectedAccount.추정순자산 / 100) * _ratioModel.ratio;
            var 구매가능수량 = (int)구매가능금액 / price;
            return 구매가능수량;
        }

        private void AutoOrder()
        {
            for (int i = 0; i < _listOrderModel.Count; i++)
            {
                OrderModel orderModel = _listOrderModel[i];

                switch(orderModel.ordertype)
                {
                    case "매수":
                        if (_listAutoOrder.FindIndex(r => r.orderdate == orderModel.orderdate) < 0 && _listManualOrder.FindIndex(r => r.orderdate == orderModel.orderdate) < 0 && orderModel.status != true)
                        {
                            CSPAT00600.SetFieldData("CSPAT00600InBlock1", "AcntNo", 0, UserData.SelectedAccount.계좌번호);
                            CSPAT00600.SetFieldData("CSPAT00600InBlock1", "InptPwd", 0, Properties.Settings.Default.AccountPW);
                            CSPAT00600.SetFieldData("CSPAT00600InBlock1", "IsuNo", 0, orderModel.expcode);
                            orderModel.ordercount = GetSellRatioCount(orderModel.orderprice);
                            CSPAT00600.SetFieldData("CSPAT00600InBlock1", "OrdQty", 0, orderModel.ordercount.ToString());
                            CSPAT00600.SetFieldData("CSPAT00600InBlock1", "OrdprcPtnCode", 0, "03"); // 00 지정가 03 시장가

                            //시장가 매수일때 상한가 입력인지 공백입력인지 확인필요
                            CSPAT00600.SetFieldData("CSPAT00600InBlock1", "OrdPrc", 0, "");
                            //CSPAT00600.SetFieldData("CSPAT00600InBlock1", "OrdPrc", 0, _upLimitPrice);
                            CSPAT00600.SetFieldData("CSPAT00600InBlock1", "BnsTpCode", 0, "2"); // 2 매수

                            Order order = new Order();
                            order.expcode = orderModel.expcode;
                            order.expname = orderModel.expname;
                            order.orderdate = orderModel.orderdate;
                            order.orderprice = orderModel.orderprice;
                            order.count = orderModel.ordercount;
                            _listAutoOrder.Add(order);

                            CSPAT00600.SetFieldData("CSPAT00600InBlock1", "MgntrnCode", 0, "000"); //신용거래코드 : 000보통
                            CSPAT00600.SetFieldData("CSPAT00600InBlock1", "OrdCndiTpCode", 0, "0"); // 0 : 없음 , 1 : IOC , 2 : FOK

                            CSPAT00600.Request(true);

                            orderModel.status = true;
                        }
                        break;

                    case "매도":
                        int idx = _listHoldings.FindIndex(r => r.보유종목코드 == orderModel.expcode);
                        if (idx > -1)
                        {
                            int count = 0;

                            //if (_listAutoOrder.FindIndex(r => r.expcode == orderModel.expcode) > -1 || _listManualOrder.FindIndex(r => r.expcode == orderModel.expcode) > -1)
                            {
                                CSPAT00600.SetFieldData("CSPAT00600InBlock1", "AcntNo", 0, UserData.SelectedAccount.계좌번호);
                                CSPAT00600.SetFieldData("CSPAT00600InBlock1", "InptPwd", 0, Properties.Settings.Default.AccountPW);
                                CSPAT00600.SetFieldData("CSPAT00600InBlock1", "IsuNo", 0, orderModel.expcode);
                                if (Properties.Settings.Default.BulkSell)
                                    count = (int)_listHoldings[idx].잔고수량;
                                else
                                {
                                    //int orderIdx = _listAutoOrder.FindIndex(r => r.expcode == orderModel.expcode);
                                    //if (orderIdx > -1)
                                    //{
                                    //    count = _listAutoOrder[orderIdx].count;
                                    //}
                                    //if(count == 0)
                                    //    count = GetSellRatioCount(orderModel.orderprice);
                                    count = _listOrderModel.Find(o => o.expcode == orderModel.expcode).ordercount;
                                    if(count == 0)
                                    {
                                        count = (int)_listHoldings[idx].잔고수량;
                                    }
                                }

                                CSPAT00600.SetFieldData("CSPAT00600InBlock1", "OrdQty", 0, count.ToString());
                                CSPAT00600.SetFieldData("CSPAT00600InBlock1", "OrdprcPtnCode", 0, "03"); // 00 지정가 03 시장가

                                //시장가 매수일때 상한가 입력인지 공백입력인지 확인필요
                                CSPAT00600.SetFieldData("CSPAT00600InBlock1", "OrdPrc", 0, " ");
                                //CSPAT00600.SetFieldData("CSPAT00600InBlock1", "OrdPrc", 0, _upLimitPrice);
                                CSPAT00600.SetFieldData("CSPAT00600InBlock1", "BnsTpCode", 0, "1"); //1 매도

                                CSPAT00600.SetFieldData("CSPAT00600InBlock1", "MgntrnCode", 0, "000"); //신용거래코드 : 000보통
                                CSPAT00600.SetFieldData("CSPAT00600InBlock1", "OrdCndiTpCode", 0, "0"); // 0 : 없음 , 1 : IOC , 2 : FOK

                                CSPAT00600.Request(true);

                                int index = _listAutoOrder.FindIndex(r => r.expcode == orderModel.expcode);
                                if (index > -1)
                                {
                                    _listAutoOrder.RemoveAt(index);
                                }
                                index = _listManualOrder.FindIndex(r => r.expcode == orderModel.expcode);
                                if (index > -1)
                                    _listManualOrder.RemoveAt(index);

                                //index = _listOrderModel.FindIndex(r => r.expcode == orderModel.expcode && r.ordertype == "매수");
                                //if (index > -1)
                                //    _listOrderModel.RemoveAt(index);
                                //foreach (var model in models)
                                //{
                                //    _listOrderModel.Remove(model);
                                //}
                                _listSellOrderModel.Remove(orderModel);
                                orderModel.status = true;
                            }
                        }
                        break;
                }

                
            }
        }

        private void RefreshThread()
        {
            while (_IsRefresh)
            {
                //for (int i = 0; i < _listOrderSuccessModel.Count; i++)
                //{
                //    t1102.SetFieldData("t1102InBlock", "shcode", 0, _listOrderSuccessModel[i].expcode);
                //    t1102.Request(true);
                //    Thread.Sleep(100);
                //}

                SetAccountData();
                SetBuySellData();
                Thread.Sleep(1000);
            }
        }

        private void TimerThread()
        {
            while(_IsRefresh)
            {
                t0167.SetFieldData("t0167InBlock", "id", 0, UserData.ebestID);
            }
        }

        private void s3_ReceiveData(string szCode)
        {
            string code = s3.GetFieldData("OutBlock", "shcode");
            string price = s3.GetFieldData("OutBlock", "price");


            var stock = _listStocks.FindAll(r => r.단축코드 == code);
            if (stock != null)
            {
                for (int i = 0; i < stock.Count; i++)
                {
                    stock[i].현재가 = Convert.ToInt32(price);
                }
            }

            //var auto = _listAutoOrder.FindAll(r => r.expcode == code);
            //if(auto != null)
            //{
            //    for (int i = 0; i < auto.Count; i++)
            //    {
            //        _listAutoOrder[i].price = Convert.ToInt32(price);
            //    }
            //}

            //var manual = _listManualOrder.FindAll(r => r.expcode == code);
            //if (manual != null)
            //{
            //    for (int i = 0; i < manual.Count; i++)
            //    {
            //        _listManualOrder[i].price = Convert.ToInt32(price);
            //    }
            //}

            //gridControl2.RefreshDataSource();
            //gridControl3.RefreshDataSource();
        }

        private void t1102_ReceiveData(string szTrCode)
        {
            //string name = t1102.GetFieldData("t1102OutBlock", "hname", 0);
            //string price = t1102.GetFieldData("t1102OutBlock", "price", 0);

            //int idx = _listAutoOrder.FindIndex(r => r.expname == name);

            //_listAutoOrder[idx].expname = name;
            //_listAutoOrder[idx].price = Convert.ToInt32(price);

            //gridControl2.RefreshDataSource();
            //gridControl3.RefreshDataSource();
        }

        private void t0167_ReceiveMessage(bool bIsSystemError, string nMessageCode, string szMessage)
        {
            if (bIsSystemError)
            {
                MessageBox.Show(nMessageCode + " : " + szMessage, "알림");
            }
        }

        private void t0167_ReceiveData(string szTrCode)
        {
            string time = t0167.GetFieldData("t0167OutBlock", "time", 0);
            time = time.Substring(8, 4);

            TimeSpan ts = DateTime.Now.ToLocalTime() - Convert.ToDateTime(time);
            if (ts.TotalSeconds > 0)
                _IsRun = false;
        }


        private void t8430_ReceiveMessage(bool bIsSystemError, string nMessageCode, string szMessage)
        {
            //if (bIsSystemError)
            //{
            //    MessageBox.Show(nMessageCode + " : " + szMessage, "알림");
            //}
        }

        //주식 종목 조회
        private void t8430_ReceiveData(string szTrCode)
        {
            try
            {
                int cnt = t8430.GetBlockCount("t8430OutBlock");
                for (int i = 0; i < cnt; i++)
                {
                    Stock stock = new Stock();
                    stock.종목명 = t8430.GetFieldData("t8430OutBlock", "hname", i);
                    stock.단축코드 = t8430.GetFieldData("t8430OutBlock", "shcode", i);
                    stock.확장코드 = t8430.GetFieldData("t8430OutBlock", "expcode", i);
                    stock.ETF = t8430.GetFieldData("t8430OutBlock", "etfgubun", i);
                    stock.상한가 = int.Parse(t8430.GetFieldData("t8430OutBlock", "uplmtprice", i)); //api : 실현손익
                    stock.하한가 = int.Parse(t8430.GetFieldData("t8430OutBlock", "dnlmtprice", i)); //api : 실현손익
                    stock.전일가 = int.Parse(t8430.GetFieldData("t8430OutBlock", "jnilclose", i)); //api : 실현손익
                    stock.주문수량단위 = t8430.GetFieldData("t8430OutBlock", "memedan", i);
                    stock.기준가 = int.Parse(t8430.GetFieldData("t8430OutBlock", "recprice", i)); //api : 실현손익
                    stock.구분 = t8430.GetFieldData("t8430OutBlock", "gubun", i);
                    _listStocks.Add(stock);
                    s3.SetFieldData("InBlock", "shcode", stock.단축코드);
                    s3.AdviseRealData();
                }

                if(cnt > 0)
                {
                    _IsRefresh = true;

                    Thread t2 = new Thread(new ThreadStart(RefreshThread));
                    t2.Start();
                }
                else
                {
                    MessageBox.Show("종목정보를 가져오지 못했습니다.");
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void AddRepositoryButton()
        {
            repositoryItemButtonEdit1.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            repositoryItemButtonEdit1.ButtonClick += edit_BuyButtonClick;
            repositoryItemButtonEdit1.Buttons[0].Caption = "매수";
            repositoryItemButtonEdit1.Buttons[0].Kind = DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph;
            //edit.Buttons[0].Appearance.BackColor = Color.Black;
            gridView1.Columns["Status"].ColumnEdit = repositoryItemButtonEdit1;
            gridView1.Columns["Status"].AppearanceCell.BackColor = Color.FromArgb(170,57,57);
            gridView1.Appearance.FocusedRow.BackColor = Color.Transparent;
            gridView1.Appearance.FocusedCell.BackColor = Color.FromArgb(170, 57, 57);
            gridView1.Columns["Status"].AppearanceCell.ForeColor = Color.White;

            repositoryItemButtonEdit7.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            repositoryItemButtonEdit7.ButtonClick += edit_SellButtonClick;
            repositoryItemButtonEdit7.Buttons[0].Caption = "매도";
            repositoryItemButtonEdit7.Buttons[0].Kind = DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph;
            //edit.Buttons[0].Appearance.BackColor = Color.Black;
            gridView7.Columns["Status"].ColumnEdit = repositoryItemButtonEdit7;
            gridView7.Columns["Status"].AppearanceCell.BackColor = Color.FromArgb(65, 108, 183);
            gridView7.Appearance.FocusedRow.BackColor = Color.Transparent;
            gridView7.Appearance.FocusedCell.BackColor = Color.FromArgb(65, 108, 183);
            gridView7.Columns["Status"].AppearanceCell.ForeColor = Color.White;
        }

        private void edit_BuyButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int index = -1;

                index = gridView1.FocusedRowHandle;

                int count = GetSellRatioCount(_listBuyOrderModel[index].orderprice);

                //종목코드 필요함.
                BuySellCheckForm buySellCheckForm = new BuySellCheckForm(_listBuyOrderModel[index], count.ToString());
                if (buySellCheckForm.ShowDialog() == DialogResult.OK)
                {
                    Order order = new Order();
                    order.expcode = _listBuyOrderModel[index].expcode;
                    order.expname = _listBuyOrderModel[index].expname;
                    order.orderdate = _listBuyOrderModel[index].orderdate;
                    order.orderprice = _listBuyOrderModel[index].price;
                    
                    _listManualOrder.Add(order);
                    //gridControl3.DataSource = _listManualOrder.ToList();
                    //gridControl3.RefreshDataSource();
                }
            }
            catch
            {
                MessageBox.Show("매수 실패! 고객센터에 문의바랍니다.");
            }
         
        }

        private void edit_SellButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                var code = gridView7.GetFocusedRowCellValue("보유종목코드");
                var 잔고수량 = gridView7.GetRowCellValue(gridView7.GetFocusedDataSourceRowIndex(), "잔고수량");
                Console.WriteLine(code);
                Console.WriteLine(잔고수량);
                BuySellCheckForm buySellCheckForm = new BuySellCheckForm("매도", code.ToString(), 잔고수량.ToString());
                if (buySellCheckForm.ShowDialog() == DialogResult.OK)
                {
                    int idx = _listManualOrder.FindIndex(r => r.expcode == code.ToString());
                    if (idx > -1)
                    {
                        _listManualOrder.RemoveAt(idx);
                        //gridControl3.DataSource = _listManualOrder.ToList();
                        //gridControl3.RefreshDataSource();
                    }
                    idx = _listAutoOrder.FindIndex(r => r.expcode == code.ToString());
                    if (idx > -1)
                    {
                        _listAutoOrder.RemoveAt(idx);
                        //gridControl2.DataSource = _listAutoOrder.ToList();
                        //gridControl2.RefreshDataSource();
                    }
                    ChangeControlMode();
                }
            }
            catch
            {
                MessageBox.Show("매도 실패! 고객센터에 문의바랍니다.");
            }
        }

        private void cbAccountList_SelectedIndexChanged(object sender, EventArgs e)
        {
            UserData.SelectedAccount = UserData.listAccount.Find(a => a.계좌번호 == cbAccountList.SelectedItem.ToString());
            if(_IsRefresh)
            {
                SetAccountData();
                SetBuySellData();
            }
        }

        void SetAccountData()
        {
            t0424.SetFieldData("t0424InBlock", "accno", 0, UserData.SelectedAccount.계좌번호);
            t0424.SetFieldData("t0424InBlock", "passwd", 0, Properties.Settings.Default.AccountPW);
            t0424.Request(true);
        }

        void SetBuySellData()
        {
            t0425.SetFieldData("t0425InBlock", "accno", 0, UserData.SelectedAccount.계좌번호);
            t0425.SetFieldData("t0425InBlock", "passwd", 0, Properties.Settings.Default.AccountPW);
            t0425.SetFieldData("t0425InBlock", "chegb", 0, "0"); //0전체 1체결 2미체결
            t0425.SetFieldData("t0425InBlock", "medosu", 0, "0"); //0전체 1매도 2매수
            t0425.SetFieldData("t0425InBlock", "sortgb", 0, "1"); //1 역순 2 순
            t0425.Request(true);
        }
        
        void t0425_ReceiveMessage(bool bIsSystemError, string nMessageCode, string szMessage)
        {
            //if (bIsSystemError)
            //{
            //    //MessageBox.Show(nMessageCode + " : " + szMessage, "알림");
            //}
        }

        void t0425_ReceiveData(string szTrCode)//주식 체결/미체결
        {
            List<체결> dataList = new List<체결>();
            _listSignedModel.Clear();

            int rowIndex;
            int focusRow;

            try
            {
                int cnt = t0425.GetBlockCount("t0425OutBlock1");

                for (int i = 0; i < cnt; i++)
                {
                    long 잔량 = long.Parse(t0425.GetFieldData("t0425OutBlock1", "ordrem", i));

                    //if (잔량 > 0)
                    {
                        체결 data = new 체결();

                        data.원주문번호 = long.Parse(t0425.GetFieldData("t0425OutBlock1", "ordno", i)).ToString();
                        data.체결날짜 = "";

                        string 코드 = t0425.GetFieldData("t0425OutBlock1", "expcode", i);
                        data.종목명 = _listStocks.Find(r => r.단축코드 == 코드).종목명;
                        data.종목코드 = 코드;
                        data.주문구분 = t0425.GetFieldData("t0425OutBlock1", "medosu", i);
                        data.주문수량 = t0425.GetFieldData("t0425OutBlock1", "qty", i);
                        data.주문가격 = long.Parse(t0425.GetFieldData("t0425OutBlock1", "price", i));
                        data.현재가 = long.Parse(t0425.GetFieldData("t0425OutBlock1", "price1", i));
                        var or = _listOrderModel.FindAll(r => r.expcode == data.종목코드);
                        
                        if (or != null)
                        {
                            for (int x = 0; x < or.Count; x++)
                            {
                                int idx = _listOrderModel.FindIndex(o => o.expcode == data.종목코드);

                                //or[x].orderprice = (int)data.매입금액;
                                or[x].price = (int)data.현재가;
                                //gridControl2.DataSource = _listAutoOrder.ToList();
                                //gridControl2.RefreshDataSource();
                            }
                        }
                        data.체결수량 = long.Parse(t0425.GetFieldData("t0425OutBlock1", "cheqty", i));
                        data.미체결잔량 = 잔량;
                        data.체결가격 = long.Parse(t0425.GetFieldData("t0425OutBlock1", "cheprice", i));

                        data.체결상태 = t0425.GetFieldData("t0425OutBlock1", "status", i);
                        if(data.미체결잔량 > 0)
                            dataList.Add(data);
                        else
                            _listSignedModel.Add(data);
                    }
                }

                /// 미체결 내역 업데이트
                rowIndex = gridView6.TopRowIndex;
                focusRow = gridView6.FocusedRowHandle;
                gridControl6.BeginUpdate();

                gridControl6.DataSource = dataList.ToList();
                gridControl6.RefreshDataSource();

                gridView6.FocusedRowHandle = focusRow;
                gridView6.TopRowIndex = rowIndex;
                gridControl6.EndUpdate();
                ///

                /// 체결내역 업데이트
                rowIndex = gridView8.TopRowIndex;
                focusRow = gridView8.FocusedRowHandle;
                gridControl8.BeginUpdate();

                gridControl8.DataSource = _listSignedModel.ToList();
                gridControl8.RefreshDataSource();

                gridView8.FocusedRowHandle = focusRow;
                gridView8.TopRowIndex = rowIndex;
                gridControl8.EndUpdate();
                /// 


                rowIndex = gridView5.TopRowIndex;
                focusRow = gridView5.FocusedRowHandle;
                gridControl5.BeginUpdate();

                gridControl5.DataSource = dataList.ToList();
                gridControl5.RefreshDataSource();

                gridView5.FocusedRowHandle = focusRow;
                gridView5.TopRowIndex = rowIndex;
                gridControl5.EndUpdate();

                

                if (t0425.IsNext)
                {
                    연속key_t0425 = t0425.GetFieldData("t0425OutBlock", "cts_ordno", 0);
                }
                else
                {
                }
            }
            catch(Exception ex)
            {
                rowIndex = gridView5.TopRowIndex;
                focusRow = gridView5.FocusedRowHandle;
                gridControl5.BeginUpdate();

                gridControl5.DataSource = dataList.ToList();
                gridControl5.RefreshDataSource();

                gridView5.FocusedRowHandle = focusRow;
                gridView5.TopRowIndex = rowIndex;
                gridControl5.EndUpdate();

                rowIndex = gridView6.TopRowIndex;
                focusRow = gridView6.FocusedRowHandle;
                gridControl6.BeginUpdate();

                gridControl6.DataSource = dataList.ToList();
                gridControl6.RefreshDataSource();

                gridView6.FocusedRowHandle = focusRow;
                gridView6.TopRowIndex = rowIndex;
                gridControl6.EndUpdate();
            }
        }

        void t0424_ReceiveMessage(bool bIsSystemError, string nMessageCode, string szMessage)
        {
            if (bIsSystemError)
            {
                //MessageBox.Show(nMessageCode + " : " + szMessage, "알림");
            }
        }

        void t0424_ReceiveData(string szTrCode)//주식잔고2
        {
            try
            {
                int cnt = t0424.GetBlockCount("t0424OutBlock");
                for (int i = 0; i < cnt; i++)
                {
                    AccountInfo user = UserData.SelectedAccount;
                    user.추정순자산 = int.Parse(t0424.GetFieldData("t0424OutBlock", "sunamt", i));
                    user.매입금액 = int.Parse(t0424.GetFieldData("t0424OutBlock", "mamt", i));
                    user.평가금액 = int.Parse(t0424.GetFieldData("t0424OutBlock", "tappamt", i));
                    user.평가손익 = int.Parse(t0424.GetFieldData("t0424OutBlock", "tdtsunik", i));
                    user.확정손익 = int.Parse(t0424.GetFieldData("t0424OutBlock", "dtsunik", i)); //api : 실현손익

                    UserData.user.totalestimatedassets = (int)user.추정순자산;
                    UserData.user.profit = (int)user.확정손익;

                    lbTotalEstimatedAssets.Text = string.Format("{0:n0}", user.추정순자산);
                    lbPurchaseAmount.Text = string.Format("{0:n0}", user.매입금액);
                    lbEvaluationAmount.Text = string.Format("{0:n0}", user.평가금액);
                    lbValuationGainOrLoss.Text = string.Format("{0:n0}", user.평가손익);
                    if (user.평가손익 < 0)
                    {
                        lbValuationGainOrLoss.ForeColor = Color.FromArgb(255, 7, 71, 166);
                    }
                    else if (user.평가손익 > 0)
                    {
                        lbValuationGainOrLoss.ForeColor = Color.Red;
                    }
                    else
                    {
                        lbValuationGainOrLoss.ForeColor = Color.White;
                    }

                    lbExtendedIncome.Text = string.Format("{0:n0}", user.확정손익);
                    if (user.확정손익 < 0)
                    {
                        lbExtendedIncome.ForeColor = Color.FromArgb(255, 7, 71, 166);
                    }
                    else if (user.확정손익 > 0)
                    {
                        lbExtendedIncome.ForeColor = Color.Red;
                    }
                    else
                    {
                        lbExtendedIncome.ForeColor = Color.White;
                    }

                    //UserData.listAccount.Add(user);
                }


                _listHoldings.Clear();
                int cnt2 = t0424.GetBlockCount("t0424OutBlock1");

                for (int i = 0; i < cnt2; i++)
                {
                    Holding data = new Holding();
                    data.보유종목코드 = t0424.GetFieldData("t0424OutBlock1", "expcode", i);
                    data.보유종목명 = t0424.GetFieldData("t0424OutBlock1", "hname", i);
                    data.현재가 = long.Parse(t0424.GetFieldData("t0424OutBlock1", "price", i));
                    data.평균단가 = long.Parse(t0424.GetFieldData("t0424OutBlock1", "pamt", i));
                    data.매수날짜 = "";
                    data.잔고수량 = long.Parse(t0424.GetFieldData("t0424OutBlock1", "janqty", i));
                    data.매입금액 = long.Parse(t0424.GetFieldData("t0424OutBlock1", "mamt", i));
                    data.평가금액 = long.Parse(t0424.GetFieldData("t0424OutBlock1", "appamt", i));
                    data.평가손익 = long.Parse(t0424.GetFieldData("t0424OutBlock1", "dtsunik", i));
                    data.수익률 = float.Parse(t0424.GetFieldData("t0424OutBlock1", "sunikrt", i));
                    data.매도신호시간 = "";
                    data.Status = false;

                    int idx = _listSellOrderModel.FindIndex(r => r.expcode == data.보유종목코드);
                    
                    if (idx > -1)
                    {
                        data.매도신호시간 = _listSellOrderModel[idx].orderdate;
                    }

                    var or = _listOrderModel.FindAll(r => r.expcode == data.보유종목코드);
                    if (or != null)
                    {
                        for (int x = 0; x < or.Count; x++)
                        {
                            idx = _listOrderModel.FindIndex(o => o.expcode == data.보유종목코드);

                            //or[x].orderprice = (int)data.매입금액;
                            or[x].price = (int)data.현재가;
                            //gridControl2.DataSource = _listAutoOrder.ToList();
                            //gridControl2.RefreshDataSource();
                        }
                    }

                    var auto = _listAutoOrder.FindAll(r => r.expcode == data.보유종목코드);
                    if (auto != null)
                    {
                        for (int x = 0; x < auto.Count; x++)
                        {
                            idx = _listAutoOrder.FindIndex(o => o.expcode == data.보유종목코드);

                            auto[x].cheprice = (int)data.매입금액;
                            auto[x].price = (int)data.현재가;
                            //gridControl2.DataSource = _listAutoOrder.ToList();
                            //gridControl2.RefreshDataSource();
                        }
                    }

                    var manual = _listManualOrder.FindAll(r => r.expcode == data.보유종목코드);
                    if (manual != null)
                    {
                        for (int x = 0; x < manual.Count; x++)
                        {
                            manual[x].cheprice = (int)data.매입금액;
                            manual[x].price = (int)data.현재가;
                            //gridControl3.DataSource = _listManualOrder.ToList();
                            //gridControl3.RefreshDataSource();
                        }
                    }

                    _listHoldings.Add(data);
                }

                int rowIndex = gridView2.TopRowIndex;
                int focusRow = gridView2.FocusedRowHandle;
                gridControl2.BeginUpdate();

                gridControl2.DataSource = _listOrderModel.ToList();
                gridControl2.RefreshDataSource();

                gridView2.FocusedRowHandle = focusRow;
                gridView2.TopRowIndex = rowIndex;
                gridControl2.EndUpdate();

                //gridControl4.DataSource = _listHoldings.ToList();
                //gridControl4.RefreshDataSource();

                gridControl7.DataSource = _listHoldings.ToList();
                gridControl7.RefreshDataSource();

                if (t0424.IsNext)
                {
                    연속key_t0424 = t0424.GetFieldData("t0424OutBlock", "cts_expcode", 0);
                }
                else
                {
                }
            }
            catch(Exception ex)
            {

            }
            
        }

        void CSPAT00800_ReceiveMessage(bool bIsSystemError, string nMessageCode, string szMessage)
        {
            MessageBox.Show(nMessageCode + " : " + szMessage, "알림");
        }

        void CSPAT00800_ReceiveData(string szTrCode)//주식 체결/미체결
        {
            //int cnt = CSPAT00800.GetBlockCount("CSPAT00800OutBlock1");

            //for (int i = 0; i < cnt; i++)
            //{
            //    string 체결상태 = CSPAT00800.GetFieldData("CSPAT00800OutBlock1", "status", i);
            //}
        }

        void CSPAT00600_ReceiveMessage(bool bIsSystemError, string nMessageCode, string szMessage)
        {
            //MessageBox.Show(nMessageCode + " : " + szMessage, "알림");
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


        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _IsAutoControl = false;
            _IsRefresh = false;
            _IsRun = false;
            SaveOrderModel();
            SendUserModel();
            _XASessionClass.Logout();
            _XASessionClass.DisconnectServer();
            timerOrder.Stop();
        }

        private void SendUserModel()
        {
            string callUrl = "http://Moneymachinects-env-1.eba-49j29inb.ap-northeast-2.elasticbeanstalk.com/api/user";
            //string callUrl = "http://localhost:63768/api/user";

            string json = JsonConvert.SerializeObject(UserData.user);
            //string json = JsonConvert.SerializeObject(MainForm._ratioModel);

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
        }

        private void SaveOrderModel()
        {
            try
            {
                JArray jarray = new JArray();
                JArray jarray2 = new JArray();
                if (_listAutoOrder.Count > 0)
                    jarray = JArray.FromObject(_listAutoOrder);
                if (_listManualOrder.Count > 0)
                    jarray2 = JArray.FromObject(_listAutoOrder);

                File.WriteAllText(@"D:\autoorder.json", jarray.ToString());
                File.WriteAllText(@"D:\manualorder.json", jarray2.ToString());
            }
            catch
            {

            }
        }

        private void LoadOrderModel()
        {
            try
            {
                string responseString = File.ReadAllText(@"D:\autoorder.json");
                string responseString2 = File.ReadAllText(@"D:\manualorder.json");

                var jsonArray = JArray.FromObject(JsonConvert.DeserializeObject(responseString));

                foreach (JObject jsonObj in jsonArray)
                {
                    Order order = new Order();

                    order.orderdate = jsonObj["orderdate"].ToString();
                    order.expcode = jsonObj["expcode"].ToString();
                    order.expname = jsonObj["expname"].ToString();
                    order.price = Convert.ToInt32(jsonObj["price"]);
                    order.orderprice = Convert.ToInt32(jsonObj["orderprice"]);
                    order.cheprice = Convert.ToInt64(jsonObj["cheprice"]);
                    order.count = Convert.ToInt32(jsonObj["count"]);

                    _listAutoOrder.Add(order);
                    //gridControl2.DataSource = _listAutoOrder.ToList();
                    gridControl2.RefreshDataSource();
                }

                jsonArray = JArray.FromObject(JsonConvert.DeserializeObject(responseString2));

                foreach (JObject jsonObj in jsonArray)
                {
                    Order order = new Order();

                    order.orderdate = jsonObj["orderdate"].ToString();
                    order.expcode = jsonObj["expcode"].ToString();
                    order.expname = jsonObj["expname"].ToString();
                    order.price = Convert.ToInt32(jsonObj["price"]);
                    order.orderprice = Convert.ToInt32(jsonObj["orderprice"]);
                    order.cheprice = Convert.ToInt64(jsonObj["cheprice"]);
                    order.count = Convert.ToInt32(jsonObj["count"]);

                    _listManualOrder.Add(order);
                    //gridControl3.DataSource = _listManualOrder.ToList();
                    gridControl2.RefreshDataSource();
                }

            }
            catch
            {

            }
        }


        private void timerOrder_Tick(object sender, EventArgs e)
        {
            GetOrder();
        }

        private void GetOrder()
        {
            try
            {
                string callUrl = "http://Moneymachinects-env-1.eba-49j29inb.ap-northeast-2.elasticbeanstalk.com/api/order?" + "expertidx=" + UserData.user.expertidx;
                //string callUrl = "http://localhost:63768/api/order?" + "expertidx=" + UserData.user.expertidx;

                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(callUrl); // 인코딩 UTF-8
                httpWebRequest.ContentType = "application/json; charset=UTF-8";
                httpWebRequest.Method = "GET";

                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.GetEncoding("UTF-8")))
                {
                    string responseString = streamReader.ReadToEnd();

                    var jsonArray = JArray.FromObject(JsonConvert.DeserializeObject(responseString));
                    List<OrderModel> listOrderModel = jsonArray.ToObject<List<OrderModel>>();

                    _listBuyOrderModel.Clear();
                    _listSellOrderModel.Clear();

                    foreach (OrderModel orderModel in listOrderModel)
                    {
                        orderModel.price = (int)_listStocks.Find(o => o.단축코드 == orderModel.expcode).현재가;
                        if (orderModel.ordertype == "매수")
                            _listBuyOrderModel.Add(orderModel);
                        else
                        {
                            _listSellOrderModel.Add(orderModel);
                        }
                        if(_listOrderModel.FindIndex(o => o.orderdate == orderModel.orderdate) < 0)
                        {
                            _listOrderModel.Add(orderModel);
                        }

                        int rowIndex = gridView2.TopRowIndex;
                        int focusRow = gridView2.FocusedRowHandle;
                        gridControl2.BeginUpdate();

                        gridControl2.DataSource = _listOrderModel.ToList();
                        gridControl2.RefreshDataSource();

                        gridView2.FocusedRowHandle = focusRow;
                        gridView2.TopRowIndex = rowIndex;
                        gridControl2.EndUpdate();
                    }
                    streamReader.Close();
                }

                httpWebResponse.Close();

                if (_listBuyOrderModel.Count > 0 && _listBuyOrderModel.Count > 3)
                {
                    _listBuyOrderModel.RemoveRange(3, _listBuyOrderModel.Count - 3);
                }

                gridControl1.DataSource = _listBuyOrderModel.ToList();
                gridControl1.RefreshDataSource();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "오류");
            }
        }

        private void xtraTabControl2_CustomHeaderButtonClick(object sender, DevExpress.XtraTab.ViewInfo.CustomHeaderButtonEventArgs e)
        {
            if(e.Button.Caption == "Update")
            {
                SetBuySellData();
            }
            else if(e.Button.Caption =="미체결 주문 취소")
            {
                OrderAllCancel();
            }
        }

        private void btnAccountUpdate_Click(object sender, EventArgs e)
        {
            SetAccountData();
        }

        private void OrderAllCancel()
        {
            xtraTabPage4.Show();
            xtraTabControl2.Refresh();

            if(MessageBox.Show("미체결된 주문을 전부 취소합니다.", "취소", MessageBoxButtons.OKCancel) ==DialogResult.OK)
            {
                var count = gridView6.RowCount;
                for (int i = 0; i < count; i++)
                {
                    string 원주문번호 = (string)gridView6.GetRowCellValue(i, "원주문번호");
                    long 미체결잔량 = (long)gridView6.GetRowCellValue(i, "미체결잔량");
                    string 종목번호 = (string)gridView6.GetRowCellValue(i, "종목코드");
                    CSPAT00800.SetFieldData("CSPAT00800InBlock1", "OrgOrdNo", 0, 원주문번호);
                    CSPAT00800.SetFieldData("CSPAT00800InBlock1", "AcntNo", 0, UserData.SelectedAccount.계좌번호);
                    CSPAT00800.SetFieldData("CSPAT00800InBlock1", "InptPwd", 0, Properties.Settings.Default.AccountPW);
                    CSPAT00800.SetFieldData("CSPAT00800InBlock1", "IsuNo", 0, 종목번호);
                    CSPAT00800.SetFieldData("CSPAT00800InBlock1", "OrdQty", 0, 미체결잔량.ToString());
                    CSPAT00800.Request(false);
                }

                gridControl6.RefreshDataSource();
            }
        }

        private void btnKakao_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://open.kakao.com/o/sCBRfD7c");
        }

        private void pcServiceCenter_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://helpsite.kr/");
        }

        private void timerCheckControl_Tick(object sender, EventArgs e)
        {
            timerCheckControl.Stop();
            btnControlMode.Enabled = true;
        }

        public void ChangeControlMode()
        {
            timerCheckControl.Start();
            btnControlMode.Enabled = false;

            if (btnControlMode.Text == "자동")
            {
                Properties.Settings.Default.AutoBuy = false;
                Properties.Settings.Default.Save();
                btnControlMode.Text = "수동";
                StopAutoControlThread();
            }
            else
            {
                Properties.Settings.Default.AutoBuy = true;
                Properties.Settings.Default.Save();
                btnControlMode.Text = "자동";
                StartAutoControlThread();
            }
        }

        private void timerAuto_Tick(object sender, EventArgs e)
        {
            if (lbExpert.ImageOptions.Image != null)
                lbExpert.ImageOptions.Image = null;
            else
                lbExpert.ImageOptions.Image = statusImage;
        }

        private void btnBuySell_Click(object sender, EventArgs e)
        {
            BuySellForm buySellForm = new BuySellForm();
            buySellForm.ShowDialog();
        }

        private void btnAccount_Click(object sender, EventArgs e)
        {
            AccountBalanceForm accountBalanceForm = new AccountBalanceForm();
            accountBalanceForm.ShowDialog();
        }

        private void btnYield_Click(object sender, EventArgs e)
        {
            YieldForm yieldForm = new YieldForm();
            yieldForm.ShowDialog();
        }

        private void btnSetting_Click(object sender, EventArgs e)
        {
            ConfigForm configForm = new ConfigForm();
            configForm.ShowDialog();
        }

        private void btnCommunity_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://머신홀딩스.com");
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.AutoLogin = false;
            Properties.Settings.Default.Save();
            Application.Restart();
        }

        private void btnControlMode_Click(object sender, EventArgs e)
        {
            return;
            WarningForm warningForm = new WarningForm();
            if (warningForm.ShowDialog() == DialogResult.OK)
            {
                ChangeControlMode();
            }
        }

        private void pictureEdit1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://xn--9i1bp5ghrc726b.com/");

        }
    }
}