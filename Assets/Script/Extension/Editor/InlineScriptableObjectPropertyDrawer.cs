using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Yd.Collection;

namespace Yd.Extension
{
    [CustomPropertyDrawer(typeof(ScriptableObject), true)]
    public class InlineScriptableObjectPropertyDrawer : PropertyDrawer
    {
        private static readonly GUIStyle FoldoutStyle;

        private static readonly Dictionary<int, bool> Foldout = new();

        static InlineScriptableObjectPropertyDrawer()
        {
            FoldoutStyle = new GUIStyle(EditorStyles.foldout)
            {
                margin = EditorStyles.foldoutHeader.margin,
                padding = EditorStyles.foldoutHeader.padding,
                fontStyle = EditorStyles.foldoutHeader.fontStyle,
                fontSize = EditorStyles.foldoutHeader.fontSize,
                fixedHeight = EditorStyles.foldoutHeader.fixedHeight
            };
            FoldoutStyle.margin.left = 0;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Init()
        {
            Foldout.Clear();
        }

        private static int GenerateUniqueKey(SerializedProperty property)
        {
            return property.propertyPath.GetHashCode()
                   ^ property.serializedObject.targetObject.GetHashCode(); // 生成一个唯一的键，结合属性路径和目标对象的哈希码
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var key = GenerateUniqueKey(property);
            Foldout.TryAdd(key, false);

            var verticalMargin = (int)(position.height - FoldoutStyle.fixedHeight) / 2;
            FoldoutStyle.margin.top = verticalMargin;
            FoldoutStyle.margin.bottom = verticalMargin;


            // 计算 Foldout 和 ObjectField 的位置
            Rect foldoutRect = new(position.x, position.y, EditorGUIUtility.labelWidth, FoldoutStyle.fixedHeight);
            Rect objectFieldRect = new
            (
                position.x + foldoutRect.width + FoldoutStyle.margin.right,
                position.y + FoldoutStyle.margin.top,
                position.width - EditorGUIUtility.labelWidth - 2,
                FoldoutStyle.fixedHeight
            );

            // 通过 DrawRect 的方式实现 hover 高亮
            var isHovered = position.Contains(Event.current.mousePosition);
            if (isHovered)
            {
                EditorGUI.DrawRect
                    (EditorGUI.IndentedRect(position), new Color(69f / 255, 69f / 255, 69f / 255)); // IndentedRect 获取考虑了缩进的位置
            }

            // 无论怎样设置 foldoutStyle 都无法实现 hover 高亮，因为 EditorGUI.Foldout 根本就没判断 hover ，我草
            Foldout[key] = EditorGUI.Foldout(foldoutRect, Foldout[key], label, true, FoldoutStyle);

            // 绘制选择 ScriptableObject 的框
            using (new EditorGUI.IndentLevelScope(-EditorGUI.indentLevel))
            {
                EditorGUI.PropertyField(objectFieldRect, property, GUIContent.none, true);
            }

            var scriptableObject = (ScriptableObject)property.objectReferenceValue;
            if (scriptableObject != null && Foldout[key])
            {
                var serializedScriptableObject = new SerializedObject(scriptableObject);
                var iterator = serializedScriptableObject.GetIterator();


                // NOTE 使用 EditorGUILayout 的问题是嵌套的数组不支持，需要改成使用 EditorGUI 绘制，同时重写 GetPropertyHeight
                using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
                {
                    if (iterator.NextVisible(true))
                    {
                        EditorGUILayout.PropertyField(iterator, true); // Enter MonoBehaviour
                    }

                    while (iterator.NextVisible(false))
                    {
                        using (new EditorGUI.IndentLevelScope
                            (iterator.isArray || iterator.boxedValue is SerializableIDictionaryBase ? 1 : 0))
                        {
                            EditorGUILayout.PropertyField(iterator, true);
                        }
                    }
                }

                if (serializedScriptableObject.hasModifiedProperties)
                {
                    serializedScriptableObject.ApplyModifiedProperties();
                }
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return FoldoutStyle.fixedHeight;
        }
    }
}