// using System.Collections.Generic;
// using UnityEditor;
// using UnityEngine;
//
// namespace Yd.Collection
// {
//     [CustomPropertyDrawer(typeof(SKeyValuePair<,>), true)]
//     public class SerializableKeyValuePairPropertyDrawer : PropertyDrawer
//     {
//         private static readonly Dictionary<int, bool> Foldout = new();
//         private static readonly Dictionary<int, float> Height = new();
//
//         [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
//         private static void Init()
//         {
//             Foldout.Clear();
//             Height.Clear();
//         }
//
//         private static int GenerateUniqueKey(SerializedProperty property)
//         {
//             return property.propertyPath.GetHashCode() ^ property.serializedObject.targetObject.GetHashCode(); // 生成一个唯一的键，结合属性路径和目标对象的哈希码
//         }
//
//         public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//         {
//             position.height = EditorGUIUtility.singleLineHeight;
//
//             var propertyKey = GenerateUniqueKey(property);
//             Height[propertyKey] = position.height;
//             Foldout.TryAdd(propertyKey, false);
//
//             var foldoutStyle = new GUIStyle(EditorStyles.foldout);
//
//             Foldout[propertyKey] = EditorGUI.Foldout(position, Foldout[propertyKey], label, true, foldoutStyle);
//             position.y += position.height;
//
//             if (Foldout[propertyKey])
//             {
//                 var key = property.FindPropertyRelative("key");
//                 var value = property.FindPropertyRelative("value");
//
//                 EditorGUI.PropertyField(position, key, new GUIContent("Key"));
//                 var height = EditorGUI.GetPropertyHeight(key, true) + EditorGUIUtility.standardVerticalSpacing;
//                 position.y += height;
//                 Height[propertyKey] += height;
//
//                 EditorGUI.PropertyField(position, value, new GUIContent("Value"));
//                 Height[propertyKey] += EditorGUI.GetPropertyHeight(value, true);
//             }
//             else
//             {
//                 Height[propertyKey] += EditorGUIUtility.standardVerticalSpacing;
//             }
//
//
//             Debug.Log(property.isArray);
//             Debug.Log(property.propertyPath);
//         }
//
//         public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
//         {
//             return Height.GetValueOrDefault(GenerateUniqueKey(property), EditorGUIUtility.singleLineHeight);
//         }
//     }
// }

