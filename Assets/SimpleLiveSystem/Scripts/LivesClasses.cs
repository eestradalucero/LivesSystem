using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Emilio.LivesSystem {
    [System.Serializable]
    public abstract class LivesAbstract{
        int m_maxCount = 7;
        public int MaxCount{ get { return m_maxCount; } }
        public abstract int GetCount ();
    }

    //Should I move to a scriptable object?
    [System.Serializable]
    public class LivesStandard : LivesAbstract{

        DateTime futuremostDate;
        [SerializeField] string futureMostDateString = "";
        Queue<DateTime> lifeRecoveryQueue = new Queue<DateTime>();
        [SerializeField] string[] lifeRecoveryList;
        const int LIVE_RECOVERY_TIME = 5; //In seconds
        public TimeSpan timeTillNextRecovery { get { return lifeRecoveryQueue.Count > 0 ? DateTime.Now - lifeRecoveryQueue.Peek() : TimeSpan.Zero; } }

        public LivesStandard(){
            lifeRecoveryQueue = new Queue<DateTime> ();
            futuremostDate = DateTime.Now;
        }

        public void IncreaseLives(){
            if (lifeRecoveryQueue != null && lifeRecoveryQueue.Count > 0) {
                lifeRecoveryQueue.Dequeue ();
                DatesToStrings ();
            }
        }

        const string FTM = "O";
        public void DatesToStrings(){
            futureMostDateString = futuremostDate.ToString (FTM);
            var deepCopyQueue = new Queue<DateTime> (lifeRecoveryQueue);
            lifeRecoveryList = new string[deepCopyQueue.Count];
            if (deepCopyQueue.Count == 0) {
                return;
            }
            var count = deepCopyQueue.Count;
            for (int i = 0; i < count; i++) {
                lifeRecoveryList[i] = deepCopyQueue.Dequeue ().ToString(FTM);
            }
        }

        public void DecreaseLives(DateTime time){
            if (IsFullOfLives ()) {
                var furtherMostDate = ReturnFuturemostDate (time, futuremostDate);
                lifeRecoveryQueue.Enqueue (furtherMostDate.AddSeconds (LIVE_RECOVERY_TIME));
            } else {
                var firstRecoveryDate = lifeRecoveryQueue.Peek ();
                lifeRecoveryQueue.Enqueue(firstRecoveryDate.AddSeconds(LIVE_RECOVERY_TIME * lifeRecoveryQueue.Count));
                DatesToStrings ();
            }
        }

        public bool IsFullOfLives(){
            return GetCount() >= MaxCount;
        }

        DateTime ReturnFuturemostDate(DateTime date1, DateTime date2){
            return date1 > date2 ? date1 : date2;
        }

        public bool IsOutOfLives(){
            return GetCount() <= 0;
        }

        public override int GetCount () {
            return MaxCount - lifeRecoveryQueue.Count;
        }
            
        public bool ShouldRecoverLife(DateTime now){ 
            if(now > lifeRecoveryQueue.Peek()){
                return true;
            }
            return false;
        }

        public void StringsToDates(){
            lifeRecoveryQueue = new Queue<DateTime> ();
            if (lifeRecoveryList == null || lifeRecoveryList.Length == 0) {
                return;
            }

            for (int i = 0; i < lifeRecoveryList.Length; ++i) {
                if (!string.IsNullOrEmpty (lifeRecoveryList [i])) {
                    lifeRecoveryQueue.Enqueue (DateTime.ParseExact(lifeRecoveryList[i], FTM, 
                        System.Globalization.CultureInfo.InvariantCulture));
                    Debug.Log(lifeRecoveryQueue.Peek().ToString(FTM));
                }
            }
            if (string.IsNullOrEmpty (futureMostDateString)) {
                futuremostDate = DateTime.Now;
            } else {
                futuremostDate = DateTime.ParseExact (futureMostDateString, FTM, 
                    System.Globalization.CultureInfo.InvariantCulture);
            }
        }
    }
}

