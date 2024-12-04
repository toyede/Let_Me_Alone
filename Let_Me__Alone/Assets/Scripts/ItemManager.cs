using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    private Picups picups; // 맵 데이터를 관리하는 MapCreator 스크립트 참조
    public bool haveHighPassTicket = false;
    public bool UsedHighPassTicket = false;
    public bool haveElectricShockGun = false;

    void Start()
    {
        // 씬에서 MapCreator와 GameSystem 참조 가져오기
        picups = FindObjectOfType<Picups>();
    }

    // Update is called once per frame
    void Update()
    {
        if(haveHighPassTicket)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                haveHighPassTicket = false;
                picups.UseHighPassTicket(); // 통행 티켓 사용
                UsedHighPassTicket = true;
            }

        }

        if (UsedHighPassTicket)
        {
            if(Input.GetKeyDown(KeyCode.N))
            {
                UsedHighPassTicket = false;
                picups.ResetWeightsAfterOneDay();
            }
        }
    }
}
