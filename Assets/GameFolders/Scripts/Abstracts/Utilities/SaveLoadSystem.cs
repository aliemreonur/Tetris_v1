using UnityEngine;
using System.IO;

public static class SaveLoadSystem 
{
    private readonly static string PLAYER_DATA = "AAplayerData.json";

    public static void SaveData(PlayerData playerDataToSave)
    {
        string jsonPlayerData = JsonUtility.ToJson(playerDataToSave);
        string savePath = Application.persistentDataPath + Path.DirectorySeparatorChar + PLAYER_DATA;
        File.WriteAllText(savePath, jsonPlayerData);
        //Debug.Log("Game Saved to :" + savePath);
    }


    public static PlayerData LoadData()
    {
        string loadPath = Application.persistentDataPath + Path.DirectorySeparatorChar + PLAYER_DATA;

        if (File.Exists(loadPath))
        {
            string playerDataString = File.ReadAllText(loadPath);
            PlayerData playerData = JsonUtility.FromJson<PlayerData>(playerDataString);
            return playerData;
        }
        else
        {
            Debug.Log("No saved data found");
            return new PlayerData();
        }
        
    }
    

}
