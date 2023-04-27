using System.Collections;
using System.Collections.Generic;
using Player;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private TMP_Text _heathText;
    [SerializeField] private PlayerController _player;
    
    void Update()
    {
        _heathText.text = "Health: " + _player.Health;
    }
}
