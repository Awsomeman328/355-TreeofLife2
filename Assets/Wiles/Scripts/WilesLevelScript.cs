using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wiles
{
    public class WilesLevelScript : MonoBehaviour
    {
        /// <summary>
        /// Reference to our player in the scene.
        /// </summary>
        public GameObject player;
        /// <summary>
        /// Reference to our desired projectile for the player to shoot.
        /// </summary>
        public Projectile playerProjectile;
        /// <summary>
        /// The stored value for the player's health.
        /// </summary>
        public float playerHealth = 100;
        /// <summary>
        /// The stored value for the player's maximum health.
        /// </summary>
        public float playerMaxHealth = 100;

        /// <summary>
        /// The ray that gets shot out of the camera to point at whatever the mouse is pointing to.
        /// </summary>
        Ray ray;
        /// <summary>
        /// One of our two gameOver bools for determining if someone is defeated.
        /// </summary>
        public bool gameOver = false;

        /// <summary>
        /// Update is called once per frame. This does nothing! =D
        /// </summary>
        void Start()
        {

        }

        /// <summary>
        ///  Update is called once per frame. If the this gameOver is not true, check if the mouse button has been clicked.
        ///  If mouse is clicked, update the ray to point where the mouse is and shoot a projectile.
        ///  If GameOver is true, kill the player if he is still alive.
        /// </summary>
        void Update()
        {
            if (!gameOver)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    ShootProjectile();
                }
            } else
            {
                if (player != null) Destroy(player);
            }
        }
        /// <summary>
        /// Spawn a new projectile, assigning the player as it's owner, and using the ray as it's direction.
        /// </summary>
        void ShootProjectile()
        {
            Projectile newProjectile = Instantiate(playerProjectile, player.transform.position, Quaternion.identity);
            Vector3 dir = (VectorToAttackTarget()).normalized;
            newProjectile.Shoot(player, dir);

        }
        /// <summary>
        /// The direction we want the projectile to fly towards
        /// </summary>
        /// <returns> returns a vector3 based on the calulated ray. </returns>
        Vector3 VectorToAttackTarget()
        {
            return ray.direction;
        }
        /// <summary>
        /// This function gets called from the projectile class to be able to tell when the player gets hit (b/c I cannot changed the player class)
        /// </summary>
        public void PlayerCollision()
        {
            if (gameOver) return;
            playerHealth -= 10.0f;
            if (playerHealth <= 1) gameOver = true;
        }
    }
}