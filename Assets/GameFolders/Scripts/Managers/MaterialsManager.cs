using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class MaterialsManager : Singleton<MaterialsManager>
{
    public Material FlashingMaterial => _flashingMaterial;
    public Material DefaultMaterial => _defaultMaterial;

    private Material _flashingMaterial;
    private Material _defaultMaterial;

    private AsyncOperationHandle<Material> _loadFlashingMaterial;
    private AsyncOperationHandle<Material> _loadDefaultMaterial;

    protected override void Awake()
    {
        base.Awake();
        LoadMaterials();
    }

    private void LoadMaterials()
    {
        _loadFlashingMaterial = Addressables.LoadAssetAsync<Material>("Materials/White");
        _loadFlashingMaterial.Completed += OnFlashingMaterialLoaded;

        _loadDefaultMaterial = Addressables.LoadAssetAsync<Material>("Materials/GlowMaterial");
        _loadDefaultMaterial.Completed += OnDefaultMaterialLoaded;
    }


    private void OnDefaultMaterialLoaded(AsyncOperationHandle<Material> obj)
    {
       if(_loadDefaultMaterial.Status == AsyncOperationStatus.Succeeded)
        {
            _defaultMaterial = obj.Result;
        }
    }

    private void OnFlashingMaterialLoaded(AsyncOperationHandle<Material> obj)
    {
        if (_loadFlashingMaterial.Status == AsyncOperationStatus.Succeeded)
        {
            _flashingMaterial = obj.Result;
        }
    }
}
