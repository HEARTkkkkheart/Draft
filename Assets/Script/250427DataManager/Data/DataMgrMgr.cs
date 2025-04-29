using System;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_DataType
{
    None,
    Dic,
    String,
}

public class DataClass
{
    public E_DataType myDataType;
    //存在这里的都是json的string
    public string data;

    public DataClass(E_DataType myDataType, string data)
    {
        this.myDataType = myDataType;
        this.data = data;
    }
    
    public DataClass(DataClass dataClass)
    {
        myDataType = dataClass.myDataType;
        data = dataClass. data;
    }

    public DataClass()
    {
        myDataType = E_DataType.None;
        data = String.Empty;
    }
}

public class DataMgrMgr : MonoBehaviour
{

    private Dictionary<string, Dictionary<string, DataClass>> _data = new Dictionary<string, Dictionary<string, DataClass>>();
    private string _dataFilePath = "";

    private void Start()
    {
        InitDataStruct();
        ReadLocalJson();
    }

    private void ReadLocalJson()
    {
        
    }

    private void WriteLocalJson()
    {
        
    }

    private void InitDataStruct()
    {
        var list = Resources.Load<DataConfig>("DataConfig");
        for (int i = 0; i < list.DataName.Count; i++)
        {
            var tKey = list.DataName[i];
            var tValue = new Dictionary<string, DataClass>();
            
            _data.Add(tKey, tValue);
        }
    }

    //希望提供一个可供外部查询的方法
    //返回一个字典，对这个字典的修改不会影响原有的数据
    private Dictionary<string, DataClass> ReadDataByType(string type)
    {
        return new Dictionary<string, DataClass>(_data[type]);
    }

    
    //希望提供一个可供外部保存数据的方法
    //外部修改传入的引用值时，不会对字典内数据有影响
    private void SaveDataByType(string type, Dictionary<string, DataClass> data)
    {
        _data[type] = new Dictionary<string, DataClass>(data);
    }

    private void SaveDataByTypeAndName(string type, string name, DataClass data)
    {
        if (_data.TryGetValue(type, out var value))
        {
            _data[type][name] = new DataClass(data);
        }
    }
}



























