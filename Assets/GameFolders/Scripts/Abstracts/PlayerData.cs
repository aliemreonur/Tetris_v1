using UnityEngine;

[System.Serializable]
public class PlayerData
{
    //public byte LevelID { get; private set; }
    //public Texture2D backgroundTexture;

    public byte LevelID = 1;
    public int PlayerScore = 0;

    public void SetPlayerData(byte currentLevel, int playerTotalScore)
    {
        LevelID = currentLevel;
        PlayerScore = playerTotalScore;
        SaveLoadSystem.SaveData(this);
    }
}
