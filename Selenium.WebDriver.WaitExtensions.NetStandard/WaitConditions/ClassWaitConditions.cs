// Decompiled with JetBrains decompiler
// Type: Selenium.WebDriver.WaitExtensions.WaitConditions.ClassWaitConditions
// Assembly: Selenium.WebDriver.WaitExtensions, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BED2818-318F-460C-8884-74BE363FA75D
// Assembly location: D:\OwnProjects\RadioPlusScraper\RadioPlusScraper\packages\Selenium.WebDriver.WaitExtensions.1.0.0\lib\Selenium.WebDriver.WaitExtensions.dll

using System.Linq;
using System.Text.RegularExpressions;
using OpenQA.Selenium;
using Selenium.WebDriver.WaitExtensions.WaitTypeSelections;

namespace Selenium.WebDriver.WaitExtensions.WaitConditions
{
    public class ClassWaitConditions : WaitConditionsBase, IClassWaitConditions
    {
        private readonly IWebElement _webelement;

        public ClassWaitConditions(IWebElement webelement, int delayMs)
            : base(delayMs)
        {
            _webelement = webelement;
        }

        public bool ToContain(string className)
        {
            return WaitFor(() => GetClasses().Contains(className), "Waiting for Text to change.");
        }

        public bool ToContainMatch(string regexPattern)
        {
            var regex = new Regex(regexPattern);
            return WaitFor(() => GetClasses().Any(cn => regex.Match(cn).Success), ClassesString());
        }

        public bool ToNotContain(string className)
        {
            return WaitFor(() => !ToContain(className), ClassesString());
        }

        public bool ToNotContainMatch(string regexPattern)
        {
            var regex = new Regex(regexPattern);
            return WaitFor(() => GetClasses().All(cn => !regex.Match(cn).Success), ClassesString());
        }

        private string[] GetClasses()
        {
            return _webelement.GetAttribute("class").Split(' ');
        }

        private string ClassesString()
        {
            return "classes;\n   " + _webelement.GetAttribute("class");
        }
    }
}