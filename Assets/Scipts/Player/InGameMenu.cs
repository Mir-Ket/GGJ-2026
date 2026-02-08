using UnityEngine;
using UnityEngine.InputSystem;

public class InGameMenu : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    public InputActionReference menuStartKey;
    public GameObject gameManager;

    // Update is called once per frame
    private void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
    }
    void Update()
    {
        if(menuStartKey.action.WasPressedThisFrame())
        {
            canvas.SetActive(!canvas.activeInHierarchy);
            if(canvas.activeInHierarchy == true)
            {
                gameManager.GetComponent<GameManager>().PauseGame();
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;


            }
            else
            {
                gameManager.GetComponent<GameManager>().ContinueGame();
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;


            }
        }
    }

    public void CloseMenu()
    {
        canvas.SetActive(!canvas.activeInHierarchy);
        gameManager.GetComponent<GameManager>().ContinueGame();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void RestartMenu()
    {
        gameManager.GetComponent<GameManager>().RestartLevel();

    }
    public void ExitGame()
    {
        Application.Quit();
    }

    
}
