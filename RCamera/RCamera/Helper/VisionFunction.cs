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
        public HttpResponseMessage visionString(String temp)
        {
            var client = new HttpClient();
            string url = $"https://azurercamera.azurewebsites.net/api/AzureHttp?code=dmZuH7cMgJT1NxM1WaJGr0ahB5OI1qtiaGpKRH/6HEHU4dbnKWz4og==";
            dynamic dynamicJson = new ExpandoObject();
            dynamicJson.name = temp;
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(dynamicJson);
            HttpResponseMessage result = client.PostAsync(url, new StringContent(json, Encoding.UTF8, "application/json")).Result;

            return result;
        }       
    }
}