using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System;

public class BackgroundLoader 
{
    public Action<BackgroundAdjuster> OnPrefabLoaded;

    public async Task LoadBackgroundTile()
    {
        AsyncOperationHandle<GameObject> backgrounloadingOp = Addressables.LoadAssetAsync<GameObject>("Prefabs/BackgroundTiles");
        Task loadingTask = backgrounloadingOp.Task;
        backgrounloadingOp.Completed += OnBgTilePrefabLoaded;
        await loadingTask;

    }

    private void OnBgTilePrefabLoaded(AsyncOperationHandle<GameObject> obj)
    {
        BackgroundAdjuster bgAdjuster = obj.Result.GetComponent<BackgroundAdjuster>();
        if(bgAdjuster != null)
            OnPrefabLoaded?.Invoke(bgAdjuster);
    }
}
