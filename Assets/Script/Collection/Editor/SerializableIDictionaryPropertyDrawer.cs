using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Yd.Collection
{
    [CustomPropertyDrawer(typeof(SerializableIDictionaryBase), true)]
    public class SerializableIDictionaryPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var list = property.FindPropertyRelative("list");
            // EditorGUILayout.PropertyField(list, label, true);

            EditorGUI.PropertyField(position, list, label, true);

            if (property.boxedValue != null)
            {
                var dict = (SerializableIDictionaryBase)property.boxedValue;

                var warningText = dict.GenerateWarningText();
                if (warningText == "")
                {
                    return;
                }

                var warningLines = warningText.Count(c => c == '\n') + 1;

                position.y += EditorGUI.GetPropertyHeight(list, true);
                if (!list.isExpanded)
                {
                    position.y += EditorGUIUtility.standardVerticalSpacing;
                }

                position.height = EditorGUIUtility.singleLineHeight * warningLines;
                position = EditorGUI.IndentedRect(position);
                EditorGUI.HelpBox(position, warningText, MessageType.Warning);
            }
        }

        // public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        // {
        //     return EditorGUIUtility.singleLineHeight;
        // }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var height = 0f;
            var list = property.FindPropertyRelative("list");
            height += EditorGUI.GetPropertyHeight(list, true);

            if (property.boxedValue != null)
            {
                var warningText = ((SerializableIDictionaryBase)property.boxedValue).GenerateWarningText();
                if (warningText == "")
                {
                    return height;
                }

                var warningLines = warningText.Count(c => c == '\n') + 1;

                height += EditorGUIUtility.singleLineHeight * warningLines;
            }

            return height;
        }
    }
}