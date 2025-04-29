using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Config/DataConfig")]
public class DataConfig : ScriptableObject
{
    [SerializeField]
    public List<string> DataName;
}
