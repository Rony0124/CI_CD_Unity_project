using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

namespace UnityBuilderAction {
    internal static class Builder {
        private static readonly string Eol = Environment.NewLine;

        public static void Build() {
            // Gather values from args
            //Dictionary<string, string> options = GetValidatedOptions();

            // Build addressables content
            BuildAddressables("develop");

            // Custom build
            //Build(buildTarget, options["customBuildPath"]);
        }

        private static void BuildAddressables(string addressablesProfileName) {
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

        private static Dictionary<string, string> GetValidatedOptions() {
            ParseCommandLineArguments(out Dictionary<string, string> validatedOptions);

            if (!validatedOptions.TryGetValue("projectPath", out string _)) {
                Console.WriteLine("Missing argument -projectPath");
                EditorApplication.Exit(110);
            }

            if (!validatedOptions.TryGetValue("buildTarget", out string buildTarget)) {
                Console.WriteLine("Missing argument -buildTarget");
                EditorApplication.Exit(120);
            }

            if (!Enum.IsDefined(typeof(BuildTarget), buildTarget ?? string.Empty)) {
                EditorApplication.Exit(121);
            }

            if (!validatedOptions.TryGetValue("customBuildPath", out string _)) {
                Console.WriteLine("Missing argument -customBuildPath");
                EditorApplication.Exit(130);
            }

            const string defaultEnableDevBuildValue = "false";

            if (!validatedOptions.TryGetValue("devBuild", out var isDevBuild)) {
                Console.WriteLine($"Missing argument -devBuild, defaulting to {defaultEnableDevBuildValue}.");
                validatedOptions.Add("devBuild", defaultEnableDevBuildValue);
            }

            const string defaultCustomBuildName = "TestBuild";

            if (!validatedOptions.TryGetValue("customBuildName", out string customBuildName)) {
                Console.WriteLine($"Missing argument -customBuildName, defaulting to {defaultCustomBuildName}.");
                validatedOptions.Add("customBuildName", defaultCustomBuildName);
            } else if (customBuildName == "") {
                Console.WriteLine($"Invalid argument -customBuildName, defaulting to {defaultCustomBuildName}.");
                validatedOptions.Add("customBuildName", defaultCustomBuildName);
            }

            return validatedOptions;
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
                string displayValue =  "\"" + value + "\"";

                // Assign
                Console.WriteLine($"Found flag \"{flag}\" with value {displayValue}.");
                providedArguments.Add(flag, value);
            }
        }
    }
}
