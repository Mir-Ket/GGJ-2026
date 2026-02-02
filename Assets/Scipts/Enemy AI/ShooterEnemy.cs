using UnityEngine;

public class ShooterEnemy : MonoBehaviour
{
    [SerializeField] HealthSystem _playerHealth;
    [SerializeField] float _damage;
    [SerializeField] GameObject _Vfx;

    private Animator _anim;
    private HealthSystem _enemyHealth;
    private EnemyBase _enemyBase;
    private bool _canShoot = true;

    private void Start()
    {
        _Vfx.SetActive(false);
        _anim = GetComponent<Animator>();
        _enemyBase = GetComponent<EnemyBase>();
        _enemyHealth = GetComponent<HealthSystem>();
    }

    private void Update()
    {
        HandleAnimations();
        HandleCombat();
        
        if (_enemyHealth._currentHealth <= _enemyHealth._minHealth)
            Destroy(gameObject);
    }

    private void HandleAnimations()
    {
        bool isMoving = _enemyBase._agent.velocity.magnitude > 0.1f;
        _anim.SetBool("Run", isMoving);

    }

    private void HandleCombat()
    {
        if (_enemyBase._attacked && _canShoot)
        {
            ExecuteShot();
        }
    }

    private void ExecuteShot()
    {
        if (_canShoot && _enemyBase._agent.velocity.magnitude <= 0.1f) _enemyBase._agent.SetDestination(transform.position);
        _canShoot = false;
        Debug.Log("Ateþ etti");
        
        _playerHealth.HealthDecrease(_damage);
        _Vfx.SetActive(true);
        
        Invoke(nameof(ResetVfx), 0.5f);
        Invoke(nameof(ResetShoot), 1f);
    }

    private void ResetVfx() => _Vfx.SetActive(false);
    private void ResetShoot() => _canShoot = true;
}