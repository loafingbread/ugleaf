### 🔸 3. Modifier Condition Extensions (Later)

Your condition syntax is great. Eventually, consider extending with:

- **StatAbove**
- **HasStatus**
- **TagPresent**
- **IsInState** (e.g. stance, biome, combat phase)

Just good to plan for flexibility.

---

### Stat Value Calculation

1. **BaseValue**
2. **+** Sum of Flat Modifiers
3. **+** (BaseValue × Sum of PercentAdd modifiers)
4. **×** Product of (1 + PercentMul modifiers)
5. **= Final Value**

---

### StatModifierType

// TODO: Add resource and derived stats. Examples of extending the record:
// public record ResourceStatData : StatData
// {
//     public required int RegenRate { get; init; }
// }

// public record DerivedStatData : StatData
// {
//     public required Formula Formula { get; init; }
// }

// public record DerivedResourceStatData : ResourceStatData
// {
//     public required int RegenRate { get; init; }
//     public required Formula Formula { get; init; }
// }
