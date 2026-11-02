# ToDoList

A to-do list app built with **.NET MAUI** (.NET 10) using the MVVM pattern, with a blue/purple theme.

## Features

- **Home page** – greeting, daily quote and a button to open your tasks
- **Task list** – all tasks with description and due date
- **Task details** – full view of a single task (description, priority, notes, completed state)
- **New task** – create a task with name, description, due date and priority

> Tasks are stored in memory (`MockItemStore`) with 10 sample tasks, so changes are lost when the app closes.
> The Edit and Delete buttons on the details page are not wired up yet.

## Tech stack

| | |
|---|---|
| Framework | .NET MAUI 10.0.120 (.NET 10) |
| MVVM | [CommunityToolkit.Mvvm](https://github.com/CommunityToolkit/dotnet) 8.4.2 |
| Navigation | Shell with registered routes |
| DI | Built-in `MauiProgram` service registration |
| Platforms | iOS, Android, Mac Catalyst, Windows |

## Project structure

```
ToDoList/
├── Models/           TodoItem
├── Services/         IitemStore<T> + MockItemStore (in-memory data)
├── ViewModels/       BaseViewModel, MainPage, ToDoItem, ToDoDetail, NewToDo
├── Views/            ToDoItemPage, ToDoDetailPage, NewToDoPage
├── Resources/Styles/ Colors.xaml, Styles.xaml (app theme)
├── MainPage.xaml     Home page
├── AppShell.xaml     Shell + route registration
└── MauiProgram.cs    Services, view models and pages registration
```

## Getting started

### Requirements

- .NET 10 SDK with the MAUI workload: `dotnet workload install maui`
- **macOS:** Xcode (for iOS / Mac Catalyst)
- **Android:** Android SDK + an emulator (Android Studio)

### Run

```bash
# Mac (Mac Catalyst)
dotnet build -t:Build;Run -f net10.0-maccatalyst ToDoList.csproj

# iOS Simulator
dotnet build -t:Build;Run -f net10.0-ios ToDoList.csproj

# Android Emulator
dotnet build -t:Build;Run -f net10.0-android ToDoList.csproj
```

Use `-t:Build;Run`, not just `-t:Run`: with .NET 10, `-t:Run` launches the app without building it first.

**VS Code:** add a task in `.vscode/tasks.json` that runs one of the commands above, then attach with a `coreclr` launch configuration. The `.vscode` folder is git-ignored, so it isn't part of this repo.

## Troubleshooting

**"This version of .NET for iOS requires Xcode X.Y"**
Your Xcode is newer than the .NET iOS SDK expects. `ToDoList.csproj` sets `<ValidateXcodeVersion>false</ValidateXcodeVersion>` to skip this check. To fix it properly, update the workloads (`sudo dotnet workload update`) or install the matching Xcode version.

**App opens and closes immediately**
Usually a XAML error at startup, for example:
- a `{StaticResource Name}` that isn't defined in `Colors.xaml` / `Styles.xaml`
- a Brush (e.g. `WhiteBrush`) assigned to a Color property like `TextColor`; use the Color (`White`) instead

## Author

[Venki-Sub](https://github.com/Venki-Sub)
