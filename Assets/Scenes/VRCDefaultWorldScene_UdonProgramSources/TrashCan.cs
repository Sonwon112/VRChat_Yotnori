
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

public class TrashCan : UdonSharpBehaviour
{
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("Dice")) return;
        if (other.GetComponent<VRC_Pickup>() != null)
        {
            other.GetComponent<UdonBehaviour>().SendCustomNetworkEvent(NetworkEventTarget.All,"DestroyHorse");
        }
    }
}
