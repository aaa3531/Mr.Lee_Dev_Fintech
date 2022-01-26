using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyMachineCTS
{
    public static class UserData
    {
        public static UserModel user = new UserModel();
        public static string ebestID;
        public static string ebestPW;
        public static string ebestCert;
        public static bool IsAccountLogin = false;
        public static string userName = "";
        //public static string selectedAccount = "";
        public static List<AccountInfo> listAccount = new List<AccountInfo>();
        public static AccountInfo SelectedAccount { get; set; }
    }

    public class UserModel
    {
        [JsonProperty("idx")]
        public int idx { get; set; }
        [JsonProperty("id")]
        public string id { get; set; }
        [JsonProperty("pw")]
        public string pw { get; set; }
        [JsonProperty("name")]
        public string name { get; set; }
        [JsonProperty("email")]
        public string email { get; set; }
        [JsonProperty("phone")]
        public string phone { get; set; }
        [JsonProperty("address")]
        public string address { get; set; }
        [JsonProperty("signupdate")]
        public DateTime signupdate { get; set; }
        [JsonProperty("paiddate")]
        public DateTime paiddate { get; set; }
        [JsonProperty("expertidx")]
        public int expertidx { get; set; }
        [JsonProperty("profit")]
        public long profit { get; set; }
        [JsonProperty("totalestimatedassets")]
        public long totalestimatedassets { get; set; }
        [JsonProperty("access")]
        public bool access { get; set; }
        [JsonProperty("downpayment")]
        public int downpayment { get; set; }
        [JsonProperty("companyidx")]
        public int companyidx { get; set; }
        [JsonProperty("expertname")]
        public string expertname { get; set; }
    }

    public class AccountInfo
    {
        public string 계좌번호;

        public long 추정순자산;
        public long 매입금액;
        public long 평가금액;
        public long 평가손익;
        public long 확정손익;

        public AccountInfo()
        {

        }

        public AccountInfo(string accountNum)
        {
            계좌번호 = accountNum;
        }
    }

    public class Holding
    {
        [DisplayName("보유종목코드")]
        public string 보유종목코드 { get; set; }

        [DisplayName("보유종목명")]
        public string 보유종목명 { get; set; }

        [DisplayName("매수날짜")]
        public string 매수날짜 { get; set; }

        [DisplayName("매도날짜")]
        public string 매도날짜 { get; set; }

        [DisplayName("잔고수량")]
        public long 잔고수량 { get; set; }

        [DisplayName("현재가")]
        public long 현재가 { get; set; }

        [DisplayName("평균단가")]
        public long 평균단가 { get; set; }

        [DisplayName("매입금액")]
        public long 매입금액 { get; set; }

        [DisplayName("평가금액")]
        public long 평가금액 { get; set; }

        [DisplayName("평가손익")]
        public long 평가손익 { get; set; }

        [DisplayName("수익률")]
        public float 수익률 { get; set; }

        [DisplayName("매도신호시간")]
        public string 매도신호시간 { get; set; }

        [DisplayName("Status")]
        public bool Status { get; set; }
    }

    public class 체결
    {
        [DisplayName("원주문번호")]
        public string 원주문번호 { get; set; }

        [DisplayName("체결날짜")]
        public string 체결날짜 { get; set; }

        [DisplayName("종목코드")]
        public string 종목코드 { get; set; }

        [DisplayName("종목명")]
        public string 종목명 { get; set; }

        [DisplayName("주문구분")]
        public string 주문구분 { get; set; }

        [DisplayName("주문수량")]
        public string 주문수량 { get; set; }

        [DisplayName("주문가격")]
        public long 주문가격 { get; set; }

        [DisplayName("현재가")]
        public long 현재가 { get; set; }

        [DisplayName("체결수량")]
        public long 체결수량 { get; set; }

        [DisplayName("미체결잔량")]
        public long 미체결잔량 { get; set; }

        [DisplayName("체결가격")]
        public long 체결가격 { get; set; }

        [DisplayName("체결상태")]
        public string 체결상태 { get; set; }
    }

    
}
