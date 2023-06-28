using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

namespace UnityBuilder {
    public static class Builder {
        private static readonly string Eol = Environment.NewLine;

        public static void Build() {
            Console.WriteLine("Build is Called!!");

            ParseCommandLineArguments(out Dictionary<string, string> validatedOptions);

            var scenes = EditorBuildSettings.scenes.Where(scene => scene.enabled).Select(s => s.path).ToArray();

            string path = validatedOptions["customBuildPath"];
            Boolean.TryParse(validatedOptions["devBuild"], out bool isDev);
            string profileName = isDev ? "develop" : "product";
            
            BuildAddressables(profileName);
            BuildPipeline.BuildPlayer(scenes, path, BuildTarget.Android, BuildOptions.None);
        }

        private static void ParseCommandLineArguments(out Dictionary<string, string> providedArguments) {
            providedArguments = new Dictionary<string, string>();
            string[] args = Environment.GetCommandLineArgs();

            Console.WriteLine(
                $"{Eol}" +
                $"###########################{Eol}" +
                $"#    Parsing settings     #{Eol}" +
                $"###########################{Eol}" +
                $"{Eol}"
            );

            // Extract flags with optional values
            for (int current = 0, next = 1; current < args.Length; current++, next++) {
                // Parse flag
                bool isFlag = args[current].StartsWith("-");
                if (!isFlag) continue;
                string flag = args[current].TrimStart('-');

                // Parse optional value
                bool flagHasValue = next < args.Length && !args[next].StartsWith("-");
                string value = flagHasValue ? args[next].TrimStart('-') : "";

                string displayValue = "\"" + value + "\"";

                // Assign
                Console.WriteLine($"Found flag \"{flag}\" with value {displayValue}.");
                providedArguments.Add(flag, value);
            }
        }
        
        private static void BuildAddressables(string addressablesProfileName)
        {
            Console.WriteLine("Cleaning player content.");
            AddressableAssetSettings.CleanPlayerContent();

            AddressableAssetProfileSettings profileSettings =
                AddressableAssetSettingsDefaultObject.Settings.profileSettings;
            Console.WriteLine("Using active databuilder: " +
                              AddressableAssetSettingsDefaultObject.Settings.ActivePlayerDataBuilder.Name);

            Console.WriteLine("Setting profile to: " + addressablesProfileName);
            string profileId = profileSettings.GetProfileId(addressablesProfileName);
            AddressableAssetSettingsDefaultObject.Settings.activeProfileId = profileId;

            Console.WriteLine("Starting addressables content build.");
            // Build addressable content
            AddressableAssetSettings.BuildPlayerContent();

            Console.WriteLine("Building player content finished.");
        }
    }
}
