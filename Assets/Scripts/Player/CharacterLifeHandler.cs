using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterLifeHandler : MonoBehaviour
{
    public static event Action<Character> OnPlayerDeath;
    public static event Action OnPlayerDied;
    public int Health { get; private set; }
    [SerializeField] private int defaultHealth = 1;
    private Character _character;

    private void Awake()
    {
        _character = GetComponent<Character>();
        Health = defaultHealth;
    }

    public void TakeDamage(int amount)
    {
        if (Health <= 0) return;
        Health -= amount;
        if (Health <= 0) Die();
    }

    private void Die()
    {
        StartCoroutine(DieEnumerator());
    }

    private IEnumerator DieEnumerator()
    {
        OnPlayerDeath?.Invoke(_character);
        InputManager.playerInputActions.Disable();
        yield return _character.DieEnumerator();
        OnPlayerDied?.Invoke();
    }
}
