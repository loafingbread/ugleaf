# ugleaf (**U**niversal **G**ame **Le**gerdemain **A**bstraction **F**ramework)
*It don't look like nothing, but it plays like a dream.*

UGLEAF is a modular game logic library that handles the behind the scenes (game logic) of your game: skills, items, crafting, combat, dialogue, decision trees, and more. Fully engine-agnostic, deeply configurable, and designed for total control. 

Whether you're building something new or expanding on an existing game, you can skip the boilerplate and focus on the logic that matters. No need to reinvent systems: just plug in, customize (if needed) and create.

Build the systems. Define the rules. Hook it up to anything.

# Scripts
CLI (command line interface) tool commands to manage the project.

## Dev Tooling

### Build/Compile project
```bash
dotnet build # Build project
```

### Clean up dependencies
Warnings are only shown the first time a build is run. If you want to see them again you'll need to run a clean first.
```bash
dotnet clean
```

### Testing
```bash
# Run all tests
dotnet test

# Run specific test
dotnet test --filter "{TestName}"
```

### Create new projects
#### Create new project folder
```bash
# Create a new class library
dotnet new classlib -n YourProjectName

# Create a new console application
dotnet new console -n YourProjectName

# Create a new test project
dotnet new xunit -n YourProjectName.Tests

# Create a new MSTest project
dotnet new mstest -n YourProjectName.Tests

# Create a new Godot C# project
dotnet new godot -n YourProjectName
```

#### Add it to the solution:
```bash
dotnet sln add YourProjectName/YourProjectName.csproj
```

#### Add references between projects (if needed):
```bash
dotnet add YourProjectName/YourProjectName.csproj reference GameLogic/GameLogic.csproj
```
