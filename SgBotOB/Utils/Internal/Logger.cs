using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SgBotOB.Utils.Internal
{
    /// <summary>
    /// 实现的Log输出器
    /// </summary>
    internal static class Logger
    {
        public static void Test()
        {
            Log("这是一条信息", 0);
            Log("这是一条重要", 1);
            Log("这是一条警告", 2);
            Log("这是一条错误", 3);
            Log("这是一条致命", 4);
        }
        /// <summary>
        /// 输出一条Log
        /// </summary>
        /// <param name="what">输出内容</param>
        /// <param name="level">输出等级，0~4重要性递增</param>
        public static void Log(string what, int level = 0)
        {
            try
            {
                AnsiConsole.Markup($"[green][[SgBotOB]][/] > {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                switch (level)
                {
                    case 0:
                        AnsiConsole.Markup(" [white][[信息]] [/]");
                        break;
                    case 1:
                        AnsiConsole.Markup(" [yellow][[重要]] [/]");
                        break;
                    case 2:
                        AnsiConsole.Markup(" [red][[警告]] [/]");
                        break;
                    case 3:
                        AnsiConsole.Markup(" [red][[错误]] [/]");
                        break;
                    case 4:
                        AnsiConsole.Markup(" [red][[致命]] [/]");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(level), level, null);
                }
                // AnsiConsole.Markup($"[white]{what}[/]\n");
                Console.WriteLine(what);
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n{e}");
            }
        }
        /// <summary>
        /// 登记0~4，越来越高
        /// </summary>
        private enum LogLevel
        {
            信息,
            重要,
            警告,
            错误,
            致命
        }
    }
}
