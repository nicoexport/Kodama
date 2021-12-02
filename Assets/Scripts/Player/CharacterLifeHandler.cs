using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterLifeHandler : MonoBehaviour
{
    private int health;

    public void Start()
    {
        health = 1;
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
