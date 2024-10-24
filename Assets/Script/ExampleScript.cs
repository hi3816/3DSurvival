using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleScript : MonoBehaviour
{
    private AnimationCurve curve;

    private void Start()
    {
        // 새로운 AnimationCurve 생성
        curve = new AnimationCurve();

        // 키프레임 추가 (시간, 값)
        curve.AddKey(0f, 0f);
        curve.AddKey(1f, 1f);
    }

    private void Update()
    {
        // 시간에 따라 값을 보간하여 출력
        float time = Time.time;
        float value = curve.Evaluate(time);
        Debug.Log("Time: " + time + ", Value: " + value);
    }
}
