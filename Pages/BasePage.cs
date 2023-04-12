using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using OpenQA.Selenium.Support.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OpenQA.Selenium.Internal;

namespace SeleniumSpecFlowProject.Pages
{
   abstract class BasePage
   {

      protected IWebDriver driver;
      protected WebDriverWait wait;
      private By pageLoadingMessageBox => By.CssSelector("div[class='notification processing']");
      protected readonly string cssLocator = "css";
      protected readonly string nameLocator = "name";
      protected readonly string idLocator = "id";
      protected readonly string linkTextLocator = "linktext";
      protected string _errorMessage = "//*[@class='notification error full data-error-summary']/ul/li";

      public BasePage(IWebDriver driver)
      {
         this.driver = driver;
         this.wait = new WebDriverWait(driver, TimeSpan.FromSeconds(90));
      }

      public void enterText(IWebElement element, String text)
      {
         waitForPageLoad(driver);
         element.SendKeys(text);
      }

      public void pressEnter()
      {
         waitForPageLoad(driver);
         IWebElement currentElement = driver.SwitchTo().ActiveElement();
         currentElement.SendKeys(Keys.Return);
      }

      public void waitAndClick(IWebElement element)
      {
         waitForPageLoad(driver);
         wait.Until(ExpectedConditions.InvisibilityOfElementLocated(pageLoadingMessageBox));
         element.Click();
      }

      protected void Select(By locator)
      {
         ClickableElement(locator).Click();
      }

      public void waitAndEnterText(IWebElement element, String text)
      {
         waitForPageLoad(driver);
         element.SendKeys(text);
      }

      public void selectItemFromMenubar(IWebElement parentelement, IWebElement childelement)
      {
         waitForPageLoad(driver);
         Actions action = new Actions(driver);
         action.MoveToElement(parentelement).Perform();
         action.MoveToElement(childelement).Click().Perform();
      }

      public void selectFromDropdown(IWebElement dropdown, String text)
      {
         waitForPageLoad(driver);
         var select = new SelectElement(dropdown);
         select.SelectByText(text);
      }

      public void findElementByNameAndSendKeys(string key, string value)
      {
         waitForPageLoad(driver);
         var elementName = driver.FindElement(By.Name(key));
         elementName.SendKeys(value);
      }

      public bool elementExists(By by)
      {
         waitForPageLoad(driver);
         var elements = driver.FindElements(by);
         return elements.Count > 0;
      }

      public IWebElement findElement(String locator, String variable)
      {
         waitForPageLoad(driver);
         if (locator.Equals(nameLocator))
         {
            return driver.FindElement(By.Name(variable));
         }
         else if (locator.Equals(idLocator))
         {
            return driver.FindElement(By.Id(variable));
         }
         else if (locator.Equals(linkTextLocator))
         {
            return driver.FindElement(By.LinkText(variable));
         }
         else if (locator.Equals(cssLocator))
         {
            return driver.FindElement(By.CssSelector(variable));
         }
         else
         {
            return driver.FindElement(By.XPath(variable));
         }
      }

      public IWebElement FindElement(By locator)
      {
         waitForPageLoad(driver);
         return driver.FindElement(locator);
      }

      public DefaultWait<IWebDriver> checkForElementFrequently()
      {
         waitForPageLoad(driver);
         DefaultWait<IWebDriver> fluentWait = new DefaultWait<IWebDriver>(driver);
         fluentWait.Timeout = TimeSpan.FromSeconds(120);
         fluentWait.PollingInterval = TimeSpan.FromMilliseconds(250);

         fluentWait.IgnoreExceptionTypes(typeof(NoSuchElementException));
         fluentWait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
         fluentWait.IgnoreExceptionTypes(typeof(ElementClickInterceptedException));
         fluentWait.Message = "Element to be searched not found";
         return fluentWait;
      }

      public void waitTillElementTobeClickable(By locator)
      {
         waitForPageLoad(driver);
         WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(120));
         wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
         wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
         wait.IgnoreExceptionTypes(typeof(ElementClickInterceptedException));
            object value = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(locator));
      }
      public void waitTillElementTobeVisible(By locator)
      {
         waitForPageLoad(driver);
         WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(120));
         wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
         wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
         wait.IgnoreExceptionTypes(typeof(ElementClickInterceptedException));
         wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(locator));
      }

      public void pageScrollUp()
      {
         waitForPageLoad(driver);
         IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
         js.ExecuteScript("window.scrollTo(0, 0)");
      }
      public static void waitForPageLoad(IWebDriver driver)
      {
         WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(120));
         wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));
      }
      public void waitTillElementExists(By locator)
      {
         waitForPageLoad(driver);
         WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(120));
         wait.Until(ExpectedConditions.ElementExists(locator));
      }

      public void waitTillTitleIsVisible(string title)
      {
         waitForPageLoad(driver);
         WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(120));
         wait.Until(ExpectedConditions.TitleContains(title));
      }

      public IList<IWebElement> findElements(String locator, String variable)
      {
         waitForPageLoad(driver);
         if (locator.Equals(nameLocator))
         {
            return driver.FindElements(By.Name(variable));
         }
         else if (locator.Equals(idLocator))
         {
            return driver.FindElements(By.Id(variable));
         }
         else if (locator.Equals(linkTextLocator))
         {
            return driver.FindElements(By.LinkText(variable));
         }
         else if (locator.Equals(cssLocator))
         {
            return driver.FindElements(By.CssSelector(variable));
         }
         else
         {
            return driver.FindElements(By.XPath(variable));
         }
      }

      public void pageScrollDown()
      {
         IJavaScriptExecutor jsExec = (IJavaScriptExecutor)driver;
         jsExec.ExecuteScript("window.scrollTo(0, document.body.scrollHeight)");
      }

      public void waitForSpinnerWidgetToDisappear()
      {
         waitForPageLoad(driver);
         wait.Until(ExpectedConditions.InvisibilityOfElementLocated(pageLoadingMessageBox));
      }
      protected void ClickUsingJavaScript(IWebElement element)
      {
         waitForPageLoad(driver);
         IJavaScriptExecutor jsExec = (IJavaScriptExecutor)driver;
         jsExec.ExecuteScript("arguments[0].click();", element);
      }

      public void ContainsErrorMessage(string errorMsg)
      {
         waitTillElementTobeClickable(By.XPath(_errorMessage));
         IWebElement errorMessageText = driver.FindElement(By.XPath(_errorMessage));
         string errorMsgDisplayed = errorMessageText.Text;
         if (!errorMsgDisplayed.Contains(errorMsg))
         {
            throw new 
                    Exception($"Error message not found: {errorMsg}");
         }
      }

      public IWebElement ClickableElement(By locator)
      {
         waitTillElementTobeClickable(locator);
         return FindElement(locator);
      }


      protected void TypeInText(By locator, string text)
      {
         ClickableElement(locator).Clear();
         ClickableElement(locator).SendKeys(text);
      }

      public void SelectFromDropdown(By locator, string value)
      {
         var selectable = new SelectElement(ClickableElement(locator));
         selectable.SelectByText(value);
      }

      protected void TypeInFilteredText(By locator, string text)
      {
         TypeInText(locator, text);
         Select(By.XPath("//a[text()='" + text + "']"));
      }


   }
}