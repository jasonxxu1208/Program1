using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class ObstacleData
{
    public float x;
    public float y;
    public float size;
}

[System.Serializable]
public class SaveData
{
    public float playerX;
    public float playerY;

    public List<ObstacleData> obstacles = new List<ObstacleData>();
}

public static class SaveSystem
{
    private static string savePath = Application.persistentDataPath + "/save.json";

    public static void SaveGame(Transform player)
    {
        SaveData data = new SaveData();
        data.playerX = player.position.x;
        data.playerY = player.position.y;

        GameObject[] obstacleObjects = GameObject.FindGameObjectsWithTag("Obstacle");
        foreach (GameObject obj in obstacleObjects)
        {
            ObstacleData o = new ObstacleData();
            o.x = obj.transform.position.x;
            o.y = obj.transform.position.y;
            o.size = obj.transform.localScale.x; // assuming uniform scale
            data.obstacles.Add(o);
        }

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);

        Debug.Log($"Game Saved! Path: {savePath}");
    }

    public static SaveData LoadGame()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            Debug.Log("Game Loaded!");
            return data;
        }
        else
        {
            Debug.LogWarning("No save file found!");
            return null;
        }
    }
}
