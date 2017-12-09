# Azure Function Source
Translate us -> ko
-------
Translate API : [인증주소](http://docs.microsofttranslator.com/oauth-token.html)

```

#r "System.Web"
using System.Net;
using System.Net.Http.Headers;
using System.Web;
using System.IO;
using System.Text;


public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log)
{
    log.Info("C# HTTP trigger function processed a request.");

    // parse query parameter
    string name = req.GetQueryNameValuePairs()
        .FirstOrDefault(q => string.Compare(q.Key, "name", true) == 0)
        .Value;

    // Get request body
    dynamic data = await req.Content.ReadAsAsync<object>();

    // Set name to query string or body data
    name = name ?? data?.name;

    if(name=="Azure"){
        name="Test Success";
    }  

    try
    {
        var uri = new Uri("https://api.microsofttranslator.com/v2/http.svc/Translate?text="+name+"&from=en&to=ko"); // string 을 Uri 로 형변환
        var wReq = (HttpWebRequest)WebRequest.Create(uri); // WebRequest 객체 형성 및 HttpWebRequest 로 형변환
        wReq.Method = "GET"; // 전송 방법 "GET" or "POST"
        wReq.Headers.Add("Authorization", "Bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJzY29wZSI6Imh0dHBzOi8vYXBpLm1pY3Jvc29mdHRyYW5zbGF0b3IuY29tLyIsInN1YnNjcmlwdGlvbi1pZCI6IjhiZDE3Y2FjODk2YjRkMGVhMjE4MTY4ZTA2ODQyMTFiIiwicHJvZHVjdC1pZCI6IlRleHRUcmFuc2xhdG9yLkYwIiwiY29nbml0aXZlLXNlcnZpY2VzLWVuZHBvaW50IjoiaHR0cHM6Ly9hcGkuY29nbml0aXZlLm1pY3Jvc29mdC5jb20vaW50ZXJuYWwvdjEuMC8iLCJhenVyZS1yZXNvdXJjZS1pZCI6Ii9zdWJzY3JpcHRpb25zLzJjYjg5YmU5LTQ0MjMtNDhiNC1hZmMzLTNjZGZmMDZmZmI0Zi9yZXNvdXJjZUdyb3Vwcy90cmFucy9wcm92aWRlcnMvTWljcm9zb2Z0LkNvZ25pdGl2ZVNlcnZpY2VzL2FjY291bnRzL3RyYW5zbGF0ZSIsImlzcyI6InVybjptcy5jb2duaXRpdmVzZXJ2aWNlcyIsImF1ZCI6InVybjptcy5taWNyb3NvZnR0cmFuc2xhdG9yIiwiZXhwIjoxNTEyODA2NzE0fQ.Iuyefe157nzPIQC2KYY2c4u8uhfjuIWhF4Pprk9jYtA");
        wReq.ServicePoint.Expect100Continue = false;
        wReq.CookieContainer = new CookieContainer(); 
        wReq.CookieContainer.SetCookies(uri, ""); // 넘겨줄 쿠키가 있을때 CookiContainer 에 저장
  
        using (var wRes = (HttpWebResponse)wReq.GetResponse())
        {
            Stream stream = wRes.GetResponseStream();
            StreamReader reader = new StreamReader(stream);
            name = reader.ReadToEnd();
            log.Info(name);
        }
    }
    catch (WebException ex)
    {
        if (ex.Status == WebExceptionStatus.ProtocolError && ex.Response != null)
        {
            var resp = (HttpWebResponse)ex.Response;
            if (resp.StatusCode == HttpStatusCode.NotFound)
            {
                // 예외 처리
            }
            else
            {
                // 예외 처리
            }
        }
        else
        {
            // 예외 처리
        }
  
    }

    return name == null
        ? req.CreateResponse(HttpStatusCode.BadRequest, "Please pass a name on the query string or in the request body")
        : req.CreateResponse(HttpStatusCode.OK, name);
}

```
