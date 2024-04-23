# <span style="color:blue">Domus Server API</span>

Welcome to the Domus Server API using NET 7, powering the innovative furniture quotation application. Our API serves as the backbone for seamless interaction between the Domus web application and the database, facilitating efficient browsing, customization, and quotation generation for furniture pieces.

### Features

- <span style="color:green">**Comprehensive Catalog**</span>: Browse through an extensive catalog of high-quality furniture options, categorized for easy navigation.
- <span style="color:green">**Customization Capabilities**</span>: Enable users to customize their selections with various options including fabrics, finishes, dimensions, and features.
- <span style="color:green">**Instant Quotation Generation**</span>: Generate accurate quotations instantly based on the user's selected customizations, eliminating the need for manual calculations.
- <span style="color:green">**Secure Authentication**</span>: Ensure secure access to API endpoints with robust authentication mechanisms.
- <span style="color:green">**Scalable Architecture**</span>: Built on a scalable architecture to handle a growing number of users and requests with ease.

### Contributors

- <span style="color:purple">[Nguyen Hung Hai](https://github.com/HaiHungNguyenn)</span> - Backend Developer
- <span style="color:purple">[Nguyen Thanh Duy](https://github.com/duykasama)</span> - Backend Developer

### Libraries and Tools

Domus Server API utilizes the following libraries:

- <span style="color:orange">**Entity Framework Core**</span>: Entity Framework Core is used for database access and management, providing a convenient and efficient way to interact with the underlying database.
- <span style="color:orange">**Swagger UI**</span>: Swagger UI is integrated for API documentation, allowing developers to explore and test API endpoints with ease.
- <span style="color:orange">**Identity**</span>: Identity provides authentication and authorization functionalities, ensuring secure access to API endpoints.
- <span style="color:orange">**DbUp**</span>: DbUp is used for database schema migrations, enabling seamless updates to the database schema without manual intervention.
- <span style="color:orange">**NLog**</span>: NLog is utilized for logging, allowing for effective monitoring and debugging of the application.
- <span style="color:orange">**Azure VM**</span>: Azure VM is utilized for hosting the Domus Server API, providing scalable and reliable infrastructure.
- <span style="color:orange">**Blob Storage**</span>: Blob Storage is used for storing and managing large binary data such as images and files associated with furniture items.
- <span style="color:orange">**Google OAuth2**</span>: Google OAuth2 is integrated for authentication, enabling users to log in using their Google accounts.
- <span style="color:orange">**SQL Server**</span>: SQL Server is the chosen database management system for storing and managing data efficiently.
- <span style="color:orange">**GitHub Actions**</span>: GitHub Actions are used for continuous integration and deployment, automating the build, test, and deployment processes.
- <span style="color:orange">**AutoMapper**</span>: AutoMapper is used for mapping between different objects, simplifying the process of transferring data between layers and reducing manual mapping code.
- <span style="color:orange">**Docker**</span>: Docker is used for containerization for deploying purpose.

### Core Concepts
- <span style="color:brown">**Unit of Work Pattern**</span>: The Unit of Work pattern is implemented for managing transactions and coordinating multiple repository operations within a single transaction.
- <span style="color:brown">**Repository Pattern**</span>: The Repository pattern is used to abstract data access and provide a consistent interface for interacting with different data entities.
- <span style="color:brown">**JWT**</span>: JWT (JSON Web Token) is utilized for secure authentication and authorization, allowing users to access protected resources by providing digitally signed tokens.
