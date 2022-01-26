using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyMachineCTS
{
    public class log
    {
        [DisplayName("logtime")]
        public string logtime { get; set; }
        [DisplayName("logtype")]
        public string logtype { get; set; }
        [DisplayName("logmsg")]
        public string logmsg { get; set; }

        public log(DateTime date, string type, string msg)
        {
            logtime = date.ToString("HH:mm:ss");
            logtype = type;
            logmsg = msg;
        }
    }
}
