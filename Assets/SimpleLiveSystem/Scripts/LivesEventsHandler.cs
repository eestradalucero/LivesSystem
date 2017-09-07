using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivesEventsHandler : MonoBehaviour {
    private static LivesEventsHandler instance = null;
    private static readonly object padlock = new object ();
    public static LivesEventsHandler Instance {
        get {
            lock (padlock) {
                if (instance == null) {
                    var go = new GameObject ("EventHandler");
                    var livesEventHandler = go.AddComponent<LivesEventsHandler> ();
                    instance = livesEventHandler;
                }
            }
            return instance;
        }
    }

    void Awake(){
//        Debug.Log ("Awoke Lives event handler");
        DontDestroyOnLoad (this.gameObject);
    }

	// Use this for initialization
	void Start () {
//        Debug.Log ("Started Lives event handler");
	}


    void OnEnable(){
//        Debug.Log ("Enabled Lives event handler");
    }

    void OnApplicationFocus(bool hasFocus) {
//        Debug.Log ("OnApplicationFocus " + hasFocus);
        if (hasFocus) {
//            Debug.Log ("Earned Focus, deserialize");
        } else {
//            Debug.Log ("Lost focus, serialize");
//            Debug.Log(JsonUtility.ToJson (LivesSystem.Instance));
        }
    }

    void OnApplicationPause(bool pauseStatus) {
//        Debug.Log ("OnApplicationFocus: " + pauseStatus);
    }

    void OnApplicationQuit(){
        
    }

    void OnDisable(){
//        Debug.Log ("Disabled Lives event handler");
    }

    void OnDestroy(){
//        Debug.Log ("Destroyed Lives event handler");
        instance = null;
    }
}
