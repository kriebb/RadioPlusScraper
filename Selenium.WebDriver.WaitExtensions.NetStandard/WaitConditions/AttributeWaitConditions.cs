// Decompiled with JetBrains decompiler
// Type: Selenium.WebDriver.WaitExtensions.WaitConditions.AttributeWaitConditions
// Assembly: Selenium.WebDriver.WaitExtensions, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BED2818-318F-460C-8884-74BE363FA75D
// Assembly location: D:\OwnProjects\RadioPlusScraper\RadioPlusScraper\packages\Selenium.WebDriver.WaitExtensions.1.0.0\lib\Selenium.WebDriver.WaitExtensions.dll

using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Internal;

namespace Selenium.WebDriver.WaitExtensions.WaitConditions
{
    public class AttributeWaitConditions : WaitConditionsBase, IAttributeWaitConditions
    {
        private readonly IWebElement _webelement;

        public AttributeWaitConditions(IWebElement webelement, int delayMs)
            : base(delayMs)
        {
            _webelement = webelement;
        }

        public bool ToContain(string attrName)
        {
            return WaitFor(() => !string.IsNullOrEmpty(_webelement.GetAttribute(attrName)), GetAttributesString());
        }

        public bool ToNotContain(string attrName)
        {
            return WaitFor(() => !ToContain(attrName), GetAttributesString());
        }

        public bool ToContainWithValue(string attrName, string attrValue)
        {
            return WaitFor(() =>
            {
                if (ToContain(attrName))
                    return _webelement.GetAttribute(attrName) == attrValue;
                return false;
            }, GetAttributesString());
        }

        public bool ToContainWithoutValue(string attrName, string attrValue)
        {
            return WaitFor(() =>
            {
                if (!ToContain(attrName))
                    return _webelement.GetAttribute(attrName) != attrValue;
                return false;
            }, GetAttributesString());
        }

        private IDictionary<string, object> GetElementAttributes()
        {
            return ((IJavaScriptExecutor) ((IWrapsDriver) _webelement).WrappedDriver).ExecuteScript(
                "var items = {}; for (index = 0; index < arguments[0].attributes.length; ++index) { items[arguments[0].attributes[index].name] = arguments[0].attributes[index].value }; return items;",
                (object) _webelement) as Dictionary<string, object>;
        }

        private string GetAttributesString()
        {
            var elementAttributes = GetElementAttributes();
            var str = "";
            if (elementAttributes.Count > 0)
                str = string.Join("\n   ", elementAttributes.Keys.Select(k => k + " = " + elementAttributes[k]));
            return string.Format("attribute list;{0}", str);
        }
    }
}