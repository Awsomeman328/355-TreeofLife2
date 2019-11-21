using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Wiles {
    public class WilesGUI : MonoBehaviour
    {
        /// <summary>
        /// Reference to the boss.
        /// </summary>
        public GameObject boss;
        /// <summary>
        /// Reference to the health bar at the top of the interface.
        /// </summary>
        public GameObject bossHealthBar;
        /// <summary>
        /// Reference to the extra boss health bars at the top of the interface.
        /// </summary>
        public List<GameObject> bossExtraBars = new List<GameObject>();
        /// <summary>
        /// Reference to the player in the scene
        /// </summary>
        public GameObject player;
        /// <summary>
        /// Reference to the health bar of th player at the bottom left of the interface.
        /// </summary>
        public GameObject playerHealthBar;
        /// <summary>
        /// Reference to the GameOver text in the center of the interface.
        /// </summary>
        public Text gameOverText;
        /// <summary>
        /// Reference to the LevelScript Object w/in the scene that holds a lot of the data this class uses.
        /// </summary>
        public GameObject level;
        /// <summary>
        /// The BossController Compenent on the boss.
        /// </summary>
        WilesBossController bossController;
        /// <summary>
        /// The levelScript compenent on the LevelScript Object in the scene.
        /// </summary>
        WilesLevelScript levelScript;

        /// <summary>
        ///  Start is called before the first frame update.
        ///  Here we set the levelScript and the bossController from our two references.
        /// </summary>
        void Start()
        {
            levelScript = level.GetComponent<WilesLevelScript>();
            bossController = boss.GetComponent<WilesBossController>();
            gameOverText.text = " ";
        }

        /// <summary>
        /// Update is called once per frame.
        /// Here we update our displays to correctly show our boss's hp and player hp.
        /// If the gameOver bool from either the bossController or the levelScript is set to true, then we show our GameOver text, telling the player they've lost or won respectively.
        /// </summary>
        void Update()
        {
            bossHealthBar.transform.localScale = new Vector3(bossController.health / bossController.maxHealth, 1, 1);
            playerHealthBar.transform.localScale = new Vector3(levelScript.playerHealth / levelScript.playerMaxHealth, 1, 1);
            if (bossController.extraHealth > 2) bossController.extraHealth = 2;
            if (bossController.extraHealth == 1) bossExtraBars[1].transform.localScale = new Vector3(0, 0, 0);
            if (bossController.extraHealth == 0) bossExtraBars[0].transform.localScale = new Vector3(0, 0, 0);
            if (bossController.gameOver) gameOverText.text = "GAME OVER! \n YOU WIN!";
            if (levelScript.gameOver) gameOverText.text = "GAME OVER! \n YOU LOSE!";
        }
    }
}