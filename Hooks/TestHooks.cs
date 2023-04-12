using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using AventStack.ExtentReports.Reporter;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using SeleniumSpecFlowProject.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Tracing;

namespace SanquoteTestAutomation.Hooks
{
    [Binding]
    public sealed class TestHooks
    {
        private static ExtentTest featureName;
        private static ExtentTest scenario;
        private static ExtentReports extent;
        public static string ReportPath;
        private static Random random = new Random();
        private static string feature = String.Empty;
        public static bool isTDMFailure = false;
        public static string TDMErrorMessage;

        [Before]
        public static void CreateWebDriver(ScenarioContext scenarioContext)
        {
            IWebDriver driver = CreateDriver();
            scenarioContext["WEB_DRIVER"] = driver;
        }

        public static IWebDriver CreateDriver(string remoteUri = null)
        {
            IWebDriver driver = new ChromeDriver();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
            return driver;
        }


        [After]
        public static void CloseWebDriver(ScenarioContext scenarioContext)
        {
            var driver = scenarioContext["WEB_DRIVER"] as IWebDriver;
            driver.Quit();
        }

        [BeforeTestRun]
        public static void BeforeTestRun()
        {

            string basePath = AppDomain.CurrentDomain.BaseDirectory.Replace("\\bin\\Debug\\net6.0", "");
            string reportPath = basePath + "\\Report\\";
            bool exists = System.IO.Directory.Exists(reportPath);

            if (!exists)
                System.IO.Directory.CreateDirectory(reportPath);

            string path = reportPath + "index.html";
            ExtentHtmlReporter htmlReporter = new ExtentHtmlReporter(path);
            htmlReporter.Config.Theme = AventStack.ExtentReports.Reporter.Configuration.Theme.Standard;
            extent = new ExtentReports();
            extent.AttachReporter(htmlReporter);
        }

        [BeforeFeature]
        public static void BeforeFeature(FeatureContext featureContext)
        {
            //Create dynamic feature name
            featureName = extent.CreateTest<Feature>(featureContext.FeatureInfo.Title);
            Console.WriteLine("BeforeFeature");
        }


        [BeforeScenario]
        public void BeforeScenario(ScenarioContext scenarioContext)
        {
            Console.WriteLine("BeforeScenario");
            scenario = featureName.CreateNode<Scenario>(scenarioContext.ScenarioInfo.Title);


        }

        [AfterStep]
        public void InsertReportingSteps(ScenarioContext scenarioContext)
        {
            var stepType = scenarioContext.StepContext.StepInfo.StepDefinitionType.ToString();
            if (scenarioContext.TestError == null)
            {
                if (stepType == "Given")
                    scenario.CreateNode<Given>(scenarioContext.StepContext.StepInfo.Text);
                else if (stepType == "When")
                    scenario.CreateNode<When>(scenarioContext.StepContext.StepInfo.Text);
                else if (stepType == "Then")
                    scenario.CreateNode<Then>(scenarioContext.StepContext.StepInfo.Text);
                else if (stepType == "And")
                    scenario.CreateNode<And>(scenarioContext.StepContext.StepInfo.Text);
            }
            else if (scenarioContext.TestError != null)
            {
                if (stepType == "Given")
                {
                    scenario.CreateNode<Given>(scenarioContext.StepContext.StepInfo.Text).Fail(scenarioContext.TestError.Message);
                    string path = GetShot(scenarioContext);
                    scenario.AddScreenCaptureFromPath(".\\" + path.Substring(path.Substring(0, path.LastIndexOf("\\")).LastIndexOf("\\") + 1));
                }
                else if (stepType == "When")
                {
                    scenario.CreateNode<When>(scenarioContext.StepContext.StepInfo.Text).Fail(scenarioContext.TestError.Message);
                    string path = GetShot(scenarioContext);
                    scenario.AddScreenCaptureFromPath(".\\" + path.Substring(path.Substring(0, path.LastIndexOf("\\")).LastIndexOf("\\") + 1));
                }
                else if (stepType == "Then")
                {
                    scenario.CreateNode<Then>(scenarioContext.StepContext.StepInfo.Text).Fail(scenarioContext.TestError.Message);
                    string path = GetShot(scenarioContext);
                    scenario.AddScreenCaptureFromPath(".\\" + path.Substring(path.Substring(0, path.LastIndexOf("\\")).LastIndexOf("\\") + 1));
                }
                else if (stepType == "And")
                {
                    scenario.CreateNode<And>(scenarioContext.StepContext.StepInfo.Text).Fail(scenarioContext.TestError.Message);
                    string path = GetShot(scenarioContext);
                    scenario.AddScreenCaptureFromPath(".\\" + path.Substring(path.Substring(0, path.LastIndexOf("\\")).LastIndexOf("\\") + 1));
                }
            }
        }

        [AfterScenario]
        public void AfterScenario()
        {
            Console.WriteLine("AfterScenario");
            //implement logic that has to run after executing each scenario
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            extent.Flush();
        }

        public static string GetShot(ScenarioContext scenarioContext)
        {
            var driver = scenarioContext["WEB_DRIVER"] as IWebDriver;
            return GetScreenShot(driver, "\\bin\\Debug\\net48");
        }

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static string GetScreenShot(IWebDriver driver, string replaceBasePath)
        {

            //If Technical error  capture entire log messages
            IList<IWebElement> errormsgs = driver.FindElements(By.XPath("//div[contains(@class,'data-error-summary')]//li"));
            if (errormsgs.Count > 0)
            {
                if (errormsgs.ElementAt(0).Text.Contains("Due to technical problems"))
                    errormsgs.ElementAt(0).Click();
            }
            string basePath = AppDomain.CurrentDomain.BaseDirectory.Replace(replaceBasePath, "");
            string screenshootPath = basePath + "\\Report\\Screenshot\\";
            bool exists = System.IO.Directory.Exists(screenshootPath);

            if (!exists)
                System.IO.Directory.CreateDirectory(screenshootPath);

            string path = screenshootPath + RandomString(4) + ".png";
            Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();
            screenshot.SaveAsFile(path, ScreenshotImageFormat.Png);
            return path;
        }

    }
}
