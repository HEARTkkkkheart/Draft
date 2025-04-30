using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class ConfigLoader
{
    // 加载 CSV 的通用方法
    public IEnumerator LoadCsv<T>(string fileName, Action<List<T>> onComplete) where T : new()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "Config_csv", fileName);
        List<T> csvData = new List<T>();

#if UNITY_ANDROID && !UNITY_EDITOR
        // Android 平台使用 UnityWebRequest
        using (UnityWebRequest request = UnityWebRequest.Get(filePath))
        {
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.Success)
            {
                ParseCSV(request.downloadHandler.text, csvData);
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
            ParseCsv(csvText, csvData);
        }
        else
        {
            Debug.LogError($"CSV 文件不存在: {filePath}");
        }

        yield return null; // 保持协程结构
#endif

        onComplete?.Invoke(csvData);
    }

    // 解析 CSV 内容
    private void ParseCsv<T>(string csvText, List<T> output) where T : new()
    {
        // string[] lines = csvText.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
        // Debug.Log("-----------------------------");
        // for (int i = 0; i < lines.Length; i++)
        // {
        //     Debug.Log(lines[i]);
        // }
        //
        // for (int i = 1; i < lines.Length; i++)
        // {
        //     output.Add(new List<string>());
        //     var values = lines[i].Split(',');
        //     for (int j = 0; j < values.Length; j++)
        //     {
        //         output[i - 1].Add(values[j]);
        //     }
        // }

        if (typeof(T) == typeof(CardData))
        {
            string[] lines = csvText.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length < 2) return;
            for ()
            output.Add(ParseCardData(csvText));
        }
    }

    public CardData ParseCardData(string input)
    {
        // 移除末尾的逗号并分割字段
        var fields = SplitCsv(input.TrimEnd(',')).ToList();
        if (fields.Count != 4)
            throw new FormatException("输入格式不正确，应为4个字段。");

        return new CardData
        {
            Name = fields[0],
            Cost = int.Parse(fields[1]),
            ExecutionSeq = fields[2].Split(',').Select(int.Parse).ToList(),
            ValueList = fields[3].Split(',').Select(int.Parse).ToList()
        };
    }

    private IEnumerable<string> SplitCsv(string input)
    {
        bool inQuotes = false;
        StringBuilder current = new StringBuilder();

        foreach (char c in input)
        {
            if (c == '"')
            {
                inQuotes = !inQuotes;
            }
            else if (c == ',' && !inQuotes)
            {
                yield return current.ToString();
                current.Clear();
            }
            else
            {
                current.Append(c);
            }
        }

        yield return current.ToString();
    }
}