// Decompiled with JetBrains decompiler
// Type: Selenium.WebDriver.WaitExtensions.WaitConditions.TextWaitConditions
// Assembly: Selenium.WebDriver.WaitExtensions, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BED2818-318F-460C-8884-74BE363FA75D
// Assembly location: D:\OwnProjects\RadioPlusScraper\RadioPlusScraper\packages\Selenium.WebDriver.WaitExtensions.1.0.0\lib\Selenium.WebDriver.WaitExtensions.dll

using OpenQA.Selenium;
using System;
using System.Text.RegularExpressions;

namespace Selenium.WebDriver.WaitExtensions.WaitConditions
{
  public class TextWaitConditions : WaitConditionsBase, ITextWaitConditions
  {
    private readonly IWebElement _webelement;

    public TextWaitConditions(IWebElement webelement, int waitMs)
      : base(waitMs)
    {
      this._webelement = webelement;
    }

    public bool ToEqual(string text)
    {
      return this.WaitFor((Func<bool>) (() => this._webelement.Text == text), "Waiting for Text to change.");
    }

    public bool ToNotEqual(string text)
    {
      return this.WaitFor((Func<bool>) (() => this._webelement.Text != text), "Waiting for Text to change.");
    }

    public bool ToContain(string text)
    {
      return this.WaitFor((Func<bool>) (() => this._webelement.Text.Contains(text)), "Waiting for Text to change.");
    }

    public bool ToNotContain(string text)
    {
      return this.WaitFor((Func<bool>) (() => !this._webelement.Text.Contains(text)), "Waiting for Text to change.");
    }

    public bool ToMatch(string regexPattern)
    {
      Regex regex = new Regex(regexPattern);
      return this.WaitFor((Func<bool>) (() => regex.Match(this._webelement.Text).Success), "Waiting for Text to change.");
    }

    public bool ToNotMatch(string regexPattern)
    {
      Regex regex = new Regex(regexPattern);
      return this.WaitFor((Func<bool>) (() => !regex.Match(this._webelement.Text).Success), "Waiting for Text to change.");
    }
  }
}
