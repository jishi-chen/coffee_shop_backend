using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using System;
using System.Threading;
using System.Configuration;

namespace YoutubeControl
{
    public class Program
    {
        static void Main(string[] args)
        {
            string youtubeUrl = ConfigurationManager.AppSettings["VideoUrl"];
            string playMinute = ConfigurationManager.AppSettings["PlayMinute"];
            int duration = double.TryParse(playMinute, out double tempMinute) ? Convert.ToInt32(tempMinute * 60 * 1000) : 0;


            // 設定 EdgeDriver 的選項
            EdgeOptions edgeOption = new EdgeOptions();
            edgeOption.AddAdditionalEdgeOption("useAutomationExtension", false);
            edgeOption.AddExcludedArgument("enable-automation");
            edgeOption.AddExcludedArgument("enable-logging");
            edgeOption.AddArgument("--start-maximized");
            edgeOption.AddArgument("--disable-infobars");

            // 啟動 EdgeDriver 並載入選項
            IWebDriver driver = new EdgeDriver(edgeOption);
            try
            {
                driver.Url = youtubeUrl;

                var element = driver.FindElement(By.ClassName("ytp-fullscreen-button"));
                Thread.Sleep(3000);
                element.Click();
                Thread.Sleep(duration);
            }
            finally
            {
                driver.Quit();
            }


            // 使用預設瀏覽器打開YouTube網址
            //Process process = Process.Start(new ProcessStartInfo
            //{
            //    FileName = "msedge",
            //    Arguments = youtubeUrl,
            //    UseShellExecute = true
            //});

            //Thread.Sleep(duration);

            //var edgeProcesses = Process.GetProcessesByName("msedge")
            //                           .Where(p => !p.HasExited);

            //foreach (var proc in edgeProcesses)
            //{
            //    try
            //    {
            //        proc.Kill();
            //    }
            //    catch (Exception ex)
            //    {
            //    }
            //}
        }
    }
}
