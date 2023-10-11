using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level Details", menuName = "Level")]
public class LevelSO : ScriptableObject
{
    public byte levelWidth;
    public byte levelHeight;
    public byte minimumRawToSuccess;

}
