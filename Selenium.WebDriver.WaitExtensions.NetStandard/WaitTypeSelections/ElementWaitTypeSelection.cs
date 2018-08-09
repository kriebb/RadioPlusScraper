// Decompiled with JetBrains decompiler
// Type: Selenium.WebDriver.WaitExtensions.WaitTypeSelections.ElementWaitTypeSelection
// Assembly: Selenium.WebDriver.WaitExtensions, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BED2818-318F-460C-8884-74BE363FA75D
// Assembly location: D:\OwnProjects\RadioPlusScraper\RadioPlusScraper\packages\Selenium.WebDriver.WaitExtensions.1.0.0\lib\Selenium.WebDriver.WaitExtensions.dll

using OpenQA.Selenium;
using Selenium.WebDriver.WaitExtensions.WaitConditions;

namespace Selenium.WebDriver.WaitExtensions.WaitTypeSelections
{
  public class ElementWaitTypeSelection : IElementWaitTypeSelection
  {
    private readonly IWebElement _webelement;
    private readonly int _delayMs;

    public ElementWaitTypeSelection(IWebElement webelement, int delayMs)
    {
      this._webelement = webelement;
      this._delayMs = delayMs;
    }

    public ITextWaitConditions ForText()
    {
      return (ITextWaitConditions) new TextWaitConditions(this._webelement, this._delayMs);
    }

    public IClassWaitConditions ForClasses()
    {
      return (IClassWaitConditions) new ClassWaitConditions(this._webelement, this._delayMs);
    }

    public IAttributeWaitConditions ForAttributes()
    {
      return (IAttributeWaitConditions) new AttributeWaitConditions(this._webelement, this._delayMs);
    }

    public IElementWaitConditions ForElement()
    {
      return (IElementWaitConditions) new ElementWaitConditions(this._webelement, this._delayMs);
    }
  }
}
