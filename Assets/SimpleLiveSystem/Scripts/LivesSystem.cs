using UnityEngine;
using System;
using System.Collections.Generic;
using Emilio.LivesSystem;

namespace Emilio.LivesSystem{
    public class LivesSystem {
        private static LivesSystem instance = null;
        private static readonly object padlock = new object();
        public static LivesSystem Instance { get { 
                lock (padlock) {
                    if (instance == null) {
                        instance = new LivesSystem ();
                    }
                }
                return instance;
            } 
        }
        [SerializeField] LivesStandard m_lives;
        public LivesStandard Lives { get { return m_lives; } }

        LivesSystem(){
            m_lives = new LivesStandard ();
        }

        LivesSystem(LivesStandard livesStandard){
            FeedDeserializeInfo (livesStandard);
        }

        public delegate void LifeSystemEvent(int newLivesCount);
        public LifeSystemEvent OnLifeRecovered;
        public LifeSystemEvent OnLifeConsumed;

        public void ConsumeLife(){
            if(!m_lives.IsOutOfLives()){
                m_lives.DecreaseLives(DateTime.Now);
                if(OnLifeConsumed != null){
                    OnLifeConsumed(m_lives.GetCount());
                }
            }
        }

        public void CheckRecovery(){
            if (Lives.IsFullOfLives ())
                return;
           
            DateTime now = DateTime.Now;
            if (m_lives.ShouldRecoverLife (now)) {
                m_lives.IncreaseLives ();
                if (!m_lives.IsFullOfLives ()) {
                    CheckRecovery ();
                    return;
                }
                CallRecoveryDelegate ();
            }
        }

        void CallRecoveryDelegate(){
            if(OnLifeRecovered != null){
                OnLifeRecovered(m_lives.GetCount());
            }
        }

        public LivesStandard GetInfoToSerialize () {
            m_lives.DatesToStrings ();
            return m_lives;
        }

        public void FeedDeserializeInfo (LivesStandard lives) {
            if (lives != null) {
                m_lives = lives;
                m_lives.StringsToDates ();
            } else {
                m_lives = new LivesStandard ();
            }
        }
    }
}





