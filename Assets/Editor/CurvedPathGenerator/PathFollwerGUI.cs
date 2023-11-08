//=========================================================================================================================================
//      ,------.   ,---. ,--------.,--.  ,--.     ,----.   ,------.,--.  ,--.,------.,------.   ,---. ,--------. ,-----. ,------.
//      |  .--. ' /  O  \'--.  .--'|  '--'  |    '  .-./   |  .---'|  ,'.|  ||  .---'|  .--. ' /  O  \'--.  .--''  .-.  '|  .--. '
//      |  '--' ||  .-.  |  |  |   |  .--.  |    |  | .---.|  `--, |  |' '  ||  `--, |  '--'.'|  .-.  |  |  |   |  | |  ||  '--'.'
//      |  | --' |  | |  |  |  |   |  |  |  |    '  '--'  ||  `---.|  | `   ||  `---.|  |\  \ |  | |  |  |  |   '  '-'  '|  |\  \
//      `--'     `--' `--'  `--'   `--'  `--'     `------' `------'`--'  `--'`------'`--' '--'`--' `--'  `--'    `-----' `--' '--'
//=========================================================================================================================================
//
//  PATH FOLLOWER GUI CLASS
//
//  A follower script GUI that GameObject to follow the created path
//  GameObject가생성된 path를 따라다닐 수 있도록 하는 follwer script GUI
//
//-----------------------------------------------------------------------------------------------------------------------------------------
//  2023.11.04 _ KimYC1223
//=========================================================================================================================================
using UnityEditor;
using UnityEngine;

#region PathFollwerGUI

namespace CurvedPathGenerator
{
    /// <summary>
    /// A follower script GUI that GameObject to follow the created path
    /// </summary>
    [CustomEditor(typeof(PathFollower))]
    internal class PathFollowerGUI : Editor
    {
        #region PathFollower_GUI_Variables

        /// <summary>
        /// GUI style : H1
        /// </summary>
        private GUIStyle H1Text;

        /// <summary>
        /// GUI style : BoldText
        /// </summary>
        private GUIStyle BoldText;

        /// <summary>
        /// Event to be executed at the end of the loop
        /// </summary>
        private SerializedProperty endEvent;

        /// <summary>
        /// GUI style : Component Title
        /// </summary>
        private GUIStyle ComponentTitle;

        #endregion PathFollower_GUI_Variables

        #region PathFollower_InspectorUI_Main

        //=================================================================================================================================
        // OnSceneGUI method
        //---------------------------------------------------------------------------------------------------------------------------------
        // A function called whenever the inspector is drawn.
        // 인스펙터가 그려질 때 마다 호출되는 함수
        //=================================================================================================================================
        /// <summary>
        /// A function called whenever the inspector is drawn.
        /// </summary>
        public override void OnInspectorGUI()
        {
            #region PathFollower_InspectorUI_Main_StartsUp

            PathFollower pathFollower = target as PathFollower;

            //=============================================================================================================================
            //  GUI Styles
            //-----------------------------------------------------------------------------------------------------------------------------
            //  Define and set GUI styles.
            //  GUI 스타일 정의.
            //=============================================================================================================================
            if ( ComponentTitle == null )
            {
                ComponentTitle = new GUIStyle(EditorStyles.label);
                ComponentTitle.normal.textColor = new Color(0f, 0.3882f, 0.9725f, 1f);
                ComponentTitle.fontSize = 17;
                ComponentTitle.fontStyle = FontStyle.Bold;
            }

            if ( H1Text == null )
            {
                H1Text = new GUIStyle(EditorStyles.label);
                H1Text.fontStyle = FontStyle.Bold;
                H1Text.fontSize = 15;
            }

            if ( BoldText == null )
            {
                BoldText = new GUIStyle(EditorStyles.label);
                BoldText.fontStyle = FontStyle.Bold;
            }

            #endregion PathFollower_InspectorUI_Main_StartsUp

            #region PathFollower_InspectorUI_Main_Header

            //=============================================================================================================================
            // Title Image
            //-----------------------------------------------------------------------------------------------------------------------------
            //  Draw head image.
            //  헤드 이미지 그리기.
            //=============================================================================================================================
            Texture LogoTex = (Texture2D)Resources.Load("PathFollowerScriptImg", typeof(Texture2D));
            GUILayout.Label(LogoTex, GUILayout.Width(300f), GUILayout.Height(67.5f));

            //=============================================================================================================================
            //  Language setting buttons
            //-----------------------------------------------------------------------------------------------------------------------------
            //  Supports English, Korean and Japanese. Chinese will be added later.
            //  영어, 한국어, 일본어를 지원함. 중국어는 추후 추가 예정.
            //=============================================================================================================================
            GUILayout.BeginHorizontal();
            GUI.enabled = PathGeneratorGUILanguage.CurrentLanguage != LANGUAGE.ENG;
            if ( GUILayout.Button("English", GUILayout.Height(22f)) )
            {
                Undo.RecordObject(pathFollower, "Modify " + pathFollower.gameObject.name);
                PathGeneratorGUILanguage.CurrentLanguage = LANGUAGE.ENG;
            }
            GUI.enabled = PathGeneratorGUILanguage.CurrentLanguage != LANGUAGE.KOR;
            if ( GUILayout.Button("한국어", GUILayout.Height(22f)) )
            {
                Undo.RecordObject(pathFollower, "Modify " + pathFollower.gameObject.name);
                PathGeneratorGUILanguage.CurrentLanguage = LANGUAGE.KOR;
            }
            GUI.enabled = PathGeneratorGUILanguage.CurrentLanguage != LANGUAGE.JAP;
            if ( GUILayout.Button("日本語", GUILayout.Height(22f)) )
            {
                Undo.RecordObject(pathFollower, "Modify " + pathFollower.gameObject.name);
                PathGeneratorGUILanguage.CurrentLanguage = LANGUAGE.JAP;
            }
            GUI.enabled = true;
            GUILayout.EndHorizontal();
            GUILayout.Space(8f);

            //=============================================================================================================================
            //  Language setting buttons
            //-----------------------------------------------------------------------------------------------------------------------------
            //  Display component information and developer information.
            //  컴포포넌트 정보와 개발자 정보를 표시,
            //=============================================================================================================================
            GUILayout.Label(PathGeneratorGUILanguage.GetLocalText("PF_Title"), ComponentTitle);
            GUI.enabled = false;
            GUILayout.Label(PathGeneratorGUILanguage.GetLocalText("PF_SubTitle"));
            GUI.enabled = true;
            GUILayout.Space(15f);
            GuiLine();
            GUILayout.Space(15f);

            #endregion PathFollower_InspectorUI_Main_Header

            #region PathFollower_InspectorUI_Main_Info

            //=============================================================================================================================
            // Inf
            //-----------------------------------------------------------------------------------------------------------------------------
            // Movement information
            // 움직임 정보
            //=============================================================================================================================
            GUILayout.Label(PathGeneratorGUILanguage.GetLocalText("PF_H1_Info"), H1Text);
            GUILayout.Space(3f);
            GUILayout.Label(PathGeneratorGUILanguage.GetLocalText("PF_Info_Label"));
            GUILayout.Space(10f);

            //=============================================================================================================================
            //  Path Generator component
            //  Path Generator 컴포넌트
            //=============================================================================================================================
            GUILayout.BeginHorizontal();
            GUILayout.Label(PathGeneratorGUILanguage.GetLocalText("PF_Info_Path"), GUILayout.Width(120f));
            GUILayout.Space(15f);
            Undo.RecordObject(pathFollower, "Modify " + pathFollower.gameObject.name);
            pathFollower.Generator = EditorGUILayout.ObjectField(pathFollower.Generator, typeof(PathGenerator), true) as PathGenerator;
            GUILayout.EndHorizontal();
            GUILayout.Space(10f);

            //=============================================================================================================================
            //  Movement Speed
            //  이동 속력
            //=============================================================================================================================
            GUILayout.BeginHorizontal();
            GUILayout.Label(PathGeneratorGUILanguage.GetLocalText("PF_Info_Speed"), GUILayout.Width(120f));
            GUILayout.Space(15f);
            Undo.RecordObject(pathFollower, "Modify " + pathFollower.gameObject.name);
            pathFollower.Speed = EditorGUILayout.Slider(pathFollower.Speed, 0, 600f);
            GUILayout.EndHorizontal();
            GUILayout.Space(5f);

            //=============================================================================================================================
            //  Arrival distance threshold
            //  도착 거리 임계값
            //=============================================================================================================================
            GUILayout.BeginHorizontal();
            GUILayout.Label(PathGeneratorGUILanguage.GetLocalText("PF_Info_Threshold"), GUILayout.Width(120f));
            GUILayout.Space(15f);
            Undo.RecordObject(pathFollower, "Modify " + pathFollower.gameObject.name);
            pathFollower.DistanceThreshold = EditorGUILayout.Slider(pathFollower.DistanceThreshold, 0.001f, 100f);
            GUILayout.EndHorizontal();
            GUILayout.Space(2f);

            EditorGUILayout.HelpBox(PathGeneratorGUILanguage.GetLocalText("PF_Info_Warning"), MessageType.Info);
            GUILayout.Space(5f);

            //=============================================================================================================================
            //  Turning Speed
            //  회전 속력
            //=============================================================================================================================
            GUILayout.BeginHorizontal();
            GUILayout.Label(PathGeneratorGUILanguage.GetLocalText("PF_Info_TurningSpeed"), GUILayout.Width(120f));
            GUILayout.Space(15f);
            Undo.RecordObject(pathFollower, "Modify " + pathFollower.gameObject.name);
            pathFollower.TurningSpeed = EditorGUILayout.Slider(pathFollower.TurningSpeed, 0.1f, 100f);
            GUILayout.EndHorizontal();
            GUILayout.Space(10f);

            //=============================================================================================================================
            //  Moving options
            //  움직임 옵션
            //=============================================================================================================================
            GUILayout.BeginHorizontal();
            Undo.RecordObject(pathFollower, "Modify " + pathFollower.gameObject.name);
            pathFollower.IsMove = GUILayout.Toggle(pathFollower.IsMove, PathGeneratorGUILanguage.GetLocalText("PF_Info_IsMove"));
            GUILayout.Space(15f);
            Undo.RecordObject(pathFollower, "Modify " + pathFollower.gameObject.name);
            pathFollower.IsLoop = GUILayout.Toggle(pathFollower.IsLoop, PathGeneratorGUILanguage.GetLocalText("PF_Info_IsLoop"));
            GUILayout.EndHorizontal();
            GUILayout.Space(19f);
            GuiLine();
            GUILayout.Space(15f);

            #endregion PathFollower_InspectorUI_Main_Info

            #region PathFollower_InspectorUI_Main_EventHandler

            //=============================================================================================================================
            //  End event handler
            //-----------------------------------------------------------------------------------------------------------------------------
            //  Manage the method to be executed when the route is completed
            //  경로를 완주 했을 때 실행되는 메소드 관리
            //=============================================================================================================================
            GUILayout.Label(PathGeneratorGUILanguage.GetLocalText("PF_H1_Events"), H1Text);
            GUILayout.Space(3f);
            GUILayout.Label(PathGeneratorGUILanguage.GetLocalText("PF_Events_Label"));
            GUILayout.Space(10f);

            Undo.RecordObject(pathFollower, "Modify " + pathFollower.gameObject.name);
            pathFollower.IsEndEventEnable =
                GUILayout.Toggle(pathFollower.IsEndEventEnable, PathGeneratorGUILanguage.GetLocalText("PF_Events_endEventLabel"));
            if ( pathFollower.IsEndEventEnable )
            {
                GUILayout.Space(10f);
                serializedObject.Update();
                EditorGUILayout.PropertyField(endEvent);
                serializedObject.ApplyModifiedProperties();
            }
            GUILayout.Space(35f);

            #endregion PathFollower_InspectorUI_Main_EventHandler
        }

        #endregion PathFollower_InspectorUI_Main

        #region PathFollower_InspectorUI_Main_Functions

        #region PathFollower_InspectorUI_Main_Functions_OnEnable

        //=================================================================================================================================
        // OnEnable method
        //---------------------------------------------------------------------------------------------------------------------------------
        // Methods executed when selected in the inspector window
        // 인스펙터 창에서 선택 되었을 때 실행되는 메소드
        //=================================================================================================================================
        /// <summary>
        /// Methods executed when selected in the inspector window
        /// </summary>
        public void OnEnable()
        {
            PathGeneratorGUILanguage.InitLocalization();                    // Language setting
            endEvent = serializedObject.FindProperty("EndEvent");           // for event handler
        }

        #endregion PathFollower_InspectorUI_Main_Functions_OnEnable

        #region PathFollower_InspectorUI_Main_Functions_GuiLine

        //=================================================================================================================================
        //  GUI Line method
        //---------------------------------------------------------------------------------------------------------------------------------
        //  draw a line on GUI Layout
        //  GUI Layout에 선을 그림
        //=================================================================================================================================
        /// <summary>
        /// draw a line on GUI Layout
        /// </summary>
        /// <param name="i_height">height of line</param>
        private void GuiLine(int i_height = 1)
        {
            Rect rect = EditorGUILayout.GetControlRect(false, i_height);
            rect.height = i_height;
            EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));
        }

        #endregion PathFollower_InspectorUI_Main_Functions_GuiLine

        #endregion PathFollower_InspectorUI_Main_Functions
    }
}

#endregion PathFollwerGUI