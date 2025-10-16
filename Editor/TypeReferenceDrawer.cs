using Rafasixteen.TypeReference.Runtime;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Rafasixteen.TypeReference.Editor
{
    [CustomPropertyDrawer(typeof(TypeReference<>))]
    public class TypeReferenceDrawer : PropertyDrawer
    {
        private const string TypeFullNameField = "_typeFullName";
        
        private static Type[] _availableTypes;

        private static string[] _typeFullNames;

        private static string[] _displayNames;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty typeNameProperty = property.FindPropertyRelative("_typeFullName");

            if (typeNameProperty == null)
            {
                EditorGUI.HelpBox(
                    position,
                    $"{nameof(TypeReferenceDrawer)} Error: Missing serialized field '{TypeFullNameField}' " +
                    $"in {nameof(TypeReference)}.\n" +
                    $"Ensure your {nameof(TypeReference)} class defines a [SerializeField] " +
                    $"string field named '{TypeFullNameField}'.",
                    MessageType.Error
                );

                return;
            }

            Type genericType = GetGenericArgumentType(fieldInfo.FieldType);

            if (genericType == null)
            {
                EditorGUI.HelpBox(
                    position,
                    $"{nameof(TypeReferenceDrawer)} Error: Unable to determine the generic type parameter " +
                    $"for field '{fieldInfo.Name}'.\n" +
                    $"Expected a field of type {nameof(TypeReference)}<T>, " +
                    $"an array, or a list of it.\n" +
                    $"Actual type: {fieldInfo.FieldType.FullName}",
                    MessageType.Error
                );
                return;
            }

            _availableTypes = TypeCache.GetTypesDerivedFrom(genericType)
                .Where(t => !t.IsAbstract)
                .OrderBy(t => t.Name)
                .ToArray();

            int total = _availableTypes.Length + 1;
            _typeFullNames = new string[total];
            _displayNames = new string[total];

            _typeFullNames[0] = null;
            _displayNames[0] = "None";

            for (int i = 0; i < _availableTypes.Length; i++)
            {
                _typeFullNames[i + 1] = _availableTypes[i].FullName;
                _displayNames[i + 1] = _availableTypes[i].Name;
            }

            float dropdownWidth = position.width * 0.75f;
            float labelWidth = position.width - dropdownWidth;

            Rect labelRect = new(position.x, position.y, labelWidth, position.height);
            Rect dropdownRect = new(position.x + labelWidth, position.y, dropdownWidth, position.height);

            EditorGUI.PrefixLabel(labelRect, GUIUtility.GetControlID(FocusType.Passive), label);

            int currentIndex = Array.IndexOf(_typeFullNames, typeNameProperty.stringValue);

            if (currentIndex < 0)
                currentIndex = 0;

            int selectedIndex = EditorGUI.Popup(dropdownRect, currentIndex, _displayNames);

            if (selectedIndex != currentIndex)
                typeNameProperty.stringValue = _typeFullNames[selectedIndex];
        }

        private static Type GetGenericArgumentType(Type type)
        {
            if (type.IsArray)
                type = type.GetElementType();

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(TypeReference<>))
                return type.GetGenericArguments()[0];

            return null;
        }
    }
}