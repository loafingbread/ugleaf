# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

**UGLEAF** (Universal Game Legerdemain Abstraction Framework) is an engine-agnostic C# game logic library targeting .NET 9.0. It handles stats, skills, combat, targeting, and events in a modular, data-driven way.

**Solution projects:**
- `GameLogic/` — Core library (C# class library, net9.0)
- `GameLogic.Tests/` — xUnit test suite
- `GameLogic.TestData/` — JSON fixture files used by tests
- `PrettyEnough/` — Interactive console app for manual testing

## Commands

```bash
dotnet build                              # Build solution
dotnet clean                              # Clean build artifacts
dotnet test                               # Run all tests
dotnet test --filter "TestName"           # Run a single test
dotnet run --project PrettyEnough         # Run the interactive CLI
```

## Architecture

### Registry & References

The `Registry` (`GameLogic/Registry/`) is the central hub. All game entities (Skills, Stats, Characters, Usables, Effects) are registered here and resolved through a `Reference` / `ReferenceBase<TValue, TData>` pattern. References use a two-phase **Resolve → Initialize** lifecycle. IDs are strongly typed: `ReferenceId`, `TemplateId`, `InstanceId`.

Entities are loaded from JSON via `JsonConfigLoader` (`GameLogic/Config/`) and deserialized through custom converters. `ConfigPaths` provides strongly-typed paths to test data fixtures.

### Entity & Character

`Entity` (`GameLogic/Entities/`) is the core runtime object. It composes `ITargeter`, `ITargetable`, `IAffectable`, and `IUser` via handler delegation rather than inheritance.

`Character` is built from a `CharacterTemplate` (immutable record) and holds a `StatBlock` and a collection of `Skill` instances. Factories (`CharacterFactory`, `SkillFactory`, `StatFactory`) handle instantiation.

### Stat System

Stats live under `GameLogic/Entities/Stats/` and are organized into subfolders: `Stat/`, `StatBlock/`, `Capabilities/`, `Modifiers/`, `Factories/`, `Query/`, `StatSystem/`. Two primary stat types exist: **ValueStat** and **ResourceStat**. Stats support modifiers, formulas, capabilities (bounds, max, regen), and a query interface for reading values.

### Skill & Usable System

`SkillTemplate` defines a skill's static data (name, description, tags, targeter spec, usables list). `Skill` is the runtime instance with `SkillState`. Skills are executed via `SkillResolver`. Usables (`GameLogic/Usables/`) implement `IUsable` and are consumed by `IUser` entities. Effects live in `GameLogic/Usables/Effects/`.

### Combat

Turn-based combat is in `GameLogic/Combat/TurnBased/`. `Combat` manages the loop; `CombatState` tracks live state; `TurnQueue` determines order.

### Targeting

`Targeter` and `TargeterData` (`GameLogic/Targeting/`) configure how entities select targets, including faction relationships. `ITargeter` / `ITargetable` are implemented by `Entity`.

### Events

`EventBus` (`GameLogic/Events/`) is a pub/sub bus. `EventCategories` groups events; specific event types are in `Events/Categories/`.

### PrettyEnough CLI

Commands implement `ICommand` and are registered with `CommandProcessor`. `ConsoleUI` handles formatted output. `ArgParser` parses command-line input. Use this project for exploratory/manual testing of game logic.

## Key Design Patterns

- **Resolve → Initialize** two-phase lifecycle on all `ReferenceBase<TValue, TData>` subclasses
- **Composition over inheritance** for `Entity` (delegates to handler objects)
- **Immutable records** for templates and data objects; mutable classes for runtime state
- **Factory pattern** for all entity instantiation (never `new` directly on templates)
- **IDeepCopyable\<T\>** (`GameLogic/Utils/`) for cloning runtime state
