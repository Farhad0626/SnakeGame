# 🐍 Console Snake Game

A classic **Snake Game** developed in C# as my **first C# programming task**.

This project was originally created about **one and a half years ago** as part of my early journey into C# programming.

The project is now being published on GitHub as its **first public version**.

---

## 🎮 Features

* Main Menu

  * Start Game
  * Exit
* Difficulty Selection

  * Easy
  * Medium
  * Hard
* Different snake speeds based on difficulty
* Food system 🍎
* Snake growth after eating food
* Score tracking
* Game timer
* Wall collision detection
* Self-collision detection
* Bombs in Hard mode 💣
* Game Over system
* `ESC` to return to the Main Menu
* Arrow-key controls
* Direction queue for handling rapid keyboard input
* Console-based rendering

---

## 🕹️ Controls

| Key   | Action              |
| ----- | ------------------- |
| `↑`   | Move Up             |
| `↓`   | Move Down           |
| `←`   | Move Left           |
| `→`   | Move Right          |
| `ESC` | Return to Main Menu |

---

## ⚔️ Difficulty Levels

### 🟢 Easy

* Normal snake speed
* Food enabled
* No bombs

### 🟡 Medium

* Faster snake speed
* Food enabled
* No bombs

### 🔴 Hard

* Fastest snake speed
* Food enabled
* Bombs enabled
* Touching a bomb causes Game Over

---

## 🧠 C# Concepts Used

This project demonstrates several fundamental C# concepts:

* `enum`
* `record`
* `record struct`
* Classes
* Static methods and fields
* `List<T>`
* `Queue<T>`
* Nullable value types
* Switch expressions
* Pattern matching
* `Random`
* `Stopwatch`
* `DateTime`
* Console input/output
* Game loops
* Collision detection
* State management
* Keyboard input handling

---

## 📁 Project Structure

```text
SnakeGame/
│
├── Program.cs
├── SnakeGame.csproj
├── .gitignore
└── README.md
```

The game is implemented as a console application, with the main game logic contained in `Program.cs`.

---

## 🚀 Requirements

* .NET 10 SDK
* Windows or another operating system supporting .NET 10
* A terminal capable of displaying Unicode characters

Check your installed .NET version:

```bash
dotnet --version
```

---

## ▶️ Running the Game

Clone the repository:

```bash
git clone https://github.com/Farhad0626/SnakeGame.git
```

Enter the project directory:

```bash
cd SnakeGame
```

Build the project:

```bash
dotnet build
```

Run the game:

```bash
dotnet run
```

---

## 📜 Project Background

This project was created approximately **one and a half years ago** as my **first C# programming task**.

The main goal was to learn C# fundamentals by building a small console-based game.

This repository represents one of the early steps in my C# programming journey and is now available as its **first public release**.


