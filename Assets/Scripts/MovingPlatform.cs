using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MovingPlatform : MonoBehaviour
{
    public float speed;
    Vector3 targetPos;

    Rigidbody2D rb;
    Vector3 moveDirection;

    public GameObject ways;
    public Transform[] wayPoints;
    int pointIndex;
    int pointCount;
    int direction = 1;

    public float waitDuration;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        wayPoints = new Transform[ways.transform.childCount];
        for (int i = 0; i < ways.gameObject.transform.childCount; i++)
        {
            wayPoints[i] = ways.transform.GetChild(i).gameObject.transform;
        }
    }

    private void Start()
    {
        pointIndex = 1;
        pointCount = wayPoints.Length;
        targetPos = wayPoints[1].transform.position;
        DirectionCalculate();
    }

    private void Update()
    {
        if (Vector2.Distance(transform.position, targetPos) < 0.05f)
        {
            NextPoint();
        }

    }

    private void FixedUpdate()
    {
        rb.velocity = moveDirection * speed;
    }

    void NextPoint()
    { 
        transform.position = targetPos;
        moveDirection = Vector3.zero;

        if (pointIndex == pointCount - 1)
        { 
            direction = -1;
        }

        if (pointIndex == 0)
        {
            direction = 1;
        }

        pointIndex += direction;
        targetPos = wayPoints[pointIndex].transform.position;

        StartCoroutine(WaitNextPoint());
    }

    IEnumerator WaitNextPoint() 
    {
        yield return new WaitForSeconds(waitDuration);
        DirectionCalculate();
    }

    void DirectionCalculate()
    { 
        moveDirection = (targetPos - transform.position).normalized;
    }

}
