using UnityEditor;
using UnityEngine;

namespace Yd.Tool.Editor
{
    [CustomEditor(typeof(BonesSetter))]
    public class BonesSetterEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var myTarget = (BonesSetter)target;

            if (GUILayout.Button("Set Bones"))
            {
                myTarget.SetBones();
            }
        }
    }
}