using System;

//Класс для хранения локальных сохранения и задавания им значений при первом запуске игры
public class GameData
{
    public string welcomeText;
    public int startingNumber;

    public GameData()
    {
        welcomeText = String.Empty;
        startingNumber = 0;
    }
}
