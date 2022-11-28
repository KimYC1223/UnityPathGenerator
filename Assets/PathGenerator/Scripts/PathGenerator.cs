using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//=====================================================================================================================================
/*      ,------.   ,---. ,--------.,--.  ,--.     ,----.   ,------.,--.  ,--.,------.,------.   ,---. ,--------. ,-----. ,------.  
        |  .--. ' /  O  \'--.  .--'|  '--'  |    '  .-./   |  .---'|  ,'.|  ||  .---'|  .--. ' /  O  \'--.  .--''  .-.  '|  .--. ' 
        |  '--' ||  .-.  |  |  |   |  .--.  |    |  | .---.|  `--, |  |' '  ||  `--, |  '--'.'|  .-.  |  |  |   |  | |  ||  '--'.' 
        |  | --' |  | |  |  |  |   |  |  |  |    '  '--'  ||  `---.|  | `   ||  `---.|  |\  \ |  | |  |  |  |   '  '-'  '|  |\  \  
        `--'     `--' `--'  `--'   `--'  `--'     `------' `------'`--'  `--'`------'`--' '--'`--' `--'  `--'    `-----' `--' '--'  */
//=====================================================================================================================================
//
//  PATH GENERATOR CLASS
//
//  Script to make followable path the based on Bézier curve
//  Path Generator가 만든 Path를 따라가는 기능
//
//------------------------------------------------------------------------------------------------------------------------------------
//  2020.08.30 _ KimYC1223
//====================================================================================================================================

namespace CurvedPathGenertator {

    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]

    [System.Serializable]
    public class PathGenerator : MonoBehaviour {
        public bool isClosed = false;                               // is this path closed ?
        public bool isLivePath = false;                             // is calculate the path in runtime ?
        public bool isShowingIcons = true;                          // is showing icons ?
        public int PathDensity = 5;                                 // Density of guide objects between Nodes

        public int EditMode = 0;                                    // (Editor) is individaul control mode?

        public bool CreateMeshFlag = true;                          // if this value is true, create mesh of path.
        public float LineMehsWidth = 0.2f;                          // width of line mesh
        public float LineOpacity = 0.7f;
        public float LineSpeed = 10f;
        public float LineTiling = 20f;
        public int LineRenderQueue = 3000;
        public Texture2D LineTexture;

        public List<Vector3> PathList = new List<Vector3>();        // List of Path pos
        public List<float> pathLengths = new List<float>();

        [SerializeField]
        public List<Vector3> NodeList = new List<Vector3>();        // List of Node pos
        [SerializeField]
        public List<Vector3> AngleList = new List<Vector3>();       // List of Angle pos

        public List<Vector3> NodeList_World = new List<Vector3>();  // List of Node pos
        public List<Vector3> AngleList_World = new List<Vector3>(); // List of Angle pos


        //============================================================================================================================
        // Awake method
        //----------------------------------------------------------------------------------------------------------------------------
        // init variable & position
        // 각종 변수와 position 초기화
        //============================================================================================================================
        public void Awake() {
            UpdatePath();
        }

        //============================================================================================================================
        // UpdatePath method
        //----------------------------------------------------------------------------------------------------------------------------
        // Calculate & Generate Path
        // 경로 계산 및 생성
        //============================================================================================================================
        public void UpdatePath() {
            try {
                PathList = new List<Vector3>();
                pathLengths = new List<float>();

                //====================================================================================================================
                //  check path density is bigger than 1
                //  path density가 1보다 큰 지 확인
                //====================================================================================================================
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

                //====================================================================================================================
                //  Generate path based on Bézier curve between Nodes
                //  Node들 사이에 베지어 곡선 기반의 path 생성
                //====================================================================================================================
                for (int i = 0; i < NodeList_World.Count; i++) {
                    //================================================================================================================
                    //  Select Nodes
                    //  Node 선택
                    //================================================================================================================
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

                    //================================================================================================================
                    //  Calculate Bézier curve
                    //  베지어 커브 계산
                    //================================================================================================================
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

                //====================================================================================================================
                //  In the case of a closed path, the last node is added to the path list.
                //  닫힌 경로인 경우 마지막 Node를 Path리스트에 넣어줌
                //====================================================================================================================
                if (isClosed)
                    PathList.Add(NodeList_World[0]);
                else
                    PathList.Add(NodeList_World[NodeList_World.Count - 1]);

                //====================================================================================================================
                //  Visualize the calculated path.
                //  계산된 경로를 가시화.
                //====================================================================================================================
                CreateMesh(PathList);

                float l = ( PathList[PathList.Count - 2] - PathList[PathList.Count - 1] ).magnitude;
                pathLengths.Add(pathLengths[pathLengths.Count - 1] + l);

            } catch (System.Exception e) {
                e.ToString();
            }
        }

        //============================================================================================================================
        // GetLength method
        //----------------------------------------------------------------------------------------------------------------------------
        // return path Length
        // 만들어진 경로의 길이를 리턴
        //============================================================================================================================
        public float GetLength() {
            if (pathLengths != null || pathLengths.Count > 0)
                return pathLengths[pathLengths.Count - 1];
            else return 0;
        }


        //============================================================================================================================
        // Update method
        //----------------------------------------------------------------------------------------------------------------------------
        // run code in every frame
        // 만들어진 경로의 길이를 리턴
        //============================================================================================================================
        public void Update() {
            if (isLivePath)
                UpdatePath();
        }


        //============================================================================================================================
        // Draw path mesh method
        //----------------------------------------------------------------------------------------------------------------------------
        // Calculate vertices and triangles of path mesh
        // 경로 메쉬의 정점과 삼각형 계산
        //============================================================================================================================
        private void CreateMesh(List<Vector3> pathVec) {
            if (!CreateMeshFlag) return;
            MeshRenderer renderer = transform.GetComponent<MeshRenderer>();

            Quaternion rotation = transform.rotation;
            Matrix4x4 m_reverse = Matrix4x4.Rotate(Quaternion.Inverse(rotation));
            int verNum = 2 * pathVec.Count;
            int triNum = 6 * ( pathVec.Count - 1 );
            Vector3[] vertices = new Vector3[verNum];
            int[] triangles = new int[triNum];
            Vector2[] uvs = new Vector2[verNum];

            float MaxLength = 0, currentLength = 0;
            for (int i = 1; i < pathVec.Count; i++)
                MaxLength += ( pathVec[i] - pathVec[i - 1] ).magnitude;

            for (int i = 0; i < pathVec.Count - 1; i++) {
                Vector3 dir = ( pathVec[i + 1] - pathVec[i] ).normalized;
                Vector3 new_dir1 = new Vector3(dir.z, 0, -dir.x);
                Vector3 new_dir2 = new Vector3(-dir.z, 0, dir.x);

                //====================================================================================================================
                //  Calculate the first part of the path
                //  경로의 처음 부분 계산
                //====================================================================================================================
                if (i == 0) {
                    vertices[2 * i] = ReverseTransformPoint(pathVec[i] + ( new_dir1 * ( LineMehsWidth / 2 ) ), m_reverse);
                    vertices[2 * i + 1] = ReverseTransformPoint(pathVec[i] + ( new_dir2 * ( LineMehsWidth / 2 ) ), m_reverse);
                    uvs[2 * i] = new Vector2(0.5f, -0.5f);
                    uvs[2 * i + 1] = new Vector2(-0.5f, -0.5f);

                //====================================================================================================================
                //  Calculate the middle part of the path
                //  경로의 중간 부분 계산
                //====================================================================================================================
                } else if (i > 0) {
                    currentLength += ( pathVec[i] - pathVec[i - 1] ).magnitude;

                    vertices[2 * i] = ReverseTransformPoint(pathVec[i] + ( new_dir1 * ( LineMehsWidth / 2 ) ), m_reverse);
                    vertices[2 * i + 1] = ReverseTransformPoint(pathVec[i] + ( new_dir2 * ( LineMehsWidth / 2 ) ), m_reverse);
                    uvs[2 * i] = new Vector2(0.5f, -0.5f + ( currentLength ) / ( MaxLength ));
                    uvs[2 * i + 1] = new Vector2(-0.5f, -0.5f + ( currentLength ) / ( MaxLength ));
                }

                //====================================================================================================================
                //  Calculate the last part of the path
                //  경로의 마지막 부분 계산
                //====================================================================================================================
                if (i == pathVec.Count - 2) {
                    vertices[2 * i + 2] = ReverseTransformPoint(pathVec[i + 1] + ( new_dir1 * ( LineMehsWidth / 2 ) ), m_reverse);
                    vertices[2 * i + 3] = ReverseTransformPoint(pathVec[i + 1] + ( new_dir2 * ( LineMehsWidth / 2 ) ), m_reverse);
                    uvs[2 * i + 2] = new Vector2(0.5f, 0.5f);
                    uvs[2 * i + 3] = new Vector2(-0.5f, 0.5f);
                }
            }

            //========================================================================================================================
            //  Calculate triangles in mesh
            //  메쉬의 삼각형 계산
            //========================================================================================================================
            for (int i = 0; i < pathVec.Count - 1; i++) {
                triangles[6 * i] = 2 * i + 3;
                triangles[6 * i + 1] = 2 * i + 2;
                triangles[6 * i + 2] = 2 * i;
                triangles[6 * i + 3] = 2 * i + 3;
                triangles[6 * i + 4] = 2 * i;
                triangles[6 * i + 5] = 2 * i + 1;
            }

            //========================================================================================================================
            //  Apply after mesh creation
            //  메쉬 생성 후 적용
            //========================================================================================================================
            MeshFilter PathMesh = transform.GetComponent<MeshFilter>();
            Mesh newMesh = new Mesh();
            newMesh.vertices = vertices;
            newMesh.triangles = triangles;
            newMesh.uv = uvs;
            newMesh.RecalculateBounds();
            newMesh.RecalculateNormals();
            PathMesh.mesh = newMesh;
        }


        //============================================================================================================================
        // OnDrawGizmosSelected method
        //----------------------------------------------------------------------------------------------------------------------------
        // A method that draws a gizmo when an object is selected
        // 오브젝트를 선택했을 때, 기즈모를 그리는 메소드
        //============================================================================================================================
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

        public void ResetTools() {
            Tools.hidden = false;
        }


        //=======================================================================================================================================================
        //  Revers transform point method
        //-------------------------------------------------------------------------------------------------------------------------------------------------------
        //  A method that changes world coordinates to local coordinates according to the parent's transform information
        //  world 좌표를 부모의 transform 정보에 따라 local 좌표로 변경하는 메소드
        //
        //  It proceeds in the order of " Move -> Scale up / down -> Rotate "          * Reverse order of TransformPoint methods
        //  " 이동 -> 스케일 업 / 다운 -> Rotate " 순서로 진행 됨                      * TransformPoint 메소드의 역순서임
        //=======================================================================================================================================================
        private Vector3 ReverseTransformPoint(Vector3 points, Matrix4x4 m_reverse) {
            Vector3 result = points;

            result = result - transform.position;                         // Step1 . Move           
            result = m_reverse.MultiplyPoint3x4(result);                                // Step3 . Rotate
            result = new Vector3(                                                       // Step2 . Scale-up/down
                result.x / transform.lossyScale.x,
                result.y / transform.lossyScale.y,
                result.z / transform.lossyScale.z
            );
            return result;
        }
    }

}