﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//=====================================================================================================================================
/*      ,------.   ,---. ,--------.,--.  ,--.     ,----.   ,------.,--.  ,--.,------.,------.   ,---. ,--------. ,-----. ,------.  
        |  .--. ' /  O  \'--.  .--'|  '--'  |    '  .-./   |  .---'|  ,'.|  ||  .---'|  .--. ' /  O  \'--.  .--''  .-.  '|  .--. ' 
        |  '--' ||  .-.  |  |  |   |  .--.  |    |  | .---.|  `--, |  |' '  ||  `--, |  '--'.'|  .-.  |  |  |   |  | |  ||  '--'.' 
        |  | --' |  | |  |  |  |   |  |  |  |    '  '--'  ||  `---.|  | `   ||  `---.|  |\  \ |  | |  |  |  |   '  '-'  '|  |\  \  
        `--'     `--' `--'  `--'   `--'  `--'     `------' `------'`--'  `--'`------'`--' '--'`--' `--'  `--'    `-----' `--' '--'  */
//=====================================================================================================================================
//
//  PATH FOLLWER CLASS
//
//  Script to follow the path created by "Path Generator" class
//  Path Generator가 만든 Path를 따라가는 기능
//
//-------------------------------------------------------------------------------------------------------------------------------------
//  2022.11.29 _ KimYC1223
//=====================================================================================================================================
#region PathFollower
namespace CurvedPathGenertator {

    #region PathFollwer_RequireComponents
    [RequireComponent(typeof(Rigidbody))]
    #endregion

    #region PathFollower_Class
    public class PathFollower : MonoBehaviour {

        #region PathFollower_Variables
        [System.Serializable]
        public class EndEvent : UnityEngine.Events.UnityEvent { }

        [SerializeField]
        public EndEvent endEvent;                   // method to run when this is done moving

        public PathGenerator path;                  // choose the path to move     
        public float speed = 100f;                  // move speed
        public float distanceThreshold = 0.2f;      // distance threshold
        public float turningSpeed = 10f;            // rotation speed 
        public bool isLoop = false;                 // does it move repeatedly?
        public bool isMove = true;                  // is this moving now?
        public bool isEndEventEnable = false;       // is End event enable?

        private bool checkFlag = false;             // flag variable
        private Rigidbody targetRigidbody;          // the rigidbody of the object to move
        private GameObject target;                  // object to move;
        private Vector3 nextPath;                   // the direction the obejct will move
        private int pathIndex = 1;                  // the path index the object will move
        #endregion

        #region PathFollower_StartMethod
        //=============================================================================================================================
        // Start method
        //-----------------------------------------------------------------------------------------------------------------------------
        // init variable & position
        // 각종 변수와 position 초기화
        //=============================================================================================================================
        void Start() {
            targetRigidbody = GetComponent<Rigidbody>();

            if (path != null) { 
                target = this.gameObject;
                nextPath = path.PathList[1];
                this.transform.position = path.PathList[0];
            }
        }
        #endregion

        #region PathFollower_FixedUpdateMethod
        //=============================================================================================================================
        // Fixed update method
        //-----------------------------------------------------------------------------------------------------------------------------
        // set velocity & direction, and calculate distance
        // 속도와 방향 설정 후 거리 계산
        //=============================================================================================================================
        public void FixedUpdate() {
            if (!isMove) {
                targetRigidbody.velocity = Vector3.zero;
                return;
            }

            if (path == null) {
                isMove = false; checkFlag = false;
                Debug.LogError("no path\n경로가 없음");
                return;
            }

            if(!checkFlag) {
                checkFlag = true;
                target = this.gameObject;
                nextPath = path.PathList[1];
                this.transform.position = path.PathList[0];
            }

            //=========================================================================================================================
            //  If it is not moving, stop object and return
            //  움직이지 않는다면, 물체를 멈추고 종료
            //=========================================================================================================================
            if (!isMove) {
                targetRigidbody.velocity = new Vector3(0, 0, 0);
                return;
            }

            //=========================================================================================================================
            // Function to make objects look at the next path
            // 물체가 다음 Path를 바라보게하는 기능
            //=========================================================================================================================
            Vector3 offset = nextPath - target.transform.position;
            offset.Normalize();
            Quaternion q = Quaternion.LookRotation(offset);
            targetRigidbody.rotation =
                Quaternion.Slerp(targetRigidbody.rotation,
                                            q, turningSpeed * Time.deltaTime);

            //=========================================================================================================================
            // Function to make objects follow a path
            // 물체가 path를 따라가게 하는 기능
            //=========================================================================================================================
            offset.Normalize();
            targetRigidbody.velocity = offset * speed * Time.deltaTime;

            // calculate distance between object and next path
            // 물체와 next path 경로 사이의 거리 계산
            float Distance = Vector3.Distance(nextPath, target.transform.position);

            //=========================================================================================================================
            // If it is close enough to the next path
            // next path에 충분히 가까워졌을 경우
            //=========================================================================================================================
            if (Distance < distanceThreshold) {

                //=====================================================================================================================
                // If the end of the path list is not reached, set the next path by increase path Index
                // path 리스트의 끝에 도달하지 못했다면, path Index ++ 를 통해 next path 설정
                //=====================================================================================================================
                if (pathIndex + 1 < path.PathList.Count) {
                    nextPath = path.PathList[++pathIndex];
                } else {
                    //=================================================================================================================
                    // If the object reached end of the path list,
                    // path 리스트 끝에 도달했다면, 즉, 최종 목적지에 도달했을때
                    //=================================================================================================================
                    if (path.isClosed) {
                        //=============================================================================================================
                        // If current path is closed path, back to zero of the path list
                        // 현재 path가 닫힌 경로이면, 다시 pathList[0]을 향해 전진
                        //=============================================================================================================
                        if (isLoop) {
                            // If repeatEvent isn't null, run method.
                            // repeatEvent null이 아니면, method를 실행
                            if (endEvent != null && isEndEventEnable) {
                                endEvent.Invoke();
                            }
                            nextPath = path.PathList[0];
                            pathIndex = 0;

                        //============================================================================================================
                        // If object move once, Stop move and if endEvent isn't null, run method.
                        // 물체가 한번만 움직이면 멈추고, endEvent!=null이 아니면, method를 실행
                        //============================================================================================================
                        } else {
                            StopFollow();
                            if (endEvent != null && isEndEventEnable) {
                                endEvent.Invoke();
                            }
                        }
                    } else {
                        //=============================================================================================================
                        // If current path is open path,
                        // 현재 path가 열린 경로이면,
                        //=============================================================================================================
                        if (isLoop) {
                            //=========================================================================================================
                            // and object move repeatedly reinit position & value;
                            // 그리고 물체가 반복적으로 움직인다면, position과 변수 다시 초기화
                            //=========================================================================================================
                            nextPath = path.PathList[1];
                            pathIndex = 1;
                            this.transform.position = path.PathList[0];
                            target.transform.LookAt(path.PathList[1]);
                            // If repeatEvent isn't null, run method.
                            // repeatEvent null이 아니면, method를 실행
                            if (endEvent != null && isEndEventEnable) {
                                endEvent.Invoke();
                            }
                        } else {
                            //========================================================================================================
                            // If object move once, Stop move and if endEvent isn't null, run method.
                            // 물체가 한번만 움직이면 멈추고, endEvent!=null이 아니면, method를 실행
                            //========================================================================================================
                            StopFollow();
                            if (endEvent != null && isEndEventEnable) {
                                endEvent.Invoke();
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region PathFollower_GetPassedLengthMethod
        //=============================================================================================================================
        // Get Passed Length method
        //-----------------------------------------------------------------------------------------------------------------------------
        // Method that returns the length of the path traveled so far
        // 지금까지 지나온 경로의 길이를 반환하는 메소드
        //=============================================================================================================================
        public float GetPassedLength() {
            if (path == null) return -1;

            if (pathIndex == 1)
                return ( path.PathList[0] - this.transform.position ).magnitude;

            else if (pathIndex >= path.PathList.Count)
                return path.GetLength();

            else return path.pathLengths[pathIndex - 2] +
                          ( path.PathList[pathIndex - 1] - this.transform.position ).magnitude;
        }
        #endregion

        #region PathFollower_MovementMethod
        //=============================================================================================================================
        // Stop Follow method
        //-----------------------------------------------------------------------------------------------------------------------------
        // stop move by set isMove false
        // 정지
        //=============================================================================================================================
        public void StopFollow() {
            isMove = false;
        }

        //=============================================================================================================================
        // Start Follow method
        //-----------------------------------------------------------------------------------------------------------------------------
        // Start move by set isMove true
        // 움직임
        //=============================================================================================================================
        public void StartFollow() {
            if (path == null) return;
            isMove = true;
        }
        #endregion
    }
    #endregion
}
#endregion