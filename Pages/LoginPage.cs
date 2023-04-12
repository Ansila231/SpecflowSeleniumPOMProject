using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;

namespace SeleniumSpecFlowProject.Pages
{
    class LoginPage : BasePage
    {
        private new readonly IWebDriver driver;

        private IWebElement passwordTextBox => driver.FindElement(By.Name("password"));
        private IWebElement usernameTextBox => driver.FindElement(By.Name("username"));
        private IWebElement loginButton => driver.FindElement(By.XPath("//p[@onclick='doPostForm()']"));
        private string defaultPageURL = "https://farmersfz.com/";

        private string loginLink = "div[class*='login-box']";

        public LoginPage(IWebDriver driver) : base(driver)
        {
            this.driver = driver;
        }

        public Boolean ValidateTitle()
        {
            return driver.Title.ToLower().Contains("Buy Fresh Exotic Vegetables & Fruits Online in Kochi, Trivandrum, Thrissur, Kottayam | Farmersfz.com".ToLower());
        }

        public void FindElementByNameAndSendKeys(string key, string value)
        {
            var elementName =  driver.FindElement(By.Name(key));
            elementName.SendKeys(value);
        }

        public void Login(string username, string password = "test")
        {
            IWebElement login = findElement("css", loginLink);
            login.Click();
        }

        public Boolean ISLandingPageVisible()
        {
            return driver.Title.ToLower().Contains("Buy Fresh Exotic Vegetables & Fruits Online in Kochi, Trivandrum, Thrissur, Kottayam | Farmersfz.com".ToLower());
        }

        public void goTo(string pageURL)
        {
            if (string.IsNullOrEmpty(pageURL))
            {
                pageURL = defaultPageURL;
            }

            driver.Navigate().GoToUrl(pageURL);
        }

    }
}
