using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using SeleniumSpecFlowProject.Pages;
using SeleniumSpecFlowProject.Utilities;
using System;
using TechTalk.SpecFlow;

namespace SeleniumSpecFlowProject.Steps
{
    [Binding]
    public class LoginSteps
    {
        private readonly IWebDriver driver;
        private readonly LoginPage loginPage;
      
        private string _username = null;
        private string _password = null;
        private string _siteUrl = null;

        public LoginSteps(ScenarioContext scenarioContext)
        {
            driver = scenarioContext["WEB_DRIVER"] as IWebDriver;
            loginPage = new LoginPage(driver);
        }

        [Given(@"I visit ""(.*)""")]
        public void GivenIVisit(string siteUrl)
        {
            _siteUrl = EnvironmentVariableOverride.GetAppSettingPerEnvironment(siteUrl);

            loginPage.goTo(_siteUrl);
            Assert.IsTrue(loginPage.ValidateTitle());
        }
        
        [When(@"I enter username in the ""(.*)"" field")]
        public void WhenIEnterUsernameInTheField(string userName)
        {
            _username = EnvironmentVariableOverride.GetAppSettingPerEnvironment(userName);           
        }
        
        [When(@"I enter password in the ""(.*)"" field")]
        public void WhenIEnterPasswordInTheField(string password)
        {
            _password = EnvironmentVariableOverride.GetAppSettingPerEnvironment(password);
        }
        
        [When(@"I press the ""(.*)"" button")]
        public void WhenIPressTheButton(string login)
        {
            loginPage.Login(_username, _password);
        }

        [Then(@"I should see the Farmers Home page")]
        public void ThenIShouldSeeTheFarmersHomePage()
        {
            Assert.IsTrue(loginPage.ISLandingPageVisible());
        }


    }
}
