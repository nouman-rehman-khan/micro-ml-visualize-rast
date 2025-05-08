# MicroML AST Visualizer

This web application visualizes Abstract Syntax Trees (AST) for the MicroML language. It's built using ASP.NET Core with React for the frontend.

## Project Overview

This project is an implementation of a simple web application that can parse MicroML code and display the resulting Abstract Syntax Tree. MicroML is a minimal functional programming language covered in CS 510.

## Features

- Parse MicroML code and generate Abstract Syntax Trees
- Support for:
  - Numbers and variables
  - Binary operations (+, -, *, /, ==, !=, <, >, <=, >=)
  - Function definitions (lambda or fun)
  - Function application
  - Let expressions
  - If-then-else expressions
- Visual representation of the AST

## Screenshots

# Screenshot 1
![image](https://github.com/user-attachments/assets/181b2d0c-77b0-453e-938e-3818ce134d7c)

# Screenshot 2
![image](https://github.com/user-attachments/assets/7c1f324b-5129-4333-8b9c-bf6b78884111)

# Screenshot 3
![image](https://github.com/user-attachments/assets/532316f9-e188-4939-a296-275cf7cfbe71)

# Screenshot 4
![image](https://github.com/user-attachments/assets/7da9cf17-d06b-4940-bfe3-4cd3ba7efea2)




## Technology Stack

- ASP.NET Core for the backend
- React for the frontend
- D3.js for tree visualization
- C# for the MicroML parser

## Getting Started

### Prerequisites

- .NET 7.0 SDK
- Node.js and npm

### Running the Application

1. Clone the repository
2. Restore dependencies with `dotnet restore`
3. Run the application with `dotnet run`
4. Navigate to the URL displayed in the console (typically https://localhost:44479)

## License

This project is licensed under the MIT License - see the LICENSE file for details.
