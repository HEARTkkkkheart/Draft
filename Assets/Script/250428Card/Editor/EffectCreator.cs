using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class EffectCreator : EditorWindow
{
    private List<string> items = new List<string>() { "Attack1", "Attack2" };
    private string selectedItem;
    private string targetDirectory = "Assets/Script/250428Card/EffectItem";
    private Vector2 scrollPosition;


    [MenuItem("Tools/Effect Creator")]
    public static void ShowWindow()
    {
        GetWindow<EffectCreator>("Script Generator");
    }

    void OnGUI()
    {
        GUILayout.Label("Script Generator", EditorStyles.boldLabel);

        // 目录设置
        EditorGUILayout.BeginHorizontal();
        targetDirectory = EditorGUILayout.TextField("Save Directory", targetDirectory);
        if (GUILayout.Button("Browse", GUILayout.Width(80)))
        {
            targetDirectory = EditorUtility.SaveFolderPanel("Select Save Directory", targetDirectory, "");
            if (targetDirectory.StartsWith(Application.dataPath))
            {
                targetDirectory = "Assets" + targetDirectory.Substring(Application.dataPath.Length);
            }

            Repaint();
        }

        EditorGUILayout.EndHorizontal();

        // 列表区域
        GUILayout.Space(10);
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Class List:");
        // if (GUILayout.Button("Refresh List", GUILayout.Width(100)))
        // {
        //     Repaint(); // 强制刷新界面
        // }
        EditorGUILayout.EndHorizontal();

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        foreach (var item in items)
        {
            if (GUILayout.Button(item))
            {
                selectedItem = item;
                Debug.Log("Selected: " + item);
            }
        }

        EditorGUILayout.EndScrollView();

        // 功能操作区域
        GUILayout.Space(15);
        EditorGUILayout.BeginVertical(GUI.skin.box);
        GUILayout.Label("Script Operations", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Generate Selected", GUILayout.Width(120)))
        {
            if (!string.IsNullOrEmpty(selectedItem))
            {
                GenerateScript(selectedItem);
            }
            else
            {
                EditorUtility.DisplayDialog("Error", "Please select a class first!", "OK");
            }
        }

        if (GUILayout.Button("Generate Missing", GUILayout.Width(120)))
        {
            GenerateMissingScripts();
        }

        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Delete Orphans", GUILayout.Width(120)))
        {
            DeleteOrphanScripts();
        }

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
    }

    private void GenerateScript(string className)
    {
        if (string.IsNullOrEmpty(className)) return;

        string fullPath =
            Path.Combine(Application.dataPath, targetDirectory.Replace("Assets/", "") + $"/{className}.cs");
        string assetPath = Path.Combine(targetDirectory, $"{className}.cs");

        if (File.Exists(fullPath))
        {
            if (!EditorUtility.DisplayDialog("Warning",
                    $"Script {className}.cs already exists!\nOverwrite?",
                    "Yes", "No")) return;
        }

        Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

        string scriptContent = $@"using UnityEngine;

public class {className} : IEffectComponent
{{
    public void InitData()
    {{
    }}

    public void OnApply()
    {{
    }}

    public void OnTrigger()
    {{
    }}

    public void OnRemove()
    {{
    }}
}}";

        File.WriteAllText(fullPath, scriptContent);
        AssetDatabase.Refresh();
        Debug.Log($"Generated script: {assetPath}");
    }

    private void GenerateMissingScripts()
    {
        if (items == null || items.Count == 0) return;

        foreach (var item in items)
        {
            string checkPath = Path.Combine(targetDirectory, $"{item}.cs");
            if (!AssetDatabase.LoadAssetAtPath<MonoScript>(checkPath))
            {
                GenerateScript(item);
            }
        }
    }

    private void DeleteOrphanScripts()
    {
        if (string.IsNullOrEmpty(targetDirectory))
        {
            EditorUtility.DisplayDialog("Error", "Target directory is not set.", "OK");
            return;
        }

        string fullPath = Path.Combine(Application.dataPath, targetDirectory.Replace("Assets/", ""));

        if (!Directory.Exists(fullPath))
        {
            EditorUtility.DisplayDialog("Info", "Target directory does not exist.", "OK");
            return;
        }

        Debug.Log($"Deleting orphan scripts: {fullPath}");
        string[] allScripts = Directory.GetFiles(fullPath, "*.cs", SearchOption.TopDirectoryOnly);
        Debug.Log($"allScripts: {allScripts.Length}");
        List<string> orphanFiles = new List<string>();

        foreach (var scriptPath in allScripts)
        {
            string fileName = Path.GetFileNameWithoutExtension(scriptPath);
            if (!items.Contains(fileName))
            {
                orphanFiles.Add(scriptPath);
            }
        }

        if (orphanFiles.Count == 0)
        {
            EditorUtility.DisplayDialog("Info", "No orphan scripts found.", "OK");
            return;
        }

        bool confirm = EditorUtility.DisplayDialog("Confirm Delete",
            $"Found {orphanFiles.Count} orphan scripts:\n{string.Join("\n", orphanFiles)}\nDelete them?",
            "Yes", "No");

        if (!confirm) return;

        foreach (var filePath in orphanFiles)
        {
            try
            {
                File.Delete(filePath);
                string metaFile = filePath + ".meta";
                if (File.Exists(metaFile))
                {
                    File.Delete(metaFile);
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to delete {filePath}: {e.Message}");
            }
        }

        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("Success", $"Deleted {orphanFiles.Count} orphan scripts.", "OK");
    }

    public void SetClassList(List<string> classNames)
    {
        items = classNames;
        Repaint();
    }
}