using UnityEngine;

namespace Weapon
{
    public class Weapon : MonoBehaviour
    {
        [field: SerializeField] public int Damage { get; private set; }
        [field: SerializeField] public float FireRate { get; private set; }
    }
}