# BotBtpRanker

This project extracts data related to Italian government bonds (BOT and BTP) from free resources and stores it into a SQL Server database.
It provides functionality to retrieve all active bonds at any specific time and sort them by their yield to maturity (feature usually not provided on other platforms), aiding in investment decision-making.
The UI is a Blazor Web App.

## Why use BotBtpRanker?

**To quickly find the Italian government bonds with the highest yield to maturity.**
Most websites listing these instruments do not provide this feature (at least not for free).

## Screenshots
<div style="display: flex; justify-content: space-between;">
    <img src="https://raw.githubusercontent.com/andrea-baragiola/App-screenshots/main/BotBtpRanker-Home.png" alt="Home" width="30%">
    <img src="https://raw.githubusercontent.com/andrea-baragiola/App-screenshots/main/BotBtpRanker-Bot.png" alt="Bot" width="30%">
    <img src="https://raw.githubusercontent.com/andrea-baragiola/App-screenshots/main/BotBtpRanker-Btp.png" alt="Btp" width="30%">
</div>

## Features

- **Data Extraction**: Extracts Italian government bond data (BOT and BTP) from a reliable data source.
- **Database Storage**: Stores the extracted bond data into a SQL Server database for easy retrieval and management.
- **Dynamic Sorting**: Can sort the current active bonds based on their yield to maturity, expiration date, price or yearly coupon, providing users with insights into investment options.

## Usage

To use this, follow these steps:

1. Ensure you have Docker installed on your machine.
2. Clone this repository to your local machine by running the following command:

    ```bash
    git clone https://github.com/andrea-baragiola/BotBtpRanker.git
    ```

3. Open a terminal window inside the root directory of the cloned repository
4. Run the following command to start the app:

    ```bash
    docker-compose up
    ```

6. Once the services are up and running, you can access the Blazor app at `https://localhost:64085`.
7. You can also access the MSSQL database at `localhost:11433`.

## Requirements

- Docker
- Internet connection (for downloading Docker images if not already cached)

## Contributing

Contributions are welcome! If you'd like to improve this project, feel free to fork it and submit a pull request.

## License

This project is licensed under the [MIT License](LICENSE).
