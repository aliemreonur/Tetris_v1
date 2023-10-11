using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class ThemeChanger
{
    public int SpriteIndex { get; private set; }
    public Sprite bgSprite;

    private AsyncOperationHandle<Sprite> _spriteLoadingOp;
    private Image _themeImage;
    private const string THEME = "Theme";

    public ThemeChanger(Image themeImg)
    {
        SpriteIndex = PlayerPrefs.HasKey(THEME) ? PlayerPrefs.GetInt(THEME) : 1;
        LoadSpriteWithIndex(SpriteIndex);
        _themeImage = themeImg;
    }

    public void LoadSpriteWithIndex(int index)
    {
        SpriteIndex = index;
        _spriteLoadingOp = Addressables.LoadAssetAsync<Sprite>($"Sprites/Bg{index}");
        _spriteLoadingOp.Completed += OnNewSpriteLoaded;
    }   

    private void OnNewSpriteLoaded(AsyncOperationHandle<Sprite> obj)
    {
        bgSprite = obj.Result;
        if (bgSprite == null)
            return;

        _themeImage.sprite = bgSprite;
        Board.Instance.ThemeChanged(bgSprite);
        PlayerPrefs.SetInt(THEME, SpriteIndex); //save from the settings
    }
}
