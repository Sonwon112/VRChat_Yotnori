
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms;
using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

public class CalcScore : UdonSharpBehaviour
{
    [SerializeField] public TMP_Text scoreTxt;

    private int currCnt;
    void Start()
    {
        
    }

    UdonBehaviour tmp;

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("Dice")) return;
        if (other.GetComponent<VRC_Pickup>() != null) {
            if (other.GetComponent<VRC_Pickup>().IsHeld) return;
            tmp = other.GetComponent<UdonBehaviour>();
            CallCountUp();
            //SendCustomNetworkEvent(NetworkEventTarget.All, nameof(CallCountUp));
        }
    }

    public void CallCountUp()
    {
        tmp.SetProgramVariable("isEnd", true);
        tmp.SendCustomNetworkEvent(NetworkEventTarget.All, "reduceSize");
        //other.GetComponent<UdonBehaviour>().SendCustomEvent("SetTrigger");
        currCnt++;
        scoreTxt.text = currCnt.ToString();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name.Contains("Dice")) return;
        if (other.GetComponent<VRC_Pickup>() != null) {
            CallCountDown();
            //SendCustomNetworkEvent(NetworkEventTarget.All, nameof(CallCountDown));
        }
    }

    public void CallCountDown()
    {
        currCnt = currCnt <= 0 ? 0 : currCnt--;
        scoreTxt.text = currCnt.ToString();
    }

}
