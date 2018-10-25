// Decompiled with JetBrains decompiler
// Type: Selenium.WebDriver.WaitExtensions.WaitConditions.WebElementWaitConditions
// Assembly: Selenium.WebDriver.WaitExtensions, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BED2818-318F-460C-8884-74BE363FA75D
// Assembly location: D:\OwnProjects\RadioPlusScraper\RadioPlusScraper\packages\Selenium.WebDriver.WaitExtensions.1.0.0\lib\Selenium.WebDriver.WaitExtensions.dll

using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Selenium.WebDriver.WaitExtensions.WaitConditions
{
    public class WebElementWaitConditions : IWebElementWaitConditions
    {
        private readonly By _by;
        private readonly int _waitMs;
        private readonly IWebDriver _webDriver;

        public WebElementWaitConditions(IWebDriver webDriver, int waitMs, By by)
        {
            _webDriver = webDriver;
            _waitMs = waitMs;
            _by = by;
        }

        public IWebElement ToExist()
        {
#pragma warning disable CS0618 // Type or member is obsolete
            new WebDriverWait(_webDriver, TimeSpan.FromMilliseconds(_waitMs)).Until(
                ExpectedConditions.ElementExists(_by));
#pragma warning restore CS0618 // Type or member is obsolete
            return _webDriver.FindElement(_by);
        }

        public void ToNotExist()
        {
            new WebDriverWait(_webDriver, TimeSpan.FromMilliseconds(_waitMs)).Until(ElementDoesntExists);
        }

        private bool ElementDoesntExists(IWebDriver driver)
        {
            try
            {
                driver.FindElement(_by);
                return false;
            }
            catch (NoSuchElementException)
            {
                return true;
            }
        }
    }
}