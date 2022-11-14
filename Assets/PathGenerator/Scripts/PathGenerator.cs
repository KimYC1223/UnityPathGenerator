using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

//===================================================================================================
//
//  PATH GENERATOR CLASS
//
//  Script to make followable path the based on Bézier curve
//  Path Generator가 만든 Path를 따라가는 기능
//
//---------------------------------------------------------------------------------------------------
//  2020.08.30 _ KimYC1223
//===================================================================================================

public class PathGenerator : MonoBehaviour {
    public bool isClosed = true;            // is this path closed?
    public bool isLiveRender = true;        // is draw the path in runtime?
    public int PathDensity = 30;            // Density of guide objects between Flags
    
    public List<Vector3> FlagList = new List<Vector3>();        // List of Flag pos
    public List<Vector3> AngleList = new List<Vector3>();       // List of Angle pos
    public List<Vector3> FlagList_Local = new List<Vector3>();  // List of Flag pos
    public List<Vector3> AngleList_Local = new List<Vector3>(); // List of Angle pos
    public List<Vector3> PathList = new List<Vector3>();        // List of Path pos

    //===============================================================================================
    // Awake method
    //-----------------------------------------------------------------------------------------------
    // init variable & position
    // 각종 변수와 position 초기화
    //===============================================================================================
    public void Awake() {
        UpdatePath();
    }


    //===============================================================================================
    // UpdatePath method
    //-----------------------------------------------------------------------------------------------
    // Calculate & Generate Path
    // 경로 계산 및 생성
    //===============================================================================================
    public void UpdatePath() {
        PathList = new List<Vector3>();

        //===========================================================================================
        //  check path density is bigger than 1
        //  path density가 1보다 큰 지 확인
        //===========================================================================================
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

        //===========================================================================================
        //  Generate path based on Bézier curve between Flags
        //  Flag들 사이에 베지어 곡선 기반의 path 생성
        //===========================================================================================
        for (int i = 0; i < FlagList_Local.Count; i++) {
            //=======================================================================================
            //  Select Flags
            //  Flag 선택
            //=======================================================================================
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

            //=======================================================================================
            //  Calculate Bézier curve
            //  베지어 커브 계산
            //=======================================================================================
            for (int j = 0; j < PathDensity; j++) {
                float t = (float)j / PathDensity;

                Vector3 curve = ( 1f - t ) * ( 1f - t ) * startPoint +
                               2 * ( 1f - t ) * t * middlePoint +
                               t * t * endPoint;
                PathList.Add(curve);
            }

        }

        // 닫힌 경로인 경우 마지막 Flag를 Path리스트에 넣어줌
        if (isClosed)
            PathList.Add(FlagList_Local[0]);
        Debug.Log(PathList.Count);

        ////===========================================================================================
        ////  Debug Obejct ( Flag, StartFlag, Angle ) visual control
        ////  디버그 오브젝트 (Flag, StartFlag, Angle) 비주얼 컨트롤
        ////===========================================================================================
        //if (!isDebugObject) {

        //}
    }

    public void Update() {
        if (isLiveRender)
            UpdatePath();
    }
    
    public void OnDrawGizmosSelected() {
        if(FlagList_Local != null && FlagList_Local.Count > 0) {
            for (int i = 0; i < FlagList_Local.Count; i++) {
                if (i == 0)
                    Gizmos.DrawIcon(FlagList_Local[i], "PathGenerator/PG_Start.png", true);
                else if (!isClosed && i == FlagList_Local.Count - 1)
                    Gizmos.DrawIcon(FlagList_Local[i], "PathGenerator/PG_End.png", true);
                else
                    Gizmos.DrawIcon(FlagList_Local[i], "PathGenerator/PG_Node.png", true);
            }
        }

        if (AngleList_Local != null && AngleList_Local.Count > 0)
            for (int i = 0; i < AngleList_Local.Count; i++)
                Gizmos.DrawIcon(AngleList_Local[i], "PathGenerator/PG_Handler.png", true);
    }
}