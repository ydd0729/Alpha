using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Shared.Collections
{
    [CustomPropertyDrawer(typeof(SerializableIDictionary<,,>), useForChildren: true)]
    public class SerializableIDictionaryPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Draw list of key/value pairs.
            var list = property.FindPropertyRelative("list");
            EditorGUI.PropertyField(position, list, label, true);

            // Draw warning.
            var warningText = property.FindPropertyRelative("warningText").stringValue;
            var warningLines = warningText.Count(c => c == '\n') + 1;

            if (warningText == "") return;
            position.y += EditorGUI.GetPropertyHeight(list, true);
            if (!list.isExpanded)
            {
                position.y += EditorGUIUtility.standardVerticalSpacing;
            }
            position.height = EditorGUIUtility.singleLineHeight * warningLines;
            position = EditorGUI.IndentedRect(position);
            EditorGUI.HelpBox(position, warningText, MessageType.Warning);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            // Height of KeyValue list.
            float height = 0f;
            var list = property.FindPropertyRelative("list");
            height += EditorGUI.GetPropertyHeight(list, true);

            // Height of warning.
            var warningText = property.FindPropertyRelative("warningText").stringValue;
            var warningLines = warningText.Count(c => c == '\n') + 1;

            if (warningText == "") return height;
            height += EditorGUIUtility.singleLineHeight  * warningLines;
            if (!list.isExpanded)
            {
                height += EditorGUIUtility.standardVerticalSpacing;
            }

            return height;
        }
    }
}