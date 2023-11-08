//==========================================================================================================================================
//      ,------.   ,---. ,--------.,--.  ,--.     ,----.   ,------.,--.  ,--.,------.,------.   ,---. ,--------. ,-----. ,------.
//      |  .--. ' /  O  \'--.  .--'|  '--'  |    '  .-./   |  .---'|  ,'.|  ||  .---'|  .--. ' /  O  \'--.  .--''  .-.  '|  .--. '
//      |  '--' ||  .-.  |  |  |   |  .--.  |    |  | .---.|  `--, |  |' '  ||  `--, |  '--'.'|  .-.  |  |  |   |  | |  ||  '--'.'
//      |  | --' |  | |  |  |  |   |  |  |  |    '  '--'  ||  `---.|  | `   ||  `---.|  |\  \ |  | |  |  |  |   '  '-'  '|  |\  \
//      `--'     `--' `--'  `--'   `--'  `--'     `------' `------'`--'  `--'`------'`--' '--'`--' `--'  `--'    `-----' `--' '--'
//=========================================================================================================================================
//
//  PATH GENERATOR GUI LANGUAGE CLASS
//
//  GUI language management class
//  GUI 언어 관리 클래스
//
//-----------------------------------------------------------------------------------------------------------------------------------------
//  2022.11.04 _ KimYC1223
//=========================================================================================================================================
using System.Collections.Generic;

namespace CurvedPathGenerator
{
    //=====================================================================================================================================
    // Supported languages  |  Chinese support coming soon
    // 지원되는 언어        |  중국어 지원 추가 예정
    //=====================================================================================================================================
    /// <summary>
    /// Supported languages
    /// </summary>
    public enum LANGUAGE
    { ENG, KOR, JAP };

    /// <summary>
    /// PATH GENERATOR GUI LANGUAGE CLASS
    /// </summary>
    public class PathGeneratorGUILanguage
    {
        /// <summary>
        /// currently set language
        /// </summary>
        public static LANGUAGE CurrentLanguage = LANGUAGE.ENG;

        /// <summary>
        /// english text key-value dictionary
        /// </summary>
        private static Dictionary<string, string> ENG_TEXT;

        /// <summary>
        /// Korean text key-value dictionary
        /// </summary>
        private static Dictionary<string, string> KOR_TEXT;

        /// <summary>
        /// Japanese text key-value dictionary
        /// </summary>
        private static Dictionary<string, string> JAP_TEXT;

        //=================================================================================================================================
        // Init Localization method
        //---------------------------------------------------------------------------------------------------------------------------------
        // Initialize language-specific Dictionary variables
        // 언어별 Dictionary 변수 초기화
        //=================================================================================================================================
        /// <summary>
        /// Initialize language-specific Dictionary variables
        /// </summary>
        public static void InitLocalization()
        {
            string key_text;
            if ( ENG_TEXT != null && KOR_TEXT != null && JAP_TEXT != null )
                return;

            ENG_TEXT = new Dictionary<string, string>();
            KOR_TEXT = new Dictionary<string, string>();
            JAP_TEXT = new Dictionary<string, string>();

            //=============================================================================================================================
            //  [PG] Path Generator GUI text settings
            //  [PG] Path Generator GUI 텍스트 설정
            //=============================================================================================================================
            key_text = "PG_Title";
            ENG_TEXT.Add(key_text, "Curved Path Generator");
            KOR_TEXT.Add(key_text, "곡선 경로 생성기");
            JAP_TEXT.Add(key_text, "カーブパスジェネレータ");

            key_text = "PG_SubTitle";
            ENG_TEXT.Add(key_text, "Developed by KimYC1223");
            KOR_TEXT.Add(key_text, "KimYC1223 제공");
            JAP_TEXT.Add(key_text, "KimYC1223 によって開発されました");

            key_text = "PG_Node";
            ENG_TEXT.Add(key_text, "Node");
            KOR_TEXT.Add(key_text, "노드");
            JAP_TEXT.Add(key_text, "ノード");

            key_text = "PG_Angle";
            ENG_TEXT.Add(key_text, "Angle");
            KOR_TEXT.Add(key_text, "앵글");
            JAP_TEXT.Add(key_text, "アングル");

            key_text = "PG_Center";
            ENG_TEXT.Add(key_text, "Move to center");
            KOR_TEXT.Add(key_text, "중점으로 이동");
            JAP_TEXT.Add(key_text, "中央に移動");

            key_text = "PG_Close";
            ENG_TEXT.Add(key_text, "Close");
            KOR_TEXT.Add(key_text, "닫기");
            JAP_TEXT.Add(key_text, "閉じる");

            key_text = "PG_PathDensity";
            ENG_TEXT.Add(key_text, "Path density");
            KOR_TEXT.Add(key_text, "경로 밀도");
            JAP_TEXT.Add(key_text, "パス密度");

            key_text = "PG_PathTypeChangeButton_ToOpen";
            ENG_TEXT.Add(key_text, "Change to opened path");
            KOR_TEXT.Add(key_text, "열린 패스로 변경");
            JAP_TEXT.Add(key_text, "オープンパスに変更");

            key_text = "PG_PathTypeChangeButton_ToClose";
            ENG_TEXT.Add(key_text, "Change to closed path");
            KOR_TEXT.Add(key_text, "닫힌 패스로 변경");
            JAP_TEXT.Add(key_text, "クローズドパスに変更");

            key_text = "PG_PathTypeChangeButton_isLivePath";
            ENG_TEXT.Add(key_text, "Update path in runtime");
            KOR_TEXT.Add(key_text, "런타임에 경로 업데이트");
            JAP_TEXT.Add(key_text, "ランタイムにパスアップデート");

            key_text = "PG_H1_EditorSetting";
            ENG_TEXT.Add(key_text, "Editor setting");
            KOR_TEXT.Add(key_text, "에디터 관련");
            JAP_TEXT.Add(key_text, "エディタ関連");

            key_text = "PG_PathTypeChangeButton_isLivePathWarning";
            ENG_TEXT.Add(key_text, "Updates the path every frame. Therefore, even if the node or angle changes position at runtime," +
                                    "it is applied to the path immediately. However, the amount of calculation may increase.");
            KOR_TEXT.Add(key_text, "매 프레임마다 경로를 업데이트 합니다. 따라서, 런타임에서 Node나 Angle의 위치가 바뀌어도" +
                                    "즉시 경로에 적용됩니다. 다만 계산량이 많아 질 수 있습니다.");
            JAP_TEXT.Add(key_text, "フレームごとにパスを更新します。 したがって、ランタイムでノードやアングルの" +
                                    "位置が変わっても直ちに経路に適用されます。 ただし、計算量が多くなる可能性があります。");

            key_text = "PG_EditorModeSelect_Label";
            ENG_TEXT.Add(key_text, "Node & Anchor editor mode");
            KOR_TEXT.Add(key_text, "노드 & 앵커 에디터 모드 선택");
            JAP_TEXT.Add(key_text, "ノード&アンカーエディタモードを選択");

            key_text = "PG_EditorModeSelect_Disable";
            ENG_TEXT.Add(key_text, "Normal Mode");
            KOR_TEXT.Add(key_text, "일반 제어");
            JAP_TEXT.Add(key_text, "一般制御");

            key_text = "PG_EditorModeSelect_Individual";
            ENG_TEXT.Add(key_text, "Individual");
            KOR_TEXT.Add(key_text, "개별 제어");
            JAP_TEXT.Add(key_text, "個別制御");

            key_text = "PG_EditorModeSelect_Total";
            ENG_TEXT.Add(key_text, "Total");
            KOR_TEXT.Add(key_text, "전체 제어");
            JAP_TEXT.Add(key_text, "全体制御");

            key_text = "PG_ShowLabel_Label";
            ENG_TEXT.Add(key_text, "Visual options : ");
            KOR_TEXT.Add(key_text, "Scene에서 ...");
            JAP_TEXT.Add(key_text, "シーンで ...");

            key_text = "PG_ShowLabelToggle";
            ENG_TEXT.Add(key_text, "Show labels");
            KOR_TEXT.Add(key_text, "레이블 그리기");
            JAP_TEXT.Add(key_text, "ラベルを見せる");

            key_text = "PG_ShowIconsToggle";
            ENG_TEXT.Add(key_text, "Show icons");
            KOR_TEXT.Add(key_text, "아이콘 그리기");
            JAP_TEXT.Add(key_text, "アイコンを見せる");

            key_text = "PG_TopViewModeButton_toTop";
            ENG_TEXT.Add(key_text, "Change to top view mode");
            KOR_TEXT.Add(key_text, "탑 뷰 모드로 변경");
            JAP_TEXT.Add(key_text, "トップビューモードに変更");

            key_text = "PG_TopViewModeButton_Reset";
            ENG_TEXT.Add(key_text, "Reset view mode");
            KOR_TEXT.Add(key_text, "뷰 모드 리셋");
            JAP_TEXT.Add(key_text, "ビューモードリセット");

            key_text = "PG_Colors_Label";
            ENG_TEXT.Add(key_text, "Guideline Colors");
            KOR_TEXT.Add(key_text, "가이드라인 색상");
            JAP_TEXT.Add(key_text, "ガイドラインの色");

            key_text = "PG_H1_Node";
            ENG_TEXT.Add(key_text, "Nodes");
            KOR_TEXT.Add(key_text, "노드");
            JAP_TEXT.Add(key_text, "ノード");

            key_text = "PG_NodeList_Label";
            ENG_TEXT.Add(key_text, "Node List");
            KOR_TEXT.Add(key_text, "노드 리스트");
            JAP_TEXT.Add(key_text, "ノードリスト");

            key_text = "PG_NodeListTable_No";
            ENG_TEXT.Add(key_text, "No");
            KOR_TEXT.Add(key_text, "번호");
            JAP_TEXT.Add(key_text, "番号");

            key_text = "PG_NodeListTable_LocalPosition";
            ENG_TEXT.Add(key_text, "Local position");
            KOR_TEXT.Add(key_text, "로컬 포지션");
            JAP_TEXT.Add(key_text, "ローカル·ポジション");

            key_text = "PG_NodeListTable_Edit";
            ENG_TEXT.Add(key_text, "Edit");
            KOR_TEXT.Add(key_text, "변경");
            JAP_TEXT.Add(key_text, "辺境");

            key_text = "PG_NodeListTable_Delete";
            ENG_TEXT.Add(key_text, "Delete");
            KOR_TEXT.Add(key_text, "삭제");
            JAP_TEXT.Add(key_text, "削除");

            key_text = "PG_NodeListTable_DeleteButton";
            ENG_TEXT.Add(key_text, "[-]");
            KOR_TEXT.Add(key_text, "[-]");
            JAP_TEXT.Add(key_text, "[-]");

            key_text = "PG_NodeListTable_Empty";
            ENG_TEXT.Add(key_text, "Empty");
            KOR_TEXT.Add(key_text, "비어있음");
            JAP_TEXT.Add(key_text, "空");

            key_text = "PG_NodeList_CreateNodeButton";
            ENG_TEXT.Add(key_text, "[+] Create node");
            KOR_TEXT.Add(key_text, "[+] 노드 생성");
            JAP_TEXT.Add(key_text, "[+] ノード生成");

            key_text = "PG_H1_Angle";
            ENG_TEXT.Add(key_text, "Angles");
            KOR_TEXT.Add(key_text, "앵글");
            JAP_TEXT.Add(key_text, "アングル");

            key_text = "PG_AngleList_Label";
            ENG_TEXT.Add(key_text, "Angle List");
            KOR_TEXT.Add(key_text, "앵글 리스트");
            JAP_TEXT.Add(key_text, "アングルリスト");

            key_text = "PG_AngleList_From";
            ENG_TEXT.Add(key_text, "From");
            KOR_TEXT.Add(key_text, "기점");
            JAP_TEXT.Add(key_text, "起点");

            key_text = "PG_AngleList_To";
            ENG_TEXT.Add(key_text, "To");
            KOR_TEXT.Add(key_text, "종점");
            JAP_TEXT.Add(key_text, "終点");

            key_text = "PG_TotalControl";
            ENG_TEXT.Add(key_text, "Total Control");
            KOR_TEXT.Add(key_text, "전체 제어");
            JAP_TEXT.Add(key_text, "全体制御");

            key_text = "PG_TotalControl_Label";
            ENG_TEXT.Add(key_text, "for the all nodes & angles...");
            KOR_TEXT.Add(key_text, "모든 노드와 앵글들에 대해...");
            JAP_TEXT.Add(key_text, "すべてのノードとアングルに対して...");

            key_text = "PG_TotalControl_SetZeroToX";
            ENG_TEXT.Add(key_text, "X to 0");
            KOR_TEXT.Add(key_text, "X를\n0으로");
            JAP_TEXT.Add(key_text, "Xを0に");

            key_text = "PG_TotalControl_SetZeroToY";
            ENG_TEXT.Add(key_text, "Y to 0");
            KOR_TEXT.Add(key_text, "Y를\n0으로");
            JAP_TEXT.Add(key_text, "Yを0に");

            key_text = "PG_TotalControl_SetZeroToZ";
            ENG_TEXT.Add(key_text, "Z to 0");
            KOR_TEXT.Add(key_text, "Z를\n0으로");
            JAP_TEXT.Add(key_text, "Zを0に");

            key_text = "PG_TotalControl_SetAvgToX";
            ENG_TEXT.Add(key_text, "X\nequalization");
            KOR_TEXT.Add(key_text, "X\n평준화");
            JAP_TEXT.Add(key_text, "X\n平準化");

            key_text = "PG_TotalControl_SetAvgToY";
            ENG_TEXT.Add(key_text, "Y\nequalization");
            KOR_TEXT.Add(key_text, "Y\n평준화");
            JAP_TEXT.Add(key_text, "Y\n平準化");

            key_text = "PG_TotalControl_SetAvgToZ";
            ENG_TEXT.Add(key_text, "Z\nequalization");
            KOR_TEXT.Add(key_text, "Z\n평준화");
            JAP_TEXT.Add(key_text, "Z\n平準化");

            key_text = "PG_TotalControl_SpecificValue";
            ENG_TEXT.Add(key_text, "Specific value");
            KOR_TEXT.Add(key_text, "특정값");
            JAP_TEXT.Add(key_text, "特定値");

            key_text = "PG_TotalControl_SetSpecificToX";
            ENG_TEXT.Add(key_text, "X to ...");
            KOR_TEXT.Add(key_text, "X를 ...");
            JAP_TEXT.Add(key_text, "Xをこの ...");

            key_text = "PG_TotalControl_SetSpecificToY";
            ENG_TEXT.Add(key_text, "Y to ...");
            KOR_TEXT.Add(key_text, "Y를 ...");
            JAP_TEXT.Add(key_text, "Yをこの ...");

            key_text = "PG_TotalControl_SetSpecificToZ";
            ENG_TEXT.Add(key_text, "Z to ...");
            KOR_TEXT.Add(key_text, "Z을 ...");
            JAP_TEXT.Add(key_text, "Zをこの ...");

            key_text = "PG_H1_Rendering";
            ENG_TEXT.Add(key_text, "Rendering");
            KOR_TEXT.Add(key_text, "렌더링");
            JAP_TEXT.Add(key_text, "レンダリング");

            key_text = "PG_H1_Rendering_Label";
            ENG_TEXT.Add(key_text, "A visual representation of the path.");
            KOR_TEXT.Add(key_text, "경로를 가시적으로 표현 할 수 있습니다.");
            JAP_TEXT.Add(key_text, "パスを可視的に表現することができます。");

            key_text = "PG_Rendering_isGeneratePathMesh";
            ENG_TEXT.Add(key_text, "Generate path mesh in runtime.");
            KOR_TEXT.Add(key_text, "런타임에 경로 메쉬를 생성합니다.");
            JAP_TEXT.Add(key_text, "ランタイムにパスメッシュを生成する。");

            key_text = "PG_Rendering_MaterialInfo";
            ENG_TEXT.Add(key_text, "Material Infomation");
            KOR_TEXT.Add(key_text, "머터리얼 정보");
            JAP_TEXT.Add(key_text, "マテリアル情報");

            key_text = "PG_Rendering_LineMeshWidth";
            ENG_TEXT.Add(key_text, "Width of line mesh");
            KOR_TEXT.Add(key_text, "경로 메쉬의 두께");
            JAP_TEXT.Add(key_text, "ラインメッシュの幅");

            key_text = "PG_Rendering_LineTexture";
            ENG_TEXT.Add(key_text, "Texture of\nline mesh");
            KOR_TEXT.Add(key_text, "경로 메쉬의\n텍스처");
            JAP_TEXT.Add(key_text, "ライン\nメッシュの\nテクスチャ");

            key_text = "PG_Rendering_ScrollSpeed";
            ENG_TEXT.Add(key_text, "Scroll speed");
            KOR_TEXT.Add(key_text, "스크롤 속도");
            JAP_TEXT.Add(key_text, "スクロール速度");

            key_text = "PG_Rendering_Opacity";
            ENG_TEXT.Add(key_text, "Opacity");
            KOR_TEXT.Add(key_text, "불투명도");
            JAP_TEXT.Add(key_text, "不透明度");

            key_text = "PG_Rendering_Tiling";
            ENG_TEXT.Add(key_text, "Tiling");
            KOR_TEXT.Add(key_text, "타일링");
            JAP_TEXT.Add(key_text, "タイリング");

            key_text = "PG_Rendering_Filling";
            ENG_TEXT.Add(key_text, "Filling");
            KOR_TEXT.Add(key_text, "채우기");
            JAP_TEXT.Add(key_text, "詰める");

            key_text = "PG_Rendering_RenderQueue";
            ENG_TEXT.Add(key_text, "Render queue");
            KOR_TEXT.Add(key_text, "렌더 큐");
            JAP_TEXT.Add(key_text, "レンダリングキュー");

            key_text = "PG_Rendering_RenderQueueHelp";
            ENG_TEXT.Add(key_text, "Render queue default value is 2500");
            KOR_TEXT.Add(key_text, "렌더 큐의 기본값은 2500입니다.");
            JAP_TEXT.Add(key_text, "レンダリング キューのデフォルト値は 2500 です。");

            //=============================================================================================================================
            //  [PF] Path Follower GUI text settings
            //  [PF] Path Follower GUI 텍스트 설정
            //=============================================================================================================================
            key_text = "PF_Title";
            ENG_TEXT.Add(key_text, "Curved Path Follower");
            KOR_TEXT.Add(key_text, "곡선 경로 팔로워");
            JAP_TEXT.Add(key_text, "カーブパスフォロワー");

            key_text = "PF_SubTitle";
            ENG_TEXT.Add(key_text, "Developed by KimYC1223");
            KOR_TEXT.Add(key_text, "KimYC1223 제공");
            JAP_TEXT.Add(key_text, "KimYC1223 によって開発されました");

            key_text = "PF_H1_Info";
            ENG_TEXT.Add(key_text, "Movement info");
            KOR_TEXT.Add(key_text, "이동 정보");
            JAP_TEXT.Add(key_text, "移動情報");

            key_text = "PF_Info_Label";
            ENG_TEXT.Add(key_text, "Movement information for an object");
            KOR_TEXT.Add(key_text, "경로를 따라 움직이는 물체의 이동 정보");
            JAP_TEXT.Add(key_text, "経路に沿って移動する物体の移動情報");

            key_text = "PF_Info_Speed";
            ENG_TEXT.Add(key_text, "Speed");
            KOR_TEXT.Add(key_text, "속력");
            JAP_TEXT.Add(key_text, "スピード");

            key_text = "PF_Info_Threshold";
            ENG_TEXT.Add(key_text, "Distance threshold");
            KOR_TEXT.Add(key_text, "거리 임계값");
            JAP_TEXT.Add(key_text, "距離臨界値");

            key_text = "PF_Info_Warning";
            ENG_TEXT.Add(key_text, "Too fast speed or too low a distance threshold can interfere with normal operation.");
            KOR_TEXT.Add(key_text, "너무 빠른 속도나 너무 낮은 거리 임계값은 정상적인 동작을 방해 할 수 있습니다.");
            JAP_TEXT.Add(key_text, "早すぎるか、低すぎる距離臨界値は、正常な動作を妨げる可能性があります。。");

            key_text = "PF_Info_TurningSpeed";
            ENG_TEXT.Add(key_text, "Turning speed");
            KOR_TEXT.Add(key_text, "회전 속력");
            JAP_TEXT.Add(key_text, "回転速力");

            key_text = "PF_Info_IsLoop";
            ENG_TEXT.Add(key_text, "Is loop");
            KOR_TEXT.Add(key_text, "반복 활성화");
            JAP_TEXT.Add(key_text, "反復活性化");

            key_text = "PF_Info_IsMove";
            ENG_TEXT.Add(key_text, "Is move");
            KOR_TEXT.Add(key_text, "이동 활성화");
            JAP_TEXT.Add(key_text, "移動活性化");

            key_text = "PF_Info_Path";
            ENG_TEXT.Add(key_text, "Path");
            KOR_TEXT.Add(key_text, "경로");
            JAP_TEXT.Add(key_text, "パス");

            key_text = "PF_H1_Events";
            ENG_TEXT.Add(key_text, "Events");
            KOR_TEXT.Add(key_text, "이벤트");
            JAP_TEXT.Add(key_text, "イベント");

            key_text = "PF_Events_Label";
            ENG_TEXT.Add(key_text, "Execute a method when a route is completed");
            KOR_TEXT.Add(key_text, "경로를 완주 했을 때 메소드 실행");
            JAP_TEXT.Add(key_text, "パスを完走したときのメソッド実行");

            key_text = "PF_Events_endEventLabel";
            ENG_TEXT.Add(key_text, "Execute a methods");
            KOR_TEXT.Add(key_text, "메소드 실행");
            JAP_TEXT.Add(key_text, "メソッドを実行");
        }

        //=================================================================================================================================
        //  Get Local Text
        //---------------------------------------------------------------------------------------------------------------------------------
        //  Text output corresponding to the set language
        //  설정된 언어에 해당하는 텍스트 출력
        //=================================================================================================================================
        /// <summary>
        /// Text output corresponding to the set language
        /// </summary>
        /// <param name="key_text">text key</param>
        /// <returns>translated text</returns>
        public static string GetLocalText(string key_text)
        {
            try
            {
                if ( CurrentLanguage == LANGUAGE.KOR )
                    return KOR_TEXT[key_text];
                else if ( CurrentLanguage == LANGUAGE.JAP )
                    return JAP_TEXT[key_text];
                else
                    return ENG_TEXT[key_text];
            }
            catch ( System.Exception e )
            {
                e.ToString();
                return "";
            }
        }
    }
}