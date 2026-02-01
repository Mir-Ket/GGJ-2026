using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviour
{
    [Header("AI Components")]
    [SerializeField] NavMeshAgent _agent;
    [SerializeField] Transform _playerTransform;
    [SerializeField] LayerMask _groundLayer, _playerLayer;
    [SerializeField] Animator _anim;
    [SerializeField] Animator _anim2;
    [Header("AI Patrol")]
    [SerializeField] Vector3 _walkPoint;
    [SerializeField] float _walkRange;
    [SerializeField] bool _walkPointSet;

    [Header("AI Attack")]
    [SerializeField] float _attackDelay;
    public bool _attacked;

    [Header("AI States")]
    [SerializeField] float _sightRange, _attackRange;
    [SerializeField] bool _playerInSightRange, _playerInAttackRange;

    private void Awake()
    {
        
        _anim=GetComponent<Animator>();
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        _agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckPlayer();
    }
    private void CheckPlayer()
    {
        _playerInSightRange = Physics.CheckSphere(transform.position, _sightRange, _playerLayer);
        _playerInAttackRange= Physics.CheckSphere(transform.position,_attackRange, _playerLayer);

        if (!_playerInSightRange && !_playerInAttackRange)
        {
            Patrol();
        }
        if (_playerInSightRange && !_playerInAttackRange)
        {
            FollowPlayer();
        }
        if (_playerInAttackRange && _playerInSightRange)
        {
            AttackPlayer();
        }
    }
    private void Patrol()
    {
        if (!_walkPointSet)
        {
            SearchWalkPoint();
        }
        if (_walkPointSet)
        {
            _agent.SetDestination(_walkPoint);
            _anim.SetBool("Run", true);
            _anim2.SetBool("Run", true);
        }
        Vector3 distanceToWalkPoint = transform.position - _walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
        {
            _walkPointSet = false;
        }
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-_walkRange, _walkRange);
        float randomX = Random.Range(-_walkRange, _walkRange);

        _walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(_walkPoint,-transform.up,2f,_groundLayer))
        {
            _walkPointSet = true;
        }
    }

    private void FollowPlayer()
    {
        _agent.SetDestination(_playerTransform.position);
    }
    public void AttackPlayer()
    {
       // _agent.SetDestination(transform.position);

        if (!_attacked)
        {
            Debug.Log("Attacked");


            _attacked = true;
            Invoke(nameof(ResetAttack), _attackDelay);
        }
    }

    private void ResetAttack()
    {
        _attacked = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _sightRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _walkRange);
    }
}
