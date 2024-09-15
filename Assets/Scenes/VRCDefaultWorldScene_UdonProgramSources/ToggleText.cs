
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

public class ToggleText : UdonSharpBehaviour
{
    [SerializeField, UdonSynced] bool state = false;
    public void toggleText()
    {
        Networking.SetOwner(Networking.LocalPlayer, this.gameObject);
        SendCustomNetworkEvent(NetworkEventTarget.All, nameof(NetworkingToggleText));
    }

    public void NetworkingToggleText()
    {
        state = !state;
        gameObject.SetActive(state);
    }
}
