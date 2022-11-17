using NUnit;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

//===============================================================================================================================================================
//
//  PATH GENERATOR GUI CLASS
//
//  GUI Script to help generate path based on Bézier curve 
//  베지어 곡선 기반의 path를 만드는데 도움을 주는 GUI
//
//---------------------------------------------------------------------------------------------------------------------------------------------------------------
//  2020.09.01 _ KimYC1223
//===============================================================================================================================================================

namespace CurvedPathGenertator {

    [CustomEditor(typeof(PathGenerator))]
    class PathGeneratorGUI : Editor {
        static public LANGUAGE CurrentLanguage = LANGUAGE.ENG;

        private Vector3 old_Position = new Vector3(0,0,0);
        private Vector3 old_Scale = new Vector3(1f,1f,1f);
        private Quaternion old_Rotation = Quaternion.identity;
        private GUIStyle centerStyle;
        private Vector2 scrollPosition;
        private Vector2 scrollPosition2;
        private int flagListEditIndex = -1;
        private int angleListEditIndex = -1;
        private bool isShowIndex = true;

        //=======================================================================================================================================================
        // OnSceneGUI method
        //-------------------------------------------------------------------------------------------------------------------------------------------------------
        // A function called whenever the inspector is drawn.
        // 인스펙터가 그려질 때 마다 호출되는 함수
        //=======================================================================================================================================================
        public override void OnInspectorGUI() {
            PathGenerator pathGenerator = target as PathGenerator;

            centerStyle = new GUIStyle("Label");
            centerStyle.alignment = TextAnchor.MiddleCenter;
            Texture LogoTex = (Texture2D)Resources.Load("PathGenerator/Logo/PathGeneratorScriptImg", typeof(Texture2D));
            GUILayout.Label(LogoTex, GUILayout.Width(400), GUILayout.Height(90));

            GUILayout.BeginHorizontal();
            GUI.enabled = CurrentLanguage != LANGUAGE.ENG;
            if (GUILayout.Button("English", GUILayout.Height(22))) {
                Undo.RecordObject(pathGenerator, "Modify " + pathGenerator.gameObject.name);
                CurrentLanguage = LANGUAGE.ENG;
            }
            GUI.enabled = CurrentLanguage != LANGUAGE.KOR;
            if (GUILayout.Button("한국어", GUILayout.Height(22))) {
                Undo.RecordObject(pathGenerator, "Modify " + pathGenerator.gameObject.name);
                CurrentLanguage = LANGUAGE.KOR;
            }
            GUI.enabled = CurrentLanguage != LANGUAGE.JAP;
            if (GUILayout.Button("日本語", GUILayout.Height(22))) {
                Undo.RecordObject(pathGenerator, "Modify " + pathGenerator.gameObject.name);
                CurrentLanguage = LANGUAGE.JAP;
            }
            GUI.enabled = true;
            GUILayout.EndHorizontal();
            GUILayout.Space(8);
            GUI.enabled = false;
            GUILayout.Label("  " + PathGeneratorGUILanguage.GetLocalText("PG_Title"), EditorStyles.boldLabel);
            GUILayout.Label("  " + PathGeneratorGUILanguage.GetLocalText("PG_SubTitle"));
            GUI.enabled = true;
            GUILayout.Space(8);
            GuiLine();
            GUILayout.Space(15);

            //===================================================================================================================================================
            // Open / Close path selection button
            //---------------------------------------------------------------------------------------------------------------------------------------------------
            // Choose open / close path type
            // 열린 패스 / 닫힌 패스를 정하는 버튼
            //===================================================================================================================================================
            if (pathGenerator.isClosed) {
                if (GUILayout.Button(PathGeneratorGUILanguage.GetLocalText("PG_PathTypeChangeButton_ToOpen"), GUILayout.Height(25))) {
                    Undo.RecordObject(pathGenerator, "Modify " + pathGenerator.gameObject.name);
                    pathGenerator.isClosed = false;
                }
            }
            else {
                if (GUILayout.Button(PathGeneratorGUILanguage.GetLocalText("PG_PathTypeChangeButton_ToClose"), GUILayout.Height(25))) {
                    Undo.RecordObject(pathGenerator, "Modify " + pathGenerator.gameObject.name);
                    if(pathGenerator.FlagList != null && pathGenerator.FlagList.Count >= 2) {
                        Vector3 centerPos = pathGenerator.FlagList[pathGenerator.FlagList.Count - 1] + pathGenerator.FlagList[0];
                        centerPos /= 2;
                        pathGenerator.AngleList.Add(centerPos);
                    }
                    pathGenerator.isClosed = true;
                }
            }
            GUILayout.Space(5);

            //===================================================================================================================================================
            // Edit mode selection button
            //---------------------------------------------------------------------------------------------------------------------------------------------------
            // Choose edit mode type
            // 에디터 모드를 결정하는 버튼
            //===================================================================================================================================================
            GUILayout.Label(PathGeneratorGUILanguage.GetLocalText("PG_EditorModeSelect_Label"));
            GUILayout.BeginHorizontal();
            GUI.enabled = pathGenerator.EditMode != 0;
            if (GUILayout.Button(PathGeneratorGUILanguage.GetLocalText("PG_EditorModeSelect_Disable"), GUILayout.Height(25))) {
                Undo.RecordObject(pathGenerator, "Modify " + pathGenerator.gameObject.name);
                pathGenerator.EditMode = 0;
            }
            GUI.enabled = pathGenerator.EditMode != 1;
            if (GUILayout.Button(PathGeneratorGUILanguage.GetLocalText("PG_EditorModeSelect_Individual"), GUILayout.Height(25))) {
                Undo.RecordObject(pathGenerator, "Modify " + pathGenerator.gameObject.name);
                pathGenerator.EditMode = 1;
            }
            GUI.enabled = pathGenerator.EditMode != 2;
            if (GUILayout.Button(PathGeneratorGUILanguage.GetLocalText("PG_EditorModeSelect_Total"), GUILayout.Height(25))) {
                Undo.RecordObject(pathGenerator, "Modify " + pathGenerator.gameObject.name);
                pathGenerator.EditMode = 2;
            }
            GUI.enabled = true;
            GUILayout.EndHorizontal();

            GUILayout.Space(10);
            isShowIndex = GUILayout.Toggle(isShowIndex, PathGeneratorGUILanguage.GetLocalText("PG_ShowLableToggle"));
            GUILayout.Space(10);
            GuiLine();
            GUILayout.Space(10);


            //===================================================================================================================================================
            // Node List
            //---------------------------------------------------------------------------------------------------------------------------------------------------
            // Panel to manage nodes
            // 노드를 관리 할 수 있는 패널
            //===================================================================================================================================================
            GUILayout.Label(PathGeneratorGUILanguage.GetLocalText("PG_NodeList_Label"));
            GUILayout.BeginHorizontal(EditorStyles.toolbar);
            GUILayout.Label(PathGeneratorGUILanguage.GetLocalText("PG_NodeListTable_No"), EditorStyles.toolbarButton, GUILayout.Width(30f));
            GUILayout.Label(PathGeneratorGUILanguage.GetLocalText("PG_NodeListTable_LocalPosition"), 
                                                    EditorStyles.toolbarButton, GUILayout.Width(EditorGUIUtility.currentViewWidth - 175f));
            GUILayout.Label(PathGeneratorGUILanguage.GetLocalText("PG_NodeListTable_Edit"), EditorStyles.toolbarButton, GUILayout.Width(50f));
            GUILayout.Label(PathGeneratorGUILanguage.GetLocalText("PG_NodeListTable_Delete"), EditorStyles.toolbarButton, GUILayout.Width(60f));
            GUILayout.EndHorizontal();

            scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, true,GUILayout.Height(150));
            try {
                // Scroll View Area
                if (pathGenerator.FlagList == null || pathGenerator.FlagList.Count == 0) {
                    Rect rect0 = EditorGUILayout.GetControlRect(false, 300);
                    GUIStyle centerStyle = new GUIStyle("Label");
                    centerStyle.alignment = TextAnchor.MiddleCenter;
                    centerStyle.fixedHeight = 140;
                    GUILayout.BeginArea(rect0);
                    GUILayout.Label(PathGeneratorGUILanguage.GetLocalText("PG_NodeListTable_Empty"), centerStyle);
                    GUILayout.EndArea();
                } else {
                    Rect rect0 = EditorGUILayout.GetControlRect(false, 21 * pathGenerator.FlagList.Count);
                    Rect rect1 = new Rect(rect0.x, rect0.y, EditorGUIUtility.currentViewWidth - 36f, 21 * pathGenerator.FlagList.Count);
                    GUILayout.BeginArea(rect1);

                    for (int i = 0; i < pathGenerator.FlagList.Count; i++) {

                        Vector3 targetPos = pathGenerator.FlagList[i];
                        GUILayout.BeginHorizontal(EditorStyles.toolbar);
                        GUILayout.Label((i+1).ToString(), GUILayout.Width(30f));
                        GUI.enabled = false;
                        EditorGUILayout.Vector3Field("", pathGenerator.FlagList[i], GUILayout.Width(EditorGUIUtility.currentViewWidth - 185f));
                        GUI.enabled = true;
                        if (GUILayout.Button(
                            PathGeneratorGUILanguage.GetLocalText("PG_NodeListTable_Edit"), 
                            EditorStyles.toolbarButton, GUILayout.Width(50f)))
                            DeleteFlagButtonClick(i);
                        if (GUILayout.Button(
                            PathGeneratorGUILanguage.GetLocalText("PG_NodeListTable_DeleteButton"), 
                            EditorStyles.toolbarButton, 
                            GUILayout.Width(59f))) 
                            DeleteFlagButtonClick(i);
                        GUILayout.EndHorizontal();
                    }

                }
            } catch (System.Exception e) {
                Rect rect0 = EditorGUILayout.GetControlRect(false, 300);
                GUIStyle centerStyle = new GUIStyle("Label");
                centerStyle.alignment = TextAnchor.MiddleCenter;
                centerStyle.fixedHeight = 140;
                GUILayout.BeginArea(rect0);
                GUILayout.Label(PathGeneratorGUILanguage.GetLocalText("PG_NodeListTable_Empty"), centerStyle);
                GUILayout.EndArea();
                e.ToString();
            }

            GUILayout.EndScrollView();
            GUILayout.EndArea();


            if (GUILayout.Button("Create node", GUILayout.Height(25f)))
                CreateFlagButtonClick();
        }


        void GuiLine(int i_height = 1) {
            Rect rect = EditorGUILayout.GetControlRect(false, i_height);
            rect.height = i_height;
            EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));
        }

        void CreateFlagButtonClick() {
            PathGenerator pathGenerator = target as PathGenerator;
            Undo.RecordObject(pathGenerator, "Modify " + pathGenerator.gameObject.name);
            if (pathGenerator.FlagList == null || pathGenerator.FlagList.Count < 2 ||
               pathGenerator.AngleList == null || pathGenerator.AngleList.Count < 1)
                return;

            if(pathGenerator.isClosed) {
                Vector3 new_pos = pathGenerator.FlagList[pathGenerator.FlagList.Count - 1] + pathGenerator.FlagList[0];
                new_pos /= 2;
                pathGenerator.FlagList.Add(new_pos);
                new_pos = pathGenerator.FlagList[pathGenerator.FlagList.Count - 2] + 
                                                                pathGenerator.FlagList[pathGenerator.FlagList.Count - 1];
                new_pos /= 2;
                pathGenerator.AngleList[pathGenerator.AngleList.Count - 1] = new_pos;
                new_pos = pathGenerator.FlagList[pathGenerator.FlagList.Count - 1] + pathGenerator.FlagList[0];
                new_pos /= 2;
                pathGenerator.AngleList.Add(new_pos);
            } else {
                Vector3 new_pos = 2 * pathGenerator.FlagList[pathGenerator.FlagList.Count - 1] -
                                     pathGenerator.AngleList[pathGenerator.AngleList.Count - 1];
                pathGenerator.FlagList.Add(new_pos);
                new_pos = pathGenerator.FlagList[pathGenerator.FlagList.Count - 2] +
                                                                pathGenerator.FlagList[pathGenerator.FlagList.Count - 1];
                new_pos /= 2;
                pathGenerator.AngleList.Add(new_pos);
            }
        }

        void DeleteFlagButtonClick(int i) {
            PathGenerator pathGenerator = target as PathGenerator;
            Undo.RecordObject(pathGenerator, "Modify " + pathGenerator.gameObject.name);
            if (pathGenerator.FlagList == null || pathGenerator.FlagList.Count <= 2 ||
               pathGenerator.AngleList == null || pathGenerator.AngleList.Count <= 1)
                return;

            pathGenerator.FlagList.RemoveAt(i);
            pathGenerator.FlagList_Local.RemoveAt(i);
            if(pathGenerator.isClosed || i < pathGenerator.FlagList.Count) {
                pathGenerator.AngleList.RemoveAt(i);
                pathGenerator.AngleList_Local.RemoveAt(i);
            }
        }

        void EditFlagButtonClick(int i) {
            PathGenerator pathGenerator = target as PathGenerator;

        }

        void DeleteAngleButtonClick(int i) {
            PathGenerator pathGenerator = target as PathGenerator;

        }

        void EditAngleButtonClick(int i) {
            PathGenerator pathGenerator = target as PathGenerator;

        }


        //=======================================================================================================================================================
        // OnSceneGUI method
        //-------------------------------------------------------------------------------------------------------------------------------------------------------
        // A function called whenever the GUI is drawn on the screen.
        // 스크린에 GUI가 그려질 때 마다 호출되는 함수
        //=======================================================================================================================================================
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

            if (old_Position != pathGenerator.transform.localPosition ||
                old_Rotation != pathGenerator.transform.localRotation ||
                old_Scale    != pathGenerator.transform.localScale ||
                pathGenerator.FlagList_Local.Count != FlagList.Count ||
                pathGenerator.AngleList_Local.Count != AngleList.Count) {

                old_Position = pathGenerator.transform.localPosition;
                old_Rotation = pathGenerator.transform.localRotation;
                old_Scale = pathGenerator.transform.localScale;

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

            //===================================================================================================================================================
            // Exception handling when FlagList is less than AngleList
            // FlagList가 AngleList보다 작을때의 예외 처리
            //===================================================================================================================================================
            if (FlagList.Count < AngleList.Count) {
                int offset = AngleList.Count - FlagList.Count;
                for (int i = 0; i < offset; i++) {
                    AngleList.RemoveAt(AngleList.Count - 1);
                }
            }

            //===================================================================================================================================================
            // Exception handling when there is only one flag
            // Flag가 하나 밖에 없거나 Anglelist가 비어있을때의 예외 처리
            //===================================================================================================================================================
            if (FlagList.Count < 2) {
                FlagList.Clear();
                FlagList.Add(new Vector3(Random.Range(-3f, 3f), 0f, Random.Range(-3f, 3f)));
                FlagList.Add(new Vector3(Random.Range(-3f, 3f), 0f, Random.Range(-3f, 3f)));
                Count = 2;
            }

            //===================================================================================================================================================
            // Exception handling when AngleList size is less than FlagList size
            // AngleList 크기가 FlagList보다 작은 경우 예외 처리
            //===================================================================================================================================================
            if (pathGenerator.isClosed && AngleList.Count < FlagList.Count) {
                AngleList.Clear();
                for(int i = 0; i < Count; i++)
                    AngleList.Add(new Vector3(Random.Range(-3f, 3f), 0f, Random.Range(-3f, 3f)));
            } else if (!pathGenerator.isClosed && AngleList.Count < FlagList.Count-1) {
                AngleList.Clear();
                for (int i = 0; i < Count - 1; i++)
                    AngleList.Add(new Vector3(Random.Range(-3f, 3f), 0f, Random.Range(-3f, 3f)));
            }

            //===================================================================================================================================================
            // Check objects of List. If it's not in the scene, create at the midpoint between the flags.
            // List 기반으로 오브젝트를 검사. 만약, Scene에 없으면 두 Flag 사이의 중점에 생성
            //===================================================================================================================================================
            for (int i = 0; i < Count; i++) {
                if ((i == Count - 1) && (!pathGenerator.isClosed))
                    continue;
                if (AngleList[i] == null) {
                    Vector3 nextFlag = (i == Count - 1) ? FlagList[0] : FlagList[i + 1];
                    AngleList[i]
                        = (FlagList[i] + nextFlag) / 2;
                }
            }

            //===================================================================================================================================================
            // If it is a closed loop, connect the last and first points
            // 닫힌 루프라면, 마지막과 첫 점을 이어줌
            //===================================================================================================================================================
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

            //===================================================================================================================================================
            // Calculate Bézier curve
            // 베지어 커브 계산
            //===================================================================================================================================================
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


            if(isShowIndex) {
                if (pathGenerator.FlagList_Local != null && pathGenerator.FlagList_Local.Count > 0) {
                    for (int i = 0; i < pathGenerator.FlagList_Local.Count; i++)
                        DrawTextLabelOnScene(pathGenerator.FlagList_Local[i], Color.green, "[ " + (i+1) + " ]");
                }

                if (pathGenerator.AngleList_Local != null && pathGenerator.AngleList_Local.Count > 0) {
                    for (int i = 0; i < pathGenerator.AngleList_Local.Count; i++)
                        DrawTextLabelOnScene(pathGenerator.AngleList_Local[i], Color.yellow,
                            ( pathGenerator.isClosed && i == pathGenerator.AngleList_Local.Count - 1 ) ? 
                                                            ( (i + 1) + " → 1") : (( i + 1 ) + " → " + ( i + 2 ) ));
                }
            }
        }

        private void DrawTextLabelOnScene(Vector3 worldPos, Color TextColor, string Text) {
            Vector2 guiLoc = HandleUtility.WorldToGUIPoint(worldPos);  // 오브젝트의 월드좌표를 2D 좌표로 변환
            var rect = new Rect(guiLoc.x - 20f, guiLoc.y + 10, 40, 20);    // 라벨 위치 지정
            Handles.BeginGUI();
            Color oldBgColor = GUI.backgroundColor;
            Color oldTextColor = GUI.skin.box.normal.textColor;
            GUI.skin.box.normal.textColor = TextColor;
            GUI.backgroundColor = new Color(0, 0, 0, 0.7f);     // 배경 색 지정
            GUI.Box(rect, Text);      // Box UI 표시
            GUI.backgroundColor = oldBgColor;
            GUI.skin.box.normal.textColor = oldTextColor;
            Handles.EndGUI();
        }


        public void OnEnable() {
            PathGeneratorGUILanguage.InitLocalization();
        }
    }

}
