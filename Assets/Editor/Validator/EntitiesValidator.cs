using Units.Entities;
using UnityEditor;
using UnityEngine;

namespace Editor.Validator
{
    [InitializeOnLoad]
    public class EntitiesValidate
    {
        private const string TargetDirectory = "Assets/Scripts/Units/Entities/";

        // Static constructor that gets called when the editor starts
        static EntitiesValidate()
        {
            // Register callback to check before entering play mode
            EditorApplication.playModeStateChanged += ValidatePlayMode;
        }

        private static void ValidatePlayMode(PlayModeStateChange state)
        {
            // Only validate when entering Play Mode
            if (state == PlayModeStateChange.ExitingEditMode)
            {
                // Get all script files in the project
                var scriptGuids = AssetDatabase.FindAssets("t:Script", new[] { TargetDirectory });

                foreach (var guid in scriptGuids)
                {
                    var path = AssetDatabase.GUIDToAssetPath(guid);
                    var script = AssetDatabase.LoadAssetAtPath<MonoScript>(path);

                    // Get the class type of the script
                    var scriptType = script.GetClass();

                    // Skip if the script is an interface
                    if (scriptType.IsInterface) continue;

                    // Check if the class name does not contain "Controller" and does not have the required component
                    if (!scriptType.Name.Contains("Controller"))
                    {
                        var requireComponents =
                            scriptType.GetCustomAttributes(typeof(RequireComponent), false) as RequireComponent[];

                        var hasIController = false;
                        foreach (var attribute in requireComponents!)
                            if (attribute.m_Type0 == typeof(IController))
                            {
                                hasIController = true;
                                break;
                            }

                        // If no IController component is required, return error message and prevent Play Mode
                        if (!hasIController)
                        {
                            var errorMessage =
                                $"Class {scriptType.Name} does not include '[RequireComponent(typeof(IController))]' and will prevent Play Mode.";

                            // Display error message as a dialog box (Splash)
                            EditorUtility.DisplayDialog("Play Mode Blocked", errorMessage, "OK");

                            // Log error and prevent Play Mode
                            Debug.LogError(errorMessage);
                            EditorApplication.isPlaying = false;
                            return;
                        }
                    }
                }
            }
        }
    }
}