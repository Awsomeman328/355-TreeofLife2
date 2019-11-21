using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wiles
{
    [RequireComponent(typeof(Rigidbody))]
    public class Projectile : MonoBehaviour
    {
        /// <summary>
        /// Which GameObject in the scene spwned this.
        /// </summary>
        protected GameObject owner;
        /// <summary>
        /// If the Owner has at least 1 child object, this is where we store them.
        /// </summary>
        protected List<GameObject> ownersChildren = new List<GameObject>();
        /// <summary>
        /// This object's Ridigbody.
        /// </summary>
        protected Rigidbody body;

        //public GameObject levelScript;

        /// <summary>
        /// How fast this object should go.
        /// </summary>
        float speed = 100;
        /// <summary>
        ///  How many seconds this projectile should have.
        /// </summary>
        float lifetime = 2;
        /// <summary>
        /// How many seconds this projectile has been alive.
        /// </summary>
        float age = 0;

        /// <summary>
        /// This function is called by whatever script spawned this object after this object has been instantiated.
        /// This function sets our owner, checks for if our owner has any children and adds them to our list, and sets our Rigidbody's velocity.
        /// </summary>
        /// <param name="owner"> The GameObject assosiated with spawning this Projectile. Doesn't nessesarily have to be the actual one that spawns it, 
        /// it is mainly used to determined who "shot" it in the scene. </param>
        /// <param name="direction"> The direction this projectile is being fired in. This is used to determine in which direction to apply our velocity to. </param>
        public void Shoot(GameObject owner, Vector3 direction)
        {
            this.owner = owner;
            //this.ownersChildren = owner.transform.get
            for (int i = 0; i < owner.transform.childCount; i++)
            {
                GameObject child = this.owner.transform.GetChild(i).gameObject;
                //Do something with child
                ownersChildren.Add(child);

            }
            body = GetComponent<Rigidbody>();
            body.velocity = direction * speed;
        }
        /// <summary>
        /// Start is called before the first frame update. Here we set our Rigidbody Component Reference.
        /// </summary>
        void Start()
        {
            body = GetComponent<Rigidbody>();
        }

        /// <summary>
        /// Update is called once per frame. Here we make sure to call GetOlderAndDie() every frame.
        /// </summary>
        void Update()
        {
            GetOlderAndDie();
        }
        /// <summary>
        /// This function is to ensure that after this object has been in the scene for "age" about of seconds it will despawn.
        /// </summary>
        protected void GetOlderAndDie()
        {
            age += Time.deltaTime;

            if (age >= lifetime) Destroy(gameObject);
        }
        /// <summary>
        /// This Event Function gets called whenever this object detects it has made contact/a collision w/ another object.
        /// If it collides w/ anything outside of it's owner or it's children & projectiles, it will broadcast a TakeDamage message to that object and destroy itself.
        /// </summary>
        /// <param name="collider"></param>
        void OnTriggerEnter(Collider collider)
        {

            if (isThisMyOwner(collider)) return;

            if(collider.GetComponent<PlayerController2>() != null) // We've collided w/ the player
            {// Here I'd like to add another check to make sure the projectile after hitting the player wasn't shot by the player.
                //^This^ should have already been done earlier, but it doesn't hurt to double check.
                WilesLevelScript level;
                if((level = GetComponentInParent<WilesLevelScript>()) != null)
                {
                    level.PlayerCollision();

                    print("projectile hit " + collider.gameObject + "'s " + collider);

                    Destroy(gameObject);
                }
            }


            print("projectile hit " + collider.gameObject + "'s " +  collider);

            collider.gameObject.BroadcastMessage("TakeDamage", 1); // Send a message to this object, if it has this function, run it!

            Destroy(gameObject);
        }
        /// <summary>
        /// This function gets used in the OnTriggerEnter function. 
        /// This function is used to determine whether or not what this object collided w/ was this object's owner, owner's children, or owner's projectile.
        /// </summary>
        /// <param name="collider"> The collider of the other object this one is colliding with </param>
        /// <returns> Returns true if the collider belongs to either it's owner, it's owner's children, or one of the owner's projectiles </returns>
        bool isThisMyOwner(Collider collider)
        {
            if (collider.gameObject == owner) return true; // don't hit the shooter of this projectile! (Will only work if the owner has 1 collider)
            for (int i = 0; i < ownersChildren.Count; i++) // Don't hit any of the shooter's children
            {
                //Do something with child
                if (collider.gameObject == ownersChildren[i]) return true;
            }

            Projectile p = collider.GetComponent<Projectile>(); // Checks if this collided w/ a projectile

            if (p != null && p.owner == owner) return true; // Don't collide w/ projectiles from the same owner.

            return false;
        }

    }
}