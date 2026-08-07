# Farm Farm (read like Moon Moon)

# 🌱 Sick-Ahh Farming Game 🌾

<div align="center">

  ![.NET MAUI](https://img.shields.io/badge/.NET%20MAUI-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
  ![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
  ![.NET 9](https://img.shields.io/badge/.NET%209-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
  ![SQLite](https://img.shields.io/badge/SQLite-003B57?style=for-the-badge&logo=sqlite&logoColor=white)

  *A cozy, pixel-perfect mobile farming simulator built with .NET MAUI & SQLite!* 🐱✨
  *Pixels coming soon 

</div>

---

## 🌟 Game Highlights

- 🚜 **12 Clickable Farm Plots**: Plant, water, grow, and harvest crops in real time!
- 🎒 **Dynamic Backpack Inventory**: Keep track of your seeds with quick slotting and quantity badges.
- 🌾 **Seed Shop**: Buy seeds using earned gold to expand your farm empire.
- 🐱 **Player Account & Profile**: Track lifetime earnings, plants harvested, customize your username, or reset your farm!
- 💾 **Local Persistence**: Full SQLite database integration saves your farm state automatically.

---

## 🌽 Crop Guide

| Crop | Emoji | Buy Cost | Sell Price | Growth Time |
| :--- | :---: | :---: | :---: | :---: |
| **Carrot** | 🥕 | 5 G | 6 G | 5s |
| **Corn** | 🌽 | 10 G | 12 G | 15s |
| **Tomato** | 🍅 | 15 G | 16 G | 30s |
| **Potato** | 🥔 | 20 G | 22 G | 30s |
| **Eggplant** | 🍆 | 25 G | 27 G | 45s |
| **Pepper** | 🫑 | 30 G | 33 G | 45s |

---

### Visual Indicators
- 🌱 Empty plot shows nothing
- 🌱 💧 Planted but needs watering (sprout + water drop overlay)
- 🌱 💧 Watered plant growing (sprout + watered indicator + hourglass)
- 🎯 ✨ Ready to harvest (crop emoji + sparkles)

---

## 🚀 Getting Started

### Prerequisites

1. **Visual Studio 2026** (Community or Professional)
   - Ensure you have the **.NET MAUI workload** installed
   - Check: `Tools > Get Tools and Features > Workloads > Mobile development with .NET`

2. **.NET 9 SDK**
   - Verify installation: Open PowerShell and run `dotnet --version`
   - Should show version 9.x.x or later

3. **Platform-Specific Requirements**
   - **Windows**: Included with the .NET MAUI workload
   - **Android**: Android Device or Emulator with API 21+
   - **iOS**: Mac with Xcode 15+ (Mac only)
   - **macOS**: Xcode 15+ and Mac Catalyst support (Mac only)

---

### Installation

1. **Clone the Repository**
   ```git clone https://github.com/aneeshpal2005/sick-ahh-farming-game.git cd "sick ahh farming game"```
2. **Restore NuGet Packages**
   ```dotnet restore```

---

## 🔨 Building the Application

### Build for All Supported Platforms
  ```dotnet build -c Release```
**Windows (Desktop)**
  ```dotnet build -f net9.0-windows10.0.19041.0 -c Release```
**Android**
  ```dotnet build -f net9.0-android -c Release```
**iOS** (macOS only)
  ```dotnet build -f net9.0-ios -c Release```
**macOS (Mac Catalyst)**
  ```dotnet build -f net9.0-maccatalyst -c Release```

---

## ▶️ Running the Application

### From Visual Studio

1. **Select Target Framework**: In Visual Studio, use the dropdown next to the Run button to select your target platform (e.g., "Windows Machine", "Android Emulator", or "iOS Simulator")
2. **Press F5** or click **Run** to build and launch the application

### From Command Line

**Windows (Desktop)**
  ```dotnet run -f net9.0-windows10.0.19041.0```

**Android** (requires emulator or device)
  ```dotnet run -f net9.0-android```

**iOS** (macOS only, requires simulator or device)
  ```dotnet run -f net9.0-ios```

**macOS (Mac Catalyst)**
  ```dotnet run -f net9.0-maccatalyst```

---

## 🎮 Enjoy the Game!
Happy Farming!! 🌾🐱✨

<img src="https://images-wixmp-ed30a86b8c4ca887773594c2.wixmp.com/f/0a071573-3155-4a1f-8610-3289c87744e0/dgliday-44a29434-3413-418f-a883-a9175008d9fc.png?token=eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJzdWIiOiJ1cm46YXBwOjdlMGQxODg5ODIyNjQzNzNhNWYwZDQxNWVhMGQyNmUwIiwiaXNzIjoidXJuOmFwcDo3ZTBkMTg4OTgyMjY0MzczYTVmMGQ0MTVlYTBkMjZlMCIsIm9iaiI6W1t7InBhdGgiOiIvZi8wYTA3MTU3My0zMTU1LTRhMWYtODYxMC0zMjg5Yzg3NzQ0ZTAvZGdsaWRheS00NGEyOTQzNC0zNDEzLTQxOGYtYTg4My1hOTE3NTAwOGQ5ZmMucG5nIn1dXSwiYXVkIjpbInVybjpzZXJ2aWNlOmZpbGUuZG93bmxvYWQiXX0.LxaZEkkhGRI2tINLCmzZ7THPjhSRQb056DcGjsU5mWI" width="200" alt="Flint Lockwood" />
