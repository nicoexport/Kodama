using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "RuntimeSet")]
public class RuntimeSet : ScriptableObject
{
    public Character CurrentCharacter { get; private set; }

    public void SetCurrentCharacter(Character character)
    {
        CurrentCharacter = character;
    }
}
