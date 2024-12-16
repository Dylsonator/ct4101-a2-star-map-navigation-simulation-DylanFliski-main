using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using static UnityEngine.GraphicsBuffer;


public class GalaxyGeneration : MonoBehaviour {
    public GameObject ShipPref;

    //galaxysize
    [Range(100, 1000)]
    public float MaxGalaxy;

    [Range(30, 200)]
    public float MinGalaxy;

    //planets
    private float Planets;
    [Range(100, 2000)]
    public float planetssetter;

    

    private Vector3 Spawnpos;

    //shipmovement
    public float speed = 1.0f;
    public int CurrentplanetRoute = 1;
    private int count;
    bool Moved = true;
    public Star star1;
    public Star star2;
    private float x;
    private float y;
    private float z;
    

    //NAMING
    public string[] PlanetNames;
    public string[] Phonetics;
    public string PlanetName;
    public string CurrentPhonetics;
    public List<string> UsedNames = new List<string>();

    public TextMeshProUGUI TargetStar;
    public TextMeshProUGUI CurrentStar;


    //linesetup
    public LineRenderer lines;
    [SerializeField]
    private List<GameObject> PlanetTypeList = new List<GameObject>();
    static List<Star> galaxyStarList = new List<Star>();
    public List<Star> starRoute = new List<Star>();


    //Sounds
    public AudioClip MenuNoises;
    public AudioClip ShipNoise;
    public AudioSource Cam;

    //UI
    public GameObject UI;
    private bool hiding = false;


    public void Awake() {

        string filePath = Path.Combine(Application.dataPath, "Planets.txt");
        PlanetNames = File.ReadAllLines(filePath);

        string filePath2 = Path.Combine(Application.dataPath, "Phonetics.txt");
        Phonetics = File.ReadAllLines(filePath2);

        Planets = PlayerPrefs.GetFloat("MaxPlanets");
        Spawnplanets();

    }
    private void Update() {
        if (Input.GetKeyDown(KeyCode.H)) {
            Cam.PlayOneShot(MenuNoises);
            hiding = !hiding; //swaps between true/false
        }
        hideui();
        if(star1 != null) {
            CurrentStar.text = star1.name;
        }
        if (star2 != null) {
            TargetStar.text = star2.name;
        }
       
    }
    public void hideui() {
        switch (hiding) {
            case false: UI.SetActive(false); break;
            case true: UI.SetActive(true); break; //hides/unhides ui
        }
    }

    private void setname() {
        int RandomIndex = Random.Range(0, PlanetNames.Length);
        PlanetName = PlanetNames[RandomIndex]; //setting up name of planet       
    }

    public string newname;
    public IEnumerator setPhonetics(){
        int RandomIndex2 = Random.Range(0, Phonetics.Length);
        CurrentPhonetics = Phonetics[RandomIndex2];
        newname = PlanetName + (" ") + CurrentPhonetics;

        if (UsedNames.Contains(newname)) { //loops till it gets a non used name
            StartCoroutine(setPhonetics());
        }
        yield return null;
    }
    public void Spawnplanets() {
        galaxyStarList.Clear();
        for (int i = 0; i < Planets; i++) { //looping through the planet max amount and setting them all a new location
            spawnpos();

            if (Physics.CheckSphere(Spawnpos, 4f) == false) //checking if a planet is too close
            {
                int PlanetType = Random.Range(0, PlanetTypeList.Count); //randomly chosing one of the set planets and spawning it in that location
                GameObject currentplanet = Instantiate(PlanetTypeList[PlanetType], Spawnpos, Quaternion.identity);

                setname();
                if (UsedNames.Contains(PlanetName)) {
                    StartCoroutine(setPhonetics());
                    currentplanet.name = newname;
                    UsedNames.Add(newname); //adding name to list to check later
                }
                else {
                    currentplanet.name = PlanetName;
                    UsedNames.Add(PlanetName); //adding name to list to check later
                }

                galaxyStarList.Add(currentplanet.GetComponent<Star>()); //giving it the component star (For Dijkstras)

            }
            else {
                i--; //if a planet is too close do again
            }

        }
    }

    
    public void CalculatePath()
    {
        Debug.Log("Calculated");
        if (ReturnsPath(star1, star2)) //calculates the route between first selected star and last
        {
            starRoute = FindPath(star1, star2);
            
        }
        ShipPref.transform.position = star1.transform.position;
        CurrentplanetRoute = 1;
        Moved = false;
    }
    public void StartMoveOnce() {
        ShipPref.transform.LookAt(starRoute[CurrentplanetRoute].transform.position);
        star1 = starRoute[CurrentplanetRoute]; //moves once to the next route path
        StartCoroutine(Moveship());
        Debug.Log("Moved");
    }
    public void dofullmove() {
        StartCoroutine(Wait());
              
    }
    IEnumerator Wait() {
        int i = 0;
        StartCoroutine(Moveship()); //moves all the way to final point
        ShipPref.transform.LookAt(starRoute[CurrentplanetRoute].transform.position);
        i++;
        yield return new WaitForSecondsRealtime(1.5f);
        Debug.Log(i);
        if (i != starRoute.Count) { //until the ship is at the target keep going
            StartCoroutine(Wait());
        }
        if (i == starRoute.Count) {
            i = 0;

        }
    }
   


    
    IEnumerator Moveship()
    {
        Vector3 startpos = ShipPref.transform.position;
        Vector3 endpos = starRoute[CurrentplanetRoute].transform.position;
        Cam.PlayOneShot(ShipNoise);
        float timer = 0f;
        while (timer <1f)
        {
            float time = Mathf.Pow(timer, 1);

            x = EaseClasses.easeInCubic(startpos.x, endpos.x, timer);
            y = EaseClasses.easeInCubic(startpos.y, endpos.y, timer); //lerp for the ship (From old assignment slightly changed)
            z = EaseClasses.easeInCubic(startpos.z, endpos.z, timer);

            ShipPref.transform.position = new Vector3(x, y, z);            
            timer += Time.deltaTime;
            if (timer > 1f)
            {
                Moved = true;
                CurrentplanetRoute++;
            }
            yield return null;


        }
    }
    



    public void spawnpos()
    {
        Spawnpos.x = Random.Range(-1f, 1f);
        Spawnpos.y = Random.Range(-1f, 1f); //setting up the X Y and Z for the planet to spawn in
        Spawnpos.z = Random.Range(-1f, 1f);

        Spawnpos.Normalize();

        Spawnpos *= Random.Range(MinGalaxy, MaxGalaxy); //Random range between the min and max size of the galaxy
    }
    bool ReturnsPath(Star star1, Star star2)
    {
        if (FindPath(star1, star2).Count > 0) //returning values for the star path.
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public static List<Star> FindPath(Star startStar, Star endGoalStar)
    {
        //Do the algorithm.
        //Initialise the lists to be used in the algorithm.
        //
        List<StarPath> priorityList = new List<StarPath>();

        //Construct the initial priority list from the stars in the galaxy.
        //This is what the algorithm will loop through and use to know what stars are connected to what other stars in the galaxy graph.
        foreach (Star star in galaxyStarList)
        {
            StarPath newPath = new StarPath(star, null);
            if (star == startStar)
            {
                newPath.shortestPathToStart = newPath;
                newPath.smallestDistanceToStart = 0.0f;
            }
            priorityList.Add(newPath);
        }
        priorityList = OrderPriorityListByDistance(priorityList); //Before the algorithm begins. We want to make sure the starting star is at the top priority to look at.


        //Go through the priority list and for each star look at it's connections and find the distance to the start star.
        //We know that there is a path between the start and end stars when the end node is at the start of the priority list and it has the highest priority.
        while (priorityList.Count > 0)
        {
            StarPath currentStarNode = priorityList[0];//This should be the starting star on the first iteration. This is the star with the top priority to look at.





            //Check all the stars that connect to the current star and calculate the distance to that star from the start node.
            //Once this has been done update the star path information for each node so we know the shortest path to the node.
            foreach (StarPath nextStar in GetStarPathsFromRoutesDictionary(currentStarNode.star.starRoutes, priorityList))
            {
                //Calculate the distance to the starting star by adding the distance from the start node to the current star onto the distance from the current star to the next star.
                float distance = nextStar.star.starRoutes
                    [currentStarNode.star] + currentStarNode.smallestDistanceToStart;

                //Check if this new path's distance is shorter than the current path to this star from the start node.
                if (distance < nextStar.smallestDistanceToStart)
                {
                    //Coming from the start via the current star is the shortest path to this star so update the path information.
                    nextStar.smallestDistanceToStart = distance;
                    nextStar.shortestPathToStart = currentStarNode;//This variable/line of code contains the information that tells you what path is currently the shortest one back to the starting star.
                }
            }

            //Once we reach this point the current star at the top of the priority list has fully been checked!!!
            //Now we should remove it from the priority list and then reorder it for the next iteration of the loop.
            priorityList.Remove(currentStarNode);
            if (priorityList.Count > 0)
            {
                priorityList = OrderPriorityListByDistance(priorityList);
                if (priorityList[0].star == endGoalStar && priorityList[0].smallestDistanceToStart != float.MaxValue)
                {
                    //If the star with the highest priority is now the end star then that means we have found the shortest path to the end.
                    //However the distance to the start must not be infinite else that just means we haven't found a connection.
                    //as the end star will only have the highest priority when it has the shortest distance value so we now need to backtrack and construct the path list to return.
                    List<Star> pathToEnd = new List<Star>();
                    StarPath backtrackStar = priorityList[0]; //This is currently the end goal star.
                    pathToEnd.Add(backtrackStar.star);

                    while (backtrackStar.star != startStar)
                    {
                        //We are now retracing our steps. Going through each star that is the shortest path to the current one and then adding it to the path to the end goal list.
                        backtrackStar = backtrackStar.shortestPathToStart;
                        pathToEnd.Add(backtrackStar.star);
                    }

                    //The path to the start star from the end star has been constructed. We now want to reverse it so that that the path given is from the start star to the end star instead.
                    pathToEnd.Reverse();
                    return pathToEnd;//We have the path to the goal star so we now want to return it!!!
                }
            }
        }

        //If we get out of the loop then there is no path to the end as the priority list has ran out without getting to the end star.
        return new List<Star>();
    }

    /// <summary>
    /// This function goes through all the star paths in the list and sorts them by the shortest distance path using a bubble sort.
    /// </summary>
    /// <param name="a_pathList"></param>
    /// <returns></returns>
    private static List<StarPath> OrderPriorityListByDistance(List<StarPath> a_pathList)
    {
        for (int i = 0; i < a_pathList.Count; i++)
        {
            for (int j = 0; j < a_pathList.Count - 1; j++)
            {
                StarPath first = a_pathList[j];
                StarPath second = a_pathList[j + 1];
                if (first.smallestDistanceToStart > second.smallestDistanceToStart)
                {
                    a_pathList[j] = second;
                    a_pathList[j + 1] = first;
                }
            }
        }

        return a_pathList;
    }

    /// <summary>
    /// This function gets the star path information from a star's connections.
    /// </summary>
    /// <param name="starRoutes"></param>
    /// <param name="priorityList"></param>
    /// <returns></returns>
    private static List<StarPath> GetStarPathsFromRoutesDictionary(Dictionary<Star, float> starRoutes, List<StarPath> priorityList)
    {
        List<StarPath> result = new List<StarPath>();

        foreach (KeyValuePair<Star, float> route in starRoutes)
        {
            Star star = route.Key;
            for (int i = 0; i < priorityList.Count; i++)
            {
                if (priorityList[i].star == star)
                {
                    result.Add(priorityList[i]);
                    break;
                }
            }
        }




        return result;
    }

    //Getters and setters.
    /// <summary>
    /// Set the galaxy list.
    /// </summary>
    /// <param name="stars"></param>
    public static void SetGalaxyStarList(List<Star> stars)
    {
        galaxyStarList = new List<Star>(stars);
    }

    //Structs.
    /// <summary>
    /// This class contains all the path information for a star.
    /// </summary>
    public class StarPath
    {
        public Star star;
        public StarPath shortestPathToStart;
        public float smallestDistanceToStart;

        public StarPath(Star a_current, StarPath a_shortest)
        {
            star = a_current;
            shortestPathToStart = a_shortest;
            smallestDistanceToStart = float.MaxValue;
        }
    }
}
    
