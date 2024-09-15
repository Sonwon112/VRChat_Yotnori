using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlHorse : MonoBehaviour
{
    public void DestroyHorse()
    {
        GameObject[] horses = GameObject.FindGameObjectsWithTag("Horse");
        foreach (GameObject horse in horses)
        {
            Destroy(horse);
        }
    }
}
