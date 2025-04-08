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

## Project Structure

The project follows Clean Architecture principles with three main layers:

- **Core**: Contains domain entities, interfaces, and application services
- **Infrastructure**: Implements data access and external services
- **Web**: Contains controllers, views, and web-specific models

## Getting Started

### Prerequisites

- .NET 8 SDK
- SQL Server (or SQL Server Express)
- Visual Studio 2022 or Visual Studio Code

### Installation

1. Clone the repository
```bash
git clone https://github.com/yourusername/employee-importer.git
cd employee-importer
```
2. Update the connection string in appsettings.json to point to your SQL Server instance
3. Run the Entity Framework migrations to create the database
```bash
bashdotnet ef database update
```
4. Build and run the application
```bash
bashdotnet build
dotnet run
```
5. Navigate to https://localhost:5001 or http://localhost:5000 in your web browser

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
bashdotnet test
```

## License
This project is licensed under the MIT License - see the LICENSE file for details.
