using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;


    #region Variables
    private int score;
    public float maxtime;
    private float currentTime;
    [SerializeField] private bool winState = false; //oyuncu kapiya ulastiginda calisacak
    [SerializeField] private bool loseState = false; // oyuncu oldugunde/ verilen surede tamamlayamadiginda calisacak.

    [Header("DEBUG")]
    [SerializeField] private bool debugMode = false;


    #endregion


    void Start()
    {
        if (instance == null) instance = this;

        currentTime = maxtime;   
    }

    private void Update()
    {
        if(!debugMode)
        {
            CalculateTime();

        }
    }

    public void ChangeLevel(int levelIndex)
    {
        SceneManager.LoadScene(levelIndex);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnToMainMenu()
    {
        //indeks sonradan atilacak.
        SceneManager.LoadScene("Haven");
    }

    public void AddPoints(int value)
    {
        score += value;
    }

    [ContextMenu("Add Points")]
    public void TestAddPoints()
    {
        score += 10;
    }

    private void CalculateTime()
    {
        if(currentTime >= 0 && !winState)
        {
            currentTime -= Time.deltaTime;
        }
        else 
        {
            loseState = true;
        }

        if (currentTime < 0) currentTime = 0;
        
    }

    public void SetWinState(bool var)
    {
        winState = var;
    }
    public bool GetWinState()
    {
        return winState;
    }

    public void SetLoseState(bool var)
    {
        loseState = var;
    }
    public bool GetLoseState()
    {
        return loseState;
    }

    public float GetCurrentTime()
    {
        return currentTime;
    }

    public int GetScore()
    {
        return score;
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
    }
    public void ContinueGame()
    {
        Time.timeScale = 1f;

    }


}
