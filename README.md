# BotBtpRanker

This C# project extracts data related to Italian government bonds (BOT and BTP) from free resources and stores it into a SQL Server database.
It provides functionality to retrieve all active bonds at any specific time and sort them by their yield to maturity (feature usually not provided on other platforms), aiding in investment decision-making.
The UI is a Blazor Web App.

## Why use BotBtpRanker?

**To quickly find the Italian government bonds with the highest yield to maturity.**
Most websites listing these instruments do not provide this feature (at least not for free).


## Features

- **Data Extraction**: Extracts Italian government bond data (BOT and BTP) from a reliable data source.
- **Database Storage**: Stores the extracted bond data into a SQL Server database for easy retrieval and management.
- **Dynamic Sorting**: Can sort the current active bonds based on their yield to maturity, expiration date, price or yearly coupon, providing users with insights into investment options.

## Usage

TBD

## Requirements

- (Docker)
- .NET Framework (compatible version)
- SQL Server
- Internet connection for data extraction

## Contributing

Contributions are welcome! If you'd like to improve this project, feel free to fork it and submit a pull request.

## License

This project is licensed under the [MIT License](LICENSE).
