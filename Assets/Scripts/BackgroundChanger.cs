using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BackgroundChanger : MonoBehaviour
{
    public Image backgroundImage;
    public float changeInterval = 20f;
    public Sprite[] backgrounds;

    private void Start()
    {
        Time.timeScale = 1;

        InvokeRepeating("ChangeBackground", 0f, changeInterval);
    }

    private void ChangeBackground()
    {
        int randomIndex = Random.Range(0, backgrounds.Length);
        backgroundImage.sprite = backgrounds[randomIndex];

        if (backgroundImage.sprite != null)
        {
            float worldScreenHeight = Camera.main.orthographicSize * 2f;
            float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

            backgroundImage.rectTransform.sizeDelta = new Vector2(worldScreenWidth, worldScreenHeight);
        }
    }

    public void quitGameButton()
    {
        Application.Quit();
    }

    public void playButton()
    {
        SceneManager.LoadScene(1);
    }
}
