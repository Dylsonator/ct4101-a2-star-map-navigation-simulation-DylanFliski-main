using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLoadingscreen : MonoBehaviour
{
    public void LoadingScene() {
        SceneManager.LoadScene("Loading"); //sends me back to different scene to send back to main game to refresh planets
    }
}
