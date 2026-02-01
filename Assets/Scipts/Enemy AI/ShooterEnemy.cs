using UnityEngine;

public class ShooterEnemy : MonoBehaviour
{
    [SerializeField] HealthSystem _playerHealth;
    [SerializeField] bool _isShooting;
    [SerializeField] float _damage;
    private EnemyBase _enemyBase;

    private void Start()
    {
        _enemyBase = GetComponent<EnemyBase>();
    }
    private void Update()
    {
        Shoot();
    }
    private void Shoot()
    {
        if (_enemyBase._attacked && !_isShooting)
        {
            _isShooting = true;
            Invoke(nameof(Delayer), 2f);
        }
    }
    private void Delayer()
    {
        Debug.Log("Ateþ etti");
        _playerHealth.HealthDecrease(_damage);
        _isShooting = false;
    }
}
