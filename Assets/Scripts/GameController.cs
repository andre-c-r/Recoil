using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public static Vector2 Checkpoint { get; set; } = Vector2.zero;

    public void ResetLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void Awake() {
        if (Singleton != null) Destroy(this.gameObject);

        Singleton = this;

        _playerinventory = this.gameObject.AddComponent<Inventory>();
    }
}
