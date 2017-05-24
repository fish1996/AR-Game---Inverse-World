using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassOfTarget{
    public int index = 0;
    public int whichClue;
    public Vector3 objPosition;
    public float scaleAmount;

    public ClassOfTarget()
    {

    }

    public ClassOfTarget(int i,Vector3 pos,float r)
    {
        index = i;
        objPosition = pos;
        scaleAmount = r;
    }

}
