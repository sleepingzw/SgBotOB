using SgBotOB.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SgBotOB.Model.Extra
{
    public class RootObject
    {
        public string error { get; set; }
        public List<Data> data { get; set; }
    }
    public class Data
    {
        public int pid { get; set; }
        public int p { get; set; }
        public int uid { get; set; }
        public string title { get; set; }
        public string author { get; set; }
        public bool r18 { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public List<string> tags { get; set; }
        public string ext { get; set; }
        public int aiType { get; set; }
        public long uploadDate {  get; set; }  
        public Urls urls { get; set; }
    }
    public class Urls
    {
        public string regular { get; set; }
    }


    public class SetuInfo
    {
        public string title;
        public string author;
        public List<string> tags;
        public string urls;
        public long pid;
        public string imgname;
        public string address;
        public string jsonaddress;
        public string dicaddress;
        public SetuInfo(RootObject ro)
        {
            this.title = ro.data[0].title;
            this.author = ro.data[0].author;
            this.tags = ro.data[0].tags;
            this.urls = ro.data[0].urls.regular;
            this.pid = ro.data[0].pid;
            GetNameAndAddress(urls);
        }
        private void GetNameAndAddress(string urls)
        {
            string[] tts = urls.Split('/');
            imgname = tts[tts.Length - 1];
            string a, b;
            a = imgname.Split('.')[0];
            b = imgname.Split('.')[1];
            address = Path.Combine(StaticData.ExePath!, "Data/Img/Setu/" + a + "/" + "pic." + b);
            dicaddress = Path.Combine(StaticData.ExePath!, "Data/Img/Setu/" + a);
            jsonaddress = Path.Combine(StaticData.ExePath!, "Data/Img/Setu/" + a + "/" + "data.json");
        }
        public bool FindExist()
        {
            bool result = File.Exists(address);
            return result;
        }


    }
    public class Datago
    {
        public int r18 { get; set; }
        public List<string> size { get; set; }
        public List<string> tag { get; set; }
        public string keyword { get; set; }
        public string proxy { get; set; }
    }
}
