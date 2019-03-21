using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Web;

namespace xxoo.Common
{
    /* ############### ############### ############### ############### ############### ############### ###############
     * WebClient 请求的时候出现 “基础连接已经关闭: 发送时发生错误” 问题
        // 后来发现是安全协议问题
        //.net 4.0 设置： ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
        //.net 4.5 设置： ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls1.2;
     * 
      调用实例：
      HttpModel hm = new HttpModel();
      hm.Method = HttpMethod.Get;
      hm.Url = getTokenUrl;
      string response = HttpHelper.SendRequest(hm);

     *******/

    public class HttpHelper
    {
        public event Func<string,string> GetFilterText;

        /// <summary>
        /// 构造http请求发送 
        /// </summary>
        /// <param name="hm"></param>
        /// <returns>返回文本信息</returns>
        public static string SendRequestTextCustom(HttpModel hm,ref System.Text.Encoding encod)
        {
            Stream resStream = null;
            StreamReader readStream = null;
            HttpWebRequest request = null;
            string content = string.Empty;
            HttpWebResponse httpWebResponse = null;
            try
            {
                if (hm.Url.IndexOf("http://") == -1 && hm.Url.IndexOf("https://") == -1)
                {
                    hm.Url = "http://" + hm.Url;
                }

                if (hm.Method == HttpMethod.Get)
                {
                    #region GET
                    request = WebRequest.Create(hm.Url) as HttpWebRequest;
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.Referer = hm.Url;
                    request.Headers.Set("Pragma", "no-cache");
                    request.Accept = "text/html, application/xhtml+xml, */*";
                    request.Headers.Set("Accept-Language", "zh-CN");
                    request.Headers.Set("Accept-Encoding", "gzip, deflate");
                    request.UserAgent = "Mozilla-Firefox-Spider(Wenanry)";
                    if (System.Web.HttpContext.Current.Request.Headers["Cookie"] != null)
                    {
                        request.Headers.Add("Cookie", System.Web.HttpContext.Current.Request.Headers["Cookie"]);
                    }
                    #endregion
                }
                else
                {
                    #region POST
                    System.GC.Collect();
                    request = WebRequest.Create(hm.Url) as HttpWebRequest;
                    Encoding encoding = Encoding.UTF8;
                    request.Method = "Post";
                    //request.Timeout = Convert.ToInt32(500);
                    if (System.Web.HttpContext.Current.Request.Headers["Cookie"] != null)
                    {
                        request.Headers.Add("Cookie", System.Web.HttpContext.Current.Request.Headers["Cookie"]);
                    }
                    request.Referer = "https://music.liuzhijin.cn/?name=Take%20Me%20To%20Infinity&type=ximalaya";
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.Accept = "application/json, text/javascript, */*; q=0.01";
                    request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/72.0.3626.121 Safari/537.36";
                    //request.ServicePoint.Expect100Continue = false;
                    request.KeepAlive = false;
                    request.ServicePoint.ConnectionLimit = 200;

                    if (hm.Heads != null && hm.Heads.Count != 0)
                    {
                        hm.Heads.ForEach(p =>
                        {
                            request.Headers.Add(p.Name, p.Value);
                        });
                    }

                    if (!(hm.Params == null || hm.Params.Count == 0))
                    {
                        StringBuilder buffer = new StringBuilder();
                        int i = 0;
                        foreach (HttpParams item in hm.Params)
                        {
                            if (i > 0)
                            {
                                buffer.AppendFormat("&{0}={1}", item.Name, item.Value);
                            }
                            else
                            {
                                buffer.AppendFormat("{0}={1}", item.Name, item.Value);
                            }
                            i++;
                        }

                        byte[] data = encoding.GetBytes(buffer.ToString());
                        request.ContentLength = data.Length;
                        using (Stream stream = request.GetRequestStream())
                        {
                            stream.Write(data, 0, data.Length);
                            stream.Close();
                            stream.Dispose();
                        }
                    }
                    #endregion
                }

                httpWebResponse = request.GetResponse() as HttpWebResponse;
                resStream = httpWebResponse.GetResponseStream();
                if (httpWebResponse.ContentEncoding.ToLower().Contains("gzip"))
                { // 响应流 gzip 解压
                    resStream = new GZipStream(resStream, CompressionMode.Decompress);
                }
                string contentEncoding = httpWebResponse.CharacterSet;
                //   string contentType = httpWebResponse.ContentType;
             
                if (encod == null)
                { // 取默认编码格式
                    encod = System.Text.Encoding.GetEncoding(contentEncoding);
                }
                readStream = new StreamReader(resStream, encod);
                content = readStream.ReadToEnd();
            }
            catch (Exception ex)
            {
                if (request == null)
                {
                    return ex.Message;
                }
                // 判断是否接受到相应值
                if (!request.HaveResponse)
                {
                    return "未能接收到服务器响应。";
                }
                else
                {
                    // 
                    if (ex.Message == "远程服务器返回错误: (404) 未找到。")
                    {
                        return string.Format("{0}请确保URL和请求方法正确！", ex.Message);
                    }
                    return ex.Message;
                }
            }
            finally
            {
                if (resStream != null)
                {
                    resStream.Close();
                }
                if (readStream != null)
                {
                    readStream.Close();
                }
            }
            return content;
        }
        public static string SendRequestGetText(HttpModel hm, System.Text.Encoding encod)
        {
            // System.Text.Encoding encod = null;
            return SendRequestTextCustom(hm, ref encod);
        }
        public static string SendRequestGetText(HttpModel hm)
        {
            System.Text.Encoding encod = null;
            return SendRequestTextCustom(hm, ref encod);
        }

        public static string SendRequestFormGetText(HttpModel hm)
        {
            System.Text.Encoding encod = null;
            return SendRequestForm(hm, ref encod);
        }
        /// <summary>
        /// 构造form http请求发送 
        /// </summary>
        /// <param name="hm"></param>
        /// <returns>返回文本信息</returns>
        public static string SendRequestForm(HttpModel hm, ref System.Text.Encoding encod)
        {
            Stream resStream = null;
            StreamReader readStream = null;
            HttpWebRequest request = null;
            string content = string.Empty;
            HttpWebResponse httpWebResponse = null;
            try
            {
                if (hm.Url.IndexOf("http://") == -1 && hm.Url.IndexOf("https://") == -1)
                {
                    hm.Url = "http://" + hm.Url;
                }

                
                #region POST
                request = WebRequest.Create(hm.Url) as HttpWebRequest;
                Encoding encoding = Encoding.UTF8;
                request.Method = "Post";
                //request.Timeout = Convert.ToInt32(500);
                if (System.Web.HttpContext.Current.Request.Headers["Cookie"] != null)
                {
                    request.Headers.Add("Cookie", System.Web.HttpContext.Current.Request.Headers["Cookie"]);
                }
                request.Timeout = 10000;
                request.Headers.Add("Cache-Control", "no-cache");
                request.Headers.Add("x-requested-with", "XMLHttpRequest");
                string bstr = "-------------------" + DateTime.Now.Ticks.ToString("x");
                request.ContentType = "multipart/form-data; boundary=" + bstr;
                request.ServicePoint.Expect100Continue = false;

                if (!(hm.Params == null || hm.Params.Count == 0))
                {
                    StringBuilder buffer = new StringBuilder();
                    int i = 0;
                    foreach (HttpParams item in hm.Params)
                    {
                        buffer.AppendFormat("\r\n{0}\r\nContent-Disposition: form-data; name=\"{1}\"\r\n\r\n{2}\r\n", bstr, item.Name, item.Value);
                        i++;
                    }
                    buffer.AppendFormat("\r\n{0}", bstr);
                    byte[] data = encoding.GetBytes(buffer.ToString());
                    request.ContentLength = data.Length;
                    using (Stream stream = request.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }
                }

                httpWebResponse = request.GetResponse() as HttpWebResponse;
                resStream = httpWebResponse.GetResponseStream();
                if (httpWebResponse.ContentEncoding.ToLower().Contains("gzip"))
                { // 响应流 gzip 解压
                    resStream = new GZipStream(resStream, CompressionMode.Decompress);
                }
                string contentEncoding = httpWebResponse.CharacterSet;
                //   string contentType = httpWebResponse.ContentType;

                if (encod == null)
                { // 取默认编码格式
                    encod = System.Text.Encoding.GetEncoding(contentEncoding);
                }
                readStream = new StreamReader(resStream, encod);
                content = readStream.ReadToEnd();
            }
            catch (Exception ex)
            {
                if (request == null)
                {
                    return ex.Message;
                }
                // 判断是否接受到相应值
                if (!request.HaveResponse)
                {
                    return "未能接收到服务器响应。";
                }
                else
                {
                    // 
                    if (ex.Message == "远程服务器返回错误: (404) 未找到。")
                    {
                        return string.Format("{0}请确保URL和请求方法正确！", ex.Message);
                    }
                    return ex.Message;
                }
            }
            finally
            {
                if (resStream != null)
                {
                    resStream.Close();
                }
                if (readStream != null)
                {
                    readStream.Close();
                }
            }
            return content;
        }


        /// <summary>
        /// 请求url图片并保存 
        /// </summary>
        /// <param name="hm"></param>
        /// <param name="fileDic">图片保存路径(不含扩展名)</param>
        /// <returns>返回图片路径或文本信息</returns>
        public static string SendRequestFileCustomPath(HttpModel hm, ref System.Text.Encoding encod, string fileDic)
        {
            Stream resStream = null;
            StreamReader readStream = null;
            HttpWebRequest request = null;
            string content = string.Empty;
            HttpWebResponse httpWebResponse = null;
            try
            {
                if (hm.Url.IndexOf("http://") == -1 && hm.Url.IndexOf("https://") == -1)
                {
                    hm.Url = "http://" + hm.Url;
                }

                if (hm.Method == HttpMethod.Get)
                {
                    #region GET
                    request = WebRequest.Create(hm.Url) as HttpWebRequest;
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.Referer = hm.Url;
                    request.Headers.Set("Pragma", "no-cache");
                    request.Accept = "text/html, application/xhtml+xml, */*";
                    request.Headers.Set("Accept-Language", "zh-CN");
                    request.Headers.Set("Accept-Encoding", "gzip, deflate");
                    request.UserAgent = "Mozilla-Firefox-Spider(Wenanry)";
                    if (System.Web.HttpContext.Current.Request.Headers["Cookie"] != null)
                    {
                        request.Headers.Add("Cookie", System.Web.HttpContext.Current.Request.Headers["Cookie"]);
                    }
                    #endregion
                }
                else
                {
                    #region POST
                    request = WebRequest.Create(hm.Url) as HttpWebRequest;
                    Encoding encoding = Encoding.UTF8;
                    request.Referer = hm.Url;
                    request.Accept = "text/html, application/xhtml+xml, */*";
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.UserAgent = "Mozilla-Firefox-Spider(Wenanry)";
                    request.ServicePoint.Expect100Continue = false;
                    request.Method = "Post";
                    //request.Timeout = Convert.ToInt32(500);
                    if (System.Web.HttpContext.Current.Request.Headers["Cookie"] != null)
                    {
                        request.Headers.Add("Cookie", System.Web.HttpContext.Current.Request.Headers["Cookie"]);
                    }

                    if (!(hm.Params == null || hm.Params.Count == 0))
                    {
                        StringBuilder buffer = new StringBuilder();
                        int i = 0;
                        foreach (HttpParams item in hm.Params)
                        {
                            if (i > 0)
                            {
                                buffer.AppendFormat("&{0}={1}", item.Name, item.Value);
                            }
                            else
                            {
                                buffer.AppendFormat("&{0}={1}", item.Name, item.Value);
                            }
                            i++;
                        }
                        byte[] data = encoding.GetBytes(buffer.ToString());
                        request.ContentLength = data.Length;
                        using (Stream stream = request.GetRequestStream())
                        {
                            stream.Write(data, 0, data.Length);
                        }
                    }
                    #endregion
                } 
                httpWebResponse = request.GetResponse() as HttpWebResponse;
                resStream = httpWebResponse.GetResponseStream();
                if (httpWebResponse.ContentEncoding.ToLower().Contains("gzip"))
                { // 响应流 gzip 解压
                    resStream = new GZipStream(resStream, CompressionMode.Decompress);
                }
                string contentEncoding = httpWebResponse.CharacterSet;
                string contentType = httpWebResponse.ContentType;
                Dictionary<string, string> imgdic = new Dictionary<string, string>();
                imgdic.Add("image/bmp", "bmp");
                imgdic.Add("image/gif", "gif");
                imgdic.Add("image/ief", "ief");
                imgdic.Add("image/jpeg", "jpg");
                imgdic.Add("image/png", "png");
                imgdic.Add("image/tiff", "tiff");
                imgdic.Add("image/vnd.djvu", "djv");
                imgdic.Add("image/vnd.wap.wbmp", "wbmp");
                imgdic.Add("image/x-cmu-raster", "ras");
                imgdic.Add("image/x-portable-bitmap", "pbm");
                imgdic.Add("image/x-portable-graymap", "pgm");
                imgdic.Add("image/x-portable-pixmap", "ppm");
                imgdic.Add("image/x-rgb", "rgb");
                imgdic.Add("image/x-xbitmap", "xbm");
                imgdic.Add("image/x-xpixmap", "xpm");
                imgdic.Add("image/x-xwindowdump", "xwd"); 

                if (imgdic.ContainsKey(contentType))
                {
                    string sitefile = fileDic + "." + imgdic[contentType];
                    string path = System.Web.HttpContext.Current.Server.MapPath(sitefile);
                    Directory.CreateDirectory(Path.GetDirectoryName(path));

                    using (FileStream fsWrite = new FileStream(path, FileMode.Create, FileAccess.Write))
                    {
                        byte[] buffer = new byte[1024 * 1024];
                        int i = 0;
                        while ((i = resStream.Read(buffer, 0, buffer.Length)) != 0)
                        {
                            fsWrite.Write(buffer, 0, i);
                        }
                    }
                    content = sitefile;
                }
                else
                {
                    if (encod == null)
                    { // 取默认编码格式
                        encod = System.Text.Encoding.GetEncoding(contentEncoding);
                    }
                    readStream = new StreamReader(resStream, encod);
                    content = readStream.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                if (request == null)
                {
                    return ex.Message;
                }
                // 判断是否接受到相应值
                if (!request.HaveResponse)
                {
                    return "未能接收到服务器响应。";
                }
                else
                {
                    // 
                    if (ex.Message == "远程服务器返回错误: (404) 未找到。")
                    {
                        return string.Format("{0}请确保URL和请求方法正确！", ex.Message);
                    }
                    return ex.Message;
                }
            }
            finally
            {
                if (resStream != null)
                {
                    resStream.Close();
                }
                if (readStream != null)
                {
                    readStream.Close();
                }
            }
            return content;
        }
        public static string SendRequestGetFile(HttpModel hm, ref System.Text.Encoding encod, string fileDic = "/SendRequest/Images/")
        {
            fileDic = fileDic + DateTime.Now.ToString("yyyy_MM_dd") + Guid.NewGuid().ToString();

            return SendRequestFileCustomPath(hm,ref encod, fileDic);

        }
        public static string SendRequestGetFile(HttpModel hm, string fileDic = "/SendRequest/Images/")
        {
            System.Text.Encoding encod = null;
            return SendRequestFileCustomPath(hm, ref encod, fileDic);
        }

        /// <summary>
        /// 构造http请求发送 
        /// </summary>
        /// <param name="hm"></param>
        /// <param name="encod"></param>
        /// <returns>返回文本信息或图片路径</returns>
        public static string SendHttpCustom(HttpModel hm, out System.Text.Encoding encod)
        {
            encod = null;
            if (string.IsNullOrEmpty(hm.Url))
                return "URL不能为空！}";

            string result = null;
            if (hm.EncodingType != HttpEncoding.Auto)
            { // 默认编码
                encod = System.Text.Encoding.GetEncoding((int)hm.EncodingType);
                result = HttpHelper.SendRequestGetText(hm, encod);
            }
            else if (hm.ResultType == HttpResult.Auto)
            { // 默认响应内容类型
                result = HttpHelper.SendRequestGetFile(hm,ref encod);
            }
            else if (hm.ResultType == HttpResult.Text)
            { // 内容
                result = HttpHelper.SendRequestTextCustom(hm, ref encod);
            }
            else if (hm.ResultType == HttpResult.File)
            { // 文件
                result = HttpHelper.SendRequestGetFile(hm);
            }

            return result;
        }
        public static string SendHttp(HttpModel hm, System.Text.Encoding encod = null)
        {
            return SendHttpCustom(hm,out encod);
        }

        public string SendHttpCustomEvent(HttpModel hm, out System.Text.Encoding encod)
        {
            encod = null;
            if (string.IsNullOrEmpty(hm.Url))
                return "URL不能为空！}";

            string result = null;
            if (hm.EncodingType != HttpEncoding.Auto)
            { // 默认编码
                encod = System.Text.Encoding.GetEncoding((int)hm.EncodingType);
                result = HttpHelper.SendRequestGetText(hm, encod);
            }
            else if (hm.ResultType == HttpResult.Auto)
            { // 默认响应内容类型
                result = HttpHelper.SendRequestGetFile(hm, ref encod);
            }
            else if (hm.ResultType == HttpResult.Text)
            { // 内容
                result = HttpHelper.SendRequestTextCustom(hm, ref encod);
            }
            else if (hm.ResultType == HttpResult.File)
            { // 文件
                result = HttpHelper.SendRequestGetFile(hm);
            }

            if (GetFilterText != null)
            { // 对返回数据过滤
                result = this.GetFilterText(result);
            }


            return result;
        }

		
        // 下载文件
        public static bool DownLoadFile(HttpModel hm,string localPath)
        {
            try{
                WebClient wc = new WebClient();
                wc.DownloadFile(hm.Url, localPath);
                    return true;
            }
            catch(Exception ex)
            { 
                return false;
            }
        }



        /// <summary>
        /// 第2种  ############### ############### ############### ############### ############### ############### ###############
        /// 构造formdata请求 copy过来的
        /// 
        ///  NameValueCollection nv = new NameValueCollection();
        ///  nv.Add("input", "Take Me To Infinity");
        ///  nv.Add("filter", "name");
        ///  nv.Add("type", "ximalaya");
        ///  nv.Add("page", "1");
        ///  string response = HttpHelper.HttpPostFormData("https://music.liuzhijin.cn/", 10000, null, null, nv);
        /// </summary> 
        /// <returns></returns>
        public static string HttpPostFormData(string url, int timeOut, string fileKeyName,
                                    string filePath, NameValueCollection stringDict)
        {
            string responseContent;
            var memStream = new MemoryStream();
            var webRequest = (HttpWebRequest)WebRequest.Create(url);
            // 边界符  
            var boundary = "---------------" + DateTime.Now.Ticks.ToString("x");
            // 边界符  
            var beginBoundary = Encoding.ASCII.GetBytes("--" + boundary + "\r\n");
            //var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            // 最后的结束符  
            var endBoundary = Encoding.ASCII.GetBytes("--" + boundary + "--\r\n");

            // 设置属性  
            webRequest.Method = "POST";
            webRequest.Timeout = timeOut;
            webRequest.ContentType = "multipart/form-data; boundary=" + boundary;
            webRequest.Headers.Add("x-requested-with", "XMLHttpRequest");

            //// 写入文件  
            //const string filePartHeader =
            //    "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\n" +
            //     "Content-Type: application/octet-stream\r\n\r\n";
            //var header = string.Format(filePartHeader, fileKeyName, filePath);
            //var headerbytes = Encoding.UTF8.GetBytes(header);

            //memStream.Write(beginBoundary, 0, beginBoundary.Length);
            //memStream.Write(headerbytes, 0, headerbytes.Length);

            //var buffer = new byte[1024];
            //int bytesRead; // =0  

            //while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
            //{
            //    memStream.Write(buffer, 0, bytesRead);
            //}
            var contentLine = Encoding.ASCII.GetBytes("\r\n"); memStream.Write(contentLine, 0, contentLine.Length);
            // 写入字符串的Key  
            var stringKeyHeader = "\r\n--" + boundary +
                                   "\r\nContent-Disposition: form-data; name=\"{0}\"" +
                                   "\r\n\r\n{1}\r\n";

            foreach (byte[] formitembytes in from string key in stringDict.Keys
                                             select string.Format(stringKeyHeader, key, stringDict[key])
                                                 into formitem
                                             select Encoding.UTF8.GetBytes(formitem))
            {
                memStream.Write(formitembytes, 0, formitembytes.Length);
            }

            // 写入最后的结束边界符  
            memStream.Write(endBoundary, 0, endBoundary.Length);

            webRequest.ContentLength = memStream.Length;

            Stream requestStream = null;

            try
            {
                requestStream = webRequest.GetRequestStream();
            }
            catch (Exception ex)
            {

                throw;
            }

            memStream.Position = 0;
            var tempBuffer = new byte[memStream.Length];
            memStream.Read(tempBuffer, 0, tempBuffer.Length);
            memStream.Close();

            requestStream.Write(tempBuffer, 0, tempBuffer.Length);
            requestStream.Close();

            var httpWebResponse = (HttpWebResponse)webRequest.GetResponse();

            using (var httpStreamReader = new StreamReader(httpWebResponse.GetResponseStream(),
                                                            Encoding.GetEncoding("utf-8")))
            {
                responseContent = httpStreamReader.ReadToEnd();
            }

            //fileStream.Close();
            httpWebResponse.Close();
            webRequest.Abort();

            return responseContent;
        }

    }
    public class HttpHelper<T> where T : class,new()
    {
        public event Func<string, T> GetFilterText; 
        public T SendHttpCustomEvent<T>(HttpModel hm, out System.Text.Encoding encod) where T : class,new()
        {
            T t = new T();
            string result = HttpHelper.SendHttpCustom(hm,out encod);


            if (this.GetFilterText != null)
            { // 对返回数据过滤
                t = this.GetFilterText(result) as T;
            }
            return t;
        } 
    }
	
	public class HttpModel
    {
        /// <summary>
        /// 响应内容类别
        /// </summary>
        public HttpResult ResultType { get; set; }
        /// <summary>
        /// 编码格式
        /// </summary>
        public HttpEncoding EncodingType { get; set; }
        /// <summary>
        /// get / post
        /// </summary>
        public HttpMethod Method { get; set; }
        /// <summary>
        /// 请求url
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 请求参数
        /// </summary>
        public List<HttpParams> Params { get; set; }

        /// <summary>
        /// 请求参数
        /// </summary>
        public List<HttpParams> Heads { get; set; }
    }
    /// <summary>
    /// 请求参数
    /// </summary>
    public class HttpParams
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
    /// <summary>
    /// 请求方法
    /// </summary>
    public enum HttpMethod
    { 
        Get,
        Post,
        Form,
    }
    /// <summary>
    /// 请求编码
    /// </summary>
    public enum HttpEncoding
    {
       Auto = -1,
       UTF8 = 65001,
       ASCII = 20127,
       Unicode = 1200,
       GB2312 = 936
    }
    /// <summary>
    /// 请求文件
    /// </summary>
    public enum HttpResult
    {
        Auto,
        Text,
        File
    }




    // ############### ############### ############### ############### ############### ############### ###############
    #region 第3种 copy过来的 
    /*
      // 调用示例
            //List<FormItemModel> lstPara = new List<FormItemModel>();
            //FormItemModel model = new FormItemModel();
            //model = new FormItemModel();
            //model.Key = "input";
            //model.Value = "Take Me To Infinity";
            //lstPara.Add(model);

            //model = new FormItemModel();
            //model.Key = "filter";
            //model.Value = "name";
            //lstPara.Add(model);

            //model = new FormItemModel();
            //model.Key = "type";
            //model.Value = "ximalaya";
            //lstPara.Add(model);

            //model = new FormItemModel();
            //model.Key = "page";
            //model.Value = "1";
            //lstPara.Add(model);

            //string response = FormRequest.PostForm("https://music.liuzhijin.cn/", lstPara);
         */

    /// <summary>  
    /// 表单数据项  
    /// </summary>  
    public class FormItemModel
    {
        /// <summary>  
        /// 表单键，request["key"]  
        /// </summary>  
        public string Key { set; get; }
        /// <summary>  
        /// 表单值,上传文件时忽略，request["key"].value  
        /// </summary>  
        public string Value { set; get; }
        /// <summary>  
        /// 是否是文件  
        /// </summary>  
        public bool IsFile
        {
            get
            {
                if (FileContent == null || FileContent.Length == 0)
                    return false;

                if (FileContent != null && FileContent.Length > 0 && string.IsNullOrWhiteSpace(FileName))
                    throw new Exception("上传文件时 FileName 属性值不能为空");
                return true;
            }
        }
        /// <summary>  
        /// 上传的文件名  
        /// </summary>  
        public string FileName { set; get; }
        /// <summary>  
        /// 上传的文件内容  
        /// </summary>  
        public Stream FileContent { set; get; }
    }
    public class FormRequest
    {
        // https
        private static readonly string DefaultUserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受     
        }
        /// <summary>  
        /// 使用Post方法获取字符串结果  
        /// </summary>  
        /// <param name="url"></param>  
        /// <param name="formItems">Post表单内容</param>  
        /// <param name="cookieContainer"></param>  
        /// <param name="timeOut">默认20秒</param>  
        /// <param name="encoding">响应内容的编码类型（默认utf-8）</param>  
        /// <returns></returns>  
        public static string PostForm(string url, List<FormItemModel> formItems, CookieContainer cookieContainer = null, string refererUrl = null, Encoding encoding = null, int timeOut = 20000)
        {
            HttpWebRequest request = null;
            //HTTPSQ请求  
            if (url.Substring(0, 8) == "https://")
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            }

            request = WebRequest.Create(url) as HttpWebRequest;
            request.ProtocolVersion = HttpVersion.Version10;
            #region 初始化请求对象
            request.Method = "POST";
            request.Timeout = timeOut;
            //request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            request.KeepAlive = true;
            //request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.57 Safari/537.36";
            if (!string.IsNullOrEmpty(refererUrl))
                request.Referer = refererUrl;
            if (cookieContainer != null)
                request.CookieContainer = cookieContainer;
            #endregion

            string boundary = "----" + DateTime.Now.Ticks.ToString("x");//分隔符  
            request.ContentType = string.Format("multipart/form-data; boundary={0}", boundary);
            //请求流  
            var postStream = new MemoryStream();
            #region 处理Form表单请求内容
            //是否用Form上传文件  
            var formUploadFile = formItems != null && formItems.Count > 0;
            if (formUploadFile)
            {
                //文件数据模板  
                string fileFormdataTemplate =
                    "\r\n--" + boundary +
                    "\r\nContent-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"" +
                    "\r\nContent-Type: multipart/form-data" +
                    "\r\n\r\n";
                //文本数据模板  
                string dataFormdataTemplate =
                    "\r\n--" + boundary +
                    "\r\nContent-Disposition: form-data; name=\"{0}\"" +
                    "\r\n\r\n{1}";
                foreach (var item in formItems)
                {
                    string formdata = null;
                    if (item.IsFile)
                    {
                        //上传文件  
                        formdata = string.Format(
                            fileFormdataTemplate,
                            item.Key, //表单键  
                            item.FileName);
                    }
                    else
                    {
                        //上传文本  
                        formdata = string.Format(
                            dataFormdataTemplate,
                            item.Key,
                            item.Value);
                    }

                    //统一处理  
                    byte[] formdataBytes = null;
                    //第一行不需要换行  
                    if (postStream.Length == 0)
                        formdataBytes = Encoding.UTF8.GetBytes(formdata.Substring(2, formdata.Length - 2));
                    else
                        formdataBytes = Encoding.UTF8.GetBytes(formdata);
                    postStream.Write(formdataBytes, 0, formdataBytes.Length);

                    //写入文件内容  
                    if (item.FileContent != null && item.FileContent.Length > 0)
                    {
                        using (var stream = item.FileContent)
                        {
                            byte[] buffer = new byte[1024];
                            int bytesRead = 0;
                            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
                            {
                                postStream.Write(buffer, 0, bytesRead);
                            }
                        }
                    }
                }
                //结尾  
                var footer = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");
                postStream.Write(footer, 0, footer.Length);

            }
            else
            {
                request.ContentType = "application/json;charset=UTF-8";
            }
            #endregion

            request.ContentLength = postStream.Length;

            #region 输入二进制流
            if (postStream != null)
            {
                postStream.Position = 0;
                Stream requestStream = null;
                //直接写入流  
                try
                {

                    requestStream = request.GetRequestStream();
                }
                catch (Exception ex)
                {

                    throw;
                }

                byte[] buffer = new byte[1024];
                int bytesRead = 0;
                while ((bytesRead = postStream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    requestStream.Write(buffer, 0, bytesRead);
                }

                ////debug  
                //postStream.Seek(0, SeekOrigin.Begin);  
                //StreamReader sr = new StreamReader(postStream);  
                //var postStr = sr.ReadToEnd();  
                postStream.Close();//关闭文件访问  
            }
            #endregion

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            if (cookieContainer != null)
            {
                response.Cookies = cookieContainer.GetCookies(response.ResponseUri);
            }

            using (Stream responseStream = response.GetResponseStream())
            {
                using (StreamReader myStreamReader = new StreamReader(responseStream, encoding ?? Encoding.UTF8))
                {
                    string retString = myStreamReader.ReadToEnd();
                    return retString;
                }
            }
        }
    }



    #endregion



    //第4种 webclient形式 ############### ############### ############### ############### ############### ############### ###############
    /*
    //string URLAuth = "https://music.liuzhijin.cn/";
    //WebClient webClient = new WebClient();

    //NameValueCollection formData = new NameValueCollection();
    //formData["input"] = "Take Me To Infinity";
    //formData["filter"] = "name";
    //formData["type"] = "ximalaya";
    //formData["page"] = "1";
    //string response = null;
    //try
    //{
    //    webClient.Headers["Content-Typ"] = "application/x-www-form-urlencoded";
    //    webClient.Headers["x-requested-with"] = "XMLHttpRequest";
    //    byte[] responseBytes = webClient.UploadValues(URLAuth, "POST", formData);
    //    response = Encoding.UTF8.GetString(responseBytes);
    //}
    //catch (Exception exx)
    //{

    //    throw;
    //}
    //webClient.Dispose();
     */





    #endregion
}