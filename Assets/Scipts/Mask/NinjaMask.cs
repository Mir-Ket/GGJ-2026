using UnityEngine;

public class NinjaMask : MonoBehaviour
{
    [SerializeField] ParticleSystem _slashvfx;
    private ShootSystem _shootSystem;
    private Animator _anim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _shootSystem=GetComponent<ShootSystem>();
        _anim=GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Katana();
    }
    private void Katana()
    {
        if (_shootSystem._attacked == true)
        {
            _anim.SetBool("Attack", true);
            _slashvfx.Play();
        }
        else 
        {
            _anim.SetBool("Attack", false);

        }

    }
}
