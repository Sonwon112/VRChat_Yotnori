
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

public class boardSpot : UdonSharpBehaviour
{
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<VRC_Pickup>() != null)
        {
            other.GetComponent<UdonBehaviour>().SendCustomNetworkEvent(NetworkEventTarget.All, "AdjustPosChild");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.name.Contains("Dice")) return;
        if (other.GetComponent<VRC_Pickup>() != null)
        {
            if ((bool)other.GetComponent<UdonBehaviour>().GetProgramVariable("isPosed") == true) return;
            if (!other.GetComponent<VRC_Pickup>().IsHeld)
            {
                Transform targetTransform = other.transform;
                targetTransform.position = transform.position;
                targetTransform.rotation = Quaternion.Euler(new Vector3(-90, 270, 0));
                other.GetComponent<UdonBehaviour>().SetProgramVariable("isPosed", true);
                other.GetComponent<UdonBehaviour>().SendCustomNetworkEvent(NetworkEventTarget.All, "SetTrigger");
                
            }
        }
    }
}
