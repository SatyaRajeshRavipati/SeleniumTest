using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestStagingSite
{
    [TestClass]
    public class TestContactUs
    {
        static IWebDriver m_chromeDriver;

        [ClassInitialize]
        public static void ClassSetup(TestContext a)
        {
            string currentDir = Environment.CurrentDirectory;
            currentDir += "\\..\\..\\..\\..\\..\\ChromeDriver\\";
            m_chromeDriver = new ChromeDriver(currentDir);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            m_chromeDriver.Quit();
        }

        [TestInitialize]
        public void SetUp()
        {
            m_chromeDriver.Navigate().GoToUrl("http://staging.qaworks.com:1403/contact.aspx");
        }

        [TestCleanup]
        public void TearDown()
        {
            
        }

        public bool IsElementVisibeById(string idName)
        {
            bool isVisible = false;
            try
            {
                isVisible = m_chromeDriver.FindElement(By.Id(idName)).Displayed;
            }
            catch (Exception ex)
            {
                isVisible = false;
            }

            return isVisible;
        }

        [TestMethod]
        public void TestValidSubmission()
        {
            IWebElement elementName = m_chromeDriver.FindElement(By.Id("ctl00_MainContent_NameBox"));
            elementName.SendKeys("J.Bloggs");
            IWebElement elementEmail = m_chromeDriver.FindElement(By.Id("ctl00_MainContent_EmailBox"));
            elementEmail.SendKeys("j.Bloggs@qaworks.com");
            IWebElement elementMessageBox = m_chromeDriver.FindElement(By.Id("ctl00_MainContent_MessageBox"));
            elementMessageBox.SendKeys("please contact me I want to find out more");

            IWebElement elementSendButton = m_chromeDriver.FindElement(By.Id("ctl00_MainContent_SendButton"));

            elementSendButton.Click();

            Assert.IsFalse(IsElementVisibeById("ctl00_MainContent_rfvName"), "Your name is required");
            Assert.IsFalse(IsElementVisibeById("ctl00_MainContent_rfvEmailAddress"), "An Email address is required");
            Assert.IsFalse(IsElementVisibeById("ctl00_MainContent_revEmailAddress"), "Invalid Email Address");
            Assert.IsFalse(IsElementVisibeById("ctl00_MainContent_rfvMessage"), "Please type your message");
        }

        [TestMethod]
        public void TestEnteringBlankDataAndClickSend()
        {
            IWebElement elementName = m_chromeDriver.FindElement(By.Id("ctl00_MainContent_NameBox"));
            elementName.SendKeys("");
            IWebElement elementEmail = m_chromeDriver.FindElement(By.Id("ctl00_MainContent_EmailBox"));
            elementEmail.SendKeys("");
            IWebElement elementMessageBox = m_chromeDriver.FindElement(By.Id("ctl00_MainContent_MessageBox"));
            elementMessageBox.SendKeys("");

            IWebElement elementSendButton = m_chromeDriver.FindElement(By.Id("ctl00_MainContent_SendButton"));
            elementSendButton.Click();

            Assert.IsTrue(IsElementVisibeById("ctl00_MainContent_rfvName"), "Your name is required");
            Assert.IsTrue(IsElementVisibeById("ctl00_MainContent_rfvEmailAddress"), "An Email address is required");
            Assert.IsFalse(IsElementVisibeById("ctl00_MainContent_revEmailAddress"), "Invalid Email Address");
            Assert.IsTrue(IsElementVisibeById("ctl00_MainContent_rfvMessage"), "Please type your message");
        }

        [TestMethod]
        public void TestInvaidEmail()
        {
            IWebElement elementName = m_chromeDriver.FindElement(By.Id("ctl00_MainContent_NameBox"));
            elementName.SendKeys("J.Bloggs");
            IWebElement elementEmail = m_chromeDriver.FindElement(By.Id("ctl00_MainContent_EmailBox"));
            elementEmail.SendKeys("jBloggsqaworkscom");
            IWebElement elementMessageBox = m_chromeDriver.FindElement(By.Id("ctl00_MainContent_MessageBox"));
            elementMessageBox.SendKeys("please contact me I want to find out more");

            Assert.IsFalse(IsElementVisibeById("ctl00_MainContent_rfvName"), "Your name is required");
            Assert.IsFalse(IsElementVisibeById("ctl00_MainContent_rfvEmailAddress"), "An Email address is required");
            Assert.IsTrue(IsElementVisibeById("ctl00_MainContent_revEmailAddress"), "Invalid Email Address");
            Assert.IsFalse(IsElementVisibeById("ctl00_MainContent_rfvMessage"), "Please type your message");
        }

        [TestMethod]
        public void TestInvaidEmailAndClickSend()
        {
            IWebElement elementName = m_chromeDriver.FindElement(By.Id("ctl00_MainContent_NameBox"));
            elementName.SendKeys("J.Bloggs");
            IWebElement elementEmail = m_chromeDriver.FindElement(By.Id("ctl00_MainContent_EmailBox"));
            elementEmail.SendKeys("jBloggsqaworkscom");
            IWebElement elementMessageBox = m_chromeDriver.FindElement(By.Id("ctl00_MainContent_MessageBox"));
            elementMessageBox.SendKeys("please contact me I want to find out more");

            IWebElement elementSendButton = m_chromeDriver.FindElement(By.Id("ctl00_MainContent_SendButton"));
            elementSendButton.Click();

            Exception exception = null;
            try
            {
                Assert.IsFalse(IsElementVisibeById("ctl00_MainContent_rfvName"), "Your name is required");
                Assert.IsFalse(IsElementVisibeById("ctl00_MainContent_rfvEmailAddress"), "An Email address is required");
                Assert.IsTrue(IsElementVisibeById("ctl00_MainContent_revEmailAddress"), "Invalid Email Address");
                Assert.IsFalse(IsElementVisibeById("ctl00_MainContent_rfvMessage"), "Please type your message");
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            Assert.IsNull(exception, exception.Message);
        }
    }
}
