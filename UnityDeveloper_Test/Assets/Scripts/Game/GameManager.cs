using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private int totalCubes;
    private int collectedCubes;
    public bool isGameActive = true;
    private float levelTimeLimit = 120f;
    private float currentTimer;

    [Header("Ui Refernce")]
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject winScreen;
    [SerializeField] private TextMeshProUGUI timerText;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        totalCubes = GameObject.FindGameObjectsWithTag("Collectibles").Length;
        currentTimer = levelTimeLimit;
        if (winScreen != null) winScreen.SetActive(false);
        if (gameOverScreen != null) gameOverScreen.SetActive(false);
    }

    void Update()
    {
        if (!isGameActive) return;
        currentTimer -= Time.deltaTime;

        UpdateTimerUI();
        if (currentTimer <= 0)
        {
            Debug.Log("Time Over");
            GameOver();
        }
    }
    public void OnCubeCollected()
    {
        if (!isGameActive) return;
        collectedCubes++;
        if (collectedCubes >= totalCubes)
        {
            WinGame();
        }
    }
    private void WinGame()
    {
        if (!isGameActive) return;
        isGameActive = false;
        if (winScreen != null) winScreen.SetActive(true);
        Debug.Log("All collectibles collected!");
        Debug.Log("You won the game");
    }
    public void GameOver()
    {
        if (!isGameActive) return;
        isGameActive = false;

        Debug.Log("Game Over!");
        if (gameOverScreen) gameOverScreen.SetActive(true);
    }
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    void UpdateTimerUI()
    {
        if (timerText != null)
        {
            float displayTime = Mathf.Max(0, currentTimer);
            float minutes = Mathf.FloorToInt(displayTime / 60);
            float seconds = Mathf.FloorToInt(displayTime % 60);

            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

            if (currentTimer <= 10) timerText.color = Color.red;
            else timerText.color = Color.white;
        }
    }
}

