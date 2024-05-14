using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteColliderAux : MonoBehaviour {
    BoxCollider2D colider;
    SpriteRenderer renderer;
    // Start is called before the first frame update
    void Start() {
        renderer = GetComponent<SpriteRenderer>();
        colider = GetComponent<BoxCollider2D>();
        UpdateSize();
    }

    public void UpdateSize() {
        colider.size = renderer.size;
    }


}