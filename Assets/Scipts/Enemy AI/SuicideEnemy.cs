using UnityEngine;

public class SuicideEnemy : MonoBehaviour
{
    [SerializeField] HealthSystem _playerHealth;
    [SerializeField] float _damage;

    private EnemyBase _enemyBase;
    private bool _isExplod;
    private void Start()
    {
        _enemyBase = GetComponent<EnemyBase>();
    }
    private void Update()
    {
        Suicide();
    }
    private void Suicide()
    {
        if (_enemyBase._attacked&& !_isExplod)
        {
            _isExplod = true;
            Invoke(nameof(Delayer), 2f);
        }
    }
    private void Delayer()
    {
        Debug.Log("Patladý");
        _playerHealth.HealthDecrease(_damage);
        Destroy(gameObject);
    }
}
