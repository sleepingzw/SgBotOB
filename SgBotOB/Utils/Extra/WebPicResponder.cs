using Microsoft.Extensions.Logging;
using SgBotOB.Data;
using SgBotOB.Utils.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SgBotOB.Utils.Extra
{
    internal class WebPicResponder
    {
        private const string Port = "1145";//port

        public WebPicResponder()
        {
            var listener = new HttpListener();
            listener.Prefixes.Add("http://+:" + Port + "/");

            listener.Start();
            listener.BeginGetContext(ListenerHandle, listener);
        }
        private static void ListenerHandle(IAsyncResult result)
        {
            try
            {
                var listener = (HttpListener)result.AsyncState!;
                if (!listener.IsListening) return;
                listener.BeginGetContext(ListenerHandle, listener);
                var context = listener.EndGetContext(result);
                //解析Request请求
                var request = context.Request;

                //构造Response响应
                if (!request.RawUrl!.StartsWith("/w?")) return;
                GetPics(request.RawUrl, context.Response, !string.IsNullOrEmpty(request.Headers.Get("If-Modified-Since")));
                Logger.Log("开始返回图片", 0);
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, 1);
            }

        }
        private static void GetPics(string url, HttpListenerResponse response, bool hasIfModifiedSince)
        {
            if (hasIfModifiedSince)
            {
                response.StatusCode = 304;
                response.Close();
                return;
            }

            response.StatusCode = 200;
            response.ContentType = "image/jpeg";
            response.ContentEncoding = Encoding.UTF8;
            response.AddHeader("Last-Modified", "Wed, 21 Oct 2000 12:00:00 GMT");
            var cardIdStr = url.Split('?').Last();

            var fileName = Path.Combine(StaticData.ExePath!, "Data/Img/Setu/" + cardIdStr + "/" + "pic.jpg");

            if (!File.Exists(fileName))
            {
                NoFound(url, response);
                Logger.Log($"{fileName}不存在", 1);
                return;
            }

            using (var picFile = File.OpenRead(fileName))
                picFile.CopyTo(response.OutputStream);

            response.Close();
        }
        private static void NoFound(string url, HttpListenerResponse response)
        {
            response.StatusCode = 404;
            response.ContentType = "text/html;charset=UTF-8";
            response.ContentEncoding = Encoding.UTF8;
            using (var writer = new StreamWriter(response.OutputStream, Encoding.UTF8))
                writer.Write(url + " not found");
            response.Close();
        }
    }
}
