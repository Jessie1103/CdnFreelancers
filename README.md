# CdnFreelancers

CDN Freelancers API is a RESTful API that allows CDN - Complete Developer Network to maintain a directory of freelancers for various job opportunities.

## Features

- Retrieve a list of registered users.
- Retrieve user by id.
- User registration with fields such as username, email, phone number, skill sets, and hobby.
- User details update.
- User deletion.

## Technologies Used

- ASP.NET Core
- Entity Framework Core
- SQL Server (or your preferred RDBMS)

## Getting Started

### Prerequisites

- .NET SDK
- Visual Studio
- SQL Server
- Git

### Installation

1. Clone the repository:
   ```
   git clone https://github.com/yourusername/your-repo.git
   ```
2. Open the project in Visual Studio.
3. Configure your database connection in the appsettings.json file. Modify the "FreelancerAppCon" connection string to point to your SQL Server instance.
   ```
    "ConnectionStrings": {
      "FreelancerAppCon": "Data Source=your-SQL-server;Initial Catalog=your-database; Integrated Security=true;Encrypt=True;TrustServerCertificate=True;"
    }
   ```
4. Build and run the API.

### API Endpoints
1. GET `/api/Freelancer/GetAllFreelancers`: Get a list of all registered users.
2. GET `/api/Freelancer/GetFreelancerById/{id}`: Get user details by ID.
3. POST `/api/Freelancer/CreateFreelancer`: Register a new user.
4. PUT `/api/Freelancer/UpdateFreelancer/{id}`: Update user details by ID.
5. DELETE `/api/Freelancer/DeleteFreelancer/{id}`: Delete a user by ID.

### Usage
Use Swagger at `https://localhost:7135/swagger/index.html` to interact with and test the API.

### Contributing
Contributions are welcome! Please follow these guidelines when contributing:

1. Fork the repository.
2. Create a new branch for your feature or bug fix.
3. Write clear and concise code.
4. Test your changes thoroughly.
5. Create a pull request with a detailed description of your changes.

### License
This project is licensed under the MIT License - see the `LICENSE.md` file for details.
