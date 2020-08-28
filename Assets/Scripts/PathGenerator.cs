using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PathGenerator : MonoBehaviour {
    public GameObject Flag;
    public GameObject StartFlag;
    public GameObject Angle;
    public GameObject Guide;
    public bool isClosed = true;
    public bool isDebugObject = false;
    public bool isDebugLine = false;
    public int PathDensity = 30;
    
    public List<GameObject> FlagList = new List<GameObject>();
    public List<GameObject> AngleList = new List<GameObject>();
    internal List<GameObject> PathList = new List<GameObject>();

    private Transform Roads;

    public void Awake() {
        if(PathDensity < 2) {
#if UNITY_EDITOR
            Debug.LogError("Path Density is too small. (must >= 2)");
            UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER
            Application.OpenURL("https://www.google.com");
#else
            Application.Quit();
#endif
        }

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