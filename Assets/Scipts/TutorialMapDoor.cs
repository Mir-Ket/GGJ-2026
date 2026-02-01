using UnityEngine;

public class TutorialMapDoor : MonoBehaviour
{
    private GameObject gameManager;
    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
    }

    private void OnTriggerEnter(Collider other)
    {
        gameManager.GetComponent<GameManager>().ReturnToMainMenu();
    }
}
