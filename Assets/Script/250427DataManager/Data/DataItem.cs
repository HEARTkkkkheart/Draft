using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DataMgrFunction
{
    public abstract void InitListener();
    public abstract DataClass ReadDataByName();
    public abstract Dictionary<string, DataClass> ReadDataByType();
    public abstract void SaveData();
    public abstract void InitData();
}

public class DataItem : DataMgrFunction
{ 
    private DataClass _data;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void InitListener()
    {
        
    }

    //同类型下属Prop实例化时访问
    public override DataClass ReadDataByName()
    {
        return new DataClass();
    }
    
    public override Dictionary<string, DataClass> ReadDataByType()
    {
        return new Dictionary<string, DataClass>();
    }

    public override void SaveData()
    {
        
    }

    public override void InitData()
    {
        
    }
}