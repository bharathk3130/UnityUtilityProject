using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Clickbait.Editor
{
    [CustomEditor(typeof(MonoBehaviour), true)] // Apply to all MonoBehaviours
    public class CustomScriptableObjectEditor : UnityEditor.Editor
    {
        bool _persistFoldoutState = true; // Add a toggle to decide if foldout should persist

        Dictionary<Object, bool> _showScriptableObjectFields = new();

        // Method to get a unique key for storing the foldout state in EditorPrefs
        string GetFoldoutKey(ScriptableObject obj)
        {
            return $"{target.GetType().Name}_{obj.GetInstanceID()}_Foldout";
        }

        public override void OnInspectorGUI()
        {
            // Draw the "Script" field to allow double-clicking the script to open it
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour((MonoBehaviour)target), typeof(MonoScript), false);
            EditorGUI.EndDisabledGroup();

            // Toggle to control if foldout state should persist
            // _persistFoldoutState = EditorGUILayout.Toggle("Persist Foldout State", _persistFoldoutState);

            serializedObject.Update(); // Update serialized object

            // Get all fields of the MonoBehaviour script
            SerializedProperty iterator = serializedObject.GetIterator();
            iterator.NextVisible(true); // Skip the "script" reference field

            // Loop through each field
            while (iterator.NextVisible(false))
            {
                // Draw default field UI
                EditorGUILayout.PropertyField(iterator, true);

                // Check if the field is a ScriptableObject reference
                if (iterator.propertyType == SerializedPropertyType.ObjectReference)
                {
                    Object referencedObject = iterator.objectReferenceValue;
                    if (referencedObject is ScriptableObject)
                    {
                        ScriptableObject scriptableObject = (ScriptableObject)referencedObject;

                        // Get a unique key for this ScriptableObject
                        string foldoutKey = GetFoldoutKey(scriptableObject);

                        // Handle foldout state persistence
                        if (_persistFoldoutState)
                        {
                            // Retrieve foldout state from EditorPrefs (persistent between inspector views)
                            if (!_showScriptableObjectFields.ContainsKey(referencedObject))
                            {
                                _showScriptableObjectFields.Add(referencedObject,
                                    EditorPrefs.GetBool(foldoutKey, false));
                            }
                        }
                        else
                        {
                            // Initialize the foldout state without persistence
                            if (!_showScriptableObjectFields.ContainsKey(referencedObject))
                            {
                                _showScriptableObjectFields.Add(referencedObject, false); // Start as closed
                            }
                        }

                        // Create a GUIContent with the name of the ScriptableObject
                        GUIContent foldoutLabel = new GUIContent("Show " + scriptableObject.name + " Fields");

                        // Toggle the foldout, and update the foldout state in EditorPrefs if persisting
                        bool newFoldoutState = EditorGUILayout.Foldout(
                            _showScriptableObjectFields[referencedObject],
                            foldoutLabel,
                            true // 'true' enables the label to be clicked to toggle the foldout
                        );

                        if (newFoldoutState != _showScriptableObjectFields[referencedObject])
                        {
                            // Update foldout state in the dictionary
                            _showScriptableObjectFields[referencedObject] = newFoldoutState;

                            // If persisting, store the foldout state in EditorPrefs
                            if (_persistFoldoutState)
                            {
                                EditorPrefs.SetBool(foldoutKey, newFoldoutState);
                            }
                        }

                        if (_showScriptableObjectFields[referencedObject])
                        {
                            // Increase indentation
                            EditorGUI.indentLevel++;

                            // Create a SerializedObject to show the fields of the ScriptableObject
                            SerializedObject so = new SerializedObject(scriptableObject);
                            SerializedProperty soIterator = so.GetIterator();
                            soIterator.NextVisible(true); // Skip "m_Script"

                            // Iterate through all fields in the ScriptableObject
                            while (soIterator.NextVisible(false))
                            {
                                EditorGUILayout.PropertyField(soIterator, true);
                            }

                            // Apply modified properties
                            so.ApplyModifiedProperties();

                            // Reset indentation
                            EditorGUI.indentLevel--;
                        }
                    }
                }
            }

            serializedObject.ApplyModifiedProperties(); // Apply changes to the serialized object
        }
    }
}