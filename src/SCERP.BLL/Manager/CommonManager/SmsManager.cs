using SCERP.BLL.IManager.ICommonManager;
using SCERP.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.BLL.Manager.CommonManager
{
    public class SmsManager : ISmsManager
    {
        public bool Send(string mobileNumber, string message)
        {
            //do not use single quotation (') in the message to avoid forbidden result
            message = System.Uri.EscapeUriString(message);
            string url = string.Format("{0}?username={1}&password={2}&number={3}&senderid={4}&message={5}", AppConfig.SmsGatewayUrl, AppConfig.SmsGatewayUserId, AppConfig.SmsGatewayPassword, mobileNumber, AppConfig.SmsSenderName, message);

            //url = "http://66.45.237.70/maskingapi.php?username=" + userid +
            //    "&password=" + password + "&number=" + mobileNumber + "&senderid=" + senderId + "&message=" + message;

            //String url = "http://66.45.237.70/api.php?username="
            //             + userid + "&password=" + password + "&number=" + number + "&message=" + message;
            WebRequest request = WebRequest.Create(url); // Send the 'HttpWebRequest' and wait for response.
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
           
            if(response.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }

            return false;
        }

        public bool Send(List<string> mobileNumbers, string message)
        {
            //do not use single quotation (') in the message to avoid forbidden result
            message = System.Uri.EscapeUriString(message);

            string mobileNumber = string.Join(",", mobileNumbers);

            string url = string.Format("{0}?username={1}&password={2}&number={3}&senderid={4}&message={5}", AppConfig.SmsGatewayUrl, AppConfig.SmsGatewayUserId, AppConfig.SmsGatewayPassword, mobileNumber, AppConfig.SmsSenderName, message);

            //url = "http://66.45.237.70/maskingapi.php?username=" + userid +
            //    "&password=" + password + "&number=" + mobileNumber + "&senderid=" + senderId + "&message=" + message;

            //String url = "http://66.45.237.70/api.php?username="
            //             + userid + "&password=" + password + "&number=" + number + "&message=" + message;
            WebRequest request = WebRequest.Create(url); // Send the 'HttpWebRequest' and wait for response.
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }

            return false;
        }
    }
}
