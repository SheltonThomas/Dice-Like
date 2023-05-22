using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FaceBehavior : MonoBehaviour
{
    public string Color;
    public void SayFaceColor()
    {
        Debug.Log(Color);
    }
}
