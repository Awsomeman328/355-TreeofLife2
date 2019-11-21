using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wiles
{
    public class ProjectileHoming : Projectile
    {
        /// <summary>
        /// The transform Object this object is trying to follow and hit.
        /// </summary>
        Transform target;
        /// <summary>
        /// The ammount of force we want to direct towards our target onto this object.
        /// </summary>
        private float homingForce = 10000;
        /// <summary>
        /// The max amount of force we will apply on this object to change it's course.
        /// </summary>
        float maxforce = 100;
        /// <summary>
        /// A slight override of the Projectile's Shoot method, we just make sure to set our target before calling the original Shoot function.
        /// </summary>
        /// <param name="owner"> The owner of this projectile. </param>
        /// <param name="target"> The target we want this projectile to hit/aim towards. </param>
        /// <param name="direction"> The current direction of this projectile. </param>
        public void Shoot(GameObject owner,Transform target, Vector3 direction)
        {
            this.target = target;

            Shoot(owner, direction);
        }

        /// <summary>
        /// Start is called before the first frame update. This doesn't do anything. =D
        /// </summary>
        void Start()
        {

        }



        /// <summary>
        /// Update is called once per frame.
        /// In this, we make sure that this object gets older and dies, homes in on whatever it is targeting, and rotates itself to point in the direction it is travelling.
        /// </summary>
        void Update()
        {
            GetOlderAndDie();
            Homing();

            transform.rotation = Quaternion.FromToRotation(Vector3.up, body.velocity.normalized);

        }
        /// <summary>
        /// Using steering vector math, we apply forces to this object to move it closer to its target.
        /// </summary>
        private void Homing()
        {


            Vector3 dir = (target.position - transform.position).normalized;

            Vector3 steer = dir * homingForce - body.velocity;

            

            body.AddForce(dir * Time.deltaTime);
        }
    }
}