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

    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]

    [System.Serializable]
    public class PathGenerator : MonoBehaviour {
        public bool isClosed = false;           // is this path closed ?
        public bool isLivePath = false;         // is calculate the path in runtime ?
        public bool isShowingIcons = true;      // is showing icons ?
        public int PathDensity = 5;            // Density of guide objects between Nodes

        public int EditMode = 0;   // (Editor) is individaul control mode?


        public List<Vector3> PathList = new List<Vector3>();        // List of Path pos
        public List<float> pathLengths = new List<float>();

        [SerializeField]
        public List<Vector3> NodeList = new List<Vector3>();        // List of Node pos
        [SerializeField]
        public List<Vector3> AngleList = new List<Vector3>();       // List of Angle pos

        public List<Vector3> NodeList_World = new List<Vector3>();  // List of Node pos
        public List<Vector3> AngleList_World = new List<Vector3>(); // List of Angle pos


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
            try {
                PathList = new List<Vector3>();
                pathLengths = new List<float>();

                //=========================================================================================================
                //  check path density is bigger than 1
                //  path density가 1보다 큰 지 확인
                //=========================================================================================================
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

                //=========================================================================================================
                //  Generate path based on Bézier curve between Nodes
                //  Node들 사이에 베지어 곡선 기반의 path 생성
                //=========================================================================================================
                for (int i = 0; i < NodeList_World.Count; i++) {
                    //=====================================================================================================
                    //  Select Nodes
                    //  Node 선택
                    //=====================================================================================================
                    Vector3 startPoint = NodeList_World[i];
                    Vector3 middlePoint = new Vector3();
                    Vector3 endPoint = new Vector3();
                    if (i == NodeList_World.Count - 1) {
                        if (isClosed) {
                            middlePoint = AngleList_World[i];
                            endPoint = NodeList_World[0];
                        } else {
                            break;
                        }
                    } else {
                        middlePoint = AngleList_World[i];
                        endPoint = NodeList_World[i + 1];
                    }

                    //=====================================================================================================
                    //  Calculate Bézier curve
                    //  베지어 커브 계산
                    //=====================================================================================================
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

                // 닫힌 경로인 경우 마지막 Node를 Path리스트에 넣어줌
                if (isClosed)
                    PathList.Add(NodeList_World[0]);
                else
                    PathList.Add(NodeList_World[NodeList_World.Count - 1]);

                float l = ( PathList[PathList.Count - 2] - PathList[PathList.Count - 1] ).magnitude;
                pathLengths.Add(pathLengths[pathLengths.Count - 1] + l);

                ////=========================================================================================================
                ////  Debug Obejct ( Node, StartNode, Angle ) visual control
                ////  디버그 오브젝트 (Node, StartNode, Angle) 비주얼 컨트롤
                ////=========================================================================================================
                //if (!isDebugObject) {

                //}
            } catch (System.Exception e) {
                e.ToString();
            }
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
            if (isLivePath)
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
            if(isShowingIcons) {
                Gizmos.DrawIcon(this.transform.position, "PathGenerator/PG_Anchor.png", true);
                if (NodeList_World != null && NodeList_World.Count > 0) {
                    for (int i = 0; i < NodeList_World.Count; i++) {
                        if (i == 0)
                            Gizmos.DrawIcon(NodeList_World[i], "PathGenerator/PG_Start.png", ( EditMode != 0 ));
                        else if (!isClosed && i == NodeList_World.Count - 1)
                            Gizmos.DrawIcon(NodeList_World[i], "PathGenerator/PG_End.png", ( EditMode != 0 ));
                        else 
                            Gizmos.DrawIcon(NodeList_World[i], "PathGenerator/PG_Node.png", ( EditMode != 0 ));
                    }
                }

                if (AngleList_World != null && AngleList_World.Count > 0)
                    for (int i = 0; i < AngleList_World.Count; i++)
                        Gizmos.DrawIcon(AngleList_World[i], "PathGenerator/PG_Handler.png", ( EditMode != 0 ));
            }
        }
        
    }
}