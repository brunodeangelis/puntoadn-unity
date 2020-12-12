using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StationData
{
    public string _category;
    public StationType _stationType;
    public List<Project> _projects = new List<Project>();
}
