// Decompiled with JetBrains decompiler
// Type: Selenium.WebDriver.WaitExtensions.WaitConditions.WaitConditionsBase
// Assembly: Selenium.WebDriver.WaitExtensions, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BED2818-318F-460C-8884-74BE363FA75D
// Assembly location: D:\OwnProjects\RadioPlusScraper\RadioPlusScraper\packages\Selenium.WebDriver.WaitExtensions.1.0.0\lib\Selenium.WebDriver.WaitExtensions.dll

using OpenQA.Selenium;
using System;
using System.Diagnostics;
using System.Threading;

namespace Selenium.WebDriver.WaitExtensions.WaitConditions
{
  public abstract class WaitConditionsBase
  {
    private readonly TimeSpan _interval = TimeSpan.FromMilliseconds(25.0);
    protected readonly int _waitMs;

    protected WaitConditionsBase(int waitMs)
    {
      this._waitMs = waitMs;
    }

    protected bool WaitFor(Func<bool> test, string exceptionMessage = "Waiting for Text to change.")
    {
      Stopwatch stopwatch = new Stopwatch();
      stopwatch.Start();
      if (test())
        return true;
      while (stopwatch.ElapsedMilliseconds <= (long) this._waitMs)
      {
        if (test())
          return true;
        Thread.Sleep(this._interval);
      }
      throw new WebDriverTimeoutException(exceptionMessage);
    }
  }
}
