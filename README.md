# RevueCrafters - NUnit Web Testing

RevueCrafters is an NUnit-based testing solution for the web application "Revue Crafters." This project follows a **No POM (Page Object Model)** approach, meaning that test logic directly interacts with web elements instead of using a separate page abstraction layer.

## Technologies Used
- **C#**
- **NUnit**
- **Selenium WebDriver**
- **ChromeDriver**
- **WebDriverWait & Actions API**

## Features
- Automates web UI testing for the Revue Crafters platform.
- Validates critical user flows, such as:
  - User login
  - Creating revues (with valid and invalid data)
  - Searching for revues
  - Editing revue details
  - Deleting revues and verifying their removal
- Uses `WebDriverWait` for synchronization.
- Implements `Actions` for enhanced interactions.
- Randomized title and description generation for test data.

## Test Cases
| Test Name | Description |
|-----------|-------------|
| `CreateRevueWithInvalidDataTest` | Ensures validation messages appear when submitting empty fields. |
| `CreateRandomTitleDescriptionTest` | Creates a revue with a randomly generated title and description. |
| `SearchForReviewTest` | Searches for the last created revue to verify its existence. |
| `EditLastCreatedRevueTitleTest` | Edits the last created revue and confirms the changes. |
| `DeleteLastCreatedRevueTitleTest` | Deletes the last created revue and ensures it is removed from the list. |
| `SearchForDeletedRevue` | Attempts to search for a deleted revue and verifies the appropriate error message. |

## Installation & Setup
1. Clone the repository:
   ```sh
   git clone https://github.com/yourusername/RevueCrafters.git
   ```
2. Open the solution in **Visual Studio**.
3. Install dependencies via NuGet Package Manager:
   ```sh
   Install-Package Selenium.WebDriver
   Install-Package Selenium.Support
   Install-Package NUnit
   Install-Package NUnit3TestAdapter
   ```
4. Ensure **ChromeDriver** is installed and matches your Chrome browser version.

## Running Tests
- Run tests directly from Visual Studio Test Explorer.
- Alternatively, use the following command in the terminal:
  ```sh
  dotnet test
  ```

## Notes
- The test suite initializes **ChromeDriver** and logs in to the application before executing test cases.
- The `BaseUrl` is set to `https://d3s5nxhwblsjbi.cloudfront.net`.
- Ensure your test credentials (`test@test1.com` / `123456`) exist in the application.

## Future Enhancements
- Implement **POM (Page Object Model)** for better test maintainability.
- Integrate with CI/CD pipelines (GitHub Actions / Azure DevOps).
- Extend test coverage for additional features.
