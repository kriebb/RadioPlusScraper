// Decompiled with JetBrains decompiler
// Type: Selenium.WebDriver.WaitExtensions.WaitConditions.ClassWaitConditions
// Assembly: Selenium.WebDriver.WaitExtensions, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BED2818-318F-460C-8884-74BE363FA75D
// Assembly location: D:\OwnProjects\RadioPlusScraper\RadioPlusScraper\packages\Selenium.WebDriver.WaitExtensions.1.0.0\lib\Selenium.WebDriver.WaitExtensions.dll

using OpenQA.Selenium;
using Selenium.WebDriver.WaitExtensions.WaitTypeSelections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Selenium.WebDriver.WaitExtensions.WaitConditions
{
  public class ClassWaitConditions : WaitConditionsBase, IClassWaitConditions
  {
    private readonly IWebElement _webelement;

    public ClassWaitConditions(IWebElement webelement, int delayMs)
      : base(delayMs)
    {
      this._webelement = webelement;
    }

    public bool ToContain(string className)
    {
      return this.WaitFor((Func<bool>) (() => ((IEnumerable<string>) this.GetClasses()).Contains<string>(className)), "Waiting for Text to change.");
    }

    private string[] GetClasses()
    {
      return this._webelement.GetAttribute("class").Split(' ');
    }

    public bool ToContainMatch(string regexPattern)
    {
      Regex regex = new Regex(regexPattern);
      return this.WaitFor((Func<bool>) (() => ((IEnumerable<string>) this.GetClasses()).Any<string>((Func<string, bool>) (cn => regex.Match(cn).Success))), this.ClassesString());
    }

    public bool ToNotContain(string className)
    {
      return this.WaitFor((Func<bool>) (() => !this.ToContain(className)), this.ClassesString());
    }

    public bool ToNotContainMatch(string regexPattern)
    {
      Regex regex = new Regex(regexPattern);
      return this.WaitFor((Func<bool>) (() => ((IEnumerable<string>) this.GetClasses()).All<string>((Func<string, bool>) (cn => !regex.Match(cn).Success))), this.ClassesString());
    }

    private string ClassesString()
    {
      return "classes;\n   " + this._webelement.GetAttribute("class");
    }
  }
}
