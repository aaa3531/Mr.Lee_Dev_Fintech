using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XA_DATASETLib;

namespace MoneyMachineCTS
{

    //class Order
    //{
    //    XAQueryClass CSPAT00600;

    //    public void OrderConnect(XAQueryClass name)
    //    {
    //        CSPAT00600 = name;
    //        CSPAT00600.ReceiveData += new _IXAQueryEvents_ReceiveDataEventHandler(CSPAT00600_ReceiveData);
    //    }

    //    public void Buying(string 계좌번호, string 비밀번호, string 종목번호, long 주문수량, double 주문가)
    //    {
    //        string 매매구분 = "2"; // 1 : 매도, 2 : 매수
    //        string 호가유형코드 = "00"; // 00@ 지정가, 03@시장가, 05@조건부지정가, 06@최유리지정가, 07@최우선지정가, 61@장개시전시간외종가, 81@시간외종가, 82@시간외단일가      
    //        string 신용거래코드 = "000"; // 000 : 보통
    //        string 주문조건구분 = "0"; // 0 : 없음 , 1 : IOC , 2 : FOK

    //        CSPAT00600.SetFieldData("CSPAT00600InBlock1", "AcntNo", 0, 계좌번호);
    //        CSPAT00600.SetFieldData("CSPAT00600InBlock1", "InptPwd", 0, 비밀번호);
    //        CSPAT00600.SetFieldData("CSPAT00600InBlock1", "IsuNo", 0, 종목번호);
    //        CSPAT00600.SetFieldData("CSPAT00600InBlock1", "OrdQty", 0, 주문수량.ToString());
    //        CSPAT00600.SetFieldData("CSPAT00600InBlock1", "OrdPrc", 0, 주문가.ToString());
    //        CSPAT00600.SetFieldData("CSPAT00600InBlock1", "BnsTpCode", 0, 매매구분);
    //        CSPAT00600.SetFieldData("CSPAT00600InBlock1", "OrdprcPtnCode", 0, 호가유형코드);
    //        CSPAT00600.SetFieldData("CSPAT00600InBlock1", "MgntrnCode", 0, 신용거래코드);
    //        //CSPAT00600.SetFieldData("CSPAT00600InBlock1", "LoanDt", 0, 대출일);
    //        CSPAT00600.SetFieldData("CSPAT00600InBlock1", "OrdCndiTpCode", 0, 주문조건구분);

    //        CSPAT00600.Request(true);
    //    }
        
    //    public void Sell(string 계좌번호, string 비밀번호, string 종목번호, long 주문수량, double 주문가)
    //    {
    //        string 매매구분 = "1"; // 1 : 매도, 2 : 매수
    //        string 호가유형코드 = "00"; // 00@ 지정가, 03@시장가, 05@조건부지정가, 06@최유리지정가, 07@최우선지정가, 61@장개시전시간외종가, 81@시간외종가, 82@시간외단일가      
    //        string 신용거래코드 = "000"; // 000 : 보통
    //        string 주문조건구분 = "0"; // 0 : 없음 , 1 : IOC , 2 : FOK

    //        CSPAT00600.SetFieldData("CSPAT00600InBlock1", "AcntNo", 0, 계좌번호);
    //        CSPAT00600.SetFieldData("CSPAT00600InBlock1", "InptPwd", 0, 비밀번호);
    //        CSPAT00600.SetFieldData("CSPAT00600InBlock1", "IsuNo", 0, 종목번호);
    //        CSPAT00600.SetFieldData("CSPAT00600InBlock1", "OrdQty", 0, 주문수량.ToString());
    //        CSPAT00600.SetFieldData("CSPAT00600InBlock1", "OrdPrc", 0, 주문가.ToString());
    //        CSPAT00600.SetFieldData("CSPAT00600InBlock1", "BnsTpCode", 0, 매매구분);
    //        CSPAT00600.SetFieldData("CSPAT00600InBlock1", "OrdprcPtnCode", 0, 호가유형코드);
    //        CSPAT00600.SetFieldData("CSPAT00600InBlock1", "MgntrnCode", 0, 신용거래코드);
    //        //CSPAT00600.SetFieldData("CSPAT00600InBlock1", "LoanDt", 0, 대출일);
    //        CSPAT00600.SetFieldData("CSPAT00600InBlock1", "OrdCndiTpCode", 0, 주문조건구분);

    //        CSPAT00600.Request(true);
    //    }

    //    void CSPAT00600_ReceiveData(string szTrCode)//주식잔고2
    //    {
    //        int cnt = CSPAT00600.GetBlockCount("CSPAT00600OutBlock1");
    //        for (int i = 0; i < cnt; i++)
    //        {
                
    //        }

    //        int cnt2 = CSPAT00600.GetBlockCount("CSPAT00600OutBlock2");
    //        for (int i = 0; i < cnt2; i++)
    //        {
                
    //        }

    //        if (CSPAT00600.IsNext)
    //        {
    //            // 연속key_t0424 = t0424.GetFieldData("t0424OutBlock", "cts_expcode", 0);

    //            // myTimer_t0424.Interval = 1000;
    //            // myTimer_t0424.Start();
    //        }
    //        else
    //        {
    //            //  myTimer_t0424.Stop();
    //        }
    //    }
    //}
}
