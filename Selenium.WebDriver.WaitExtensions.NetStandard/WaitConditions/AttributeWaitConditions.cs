// Decompiled with JetBrains decompiler
// Type: Selenium.WebDriver.WaitExtensions.WaitConditions.AttributeWaitConditions
// Assembly: Selenium.WebDriver.WaitExtensions, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BED2818-318F-460C-8884-74BE363FA75D
// Assembly location: D:\OwnProjects\RadioPlusScraper\RadioPlusScraper\packages\Selenium.WebDriver.WaitExtensions.1.0.0\lib\Selenium.WebDriver.WaitExtensions.dll

using OpenQA.Selenium;
using OpenQA.Selenium.Internal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Selenium.WebDriver.WaitExtensions.WaitConditions
{
  public class AttributeWaitConditions : WaitConditionsBase, IAttributeWaitConditions
  {
    private readonly IWebElement _webelement;

    public AttributeWaitConditions(IWebElement webelement, int delayMs)
      : base(delayMs)
    {
      this._webelement = webelement;
    }

    public bool ToContain(string attrName)
    {
      return this.WaitFor((Func<bool>) (() => !string.IsNullOrEmpty(this._webelement.GetAttribute(attrName))), this.GetAttributesString());
    }

    public bool ToNotContain(string attrName)
    {
      return this.WaitFor((Func<bool>) (() => !this.ToContain(attrName)), this.GetAttributesString());
    }

    public bool ToContainWithValue(string attrName, string attrValue)
    {
      return this.WaitFor((Func<bool>) (() =>
      {
        if (this.ToContain(attrName))
          return this._webelement.GetAttribute(attrName) == attrValue;
        return false;
      }), this.GetAttributesString());
    }

    public bool ToContainWithoutValue(string attrName, string attrValue)
    {
      return this.WaitFor((Func<bool>) (() =>
      {
        if (!this.ToContain(attrName))
          return this._webelement.GetAttribute(attrName) != attrValue;
        return false;
      }), this.GetAttributesString());
    }

    private IDictionary<string, object> GetElementAttributes()
    {
      return (IDictionary<string, object>) (((IJavaScriptExecutor) ((IWrapsDriver) this._webelement).WrappedDriver).ExecuteScript("var items = {}; for (index = 0; index < arguments[0].attributes.length; ++index) { items[arguments[0].attributes[index].name] = arguments[0].attributes[index].value }; return items;", (object) this._webelement) as Dictionary<string, object>);
    }

    private string GetAttributesString()
    {
      IDictionary<string, object> elementAttributes = this.GetElementAttributes();
      string str = "";
      if (elementAttributes.Count > 0)
        str = string.Join("\n   ", elementAttributes.Keys.Select<string, string>((Func<string, string>) (k => k + " = " + elementAttributes[k])));
      return string.Format("attribute list;{0}", (object) str);
    }
  }
}
