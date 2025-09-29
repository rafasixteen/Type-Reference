using Rafasixteen.TypeReference.Runtime;
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Rafasixteen.TypeReference.Editor
{
    [CustomPropertyDrawer(typeof(TypeReference<>), true)]
    public class TypeReferenceDrawer : PropertyDrawer
    {
        private static Type[] _availableTypes;

        private static string[] _typeFullNames;

        private static string[] _displayNames;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty typeProperty = property.FindPropertyRelative("_typeFullName");
            Type fieldType = fieldInfo.FieldType.GetGenericArguments().First();

            _availableTypes = TypeCache.GetTypesDerivedFrom(fieldType)
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

            int currentIndex = Array.IndexOf(_typeFullNames, typeProperty.stringValue);

            if (currentIndex < 0)
                currentIndex = 0;

            int selectedIndex = EditorGUI.Popup(dropdownRect, currentIndex, _displayNames);

            if (selectedIndex != currentIndex)
                typeProperty.stringValue = _typeFullNames[selectedIndex];
        }
    }
}