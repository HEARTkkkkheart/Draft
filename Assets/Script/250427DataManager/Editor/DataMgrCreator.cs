using System;
using UnityEngine;
using UnityEditor;
using System.IO;

public class DataMgrCreator : EditorWindow
{
    
    private string _folderPath = "Assets/Script/Manager";
    private string _scriptName;
    
    
    [MenuItem("Tools/DataManager Creator")]
    public static void ShowWindow()
    {
        GetWindow(typeof(DataMgrCreator));
    }

    private void OnGUI()
    {
        EditorGUILayout.HelpBox("操作顺序：\n1.输入名称(请输入合法的文件名)\n2.点击New a DataMgr Script按钮", MessageType.Info);

        GUILayout.Label("Select Folder", EditorStyles.boldLabel);
        if (GUILayout.Button("Select Folder"))
        {
            _folderPath = EditorUtility.OpenFolderPanel("Select Folder", _folderPath, "");
            if (_folderPath.Contains(Application.dataPath))
            {
                _folderPath = "Assets" + _folderPath.Replace(Application.dataPath, "");
            }
        }
        GUILayout.Label("Folder Path: " + _folderPath);
        
        
        GUILayout.Label("Add Data Type", EditorStyles.boldLabel);
        EditorGUILayout.BeginVertical("box");
        _scriptName = EditorGUILayout.TextField($"Name: ", _scriptName);
        EditorGUILayout.EndVertical();
        
        if (GUILayout.Button("New a DataMgr Script"))
        {
            CreateScript();
            _scriptName = string.Empty;
            GUI.FocusControl(null); // 清除焦点，以便文本字段更新
        }
    }
    
    private void CreateScript()
    {
        // 如果用户取消保存文件，则返回
        if (_scriptName == "")
        {
            Debug.Log("请输入合法的文件名");
            return;
        }
        if (!Char.IsLetter(_scriptName[0]))
        {
            Debug.Log("首字母必须是字母");
            return;
        }
        
        // 创建脚本文件的路径
        string path = _folderPath + "/" + _scriptName + ".cs";
        
        // 创建新的C#脚本文件并写入基本内容
        using (StreamWriter writer = new StreamWriter(path))
        {
            writer.WriteLine("using UnityEngine;");
            writer.WriteLine("using System.Collections.Generic;");
            writer.WriteLine("");
            writer.WriteLine("public class " + _scriptName + " : DataMgrFunction");
            writer.WriteLine("{");
            writer.WriteLine("\tprivate DataClass _data;");
            writer.WriteLine("");
            writer.WriteLine("\tvoid Start()");
            writer.WriteLine("\t{");
            writer.WriteLine("\t\t// 在这里编写Start方法的代码");
            writer.WriteLine("\t}");
            writer.WriteLine("");
            writer.WriteLine("\tvoid Update()");
            writer.WriteLine("\t{");
            writer.WriteLine("\t\t// 在这里编写Update方法的代码");
            writer.WriteLine("\t}");
            writer.WriteLine("");
            writer.WriteLine("\tpublic override void InitListener()");
            writer.WriteLine("\t{");
            writer.WriteLine("");
            writer.WriteLine("\t}");
            writer.WriteLine("");
            writer.WriteLine("\tpublic override DataClass ReadDataByName()");
            writer.WriteLine("\t{");
            writer.WriteLine("\t\treturn new DataClass();");
            writer.WriteLine("\t}");
            writer.WriteLine("");
            writer.WriteLine("\tpublic override Dictionary<string, DataClass> ReadDataByType()");
            writer.WriteLine("\t{");
            writer.WriteLine("\t\treturn new Dictionary<string, DataClass>();");
            writer.WriteLine("\t}");
            writer.WriteLine("");
            writer.WriteLine("\tpublic override void SaveData()");
            writer.WriteLine("\t{");
            writer.WriteLine("\t\t// 在这里编写SaveData方法的代码");
            writer.WriteLine("\t}");
            writer.WriteLine("");
            writer.WriteLine("\tpublic override void InitData()");
            writer.WriteLine("\t{");
            writer.WriteLine("\t\t// 在这里编写InitData方法的代码");
            writer.WriteLine("\t}");
            writer.WriteLine("}");
        }
        
        // 刷新Unity资源
        AssetDatabase.Refresh();
    }
}
