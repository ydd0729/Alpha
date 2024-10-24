#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class ConvertMaterialsToURP : EditorWindow
{
    private int conversionOption;
    private readonly string[] options = { "Chosen Folder + All Subfolders", "Only Chosen Folder", "Whole Project" };
    private string path;
    private bool showInstructions;

    private void OnGUI()
    {
        GUILayout.Label("Convert materials to URP", EditorStyles.boldLabel);
        GUILayout.Space(20);

        conversionOption = EditorGUILayout.Popup("Conversion Scope", conversionOption, options);

        GUILayout.Space(30);
        if (GUILayout.Button("Select Folder") && conversionOption != 2)
        {
            path = EditorUtility.OpenFolderPanel("Select Folder with Materials", "", "");
            if (!string.IsNullOrEmpty(path))
            {
                ConvertMatsToURP(path, conversionOption);
            }
        }

        if (conversionOption == 2)
        {
            if (GUILayout.Button("Convert Entire Project"))
            {
                ConvertMatsToURP(Application.dataPath, conversionOption);
            }
        }

        GUILayout.Space(30);
        EditorGUILayout.HelpBox(
                "Please note that this process is irreversible.\n Before using this script, we recommend you to make a backup of your project to avoid unwanted changes.",
                MessageType.Warning
            );
        GUILayout.Space(30);
        showInstructions = EditorGUILayout.Foldout(showInstructions, "Instructions for the converted materials of the Stylized Character assets");
        if (showInstructions)
        {
            EditorGUILayout.HelpBox(
                    "\n  For converted materials to URP of the Stylized Character assets we recommend to change 'Sorting Priority' Parameter for the layered materials like Body for proper rendering of the pants/gloves, etc. in 'Advanced Options' Foldout menu of the material.\n" +
                    "\n  Recommended values on Sorting Priority\n\n" + "  0: 旴ody\n" + "      旽ead\n\n" + "  1: 旴ody_Add\n" + "      旽ead_Add\n\n" + "  2: 旴ody_Cloth\n" +
                    "      昒H (Underhair)\n\n" +
                    "  3: 旵hest\n\n" + "  4: 昉ants\n\n" + "  5: 旼loves\n",
                    MessageType.Info
                );
        }
    }

    [MenuItem("Tools/Material Converter to URP")]
    public static void ShowWindow()
    {
        var window = GetWindow<ConvertMaterialsToURP>("Material Converter to URP");
        window.minSize = new Vector2(300, 300);
    }

    private void ConvertMatsToURP(string folderPath, int option)
    {
        var searchOption = option == 0 ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
        var materialFiles = Directory.GetFiles(folderPath, "*.mat", searchOption);
        var convertedMaterialsCount = 0;

        AssetDatabase.StartAssetEditing();

        foreach (var materialFile in materialFiles)
        {
            var relativePath = "Assets" + materialFile.Replace(Application.dataPath, "").Replace('\\', '/');
            var material = AssetDatabase.LoadAssetAtPath<Material>(relativePath);

            if (material != null)
            {
                if (ConvertMaterial(material))
                {
                    convertedMaterialsCount++;
                    EditorUtility.SetDirty(material);
                    Debug.Log("Converted Material: " + relativePath, material);
                }
            }
        }

        AssetDatabase.StopAssetEditing();
        AssetDatabase.SaveAssets();
        Debug.Log($"Conversion complete. Converted {convertedMaterialsCount} materials to URP.");
    }

    private bool ConvertMaterial(Material material)
    {
        if (material.shader.name == "Standard")
        {
            var OldMat = new Material(material);
            material.shader = Shader.Find("Universal Render Pipeline/Lit");

            if (OldMat.HasProperty("_MainTex") && material.HasProperty("_BaseMap"))
            {
                material.SetTexture("_BaseMap", OldMat.GetTexture("_MainTex"));
                material.SetColor("_BaseColor", OldMat.GetColor("_Color"));
                Debug.Log("Albedo and Color transferred successfully.");
            }
            else
            {
                Debug.Log("Failed to find _MainTex or _BaseMap properties.");
            }

            if (OldMat.HasProperty("_Glossiness"))
            {
                material.SetFloat("_Smoothness", OldMat.GetFloat("_Glossiness"));
            }

            #region Render Modes

            if (OldMat.GetFloat("_Mode") == 1) // Cutout
            {
                material.SetFloat("_Surface", 0);
                material.SetFloat("_AlphaClip", 1);
                material.SetFloat("_Cutoff", material.GetFloat("_Cutoff"));
                material.renderQueue = 2450;
            }

            if (OldMat.GetFloat("_Mode") == 2) // Fade
            {
                material.SetFloat("_Surface", 1);
                material.renderQueue = 3000;
                material.SetInt("_Blend", (int)BlendMode.SrcAlpha);
                material.SetInt("_BlendOp", (int)BlendOp.Add);
                material.SetInt("_SrcBlend", (int)BlendMode.SrcAlpha);
                material.SetInt("_DstBlend", (int)BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.EnableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.SetFloat("_AlphaClip", 1);
                material.SetFloat("_AlphaCutoff", 0.0f);
            }

            #endregion

            return true;
        }

        return false;
    }
}
#endif