using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    private GameData gameData;
    private List<ISaveManager> saveManagers = new List<ISaveManager>();
    private FileDataHandle dataHandle;

    [SerializeField] private string fileName;
    private void Awake()
    {
        if(instance != null)
        {
            Destroy(instance.gameObject);
        }
        else
        {
            instance = this;
        }
    }
    private void Start()
    {
        dataHandle = new FileDataHandle(Application.persistentDataPath,fileName);
        IEnumerable<ISaveManager> allSaveManager = FindObjectsOfType<MonoBehaviour>().OfType<ISaveManager>();
        saveManagers =new List<ISaveManager>(allSaveManager);
        LoadGame();
        
    }

    public void NewGame()
    {
        gameData = new GameData();
    }

    public void LoadGame()
    {
        gameData = dataHandle.Load();
        if(gameData == null)
        {
            Debug.LogWarning("game data not found ");
            NewGame();
        }

        foreach(ISaveManager saveManager in saveManagers)
        {
            saveManager.LoadData(gameData);
        }
        Debug.Log("game was load " + gameData.coin);

    }

    public void SaveGame() {
        foreach (ISaveManager saveManager in saveManagers)
        {
            saveManager.SaveData(ref gameData);
        }
        dataHandle.Save(gameData);
        Debug.Log("game was saved: "+ gameData.coin+" coin");
    }

    public void SaveComponent(ISaveManager iSave)
    {
        iSave.SaveData(ref gameData);
        dataHandle.Save(gameData);
    }

    public void LoadComponent(ISaveManager iLoad)
    {
        //dataHandle.Load();
        iLoad.LoadData(gameData);
    }
    private void OnApplicationQuit()
    {
        SaveGame();
    }
}
