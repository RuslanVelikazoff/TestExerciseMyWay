using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] 
    private Slider loadingSlider;

    private bool startGame = false;

    private float timeLeft;
    private float loadingTime = 3;

    [SerializeField] 
    private GameScreen gameScreen;

    private void Update()
    {
        if (timeLeft < loadingTime)
        {
            timeLeft += Time.deltaTime;
            loadingSlider.value = timeLeft;
        }
        else
        {
            if (startGame)
            {
                this.gameObject.SetActive(false);
                gameScreen.gameObject.SetActive(true);
            }
        }
        
    }

    public void StartGame()
    {
        startGame = true;
    }
}
