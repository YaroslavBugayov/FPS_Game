using System;
using System.Collections;
using Interfaces;
using Player;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemy
{
    [RequireComponent(typeof(Rigidbody), typeof(Animator))]
    public class EnemyController : MonoBehaviour, IAttacker, IDamageable
    {
        [SerializeField] private int _health;
        [SerializeField] private float _enemySpeed;
        [SerializeField] private float _turnSpeed;
        [SerializeField] private int _damage;
        [SerializeField] private float _attackRadius;
        [SerializeField] private SkinnedMeshRenderer _meshRenderer;
        [SerializeField] private TMP_Text _healthText;
        [SerializeField] private int _score;

        private Rigidbody _rigidbody;
        private Animator _animator;
        private Transform _enemy;
        private GameObject _target;
        private bool _isAttack = false;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _animator = GetComponent<Animator>();
            _enemy = _rigidbody.transform;
        }

        private void Start()
        {
            _target = GameObject.Find("Player");
            _healthText.text = _health.ToString();
        }

        private void Update()
        {
            if (!_isAttack)
            {
                if (Vector3.Distance(_enemy.position, _target.transform.position) > 4)
                {
                    Move();
                    _animator.SetBool("IsWalk", true);
                }
                else
                {
                    StartCoroutine(SetAttack());
                    _animator.SetBool("IsWalk", false);
                }
            }
            
        }

        private IEnumerator SetAttack()
        {
            _isAttack = true;
            _animator.SetTrigger("Attack");
            yield return new WaitForSeconds(3f);
            _isAttack = false;
        }

        private void Move()
        {
            var targetDirection = _enemy.position - _target.transform.position;
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

        public void Attack()
        {
            var hitColliders = Physics.OverlapSphere(transform.position, _attackRadius);
            foreach (var hitCollider in hitColliders)
            {
                var target = hitCollider.gameObject.GetComponent<PlayerController>();
                if (target != null)
                {
                    target.TakeDamage(_damage);
                    break;
                }
            }
        }

        public void TakeDamage(int damage)
        {
            _health -= damage;
            _healthText.text = _health.ToString();
            if (_health < 1)
            {
                if (Random.Range(0, 4) == 0)
                {
                    var heart = Resources.Load<GameObject>("Heart");
                    Instantiate(heart, transform.position, Quaternion.Euler(-90, 0, 0));
                }

                GameObject.Find("Player").GetComponent<PlayerController>().Score += _score;
                Destroy(gameObject);
            }
        }
        
    }
}
