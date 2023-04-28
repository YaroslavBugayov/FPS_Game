using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private TMP_Text _heathText;
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private PlayerController _player;
    [SerializeField] private GameObject _deathPanel;
    [SerializeField] private Button _restartButton;

    private void Awake()
    {
        _deathPanel.SetActive(false);
    }

    private void Start()
    {
        _restartButton.onClick.AddListener(Death);
    }

    private void Update()
    {
        _heathText.text = "Health: " + (_player.Health >= 0 ? _player.Health : 0);
        _scoreText.text = "Score: " + _player.Score;
    }

    public void Death()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        _deathPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void Restart()
    {
        SceneManager.LoadScene("SampleScene");
        Time.timeScale = 1;
    }
}
