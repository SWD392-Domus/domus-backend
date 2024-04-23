# Domus Server API

Welcome to the Domus Server API using NET 7, powering the innovative furniture quotation application. Our API serves as the backbone for seamless interaction between the Domus web application and the database, facilitating efficient browsing, customization, and quotation generation for furniture pieces.

### Features

- **Comprehensive Catalog**: Browse through an extensive catalog of high-quality furniture options, categorized for easy navigation.
- **Customization Capabilities**: Enable users to customize their selections with various options including fabrics, finishes, dimensions, and features.
- **Instant Quotation Generation**: Generate accurate quotations instantly based on the user's selected customizations, eliminating the need for manual calculations.
- **Secure Authentication**: Ensure secure access to API endpoints with robust authentication mechanisms.
- **Scalable Architecture**: Built on a scalable architecture to handle a growing number of users and requests with ease.

### Contributors

- [Nguyen Hung Hai](https://github.com/HaiHungNguyenn) - Backend Developer
- [Nguyen Thanh Duy](https://github.com/duykasama) - Backend Developer

### Libraries and Tools

Domus Server API utilizes the following libraries:

- **Entity Framework Core**: Entity Framework Core is used for database access and management, providing a convenient and efficient way to interact with the underlying database.
- **Swagger UI**: Swagger UI is integrated for API documentation, allowing developers to explore and test API endpoints with ease.
- **Identity**: Identity provides authentication and authorization functionalities, ensuring secure access to API endpoints.
- **DbUp**: DbUp is used for database schema migrations, enabling seamless updates to the database schema without manual intervention.
- **NLog**: NLog is utilized for logging, allowing for effective monitoring and debugging of the application.
- **Azure VM**: Azure VM is utilized for hosting the Domus Server API, providing scalable and reliable infrastructure.
- **Blob Storage**: Blob Storage is used for storing and managing large binary data such as images and files associated with furniture items.
- **Google OAuth2**: Google OAuth2 is integrated for authentication, enabling users to log in using their Google accounts.
- **SQL Server**: SQL Server is the chosen database management system for storing and managing data efficiently.
- **GitHub Actions**: GitHub Actions are used for continuous integration and deployment, automating the build, test, and deployment processes.
- **AutoMapper**: AutoMapper is used for mapping between different objects, simplifying the process of transferring data between layers and reducing manual mapping code.
- **Docker**: Docker is used for containerization for deploying purpose.

### Core Concepts
- **Unit of Work Pattern**: The Unit of Work pattern is implemented for managing transactions and coordinating multiple repository operations within a single transaction.
- **Repository Pattern**: The Repository pattern is used to abstract data access and provide a consistent interface for interacting with different data entities.
- **JWT**: JWT (JSON Web Token) is utilized for secure authentication and authorization, allowing users to access protected resources by providing digitally signed tokens.
