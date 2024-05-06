using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StandardMovement {
    public class PlayerAnimatorControl : MonoBehaviour {
        [SerializeField] Animator anim;
        [SerializeField] Player player;
        [SerializeField] Vector2Int randonvalueMinMax;

        int dashTimer = 0;

        private void Awake() {
            if (anim == null) anim = this.GetComponent<Animator>();
            if (player == null) player = GetComponent<Player>();
            InvokeRepeating("ChangeRandonValue", 1, 1);
        }

        private void FixedUpdate() {
            CalculateDashTimer();
        }

        public void StartJump() {
            anim.SetTrigger("jump");
        }
        public void EndJump() {
            anim.SetTrigger("endJump");
        }

        public void Spin() {
            anim.SetTrigger("spin");
        }
        public void AirDash() {
            anim.SetBool("airDash", true);
            dashTimer = 12;
        }

        void CalculateDashTimer() {
            if (dashTimer <= 0) return;
            dashTimer--;
            if (dashTimer == 0)
                anim.SetBool("airDash", false);
        }

        public void Dash() {
            anim.SetTrigger("dash");
        }

        public void SetWallSlidingVariable(bool input) {
            anim.SetBool("wallSliding", input);
        }
        public void SetGrounderVariable(bool input) {
            anim.SetBool("grounder", input);
        }

        void ChangeRandonValue() {
            anim.SetInteger("randonValue", Random.Range(randonvalueMinMax.x, randonvalueMinMax.y + 1));
        }
    }
}
