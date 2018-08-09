// Decompiled with JetBrains decompiler
// Type: Selenium.WebDriver.WaitExtensions.WaitConditions.ElementWaitConditions
// Assembly: Selenium.WebDriver.WaitExtensions, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BED2818-318F-460C-8884-74BE363FA75D
// Assembly location: D:\OwnProjects\RadioPlusScraper\RadioPlusScraper\packages\Selenium.WebDriver.WaitExtensions.1.0.0\lib\Selenium.WebDriver.WaitExtensions.dll

using OpenQA.Selenium;
using System;

namespace Selenium.WebDriver.WaitExtensions.WaitConditions
{
  public class ElementWaitConditions : WaitConditionsBase, IElementWaitConditions
  {
    private readonly IWebElement _webelement;

    public ElementWaitConditions(IWebElement webelement, int waitMs)
      : base(waitMs)
    {
      this._webelement = webelement;
    }

    public void ToBeVisible()
    {
      this.WaitFor((Func<bool>) (() => this._webelement.Displayed), "Waiting for Text to change.");
    }

    public void ToBeInvisible()
    {
      this.WaitFor((Func<bool>) (() => !this._webelement.Displayed), "Waiting for Text to change.");
    }

    public void ToBeDisabled()
    {
      this.WaitFor((Func<bool>) (() => !this._webelement.Enabled), "Waiting for Text to change.");
    }

    public void ToBeEnabled()
    {
      this.WaitFor((Func<bool>) (() => this._webelement.Enabled), "Waiting for Text to change.");
    }

    public void ToBeSelected()
    {
      this.WaitFor((Func<bool>) (() => this._webelement.Selected), "Waiting for Text to change.");
    }

    public void ToNotBeSelected()
    {
      this.WaitFor((Func<bool>) (() => !this._webelement.Selected), "Waiting for Text to change.");
    }
  }
}
