
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

public class Yot : UdonSharpBehaviour
{
    [SerializeField]public GameObject[] YotObject;
    public float[] XPos;
    public float[] YPos;
    public float[] rotation;

    public float torquePow = 2f;
    public float upPow = 6f;

    [SerializeField, UdonSynced]private bool isThrowing = false;
    void Start()
    {
        
    }

    public void ThrowYot()
    {
        //Networking.SetOwner(Networking.LocalPlayer, this.gameObject);
        SendCustomNetworkEvent(NetworkEventTarget.All, "NetworkThrowYot");
    }

    public void NetworkThrowYot()
    {
        isThrowing = true;
        //Debug.Log("Throw");
        for (int i = 0; i < YotObject.Length; i++)
        {
            YotObject[i].transform.localPosition = new Vector3(XPos[i], YPos[i], 0f);
            YotObject[i].transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, rotation[i]));
            YotObject[i].GetComponent<Rigidbody>().AddForce(Vector3.up * upPow, ForceMode.Impulse);
            YotObject[i].GetComponent<Rigidbody>().AddTorque(Vector3.right * torquePow, ForceMode.Impulse);
        }
    }

    public void ResetYot()
    {
        //Networking.SetOwner(Networking.LocalPlayer, this.gameObject);
        SendCustomNetworkEvent(NetworkEventTarget.All, "NetworkResetYot");
    }

    public void NetworkResetYot()
    {
        for (int i = 0; i < YotObject.Length; i++)
        {
            YotObject[i].transform.localPosition = new Vector3(XPos[i], 2f, 0f);
            YotObject[i].transform.rotation = new Quaternion();
            //YotObject[i].GetComponent<Rigidbody>().AddForce(Vector3.up * 15f, ForceMode.VelocityChange);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.name.Contains("Yot") && isThrowing)
        {
            
            if(GetComponent<AudioSource>()!=null && !GetComponent<AudioSource>().isPlaying)
            {
                GetComponent<AudioSource>().Play();
                isThrowing = false;
            }

        }
        else
        {
            return;
        }
    }

}
