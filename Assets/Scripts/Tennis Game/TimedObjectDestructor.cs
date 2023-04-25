using System;
using UnityEngine;

namespace UnityStandardAssets.Utility
{
    public class TimedObjectDestructor : MonoBehaviour
    {
        [SerializeField] private float m_TimeOut = 1.0f;
        [SerializeField] private bool m_DetachChildren = false;
        public int nbTouched = 0;
        public float timeStay;

        private void Awake() {
            Invoke("DestroyNow", m_TimeOut);
        }

        /// <summary>
        /// Number of time the ball touches the ground or the walls, with 5 touched, the ball is destroyed
        /// </summary>
        public void AddNbTouched() {
            nbTouched++;
            if(nbTouched >= 5) {
                DestroyNow();
            }
        }

        /// <summary>
        /// A security, if the ball is staying on the floor for 3 seconds, it disappears
        /// </summary>
        /// <param name="collision"></param>
        private void OnCollisionStay(Collision collision) {
            if (collision.gameObject.tag == "Bounce") {
                if (timeStay <= 3) {
                    timeStay += Time.deltaTime;
                }
                else {
                    DestroyNow();
                }
            }
        }

        /// <summary>
        /// If the ball exit the floor, the time staying on the floor is reset to 0
        /// </summary>
        /// <param name="collision"></param>
        private void OnCollisionExit(Collision collision) {
            timeStay = 0;
        }

        /// <summary>
        /// If the ball touched the ground or the wall, add +1 to the hit ball incrementer
        /// </summary>
        /// <param name="collision"></param>
        private void OnCollisionEnter(Collision collision) {
            if(collision.gameObject.tag == "Bounce") {
                this.gameObject.GetComponent<AudioSource>().Play();
                AddNbTouched();
            }
            if(collision.gameObject.tag == "Destroyer" && this.gameObject.tag =="Ball") {
                DestroyNow();
            }
        }

        /// <summary>
        /// Destroy the ball
        /// </summary>
        public void DestroyNow()
        {
            if (m_DetachChildren)
            {
                transform.DetachChildren();
            }
            LevelTennisMgr.instance.instantiatedBalls--;
            Destroy(gameObject);
        }
    }
}
