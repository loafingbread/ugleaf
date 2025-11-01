namespace GameLogic.Config;

using System.Text.Json;
using System.Text.Json.Serialization;
using GameLogic.Entities.Characters;
using GameLogic.Entities.Skills;
using GameLogic.Entities.Stats;
using GameLogic.Registry;
using GameLogic.Usables;
using GameLogic.Usables.Effects;

// Reusuable config loader utility
public static class JsonConfigLoader
{
    public static readonly JsonSerializerOptions options = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters =
        {
            new JsonStringEnumConverter(),
            // new StatConfigRecordConverter(),
            new GameLogic.Config.JsonLoader.ReferenceUnionSpecConverter(),
        },
    };

    public static T LoadFromFile<T>(string path)
    {
        string json = File.ReadAllText(path);
        return System.Text.Json.JsonSerializer.Deserialize<T>(json, options)
            ?? throw new InvalidOperationException($"Failed to load {typeof(T).Name} from {path}");
    }

    // public static ReferenceUnionSpec LoadReferenceUnionSpecFromFile(string path)
    // {
    //     string json = File.ReadAllText(path);
    //     ReferenceUnionSpec referenceUnionSpec =
    //         System.Text.Json.JsonSerializer.Deserialize<ReferenceUnionSpec>(json, options)
    //         ?? throw new InvalidOperationException(
    //             $"Failed to load {typeof(ReferenceUnionMetadata).Name} from {path}"
    //         );

    //     return LoadReferenceUnionSpecKind(path, options, referenceUnionSpec.Metadata);
    // }

    // private static ReferenceUnionSpec LoadReferenceUnionSpecKind(
    //     string path,
    //     JsonSerializerOptions options,
    //     ReferenceUnionMetadata referenceMetadata
    // )
    // {
    //     string json = File.ReadAllText(path);

    //     switch (referenceMetadata.Kind)
    //     {
    //         case EReferenceKind.Ref:
    //             return System.Text.Json.JsonSerializer.Deserialize<RefSpec>(json, options)
    //                 ?? throw new InvalidOperationException(
    //                     $"Failed to load {typeof(RefSpec).Name} from {path}"
    //                 );
    //         case EReferenceKind.Override:
    //             return LoadOverrideSpec(path, referenceMetadata);
    //         case EReferenceKind.Inline:
    //             return LoadInlineSpec(path, referenceMetadata);
    //         case EReferenceKind.Instance:
    //             return LoadInstanceSpec(path, referenceMetadata);
    //         default:
    //             throw new InvalidOperationException(
    //                 $"Invalid reference kind: {referenceMetadata.Kind}"
    //             );
    //     }
    // }

    // private static ReferenceUnionSpec LoadOverrideSpec(
    //     string path,
    //     ReferenceUnionMetadata referenceMetadata
    // )
    // {
    //     string json = File.ReadAllText(path);

    //     switch (referenceMetadata.TemplateType)
    //     {
    //         case ETemplateType.Skill:
    //             return System.Text.Json.JsonSerializer.Deserialize<
    //                     OverrideSpec<SkillTemplateRecord, SkillOverrideRecord>
    //                 >(json, options)
    //                 ?? throw new InvalidOperationException(
    //                     $"Failed to load {typeof(OverrideSpec<SkillTemplateRecord, SkillOverrideRecord>).Name} from {path}"
    //                 );
    //         case ETemplateType.Usable:
    //             return System.Text.Json.JsonSerializer.Deserialize<
    //                     OverrideSpec<UsableTemplateRecord, UsableOverrideRecord>
    //                 >(json, options)
    //                 ?? throw new InvalidOperationException(
    //                     $"Failed to load {typeof(OverrideSpec<UsableTemplateRecord, UsableOverrideRecord>).Name} from {path}"
    //                 );
    //         case ETemplateType.Effect:
    //             return System.Text.Json.JsonSerializer.Deserialize<
    //                     OverrideSpec<EffectTemplateRecord, EffectOverrideRecord>
    //                 >(json, options)
    //                 ?? throw new InvalidOperationException(
    //                     $"Failed to load {typeof(OverrideSpec<EffectTemplateRecord, EffectOverrideRecord>).Name} from {path}"
    //                 );
    //         case ETemplateType.Character:
    //             return System.Text.Json.JsonSerializer.Deserialize<
    //                     OverrideSpec<CharacterTemplateRecord, CharacterOverrideRecord>
    //                 >(json, options)
    //                 ?? throw new InvalidOperationException(
    //                     $"Failed to load {typeof(OverrideSpec<CharacterTemplateRecord, CharacterOverrideRecord>).Name} from {path}"
    //                 );
    //         case ETemplateType.Stat:
    //             return System.Text.Json.JsonSerializer.Deserialize<
    //                     OverrideSpec<StatTemplateRecord, StatOverrideRecord>
    //                 >(json, options)
    //                 ?? throw new InvalidOperationException(
    //                     $"Failed to load {typeof(OverrideSpec<StatTemplateRecord, StatOverrideRecord>).Name} from {path}"
    //                 );
    //         default:
    //             throw new InvalidOperationException(
    //                 $"Invalid template type: {referenceMetadata.TemplateType}"
    //             );
    //     }
    // }

    // private static ReferenceUnionSpec LoadInlineSpec(
    //     string path,
    //     ReferenceUnionMetadata referenceMetadata
    // )
    // {
    //     string json = File.ReadAllText(path);

    //     Console.WriteLine($"Loading JSON from {path}:");
    //     Console.WriteLine(json);

    //     try
    //     {
    //         // Use the converter by deserializing as ReferenceUnionSpec
    //         // The converter will handle the polymorphic deserialization correctly
    //         return System.Text.Json.JsonSerializer.Deserialize<ReferenceUnionSpec>(json, options)
    //             ?? throw new InvalidOperationException($"Failed to load InlineSpec from {path}");
    //     }
    //     catch (Exception ex)
    //     {
    //         Console.WriteLine(
    //             $"Failed to load {referenceMetadata.TemplateType.ToString()} from {path}: {ex.Message}"
    //         );
    //         throw;
    //     }
    // }

    // private static ReferenceUnionSpec LoadInstanceSpec(
    //     string path,
    //     ReferenceUnionMetadata referenceMetadata
    // )
    // {
    //     string json = File.ReadAllText(path);

    //     switch (referenceMetadata.TemplateType)
    //     {
    //         case ETemplateType.Skill:
    //             return System.Text.Json.JsonSerializer.Deserialize<
    //                     InstanceSpec<SkillTemplateRecord, Skill>
    //                 >(json, options)
    //                 ?? throw new InvalidOperationException(
    //                     $"Failed to load {typeof(InstanceSpec<SkillTemplateRecord, Skill>).Name} from {path}"
    //                 );
    //         case ETemplateType.Usable:
    //             return System.Text.Json.JsonSerializer.Deserialize<
    //                     InstanceSpec<UsableTemplateRecord, Usable>
    //                 >(json, options)
    //                 ?? throw new InvalidOperationException(
    //                     $"Failed to load {typeof(InstanceSpec<UsableTemplateRecord, Usable>).Name} from {path}"
    //                 );
    //         case ETemplateType.Effect:
    //             return System.Text.Json.JsonSerializer.Deserialize<
    //                     InstanceSpec<EffectTemplateRecord, Effect>
    //                 >(json, options)
    //                 ?? throw new InvalidOperationException(
    //                     $"Failed to load {typeof(InstanceSpec<EffectTemplateRecord, Effect>).Name} from {path}"
    //                 );
    //         case ETemplateType.Character:
    //             return System.Text.Json.JsonSerializer.Deserialize<
    //                     InstanceSpec<CharacterTemplateRecord, Character>
    //                 >(json, options)
    //                 ?? throw new InvalidOperationException(
    //                     $"Failed to load {typeof(InstanceSpec<CharacterTemplateRecord, Character>).Name} from {path}"
    //                 );
    //         case ETemplateType.Stat:
    //             return System.Text.Json.JsonSerializer.Deserialize<
    //                     InstanceSpec<StatTemplateRecord, Stat>
    //                 >(json, options)
    //                 ?? throw new InvalidOperationException(
    //                     $"Failed to load {typeof(InstanceSpec<StatTemplateRecord, Stat>).Name} from {path}"
    //                 );
    //         default:
    //             throw new InvalidOperationException(
    //                 $"Invalid template type: {referenceMetadata.TemplateType}"
    //             );
    //     }
    // }

    // public static List<T> LoadAllFromFolder<T>(string folderPath)
    // {
    //     List<T> list = new();

    //     foreach (string filePath in Directory.GetFiles(folderPath, "*.json"))
    //     {
    //         try
    //         {
    //             T config = LoadFromFile<T>(filePath);
    //             list.Add(config);
    //         }
    //         catch (Exception ex)
    //         {
    //             Console.WriteLine($"Warning: Failed to load {filePath}: {ex.Message}");
    //         }
    //     }

    //     return list;
    // }
}
