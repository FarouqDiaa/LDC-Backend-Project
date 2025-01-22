# LDC Backend Project  

This repository showcases the backend implementation for the **Link Datacenter (LDC) Internship Project**, developed using a **customized clean architecture** in **.NET Core**. It incorporates modern design patterns, follows SOLID principles, and implements robust mechanisms for scalability, security, and maintainability.  


## Table of Contents  

- [Features](#features)  
- [Architecture](#architecture)  
- [Technologies Used](#technologies-used)  
- [Setup and Installation](#setup-and-installation)  
- [Usage](#usage)  
- [API Documentation](#api-documentation)  
- [Contributing](#contributing)  

## Features  

1. **Custom Clean Architecture**  
   - Clear separation of concerns across multiple layers: Runtime, Presentation, Business Domain, and Infrastructure.  

2. **Authentication & Authorization**  
   - JWT-based authentication.  
   - Role-based access control (RBAC).  

3. **Global Exception Handling**  
   - Middleware for centralized exception handling with descriptive error responses.  

4. **Caching Mechanism**  
   - Optimized memory caching to enhance performance for frequently accessed data.  

5. **Database Management**  
   - Entity Framework Core for seamless integration with SQL Server.  
   - Migrations to manage schema changes effectively.  

6. **Data Validation**  
   - Leveraging data annotations and validation filters for robust input validation.  

7. **Logging**  
   - Configured logging with console and debug outputs for tracking critical operations.  

8. **Service Design Pattern**  
   - Encapsulation of business logic in service classes to promote reusability.  

9. **Repository Design Pattern**  
   - Abstract data access logic using interfaces for better flexibility and testing.  

10. **AutoMapper Integration**  
    - Simplified object-to-object mapping for DTOs and view models (VMs).  

11. **Quality Assurance Workflow**  
    - Collaboration in database and Figma designs.  
    - Testing and feedback incorporation to ensure high-quality deliverables.  

---  

## Architecture  

The project follows a **customized clean architecture**, designed for scalability and maintainability:  

1. **Runtime Layer**  
   - Configures the application at runtime, including dependency injection, database connections, logging, and middleware.  
   - Key file: `ServiceExtensions.cs`  
   - Configures JWT authentication, services, and memory caching.  

2. **Presentation Layer**  
   - Handles user interaction via APIs.  
   - Includes:  
     - **Controllers** for `Customers`, `Orders`, and `Products`.  
     - Middleware for handling custom exceptions and authorization.  

3. **Business Domain Layer**  
   - Core logic of the application.  
   - Contains:  
     - **Services** implementing business rules.  
     - **DTOs** for data transfer.  
     - **Responses** and **VMs** for consistent API output.  
     - Authorization middleware for secured endpoints.  

4. **Infrastructure Layer**  
   - Responsible for data persistence and interaction with the database.  
   - Includes:  
     - Repositories implementing the **repository design pattern**.  
     - Entities with data annotations for validation.  
     - Database migrations.  

---  

## Technologies Used  

- **Framework**: .NET Core  
- **Database**: SQL Server with Entity Framework Core  
- **Authentication**: JWT tokens  
- **Validation**: Data Annotations, Global Filters  
- **Caching**: Memory Cache  
- **Object Mapping**: AutoMapper  
- **Design Patterns**:  
  - Repository Design Pattern  
  - Service Design Pattern  
- **SOLID Principles**: Adherence to software design best practices  
- **Middleware**: Custom exception handling and authorization  

---  

## Setup and Installation  

1. **Clone the Repository**  
   ```bash  
   git clone https://github.com/FarouqDiaa/LDC-Backend-Project.git  
   cd LDC-Backend-Project  
   ```  

2. **Install Dependencies**  
   Restore NuGet packages:  
   ```bash  
   dotnet restore  
   ```  

3. **Set Up Database**  
   - Update `appsettings.json` with your SQL Server connection string.  
   - Apply migrations to the database:  
     ```bash  
     dotnet ef database update  
     ```  

4. **Run the Application**  
   ```bash  
   dotnet run  
   ```  

---  

## Usage  

- The application is accessible at `http://localhost:<port>`.  
- Use API testing tools like **Postman** to interact with the endpoints.  

## Contributing  

Contributions are welcome! Please follow these steps:  

1. Fork the repository.  
2. Create a new branch:  
   ```bash  
   git checkout -b feature-name  
   ```  
3. Commit your changes:  
   ```bash  
   git commit -m "Add feature"  
   ```  
4. Push your branch:  
   ```bash  
   git push origin feature-name  
   ```  
5. Open a pull request.  
