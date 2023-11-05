//=========================================================================================================================================
//      ,------.   ,---. ,--------.,--.  ,--.     ,----.   ,------.,--.  ,--.,------.,------.   ,---. ,--------. ,-----. ,------.
//      |  .--. ' /  O  \'--.  .--'|  '--'  |    '  .-./   |  .---'|  ,'.|  ||  .---'|  .--. ' /  O  \'--.  .--''  .-.  '|  .--. '
//      |  '--' ||  .-.  |  |  |   |  .--.  |    |  | .---.|  `--, |  |' '  ||  `--, |  '--'.'|  .-.  |  |  |   |  | |  ||  '--'.'
//      |  | --' |  | |  |  |  |   |  |  |  |    '  '--'  ||  `---.|  | `   ||  `---.|  |\  \ |  | |  |  |  |   '  '-'  '|  |\  \
//      `--'     `--' `--'  `--'   `--'  `--'     `------' `------'`--'  `--'`------'`--' '--'`--' `--'  `--'    `-----' `--' '--'
//
//=========================================================================================================================================
//
//  PATH FOLLWER CLASS
//
//  Script to follow the path created by "Path Generator" class
//  Path Generator가 만든 Path를 따라가는 기능
//
//-----------------------------------------------------------------------------------------------------------------------------------------
//  2023.11.04 _ KimYC1223
//=========================================================================================================================================
using UnityEngine;

#region PathFollower

namespace CurvedPathGenertator
{
    #region PathFollwer_RequireComponents

    /// <summary>
    /// Script to follow the path created by <see cref="PathGenerator" /> class
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class PathFollower : MonoBehaviour
    {
        #region PathFollower_Variables

        /// <summary>
        /// method to run when this is done moving
        /// </summary>
        public UnityEngine.Events.UnityEvent EndEvent;

        /// <summary>
        /// choose the path to move
        /// </summary>
        public PathGenerator Generator;

        /// <summary>
        /// move speed
        /// </summary>
        public float Speed = 100f;

        /// <summary>
        /// distance threshold
        /// </summary>
        public float DistanceThreshold = 0.2f;

        /// <summary>
        /// rotation speed
        /// </summary>
        public float TurningSpeed = 10f;

        /// <summary>
        /// does it move repeatedly?
        /// </summary>
        public bool IsLoop = false;

        /// <summary>
        /// is this moving now?
        /// </summary>
        public bool IsMove = true;

        /// <summary>
        /// is End event enable?
        /// </summary>
        public bool IsEndEventEnable = false;

        /// <summary>
        /// flag variable
        /// </summary>
        private bool checkFlag = false;

        /// <summary>
        /// the rigidbody of the object to move
        /// </summary>
        private Rigidbody targetRigidbody;

        /// <summary>
        /// object to move
        /// </summary>
        private GameObject target;

        /// <summary>
        /// the destination which the object will move
        /// </summary>
        private Vector3 nextPath;

        /// <summary>
        /// the path index the object will move
        /// </summary>
        private int pathIndex = 1;

        #endregion PathFollower_Variables

        #region PathFollower_StartMethod

        //=================================================================================================================================
        // Start method
        //---------------------------------------------------------------------------------------------------------------------------------
        // init variable & position
        // 각종 변수와 position 초기화
        //=================================================================================================================================
        /// <summary>
        /// init variable & position
        /// </summary>
        private void Start()
        {
            targetRigidbody = GetComponent<Rigidbody>();

            if ( Generator != null )
            {
                target = this.gameObject;
                nextPath = Generator.PathList[1];
                this.transform.position = Generator.PathList[0];
            }
        }

        #endregion PathFollower_StartMethod

        #region PathFollower_FixedUpdateMethod

        //=================================================================================================================================
        // Fixed update method
        //---------------------------------------------------------------------------------------------------------------------------------
        // set velocity & direction, and calculate distance
        // 속도와 방향 설정 후 거리 계산
        //=================================================================================================================================
        /// <summary>
        /// set velocity & direction, and calculate distance
        /// </summary>
        public void FixedUpdate()
        {
            //=============================================================================================================================
            //  If it is not moving, stop object and return
            //  움직이지 않는다면, 물체를 멈추고 종료
            //=============================================================================================================================
            if ( !IsMove )
            {
                targetRigidbody.velocity = Vector3.zero;
                return;
            }

            if ( Generator == null )
            {
                IsMove = false; checkFlag = false;
                Debug.LogError("no path");
                return;
            }

            if ( !checkFlag )
            {
                checkFlag = true;
                target = this.gameObject;
                nextPath = Generator.PathList[1];
                this.transform.position = Generator.PathList[0];
            }

            //=============================================================================================================================
            // Function to make objects look at the next path
            // 물체가 다음 Path를 바라보게하는 기능
            //=============================================================================================================================
            Vector3 offset = nextPath - target.transform.position;
            offset.Normalize();
            Quaternion q = Quaternion.LookRotation(offset);
            targetRigidbody.rotation =
                Quaternion.Slerp(targetRigidbody.rotation, q, TurningSpeed * Time.deltaTime);

            //=============================================================================================================================
            // Function to make objects follow a path
            // 물체가 path를 따라가게 하는 기능
            //=============================================================================================================================
            offset.Normalize();
            targetRigidbody.velocity = Speed * Time.deltaTime * offset;

            // calculate distance between object and next path
            // 물체와 next path 경로 사이의 거리 계산
            float Distance = Vector3.Distance(nextPath, target.transform.position);

            //=============================================================================================================================
            // If it is close enough to the next path
            // next path에 충분히 가까워졌을 경우
            //=============================================================================================================================
            if ( Distance < DistanceThreshold )
            {
                //=========================================================================================================================
                // If the end of the path list is not reached, set the next path by increase path Index
                // path 리스트의 끝에 도달하지 못했다면, path Index ++ 를 통해 next path 설정
                //=========================================================================================================================
                if ( pathIndex + 1 < Generator.PathList.Count )
                {
                    nextPath = Generator.PathList[++pathIndex];
                }
                else
                {
                    //=====================================================================================================================
                    // If the object reached end of the path list,
                    // path 리스트 끝에 도달했다면, 즉, 최종 목적지에 도달했을때
                    //=====================================================================================================================
                    if ( Generator.IsClosed )
                    {
                        //=================================================================================================================
                        // If current path is closed path, back to zero of the path list
                        // 현재 path가 닫힌 경로이면, 다시 pathList[0]을 향해 전진
                        //=================================================================================================================
                        if ( IsLoop )
                        {
                            // If repeatEvent isn't null, run method.
                            // repeatEvent null이 아니면, method를 실행
                            if ( EndEvent != null && IsEndEventEnable )
                            {
                                EndEvent.Invoke();
                            }
                            nextPath = Generator.PathList[0];
                            pathIndex = 0;

                            //=============================================================================================================
                            // If object move once, Stop move and if endEvent isn't null, run method.
                            // 물체가 한번만 움직이면 멈추고, endEvent!=null이 아니면, method를 실행
                            //=============================================================================================================
                        }
                        else
                        {
                            StopFollow();
                            if ( EndEvent != null && IsEndEventEnable )
                            {
                                EndEvent.Invoke();
                            }
                        }
                    }
                    else
                    {
                        //=================================================================================================================
                        // If current path is open path,
                        // 현재 path가 열린 경로이면,
                        //=================================================================================================================
                        if ( IsLoop )
                        {
                            //=============================================================================================================
                            // and object move repeatedly reinit position & value;
                            // 그리고 물체가 반복적으로 움직인다면, position과 변수 다시 초기화
                            //=============================================================================================================
                            nextPath = Generator.PathList[1];
                            pathIndex = 1;
                            this.transform.position = Generator.PathList[0];
                            target.transform.LookAt(Generator.PathList[1]);
                            // If repeatEvent isn't null, run method.
                            // repeatEvent null이 아니면, method를 실행
                            if ( EndEvent != null && IsEndEventEnable )
                            {
                                EndEvent.Invoke();
                            }
                        }
                        else
                        {
                            //============================================================================================================
                            // If object move once, Stop move and if endEvent isn't null, run method.
                            // 물체가 한번만 움직이면 멈추고, endEvent!=null이 아니면, method를 실행
                            //============================================================================================================
                            StopFollow();
                            if ( EndEvent != null && IsEndEventEnable )
                            {
                                EndEvent.Invoke();
                            }
                        }
                    }
                }
            }
        }

        #endregion PathFollower_FixedUpdateMethod

        #region PathFollower_GetPassedLengthMethod

        //=================================================================================================================================
        // Get Passed Length method
        //---------------------------------------------------------------------------------------------------------------------------------
        // Method that returns the length of the path traveled
        // 지금까지 지나온 경로의 길이를 반환하는 메소드
        //=================================================================================================================================
        /// <summary>
        /// Method that returns the length of the path traveled
        /// </summary>
        /// <returns>lengh of path traveled</returns>
        public float GetPassedLength()
        {
            if ( Generator == null ) return -1;

            if ( pathIndex == 1 )
            {
                return ( Generator.PathList[0] - this.transform.position ).magnitude;
            }
            else if ( pathIndex >= Generator.PathList.Count )
            {
                return Generator.GetLength();
            }
            else
            {
                return Generator.PathLengths[pathIndex - 2] + ( Generator.PathList[pathIndex - 1] - this.transform.position ).magnitude;
            }
        }

        #endregion PathFollower_GetPassedLengthMethod

        #region PathFollower_MovementMethod

        //=================================================================================================================================
        // Stop Follow method
        //---------------------------------------------------------------------------------------------------------------------------------
        // stop move by set isMove false
        // 정지
        //=================================================================================================================================
        /// <summary>
        /// stop following path
        /// </summary>
        public void StopFollow()
        {
            IsMove = false;
        }

        //=================================================================================================================================
        // Start Follow method
        //---------------------------------------------------------------------------------------------------------------------------------
        // Start move by set isMove true
        // 움직임
        //=================================================================================================================================
        /// <summary>
        /// start following path
        /// </summary>
        public void StartFollow()
        {
            if ( Generator == null )
            {
                return;
            }
            IsMove = true;
        }

        #endregion PathFollower_MovementMethod
    }

    #endregion PathFollwer_RequireComponents
}

#endregion PathFollower