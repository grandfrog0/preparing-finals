using System.IO;
using UnityEngine;

public class GameSaver : MonoBehaviour
{
    private static string SavePath => Application.persistentDataPath;
    private static string InventorySavePath => Path.Combine(SavePath, "inventory.json");

    public PlayerInventory Inventory;

    private void Awake()
    {
        if (File.Exists(InventorySavePath))
        {
            string json = File.ReadAllText(InventorySavePath);
            JsonUtility.FromJsonOverwrite(json, Inventory);
        }
    }

    private void Start()
    {
        Debug.Log(Inventory.Inventory.Count);
    }

    private void OnApplicationQuit()
    {
        string json = JsonUtility.ToJson(Inventory, prettyPrint: true); 
        Debug.Log(("SAVE", json, string.Join(", ", Inventory.Inventory)));
        File.WriteAllText(InventorySavePath, json);
    }
}
