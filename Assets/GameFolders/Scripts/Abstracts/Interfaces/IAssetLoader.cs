using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IAssetLoader 
{
    event Action OnAssetsLoaded;
    void LoadAssets();
    void ReleaseMemory();
}
