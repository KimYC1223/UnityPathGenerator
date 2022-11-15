using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

//=========================================================================================================================
//
//  PATH GENERATOR CLASS
//
//  Script to make followable path the based on Bézier curve
//  Path Generator가 만든 Path를 따라가는 기능
//
//-------------------------------------------------------------------------------------------------------------------------
//  2020.08.30 _ KimYC1223
//=========================================================================================================================

namespace CurvedPathGenertator {
    [System.Serializable]
    public class PathGenerator : MonoBehaviour {
        public bool isClosed = true;            // is this path closed?
        public bool isLiveRender = true;        // is draw the path in runtime?
        public int PathDensity = 30;            // Density of guide objects between Flags

        public int EditMode = 0;   // (Editor) is individaul control mode?


        public List<Vector3> PathList = new List<Vector3>();        // List of Path pos
        public List<float> pathLengths = new List<float>();

        [SerializeField]
        public List<Vector3> FlagList = new List<Vector3>();        // List of Flag pos
        [SerializeField]
        public List<Vector3> AngleList = new List<Vector3>();       // List of Angle pos

        public List<Vector3> FlagList_Local = new List<Vector3>();  // List of Flag pos
        public List<Vector3> AngleList_Local = new List<Vector3>(); // List of Angle pos


        //=================================================================================================================
        // Awake method
        //-----------------------------------------------------------------------------------------------------------------
        // init variable & position
        // 각종 변수와 position 초기화
        //=================================================================================================================
        public void Awake() {
            UpdatePath();
        }

        //=================================================================================================================
        // UpdatePath method
        //-----------------------------------------------------------------------------------------------------------------
        // Calculate & Generate Path
        // 경로 계산 및 생성
        //=================================================================================================================
        public void UpdatePath() {
            PathList = new List<Vector3>();
            pathLengths = new List<float>();

            //=============================================================================================================
            //  check path density is bigger than 1
            //  path density가 1보다 큰 지 확인
            //=============================================================================================================
            if (PathDensity < 2) {
#if UNITY_EDITOR
                Debug.LogError("Path Density is too small. (must >= 2)");
                UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER
            Application.OpenURL("about:blank");
#else
            Application.Quit();
#endif
            }

            //=============================================================================================================
            //  Generate path based on Bézier curve between Flags
            //  Flag들 사이에 베지어 곡선 기반의 path 생성
            //=============================================================================================================
            for (int i = 0; i < FlagList_Local.Count; i++) {
                //=========================================================================================================
                //  Select Flags
                //  Flag 선택
                //=========================================================================================================
                Vector3 startPoint = FlagList_Local[i];
                Vector3 middlePoint = new Vector3();
                Vector3 endPoint = new Vector3();
                if (i == FlagList_Local.Count - 1) {
                    if (isClosed) {
                        middlePoint = AngleList_Local[i];
                        endPoint = FlagList_Local[0];
                    } else {
                        break;
                    }
                } else {
                    middlePoint = AngleList_Local[i];
                    endPoint = FlagList_Local[i + 1];
                }

                //=========================================================================================================
                //  Calculate Bézier curve
                //  베지어 커브 계산
                //=========================================================================================================
                for (int j = 0; j < PathDensity; j++) {
                    float t = (float)j / PathDensity;

                    Vector3 curve = ( 1f - t ) * ( 1f - t ) * startPoint +
                                   2 * ( 1f - t ) * t * middlePoint +
                                   t * t * endPoint;
                    PathList.Add(curve);
                    if (PathList.Count == 2) {
                        float length = ( PathList[0] - curve ).magnitude;
                        pathLengths.Add(length);
                    } else if (PathList.Count > 2) {
                        float length = ( PathList[PathList.Count - 2] - curve ).magnitude;
                        pathLengths.Add(pathLengths[pathLengths.Count - 1] + length);
                    }
                }

            }

            // 닫힌 경로인 경우 마지막 Flag를 Path리스트에 넣어줌
            if (isClosed)
                PathList.Add(FlagList_Local[0]);
            else
                PathList.Add(FlagList_Local[FlagList_Local.Count - 1]);

            float l = ( PathList[PathList.Count - 2] - PathList[PathList.Count - 1] ).magnitude;
            pathLengths.Add(pathLengths[pathLengths.Count - 1] + l);

            ////=============================================================================================================
            ////  Debug Obejct ( Flag, StartFlag, Angle ) visual control
            ////  디버그 오브젝트 (Flag, StartFlag, Angle) 비주얼 컨트롤
            ////=============================================================================================================
            //if (!isDebugObject) {

            //}
        }

        //=================================================================================================================
        // GetLength method
        //-----------------------------------------------------------------------------------------------------------------
        // return path Length
        // 만들어진 경로의 길이를 리턴
        //=================================================================================================================
        public float GetLength() {
            if (pathLengths != null || pathLengths.Count > 0)
                return pathLengths[pathLengths.Count - 1];
            else return 0;
        }


        //=================================================================================================================
        // Update method
        //-----------------------------------------------------------------------------------------------------------------
        // run code in every frame
        // 만들어진 경로의 길이를 리턴
        //=================================================================================================================
        public void Update() {
            if (isLiveRender)
                UpdatePath();
        }


        //=================================================================================================================
        // OnDrawGizmosSelected method
        //-----------------------------------------------------------------------------------------------------------------
        // A method that draws a gizmo when an object is selected
        // 오브젝트를 선택했을 때, 기즈모를 그리는 메소드
        //=================================================================================================================
        public void OnDrawGizmosSelected() {
            Tools.hidden = (EditMode != 0);
            Gizmos.DrawIcon(this.transform.position, "PathGenerator/PG_Anchor.png", true);
            if (FlagList_Local != null && FlagList_Local.Count > 0) {
                for (int i = 0; i < FlagList_Local.Count; i++) {
                    if (i == 0)
                        Gizmos.DrawIcon(FlagList_Local[i], "PathGenerator/PG_Start.png", ( EditMode == 1 ));
                    else if (!isClosed && i == FlagList_Local.Count - 1)
                        Gizmos.DrawIcon(FlagList_Local[i], "PathGenerator/PG_End.png", ( EditMode == 1 ));
                    else 
                        Gizmos.DrawIcon(FlagList_Local[i], "PathGenerator/PG_Node.png", ( EditMode == 1 ));
                }
            }

            if (AngleList_Local != null && AngleList_Local.Count > 0)
                for (int i = 0; i < AngleList_Local.Count; i++)
                    Gizmos.DrawIcon(AngleList_Local[i], "PathGenerator/PG_Handler.png", ( EditMode == 1 ));
        }
        
    }
}