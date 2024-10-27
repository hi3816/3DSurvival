using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public ItemData itemToGive;         // 플레이어가 얻을 아이템 데이터 객체
    public int quantityPerHit = 1;      // 히트(타격) 당 얻는 아이템 수량
    public int capacity;                // 아이템 최대 수용량


    public void Gather(Vector3 hitPoint, Vector3 hitNormal)
    {
        for (int i = 0; i < quantityPerHit; i++)
        {
            if (capacity <= 0) break;
            capacity -= 1;
            Instantiate(itemToGive.dropPrefab, hitPoint + Vector3.up, Quaternion.LookRotation(hitNormal, Vector3.up));

        }
    }

}
