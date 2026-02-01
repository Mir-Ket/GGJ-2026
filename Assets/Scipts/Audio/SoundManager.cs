using UnityEngine;
using UnityEngine.UI;

public enum SoundType
{
    PLAYER_HEAL,
    PLAYER_HURT,
    PLAYER_SLIDE,

    HOOK_LAUNCH,
    HOOK_HIT,
    KATANA_AIR,
    KATANA_HIT,
    PISTOL,

    BOMBALAK1,
    ENEMY2_ATTACK,
    ENEMY2_DEAD,


}


[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] soundList;
    private static SoundManager instance;
    private AudioSource audioSource;
    [SerializeField] private Slider audioSlider;


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
        //main audio
        audioSource = GetComponent<AudioSource>();

        if (!PlayerPrefs.HasKey("SFX Volume"))
        {
            PlayerPrefs.SetFloat("SFX Volume", 1f);
        }
        else
        {
            LoadPrefs();

        }



    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void PlaySound(SoundType sound, float volume = 1)
    {
        instance.audioSource.PlayOneShot(instance.soundList[(int)sound], volume);
    }

    public void ChangeVolume()
    {
        if (audioSlider != null)
        {
            audioSource.volume = audioSlider.value;
            SavePrefs(audioSlider.value);
        }

    }

    private void SavePrefs(float value)
    {
        PlayerPrefs.SetFloat("SFX Volume", value);
    }

    private void LoadPrefs()
    {
        PlayerPrefs.GetFloat("SFX Volume");
    }




}
