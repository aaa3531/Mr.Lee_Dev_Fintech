using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace eBestAPI.Common
{
    public class Stock
    {
        public string 종목명 { get; set; }
        public string 단축코드 { get; set; }
        public string 확장코드 { get; set; }
        public string ETF { get; set; }
        public long 상한가 { get; set; }
        public long 하한가 { get; set; }
        public long 전일가 { get; set; }
        public string 주문수량단위 { get; set; }
        public long 기준가 { get; set; }
        public string 구분 { get; set; }
        public long 현재가 { get; set; }

    }

    public class 거래내역비교 : IEqualityComparer<거래내역>
    {

        public bool Equals(거래내역 x, 거래내역 y)
        {
            return x.종목명 == y.종목명;
        }

        public int GetHashCode(거래내역 obj)
        {
            return (obj.거래일자 + obj.거래유형 + obj.종목명 + +obj.외화수수료 + +obj.거래단가 + +obj.거래수량 + +obj.거래금액 +
                +obj.거래수량 + +obj.거래금액 + +obj.세금합계금액).GetHashCode();
        }
    }

    public class 거래내역
    {
        [DisplayName("거래일자")]
        public string 거래일자 { get; set; }
        [DisplayName("거래유형")]
        public string 거래유형 { get; set; }
        [DisplayName("종목명")]
        public string 종목명 { get; set; }
        [DisplayName("외화수수료")]
        public double 외화수수료 { get; set; }
        [DisplayName("거래단가")]
        public double 거래단가 { get; set; }
        [DisplayName("거래수량")]
        public long 거래수량 { get; set; }
        [DisplayName("거래금액")]
        public double 거래금액 { get; set; }
        [DisplayName("세금합계금액")]
        public double 세금합계금액 { get; set; }
    }
    public class 당일매도실현손익
    {
        [DisplayName("종목명")]
        public string 종목명 { get; set; }
        [DisplayName("추정실현손익")]
        public double 추정실현손익 { get; set; }
        [DisplayName("수익률")]
        public double 수익률 { get; set; }
        [DisplayName("매도수량")]
        public long 매도수량 { get; set; }
        [DisplayName("매도단가")]
        public int 매도단가 { get; set; }
        [DisplayName("매도금액")]
        public int 매도금액 { get; set; }
        [DisplayName("매도수수료")]
        public double 매도수수료 { get; set; }
        [DisplayName("제세금")]
        public double 제세금 { get; set; }
        [DisplayName("매입단가")]
        public int 매입단가 { get; set; }
        [DisplayName("매수금액")]
        public int 매수금액 { get; set; }
        [DisplayName("매수수수료")]
        public double 매수수수료 { get; set; }
    }

    public class ServerData
    {
        public int profit = 0;
        public int totalestimatedassets = 0;
        public int memberCount = 0;

        public static ServerData GetServerData()
        {
            ServerData serverData = new ServerData();

            try
            {
                string callUrl = "http://Moneymachinects-env-1.eba-49j29inb.ap-northeast-2.elasticbeanstalk.com/api/profit?";
                //string callUrl = "http://localhost:63768/api/profit?";

                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(callUrl); // 인코딩 UTF-8
                httpWebRequest.ContentType = "application/json; charset=UTF-8";
                httpWebRequest.Method = "GET";

                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.GetEncoding("UTF-8"));
                string responseString = streamReader.ReadToEnd();
                streamReader.Close();
                httpWebResponse.Close();
                if (responseString.IndexOf("false") < 0)
                {
                    serverData = JsonConvert.DeserializeObject<ServerData>(responseString);
                    return serverData;
                }
                else
                    throw new Exception("누적 수익금 조회 실패");
            }
            catch (Exception ex)
            {
                return serverData;
            }
        }
    }

    public class Order
    {
        [DisplayName("orderdate")]
        public string orderdate { get; set; }
        [DisplayName("expcode")]
        public string expcode { get; set; }
        [DisplayName("expname")]
        public string expname { get; set; }
        [DisplayName("price")]
        public int price { get; set; }
        [DisplayName("orderprice")]
        public int orderprice { get; set; }
        [DisplayName("cheprice")]
        public long cheprice { get; set; }
        [DisplayName("count")]
        public int count { get; set; }
    }

    public class OrderModel
    {
        [DisplayName("expertidx")]
        public int expertidx { get; set; }
        [DisplayName("expcode")]
        public string expcode { get; set; }
        [DisplayName("expname")]
        public string expname { get; set; }
        [DisplayName("orderprice")]
        public int orderprice { get; set; }
        [DisplayName("price")]
        public int price { get; set; }
        [DisplayName("orderdate")]
        public string orderdate { get; set; }
        [DisplayName("ordertype")]
        public string ordertype { get; set; }
        [DisplayName("status")]
        public bool status { get; set; }
        [DisplayName("ordercount")]
        public int ordercount { get; set; }
    }

    public class YieldModel
    {
        [DisplayName("idx")]
        public int idx { get; set; }
        [DisplayName("expertidx")]
        public int expertidx { get; set; }
        [DisplayName("expname")]
        public string expname { get; set; }
        [DisplayName("orderprice")]
        public int orderprice { get; set; }
        [DisplayName("price")]
        public int price { get; set; }
        [DisplayName("valuation")]
        public int valuation { get; set; }
        [DisplayName("yield")]
        public double yield { get; set; }
    }

    public class RatioModel
    {
        [JsonProperty("idx")]
        public int idx { get; set; }
        [JsonProperty("ratio")]
        public int ratio { get; set; }
    }

    public class ExpertModel
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
        [JsonProperty("companyidx")]
        public int companyidx { get; set; }
        [JsonProperty("profit")]
        public int profit { get; set; }
        [JsonProperty("totalestimatedassets")]
        public int totalestimatedassets { get; set; }
    }

    public class CompanyModel
    {
        [JsonProperty("idx")]
        public int idx { get; set; }
        [JsonProperty("name")]
        public string name { get; set; }
        [JsonProperty("id")]
        public string id { get; set; }
        [JsonProperty("pw")]
        public string pw { get; set; }
    }
}
