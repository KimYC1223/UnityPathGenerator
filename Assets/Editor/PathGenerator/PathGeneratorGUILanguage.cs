using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//=========================================================================================================================
//
//  PATH GENERATOR GUI LANGUAGE CLASS
//
//  GUI language management class
//  GUI 언어 관리 클래스
//
//-------------------------------------------------------------------------------------------------------------------------
//  2022.11.17 _ KimYC1223
//=========================================================================================================================
namespace CurvedPathGenertator {
    public enum LANGUAGE { ENG, KOR, JAP };
    // Chinese support coming soon
    // 중국어 지원 추가 예정

    public class PathGeneratorGUILanguage {
        static Dictionary<string, string> ENG_TEXT;
        static Dictionary<string, string> KOR_TEXT;
        static Dictionary<string, string> JAP_TEXT;

        static public void InitLocalization() {
            string key_text;
            //if (ENG_TEXT != null && KOR_TEXT != null && JAP_TEXT != null)
            //    return;

            ENG_TEXT = new Dictionary<string, string>();
            KOR_TEXT = new Dictionary<string, string>();
            JAP_TEXT = new Dictionary<string, string>();

            //=============================================================================================================
            //  [PG] Path Generator GUI 텍스트 설정
            //=============================================================================================================
            key_text = "PG_Title";
            ENG_TEXT.Add(key_text, "Curved Path Generator");
            KOR_TEXT.Add(key_text, "곡선 경로 생성기");
            JAP_TEXT.Add(key_text, "曲線パスジェネレータ");

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
            JAP_TEXT.Add(key_text, "角度");

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
            JAP_TEXT.Add(key_text, "ベイクドパス");

            key_text = "PG_H1_EditorSetting";
            ENG_TEXT.Add(key_text, "Editor setting");
            KOR_TEXT.Add(key_text, "에디터 관련");
            JAP_TEXT.Add(key_text, "エディタ関連");

            key_text = "PG_PathTypeChangeButton_isLivePathWarning";
            ENG_TEXT.Add(key_text, "Updates the path every frame. Therefore, even if the node or angle changes position at runtime, it is applied to the path immediately. However, the amount of calculation may increase.");
            KOR_TEXT.Add(key_text, "매 프레임마다 경로를 업데이트 합니다. 따라서, 런타임에서 Node나 Angle의 위치가 바뀌어도 즉시 경로에 적용됩니다. 다만 계산량이 많아 질 수 있습니다.");
            JAP_TEXT.Add(key_text, "フレームごとにパスを更新します。 したがって、ランタイムでNodeやAngleの位置が変わっても直ちに経路に適用されます。 ただし、計算量が多くなる可能性があります。");

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
            JAP_TEXT.Add(key_text, "シーン内の ...");

            key_text = "PG_ShowLabelToggle";
            ENG_TEXT.Add(key_text, "Show labels");
            KOR_TEXT.Add(key_text, "레이블 그리기");
            JAP_TEXT.Add(key_text, "ラベルの表示");

            key_text = "PG_ShowIconsToggle";
            ENG_TEXT.Add(key_text, "Show icons");
            KOR_TEXT.Add(key_text, "아이콘 그리기");
            JAP_TEXT.Add(key_text, "アイコンの表示");

            key_text = "PG_TopViewModeButton_toTop";
            ENG_TEXT.Add(key_text, "Change to top view mode");
            KOR_TEXT.Add(key_text, "탑 뷰 모드로 변경");
            JAP_TEXT.Add(key_text, "トップビューモードに変更する");

            key_text = "PG_TopViewModeButton_Reset";
            ENG_TEXT.Add(key_text, "Reset view mode");
            KOR_TEXT.Add(key_text, "뷰 모드 리셋");
            JAP_TEXT.Add(key_text, "ビューモードをリセットする");

            key_text = "PG_Colors_Label";
            ENG_TEXT.Add(key_text, "Guideline Colors");
            KOR_TEXT.Add(key_text, "가이드라인 색상");
            JAP_TEXT.Add(key_text, "ガイドライン 色");

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
            JAP_TEXT.Add(key_text, "空いています。");

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
            JAP_TEXT.Add(key_text, "すべてのノードと角度に対して...");

            key_text = "PG_TotalControl_SetZeroToX";
            ENG_TEXT.Add(key_text, "X to 0");
            KOR_TEXT.Add(key_text, "X값을\n0으로");
            JAP_TEXT.Add(key_text, "X値を0に");

            key_text = "PG_TotalControl_SetZeroToY";
            ENG_TEXT.Add(key_text, "Y to 0");
            KOR_TEXT.Add(key_text, "Y값을\n0으로");
            JAP_TEXT.Add(key_text, "Y値を0に");

            key_text = "PG_TotalControl_SetZeroToZ";
            ENG_TEXT.Add(key_text, "Z to 0");
            KOR_TEXT.Add(key_text, "Z값을\n0으로");
            JAP_TEXT.Add(key_text, "Z値を0に");

            key_text = "PG_TotalControl_SetAvgToX";
            ENG_TEXT.Add(key_text, "X\nequalization");
            KOR_TEXT.Add(key_text, "X값\n평준화");
            JAP_TEXT.Add(key_text, "X値\n平準化");

            key_text = "PG_TotalControl_SetAvgToY";
            ENG_TEXT.Add(key_text, "Y\nequalization");
            KOR_TEXT.Add(key_text, "Y값\n평준화");
            JAP_TEXT.Add(key_text, "Y値\n平準化");

            key_text = "PG_TotalControl_SetAvgToZ";
            ENG_TEXT.Add(key_text, "Z\nequalization");
            KOR_TEXT.Add(key_text, "Z값\n평준화");
            JAP_TEXT.Add(key_text, "Z値\n平準化");

            key_text = "PG_TotalControl_SpecificValue";
            ENG_TEXT.Add(key_text, "Specific value");
            KOR_TEXT.Add(key_text, "특정값");
            JAP_TEXT.Add(key_text, "特定の値");

            key_text = "PG_TotalControl_SetSpecificToX";
            ENG_TEXT.Add(key_text, "X to ...");
            KOR_TEXT.Add(key_text, "X값을 ...");
            JAP_TEXT.Add(key_text, "X値をこの ...");

            key_text = "PG_TotalControl_SetSpecificToY";
            ENG_TEXT.Add(key_text, "Y to ...");
            KOR_TEXT.Add(key_text, "Y값을 ...");
            JAP_TEXT.Add(key_text, "Y値をこの ...");

            key_text = "PG_TotalControl_SetSpecificToZ";
            ENG_TEXT.Add(key_text, "Z to ...");
            KOR_TEXT.Add(key_text, "Z값을 ...");
            JAP_TEXT.Add(key_text, "Z値をこの ...");

            key_text = "PG_H1_Rendering";
            ENG_TEXT.Add(key_text, "Rendering");
            KOR_TEXT.Add(key_text, "렌더링");
            JAP_TEXT.Add(key_text, "レンダリング");

            key_text = "PG_H1_Rendering_Label";
            ENG_TEXT.Add(key_text, "A visual representation of the path.");
            KOR_TEXT.Add(key_text, "경로를 가시적으로 표현 할 수 있습니다.");
            JAP_TEXT.Add(key_text, "パスの視覚的表現");

            key_text = "PG_Rendering_isGeneratePathMesh";
            ENG_TEXT.Add(key_text, "Generate path mesh in runtime");
            KOR_TEXT.Add(key_text, "런타임에 경로 메쉬를 생성합니다.");
            JAP_TEXT.Add(key_text, "実行時にパスメッシュを生成する");

            key_text = "PG_Rendering_MaterialInfo";
            ENG_TEXT.Add(key_text, "Material Infomation");
            KOR_TEXT.Add(key_text, "머터리얼 정보");
            JAP_TEXT.Add(key_text, "材料情報");

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
            JAP_TEXT.Add(key_text, "塗りつぶす");

            key_text = "PG_Rendering_RenderQueue";
            ENG_TEXT.Add(key_text, "Render queue");
            KOR_TEXT.Add(key_text, "렌더 큐");
            JAP_TEXT.Add(key_text, "レンダリングキュー");

            key_text = "PG_Rendering_RenderQueueHelp";
            ENG_TEXT.Add(key_text, "Render queue default value is 3000");
            KOR_TEXT.Add(key_text, "렌더 큐의 기본값은 3000입니다.");
            JAP_TEXT.Add(key_text, "レンダリング キューのデフォルト値は 3000 です");
        }

        static public string GetLocalText(string key_text) {
            try{
                if (PathGeneratorGUI.CurrentLanguage == LANGUAGE.KOR)
                    return KOR_TEXT[key_text];
                else if (PathGeneratorGUI.CurrentLanguage == LANGUAGE.JAP)
                    return JAP_TEXT[key_text];
                else
                    return ENG_TEXT[key_text];
            } catch (System.Exception e) {
                e.ToString();
                return "";
            }
        }
    }
}