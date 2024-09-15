
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Crmf;
using System.Collections;
using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

public class CardControl : UdonSharpBehaviour
{
    public string[] cardTextArr;
    [SerializeField] public TMP_Text cardText;

    [SerializeField] private Animator cardAnimator;

    [SerializeField, UdonSynced] int pos;
    [SerializeField, UdonSynced] string text;
    void Start()
    {
        cardAnimator = GetComponent<Animator>();
    }

    public void OpenCard() {
        Networking.SetOwner(Networking.LocalPlayer,this.gameObject);
        SendCustomNetworkEvent(NetworkEventTarget.All, nameof(NetworkOpenCard));
    }

    public void NetworkOpenCard()
    {
        pos = Random.Range(0, cardTextArr.Length);
        text = cardTextArr[pos].Replace("\\n", "\n");
        cardAnimator.SetBool("Open", true);
    }

    public void SelectCard()
    {
        //Networking.SetOwner(Networking.LocalPlayer, this.gameObject);
        SendCustomNetworkEvent(NetworkEventTarget.All, nameof(NetworkSelectCard));
    }

    public void NetworkSelectCard()
    {
        cardAnimator.SetBool("Brightness", true);
        
        cardText.text = text;
        //Debug.Log(randPos);
        cardAnimator.SetBool("Brightness", false);
    }
    
    public void CloseCard()
    {
        Networking.SetOwner(Networking.LocalPlayer, this.gameObject);
        SendCustomNetworkEvent(NetworkEventTarget.All, nameof(NetworkCloseCard));
    }

    public void NetworkCloseCard()
    {
        cardAnimator.SetBool("Open", false);
    }

    public void TextReset()
    {
        cardText.text = "?";
    }

}
