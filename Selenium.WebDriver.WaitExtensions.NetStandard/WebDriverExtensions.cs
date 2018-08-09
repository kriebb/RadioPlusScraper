﻿// Decompiled with JetBrains decompiler
// Type: Selenium.WebDriver.WaitExtensions.WebDriverExtensions
// Assembly: Selenium.WebDriver.WaitExtensions, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BED2818-318F-460C-8884-74BE363FA75D
// Assembly location: D:\OwnProjects\RadioPlusScraper\RadioPlusScraper\packages\Selenium.WebDriver.WaitExtensions.1.0.0\lib\Selenium.WebDriver.WaitExtensions.dll

using OpenQA.Selenium;
using Selenium.WebDriver.WaitExtensions.WaitTypeSelections;

namespace Selenium.WebDriver.WaitExtensions
{
  public static class WebDriverExtensions
  {
    public static IWaitTypeSelection Wait(this IWebDriver webDriver, int ms = 500)
    {
      return (IWaitTypeSelection) new WaitTypeSelection(webDriver, ms);
    }
  }
}
