using UnityEngine;

public class PlayerRestart : MonoBehaviour
{
    public GameObject player;
    public Transform restartPos;


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            ReturnToStart();
        }
    }

    private void ReturnToStart()
    {
        player.transform.position = restartPos.position;
    }


}
