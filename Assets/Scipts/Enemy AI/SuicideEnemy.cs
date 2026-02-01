using UnityEngine;

public class SuicideEnemy : MonoBehaviour
{
    [SerializeField] HealthSystem _playerHealth;
    [SerializeField] float _damage;
    [SerializeField] GameObject _Vfx;

    private HealthSystem _enemyHealth;
    private EnemyBase _enemyBase;
    private Animator _anim;
    private bool _isExplod;
    private void Start()
    {
        _Vfx.SetActive(false);
        _anim = GetComponent<Animator>();
        _enemyBase = GetComponent<EnemyBase>();
        _enemyHealth = GetComponent<HealthSystem>();
    }
    private void Update()
    {
        Suicide();
    }
    private void Suicide()
    {
        if (_enemyBase._walkPointSet==true)
        {
            _anim.SetBool("Run", true);
        }
        if (_enemyBase._attacked&& !_isExplod)
        {
            _isExplod = true;
            Invoke(nameof(Vfx), 1.94f);

        }

        if (_enemyHealth._currentHealth<=_enemyHealth._minHealth)
        {
            Destroy(gameObject);
        }
    }
    private void Vfx()
    {
        Invoke(nameof(Delayer), 0.06f);
        _Vfx.SetActive(true);
    }
    private void Delayer()
    {
        Debug.Log("Patladý");
        _playerHealth.HealthDecrease(_damage);
        Destroy(gameObject);
    }

}
