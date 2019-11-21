using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wiles {
    public class BossWeakSpot : MonoBehaviour
    {
        /// <summary>
        /// Reference to the boss.
        /// </summary>
        public GameObject boss;
        /// <summary>
        /// Reference to the bossController component on the boss Object.
        /// </summary>
        WilesBossController bossController;

        /// <summary>
        ///  Start is called before the first frame update.
        ///  This just sets our bossController reference.
        /// </summary>
        void Start()
        {
            bossController = boss.GetComponent<WilesBossController>();
        }

        /// <summary>
        ///  Update is called once per frame.  This does nothing! =D
        /// </summary>
        void Update()
        {

        }
        /// <summary>
        /// This function gets called whenever a projectile collides w/ this object.
        /// All this function does is relay the message to the bossController.
        /// </summary>
        /// <param name="dmg"> The number amount of damage to do to the boss. </param>
        public void TakeDamage(int dmg)
        {
            bossController.takeDamage(dmg);
        }
    }
}