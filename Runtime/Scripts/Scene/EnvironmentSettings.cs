using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentSetting : Entity
{
    public EnvironmentSetting() { }

    public int DynamicEnvironmentType { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string ParamName1 { get; set; }
    public string ParamName2 { get; set; }
    public string ParamType1 { get; set; }
    public string ParamType2 { get; set; }
    public string ParamValue1 { get; set; }
    public string ParamValue2 { get; set; }
}