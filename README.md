# ugleaf (**U**niversal **G**ame **Le**gerdemain **A**bstraction **F**ramework)
*It don't look like nothing, but it plays like a dream.*

UGLEAF is a modular game logic library that handles the behind the scenes (game logic) of your game: skills, items, crafting, combat, dialogue, decision trees, and more. Fully engine-agnostic, deeply configurable, and designed for total control. 

Whether you're building something new or expanding on an existing game, you can skip the boilerplate and focus on the logic that matters. No need to reinvent systems: just plug in, customize (if needed) and create.

Build the systems. Define the rules. Hook it up to anything.

**Build**
```bash
dotnet build
```

**Clean**
Warnings are only shown the first time a build is run. If you want to see them again you'll need to run a clean first.
```bash
dotnet clean
```

**Test**
```bash
/* All tests */
dotnet test

/* Specific test */
dotnet test --filter "{TestName}"
```