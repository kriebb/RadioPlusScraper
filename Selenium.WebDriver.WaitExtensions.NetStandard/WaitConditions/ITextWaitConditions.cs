// Decompiled with JetBrains decompiler
// Type: Selenium.WebDriver.WaitExtensions.WaitConditions.ITextWaitConditions
// Assembly: Selenium.WebDriver.WaitExtensions, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BED2818-318F-460C-8884-74BE363FA75D
// Assembly location: D:\OwnProjects\RadioPlusScraper\RadioPlusScraper\packages\Selenium.WebDriver.WaitExtensions.1.0.0\lib\Selenium.WebDriver.WaitExtensions.dll

namespace Selenium.WebDriver.WaitExtensions.WaitConditions
{
    public interface ITextWaitConditions
    {
        bool ToEqual(string text);

        bool ToContain(string text);

        bool ToNotEqual(string text);

        bool ToNotContain(string text);

        bool ToMatch(string regex);

        bool ToNotMatch(string regex);
    }
}