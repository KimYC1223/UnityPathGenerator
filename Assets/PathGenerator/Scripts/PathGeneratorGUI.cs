using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//===================================================================================================
//
//  PATH GENERATOR GUI CLASS
//
//  GUI Script to help generate path based on Bézier curve 
//  베지어 곡선 기반의 path를 만드는데 도움을 주는 GUI
//
//---------------------------------------------------------------------------------------------------
//  2020.09.01 _ KimYC1223
//===================================================================================================

[CustomEditor(typeof(PathGenerator))]
class PathGeneratorGUI : Editor {

    //===============================================================================================
    // OnSceneGUI method
    //-----------------------------------------------------------------------------------------------
    // A function called whenever the GUI is drawn on the screen.
    // 스크린에 GUI가 그려질 때 마다 호출되는 함수
    //===============================================================================================
    void OnSceneGUI() {
        
        float sphereSize = 0.05f;                                   // 핸들러 크기
        PathGenerator pathGenerator = target as PathGenerator;      // 조절할 PathGenerator
        List<GameObject> FlagList = pathGenerator.FlagList;
        List<GameObject> AngleList = pathGenerator.AngleList;
        int Density = pathGenerator.PathDensity;

        int Count = FlagList.Count;
        int Count2 = AngleList.Count;

        //===========================================================================================
        // Exception handling when FlagList is less than AngleList
        // FlagList가 AngleList보다 작을때의 예외 처리
        //===========================================================================================
        if (FlagList.Count < AngleList.Count) {
            int offset = AngleList.Count - FlagList.Count;
            for (int i = 0; i < offset; i++) {
                DestroyImmediate(AngleList[AngleList.Count - 1]);
                AngleList.RemoveAt(AngleList.Count - 1);
            }
        }

        //===========================================================================================
        // Exception handling when there is only one flag
        // Flag가 하나 밖에 없을 때의 예외 처리
        //===========================================================================================
        Transform[] TotalParents = pathGenerator.transform.GetComponentsInChildren<Transform>();
        if (FlagList.Count < 2) {
            FlagList.Clear();
            AngleList.Clear();
            int max = TotalParents.Length;
            for(int i = 1; i < max; i++) {
                try { DestroyImmediate(TotalParents[i].gameObject); }
                catch (System.Exception e) { e.ToString(); }
            }
            return ;
        }

        //===========================================================================================
        // Check the elements of the Flag List. If name is not "Flag (n)", remove from array
        // Flag List의 원소를 점검함. 이름이 "Flag (n)"이 아니면, 배열에서 제거
        //===========================================================================================
        for (int i = 1; i < Count; i++) {
            try {
                if (FlagList[i] == null) continue;
                if (FlagList[i].name != ("Flag (" + i + ")"))
                    FlagList[i] = null;
            } catch (System.Exception e) {
                e.ToString();
                break;
            }
        }

        //===========================================================================================
        // Check the elements of the Angle List. If name is not "Angle (n)", remove from array
        // Angle List의 원소를 점검함. 이름이 "Angle (n)"이 아니면, 배열에서 제거
        //===========================================================================================
        for (int i = 1; i < Count2; i++) {
            try {
                if (AngleList[i] == null) continue;
                if (AngleList[i].name != ("Angle (" + i + ")"))
                    AngleList[i] = null;
            } catch (System.Exception e) {
                e.ToString();
                break;
            }
        }

        //===========================================================================================
        // Declare the parent of Preview objects
        // Preview 오브젝트들의 부모 오브젝트 선언
        //===========================================================================================
        GameObject AngleRoot = null;
        GameObject FlagRoot = null;
        GameObject RoadRoot = null;

        //===========================================================================================
        // Look up the parent of the Preview objects by name in the scene.
        // Scene에서 Preview 오브젝트들의 부모 오브젝트를 찾아봄.
        //===========================================================================================
        foreach (Transform t in TotalParents) {
            if (t.gameObject.name == "Angles")
                AngleRoot = t.gameObject;
            else if (t.gameObject.name == "Flags")
                FlagRoot = t.gameObject;
            else if (t.gameObject.name == "Roads")
                RoadRoot = t.gameObject;
        }

        //===========================================================================================
        // If AngleRoot does not exist, create one
        // AngleRoot가 없으면, 새로 만듦
        //===========================================================================================
        if (AngleRoot == null) {
            AngleRoot = new GameObject();
            AngleRoot.transform.SetParent(pathGenerator.transform);
            AngleRoot.transform.localPosition = new Vector3();
            AngleRoot.transform.localRotation = new Quaternion();
            AngleRoot.transform.localScale = new Vector3(1f,1f,1f);
            AngleRoot.name = "Angles";
        }

        //===========================================================================================
        // If FlagRoot does not exist, create one
        // FlagRoot가 없으면, 새로 만듦
        //===========================================================================================
        if (FlagRoot == null) {
            FlagRoot = new GameObject();
            FlagRoot.transform.SetParent(pathGenerator.transform);
            FlagRoot.transform.localPosition = new Vector3();
            FlagRoot.transform.localRotation = new Quaternion();
            FlagRoot.transform.localScale = new Vector3(1f, 1f, 1f);
            FlagRoot.name = "Flags";
        }

        //===========================================================================================
        // If RoadRoot does not exist, create one
        // RoadRoot가 없으면, 새로 만듦
        //===========================================================================================
        if (RoadRoot == null) {
            RoadRoot = new GameObject();
            RoadRoot.transform.SetParent(pathGenerator.transform);
            RoadRoot.transform.localPosition = new Vector3();
            RoadRoot.transform.localRotation = new Quaternion();
            RoadRoot.transform.localScale = new Vector3(1f, 1f, 1f);
            RoadRoot.name = "Roads";
        }

        Transform[] AngleRootChild = AngleRoot.transform.GetComponentsInChildren<Transform>();
        Transform[] FlagRootChild = FlagRoot.transform.GetComponentsInChildren<Transform>();

        //===========================================================================================
        // Compare the List with the child objects of the Root, If it's not a specific one, remove it.
        // Scene에서 찾은 Root들의 자식 객체과 List를 비교하여, 특정한 오브젝트가 아니면 제거
        //===========================================================================================
        foreach (Transform t in FlagRootChild) {
            if (t.gameObject.name == "Flags")
                continue;
            bool flag = false;
            foreach(GameObject go in FlagList) {
                if(go == t.gameObject) {
                    flag = true;
                    break;
                }
            }
            if (!flag)
                DestroyImmediate(t.gameObject);
        }

        //===========================================================================================
        // Check objects of the List. If it's not in the scene, create in a random location around it.
        // List 기반으로 오브젝트를 검사. 만약, Scene에 없으면 주변 랜덤한 위치에 생성
        //===========================================================================================
        for (int i = 0; i < Count; i++) {
            if (FlagList[i] != null)
                continue;

            bool nameCheck = false;
            foreach(Transform t in FlagRootChild) {
                if (t.gameObject.name == ("Flag (" + i + ")")) {
                    nameCheck = true;
                    FlagList[i] = t.gameObject;
                    break;
                }
            }
            if (!nameCheck) {
                if (i == 0) FlagList[i] = Instantiate(pathGenerator.StartFlag, FlagRoot.transform);
                else FlagList[i] = Instantiate(pathGenerator.Flag, FlagRoot.transform);
                FlagList[i].transform.localPosition
                    = new Vector3(Random.Range(-3f,3f),0f, Random.Range(-3f, 3f));
                FlagList[i].name = "Flag (" + i + ")";
            }
        }

        //===========================================================================================
        // Check objects of List. If it's not in the scene, create at the midpoint between the flags.
        // List 기반으로 오브젝트를 검사. 만약, Scene에 없으면 두 Flag 사이의 중점에 생성
        //===========================================================================================
        for (int i = 0; i < Count; i++) {
            if ((i == Count - 1) && (!pathGenerator.isClosed))
                continue;
            try {
                if (AngleList[i] != null)
                    continue;
            } catch( System.Exception e) {
                e.ToString();
                int offset = FlagList.Count - AngleList.Count;
                for(int k = 0; k < offset; k++)
                    AngleList.Add(null);
            }

            bool nameCheck = false;
            foreach (Transform t in AngleRootChild) {
                if (t.gameObject.name == ("Angle (" + i + ")")) {
                    nameCheck = true;
                    AngleList[i] = t.gameObject;
                    break;
                }
            }
            if (!nameCheck) {
                AngleList[i] = Instantiate(pathGenerator.Angle, AngleRoot.transform);
                GameObject nextFlag = (i == Count - 1) ? FlagList[0] : FlagList[i + 1];
                AngleList[i].transform.localPosition
                    = (FlagList[i].transform.localPosition + nextFlag.transform.localPosition) / 2;
                AngleList[i].name = "Angle (" + i + ")";
            }
        }

        //===========================================================================================
        // If it is a closed loop, connect the last and first points
        // 닫힌 루프라면, 마지막과 첫 점을 이어줌
        //===========================================================================================
        try {
            if (!pathGenerator.isClosed && AngleList[Count - 1] != null) {
                DestroyImmediate(AngleList[Count - 1]);
                AngleList[Count - 1] = null;
            }
        } catch (System.Exception e) {
            e.ToString();
            int offset = FlagList.Count - AngleList.Count;
            for (int k = 0; k < offset; k++)
                AngleList.Add(null);
        }

        //===========================================================================================
        // Calculate Bézier curve
        // 베지어 커브 계산
        //===========================================================================================
        for (int i = 0; i < Count; i++) {
            FlagList[i].transform.position = Handles.PositionHandle(FlagList[i].transform.position, Quaternion.identity);
            if (AngleList[i] != null) {
                AngleList[i].transform.position = Handles.PositionHandle(AngleList[i].transform.position, Quaternion.identity);

                Vector3 startPoint = FlagList[i].transform.position;
                Vector3 middlePoint = AngleList[i].transform.position;
                Vector3 endPoint = (i == Count - 1) ?
                    FlagList[0].transform.position :
                    FlagList[i + 1].transform.position;

                Handles.color = Color.green;
                Handles.DrawLine(startPoint, middlePoint);

                Handles.color = Color.yellow;
                Handles.DrawLine(middlePoint, endPoint);

                for (int j = 0; j < Density; j++) {
                    float t = (float)j / Density;

                    Vector3 curve = (1f - t) * (1f - t) * startPoint +
                                   2 * (1f - t) * t * middlePoint +
                                   t * t * endPoint;

                    Handles.color = Color.cyan;
                    Handles.SphereHandleCap(i * Density + j, curve, Quaternion.identity, sphereSize,EventType.Repaint);
                }
            }
        }
        
    }
}
