using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lookatcam : MonoBehaviour
{
    public GameObject lookat;


    public void Awake() {
        lookat = GameObject.FindGameObjectWithTag("MainCamera"); //camera(for it to look at)
    }
    private void LateUpdate() {
        transform.LookAt(lookat.transform); //constantly making sure its looking at the camera with correct rotation(setting it to just look makes it backwards)
        transform.RotateAround(transform.position, transform.up, 180f);
    }
}
