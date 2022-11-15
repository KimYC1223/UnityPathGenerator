using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

//=======================================================================================================================================
//
//  PATH GENERATOR GUI CLASS
//
//  GUI Script to help generate path based on Bézier curve 
//  베지어 곡선 기반의 path를 만드는데 도움을 주는 GUI
//
//---------------------------------------------------------------------------------------------------------------------------------------
//  2020.09.01 _ KimYC1223
//=======================================================================================================================================

namespace CurvedPathGenertator {
    [CustomEditor(typeof(PathGenerator))]
    class PathGeneratorGUI : Editor {
        private Vector3 Old_Position = new Vector3(0,0,0);
        private Vector3 Old_Scale = new Vector3(1f,1f,1f);
        private Quaternion Old_Rotation = Quaternion.identity;
        private GUIStyle centerStyle;

        //===============================================================================================================================
        // OnSceneGUI method
        //-------------------------------------------------------------------------------------------------------------------------------
        // A function called whenever the inspector is drawn.
        // 인스펙터가 그려질 때 마다 호출되는 함수
        //===============================================================================================================================
        public override void OnInspectorGUI() {
            PathGenerator pathGenerator = target as PathGenerator;

            centerStyle = new GUIStyle("Label");
            centerStyle.alignment = TextAnchor.MiddleCenter;
            Texture LogoTex = (Texture2D)Resources.Load("PathGenerator/Logo/PathGeneratorScriptImg", typeof(Texture2D));
            GUILayout.Label(LogoTex, GUILayout.Width(400), GUILayout.Height(90));
            GUI.enabled = false;
            GUILayout.Label("  Curved Path Genertator   |   곡선 경로 생성기", EditorStyles.boldLabel);
            GUILayout.Label("  Developed by KimYC1223");
            GUI.enabled = true;
            GUILayout.Space(10);
            GuiLine();
            GUILayout.Space(10);

            //===========================================================================================================================
            // Open / Close path selection button
            //---------------------------------------------------------------------------------------------------------------------------
            // Choose open / close path type
            // 열린 패스 / 닫힌 패스를 정하는 버튼
            //===========================================================================================================================
            if (pathGenerator.isClosed) {
                if (GUILayout.Button("Change to open path", GUILayout.Height(25))) {
                    Undo.RecordObject(pathGenerator, "Modify " + pathGenerator.gameObject.name);
                    pathGenerator.isClosed = false;
                }
            }
            else {
                if (GUILayout.Button("Change to closed path", GUILayout.Height(25))) {
                    Undo.RecordObject(pathGenerator, "Modify " + pathGenerator.gameObject.name);
                    pathGenerator.isClosed = true;
                }
            }
            GUILayout.Space(5);

            //===========================================================================================================================
            // Open / Close path selection button
            //---------------------------------------------------------------------------------------------------------------------------
            // Choose open / close path type
            // 열린 패스 / 닫힌 패스를 정하는 버튼
            //===========================================================================================================================
            if (pathGenerator.EditMode == 0) {
                if (GUILayout.Button("Change to individual edit mode", GUILayout.Height(25))) {
                    Undo.RecordObject(pathGenerator, "Modify " + pathGenerator.gameObject.name);
                    pathGenerator.EditMode = 1;
                }
            } else if (pathGenerator.EditMode == 1) {
                if (GUILayout.Button("Change to total edit mode", GUILayout.Height(25))) {
                    Undo.RecordObject(pathGenerator, "Modify " + pathGenerator.gameObject.name);
                    pathGenerator.EditMode = 2;
                }
            } else {
                if (GUILayout.Button("Change to normal mode", GUILayout.Height(25))) {
                    Undo.RecordObject(pathGenerator, "Modify " + pathGenerator.gameObject.name);
                    pathGenerator.EditMode = 0;
                }
            }
        }

        void GuiLine(int i_height = 1) {
            Rect rect = EditorGUILayout.GetControlRect(false, i_height);
            rect.height = i_height;
            EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));
        }

        //===============================================================================================================================
        // OnSceneGUI method
        //-------------------------------------------------------------------------------------------------------------------------------
        // A function called whenever the GUI is drawn on the screen.
        // 스크린에 GUI가 그려질 때 마다 호출되는 함수
        //===============================================================================================================================
        public void OnSceneGUI() {
            PathGenerator pathGenerator = target as PathGenerator;      // 조절할 PathGenerator
            Undo.RecordObject(pathGenerator, "Modify " + pathGenerator.gameObject.name);

            Quaternion rotation = pathGenerator.transform.rotation;
            Matrix4x4 m_rotate  = Matrix4x4.Rotate(rotation);
            Matrix4x4 m_reverse = Matrix4x4.Rotate(Quaternion.Inverse(rotation));
            List<Vector3> FlagList = pathGenerator.FlagList;
            List<Vector3> AngleList = pathGenerator.AngleList;
            int Density = pathGenerator.PathDensity;

            int Count = FlagList.Count;

            if (Old_Position != pathGenerator.transform.localPosition ||
                Old_Rotation != pathGenerator.transform.localRotation ||
                Old_Scale    != pathGenerator.transform.localScale ||
                pathGenerator.FlagList_Local.Count != FlagList.Count ||
                pathGenerator.AngleList_Local.Count != AngleList.Count) {

                Old_Position = pathGenerator.transform.localPosition;
                Old_Rotation = pathGenerator.transform.localRotation;
                Old_Scale = pathGenerator.transform.localScale;

                pathGenerator.FlagList_Local = new List<Vector3>();
                for (int i = 0; i < FlagList.Count; i++) {
                    pathGenerator.FlagList_Local.Add(FlagList[i]);
                    pathGenerator.FlagList_Local[i] = m_rotate.MultiplyPoint3x4(pathGenerator.FlagList_Local[i]);
                    pathGenerator.FlagList_Local[i] = new Vector3(
                        pathGenerator.FlagList_Local[i].x * pathGenerator.transform.lossyScale.x,
                        pathGenerator.FlagList_Local[i].y * pathGenerator.transform.lossyScale.y,
                        pathGenerator.FlagList_Local[i].z * pathGenerator.transform.lossyScale.z
                    );
                    pathGenerator.FlagList_Local[i] += pathGenerator.transform.position;
                }

                pathGenerator.AngleList_Local = new List<Vector3>();
                for (int i = 0; i < AngleList.Count; i++) {
                    pathGenerator.AngleList_Local.Add(AngleList[i]);
                    pathGenerator.AngleList_Local[i] = m_rotate.MultiplyPoint3x4(pathGenerator.AngleList_Local[i]);
                    pathGenerator.AngleList_Local[i] = new Vector3(
                        pathGenerator.AngleList_Local[i].x * pathGenerator.transform.lossyScale.x,
                        pathGenerator.AngleList_Local[i].y * pathGenerator.transform.lossyScale.y,
                        pathGenerator.AngleList_Local[i].z * pathGenerator.transform.lossyScale.z
                    );
                    pathGenerator.AngleList_Local[i] += pathGenerator.transform.position;
                }
            }

            //===========================================================================================================================
            // Exception handling when FlagList is less than AngleList
            // FlagList가 AngleList보다 작을때의 예외 처리
            //===========================================================================================================================
            if (FlagList.Count < AngleList.Count) {
                int offset = AngleList.Count - FlagList.Count;
                for (int i = 0; i < offset; i++) {
                    AngleList.RemoveAt(AngleList.Count - 1);
                }
            }

            //===========================================================================================================================
            // Exception handling when there is only one flag
            // Flag가 하나 밖에 없거나 Anglelist가 비어있을때의 예외 처리
            //===========================================================================================================================
            if (FlagList.Count < 2) {
                FlagList.Clear();
                FlagList.Add(new Vector3(Random.Range(-3f, 3f), 0f, Random.Range(-3f, 3f)));
                FlagList.Add(new Vector3(Random.Range(-3f, 3f), 0f, Random.Range(-3f, 3f)));
                Count = 2;
            }

            //===========================================================================================================================
            // Exception handling when AngleList size is less than FlagList size
            // AngleList 크기가 FlagList보다 작은 경우 예외 처리
            //===========================================================================================================================
            if (pathGenerator.isClosed && AngleList.Count < FlagList.Count) {
                AngleList.Clear();
                for(int i = 0; i < Count; i++)
                    AngleList.Add(new Vector3(Random.Range(-3f, 3f), 0f, Random.Range(-3f, 3f)));
            } else if (!pathGenerator.isClosed && AngleList.Count < FlagList.Count-1) {
                AngleList.Clear();
                for (int i = 0; i < Count - 1; i++)
                    AngleList.Add(new Vector3(Random.Range(-3f, 3f), 0f, Random.Range(-3f, 3f)));
            }

            //===========================================================================================================================
            // Check objects of List. If it's not in the scene, create at the midpoint between the flags.
            // List 기반으로 오브젝트를 검사. 만약, Scene에 없으면 두 Flag 사이의 중점에 생성
            //===========================================================================================================================
            for (int i = 0; i < Count; i++) {
                if ((i == Count - 1) && (!pathGenerator.isClosed))
                    continue;
                if (AngleList[i] == null) {
                    Vector3 nextFlag = (i == Count - 1) ? FlagList[0] : FlagList[i + 1];
                    AngleList[i]
                        = (FlagList[i] + nextFlag) / 2;
                }
            }

            //===========================================================================================================================
            // If it is a closed loop, connect the last and first points
            // 닫힌 루프라면, 마지막과 첫 점을 이어줌
            //===========================================================================================================================
            try {
                if (!pathGenerator.isClosed && AngleList[Count - 1] != null) {
                    AngleList.RemoveAt(AngleList.Count - 1);
                }
            } catch (System.Exception e) {
                e.ToString();
            }


            if ( pathGenerator.EditMode == 2 && ( pathGenerator.FlagList_Local != null || pathGenerator.AngleList_Local != null ) &&
                 ( pathGenerator.FlagList_Local.Count > 0 || pathGenerator.AngleList_Local.Count > 0) ) {
                Vector3 centerPos = Vector3.zero;
                for (int i = 0; i < pathGenerator.FlagList_Local.Count; i++)
                    centerPos += pathGenerator.FlagList_Local[i];
                for (int i = 0; i < pathGenerator.AngleList_Local.Count; i++)
                    centerPos += pathGenerator.AngleList_Local[i];

                
                centerPos /= ((int)pathGenerator.FlagList_Local.Count + (int)pathGenerator.AngleList_Local.Count);

                Vector3 new_centerPos = Handles.PositionHandle(centerPos, pathGenerator.transform.localRotation);
                Vector3 offset = new_centerPos - centerPos;
                for (int i = 0; i < pathGenerator.FlagList_Local.Count; i++) {
                    pathGenerator.FlagList[i] += offset;
                    pathGenerator.FlagList_Local[i] += offset;
                }
                for (int i = 0; i < pathGenerator.AngleList_Local.Count; i++) {
                    pathGenerator.AngleList[i] += offset;
                    pathGenerator.AngleList_Local[i] += offset;
                }
            }

            //===========================================================================================================================
            // Calculate Bézier curve
            // 베지어 커브 계산
            //===========================================================================================================================
            try {
                for (int i = 0; i < Count; i++) {
                    if (pathGenerator.EditMode == 1) {
                        pathGenerator.FlagList_Local[i] = Handles.PositionHandle(pathGenerator.FlagList_Local[i],
                                                              pathGenerator.transform.localRotation);
                        pathGenerator.FlagList[i] = pathGenerator.FlagList_Local[i] - pathGenerator.transform.position;
                        pathGenerator.FlagList[i] = m_reverse.MultiplyPoint3x4(pathGenerator.FlagList[i]);
                        pathGenerator.FlagList[i] = new Vector3(
                            pathGenerator.FlagList[i].x / pathGenerator.transform.lossyScale.x,
                            pathGenerator.FlagList[i].y / pathGenerator.transform.lossyScale.y,
                            pathGenerator.FlagList[i].z / pathGenerator.transform.lossyScale.z
                        );
                    }

                    try {
                        if ( (!pathGenerator.isClosed && ( i < Count-1) && AngleList[i] != null) || 
                                (pathGenerator.isClosed && AngleList[i] != null )) {
                            if (pathGenerator.EditMode == 1) {
                                pathGenerator.AngleList_Local[i] = Handles.PositionHandle(pathGenerator.AngleList_Local[i],
                                                                       pathGenerator.transform.localRotation);
                                pathGenerator.AngleList[i] = pathGenerator.AngleList_Local[i] - pathGenerator.transform.position;
                                pathGenerator.AngleList[i] = m_reverse.MultiplyPoint3x4(pathGenerator.AngleList[i]);
                                pathGenerator.AngleList[i] = new Vector3(
                                    pathGenerator.AngleList[i].x / pathGenerator.transform.lossyScale.x,
                                    pathGenerator.AngleList[i].y / pathGenerator.transform.lossyScale.y,
                                    pathGenerator.AngleList[i].z / pathGenerator.transform.lossyScale.z
                                );
                            }

                            Vector3 startPoint = pathGenerator.FlagList_Local[i];
                            Vector3 middlePoint = pathGenerator.AngleList_Local[i];
                            Vector3 endPoint = (i == Count - 1) ? 
                                        pathGenerator.FlagList_Local[0] : pathGenerator.FlagList_Local[i + 1];

                            Handles.color = Color.green;
                            Handles.DrawDottedLine(startPoint, middlePoint,1f);
                            Handles.color = Color.yellow;
                            Handles.DrawDottedLine(middlePoint, endPoint,1f);

                            List<Vector3> test = new List<Vector3>();
                            for (int j = 0; j < Density; j++) {
                                float t = (float)j / Density;
                                Vector3 curve = (1f - t) * (1f - t) * startPoint +
                                               2 * (1f - t) * t * middlePoint +
                                               t * t * endPoint;
                                test.Add(curve);

                            }
                            test.Add(endPoint);
                            Handles.color = Color.cyan;
                            Handles.DrawPolyLine(test.ToArray<Vector3>());
                        }
                    } catch (System.Exception e) {
                        e.ToString();
                    }
                } 
            } catch (System.Exception e) {
                e.ToString();
            }
        }

    }

}
