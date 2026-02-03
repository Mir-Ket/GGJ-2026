using UnityEngine;

public class MilkBoy : MonoBehaviour
{
    [SerializeField] ParticleSystem _milkVfx;
    private Animator _anim;
    private ShootSystem _shootSystem;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _anim = GetComponent<Animator>();
        _shootSystem = GetComponent<ShootSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        Milk();
    }
    private void Milk()
    {


        if (_shootSystem._attacked == true) { _milkVfx.Play(); }

        if (_shootSystem._currentAmmo<=_shootSystem._MinAmmo)
        {
        _milkVfx.Stop();

        }

    }
}
