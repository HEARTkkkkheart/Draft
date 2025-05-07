using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System.Text;


public class EffectCreator : EditorWindow
{
    private enum FileType
    {
        Buff = 0,
        Card = 1,
    }

    // private List<string> items = new List<string>() { "Attack1", "Attack2" };
    private List<BuffData> BuffDataList = new List<BuffData>();

    private List<CardData> CardDataList = new List<CardData>();
    private string selectedItem;
    private FileType _type;
    private string BuffTargetDirectory = "Assets/Script/250428Card/BuffItem";
    private string CardTargetDirectory = "Assets/Script/250428Card/CardItem";
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
        if (GUILayout.Button("LoadConfig"))
        {
            LoadConfig();
        }

        // if (GUILayout.Button("LoadConfig"))
        // {
        //     LoadConfig();
        // }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        BuffTargetDirectory = EditorGUILayout.TextField("Save Directory", BuffTargetDirectory);
        if (GUILayout.Button("Browse", GUILayout.Width(80)))
        {
            BuffTargetDirectory = EditorUtility.SaveFolderPanel("Select Save Directory", BuffTargetDirectory, "");
            if (BuffTargetDirectory.StartsWith(Application.dataPath))
            {
                BuffTargetDirectory = "Assets" + BuffTargetDirectory.Substring(Application.dataPath.Length);
            }

            Repaint();
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        CardTargetDirectory = EditorGUILayout.TextField("Save Directory", CardTargetDirectory);
        if (GUILayout.Button("Browse", GUILayout.Width(80)))
        {
            CardTargetDirectory = EditorUtility.SaveFolderPanel("Select Save Directory", CardTargetDirectory, "");
            if (CardTargetDirectory.StartsWith(Application.dataPath))
            {
                CardTargetDirectory = "Assets" + CardTargetDirectory.Substring(Application.dataPath.Length);
            }

            Repaint();
        }

        EditorGUILayout.EndHorizontal();

        // 列表区域
        GUILayout.Space(10);
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Buff List:");
        // if (GUILayout.Button("Refresh List", GUILayout.Width(100)))
        // {
        //     Repaint(); // 强制刷新界面
        // }
        EditorGUILayout.EndHorizontal();

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        foreach (var item in BuffDataList)
        {
            if (GUILayout.Button(item.Name))
            {
                selectedItem = item.Name;
                _type = FileType.Buff;
                Debug.Log("Selected: " + item.Name);
            }
        }

        EditorGUILayout.EndScrollView();

        GUILayout.Space(10);
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Card List:");
        // if (GUILayout.Button("Refresh List", GUILayout.Width(100)))
        // {
        //     Repaint(); // 强制刷新界面
        // }
        EditorGUILayout.EndHorizontal();

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        foreach (var item in CardDataList)
        {
            if (GUILayout.Button(item.Name))
            {
                selectedItem = item.Name;
                _type = FileType.Card;
                Debug.Log("Selected: " + item.Name);
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
        string file = null;
        if (_type == FileType.Buff)
        {
            file = BuffTargetDirectory;
        }
        else
        {
            file = CardTargetDirectory;
        }

        if (string.IsNullOrEmpty(className)) return;

        string fullPath =
            Path.Combine(Application.dataPath, file.Replace("Assets/", "") + $"/{className}.cs");
        string assetPath = Path.Combine(file, $"{className}.cs");

        if (File.Exists(fullPath))
        {
            if (!EditorUtility.DisplayDialog("Warning",
                    $"Script {className}.cs already exists!\nOverwrite?",
                    "Yes", "No")) return;
        }

        Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

        string scriptContent = $@"using UnityEngine;

public class {className} : IBuffComponent
{{
    public void InitData()
    {{
    }}

    public void OnApply(GameObject target, GameObject user)
    {{
    }}

    public void OnTrigger(GameObject target, GameObject user)
    {{
    }}

    public void OnRemove(GameObject target, GameObject user)
    {{
    }}
}}";

        File.WriteAllText(fullPath, scriptContent);
        LoadConfig();
        Debug.Log($"Generated script: {assetPath}");
    }

    private void GenerateMissingScripts()
    {
        if (BuffDataList != null && BuffDataList.Count != 0)
        {
            foreach (var item in BuffDataList)
            {
                string checkPath = Path.Combine(BuffTargetDirectory, $"{item.Name}.cs");
                if (!AssetDatabase.LoadAssetAtPath<MonoScript>(checkPath))
                {
                    _type = FileType.Buff;
                    GenerateScript(item.Name);
                }
            }
        }

        if (CardDataList != null && CardDataList.Count != 0)
        {
            foreach (var item in CardDataList)
            {
                string checkPath = Path.Combine(CardTargetDirectory, $"{item.Name}.cs");
                if (!AssetDatabase.LoadAssetAtPath<MonoScript>(checkPath))
                {
                    _type = FileType.Card;
                    GenerateScript(item.Name);
                }
            }
        }
    }

    private void DeleteOrphanScripts()
    {
        if (string.IsNullOrEmpty(BuffTargetDirectory))
        {
            EditorUtility.DisplayDialog("Error", "Target directory is not set.", "OK");
            return;
        }

        string fullPath = Path.Combine(Application.dataPath, BuffTargetDirectory.Replace("Assets/", ""));

        if (!Directory.Exists(fullPath))
        {
            EditorUtility.DisplayDialog("Info", "Target directory does not exist.", "OK");
            return;
        }

        Debug.Log($"Deleting orphan scripts: {fullPath}");
        string[] allBuffScripts = Directory.GetFiles(fullPath, "*.cs", SearchOption.TopDirectoryOnly);
        Debug.Log($"allScripts: {allBuffScripts.Length}");
        List<string> orphanBuffFiles = new List<string>();

        foreach (var scriptPath in allBuffScripts)
        {
            string fileName = Path.GetFileNameWithoutExtension(scriptPath);
            foreach (var buff in BuffDataList)
            {
                if (buff.Name != fileName)
                {
                    orphanBuffFiles.Add(scriptPath);
                }
            }
        }

        if (orphanBuffFiles.Count == 0)
        {
            EditorUtility.DisplayDialog("Info", "No orphan scripts found.", "OK");
            return;
        }

        bool confirmBuff = EditorUtility.DisplayDialog("Confirm Delete",
            $"Found {orphanBuffFiles.Count} orphan scripts:\n{string.Join("\n", orphanBuffFiles)}\nDelete them?",
            "Yes", "No");

        if (!confirmBuff) return;

        foreach (var filePath in orphanBuffFiles)
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
        
        
        if (string.IsNullOrEmpty(CardTargetDirectory))
        {
            EditorUtility.DisplayDialog("Error", "Target directory is not set.", "OK");
            return;
        }

        fullPath = Path.Combine(Application.dataPath, CardTargetDirectory.Replace("Assets/", ""));

        if (!Directory.Exists(fullPath))
        {
            EditorUtility.DisplayDialog("Info", "Target directory does not exist.", "OK");
            return;
        }

        Debug.Log($"Deleting orphan scripts: {fullPath}");
        string[] allCardScripts = Directory.GetFiles(fullPath, "*.cs", SearchOption.TopDirectoryOnly);
        Debug.Log($"allScripts: {allCardScripts.Length}");
        List<string> orphanCardFiles = new List<string>();

        foreach (var scriptPath in allCardScripts)
        {
            string fileName = Path.GetFileNameWithoutExtension(scriptPath);
            foreach (var buff in CardDataList)
            {
                if (buff.Name != fileName)
                {
                    orphanCardFiles.Add(scriptPath);
                }
            }
        }

        if (orphanCardFiles.Count == 0)
        {
            EditorUtility.DisplayDialog("Info", "No orphan scripts found.", "OK");
            return;
        }

        bool confirmCard = EditorUtility.DisplayDialog("Confirm Delete",
            $"Found {orphanCardFiles.Count} orphan scripts:\n{string.Join("\n", orphanCardFiles)}\nDelete them?",
            "Yes", "No");

        if (!confirmCard) return;

        foreach (var filePath in orphanCardFiles)
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
        EditorUtility.DisplayDialog("Success", $"Deleted {orphanCardFiles.Count + orphanBuffFiles.Count} orphan scripts.", "OK");
    }

    private void LoadConfig()
    {
        BuffDataList = new List<BuffData>();
        CardDataList = new List<CardData>();
        foreach (KeyValuePair<string, string> kv in GameConfigFiles.GameConfigFilesDic)
        {
            LoadCSV(kv.Key, kv.Value);
        }
    }


    // 加载 CSV 的通用方法
    public void LoadCSV(string key, string fileName)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "Config_csv", fileName);

#if UNITY_ANDROID && !UNITY_EDITOR
        // Android 平台使用 UnityWebRequest
        using (UnityWebRequest request = UnityWebRequest.Get(filePath))
        {
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.Success)
            {
                ParseCSV(key, request.downloadHandler.text);
            }
            else
            {
                Debug.LogError($"CSV 加载失败: {request.error}");
            }
        }
#else
        // 其他平台直接读取文件
        if (File.Exists(filePath))
        {
            string csvText = File.ReadAllText(filePath, System.Text.Encoding.UTF8);
            ParseCsv(key, csvText);
        }
        else
        {
            Debug.LogError($"CSV 文件不存在: {filePath}");
        }

#endif
    }

    // 解析 CSV 内容
    private void ParseCsv(string key, string csvText)
    {
        switch (key)
        {
            case "cfg_Card":
                LoadCardConfig(csvText);
                break;
            case "cfg_Buff":
                LoadBuffConfig(csvText);
                break;
        }
    }

    private void LoadCardConfig(string csvText)
    {
        // 分割输入为行，并移除空行
        var lines = csvText.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

        for (int i = 1; i < lines.Length; i++)
        {
            var line = lines[i];
            var trimmedLine = line.Trim();
            if (string.IsNullOrEmpty(trimmedLine))
                continue;

            var fields = ParseLine(trimmedLine)
                .Where(f => !string.IsNullOrEmpty(f))
                .Take(4)
                .ToList();

            if (fields.Count != 4)
                continue; // 跳过无效行

            var card = new CardData()
            {
                Name = fields[0],
                Cost = int.Parse(fields[1]),
                ExecutionSeq = fields[2].Split(',').Select(int.Parse).ToList(),
                ValueList = fields[3].Split(',').Select(int.Parse).ToList()
            };

            CardDataList.Add(card);
        }
    }

    private static List<string> ParseLine(string line)
    {
        List<string> fields = new List<string>();
        StringBuilder currentField = new StringBuilder();
        bool inQuotes = false;

        foreach (char c in line)
        {
            if (c == '"')
            {
                inQuotes = !inQuotes;
            }
            else if (c == ',' && !inQuotes)
            {
                fields.Add(currentField.ToString());
                currentField.Clear();
            }
            else
            {
                currentField.Append(c);
            }
        }

        // 添加最后一个字段
        fields.Add(currentField.ToString());

        return fields;
    }

    private void LoadBuffConfig(string csvText)
    {
        string[] lines = csvText.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < lines.Length; i++)
        {
            Debug.Log(lines[i]);
        }

        for (int i = 1; i < lines.Length; i++)
        {
            var values = lines[i].Split(',');
            BuffDataList.Add(new BuffData()
            {
                Id = int.Parse(values[0]),
                Name = values[1],
                Type = (E_BuffType)int.Parse(values[2]),
                Mechanism = (E_MechanismType)int.Parse(values[3]),
            });
        }
    }
}