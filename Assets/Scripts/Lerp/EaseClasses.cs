using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EaseClasses {
    public static float easeInCubic(float start, float end, float time) {
        return start + (end - start) * time * time * time;
    }
}
