# 윷놀이 월드

### 개요

- 프로젝트 명 : 추석 컨텐츠 용 윷놀이 월드
- 제작 인원 및 역할 : 4인 ( 역할 - 리소스 탐색 및 제작, 월드 제작, 기능 구현, 테스트 )
- 제작 기간 : 2024. 08. 25 ~ 2024. 09. 18
- 제작 동기 :
    
     추석 컨텐츠를 위해 4명의 크리에이터가 함께 VRChat월드에서 윷놀이를 진행하기 위해 VR 월드를 제작하게 되었습니다.
    

### 사용 기술 및 도구

- Unity를 통해서 전반적인 월드를 제작 및 구성
- VRChat에서 동작을 하여야하기 때문에 VRChat에서 제공하는 VRC SDK의 C# 기반의 UdonSharpScript(U#)을 사용하여 월드의 기능과 멀티플레이 환경을 구현하였습니다.

### 제작 과정

- 기획 회의 과정에서 기존 윷놀이 규칙 뿐만 아니라 카드를 뽑는 구간을 두어 기존의 윷놀이와의 차별점을 두기로 하였습니다.
- 제작 전 설계과정에서 윷을 어떻게 던질지, 말을 윷놀이 칸에서 어떻게 움직이게 할 것인지, 전체적인 윷놀이 진행 상황과, 윷을 던졌을 때 결과를 시청자들에게 어떻게 표시할 것인지를 고려하여야 했고 뿐만 아니라 카드 뽑기는 어떻게 구현할 것인지도 고려하여야 했습니다.
- 우선 시청자들에게 전체적인 말의 흐름과 윷을 편리하게 보여주기 위해서 추가적인 카메라에 Output을 텍스쳐에 담아 화면 역할을 수행할 Plane에 송출하여 실시간으로 전체적인 흐름과 윷의 결과값을 보여주는 방향으로 기능을 구현하였습니다.

![스크린샷 2024-09-21 200940.png](img/%25EC%258A%25A4%25ED%2581%25AC%25EB%25A6%25B0%25EC%2583%25B7_2024-09-21_200940.png)

![스크린샷 2024-09-21 200953.png](img/%25EC%258A%25A4%25ED%2581%25AC%25EB%25A6%25B0%25EC%2583%25B7_2024-09-21_200953.png)

![스크린샷 2024-09-21 201019.png](img/%25EC%258A%25A4%25ED%2581%25AC%25EB%25A6%25B0%25EC%2583%25B7_2024-09-21_201019.png)
<div style="display:flex">
	<img src = "img/%25EC%258A%25A4%25ED%2581%25AC%25EB%25A6%25B0%25EC%2583%25B7_2024-09-21_201047.png"></img>
	<img src = "img/%25EC%258A%25A4%25ED%2581%25AC%25EB%25A6%25B0%25EC%2583%25B7_2024-09-21_201054.png"></img>	
</div>
- 윷을 던지는 것은 VRChat을 활용하였지만, 풀트래킹이 아닌 PC를 통해 플레이하는 것을 고려하여 버튼을 사용하여 던지는 것을 결정하였고, 단순히 위로 힘을 가하는 것이 아닌 회전하는 힘도 같이 가하여서 실제 윷놀이처럼 윷이 회전하게 구현하였습니다.

![스크린샷 2024-09-21 201937.png](img/%25EC%258A%25A4%25ED%2581%25AC%25EB%25A6%25B0%25EC%2583%25B7_2024-09-21_201937.png)

- 말을 움직일 때 같은 팀의 말을 겹칠 수 있게, 다른팀의 말은 원래 위치로 이동하게끔 구현을 하였으며, VRChat에서 제공하는 멀티플레이 구현을 위한 SendCustomNetworkEvent를 사용하였습니다.
<div style="display:flex;">
	<img src = "img/%25EC%258A%25A4%25ED%2581%25AC%25EB%25A6%25B0%25EC%2583%25B7_2024-09-21_202013.png"></img>
	<img src = "img/%25EC%258A%25A4%25ED%2581%25AC%25EB%25A6%25B0%25EC%2583%25B7_2024-09-21_202058.png"></img>	
</div>

- 카드 뽑기 기능 구현을 위해서 애니메이션을 활용하였고, UI에서 Mask 컴포넌트를 통해 출력 영역을 제한하고, Emissive Material과 PostProcessing을 활용하여 뽑기를 조금 더 효과적으로 표현하였습니다.
<div style="display:flex">
	<img src = "img/%25EC%258A%25A4%25ED%2581%25AC%25EB%25A6%25B0%25EC%2583%25B7_2024-09-21_202242.png"></img>
	<img src = "img/%25EC%258A%25A4%25ED%2581%25AC%25EB%25A6%25B0%25EC%2583%25B7_2024-09-21_202236.png"></img>
	<img src = "img/%25EC%258A%25A4%25ED%2581%25AC%25EB%25A6%25B0%25EC%2583%25B7_2024-09-21_202249.png"></img>
</div>

### 핵심 코드

VRChat에서 제공하는 SDK에서는 C# Script를 Component에 직접 참조하는 것이 아닌, C# Script에 작성한 함수를 UdonBehaviour이라는 Component에서 호출하는 방식으로 진행이 되어서 SendCustomEvent와 SendCustomNetworkEvent에 함수명을 전달하는 코드로 구성됩니다.

- 윷을 던지는 코드

```csharp
// UdonBehaviour에서 호출하게 되는 함수
public void ThrowYot()
{
     //Networking.SetOwner(Networking.LocalPlayer, this.gameObject);
     SendCustomNetworkEvent(NetworkEventTarget.All, "NetworkThrowYot");
}

// 멀티플레이 환경에서 동일하게 동작해야하기 때문에 SendCustomeNetworkEvent에서 호출
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
```

- 말을 겹치게하는 코드, 삭제하는 코드

```csharp
        if (other.name.Contains("Dice")) return; // VRC에서 제공하는 sdk에서는 tag를 사용하지 못한다
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
						
						// 팀이 선택 되지 않고, 동일한 말인 경우
            if (teamNum == 0 && (int)target.GetProgramVariable("id") != id)
            {
                Debug.Log(gameObject.name + "," + target.name);
                target.SetProgramVariable("isAfter", true);
                DestroyHorse();
            }
            // 같은 팀인 경우
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
            // 다른팀의 경우
            else
            {
                Debug.Log(gameObject.name + "," + target.name);
                target.SetProgramVariable("isAfter", true);
                DestroyHorse();
            }

        }
```

- 카드뽑기

```csharp
    // 플레이어가 카드 뽑기 버튼을 눌렀을때 호출되는 함수
    public void OpenCard() {
        Networking.SetOwner(Networking.LocalPlayer,this.gameObject);
        SendCustomNetworkEvent(NetworkEventTarget.All, nameof(NetworkOpenCard));
    }

    public void NetworkOpenCard()
    {
        pos = Random.Range(0, cardTextArr.Length);
        text = cardTextArr[pos].Replace("\\n", "\n");
        cardAnimator.SetBool("Open", true); // 카드 표시화면에 카드가 나타나게하는 애니메이션 실행
    }
		
		// 나타나는 애니메이션 실행 후 종료시에 호출되어 텍스트를 변경하는 함수
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
    
    // 일정 시간 동안 표시 후 다시 카드를 다시 사라지게 만드는 함수
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
```

### 제작하면서 어려웠던 점 및 새롭게 알게된 점

- 카메라에 output을 텍스쳐와 연결하여 표시되는 화면 구현이 가능하다는 것을 알게 되었고, 추후 게임을 만들거나, 컨텐츠를 만들때 자주 활용될 것 같습니다.
- Post Processing에서 처음으로 Bloom 효과를 사용하여 보았고, 추후에 빛 효과가 필요한 프로젝트에서 Emissive Material과 적절히 결합하여 잘 활용할 수 있을 것 같습니다.
- VRChat에서 제공하는 U#만을 사용하여야 하기 때문에 포톤 서버자체 기능을 활용하는 것이 아닌 SendCustomeNetworkEvent를 사용하여야 했기에 원하는 기능을 구현하는 것이 까다로웠습니다.

### 플레이 영상

[https://youtu.be/ZmRc_LoigdY?t=9607](https://youtu.be/ZmRc_LoigdY?t=9607)
