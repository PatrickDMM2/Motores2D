//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;


//public class DisapPlatform : MonoBehaviour
//{
//    float _timeToWait = 5;
//    public float _secondsToWait = 1;
//    public float _elapsedTime = 0;
//    public SpriteRenderer SpriteRenderer;
//    public Color color1;
//    public Color color2;
//    bool Appear = false;

//    void Start()
//    {
//        StartCoroutine(WaitTime());
//    }

//    void Update()
//    {
        
//    }

//    IEnumerator WaitTime() 
//    {
//        float currentTime = 0;
//        var color = SpriteRenderer.color;
//        color.a = 1;


//        while (currentTime < _timeToWait) 
//        {
//            currentTime += Time.deltaTime;
//            if (!Appear)
//            { 
//                color.a = Mathf.Lerp(1,0, currentTime / _timeToWait);
//            }
//            else
//            { 
//                color.a Mathf.Lerp(0, 1, currentTime / _timeToWait);
//            }
//            SpriteRenderer.color = color;
//            yield return null;
//        }
//        color.a = 0;
//        SpriteRenderer.color = color;
//        Appear = !Appear;
//        Debug.Log("Time Out");
//        yield return null;
//    }
//}
