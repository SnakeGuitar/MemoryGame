# 🧠 Memory Game (TCP/IP)

A multiplayer Memory Game developed in **C#** using the **.NET Framework**. This project implements a Client-Server architecture where the **Client** is built with **WPF** (Windows Presentation Foundation) and communicates via **WCF** (Windows Communication Foundation) with a centralized **Server**.

The system features real-time multiplayer gameplay, user authentication, social features, and comprehensive profile management.

## 📋 Table of Contents
- [Features](#-features)
- [Technologies Used](#-technologies-used)
- [Architecture](#-architecture)
- [Getting Started](#-getting-started)
- [Configuration](#-configuration)
- [Localization](#-localization)
- [Graphics & Credits](#-graphics--credits)
- [License](#-license)

## ✨ Features

### 👤 User System
- **Authentication**: Secure Login and Registration system.
- **Account Verification**: Email verification via code (SMTP).
- **Guest Mode**: Play without creating a permanent account.
- **Profile Management**: customizable profiles with avatars and personal information.

### 🎮 Gameplay
- **Singleplayer**: Practice mode with customizable difficulty levels.
- **Multiplayer**: Real-time matches against other players over TCP/IP.
- **Lobby System**: Create or join game lobbies to match with other users.
- **Game Logic**: Classic memory card matching mechanics with turn management.

### 🤝 Social & Extras
- **Friends System**: Send and accept friend requests.
- **Invitation**: Invite friends to your game lobby.
- **User Reporting**: Report users for inappropriate behavior.
- **Leaderboards/Stats**: View match history and player statistics.

## 🛠 Technologies Used

### Client Side
- **Framework**: .NET Framework 4.7.2
- **UI Library**: WPF (Windows Presentation Foundation)
- **Pattern**: MVVM (Model-View-ViewModel) for clean code separation.
- **Communication**: WCF Client (Service References).

### Server Side
- **Framework**: .NET Framework (Console Application host for WCF).
- **Communication**: WCF (Windows Communication Foundation) using TCP binding for low latency.
- **Database**: SQL Server.
- **ORM**: Entity Framework 6 (Database First / Model First).
- **Security**: `BCrypt.Net` for secure password hashing.

## 🏗 Architecture

The solution is divided into two main projects:

1.  **`Server`**: Hosts the game logic, manages database connections (SQL Server via EF6), and exposes WCF endpoints (User Service, Game Service, Lobby Service).
2.  **`Client`**: A WPF Desktop application that connects to the server endpoints. It handles the UI, animations, and localization.

## 🚀 Getting Started

### Prerequisites
- Visual Studio 2019 or 2022.
- SQL Server (Express or Developer edition).
- .NET Framework 4.7.2 SDK.

### Installation

1.  **Clone the repository**
    ```bash
    git clone [https://github.com/snakeguitar/memorygame.git](https://github.com/snakeguitar/memorygame.git)
    ```

2.  **Database Setup**
    * Ensure SQL Server is running.
    * Execute the database script (provided in `Server/Database`) or let Entity Framework generate the schema if configured for Code First (check `MemoryGame.edmx` connection string).

3.  **Server Configuration**
    * Open `Server/Server.sln`.
    * Open `App.config` and update the connection string to point to your local SQL Server instance.
    * Update the email credentials in the Email Helper configuration if you want to test the verification code feature.
    * **Build and Run** the Server project. It must be running for clients to connect.

4.  **Client Configuration**
    * Open `Client/Client.sln`.
    * Open `App.config` and ensure the WCF endpoints point to `localhost` (or the IP where the server is running).
    * **Build and Run** the Client project.

## 🌍 Localization

The application is fully localized and supports multiple languages. The language is automatically detected or can be selected in the settings.

* 🇺🇸 **English** (Default)
* 🇲🇽 **Spanish** (es-MX)
* 🇨🇳 **Chinese** (zh-CN)
* 🇯🇵 **Japanese** (ja-JP)
* 🇰🇷 **Korean** (ko-KR)

## 🎨 Graphics & Credits

The visual assets used in this game are categorized as follows:

* **Original Designs**: The black and white illustrations (located in `Resources/Images/Cards/Fronts/Normal`) are original creations and intellectual property of the repository author.
* **Colorized Versions**: The color illustrations (located in `Resources/Images/Cards/Fronts/Color`) were generated using **Artificial Intelligence (AI)** tools, based on the original black and white sketches mentioned above.

> *Note: These assets are intended for educational and project demonstration purposes.*

## 📄 License

This project is developed for educational purposes.
