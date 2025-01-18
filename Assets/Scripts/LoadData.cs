using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Serialization;

public class WelcomeData
{
    public string welcomeText;
}

public class LoadData : MonoBehaviour
{
    private string serverURL = "";

    [SerializeField] 
    private string bundleNameInServer;
    [SerializeField] 
    private string welcomeJSONNameInServer; 

    private AssetBundle spriteBundle;

    private IEnumerator Start()
    {
        Coroutine getBundleSprite = StartCoroutine(GetBundleSpriteFromServer());

        yield return getBundleSprite;
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
    }

    #endregion
}
