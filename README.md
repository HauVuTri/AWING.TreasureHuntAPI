# Treasure Hunt Backend

## Overview

This is the backend API for the Treasure Hunt application. It is built using ASP.NET Core and provides endpoints to manage treasure maps, perform calculations, and handle user authentication.

## Features

- User authentication with JWT
- API endpoints for treasure map operations
- Fuel calculation based on treasure map data
- Secure communication with frontend

## Technologies Used

- ASP.NET Core 8.0
- MySQL
- JWT Authentication

## Prerequisites

- .NET Core 8.0 SDK
- MySQL Server
- Visual Studio 2022 or VS Code

## Getting Started

1. **Clone the repository:**

   ```bash
   git clone https://github.com/yourusername/treasure-hunt-backend.git
   cd treasure-hunt-backend
2. Set up the database
- Can use directly DB in appsetting.development.json
- Or create local:
	- Create a MySQL database named treasure_hunt_db.
	
	- Update the connection string in appsettings.Development.json

3. Apply migrations:
	- dotnet ef database update
4. dotnet run

5. Create a user by using Authentication/register api.


