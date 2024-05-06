using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameController : MonoBehaviour {
    public static GameController Singleton;

    [HideInInspector]
    public StandardMovement.PlayerInput mainCharacterInput;

    [HideInInspector]
    public Player mainCharacter;

    private void Awake() {
        if (Singleton != null) Destroy(this.gameObject);

        Singleton = this;
    }
}
