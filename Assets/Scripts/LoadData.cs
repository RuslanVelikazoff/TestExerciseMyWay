using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class WelcomeData
{
    public string welcomeText;
}

public class SettingsData
{
    public int startingNumber;
}

public class LoadData : MonoBehaviour
{
    private string serverURL = "";

    [SerializeField] 
    private string bundleNameInServer;
    [SerializeField] 
    private string welcomeJSONNameInServer;
    [SerializeField] 
    private string settingsJSONNameInServer;

    private AssetBundle spriteBundle;

    private IEnumerator Start()
    {
        Coroutine getBundleSprite = StartCoroutine(GetBundleSpriteFromServer());

        yield return getBundleSprite;

        Coroutine getWelcomeText = StartCoroutine(GetSettingsFromJSONFileInServer());

        yield return getWelcomeText;

        if (LocalData.Instance.GetStartingNumber() == 0)
        {
            Coroutine getStartingNumber = StartCoroutine(GetSettingsFromJSONFileInServer());

            yield return getStartingNumber;
            
        }
    }

    #region GetBundleSpriteFromServer
    
    IEnumerator GetBundleSpriteFromServer()
    {
        UnityWebRequest request = UnityWebRequest.Get(serverURL + "/" + bundleNameInServer);

        yield return request.SendWebRequest();
        
        if (request.result == UnityWebRequest.Result.ConnectionError 
            || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError($"Ошибка: {request.error}");
        }
        else
        {
            var assetBundle = DownloadHandlerAssetBundle.GetContent(request);

            yield return assetBundle;

            StartCoroutine(LoadAssetFromBundle("ButtonSprite"));
        }
    }

    private IEnumerator LoadAssetFromBundle(string assetName)
    {
        if (spriteBundle == null)
        {
            Debug.LogError("Asset Bundle не выгружен");
            yield break;
        }

        Sprite buttonSprite = spriteBundle.LoadAsset<Sprite>(assetName);

        yield return buttonSprite;

        if (buttonSprite != null)
        {
            Debug.Log("Спрайт успешно выгружен");
            //Накинуть спрайт на кнопку
        }
        else
        {
            Debug.LogError("Ошибка выгрузки спрайта");
            yield break;
        }
    }
    
    #endregion

    #region GetWelcomeTextFromServer

    IEnumerator GetWelcomeTextFromJSONFileInServer()
    {
        UnityWebRequest request = UnityWebRequest.Get(serverURL + "/" + welcomeJSONNameInServer);

        yield return request.SendWebRequest();
        
        if (request.result == UnityWebRequest.Result.ConnectionError 
            || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError($"Ошибка: {request.error}");
        }
        else
        {
            string jsonResponse = request.downloadHandler.text;
            
            ParseWelcomeTextJSON(jsonResponse);
        }
    }

    private void ParseWelcomeTextJSON(string json)
    {
        WelcomeData welcomeData = JsonUtility.FromJson<WelcomeData>(json);
        
        LocalData.Instance.SetWelcomeText(welcomeData.welcomeText);
        
        //SetWelcomeText
    }

    #endregion

    #region GetSettingsDataFromServer

    IEnumerator GetSettingsFromJSONFileInServer()
    {
        UnityWebRequest request = UnityWebRequest.Get(serverURL + "/" + settingsJSONNameInServer);

        yield return request.SendWebRequest();
        
        if (request.result == UnityWebRequest.Result.ConnectionError 
            || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError($"Ошибка: {request.error}");
        }
        else
        {
            string jsonResponse = request.downloadHandler.text;
            
            ParseSettingsJSON(jsonResponse);
        }
    }

    private void ParseSettingsJSON(string json)
    {
        SettingsData settingsData = JsonUtility.FromJson<SettingsData>(json);

        LocalData.Instance.SetStartingNumber(settingsData.startingNumber);
        
        //Set staring number
    }

    #endregion
}
