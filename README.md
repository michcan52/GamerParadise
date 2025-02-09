GamerParadise
GamerParadise is a desktop application developed in C# using WPF. It is designed to manage and launch applications—such as games or utilities—and provides remote control functionality. The project is built with gamepad support and is intended to work seamlessly with Sunshine and Apollo.

Features
Application Management:

An administration window (AdminWindow) for managing your application list, with capabilities to add, edit, update, and delete items.
File selection through an OpenFileDialog, which allows you to choose executable files (.exe) and automatically fills in the name if left blank.
Configuration Management:

Uses the AppConfig class to load and save the application configuration, ensuring that your list of applications persists across sessions.
Main Interface:

A primary window (MainWindow) that serves as the central hub for interacting with the application.
Supports launching processes and integrates with the remote control server.
Process Management:

The ProcessHelper provides methods to run and manage external processes, making it easy to launch applications with specific arguments.
Remote Control Server:

The RemoteControlServer module is implemented to receive remote commands, allowing for external control of the application.
Designed to work with Sunshine and Apollo, which enhances its use in remote gaming and streaming setups.
Gamepad Support:

GamerParadise includes support for gamepads, enabling a smoother and more intuitive user experience when navigating the interface or controlling the application remotely.
Requirements
Operating System:
Windows (since the application uses WPF for the user interface).

.NET Framework:
The project requires a compatible .NET Framework version (or .NET Core/5+) for WPF applications. Please refer to the GamerParadise.csproj file for the exact framework version.

Development Environment:
Visual Studio 2019 (or later) is recommended. Visual Studio Code with the appropriate C# and XAML extensions can also be used.

Installation and Execution
Clone the Repository:

bash
Copiar
git clone https://github.com/michcan52/GamerParadise.git
Open the Solution:

Open the GamerParadise.sln file (or directly load GamerParadise.csproj) in your preferred IDE.
Restore Dependencies:

If the project uses NuGet packages, make sure to restore them.
Build and Run:

Compile the solution and run the project. The MainWindow will launch, giving you access to the application's various functionalities.
Key Components
AdminWindow.xaml / AdminWindow.xaml.cs
Purpose:
Provides an interface to manage the list of applications.
Functionality:
Allows selecting executable files via an OpenFileDialog.
Includes features to add, edit, update, and delete application entries.
Performs basic validations to ensure that both the name and path are provided.
AppConfig.cs
Purpose:
Handles loading and saving the application's configuration, particularly the list of managed applications.
Usage:
This class ensures that any changes made through the AdminWindow are persisted.
MainWindow.xaml / MainWindow.xaml.cs
Purpose:
Serves as the main user interface for interacting with GamerParadise.
Functionality:
Provides access to launching applications, integrating with remote control functionalities, and possibly gamepad interactions.
ProcessHelper.cs
Purpose:
Contains helper methods for starting and managing external processes.
Usage:
Simplifies launching applications with specified arguments.
RemoteControlServer.cs
Purpose:
Implements a server that listens for remote commands.
Usage:
Facilitates remote control of the application and is designed to work in conjunction with Sunshine and Apollo.
Note:
If you have specific configuration details (such as ports or protocols), please update this section accordingly.
Contributing
Contributions are welcome! To contribute to GamerParadise:

Fork the repository.
Create a new branch with a descriptive name for your changes.
Submit a pull request with a detailed description of your modifications.
Follow the project's coding guidelines and maintain consistency with the existing code structure.
License
This project is distributed under the [insert license, e.g., MIT License]. Please refer to the LICENSE file for further details.

Contact
For questions, suggestions, or contributions, please open an issue on GitHub or contact the maintainers directly.
