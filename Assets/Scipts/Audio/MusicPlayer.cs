using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip[] musicList;
    public AudioSource audioSource;
    public int musicIndex= 0;


    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.clip = musicList[musicIndex];
        audioSource.Play();
    }


}
