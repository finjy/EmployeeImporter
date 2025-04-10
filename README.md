# Employee Data Importer

A simple ASP.NET Core MVC application that allows users to import employee data from CSV files, store it in a SQL Server database, and manage the data through an interactive web interface.

## Features

- CSV file import with detailed processing report
- Clean Architecture implementation
- DataTables integration for advanced data display
- Sorting and searching capabilities
- Inline editing functionality
- Responsive design

## Technology Stack

- ASP.NET Core MVC (.NET 8)
- Entity Framework Core (Code First)
- SQL Server
- DataTables.net
- CsvHelper

## Getting Started

### Prerequisites

- .NET 8 SDK  
- Docker  
- SQL Server (or use the Dockerized version)  
- Visual Studio 2022 or Visual Studio Code  

### Installation

1. Clone the repository  
```bash
git clone https://github.com/finjy/EmployeeImporter.git
```

### Run with Docker

1. Make sure Docker Desktop is running  
2. In the project root directory, build and run the containers:  
```bash
docker-compose up --build
```
3. Open your browser and go to:  
- http://localhost:8080

The SQL Server container will be created automatically, and the application will apply database migrations on startup.

### Run from Visual Studio

1. Open the solution in Visual Studio  
2. Ensure that **EmployeeImporter** is set as the startup project  
3. Run the project with IIS Express or Kestrel  
4. The database connection string in `appsettings.Development.json` should point to your local SQL Server instance  
5. Migrations will be applied automatically at startup

## Usage

1. On the main page, use the "Browse File" button to select a CSV file  
2. Click "Import" to process the file  
3. The application will display the number of successfully processed records  
4. Use the table below to view, sort, search, and edit the imported data  
    - Click on column headers to sort  
    - Use the search box to filter records  
    - Double-click on a cell to edit its value  

## CSV File Format

The application expects CSV files with the following headers:
  - Personnel_Records.Payroll_Number  
  - Personnel_Records.Forenames  
  - Personnel_Records.Surname  
  - Personnel_Records.Date_of_Birth  
  - Personnel_Records.Telephone  
  - Personnel_Records.Mobile  
  - Personnel_Records.Address  
  - Personnel_Records.Address_2  
  - Personnel_Records.Postcode  
  - Personnel_Records.EMail_Home  
  - Personnel_Records.Start_Date  

## Testing

Run the included unit tests with:  
```bash
dotnet test
```

## License

This project is licensed under the MIT License.
