# Computer Builder MVC Application

## Overview
This project is an ASP.NET Core MVC application built with .NET 9.0. It allows users to browse computer components, customize pre-configured computer builds, manage a shopping cart, and proceed through a checkout process. The application also includes features for user feedback and product reviews.

## Project Structure

-   **`/` (Root Directory)**
    -   `ComputerBuilderMvcApp.csproj`: The MSBuild project file defining project properties, dependencies, and target framework (net9.0).
    -   `ComputerBuilderMvcApp.sln`: Visual Studio Solution file.
    -   `Program.cs`: Application entry point, service configuration, and HTTP request pipeline setup.
    -   `appsettings.json`: Application configuration settings.
    -   `libman.json`: Configuration for client-side library management.
    -   `documentation.txt`: Developer notes and analysis for the project.
    -   `README.md`: This file.
-   **`Areas/Identity/`**: Contains pages and services for ASP.NET Core Identity, managing user authentication and authorization.
-   **`Controllers/`**: Handles incoming HTTP requests and orchestrates responses.
    -   `BuilderController.cs`: Manages computer customization logic.
    -   `CartController.cs`: Handles shopping cart operations.
    -   `ComponentsController.cs`: Manages display and interaction with individual components.
    -   `HomeController.cs`: Serves main pages like Home, Contact, and Feedback.
    -   `ReviewController.cs`: Manages user reviews for products.
-   **`Data/`**: Contains data-related files.
    -   `DbContext.cs`: Entity Framework Core database context.
    -   `Migrations/`: EF Core database migration files.
    -   `dbContext.db`: SQLite database file.
-   **`Models/`**: Defines C# classes representing the application's data entities (e.g., [`Cart`](Models/Cart.cs), [`Component`](Models/Component.cs), `Customer`, `Order`, [`Review`](Models/Review.cs)).
-   **`Services/`**: Intended for business logic services (structure present, specific services to be implemented as needed).
-   **`ViewModels/`**: Contains C# classes specifically designed to pass data between controllers and views (e.g., [`FeedbackViewModel`](ViewModels/FeedbackViewModel.cs)).
-   **`Views/`**: Contains Razor (.cshtml) files for rendering the HTML user interface.
-   **`wwwroot/`**: Serves static files:
    -   `css/`: Stylesheets.
    -   `js/`: JavaScript files (e.g., [`site.js`](wwwroot/js/site.js) for client-side interactivity).
    -   `images/`: Image assets, including rating stars.
    -   `lib/`: Client-side libraries.
-   **`.config/`**: Contains configuration files for .NET tools, like `dotnet-tools.json`.
-   **`Properties/`**: Contains project property files like `launchSettings.json` which defines profiles for launching the application.

## Setup Instructions

1.  **Prerequisites**:
    *   .NET SDK 9.0 or later.
2.  **Clone the Repository**:
    ```bash
    git clone <repository-url>
    cd ComputerBuilderMvcApp
    ```
3.  **Restore Dependencies**:
    ```bash
    dotnet restore
    ```
4.  **Database Migrations**:
    Ensure Entity Framework Core tools are installed. If not, you might need to install them (globally or as a local tool as per `.config/dotnet-tools.json`).
    Apply migrations to create the SQLite database and schema:
    ```bash
    dotnet ef database update
    ```
5.  **Run the Application**:
    ```bash
    dotnet run
    ```
6.  **Access the Application**:
    Open a web browser and navigate to the URL specified in the application output (e.g., `http://localhost:5000` or `https://localhost:5001`). Check `Properties/launchSettings.json` for specific ports.

## Features

-   **User Authentication**: Registration and login using ASP.NET Core Identity.
-   **Component Browsing**: View a catalog of computer components.
-   **Computer Customization**:
    -   Browse pre-configured computer builds.
    -   Customize components (CPU, RAM, GPU, etc.) for selected builds.
    -   Dynamic build summary with real-time price updates on the client-side ([`wwwroot/js/site.js`](wwwroot/js/site.js) - `updateBuildSummary`).
-   **Shopping Cart**:
    -   Add customized builds or individual components to the cart.
    -   View and manage cart contents.
    -   Mini-cart summary in the header with item count and total price ([`wwwroot/js/site.js`](wwwroot/js/site.js) - `updateCartSummaryDisplay`).
-   **Checkout Process**: Finalize selections and proceed to an order confirmation step.
-   **Product Reviews**:
    -   Submit reviews with star ratings and comments for components/products.
    -   Interactive star rating input ([`wwwroot/js/site.js`](wwwroot/js/site.js)).
-   **User Interaction**:
    -   Contact page for inquiries.
    -   Feedback submission page ([`ViewModels/FeedbackViewModel.cs`](ViewModels/FeedbackViewModel.cs), [`Views/Home/Feedback.cshtml`](Views/Home/Feedback.cshtml)).
-   **Email Notifications**: Utilizes SendGrid for sending emails (e.g., feedback confirmation, order updates).

## Technologies Used

-   **Backend**: ASP.NET Core MVC (.NET 9.0)
-   **Database**: Entity Framework Core with SQLite
-   **Authentication**: ASP.NET Core Identity
-   **Frontend**: HTML, CSS, JavaScript
    -   Client-side libraries (e.g., Bootstrap, jQuery) managed via LibMan (`libman.json`).
-   **Email Service**: SendGrid
-   **JSON Handling**: Newtonsoft.Json
-   **Development Environment**: Visual Studio Code, .NET SDK, EF Core Tools

## Usage

-   Start on the home page to view featured computers or navigate directly to component listings.
-   Select a pre-configured computer to view its details and begin customization.
-   Use the dropdowns to select desired components; the summary and total price will update automatically.
-   Add your configuration or individual components to the shopping cart.
-   Access the cart to review items and proceed to checkout.
-   Leave reviews for products you've interacted with.
-   Use the Contact or Feedback pages for inquiries or to provide site feedback.

## License
This project is licensed under the terms specified in the `LICENSE.TXT` file.
```// filepath: c:\School\COMP466\TMA3A\EcommerceP4\ComputerBuilderMvcApp\README.md
# Computer Builder MVC Application

## Overview
This project is an ASP.NET Core MVC application built with .NET 9.0. It allows users to browse computer components, customize pre-configured computer builds, manage a shopping cart, and proceed through a checkout process. The application also includes features for user feedback and product reviews.

## Project Structure

-   **`/` (Root Directory)**
    -   `ComputerBuilderMvcApp.csproj`: The MSBuild project file defining project properties, dependencies, and target framework (net9.0).
    -   `ComputerBuilderMvcApp.sln`: Visual Studio Solution file.
    -   `Program.cs`: Application entry point, service configuration, and HTTP request pipeline setup.
    -   `appsettings.json`: Application configuration settings.
    -   `libman.json`: Configuration for client-side library management.
    -   `documentation.txt`: Developer notes and analysis for the project.
    -   `README.md`: This file.
-   **`Areas/Identity/`**: Contains pages and services for ASP.NET Core Identity, managing user authentication and authorization.
-   **`Controllers/`**: Handles incoming HTTP requests and orchestrates responses.
    -   `BuilderController.cs`: Manages computer customization logic.
    -   `CartController.cs`: Handles shopping cart operations.
    -   `ComponentsController.cs`: Manages display and interaction with individual components.
    -   `HomeController.cs`: Serves main pages like Home, Contact, and Feedback.
    -   `ReviewController.cs`: Manages user reviews for products.
-   **`Data/`**: Contains data-related files.
    -   `DbContext.cs`: Entity Framework Core database context.
    -   `Migrations/`: EF Core database migration files.
    -   `dbContext.db`: SQLite database file.
-   **`Models/`**: Defines C# classes representing the application's data entities (e.g., [`Cart`](Models/Cart.cs), [`Component`](Models/Component.cs), `Customer`, `Order`, [`Review`](Models/Review.cs)).
-   **`Services/`**: Intended for business logic services (structure present, specific services to be implemented as needed).
-   **`ViewModels/`**: Contains C# classes specifically designed to pass data between controllers and views (e.g., [`FeedbackViewModel`](ViewModels/FeedbackViewModel.cs)).
-   **`Views/`**: Contains Razor (.cshtml) files for rendering the HTML user interface.
-   **`wwwroot/`**: Serves static files:
    -   `css/`: Stylesheets.
    -   `js/`: JavaScript files (e.g., [`site.js`](wwwroot/js/site.js) for client-side interactivity).
    -   `images/`: Image assets, including rating stars.
    -   `lib/`: Client-side libraries.
-   **`.config/`**: Contains configuration files for .NET tools, like `dotnet-tools.json`.
-   **`Properties/`**: Contains project property files like `launchSettings.json` which defines profiles for launching the application.

## Setup Instructions

1.  **Prerequisites**:
    *   .NET SDK 9.0 or later.
2.  **Clone the Repository**:
    ```bash
    git clone <repository-url>
    cd ComputerBuilderMvcApp
    ```
3.  **Restore Dependencies**:
    ```bash
    dotnet restore
    ```
4.  **Database Migrations**:
    Ensure Entity Framework Core tools are installed. If not, you might need to install them (globally or as a local tool as per `.config/dotnet-tools.json`).
    Apply migrations to create the SQLite database and schema:
    ```bash
    dotnet ef database update
    ```
5.  **Run the Application**:
    ```bash
    dotnet run
    ```
6.  **Access the Application**:
    Open a web browser and navigate to the URL specified in the application output (e.g., `http://localhost:5000` or `https://localhost:5001`). Check `Properties/launchSettings.json` for specific ports.

## Features

-   **User Authentication**: Registration and login using ASP.NET Core Identity.
-   **Component Browsing**: View a catalog of computer components.
-   **Computer Customization**:
    -   Browse pre-configured computer builds.
    -   Customize components (CPU, RAM, GPU, etc.) for selected builds.
    -   Dynamic build summary with real-time price updates on the client-side ([`wwwroot/js/site.js`](wwwroot/js/site.js) - `updateBuildSummary`).
-   **Shopping Cart**:
    -   Add customized builds or individual components to the cart.
    -   View and manage cart contents.
    -   Mini-cart summary in the header with item count and total price ([`wwwroot/js/site.js`](wwwroot/js/site.js) - `updateCartSummaryDisplay`).
-   **Checkout Process**: Finalize selections and proceed to an order confirmation step.
-   **Product Reviews**:
    -   Submit reviews with star ratings and comments for components/products.
    -   Interactive star rating input ([`wwwroot/js/site.js`](wwwroot/js/site.js)).
-   **User Interaction**:
    -   Contact page for inquiries.
    -   Feedback submission page ([`ViewModels/FeedbackViewModel.cs`](ViewModels/FeedbackViewModel.cs), [`Views/Home/Feedback.cshtml`](Views/Home/Feedback.cshtml)).
-   **Email Notifications**: Utilizes SendGrid for sending emails (e.g., feedback confirmation, order updates).

## Technologies Used

-   **Backend**: ASP.NET Core MVC (.NET 9.0)
-   **Database**: Entity Framework Core with SQLite
-   **Authentication**: ASP.NET Core Identity
-   **Frontend**: HTML, CSS, JavaScript
-   **Email Service**: SendGrid
-   **JSON Handling**: Newtonsoft.Json** assignment 3
-   **Development Environment**: Visual Studio, .NET SDK, EF Core Tools, SQLite

## Usage

-   Start on the home page to view featured computers or navigate directly to component listings.
-   Select a pre-configured computer to view its details and begin customization.
-   Use the dropdowns to select desired components; the summary and total price will update automatically.
-   Add your configuration or individual components to the shopping cart.
-   Access the cart to review items and proceed to checkout.
-   Leave reviews for products you've interacted with.
-   Use the Contact or Feedback pages for inquiries or to provide site feedback.

