using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class UIScript : MonoBehaviour
{
    [Range(0f, 10f)]
    public float RoutesSetter;

    [Header("Scripts")]
    public GalaxyGeneration GalaxyGenScript;

    [Header("Sliders")]
    public Slider MaxPlanets;
    public Slider MaxInner;
    public Slider MaxOuter;
    public Slider MaxRoutes;
    public Slider MaxRoutesDistance;

    [Header("Text")]
    public TextMeshProUGUI MaxPlanetsTxt;
    public TextMeshProUGUI MaxInnerTxt;
    public TextMeshProUGUI MaxOuterTxt;
    public TextMeshProUGUI MaxRoutesTxt;
    public TextMeshProUGUI MaxRoutesDistanceTxt;

    private void Awake() {
        SetValues();
        MaxPlanets.value = PlayerPrefs.GetFloat("MaxPlanets");
        MaxInner.value = PlayerPrefs.GetFloat("MaxInner");
        MaxOuter.value = PlayerPrefs.GetFloat("MaxOuter"); //making sure all the sliders are using playerprefs
        MaxRoutes.value = PlayerPrefs.GetFloat("MaxRoutes");
        MaxRoutesDistance.value = PlayerPrefs.GetFloat("MaxRouteDist"); 
        
    }
    private void Update()
    {
        Savevalues();
        Settexts();  
        SetValues();
        quit();
    }

    private void Start() {        
    }
    private void Savevalues() {
        PlayerPrefs.SetFloat("MaxPlanets", MaxPlanets.value);
        PlayerPrefs.SetFloat("MaxInner", MaxInner.value); //setting all the values of playerprefs
        PlayerPrefs.SetFloat("MaxOuter", MaxOuter.value);
        PlayerPrefs.SetFloat("MaxRoutes", MaxRoutes.value);
        PlayerPrefs.SetFloat("MaxRouteDist", MaxRoutesDistance.value);
    } 
    public void Settexts()
    {
        MaxPlanetsTxt.text = MaxPlanets.value.ToString();
        MaxInnerTxt.text = MaxInner.value.ToString();
        MaxOuterTxt.text = MaxOuter.value.ToString(); //setting the slider value to text
        MaxRoutesTxt.text = MaxRoutes.value.ToString();
        MaxRoutesDistanceTxt.text = MaxRoutesDistance.value.ToString();
    }
    public void SetValues()
    {
        GalaxyGenScript.planetssetter = PlayerPrefs.GetFloat("MaxPlanets");
        GalaxyGenScript.MinGalaxy = PlayerPrefs.GetFloat("MaxInner"); //setting player prefs so that info saves on reload
        GalaxyGenScript.MaxGalaxy = PlayerPrefs.GetInt("MaxOuter");

    }

    public void quit() {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
            Debug.Log("quit");
        }
    }
}
