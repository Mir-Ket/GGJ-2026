using UnityEngine;

public class ShooterEnemy : MonoBehaviour
{
    [SerializeField] HealthSystem _playerHealth;
    [SerializeField] bool _isShooting;
    [SerializeField] float _damage;
    [SerializeField] GameObject _Vfx;

    private Animator _anim;
    private HealthSystem _enemyHealth;

    private EnemyBase _enemyBase;

    private void Start()
    {
        _Vfx.SetActive(false);
        _anim = GetComponent<Animator>();
        _enemyBase = GetComponent<EnemyBase>();
        _enemyHealth = GetComponent<HealthSystem>();

    }
    private void Update()
    {
        Shoot();
    }
    private void Shoot()
    {
        if (_enemyBase._walkPointSet==true)
        {
            _anim.SetBool("Run", true);
        }

         if (_enemyBase._attacked && !_isShooting)
            {
            _isShooting = true;
            _Vfx?.SetActive(false);
            InvokeRepeating(nameof(Delayer), 2f,2f);
        }
        if (_enemyHealth._currentHealth <= _enemyHealth._minHealth)
        {
            Destroy(gameObject);
        }
    }
    private void Delayer()
    {
        Debug.Log("Ateþ etti");
        _playerHealth.HealthDecrease(_damage);
        _Vfx?.SetActive(true);
        _isShooting = false;
    }
}
