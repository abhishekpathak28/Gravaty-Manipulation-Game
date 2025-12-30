using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private int totalCubes;
    private int collectedCubes;

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
    }

    void Update()
    {

    }
    public void OnCubeCollected()
    {
        collectedCubes++;
        if (collectedCubes >= totalCubes)
        {
            WinGame();
        }
    }
    private void WinGame()
    {
        Debug.Log("All collectibles collected!");
        Debug.Log("You won the game");
    }
}

