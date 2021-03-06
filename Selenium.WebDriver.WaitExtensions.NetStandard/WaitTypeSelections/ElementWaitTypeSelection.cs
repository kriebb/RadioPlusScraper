﻿// Decompiled with JetBrains decompiler
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
        private readonly int _delayMs;
        private readonly IWebElement _webelement;

        public ElementWaitTypeSelection(IWebElement webelement, int delayMs)
        {
            _webelement = webelement;
            _delayMs = delayMs;
        }

        public ITextWaitConditions ForText()
        {
            return new TextWaitConditions(_webelement, _delayMs);
        }

        public IClassWaitConditions ForClasses()
        {
            return new ClassWaitConditions(_webelement, _delayMs);
        }

        public IAttributeWaitConditions ForAttributes()
        {
            return new AttributeWaitConditions(_webelement, _delayMs);
        }

        public IElementWaitConditions ForElement()
        {
            return new ElementWaitConditions(_webelement, _delayMs);
        }
    }
}