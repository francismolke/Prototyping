using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DownloadGondolaVideo
{
    class Program
    {
        static void Main(string[] args)
        {

            GDL();
        }

        private static void GDL()
        {
            //ChromeOptions options = new ChromeOptions();
            //var service = ChromeDriverService.CreateDefaultService();
            FirefoxOptions options = new FirefoxOptions();
            var service = FirefoxDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;
            IWebDriver driver = new FirefoxDriver(service, new FirefoxOptions());
            driver.Url = "https://gondola.stravers.net/GondolaStairs.webm";
            var element = driver.FindElement(By.XPath("//video"));
            Actions action = new Actions(driver);
            action.ContextClick(element).KeyDown(Keys.Shift).SendKeys("S").Build().Perform();
                //SendKeys(Keys.ArrowDown).SendKeys(Keys.ArrowDown).SendKeys(Keys.ArrowDown).SendKeys(Keys.ArrowDown).SendKeys(Keys.Enter).Build().Perform();
            //Build().Perform(); .SendKeys(Keys.Shift).SendKeys("S")
           // Thread.Sleep(10000);
              //action.SendKeys(Keys.Shift).SendKeys(Keys.ArrowDown).SendKeys(Keys.ArrowDown).SendKeys(Keys.ArrowDown).SendKeys(Keys.ArrowDown).SendKeys(Keys.Enter).Perform();
        }
    }
}
