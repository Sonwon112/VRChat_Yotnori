

using System.Linq;
using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;


public class Horse_Manager : UdonSharpBehaviour
{
    [SerializeField, UdonSynced]
    public int totalCnt = 1;
    [SerializeField] public TMP_Text controlText;
    [SerializeField] public TMP_Text[] IconText;
    [SerializeField] public GameObject[] spawner;

    public GameObject spawnedHorseParent;

    [SerializeField,UdonSynced] int[] currCnt = new int[4];

    [SerializeField, UdonSynced] public int cnt0 = 0;
    [SerializeField, UdonSynced] public int cnt1 = 0;
    [SerializeField, UdonSynced] public int cnt2 = 0;
    [SerializeField, UdonSynced] public int cnt3 = 0;

    [SerializeField] private UdonBehaviour[] spawnerUdon = new UdonBehaviour[4];

    void Start()
    {
        for (int i = 0; i < spawner.Length; i++) {
            spawnerUdon[i] = spawner[i].GetComponent<UdonBehaviour>();
        }
        ResetCnt();
    }

    public void increaseTotalCnt()
    {
        Networking.SetOwner(Networking.LocalPlayer, this.gameObject);
        SendCustomNetworkEvent(NetworkEventTarget.All, "NetworkIncreaseTotalCnt");
    }

    public void NetworkIncreaseTotalCnt()
    {
        totalCnt = totalCnt + 1;
        setControlText();
        for (int i = 0; i < currCnt.Length; i++)
        {
            increaseCurrCnt(i);
            RefreshText(i);
        }
        InDeCreaseCnt();
    }

    public void decreaseTotalCnt()
    {
        Networking.SetOwner(Networking.LocalPlayer, this.gameObject);
        SendCustomNetworkEvent(NetworkEventTarget.All, "NetworkDecreaseTotalCnt");
    }

    public void NetworkDecreaseTotalCnt()
    {
        totalCnt = totalCnt - 1;
        setControlText();
        for (int i = 0; i < currCnt.Length; i++)
        {
            decreaseCurrCnt(i);
            RefreshText(i);
        }
        InDeCreaseCnt();
    }

    public void increaseCurrCnt(int id)
    {
        currCnt[id] += 1;
        spawnerUdon[id].SetProgramVariable("currCnt", currCnt[id]);
    }
    public void decreaseCurrCnt(int id)
    {
        currCnt[id] -= 1;
        spawnerUdon[id].SetProgramVariable("currCnt", currCnt[id]);
    }

    void InDeCreaseCnt() {
        cnt0 = currCnt[0];
        cnt1 = currCnt[1];
        cnt2 = currCnt[2];
        cnt3 = currCnt[3];
    }
    public void syncedCurrCnt()
    {
        currCnt[0] = cnt0;
        currCnt[1] = cnt1;
        currCnt[2] = cnt2;
        currCnt[3] = cnt3;
    }
    public void RefreshAllText()
    {
        for (int id = 0; id < IconText.Length; id++)
        {
            IconText[id].text = currCnt[id].ToString();
        }

    }


    void setControlText()
    {
        controlText.text = totalCnt.ToString();
    }

    
    void RefreshText(int id)
    {
        IconText[id].text = currCnt[id].ToString();
    }


    void ResetCnt()
    {
        setControlText();
        for (int i = 0; i < IconText.Length; i++)
        {
            currCnt[i] = totalCnt;
            spawnerUdon[i].SetProgramVariable("currCnt", currCnt[i]);
        }
        RefreshAllText();

        cnt0 = currCnt[0];
        cnt1 = currCnt[1];
        cnt2 = currCnt[2];
        cnt3 = currCnt[3];
    }

    public void ResetHorse() {
       
        SendCustomNetworkEvent(NetworkEventTarget.All, nameof(NetworkResetHorse));
    }

    public void NetworkResetHorse()
    {
        ResetCnt();
        GameObject[] horses = new GameObject[spawnedHorseParent.transform.childCount];

        for (int i = 0; i < horses.Length; i++)
        {
            horses[i] = spawnedHorseParent.transform.GetChild(i).gameObject;
        }

        foreach (GameObject horse in horses)
        {
            Networking.SetOwner(Networking.GetOwner(horse), horse);
            horse.GetComponent<UdonBehaviour>().SendCustomEvent("DestroyHorse");
            //horse.GetComponent<UdonBehaviour>().SendCustomEvent("increaseSize");
        }

        /*foreach (GameObject spawn in spawner)
        {
            if (spawn.transform.childCount > 0)
                Destroy(spawn.transform.GetChild(0).gameObject);
        }*/
    }




}
