using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Text;

public class GameLoadConfig : MonoBehaviour
{
    public List<BuffData> BuffDataList = new List<BuffData>();

    public List<CardData> CardDataList = new List<CardData>();

    private void Start()
    {
        LoadConfig();
    }

    private void LogConfig()
    {
        for (int i = 0; i < BuffDataList.Count; i++)
        {
            var buffData = BuffDataList[i];
            Debug.Log($"buffData[{i}] = {buffData.Id}, {buffData.Name}, {buffData.Type}, {buffData.TriggerTiming}");
        }
    }

    private void LoadConfig()
    {
        foreach (KeyValuePair<string, string> kv in GameConfigFiles.GameConfigFilesDic)
        {
            StartCoroutine(LoadCSVConfig(kv));
        }
    }

    private IEnumerator LoadCSVConfig(KeyValuePair<string, string> kv)
    {
        yield return LoadCSV(kv.Key, kv.Value);
    }


    // 加载 CSV 的通用方法
    public IEnumerator LoadCSV(string key, string fileName)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "Config_csv", fileName);
        Debug.Log($"filePath : {filePath}");

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

        yield return null; // 保持协程结构
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
                TriggerTiming = (E_TriggerTiming)int.Parse(values[3]),
            });
        }
        
    }
}