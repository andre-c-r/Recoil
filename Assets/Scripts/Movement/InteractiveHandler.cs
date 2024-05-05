using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveHandler : MonoBehaviour {
    private void OnCollisionEnter2D (Collision2D collision) {
        if (collision.gameObject.layer == 8) //layer = floor
            this.transform.parent = collision.transform;
    }

    private void OnCollisionExit2D (Collision2D collision) {
        if (collision.gameObject.layer == 8) {//layer = floor
            if(collision.gameObject.transform == this.gameObject.transform.parent) this.transform.parent = null;
        }
    }
}
