using UnityEngine;
using System.Threading.Tasks;

public class PieceFlasher : IFlasher
{
    private SpriteRenderer _renderer;

    public PieceFlasher(SpriteRenderer renderer)
    {
        _renderer = renderer;
    }

    public async Task StartFlashing()
    {
        for (int i = 0; i < 2; i++)
        {
            await Awaitable.WaitForSecondsAsync(0.25f);
            _renderer.material = MaterialsManager.Instance.FlashingMaterial;
            await Awaitable.WaitForSecondsAsync(0.25f);
            _renderer.material = MaterialsManager.Instance.DefaultMaterial;
        }
    }

    public void SetMaterial()
    {
        _renderer.material = MaterialsManager.Instance.DefaultMaterial;
    }
}
