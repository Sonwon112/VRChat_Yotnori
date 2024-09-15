
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

public class Horse_Spawner : UdonSharpBehaviour
{
    public int id;
    public GameObject Horse;
    public GameObject Manager;

    [SerializeField, UdonSynced] public int currCnt;

    private string varName;
    private UdonBehaviour ManagerUdon;
    [SerializeField, UdonSynced]private int teamNum;

    private void Start()
    {
        varName = "cnt"+id;
        ManagerUdon = Manager.GetComponent<UdonBehaviour>();
    }

    private void Update()
    {
        
        if(transform.childCount == 0 && teamNum != 0)
        {   
            if(currCnt > 0)
            {
                Networking.SetOwner(Networking.LocalPlayer, this.gameObject);
                SendCustomNetworkEvent(NetworkEventTarget.All, nameof(NetworkInstantiateHorse));
            }            
        }
    }

    public void NetworkInstantiateHorse()
    {
        GameObject horse = Instantiate(Horse, transform.position, Horse.transform.rotation, transform);
        UdonBehaviour horseUdon = horse.GetComponent<UdonBehaviour>();
        currCnt--;

        updateCnt();

        switch (teamNum)
        {
            case 1:
                horseUdon.SendCustomNetworkEvent(NetworkEventTarget.All,"setTeam1");
                break;
            case 2:
                horseUdon.SendCustomNetworkEvent(NetworkEventTarget.All, "setTeam2");
                break;
        }

        horseUdon.SetProgramVariable("id", id);
        horseUdon.SetProgramVariable("SpawnerGameObject", gameObject);
    }

    public void DestroyedHorse()
    {
        currCnt++;
        updateCnt();
    }

    public void setTeam1()
    {
        if(teamNum == 1)
        {
            teamNum = 0;
        }
        else
        {
            teamNum = 1;
        }
        
    }
    public void setTeam2()
    {
        if (teamNum == 2)
        {
            teamNum = 0;
        }
        else
        {
            teamNum = 2;
        }
    }

    void updateCnt()
    {
        ManagerUdon.SetProgramVariable(varName, currCnt);
        ManagerUdon.SendCustomNetworkEvent(NetworkEventTarget.All, "syncedCurrCnt");
        ManagerUdon.SendCustomNetworkEvent(NetworkEventTarget.All, "RefreshAllText");
    }
}
