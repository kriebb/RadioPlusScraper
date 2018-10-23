// Decompiled with JetBrains decompiler
// Type: Selenium.WebDriver.WaitExtensions.WaitConditions.WebPageWaitConditions
// Assembly: Selenium.WebDriver.WaitExtensions, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BED2818-318F-460C-8884-74BE363FA75D
// Assembly location: D:\OwnProjects\RadioPlusScraper\RadioPlusScraper\packages\Selenium.WebDriver.WaitExtensions.1.0.0\lib\Selenium.WebDriver.WaitExtensions.dll

using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Selenium.WebDriver.WaitExtensions.WaitConditions;

#pragma warning disable 618

namespace Selenium.WebDriver.WaitExtensions.NetStandard.WaitConditions
{
    public class WebPageWaitConditions : IWebPageWaitConditions
    {
        private readonly IWebDriver _webDriver;
        private readonly int _waitMs;

        public WebPageWaitConditions(IWebDriver webDriver, int waitMs)
        {
            _webDriver = webDriver;
            _waitMs = waitMs;
        }

        public void TitleToEqual(string title)
        {
            new WebDriverWait(_webDriver, TimeSpan.FromMilliseconds(_waitMs)).Until(ExpectedConditions.TitleIs(title));
        }

        public void TitleToContain(string title)
        {
            new WebDriverWait(_webDriver, TimeSpan.FromMilliseconds(_waitMs)).Until(ExpectedConditions.TitleContains(title));
        }

        public void UrlToEqual(string url)
        {
            new WebDriverWait(_webDriver, TimeSpan.FromMilliseconds(_waitMs)).Until(ExpectedConditions.UrlToBe(url));
        }

        public void UrlToContain(string url)
        {
            new WebDriverWait(_webDriver, TimeSpan.FromMilliseconds(_waitMs)).Until(ExpectedConditions.UrlContains(url));
        }

        public void UrlToMatch(string regex)
        {
            new WebDriverWait(_webDriver, TimeSpan.FromMilliseconds(_waitMs)).Until(ExpectedConditions.UrlMatches(regex));
        }

        public void ReadyStateComplete()
        {
            new WebDriverWait(_webDriver, TimeSpan.FromMilliseconds(_waitMs)).Until(driver => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));
        }
    }
}
