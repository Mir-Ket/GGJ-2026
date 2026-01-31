using UnityEngine;

public enum SoundType
{
    PLAYER_HEAL,
    PLAYER_HURT,
    PLAYER_DEAD,
    PLAYER_SLIDE,

    HOOK_LAUNCH,
    HOOK_PULL,
    KATANA,
    PISTOL,
    MICROPHONE,

    BOMBALAK1,
    BOMBALAK2,
    ENEMY2_ATTACK,
    ENEMY2_DEAD,


}

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] soundList;
    private static SoundManager instance;
    private AudioSource audioSource;
    [SerializeField] private AudioSource playerAudio;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;

        }
        else
        {
            Destroy(this.gameObject);
        }

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void PlaySound(SoundType sound, float volume = 1)
    {
        instance.audioSource.PlayOneShot(instance.soundList[(int)sound], volume);
    }

  
}
