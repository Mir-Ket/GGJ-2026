using UnityEngine;

public class NinjaMask : MonoBehaviour
{
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
        if (_shootSystem._shooting == true)
        {
            _anim.SetBool("Attack", true);
        }
        else 
        {
            _anim.SetBool("Attack", true);

        }

    }
}
