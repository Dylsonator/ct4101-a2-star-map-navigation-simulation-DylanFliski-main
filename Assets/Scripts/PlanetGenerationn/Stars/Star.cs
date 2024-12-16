
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using static DijkstraSimplified;

public class Star : MonoBehaviour, IPointerEnterHandler{

    //scripts
    public UIScript UI;

    //names
    public GameObject NamePrefab;
    private GameObject ThisStar;
    private GameObject CurrentTextName;



    //djikstras
    public Dictionary<Star, float> starRoutes = new Dictionary<Star, float>();

    //line rendering / setting up routes
    public GameObject lineRenderer;
    public GameObject UIObject;
    [Range(0f, 75f)]
    public float maxDistance = 75;
    private float maxRoutes = 0;
    private int maxConnections = 5;

    public void Awake() {

        UIObject = GameObject.FindGameObjectWithTag("UI");
        UI = UIObject.GetComponent<UIScript>(); //finding the UI object for the script

        maxRoutes = Random.Range(0, PlayerPrefs.GetFloat("MaxRoutes"));
        maxDistance = PlayerPrefs.GetFloat("MaxRouteDist"); //gets the playerprefs allowing value changes to be saved
        ThisStar = this.gameObject;
    }
    public void Start() {
        CheckDistance();       //resets and starts distance check
    }


    
    public void CheckDistance() {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, maxDistance); //checks a sphere in a max size around the star


        for (int i = 0; i < maxRoutes; i++) //will loop until maxroutes is greater
        {
            if (hitColliders.Length == i) {
                break;
            }
            Star destinationStar = hitColliders[i].GetComponent<Star>(); //the current star is selected out of max routes


            if (destinationStar == null) {

                break; //if its nothing end this function

            }

            if (!starRoutes.ContainsKey(destinationStar) && destinationStar.starRoutes.Count < destinationStar.maxConnections)
            //if it doesnt contain this key and the count is less than
            { //max amount it keeps going
                float dist = Vector3.Distance(this.transform.position, destinationStar.transform.position);
                starRoutes.Add(destinationStar, dist); //works out the distance between stars in worldspace
                if (!destinationStar.starRoutes.ContainsKey(this)) { //if it doesnt contain this value add it to the list of routes
                    destinationStar.starRoutes.Add(this, dist);
                }
            }
        }
        foreach (Star route in starRoutes.Keys) {
            Collider[] Colliders = Physics.OverlapSphere(route.transform.position, 3f);
            for (int i = 0; i < Colliders.Length; i++) //every route sets up a line
            {
                //if (Colliders[i].GetComponentInChildren<LineRenderer>())
                //{                    
                //    break; //if it already has a linerenderer dont set up a new one
                //}
                //else
                //{
                LineRenderer line = Instantiate(lineRenderer, this.transform.position, Quaternion.identity, transform).GetComponent<LineRenderer>();
                line.SetPosition(0, this.transform.position); //finds the objects positions and sets the line renderer between them
                line.SetPosition(1, route.transform.position);
            }
        }
    }

    Vector3 Yup = new Vector3(0, 5, 0);
    public void OnPointerEnter(PointerEventData pointerEventData) {
        CurrentTextName = Instantiate(NamePrefab, transform.position + Yup, this.transform.rotation);

        CurrentTextName.GetComponent<TextMeshPro>().text = this.name.ToString(); //if pointer enters the stars collision it creates a text and names it
    }
    public void ExitMouse() { //some reason onpointerexit wouldnt work so had to use the eventmanager
        Destroy(CurrentTextName);
        Debug.Log("Currently Exiting " + name, this); //if it exits it deletes the text
    }
}



