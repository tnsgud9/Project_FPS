using System.Collections.Generic;
using Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Managers
{
    public class InputManager : Singleton<InputManager>
    {
        private const string SaveFileName = "keyMappings.json"; // File name for saving and loading key mappings
        [SerializeField] private List<InputActionAsset> inputActionAssets; // List of Input Action Assets
        private Dictionary<string, string> _keyMappings; // Stores action names and their corresponding key bindings

        protected override void Awake()
        {
            base.Awake();
            LoadKeyMappings(); // Load key mappings from JSON on initialization
        }

        private void Start()
        {
            ApplyKeyMappings(); // Apply loaded key mappings to Input System
        }

        /// <summary>
        ///     Loads key mappings from a JSON file using FileSystem.
        /// </summary>
        public void LoadKeyMappings()
        {
            try
            {
                _keyMappings = FileSystem.Load<Dictionary<string, string>>(SaveFileName); // Load key mappings from file
            }
            catch
            {
                _keyMappings =
                    new Dictionary<string, string>(); // Initialize an empty mapping if the file doesn't exist
            }
        }

        /// <summary>
        ///     Saves the current key mappings to a JSON file using FileSystem.
        /// </summary>
        public void SaveKeyMappings()
        {
            FileSystem.Save(SaveFileName, _keyMappings); // Save key mappings to file
        }

        /// <summary>
        ///     Applies the key mappings to all InputActionAssets in the list.
        /// </summary>
        public void ApplyKeyMappings()
        {
            foreach (var asset in inputActionAssets)
            foreach (var actionMap in asset.actionMaps)
            foreach (var action in actionMap.actions)
                if (_keyMappings.TryGetValue(action.name, out var key))
                    action.ApplyBindingOverride(0,
                        new InputBinding { overridePath = key }); // Override binding with the mapped key
        }

        /// <summary>
        ///     Updates the key mapping for a specific action.
        /// </summary>
        /// <param name="actionName">The name of the action to update.</param>
        /// <param name="newKey">The new key binding for the action.</param>
        public void UpdateKeyMapping(string actionName, string newKey)
        {
            _keyMappings[actionName] = $"<Keyboard>/{newKey}"; // Update the dictionary with the new key binding
            SaveKeyMappings(); // Save changes to the JSON file
            ApplyKeyMappings(); // Apply updated mappings to the Input System
        }

        /// <summary>
        ///     Retrieves the current key binding for a specific action.
        /// </summary>
        /// <param name="actionName">The name of the action to check.</param>
        /// <returns>The key binding for the action, or "Not Set" if not found.</returns>
        public string GetCurrentKey(string actionName)
        {
            return _keyMappings.GetValueOrDefault(actionName, "Not Set");
        }
    }
}