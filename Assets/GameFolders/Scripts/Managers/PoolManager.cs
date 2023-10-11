using UnityEngine;
using System.Collections.Generic;
using System;

public class PoolManager : Singleton<PoolManager>
{
    public Action OnPrefabsLoaded;

    [SerializeField] private Pools[] _shapePools;
    [SerializeField] private byte _amountPerShape = 25;
    private Vector2 _spawnPoint = new Vector2(-20, -100);
    [SerializeField] private List<Pools> _poolsList;

    private IAssetLoader _shapeLoader;
    private Transform _shapesParent;

    protected override void Awake()
    {
        base.Awake();
        _shapeLoader = new ShapeLoader(this);
        _shapeLoader.OnAssetsLoaded += SpawnShapesPoolObjects;
        SetContainers();
    }

    public Shape RequestShape(int shapeID, Vector2 requestPos)
    {
        if (shapeID < 0 || shapeID > _poolsList.Count)
            return null;


        foreach (var shape in _poolsList[shapeID].pooledObjects)
        {
            if (!shape.gameObject.activeInHierarchy) //
            {
                shape.transform.position = requestPos;
                shape.gameObject.SetActive(true);
                shape.Initialize();
                return shape;
            }
        }
        return RequestNewShape(shapeID, requestPos);
    }

    public void AddPool(Pools poolToAdd)
    {
        if (_poolsList.Contains(poolToAdd))
            return;
        _poolsList.Add(poolToAdd);
    }

    private void SpawnShapesPoolObjects()
    {
        for (int i = 0; i < _amountPerShape; i++)
        {
            for (int j = 0; j < _poolsList.Count; j++)
            {
                Shape spawnedShape = Instantiate(_poolsList[j].objPrefab, _spawnPoint, Quaternion.identity, _shapesParent);
                _poolsList[j].pooledObjects.Add(spawnedShape);
                spawnedShape.gameObject.SetActive(false);
            }
        }
        OnPrefabsLoaded?.Invoke();
    }

    private void SetContainers()
    {
        _shapesParent = transform.GetChild(0);
        if (_shapesParent == null)
            Debug.Log("The shapes parent is null");
    }

    private Shape RequestNewShape(int id, Vector2 posToGo)
    {
        Shape spawnedNewShape = Instantiate(_poolsList[id].objPrefab, posToGo, Quaternion.identity, _shapesParent);
        _poolsList[id].pooledObjects.Add(spawnedNewShape);
        return spawnedNewShape;
    }

    private void OnDisable()
    {
        _shapeLoader.ReleaseMemory();
    }
}


