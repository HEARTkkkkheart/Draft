using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UObject = UnityEngine.Object;

public class GameLoadConfig : MonoBehaviour
{
    // public string filePath = "Assets/";

    private Dictionary<string, List<List<string>>> _tConfigDic = new Dictionary<string, List<List<string>>>();

    private List<CardData> _cardDatas = new List<CardData>();

    public Dictionary<string, List<List<string>>> GetConfigDic()
    {
        return _tConfigDic;
    }

    private void Start()
    {
        LoadConfig();
    }

    private void LogConfig()
    {
        foreach (KeyValuePair<string, string> kv in GameConfigFiles.GameConfigFilesDic)
        {
            Debug.Log($"Game Config File: {kv.Key} - {kv.Value}");
            for (int i = 0; i < _tConfigDic[kv.Key].Count; i++)
            {
                for (int j = 0; j < _tConfigDic[kv.Key][i].Count; j++)
                {
                    Debug.Log($"tConfigDic[kv.Value][i][j] : {_tConfigDic[kv.Key][i][j]}");
                }
            }
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
        if (kv.Key == "cfg_Card")
        {
            yield return ConfigLoader.LoadCsv<CardData>(kv.Value, (csvData) =>
            {
                if (csvData == null || csvData.Count == 0)
                {
                    Debug.LogError("配置表加载失败");
                    return;
                }

                _cardDatas = new List<CardData>(csvData);
                // LogConfig();
            });
        }
    }
}