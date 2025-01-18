using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameScreen : MonoBehaviour
{
    [SerializeField] 
    private Button clickButton;
    [SerializeField] 
    private Button updateButton;
    [SerializeField]
    private TextMeshProUGUI numberText;
    [SerializeField] 
    private TextMeshProUGUI welcomeText;

    private void OnEnable()
    {
        numberText.text = LocalData.Instance.GetStartingNumber().ToString();
        welcomeText.text = LocalData.Instance.GetWelcomeText();
        
        ButtonClickAction();
    }

    private void ButtonClickAction()
    {
        if (clickButton != null)
        {
            clickButton.onClick.RemoveAllListeners();
            clickButton.onClick.AddListener(() =>
            {
                LocalData.Instance.PlusStartingNumber();
            });
        }

        if (updateButton != null)
        {
            updateButton.onClick.RemoveAllListeners();
            updateButton.onClick.AddListener(() =>
            {
                //Также можно создать отдельный метод в LoadData и вызвать его, предварительно открыв экран загрузки и закрыв текущий экран
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            });
        }
    }

    public void SetButtonSprite(Sprite buttonSprite)
    {
        clickButton.GetComponent<Image>().sprite = buttonSprite;
    }
}
