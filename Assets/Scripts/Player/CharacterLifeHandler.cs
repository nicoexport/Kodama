using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterLifeHandler : MonoBehaviour
{
    public event Action<Character> OnCharacterDeath;
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
        OnCharacterDeath?.Invoke(_character);
    }
    
}
