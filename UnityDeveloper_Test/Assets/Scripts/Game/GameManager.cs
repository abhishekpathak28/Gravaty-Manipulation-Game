using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private int totalCubes;
    private int collectedCubes;
    public bool isGameActive = true;

    [Header("Ui Refernce")]
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject winScreen;
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
        if (winScreen != null) winScreen.SetActive(false);
        if (gameOverScreen != null) gameOverScreen.SetActive(false);
    }

    void Update()
    {

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
}

