// Decompiled with JetBrains decompiler
// Type: Selenium.WebDriver.WaitExtensions.WaitTypeSelections.WaitTypeSelection
// Assembly: Selenium.WebDriver.WaitExtensions, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BED2818-318F-460C-8884-74BE363FA75D
// Assembly location: D:\OwnProjects\RadioPlusScraper\RadioPlusScraper\packages\Selenium.WebDriver.WaitExtensions.1.0.0\lib\Selenium.WebDriver.WaitExtensions.dll

using OpenQA.Selenium;
using Selenium.WebDriver.WaitExtensions.WaitConditions;

namespace Selenium.WebDriver.WaitExtensions.WaitTypeSelections
{
  public class WaitTypeSelection : IWaitTypeSelection
  {
    private readonly IWebDriver _webDriver;
    private readonly int _waitMs;

    public WaitTypeSelection(IWebDriver webDriver, int waitMs)
    {
      this._webDriver = webDriver;
      this._waitMs = waitMs;
    }

    public IWebElementWaitConditions ForElement(By by)
    {
      return (IWebElementWaitConditions) new WebElementWaitConditions(this._webDriver, this._waitMs, by);
    }

    public IWebPageWaitConditions ForPage()
    {
      return (IWebPageWaitConditions) new WebPageWaitConditions(this._webDriver, this._waitMs);
    }
  }
}
