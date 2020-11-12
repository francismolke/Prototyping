using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HeadlessChromeDriver
{
    class Program
    {
        public static void Main(string[] args)
        {
            //IGDL();
            IGFollow();
        }

        private static void IGFollow()
        {
            var service = ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;
            IWebDriver driver = new ChromeDriver(service, new ChromeOptions());
            driver.Url = "https://www.instagram.com/gazzo.if/";


            if (IsElementPresent(By.CssSelector("button.ffKix.sqdOP.L3NKy.y3zKF"), driver))
            {
                driver.FindElement(By.CssSelector("button.ffKix.sqdOP.L3NKy.y3zKF")).Click();
                Thread.Sleep(2000);
            }
            else if (IsElementPresent(By.CssSelector("button._5f5mN.jIbKX._6VtSN.yZn4P"), driver))
            {
                driver.FindElement(By.CssSelector("button._5f5mN.jIbKX._6VtSN.yZn4P")).Click();
                Thread.Sleep(2000);
            }
            else if (IsElementPresent(By.CssSelector("button.sqdOP.L3NKy.y3zKF"), driver))
            {
                driver.FindElement(By.CssSelector("button.sqdOP.L3NKy.y3zKF")).Click();
                Thread.Sleep(2000);
            }

        }
        






        public static void IGDL()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--headless");

            var service = ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;
            IWebDriver driver = new ChromeDriver(service, options);
            driver.Url = "https://www.instagram.com/p/B9pPpy8FoiW/";
            MakeLinkName("https://www.instagram.com/p/B9pPpy8FoiW/");
            Program pm = new Program();
            string downloadLink = pm.DownloadLinkExpress(driver);
            string profilename = GetProfileName(driver);
            using (WebClient wc = new WebClient())
            {
                wc.DownloadFile(downloadLink, "TestVideoTest.mp4");
            }
            driver.Close();
        }

        private static void MakeLinkName(string v)
        {
           // string neuerstring = v.Substring(v.LastIndexOf(@"/") - 11).Trim();
            string neuerstring = v.Substring(28).Trim();
            neuerstring = neuerstring.Remove(11).Trim();
        }

        private static string GetProfileName(IWebDriver driver)
        {
            string profilename = "";
            if (IsElementPresent(By.CssSelector("a.sqdOP.yWX7d._8A5w5.ZIAjV"), driver))
            {
                profilename = driver.FindElement(By.CssSelector("a.sqdOP.yWX7d._8A5w5.ZIAjV")).Text;

            }
            return profilename;
        }


        private string DownloadLinkExpress(IWebDriver driver)
        {
            string downloadLink = "";
            if (IsElementPresent(By.XPath("//meta[@property='og:video']"), driver))
            {
                downloadLink = driver.FindElement(By.XPath("//meta[@property='og:video']")).GetAttribute("content");

            }
            else if (IsElementPresent(By.XPath("//video[@class='tWeCl']"), driver))
            {
                downloadLink = driver.FindElement(By.XPath("//video[@class='tWeCl']")).GetAttribute("src");

            }
            return downloadLink;
        }

        private static bool IsElementPresent(By by, IWebDriver driver)
        {
            try
            {
                driver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
    }
}
