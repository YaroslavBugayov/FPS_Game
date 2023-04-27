using UnityEngine;

namespace Enemy
{
    public class SpawnController : MonoBehaviour
    {
        [SerializeField] private GameObject[] _enemies;
        [SerializeField] private GameObject[] _spawnPoints;
        [SerializeField] private float _swawnSpeedIncreace;

        private float _spawnPeriodicity = 5f;

        private void Start() => InvokeRepeating(nameof(SpawnEnemy), 0f, 2f);

        private void SpawnEnemy()
        {
            if (Random.Range(0, 40) < _spawnPeriodicity)
            {
                Instantiate(_enemies[Random.Range(0, 2)], 
                    _spawnPoints[Random.Range(0, _spawnPoints.Length)].transform.position,
                    Quaternion.identity);
                _spawnPeriodicity += _swawnSpeedIncreace;
            }
        }
    }
}
