using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveLoader 
{
    public bool HasGameSaved();
    public void SaveGame();
    public void LoadGame();

}
