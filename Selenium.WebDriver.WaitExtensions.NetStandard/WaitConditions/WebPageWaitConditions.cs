// Decompiled with JetBrains decompiler
// Type: Selenium.WebDriver.WaitExtensions.WaitConditions.WebPageWaitConditions
// Assembly: Selenium.WebDriver.WaitExtensions, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BED2818-318F-460C-8884-74BE363FA75D
// Assembly location: D:\OwnProjects\RadioPlusScraper\RadioPlusScraper\packages\Selenium.WebDriver.WaitExtensions.1.0.0\lib\Selenium.WebDriver.WaitExtensions.dll

using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace Selenium.WebDriver.WaitExtensions.WaitConditions
{
  public class WebPageWaitConditions : IWebPageWaitConditions
  {
    private readonly IWebDriver _webDriver;
    private readonly int _waitMs;

    public WebPageWaitConditions(IWebDriver webDriver, int waitMs)
    {
      this._webDriver = webDriver;
      this._waitMs = waitMs;
    }

    public void TitleToEqual(string title)
    {
      ((DefaultWait<IWebDriver>) new WebDriverWait(this._webDriver, TimeSpan.FromMilliseconds((double) this._waitMs))).Until<bool>(ExpectedConditions.TitleIs(title));
    }

    public void TitleToContain(string title)
    {
      ((DefaultWait<IWebDriver>) new WebDriverWait(this._webDriver, TimeSpan.FromMilliseconds((double) this._waitMs))).Until<bool>(ExpectedConditions.TitleContains(title));
    }

    public void UrlToEqual(string url)
    {
      ((DefaultWait<IWebDriver>) new WebDriverWait(this._webDriver, TimeSpan.FromMilliseconds((double) this._waitMs))).Until<bool>(ExpectedConditions.UrlToBe(url));
    }

    public void UrlToContain(string url)
    {
      ((DefaultWait<IWebDriver>) new WebDriverWait(this._webDriver, TimeSpan.FromMilliseconds((double) this._waitMs))).Until<bool>(ExpectedConditions.UrlContains(url));
    }

    public void UrlToMatch(string regex)
    {
      ((DefaultWait<IWebDriver>) new WebDriverWait(this._webDriver, TimeSpan.FromMilliseconds((double) this._waitMs))).Until<bool>(ExpectedConditions.UrlMatches(regex));
    }

    public void ReadyStateComplete()
    {
      ((DefaultWait<IWebDriver>) new WebDriverWait(this._webDriver, TimeSpan.FromMilliseconds((double) this._waitMs))).Until<bool>((Func<IWebDriver, bool>) (driver => ((IJavaScriptExecutor) driver).ExecuteScript("return document.readyState").Equals((object) "complete")));
    }
  }
}
