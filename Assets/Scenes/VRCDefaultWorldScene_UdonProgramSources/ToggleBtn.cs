
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

public class ToggleBtn : UdonSharpBehaviour
{
    [SerializeField] public GameObject CheckMark;

    [SerializeField] private Image currImage;
    [SerializeField,UdonSynced]bool isChecked = false;
    void Start()
    {
        currImage = GetComponent<Image>();
    }

    public void toggleSelect()
    {
        //Networking.SetOwner(Networking.LocalPlayer,this.gameObject);
        SendCustomNetworkEvent(NetworkEventTarget.All, nameof(NetworkToggleSelect));
    }

    public void NetworkToggleSelect()
    {
        isChecked = !isChecked;
        CheckMark.SetActive(isChecked);
        if (isChecked)
            currImage.color = Color.gray;
        else
            currImage.color = Color.white;
    }

    

}
