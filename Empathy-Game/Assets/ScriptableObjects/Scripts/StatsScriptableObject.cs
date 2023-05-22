using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ CreateAssetMenuAttribute(fileName = "StatsScriptableObject", menuName = "ScriptableObject/StateObject")]
public class StatsScriptableObject : ScriptableObject
{
    public int PersonalPoints;
    public int TeamPoints;
}
