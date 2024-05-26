using SgBotOB.Model.Extra;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Joins;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SgBotOB.Utils.Internal
{
    internal static class DeviceMonitor
    {
        static public List<string> OutTemperature()
        {
            var str = ExecuteCommand("sensors");
            var matches = Regex.Matches(str[2], @"[-+]?\d+(\.\d+)?°C");
            var ret = new List<string>();
            foreach (var match in matches)
            {
                ret.Add(match.ToString()!);
            }
            var matches1 = Regex.Matches(str[6], @"[-+]?\d+(\.\d+)?°C");
            foreach (var match in matches1)
            {
                ret.Add(match.ToString()!);
            }
            return ret;
        }
        static public (double,long,double,long) GetCPUandMemory(string processType)
        {
            try
            {
                var str = ExecuteCommand("ps -aux");
                var str_l = str.Where(o => !string.IsNullOrWhiteSpace(o)).Select(o => o.Split(' ', StringSplitOptions.RemoveEmptyEntries));
                var c = str_l.FirstOrDefault();
                var l = str_l.Skip(1);

                var data = l.Select(o =>
                {
                    Dictionary<string, string> temp = new Dictionary<string, string>();
                    for (var i = 0; i < c.Length; i++)
                        if (i == 10) temp.Add(c[i], string.Join(" ", o.Skip(10)));
                        else temp.Add(c[i], o[i]);
                    return temp;
                });
                var ps = data.FirstOrDefault(o => o["COMMAND"].Contains(processType));                
                var cpuPercent = Convert.ToDouble(ps["%CPU"]);//CPU 占用比例

                var memoryValue = Convert.ToInt64(ps["RSS"]);//真实内存
                var memoryPercent = Convert.ToDouble(ps["%MEM"]);//内存占用比例
                var memoryOther = Convert.ToInt64(ps["VSZ"]);//虚拟内存
                return (cpuPercent,memoryValue,memoryPercent,memoryOther);
            }
            catch (Exception ex)
            {
                throw;                
            }
        }
        /// <summary>
        /// 执行 linux命令
        /// </summary>
        /// <param name="pmCommand"></param>
        /// <returns></returns>
        static string[] ExecuteCommand(string pmCommand)
        {
            var process = new System.Diagnostics.Process
            {
                StartInfo = new System.Diagnostics.ProcessStartInfo("/bin/bash", "")
            };
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.UseShellExecute = false;
            process.Start();
            process.StandardInput.WriteLine(pmCommand);
            //process.StandardInput.WriteLine("netstat -an |grep ESTABLISHED |wc -l");
            process.StandardInput.Close();
            var cpuInfo = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            process.Dispose();
            var lines = cpuInfo.Split('\n');
            return lines;
        }        
    }
}
