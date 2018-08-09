// Decompiled with JetBrains decompiler
// Type: Selenium.WebDriver.WaitExtensions.WaitConditions.WebElementWaitConditions
// Assembly: Selenium.WebDriver.WaitExtensions, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BED2818-318F-460C-8884-74BE363FA75D
// Assembly location: D:\OwnProjects\RadioPlusScraper\RadioPlusScraper\packages\Selenium.WebDriver.WaitExtensions.1.0.0\lib\Selenium.WebDriver.WaitExtensions.dll

using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace Selenium.WebDriver.WaitExtensions.WaitConditions
{
  public class WebElementWaitConditions : IWebElementWaitConditions
  {
    private readonly IWebDriver _webDriver;
    private readonly int _waitMs;
    private readonly By _by;

    public WebElementWaitConditions(IWebDriver webDriver, int waitMs, By by)
    {
      this._webDriver = webDriver;
      this._waitMs = waitMs;
      this._by = by;
    }

    public IWebElement ToExist()
    {
      ((DefaultWait<IWebDriver>) new WebDriverWait(this._webDriver, TimeSpan.FromMilliseconds((double) this._waitMs))).Until<IWebElement>(ExpectedConditions.ElementExists(this._by));
      return this._webDriver.FindElement(this._by);
    }

    public void ToNotExist()
    {
      ((DefaultWait<IWebDriver>) new WebDriverWait(this._webDriver, TimeSpan.FromMilliseconds((double) this._waitMs))).Until<bool>(new Func<IWebDriver, bool>(this.ElementDoesntExists));
    }

    private bool ElementDoesntExists(IWebDriver driver)
    {
      try
      {
        driver.FindElement(this._by);
        return false;
      }
      catch (NoSuchElementException ex)
      {
        return true;
      }
    }
  }
}
