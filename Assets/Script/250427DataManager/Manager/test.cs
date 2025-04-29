using UnityEngine;
using System.Collections.Generic;

public class test : DataMgrFunction
{
	private DataClass _data;

	void Start()
	{
		// 在这里编写Start方法的代码
	}

	void Update()
	{
		// 在这里编写Update方法的代码
	}

	public override void InitListener()
	{

	}

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
		// 在这里编写SaveData方法的代码
	}

	public override void InitData()
	{
		// 在这里编写InitData方法的代码
	}
}
