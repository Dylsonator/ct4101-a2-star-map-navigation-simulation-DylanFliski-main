using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadnewPlanets : MonoBehaviour {
    // Start is called before the first frame update
    public void Update() {
        SceneManager.LoadScene("MainGame"); //instantly changes back to maingame

    }
}
