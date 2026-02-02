using UnityEngine;

public class NinjaMask : MonoBehaviour
{

    [SerializeField] ParticleSystem _slashvfx;

    [Header("Explode System")]
    [SerializeField] float _explodRange;
    [SerializeField] float _explodDamage;
    [SerializeField] float _rotationSpeed;


    [SerializeField] LayerMask _layerMask;

    private ShootSystem _shootSystem;
    private HealthSystem _healthSystem;
    private Animator _anim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _anim = GetComponent<Animator>();
        _healthSystem = GetComponent<HealthSystem>();
        _shootSystem = GetComponent<ShootSystem>();
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
            Explod();
        }
        else 
        {
            _anim.SetBool("Attack", false);

        }

    }
    public void Explod()
    {
        if (Physics.CheckSphere(transform.position, _explodRange, _layerMask))
        {
            _healthSystem.HealthDecrease(_explodDamage);
            transform.Rotate(Vector3.up * _rotationSpeed * Time.deltaTime);
            _slashvfx.Play();
        }
    }
}
