# PrettyEnough 🎮

A beautiful, interactive testing shell for your game logic. PrettyEnough provides a text-based adventure mode for debugging and testing game features in real-time.

## Features

- 🎯 **Interactive Commands**: Test game logic with simple commands
- 📊 **Pretty Output**: Colorful, formatted display of game state
- 🔧 **Real-time Testing**: Modify stats and see immediate results
- 📈 **Comprehensive Stats**: View all stat information in detail
- 🚀 **Easy to Use**: Simple command interface with helpful prompts

## Getting Started

### Build and Run
```bash
# From the root directory
dotnet build PrettyEnough
dotnet run --project PrettyEnough
```

### Available Commands

| Command | Description | Usage |
|---------|-------------|-------|
| `help` | Show available commands | `help [command]` |
| `stats` | Display all current stats | `stats [stat_name]` |
| `modify` | Modify a resource stat | `modify <stat_name> <amount>` |
| `set` | Set a resource stat value | `set <stat_name> <value>` |
| `info` | Show game state information | `info` |
| `clear` | Clear the console screen | `clear` |
| `exit` | Exit the application | `exit` or `quit` |

## Examples

### View All Stats
```
🎯 > stats
```

### View Specific Stat
```
🎯 > stats resource_stat_health
```

### Modify Health
```
🎯 > modify resource_stat_health -20
```

### Set Health to Specific Value
```
🎯 > set resource_stat_health 50
```

### Get Game Information
```
🎯 > info
```

## Architecture

- **Program.cs**: Main entry point and game loop
- **GameState.cs**: Holds current game state
- **UI/ConsoleUI.cs**: Pretty formatting and display logic
- **Commands/**: Command implementations for different actions

## Future Enhancements

- Combat system testing
- Character management
- Skill testing
- Event system debugging
- Save/load functionality

## Development

PrettyEnough is designed to grow with your game logic. Add new commands by:

1. Implementing the `ICommand` interface
2. Registering the command in `CommandProcessor.RegisterCommands()`
3. Adding help documentation

This makes it easy to test new features as you develop them!
