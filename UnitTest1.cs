using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System.Text;

namespace RevueCraftersNoPOM
{
    public class Tests
    {
        private string BaseUrl = "https://d3s5nxhwblsjbi.cloudfront.net";
        private static string? lastCreatedRevueTitle;
        private static string? lastCreatedRevueDescription;
        private IWebDriver driver;
        private Actions actions;
        private WebDriverWait wait;
        
        

        [OneTimeSetUp]
        public void SetUp()
        {
            var chromeOptions = new ChromeOptions();


            chromeOptions.AddUserProfilePreference("profile.password_manager_enabled", false);
            chromeOptions.AddArguments("--disable-search-engine-choice-screen");

            driver = new ChromeDriver(chromeOptions);


            actions = new Actions(driver);

            driver.Navigate().GoToUrl(BaseUrl);

            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10); 
            
            driver.FindElement(By.XPath("//a[@class='nav-link text-dark' and text()='Login']")).Click();
            driver.FindElement(By.Id("form3Example3")).SendKeys("test@test1.com");            
            driver.FindElement(By.Id("form3Example4")).SendKeys("123456");
            driver.FindElement(By.Id("form3Example4")).SendKeys(Keys.Enter);

        }

        [OneTimeTearDown]
        public void TearDown()
        {
            driver.Quit();
            driver.Dispose();
        }
        [Test, Order(1)]
        public void CreateRevueWithInvalidDataTest()
        {
            driver.Navigate().GoToUrl($"{BaseUrl}/Revue/Create");

            var formCard = driver.FindElement(By.XPath("//div[@class='card-body p-md-5']"));
            
            actions.ScrollToElement(formCard).Perform();

            var titleInput = driver.FindElement(By.Id("form3Example1c"));

            titleInput.SendKeys("");

            var descriptionInput = driver.FindElement(By.Id("form3Example4cd"));

            descriptionInput.SendKeys("");

            driver.FindElement(By.XPath("//button[@type='submit']")).Click();
            
            Assert.That(driver.Url, Is.EqualTo("https://d3s5nxhwblsjbi.cloudfront.net/Revue/Create"));

            var mainErrorMsg = driver.FindElement(By.XPath("//li [text()='Unable to create new Revue!']"));

            Assert.That(mainErrorMsg.Text.Trim(), Is.EqualTo("Unable to create new Revue!"), "Main error message is not as expected.");
        }
        [Test, Order(2)]
        public void CreateRandomTitleDescriptionTest()
        {
            lastCreatedRevueTitle = "Revue:" + GenerateRandomString(6);
            lastCreatedRevueDescription = "Description:" + GenerateRandomString(12);

            driver.Navigate().GoToUrl($"{BaseUrl}/Revue/Create#createRevue");

            var createForm = driver.FindElement(By.CssSelector("div.card-body.p-md-5"));
            Actions actions = new Actions(driver);
            actions.MoveToElement(createForm).Perform();
                        
            driver.FindElement(By.Id("form3Example1c")).SendKeys(lastCreatedRevueTitle);
            driver.FindElement(By.Id("form3Example4cd")).SendKeys(lastCreatedRevueDescription);
            driver.FindElement(By.XPath("//button[@class='btn btn-primary btn-lg']")).Click();
                        
            Assert.That(driver.Url.Equals($"{BaseUrl}/Revue/MyRevues#createRevue"), "URL is not as expected.");

            var revues = driver.FindElements(By.CssSelector("div.card.mb-4.box-shadow"));
            var lastRevueTitle = revues.Last().FindElement(By.CssSelector("div.text-muted.text-center"));

            string actualRevueTitle = lastRevueTitle.Text.Trim();
            Assert.That(actualRevueTitle, Is.EqualTo(lastCreatedRevueTitle), "Last revue title is not as expected!");
        }

        [Test, Order(3)]
        public void SearchForReviewTest()
        {
            driver.Navigate().GoToUrl($"{BaseUrl}/Revue/MyRevues#myRevues");

            var searchField = driver.FindElement(By.XPath("//input[@id='keyword']"));
            Actions actions = new Actions(driver);
            actions.MoveToElement(searchField).Perform();


            var searchInput = driver.FindElement(By.XPath("//input[@id='keyword']"));
            searchInput.SendKeys(lastCreatedRevueTitle);
            driver.FindElement(By.XPath("//i[@class='fas fa-search']")).Click();

            var revueTitle = driver.FindElement(By.CssSelector(".text-muted.text-center")).Text;
            Assert.That(revueTitle, Is.EqualTo(lastCreatedRevueTitle), "Search title and result title do not match!");
        }

        [Test, Order(4)]
        public void EditLastCreatedRevueTitleTest()
        {
            lastCreatedRevueTitle = "Revue N: " + GenerateRandomString(5);
            lastCreatedRevueDescription = "Revue Description: " + GenerateRandomString(10);

            driver.Navigate().GoToUrl($"{BaseUrl}/Revue/Create#createRevue");

            var createForm = driver.FindElement(By.CssSelector("div.card-body.p-md-5"));
            Actions actions = new Actions(driver);
            actions.MoveToElement(createForm).Perform();

            driver.FindElement(By.Id("form3Example1c")).SendKeys(lastCreatedRevueTitle);
            driver.FindElement(By.Id("form3Example4cd")).SendKeys(lastCreatedRevueDescription);
            driver.FindElement(By.CssSelector("button.btn.btn-primary.btn-lg")).Click();

            string currentUrl = driver.Url;
            Assert.That(currentUrl, Is.EqualTo($"{BaseUrl}/Revue/MyRevues#createRevue"), "The page should redirect to My Revues.");

            var revues = driver.FindElements(By.CssSelector("div.card.mb-4.box-shadow"));
            var lastRevueTitleElement = revues.Last().FindElement(By.CssSelector("div.text-muted.text-center"));

            string actualRevueTitle = lastRevueTitleElement.Text.Trim();
            Assert.That(actualRevueTitle, Is.EqualTo(lastCreatedRevueTitle), "The last created revue title does not match the expected value.");

        }

        [Test, Order(5)]
        public void DeleteLastCreatedRevueTitleTest()
        {
            driver.Navigate().GoToUrl($"{BaseUrl}/Revue/MyRevues");

            var revues = driver.FindElements(By.CssSelector("div.card.mb-4.box-shadow"));
            Assert.IsTrue(revues.Count > 0, "No revues were found on the page");

            var lastRevueElement = revues.Last();
            Actions actions = new Actions(driver);
            actions.MoveToElement(lastRevueElement).Perform();

            var deleteButton = lastRevueElement.FindElement(By.CssSelector(" div > div.card-body.card-footer > div > div > a:nth-child(3)"));
            deleteButton.Click();

            var currentUrl = driver.Url;
            Assert.That(currentUrl, Is.EqualTo($"{BaseUrl}/Revue/MyRevues"), "Not redirected correctly after deleting a revue.");

            revues = driver.FindElements(By.CssSelector("div.card.mb-4.box-shadow"));
            var lastRevueTitleElement = revues.Last().FindElement(By.CssSelector("div.text-muted.text-center"));

            string actualRevueTitle = lastRevueTitleElement.Text.Trim();
            Assert.That(actualRevueTitle, Is.Not.EqualTo(lastCreatedRevueTitle), "Revues count did not decrease by 1");
        }

        [Test, Order(6)]
        public void SearchForDeletedRevue()
        {
            driver.Navigate().GoToUrl($"{BaseUrl}/Revue/MyRevues");
            
            var searchField = driver.FindElement(By.XPath("//input[@id='keyword']"));            

            Actions actions = new Actions(driver);
            actions.MoveToElement(searchField).Perform();
            

            var searchInput = driver.FindElement(By.XPath("//input[@id='keyword']"));
            searchInput.SendKeys(lastCreatedRevueTitle);
            driver.FindElement(By.XPath("//i[@class='fas fa-search']")).Click();

            var errorMessage = driver.FindElement(By.XPath("//span[@class='col-12 text-muted']"));
            string errorMessageTrimmed = errorMessage.Text.Trim();

            Assert.That(errorMessageTrimmed, Is.EqualTo("No Revues yet!"));
        }

 



        public static string GenerateRandomString(int length)
        {
            char[] chars =
                "abcdefghijklmopqrstuvwxyz ".ToCharArray();
            if (length <= 0)
            {
                throw new ArgumentException("Length must be greater than zero!", nameof(length));
            }

            var random = new Random();
            var result = new StringBuilder(length);

            for (int i = 0; i < length; i++) 
            {
                result.Append(chars[random.Next(chars.Length)]);
            }
            return result.ToString();
        }
    }
}