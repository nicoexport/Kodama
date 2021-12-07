using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterLifeHandler : MonoBehaviour
{
    [SerializeField]
    private int defaultHealth = 1;
    public int health { get; private set; }

    public void Start()
    {
        health = defaultHealth;
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0) PlayerDie();
    }

    public void PlayerDie()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
