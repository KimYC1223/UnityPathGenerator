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
    void OnSceneGUI() {
        
        float sphereSize = 0.05f;
        PathGenerator pathGenerator = target as PathGenerator;
        List<GameObject> FlagList = pathGenerator.FlagList;
        List<GameObject> AngleList = pathGenerator.AngleList;
        int Density = pathGenerator.PathDensity;

        int Count = FlagList.Count;
        int Count2 = AngleList.Count;

        if (FlagList.Count < AngleList.Count) {
            int offset = AngleList.Count - FlagList.Count;
            for (int i = 0; i < offset; i++) {
                DestroyImmediate(AngleList[AngleList.Count - 1]);
                AngleList.RemoveAt(AngleList.Count - 1);
            }
        }

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

        GameObject AngleRoot = null;
        GameObject FlagRoot = null;
        GameObject RoadRoot = null;

        foreach(Transform t in TotalParents) {
            if (t.gameObject.name == "Angles")
                AngleRoot = t.gameObject;
            else if (t.gameObject.name == "Flags")
                FlagRoot = t.gameObject;
            else if (t.gameObject.name == "Roads")
                RoadRoot = t.gameObject;
        }

        if (AngleRoot == null) {
            AngleRoot = new GameObject();
            AngleRoot.transform.SetParent(pathGenerator.transform);
            AngleRoot.transform.localPosition = new Vector3();
            AngleRoot.transform.localRotation = new Quaternion();
            AngleRoot.transform.localScale = new Vector3(1f,1f,1f);
            AngleRoot.name = "Angles";
        }

        if (FlagRoot == null) {
            FlagRoot = new GameObject();
            FlagRoot.transform.SetParent(pathGenerator.transform);
            FlagRoot.transform.localPosition = new Vector3();
            FlagRoot.transform.localRotation = new Quaternion();
            FlagRoot.transform.localScale = new Vector3(1f, 1f, 1f);
            FlagRoot.name = "Flags";
        }
        
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