using UnityEngine;

public class SuicideEnemy : MonoBehaviour
{
    [SerializeField] HealthSystem _playerHealth;
    [SerializeField] float _damage;
    [SerializeField] GameObject _Vfx;

    private HealthSystem _enemyHealth;
    private EnemyBase _enemyBase;
    private Animator _anim;
    private bool _hasExploded;

    private void Start()
    {
        _Vfx.SetActive(false);
        _anim = GetComponent<Animator>();
        _enemyBase = GetComponent<EnemyBase>();
        _enemyHealth = GetComponent<HealthSystem>();
    }

    private void Update()
    {
        _anim.SetBool("Run", _enemyBase._agent.velocity.magnitude > 0.1f);

        if (_enemyBase._attacked && !_hasExploded)
        {
            StartExplosionSequence();
        }

        if (_enemyHealth._currentHealth <= _enemyHealth._minHealth)
            Destroy(gameObject);
    }

    private void StartExplosionSequence()
    {
        _hasExploded = true;
        Invoke(nameof(Explode), 0.5f); 
    }

    private void Explode()
    {
        Debug.Log("Düþman Patladý!");
        _Vfx.transform.parent = null; 
        _Vfx.SetActive(true);
        
        _playerHealth.HealthDecrease(_damage);
        
        Destroy(gameObject);
        Destroy(_Vfx, 2f); 
    }
}