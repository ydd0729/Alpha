using UnityEditor;
using UnityEngine;

namespace Yd.StylizedHumanMaleV2
{
    [CustomEditor(typeof(StylizedHumanMaleV2ConfigureButton))]
    public class StylizedHumanMaleV2ConfigureButtonEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Configure"))
            {
                ((StylizedHumanMaleV2ConfigureButton)target).gameObject.GetComponent<StylizedHumanMaleV2>().Configure();
            }
        }
    }
}