using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

[System.Serializable]
public class EndEvent : UnityEngine.Events.UnityEvent<GameObject> { }

public class PathFollower : MonoBehaviour
{
    public EventArgs endEvent;
    public PathGenerator path;
    public float speed = 100f;
    public float turningSpeed = 10f;
    public bool isLoop = false;
    public bool isMove = true;

    private Rigidbody TargetRigidbody;
    private GameObject Target;
    private GameObject NextAngle;
    private int AngleStep = 1;

    

    void Start()
    {
        TargetRigidbody = GetComponent<Rigidbody>();
        if (path == null) {
            Debug.LogError("경로가 없음");
        }
        Target = this.gameObject;
        NextAngle = path.PathList[1];
        this.transform.position = path.PathList[0].transform.position;
        if (NextAngle == null)
            Debug.Log("gg");
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isMove) {
            TargetRigidbody.velocity = new Vector3(0, 0, 0);
            return;
        }
        // =====================================================================
        //  자동차가 가이드를 바라보게하는 기능
        // =====================================================================
        Vector3 offset = NextAngle.transform.position - Target.transform.position;
        offset.Normalize();
        Quaternion q = Quaternion.LookRotation(offset);
        TargetRigidbody.rotation =
            Quaternion.Slerp(TargetRigidbody.rotation,
                                        q, turningSpeed * Time.deltaTime);


        // =====================================================================
        //  자동차가 가이드를 따라가게 하는 기능
        // =====================================================================
        offset.Normalize();
        TargetRigidbody.velocity = offset * speed * Time.deltaTime;

        // 두 거리 계산
        float Distance = Vector3.Distance(NextAngle.transform.position,
                                          Target.transform.position);

        // =====================================================================
        //  가이드에 가까워 졌을 경우
        // =====================================================================
        if (Distance < 0.2f) {
            
            if(AngleStep >= path.PathList.Count) {
                if (path.isClosed) {
                    NextAngle = path.PathList[0];
                    AngleStep = 0;
                } else {
                    if(isLoop) {
                        NextAngle = path.PathList[1];
                        AngleStep = 1;
                        this.transform.position = path.PathList[0].transform.position;
                        Target.transform.LookAt(path.PathList[1].transform);
                    } else {
                        StopFollow();
                    }
                }
            } else {
                NextAngle = path.PathList[AngleStep++];
            } 
        }
    }


    public void StopFollow() {
        isMove = false;
    }

    public void StartFollow() {
        isMove = true;
    }
}
