using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Net.Http;
using System.Dynamic;

namespace RCamera.Helper
{
    public class Web
    {        
        /// <summary>
        /// Web에 영문 string 값 전송하고 번역한 string을 받아옴
        /// </summary>
        /// <param name="temp"></param>
        /// <returns></returns>
        public HttpResponseMessage visionString(string temp)
        {
            var client = new HttpClient();
            string url = $"https://transazure.azurewebsites.net/api/suzi?code=voyMSLnIjuEMCWtsL8v8pShabMytT7Sw8841oX/eXb3LMNOZesI8Sg==";
            dynamic dynamicJson = new ExpandoObject();
            dynamicJson.name = temp;
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(dynamicJson);
            HttpResponseMessage result = client.PostAsync(url, new StringContent(json, Encoding.UTF8, "application/json")).Result;

            return result;
        }       
    }
}