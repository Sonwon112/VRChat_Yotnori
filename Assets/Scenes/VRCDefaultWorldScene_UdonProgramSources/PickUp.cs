
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

public class PickUp : UdonSharpBehaviour
{
    GameObject spawnedParent;
    Transform CarryPos;
    [SerializeField, UdonSynced] int teamNum;
    [SerializeField, UdonSynced] bool onBoard = false;
    [SerializeField, UdonSynced] bool isAfter = false;
    [SerializeField, UdonSynced] bool isPosed = false;
    [SerializeField, UdonSynced] bool isEnd = false;


    [SerializeField, UdonSynced] public int id;
    [SerializeField] public GameObject SpawnerGameObject;
    public Vector3 originPos;
    private void Start()
    {
        CarryPos = transform.Find("CarryPos");
        //Debug.Log(id +" : "+CarryPos.position);
        spawnedParent = GameObject.Find("Spawned");

    }

    public override void OnPickup()
    {
        base.OnPickup();
        //Networking.SetOwner(Networking.LocalPlayer, this.gameObject);
        SendCustomNetworkEvent(NetworkEventTarget.All, nameof(CallPickUp));
    }


    public void CallPickUp()
    {
        GetComponent<BoxCollider>().isTrigger = false;
        GetComponent<Rigidbody>().isKinematic = false;
        transform.parent = spawnedParent.transform;
        onBoard = true;
        isPosed = false;
        if (isEnd)
        {
            isEnd = false;
            increaseSize();
        }
    }

    public void SetTrigger()
    {
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<BoxCollider>().isTrigger = true;
        isAfter = false;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.name.Contains("Dice")) return;
        if (isAfter || isEnd) return;
        if (GetComponent<VRC_Pickup>().IsHeld) return;
        if (other.GetComponent<VRC_Pickup>() != null)
        {
            if ((bool)other.GetComponent<UdonBehaviour>().GetProgramVariable("onBoard") == false) return;
            if ((bool)other.GetComponent<UdonBehaviour>().GetProgramVariable("isPosed") == true) return;
            if (other.GetComponent<VRC_Pickup>().IsHeld) return;

            UdonBehaviour target = other.GetComponent<UdonBehaviour>();
            int targetTeamNum = (int)target.GetProgramVariable("teamNum");
            //Debug.Log("targetNum : " + targetTeamNum);

            Networking.SetOwner(Networking.LocalPlayer, this.gameObject);
            Networking.SetOwner(Networking.LocalPlayer, other.gameObject);

            if (teamNum == 0 && (int)target.GetProgramVariable("id") != id)
            {
                Debug.Log(gameObject.name + "," + target.name);
                target.SetProgramVariable("isAfter", true);
                DestroyHorse();
            }
            else if (teamNum == targetTeamNum)
            {
                //Debug.Log(CarryPos.position);
                target.SetProgramVariable("isAfter", true);
                target.transform.position = CarryPos.position;
                target.transform.rotation = Quaternion.Euler(new Vector3(-90, 270, 0));
                target.SetProgramVariable("isPosed", true);
                target.SendCustomNetworkEvent(NetworkEventTarget.All, "SetTrigger");
                target.transform.parent = transform;
                target.GetComponent<VRC_Pickup>().enabled= false;
            }
            else
            {
                Debug.Log(gameObject.name + "," + target.name);
                target.SetProgramVariable("isAfter", true);
                DestroyHorse();
            }

        }
    }

    public void AdjustPosChild()
    {
        if (transform.childCount <= 1) return;
        for(int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name.Equals("CarryPos")) continue;
            transform.GetChild(i).localPosition = CarryPos.localPosition;
            transform.GetChild(i).rotation = Quaternion.Euler(new Vector3(-90, 270, 0));
            transform.GetChild(i).GetComponent<UdonBehaviour>().SendCustomNetworkEvent(NetworkEventTarget.All, nameof(AdjustPosChild));
        }
    }

    public void setTeam1()
    {
        Networking.SetOwner(Networking.LocalPlayer, this.gameObject);
        SendCustomNetworkEvent(NetworkEventTarget.All, nameof(NetworkSetTeam1));
    }

    public void NetworkSetTeam1()
    {
        if(teamNum == 1) teamNum = 0;
        else teamNum = 1;
    }


    public void setTeam2()
    {
        Networking.SetOwner(Networking.LocalPlayer, this.gameObject);
        SendCustomNetworkEvent(NetworkEventTarget.All, nameof(NetworkSetTeam2));
    }
    public void NetworkSetTeam2()
    {
        if (teamNum == 2) teamNum = 0;
        else teamNum = 2;
    }

    public void reduceSize()
    {
        transform.localScale = new Vector3(10, 10, 10);
    }

    public void increaseSize()
    {
        transform.localScale = new Vector3(50, 50, 50);
    }

    public void DestroyHorse()
    {
        //SpawnerGameObject.GetComponent<UdonBehaviour>().SendCustomEvent("DestroyedHorse");
        //Destroy(gameObject);
        if(transform.childCount > 1)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.name.Equals("CarryPos")) continue;
                GameObject target = transform.GetChild(i).gameObject;
                Networking.SetOwner(Networking.LocalPlayer, target.gameObject);
                if(target.GetComponent<UdonBehaviour>() != null)
                    target.GetComponent<UdonBehaviour>().SendCustomNetworkEvent(NetworkEventTarget.All, nameof(DestroyHorse));
            }
        }
        transform.parent = spawnedParent.transform;
        GetComponent<VRC_Pickup>().enabled = true;
        transform.localPosition = originPos;
        transform.rotation = Quaternion.Euler(new Vector3(-90, 90, 0));
        SetTrigger();
        isEnd = false;
        SendCustomEventDelayedSeconds(nameof(increaseSize), 0.5f);

    }
}
