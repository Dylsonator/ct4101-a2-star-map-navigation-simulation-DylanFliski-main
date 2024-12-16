using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Mainmenu : MonoBehaviour {
    public void startgame() {
        SceneManager.LoadScene("MainGame"); //load main game
    }
    public void endgame() {
        Application.Quit(); //quit
    }
}
