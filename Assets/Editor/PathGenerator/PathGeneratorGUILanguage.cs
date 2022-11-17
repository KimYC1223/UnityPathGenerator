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
            if (ENG_TEXT != null && KOR_TEXT != null && JAP_TEXT != null)
                return;

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


            key_text = "PG_PathTypeChangeButton_ToOpen";
            ENG_TEXT.Add(key_text, "Change to opened path");
            KOR_TEXT.Add(key_text, "열린 패스로 변경");
            JAP_TEXT.Add(key_text, "オープンパスに変更");

            key_text = "PG_PathTypeChangeButton_ToClose";
            ENG_TEXT.Add(key_text, "Change to closed path");
            KOR_TEXT.Add(key_text, "닫힌 패스로 변경");
            JAP_TEXT.Add(key_text, "クローズドパスに変更");

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

            key_text = "PG_ShowLableToggle";
            ENG_TEXT.Add(key_text, "Show Lable in Scene");
            KOR_TEXT.Add(key_text, "Scene에서 번호 보이기");
            JAP_TEXT.Add(key_text, "Sceneで番号を表示");

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
            ENG_TEXT.Add(key_text, "X");
            KOR_TEXT.Add(key_text, "X");
            JAP_TEXT.Add(key_text, "X");

            key_text = "PG_NodeListTable_Empty";
            ENG_TEXT.Add(key_text, "Empty");
            KOR_TEXT.Add(key_text, "비어있음");
            JAP_TEXT.Add(key_text, "空いています。");

            key_text = "PG_NodeList_CreateNodeButton";
            ENG_TEXT.Add(key_text, "Create node");
            KOR_TEXT.Add(key_text, "노드 생성");
            JAP_TEXT.Add(key_text, "ノード生成");
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