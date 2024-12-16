using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour {
    public Transform CameraPos;
    public GameObject pivotLR;
    public GameObject pivotUD;
    public Camera cam;
    public float rotation;

    [SerializeField] private bool LeftBool = false;
    [SerializeField] private bool RightBool = false;
    [SerializeField] private bool UpBool = false;
    [SerializeField] private bool DownBool = false;

    private void Update() {
        Inputs();
    }

    private void FixedUpdate() {
        transform.position = CameraPos.position;//makes sure camera is always on player
        cam.transform.LookAt(pivotUD.transform);
        Cam();

    }
    public void Cam() {
        if (LeftBool == true) {
            pivotLR.transform.Rotate(new Vector3(0, -Time.deltaTime * rotation, 0));
        }
        if (RightBool == true) {
            pivotLR.transform.Rotate(new Vector3(0, Time.deltaTime * rotation, 0)); //rotates on a pivot l
        }
        if (UpBool == true) {
            pivotUD.transform.Rotate(new Vector3(Time.deltaTime * rotation, 0, 0));
        }
        if (DownBool == true) {
            pivotUD.transform.Rotate(new Vector3(-Time.deltaTime * rotation, 0, 0));

        }
    }
    public void Inputs() {
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            RightBool = true;
        }
        if (Input.GetKeyUp(KeyCode.RightArrow)) {
            RightBool = false;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            LeftBool = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow)) {
            LeftBool = false;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            UpBool = true;
        }
        if (Input.GetKeyUp(KeyCode.UpArrow)) {
            UpBool = false;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            DownBool = true;
        }
        if (Input.GetKeyUp(KeyCode.DownArrow)) {
            DownBool = false;
        }
    }
}



