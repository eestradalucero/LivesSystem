using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Emilio.LivesSystem;

public class LivesViewExample : MonoBehaviour {
    [SerializeField] Text numberOfLivesLabel;
    [SerializeField] Text maxNumberOfLivesLabel;
    [SerializeField] Text timeTillNextLiveLabel;
    LivesSystem lifeSystem;


    void Awake(){
        lifeSystem = LivesSystem.Instance;
        LivesStandard info = JsonUtility.FromJson<LivesStandard>(PlayerPrefs.GetString (SAVESTRING));
        lifeSystem.FeedDeserializeInfo (info);
        UpdateNumberOfLives (); 
    }

	void Start(){
        lifeSystem.CheckRecovery ();
        timeTillNextLiveLabel.text = lifeSystem.Lives.timeTillNextRecovery.ToString ();
        maxNumberOfLivesLabel.text = lifeSystem.Lives.MaxCount.ToString ();
        UpdateNumberOfLives ();
	}

    void UpdateNumberOfLives(){
        numberOfLivesLabel.text = lifeSystem.Lives.GetCount().ToString ();
    }


    void Update(){
        if (Input.GetKeyDown (KeyCode.D)) {
            PlayerPrefs.DeleteAll ();
            PlayerPrefs.Save ();
        }
        if (Input.GetKeyDown (KeyCode.A)) {
            lifeSystem.ConsumeLife ();
        }
        if (!lifeSystem.Lives.IsFullOfLives ()) {
            lifeSystem.CheckRecovery ();
        }
        timeTillNextLiveLabel.text = lifeSystem.Lives.timeTillNextRecovery.ToString ();
    }


	void OnEnable(){
		lifeSystem.OnLifeConsumed += OnLifeConsumedHandler;
		lifeSystem.OnLifeRecovered += OnLifeRecoveredHandler;
	}

	void OnDisable(){
		lifeSystem.OnLifeConsumed -= OnLifeConsumedHandler;
		lifeSystem.OnLifeRecovered -= OnLifeRecoveredHandler;
	}

    const string SAVESTRING = "LiveInfo";
	void OnLifeConsumedHandler(int newLivesCount){
        PlayerPrefs.SetString (SAVESTRING, JsonUtility.ToJson (lifeSystem.GetInfoToSerialize ()));
        PlayerPrefs.Save ();
        UpdateNumberOfLives ();


	}

	void OnLifeRecoveredHandler(int newLivesCount){
        Debug.Log ("Saved lives");
        PlayerPrefs.SetString (SAVESTRING, JsonUtility.ToJson (lifeSystem.GetInfoToSerialize ()));
        PlayerPrefs.Save ();
        UpdateNumberOfLives (); 

	}

}
