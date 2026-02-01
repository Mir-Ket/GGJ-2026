using UnityEngine;
using TMPro;


public class TimeAndPointCanvas : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI scoreText;
    [SerializeField] private GameManager gameManager;

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").gameObject.GetComponent<GameManager>();
        if(gameManager == null)
        {
            Debug.LogError("Game Manager Bulunamadi");
        }
    }

    // Update is called once per frame
    void Update()
    {
        float time = gameManager.GetCurrentTime();
        //Zaman ve puani GameManager Uzerinden Cek
        timeText.text = "Remaining Time: " + (int)time;
        scoreText.text = "Score : " + gameManager.GetScore();
    }

    
}
