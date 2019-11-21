using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wiles
{
    public class WilesBossController : MonoBehaviour
    {
        /// <summary>
        /// The distance the boss can see the Player. (Old code, probably need to delete.)
        /// </summary>
        public float visionDistanceThreshold = 10;
        /// <summary>
        /// The distance in which the boss will stop approaching tha player. (Old code, probably need to delete.)
        /// </summary>
        public float pursueDistanceThreshold = 7;
        /// <summary>
        /// The speed at which the boos can move.
        /// </summary>
        public float speed = 10;
        /// <summary>
        /// Reference to the Projectile Prefab to fire.
        /// </summary>
        public Projectile prefabProjectile;
        /// <summary>
        /// Reference to the ProjectileHoiming Prefab to fire.
        /// </summary>
        public ProjectileHoming prefabProjectileHoming;
        /// <summary>
        /// The amount of health the boss currently has
        /// </summary>
        public float health = 100;
        /// <summary>
        /// The maximum amount of health the boss can have per bar of health.
        /// </summary>
        public float maxHealth = 100;
        /// <summary>
        /// The number of extra bars of health the boss has. When the boss's health reaches 0, if this in greater than 0, this value goes down and the boss's health is restored
        /// </summary>
        public int extraHealth = 2;
        /// <summary>
        /// The boolean to tell if the boss has been defeated.
        /// </summary>
        public bool gameOver = false;
        
        [HideInInspector]
        public Vector3 velocity = new Vector3();
        
        /// <summary>
        /// Reference to the LevelScript GameObject.
        /// </summary>
        public GameObject level;
        /// <summary>
        /// The transform we wish to have the boss attack.
        /// </summary>
        public Transform attackTarget { get; private set; }
        /// <summary>
        /// The current state the boss in right now.
        /// </summary>
        WilesBossState currentState;

        //GameObject boss;

        /// <summary>
        /// Start is called before the first frame update. This makes sure that if there is a player in the scene, we set it as our current attackTarget.
        /// </summary>
        void Start()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if(player !=null) attackTarget = player.transform;
            
        }
        /// <summary>
        /// This function is called from our weakSpot scripts on parts of the boss's children to have the boss take damage.
        /// If this object's GameOver bool is false, this function won't run.
        /// This function also handles our extra bars of health.
        /// </summary>
        /// <param name="dmg"> The number amount of damage the boss will receave. </param>
        public void takeDamage(float dmg)
        {
            if (gameOver) return;
            health -= dmg;
            if (health <= 0)
            {
                if (extraHealth > 0)
                {
                    extraHealth--;
                    health += maxHealth;
                }
                else { gameOver = true; }
            }
        }

        /// <summary>
        /// Update is called once per frame. This handles our behavior and state switching for our bossStates.
        /// </summary>
        void Update()
        {
            if (gameOver) SwitchToState(new WilesBossStateDying());
            if (currentState == null) SwitchToState( new WilesBossStateIdle());
            if (currentState != null) SwitchToState(currentState.Update(this));
            //test();
            rotateTowardsTarget();
            if (gameOver) SwitchToState(new WilesBossStateDying());
        }
        /// <summary>
        /// This is the function called when we want to change which state the boss should be in.
        /// </summary>
        /// <param name="newState"> The new state we wish for the boss to transistion into. </param>
        private void SwitchToState(WilesBossState newState)
        {
            if (newState != null)
            {
                if(currentState != null) currentState.OnEnd(this);
                currentState = newState;
                currentState.OnStart(this);
            }
        }
        /// <summary>
        /// The vector to our current attack target.
        /// </summary>
        /// <returns> The direction from this object to our target's transform. </returns>
        public Vector3 VectorToAttackTarget()
        {
            return attackTarget.position - transform.position;
        }

        public float DistanceToAttackTarget()
        {
            return VectorToAttackTarget().magnitude;

        }
        /// <summary>
        /// I'm not sure if this function gets used anywhere, but I'm not deleting it now just in case.
        /// </summary>
        /// <returns> Whether or not the boss can see the attack target. If there is another object in the way of the raycast, it cannot "see" it. </returns>
        public bool CanSeeAttackTarget()
        {
            // if dis < threshold && raycast from player to boss hits...
            // transition to pursue
            if (attackTarget == null) return false;
            Vector3 vectorBetween = VectorToAttackTarget();

            if (vectorBetween.sqrMagnitude < visionDistanceThreshold * visionDistanceThreshold)
            {
                // player is close enough to boss to activate it...
                Ray ray = new Ray(transform.position, vectorBetween.normalized);
                // RaycastHit hit;
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    //Debug.Log("Raycast from boss to player hit something!!");
                    if (hit.transform == attackTarget) return true; // clear line of vision!
                }
            }
            return false;
        }
        /// <summary>
        /// This function is meant to rotate the boss to always be looking at the player. Currently is not working.
        /// </summary>
        void rotateTowardsTarget()
        {
            if (CanSeeAttackTarget())
            {
                gameObject.transform.rotation = Quaternion.Euler(VectorToAttackTarget());
            }
        }

        /// <summary>
        /// Spawn a new projectile, assigning the boss as it's owner.
        /// </summary>
        public void ShootProjectile()
        {
            Projectile newProjectile = Instantiate(prefabProjectile, transform.position, Quaternion.identity, level.transform);
            Vector3 dir = (VectorToAttackTarget()).normalized;
            newProjectile.Shoot(gameObject, dir);
            
        }
        /// <summary>
        /// Spawn a new projectileHoming, assigning the boss as it's owner.
        /// </summary>
        public void ShootHomingProjectile()
        {
            ProjectileHoming newProjectile = Instantiate(prefabProjectileHoming, transform.position, Quaternion.identity);
            newProjectile.Shoot(gameObject, attackTarget, Vector3.up);
        }
    }
}