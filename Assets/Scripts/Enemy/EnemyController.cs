using UnityEngine;

namespace Enemy
{
    [RequireComponent(typeof(Rigidbody))]
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private int _health;
        [SerializeField] private float _enemySpeed;
        [SerializeField] private float _turnSpeed;

        private Rigidbody _rigidbody;
        private Transform _enemy;
        private Transform _target;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _enemy = _rigidbody.transform;
        }

        private void Start()
        {
            _target = GameObject.Find("Player").transform;
        }

        private void Update()
        {
            // if (Vector3.Distance(_enemy.position, _target.position) > 4)
            // {
                Move();
            // }
            // else
            // {
                // Attack();
            // }
        }

        private void Attack()
        {
            
        }

        private void Move()
        {
            var targetDirection = _enemy.position - _target.position;
            targetDirection = new Vector3(targetDirection.x, 0f, targetDirection.z);
            var singleStep = _turnSpeed * Time.deltaTime;
            // _enemy.forward = -targetDirection.normalized;
            var look = Vector3.RotateTowards(
                _enemy.forward, 
                targetDirection, 
                singleStep, 
                0f
                );
            _enemy.rotation = Quaternion.LookRotation(look);
            _enemy.Translate(Vector3.back * _enemySpeed);
        }
    }
}
