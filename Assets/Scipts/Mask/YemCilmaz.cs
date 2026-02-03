using UnityEngine;

public class YemCilmaz : MonoBehaviour
{
    private Animator _anim;
    private ShootSystem _shootSystem;


    [Header("Explode System")]
    [SerializeField] float _explodRange;
    [SerializeField] float _explodDamage;
    [SerializeField] ParticleSystem _explodeVfx;
    [SerializeField] LayerMask _layerMask;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _anim = GetComponent<Animator>();
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
        else { _anim.SetBool("Reload", false); Debug.Log(_anim); }

        if (_shootSystem._attacked == true) 
        { 
            _anim.SetBool("Attack", true);
            Invoke(nameof(Explod), 0.5f);
        }

        else { _anim.SetBool("Attack", false); }
    }
    private void Explod()
    {

            Collider[] hitColliders = Physics.OverlapSphere(transform.position, _explodRange, _layerMask);

            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.TryGetComponent(out HealthSystem enemyHealth))
                {
                    enemyHealth.HealthDecrease(_explodDamage);
                }
            }


            Instantiate(_explodeVfx, transform.position, Quaternion.identity);
            _explodeVfx.Play();
        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow; Gizmos.DrawWireSphere(transform.position, _explodRange);
    }
}
