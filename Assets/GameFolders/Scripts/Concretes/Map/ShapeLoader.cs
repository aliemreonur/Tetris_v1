using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System;

public class ShapeLoader : IAssetLoader
{
    #region Fields&Properties
    public event Action OnAssetsLoaded;
    private AsyncOperationHandle<IList<GameObject>> _loadAssetsOperation;
    private const string ASSET_KEY = "Shapes";
    private PoolManager _poolManager;
    #endregion

    #region Public Methods&Const
    public ShapeLoader(PoolManager poolManager)
    {
        _poolManager = poolManager;
        LoadAssets();
        _loadAssetsOperation.Completed += (ctx) => { OnAssetsLoaded?.Invoke(); };
    }

    public void LoadAssets()
    {
        _loadAssetsOperation = Addressables.LoadAssetsAsync<GameObject>(ASSET_KEY, obj =>
        {
            Shape loadedShape = obj.GetComponent<Shape>();
            if (loadedShape == null)
                Debug.LogError("The addressable object could not gather the shape cs");
            AddNewShapeToPools(loadedShape);
            loadedShape.gameObject.SetActive(false);
        });
    }

    public void ReleaseMemory()
    {
        Addressables.Release(_loadAssetsOperation);
    }

    public void AddNewShapeToPools(Shape loadedShape) //dont like the protection level
    {
        Pools newPool = new Pools();
        newPool.objPrefab = loadedShape;
        newPool.pooledObjects = new List<Shape>();
        _poolManager.AddPool(newPool);
    }
    #endregion
}
