using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Button
{
    class Program
    {
        static void Main(string[] args)
        {
            var service = ChromeDriverService.CreateDefaultService();
            IWebDriver driver = new ChromeDriver(service, new ChromeOptions());
            service.HideCommandPromptWindow = true;
            driver.Url = "https://www.instagram.com/p/B_3c9m6FYNe/";
            var aufrufeRoh = driver.FindElement(By.ClassName("vcOH2")).Text;
            driver.FindElement(By.ClassName("vcOH2")).Click();
            //div[@class='v1Nh3 kIKUG  _bz0w']//a[@href]"
            var likesRoh = driver.FindElement(By.ClassName("vJRqr")).Text;
            
       //     var html = driver.PageSource;
            // mediaFileName.Substring(mediaFileName.IndexOf('-') + 1).Trim());
            var aufruf = aufrufeRoh.Split(' ').First();
            var likes = likesRoh.Substring(likesRoh.IndexOf(' ') + 1).Trim();
            likes = likes.Split(' ').First();
            // var haha = aufrufe.Substring(aufrufe.IndexOf(' ') + 1).TrimEnd(' ');
            //            16.800 Aufrufe
            //              Gefällt 5.324 Mal

          //  Console.WriteLine(html + "etwas" + likes);
            double summe = double.Parse(likes, NumberStyles.AllowDecimalPoint, CultureInfo.CreateSpecificCulture("en-GB")) / double.Parse(aufruf, NumberStyles.AllowDecimalPoint, CultureInfo.CreateSpecificCulture("en-GB"));
            summe = summe * 100;
            summe = Math.Round(summe, 2);
            Console.WriteLine(summe);
        }
    }
}
