using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;
using NUnit.Framework;
namespace SSHZhakar.Tests
{
    class UnitTests2
    {
        private IWebDriver driver;
        public IDictionary<string, object> vars { get; private set; }
        private IJavaScriptExecutor js;
        [SetUp]
        public void SetUp()
        {
            driver = new ChromeDriver();
            js = (IJavaScriptExecutor)driver;
            vars = new Dictionary<string, object>();
        }
        [TearDown]
        protected void TearDown()
        {
            driver.Quit();
        }
        private static string FILENAME = "zhakardata.xlsx";
        private static string FILENAME2 = "zhakardata.xlsx";


        public static IEnumerable<TestCaseData> LoginData()
        {
            return ExcelReader.ReadFromExcel(FILENAME, "Sh1");

        }

        public static IEnumerable<TestCaseData> RegistrationData()
        {
            return ExcelReader.ReadFromExcel(FILENAME2, "Sh2");

        }
        public static IEnumerable<TestCaseData> LogoutData()
        {
            return ExcelReader.ReadFromExcel(FILENAME2, "Sh1");

        }
        public static IEnumerable<TestCaseData> ChangeNameData()
        {
            return ExcelReader.ReadFromExcel(FILENAME2, "Sh3");

        }
        public static IEnumerable<TestCaseData> AddTerminalData()
        {
            return ExcelReader.ReadFromExcel(FILENAME2, "Sh4");

        }
        public static IEnumerable<TestCaseData> ChangeLastNameData()
        {
            return ExcelReader.ReadFromExcel(FILENAME2, "Sh3");

        }


        //[TestCase("alex@gmail.com","123321")]
        //[TestCase("alexander.speek@gmail.com", "123321")]
        [TestCaseSource("LoginData")]
        [Test]
        public void loginTest(string email,string password)
        {
            driver.Navigate().GoToUrl("https://localhost:5004/");
            driver.Manage().Window.Size = new System.Drawing.Size(814, 860);
            driver.FindElement(By.CssSelector("a:nth-child(2) > div")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.Name("Email")).Click();
            driver.FindElement(By.Name("Email")).SendKeys(email);
            driver.FindElement(By.Name("Password")).Click();
            driver.FindElement(By.Name("Password")).SendKeys(password);

            driver.FindElement(By.CssSelector("button")).Click();
            Thread.Sleep(1000);

            driver.FindElement(By.Id("header-photo")).Click();
            driver.FindElement(By.CssSelector("li:nth-child(1)")).Click();
            Thread.Sleep(1000);

            string text = driver.FindElement(By.CssSelector("#email_field")).Text;
            Assert.AreEqual(text, email);
            

        }
        [TestCaseSource("RegistrationData")]
        [Test]
        public void registration(string email,string password,string name,string lastname,string nickname)
        {
            driver.Navigate().GoToUrl("https://localhost:5004/");
            driver.Manage().Window.Size = new System.Drawing.Size(814, 860);
            driver.FindElement(By.CssSelector("a:nth-child(1) > div")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("body > div.container > div.reg-container > div > form > input[type=email]:nth-child(2)")).SendKeys(email);
            driver.FindElement(By.Name("Password")).Click();
            driver.FindElement(By.Name("Password")).SendKeys(password);
            driver.FindElement(By.Name("PasswordConfirm")).Click();
            driver.FindElement(By.Name("PasswordConfirm")).SendKeys(password);
            driver.FindElement(By.Name("Name")).Click();
            driver.FindElement(By.Name("Name")).SendKeys(name);
            driver.FindElement(By.Name("LastName")).Click();
            driver.FindElement(By.Name("LastName")).SendKeys(lastname);
            driver.FindElement(By.Name("NickName")).Click();
            driver.FindElement(By.Name("NickName")).SendKeys(nickname);
            driver.FindElement(By.CssSelector("button")).Click();
            Thread.Sleep(1000);

            driver.FindElement(By.CssSelector(".header-profile-username")).Click();
            driver.FindElement(By.CssSelector("li:nth-child(1)")).Click();
            Thread.Sleep(1000);

            string text = driver.FindElement(By.CssSelector("#email_field")).Text;
            Assert.AreEqual(text, email);
        }
        [TestCaseSource("LogoutData")]
        [Test]
        public void logOut(string email,string password)
        {
            driver.Navigate().GoToUrl("https://localhost:5004/");
            driver.Manage().Window.Size = new System.Drawing.Size(814, 860);
            driver.FindElement(By.CssSelector("a:nth-child(2) > div")).Click();
            Thread.Sleep(1000);

            driver.FindElement(By.Name("Email")).Click();
            driver.FindElement(By.Name("Email")).SendKeys(email);
            driver.FindElement(By.Name("Password")).Click();
            driver.FindElement(By.Name("Password")).SendKeys(password);
            driver.FindElement(By.CssSelector("button")).Click();
            Thread.Sleep(1000);

            driver.FindElement(By.CssSelector(".header-profile-username")).Click();
            driver.FindElement(By.CssSelector("li:nth-child(1)")).Click();
            Thread.Sleep(1000);

            string text = driver.FindElement(By.CssSelector("#email_field")).Text;
            if (text != email) { 
                Assert.Fail();
            }
            
            driver.FindElement(By.CssSelector(".header-profile-username")).Click();
            driver.FindElement(By.CssSelector("li:nth-child(3)")).Click();
            Thread.Sleep(1000);

            //driver.FindElement(By.CssSelector(".container")).Click();
            string mainPageText = driver.FindElement(By.CssSelector(".container")).Text;
            Assert.AreEqual(mainPageText, "ZHAKAR-SSH");


        }
        [TestCaseSource("ChangeNameData")]
        [Test]
        public void changeName(string email,string password,string newName)
        {
            driver.Navigate().GoToUrl("https://localhost:5004/");
            //driver.Manage().Window.Size = new System.Drawing.Size(814, 860);
            driver.FindElement(By.CssSelector("a:nth-child(2) > div")).Click();
            Thread.Sleep(1000);

            driver.FindElement(By.Name("Email")).Click();
            driver.FindElement(By.Name("Email")).SendKeys(email);
            driver.FindElement(By.CssSelector("form:nth-child(2)")).Click();
            driver.FindElement(By.Name("Password")).Click();
            driver.FindElement(By.Name("Password")).SendKeys(password);
            driver.FindElement(By.CssSelector("button")).Click();
            Thread.Sleep(1000);

            driver.FindElement(By.CssSelector(".header-profile-username")).Click();
            driver.FindElement(By.CssSelector("li:nth-child(1)")).Click();
            Thread.Sleep(1000);

            driver.FindElement(By.CssSelector("button:nth-child(4)")).Click();
            Thread.Sleep(500);

            driver.FindElement(By.Id("name-input")).Click();
            driver.FindElement(By.Id("name-input")).Clear();

            driver.FindElement(By.Id("name-input")).SendKeys(newName);
            driver.FindElement(By.CssSelector("button:nth-child(1)")).Click();
            Thread.Sleep(2000);

            string nName = driver.FindElement(By.Id("name_field")).Text;
            Assert.AreEqual(nName, newName);


        }
        [TestCaseSource("AddTerminalData")]
        [Test]
        public void addTerminal(string email,string password,string nameTerminal,string ip,string login,string terminalPassword)
        {
            driver.Navigate().GoToUrl("https://localhost:5004/");
            driver.Manage().Window.Size = new System.Drawing.Size(814, 860);
            driver.FindElement(By.CssSelector("a:nth-child(2) > div")).Click();
            Thread.Sleep(1000);

            driver.FindElement(By.Name("Email")).Click();
            driver.FindElement(By.Name("Email")).SendKeys(email);
            driver.FindElement(By.Name("Password")).Click();
            driver.FindElement(By.Name("Password")).SendKeys(password);
            driver.FindElement(By.Name("Password")).SendKeys(Keys.Enter);
            Thread.Sleep(1000);

            driver.FindElement(By.CssSelector(".add-connection-button-container")).Click();
            Thread.Sleep(500);

            driver.FindElement(By.Id("terminal-name-input")).Click();
            driver.FindElement(By.Id("terminal-name-input")).SendKeys(nameTerminal);
            driver.FindElement(By.Id("prelogin-id")).Click();
            Thread.Sleep(300);

            driver.FindElement(By.Id("remote-desktop-ip")).Click();
            driver.FindElement(By.Id("remote-desktop-ip")).SendKeys(ip);
            driver.FindElement(By.Id("remote-desktop-username")).Click();
            driver.FindElement(By.Id("remote-desktop-username")).SendKeys(login);
            driver.FindElement(By.Id("remote-desktop-password")).Click();
            driver.FindElement(By.Id("remote-desktop-password")).SendKeys(terminalPassword);
            driver.FindElement(By.CssSelector(".add-console-table:nth-child(2) button")).Click();
            Thread.Sleep(2000);

            string terminalNamen = driver.FindElement(By.CssSelector("body > div.container > div.terminal-link-container > div.terminal-link-title")).Text;
            Assert.AreEqual(terminalNamen, nameTerminal);
        }

        [TestCaseSource("ChangeLastNameData")]
        [Test]
        public void changeLastName(string email,string password,string newName)
        {
            driver.Navigate().GoToUrl("https://localhost:5004/");
            driver.FindElement(By.CssSelector("a:nth-child(2) > div")).Click();
            driver.FindElement(By.CssSelector("form:nth-child(2)")).Click();
            Thread.Sleep(1000);

            driver.FindElement(By.Name("Email")).Click();
            driver.FindElement(By.Name("Email")).SendKeys(email);
            driver.FindElement(By.Name("Password")).Click();
            driver.FindElement(By.Name("Password")).SendKeys(password);
            driver.FindElement(By.CssSelector("button")).Click();
            Thread.Sleep(1000);

            driver.FindElement(By.CssSelector(".header-profile-username")).Click();
            driver.FindElement(By.CssSelector("li:nth-child(1)")).Click();
            Thread.Sleep(1000);

            driver.FindElement(By.CssSelector("button:nth-child(4)")).Click();
            Thread.Sleep(1000);

            driver.FindElement(By.CssSelector("#edit-container > .info-row:nth-child(2)")).Click();
            driver.FindElement(By.Id("lastname-input")).Clear();

            driver.FindElement(By.Id("lastname-input")).SendKeys(newName);
            driver.FindElement(By.CssSelector("button:nth-child(1)")).Click();

            Thread.Sleep(2000);

            string nName = driver.FindElement(By.CssSelector("#lastname_field")).Text;
            Assert.AreEqual(nName, newName);
            driver.FindElement(By.Id("lastname_field")).Click();

        }
    }
}
