using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//===================================================================================================
//
//  PATH PATH GENERATOR CLASS
//
//  Script to make followable path the based on Bézier Curve
//  Path Generator가 만든 Path를 따라가는 기능
//
//---------------------------------------------------------------------------------------------------
//  2020.08.30 _ KimYC1223
//===================================================================================================

public class PathGenerator : MonoBehaviour {
    public GameObject Flag;                 // Visual maker of stopover
    public GameObject StartFlag;            // Visual maker of start point
    public GameObject Angle;                // Visual maker of angle setter
    public GameObject Guide;                // Visual maker of path guide
    public bool isClosed = true;            // is this path closed?
    public bool isDebugObject = false;      // show Flag and Angle objects in play mode?
    public bool isDebugLine = false;        // show guide objects in play mode?
    public int PathDensity = 30;            // Density of guide objects between Flags
    
    public List<GameObject> FlagList = new List<GameObject>();    // List of Flag objects
    public List<GameObject> AngleList = new List<GameObject>();   // List of Angle objects
    internal List<GameObject> PathList = new List<GameObject>();  // List of Path objects
    private Transform Roads;                                      // Root object of guide objects

    //===============================================================================================
    // Awake method
    //-----------------------------------------------------------------------------------------------
    // init variable & position
    // 각종 변수와 position 초기화
    //===============================================================================================
    public void Awake() {
        //===========================================================================================
        //  check path density is bigger than 1
        //  path density가 1보다 큰 지 확인
        //===========================================================================================
        if (PathDensity < 2) {
#if UNITY_EDITOR
            Debug.LogError("Path Density is too small. (must >= 2)");
            UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER
            Application.OpenURL("https://www.google.com");
#else
            Application.Quit();
#endif
        }

        //===========================================================================================
        //  Get root object of guides for Instantiate guide object
        //  guide 오브젝트를 Instantiate 하기 위한 부모 객체 가져오기
        //===========================================================================================
        Transform[] childs = this.transform.GetComponentsInChildren<Transform>();
        foreach(Transform t in childs) {
            if (t.gameObject.name == "Roads")
                Roads = t;
        }

        for(int i = 0; i < FlagList.Count; i++) {
            Vector3 startPoint = FlagList[i].transform.position;
            Vector3 middlePoint = new Vector3();
            Vector3 endPoint = new Vector3();
            if (i == FlagList.Count - 1) {
                if (isClosed) {
                    middlePoint = AngleList[i].transform.position;
                    endPoint = FlagList[0].transform.position;
                } else {
                    break;
                }
            } else {
                middlePoint = AngleList[i].transform.position;
                endPoint = FlagList[i + 1].transform.position;
            }
            StartFlag.transform.position = startPoint;

            for (int j = 0; j < PathDensity; j++) {
                float t = (float)j / PathDensity;

                Vector3 curve = (1f - t) * (1f - t) * startPoint +
                               2 * (1f - t) * t * middlePoint +
                               t * t * endPoint;

                PathList.Add(Instantiate(Guide, curve, Quaternion.identity, Roads));
            }

        }
        if(!isClosed)
            PathList.Add(Instantiate(Guide, FlagList[FlagList.Count-1].transform.position, Quaternion.identity, Roads));
        

        if (!isDebugObject) {
            foreach (GameObject element in FlagList)
                element.GetComponent<Renderer>().enabled = false;
            foreach (GameObject element in AngleList)
                if(element != null)
                    element.GetComponent<Renderer>().enabled = false;
        }

        if (!isDebugLine) {
            foreach (GameObject element in PathList)
                element.GetComponent<Renderer>().enabled = false;
        }
    }
    
}