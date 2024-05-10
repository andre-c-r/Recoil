using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;

public class GameController : MonoBehaviour {
    public static GameController Singleton;

    [HideInInspector]
    public Player mainCharacter;

    public Armory armory;

    Inventory _playerinventory;
    public Inventory playerinventory {
        get {
            return _playerinventory;
        }
    }

    public bool controller = false;

    public void ResetLevel() {

    }

    private void Awake() {
        if (Singleton != null) Destroy(this.gameObject);

        Singleton = this;

        _playerinventory = this.gameObject.AddComponent<Inventory>();
    }
}
