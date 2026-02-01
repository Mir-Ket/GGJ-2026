using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;




public class MusicPlayer : MonoBehaviour
{
    
    [SerializeField] private Slider musicSlider;

    [SerializeField] private AudioClip[] musicList;
    public AudioSource audioSource;
    public int musicIndex= 0;


    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.clip = musicList[musicIndex];
        if(!PlayerPrefs.HasKey("Music Volume"))
        {
            PlayerPrefs.SetFloat("Music Volume", 1f);
        }
        else
        {
            LoadPrefs();

        }
        audioSource.Play();
    }

    public void ChangeVolume()
    {
        if(musicSlider != null)
        {
            audioSource.volume = musicSlider.value;
            SavePrefs(musicSlider.value);
        }
        
    }

    private void SavePrefs(float value)
    {
        PlayerPrefs.SetFloat("Music Volume", value);
    }

    private void LoadPrefs()
    {
        PlayerPrefs.GetFloat("Music Volume");
    }

}
