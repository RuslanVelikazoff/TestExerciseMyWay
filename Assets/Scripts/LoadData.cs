using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LoadData : MonoBehaviour
{
    private string serverURL = "";

    [SerializeField] 
    private string stringBundleNameInServer;

    private AssetBundle spriteBundle;

    IEnumerator GetDataFromServer()
    {
        UnityWebRequest request = UnityWebRequest.Get(serverURL + "/" + stringBundleNameInServer);

        yield return request.SendWebRequest();
        
        if (request.result == UnityWebRequest.Result.ConnectionError 
            || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError($"Ошибка: {request.error}");
        }
        else
        {
            //Asset bundle
            var assetBundle = DownloadHandlerAssetBundle.GetContent(request);

            yield return assetBundle;

            StartCoroutine(LoadAssetFromBundle("ButtonSprite"));
            
            //JSON welcome text
            
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
}
