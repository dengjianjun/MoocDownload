using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MoocDownload
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("请输入存放的路径...");
            var path = Console.ReadLine();
            if (path[path.Length - 1] != '\\')
            {
                path += '\\';
            }
            var videoes = GetWebsite();
            Console.WriteLine("开始下载视频！");
            videoes.ForEach(v =>
            {
                HttpDownloadFile(v.Item1, path + v.Item2);
                Console.WriteLine("视频【{0}】下载完成...", v.Item2);
            });
            Console.WriteLine("ok");
            Console.ReadKey();
        }

        static string _videoPath = ConfigurationManager.AppSettings["VideoPath"];

        /// <summary>
        /// Http下载文件
        /// </summary>
        public static string HttpDownloadFile(string url, string path)
        {

            // 设置参数

            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            //发送请求并获取相应回应数据

            HttpWebResponse response = request.GetResponse() as HttpWebResponse;

            //直到request.GetResponse()程序才开始向目标网页发送Post请求
            Stream responseStream = response.GetResponseStream();
            //创建本地文件写入流

            Stream stream = new FileStream(path, FileMode.Create);
            byte[] bArr = new byte[1024];

            int size = responseStream.Read(bArr, 0, (int)bArr.Length);

            while (size > 0)
            {

                stream.Write(bArr, 0, size);

                size = responseStream.Read(bArr, 0, (int)bArr.Length);

            }

            stream.Close();

            responseStream.Close();

            return path;

        }

        /// <summary>
        /// 获取下载站点
        /// </summary>
        /// <returns></returns>
        public static List<Tuple<string, string>> GetWebsite()
        {
            var siteInfos = new List<Tuple<string, string>>();
            XDocument xdoc = XDocument.Load(_videoPath);
            XElement root = xdoc.Root;
            var sites = root.Elements();
            siteInfos =
                sites.Select(x => new Tuple<string, string>(x.Element("url").Value, x.Element("name").Value.Replace("\\n", ""))).ToList();
            return siteInfos;
        }
    }

}
