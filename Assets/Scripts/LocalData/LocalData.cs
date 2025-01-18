using UnityEngine;

//Класс для работы с локальными сохранениями
public class LocalData : MonoBehaviour
{
    public static LocalData Instance { get; private set; }

    private const string SaveKey = "MainLocalSave";

    private string welcomeText;
    private int startingNumber;

    private void Awake()
    {
        Instance = this;
        Load();
    }

    private void OnApplicationQuit()
    {
        Save();
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            Save();
        }
    }

    private void OnDisable()
    {
        Save();
    }

    private void Load()
    {
        var data = SaveManager.Load<GameData>(SaveKey);

        this.welcomeText = data.welcomeText;
        this.startingNumber = data.startingNumber;
    }

    private void Save()
    {
        SaveManager.Save(SaveKey,GetSaveSnapshot());
        PlayerPrefs.Save();
    }

    private GameData GetSaveSnapshot()
    {
        var data = new GameData()
        {
            welcomeText = this.welcomeText,
            startingNumber = this.startingNumber
        };

        return data;
    }

    public string GetWelcomeText()
    {
        return welcomeText;
    }

    public void SetWelcomeText(string text)
    {
        welcomeText = text;
        Save();
    }

    public int GetStartingNumber()
    {
        return startingNumber;
    }

    public void SetStartingNumber(int value)
    {
        startingNumber = value;
        Save();
    }

    public void PlusStartingNumber()
    {
        startingNumber++;
        Save();
    }
}
