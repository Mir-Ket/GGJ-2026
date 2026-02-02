using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField] ParticleSystem _gunVfx;
    private Animator _anim;
    private ShootSystem _shootSystem;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _anim=GetComponent<Animator>();
        _shootSystem = GetComponent<ShootSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        DesertEagle();
    }
    private void DesertEagle()
    {
        if (_shootSystem._reload == true) _anim.SetBool("Reload", true);
        else { _anim.SetBool("Reload", false);Debug.Log(_anim); }

        if (_shootSystem._attacked == true) {_anim.SetBool("Shoot", true); _gunVfx.Play(); }

        else { _anim.SetBool("Shoot", false);}
    }
}
