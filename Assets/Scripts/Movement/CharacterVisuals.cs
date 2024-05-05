using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterVisuals : MonoBehaviour {
    public float inputParam;
    public bool grounded;
    //public CharacterController2D characterController;
    public AudioClip jumpSound, airJumpSound;

    void Start () {
    }

    void Update () {
        inputParam = GameController.Singleton.mainCharacter.velocity.x;

        float velocityY = GameController.Singleton.mainCharacter.velocity.y;

        Movement ();
    }


    public void Movement () {
        grounded = GameController.Singleton.mainCharacter.Grounded;

        if (GameController.Singleton.mainCharacter.velocity.x > 0) {
            this.transform.eulerAngles = new Vector3 (0, 0, 0);
            GameController.Singleton.mainCharacter.DirMultipler = -1;
        }
        else if (GameController.Singleton.mainCharacter.velocity.x < 0) {
            this.transform.eulerAngles = new Vector3 (0, 180, 0);
            GameController.Singleton.mainCharacter.DirMultipler = 1;
        }
    }
}
