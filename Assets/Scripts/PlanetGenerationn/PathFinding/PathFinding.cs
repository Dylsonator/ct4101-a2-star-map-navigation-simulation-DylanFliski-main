using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    bool ReturnsPath(Star star1, Star star2) {
        if (DijkstraSimplified.FindPath(star1, star2).Count > 0) {
            return true;
        }
        else {
            return false;
        }
    }
}
