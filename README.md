**.NET Core Web API Project**

This is a sample .NET Core Web API project using .NET 7.0 TargetFramework. The project utilizes various dotnet technologies, including:

Microsoft.AspNetCore.Authentication.JwtBearer
Microsoft.AspNetCore.Identity.EntityFrameworkCore
Microsoft.AspNetCore.ResponseCompression
Microsoft.EntityFrameworkCore
Microsoft.EntityFrameworkCore.Design
Microsoft.EntityFrameworkCore.SqlServer
Microsoft.EntityFrameworkCore.Tools
NETCore.MailKit
Swashbuckle.AspNetCore
Swashbuckle.AspNetCore.Filters
Project Functions
The project consists of the following functions:

**1. Auth**
register: Allows users to register a new account.

login: Provides user authentication by logging in with valid credentials.

reset-password: Allows users to reset their account password.

forgot-password: Helps users recover their forgotten password.

confirm email: Enables users to confirm their email address.

verify email: Provides email verification for users.

send email verification: Sends an email verification link to users.

**3. Admin**
get-user: Retrieves user information.
get-role: Retrieves role information.
create role: Allows the creation of new roles.
change role: Modifies the role of a user.
edit user: Allows editing of user details.
delete user: Deletes a user from the system.
get-user-by-id: Retrieves user information based on their unique identifier.
Feel free to explore the project and make use of the provided functionalities!

**Prerequisites**
To run this project, ensure that you have the following installed:

**.NET 7.0 SDK or a compatible version**
Microsoft SQL Server or a compatible database
Any suitable development environment, such as Visual Studio or Visual Studio Code
Getting Started
Clone this repository to your local machine.
Open the project in your preferred development environment.
Update the connection string in the appsettings.json file to point to your database.
Build the project to restore dependencies.
Run the project using the appropriate commands for your development environment (e.g., dotnet run).
You should now be able to access the API endpoints using the provided routes.

**API Documentation**
The project utilizes Swagger UI to document and test the available API endpoints. Once the project is running, you can access the Swagger UI at the following URL: http://localhost:<port>/swagger

**License**
This project is licensed under the MIT License. Feel free to modify and use it as per your requirements.

If you encounter any issues or have any questions, please feel free to open an issue or reach out to the project contributors. Enjoy coding!
