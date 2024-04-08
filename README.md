# ProjectTisa
![C#](https://img.shields.io/badge/c%23-%23239120.svg?logo=csharp&logoColor=white)
![.Net](https://img.shields.io/badge/.NET-5C2D91?logo=.net&logoColor=white)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-316192?logo=postgresql&logoColor=white)
[![GitHub license](https://img.shields.io/badge/license-MIT-blue.svg)](https://github.com/Fetu56/ProjectTisa-BackEnd/blob/dev/LICENSE.txt)

**Learnin project.**
API for online shop with authorization.

Using **Entity Framework** as ORM for access databases:
* **PostgreSQL** as primary database for API. 
* **SQLite** as secondary database for tests.

Using **[Pinata](https://www.pinata.cloud/)** as external storage for content.

## Controllers
### ManageControllers
* [AdminController](/ProjectTisa.Docs/ManageControllers/AdminController.md)
* [ManagerController](/ProjectTisa.Docs/ManageControllers/ManagerController.md)
### UserRelatedControllers
* [AuthController](/ProjectTisa.Docs/UserRelatedControllers/AuthController.md)
* [UserController](/ProjectTisa.Docs/UserRelatedControllers/UserController.md)
### CrudControllers
* [CategoryController](/ProjectTisa.Docs/CrudControllers/CategoryController.md)
* [DiscountController](/ProjectTisa.Docs/CrudControllers/DiscountController.md)
* [NotificationController](/ProjectTisa.Docs/CrudControllers/NotificationController.md)
* [OrderController](/ProjectTisa.Docs/CrudControllers/OrderController.md)
* [ProductController](/ProjectTisa.Docs/CrudControllers/ProductController.md)

# Set up
## Configuration file
For successful usage of project need to set up `appsettings.json` configuration file at `/ProjectTisa` directory.
### Example of `appsettings.json` file:
```json5
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    },
    "File": {
      "Path": "app.log",
      "Append": true,
      "MinLevel": "Warning",
      "FileSizeLimitBytes": 0,
      "MaxRollingFiles": 0
    }
  },
  "AllowedHosts": "*",
  "AppInfo": {
    "ApplicationName": "Project Tisa",
    "CurrentHost": "www.tisa-backend.com",
    "AuthData": {
      "ExpirationTime": "3:00:00:00", //d:hh:mm:ss
      "Issuer": "www.tisa-backend.com",
      "Audience": "www.tisa-frontend.com",
      "IssuerSigningKey": "**",
      "IterationCount": **,
      "SaltSize": **,
      "HashAlgorithmOID": "**"
    },
    "SmtpData": {
      "Port": 587,
      "FromEmail": "**",
      "Password": "**"
    },
    "ExternalStorage": {
      "PostUrl": "https://api.pinata.cloud/pinning/pinFileToIPFS",
      "GetUrl": "https://api.pinata.cloud/data/pinList?includeCount=true",
      "Auth": "*access token to Pinata*"
    },
    "Version": "v1.0"
  },
  "ConnectionStrings": {
    "DefaultConnection": "**" //Connection string to primary database.
  }
}
```
`**` has to be changed.
## Code-first database creation
For Code-first approach with Entity Framework use following steps:
1. Open Package Manager Console in VS.
2. Run `PM> add-migration Initial`
3. Run `PM> update-database Initial`

# Contacts
[![Outlook email](https://img.shields.io/badge/Outlook-0078D4?logo=microsoft-outlook&logoColor=white)](mailto:alpritor@outlook.com)
<br>alpritor@outlook.com

[![Telegram](https://img.shields.io/badge/Telegram-2CA5E0?logo=telegram&logoColor=white)](https://t.me/alpritor)
<br>@Alpritor