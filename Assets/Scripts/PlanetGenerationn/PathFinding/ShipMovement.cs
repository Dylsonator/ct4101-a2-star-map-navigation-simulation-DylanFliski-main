using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShipMovement : MonoBehaviour
{
    
    public Camera camera;
    public AudioClip Starselect;
    public AudioSource Cam;
    private string StarName;
    bool hitObject;


    public GalaxyGeneration GalGen;
    public int count = 0;
    public void resetcount() {
        count = 0;
    }
    private void Update() {
        Selectstars();

    }

public void Selectstars() {
        if (Input.GetMouseButtonUp(0)) {
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit) & count == 0) {
                    if (hit.transform.tag == ("Star")) {
                        Debug.Log("star1");
                        GalGen.star1 = hit.transform.GetComponent<Star>();
                        Cam.PlayOneShot(Starselect);
                        count++;
                    }
                }
                else {
                    if (Physics.Raycast(ray, out hit)) {
                        if (hit.transform.tag == ("Star")) {
                            GalGen.star2 = hit.transform.GetComponent<Star>();
                            Debug.Log("star2");
                            Cam.PlayOneShot(Starselect);
                        }
                    }
                }
            }
        }
    }
}
