using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class BackgroundAdjuster : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    #region MonoMethods
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        if (_spriteRenderer == null)
            Debug.LogError("The background adjuster could not gather its sprite renderer");
    }

    private void OnEnable()
    {
        Board.Instance.OnMapSet += AdjustBackground;
    }

    private void OnDisable()
    {
        Board.Instance.OnMapSet -= AdjustBackground;
    }
    #endregion

    #region Public Methods

    public void ChangeSprite(Sprite newSprite)
    {
        _spriteRenderer.sprite = newSprite;
    }

    #endregion

    #region Private Methods
    private void AdjustBackground(byte x, byte y)
    {
        _spriteRenderer.size = new Vector2(x, y);
        transform.position = GetSpritePosition(x, y);
    }

    private Vector2 GetSpritePosition(byte x, byte y)
    {
        return new Vector2((x / 2) - 0.5f, (y / 2) - 0.5f);
    }
    #endregion
}
