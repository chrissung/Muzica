using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PlaybackManager : MonoBehaviour {

   #region Static Properties
   public static PlaybackManager Instance { get; set; }
   #endregion

   private int numSlots;
   private int numChannels;
   public bool playing;

   // sequencer related
   private int currentBeat = 3;
   private int totalBeats = -1;

   private int bpmCurr = 120;
   private int bpmMax = 240;
   private int bpmMin = 60;

   private double nextBeatTime = -1.0;
   private double beatDur = 0.0;

   private string stateKey = "playState";

   public int Tempo {
      get { return bpmCurr; }
   }

   public int Channels {
      get { return numChannels; }
   }

   public int Slots {
      get { return numSlots; }
   }

   void Awake() {
      if (PlaybackManager.Instance == null)
         PlaybackManager.Instance = this;
   }

   // Use this for initialization
   void Start() {
      playing = false;
      numSlots = TileManager.Instance.nSlots;
      numChannels = TileManager.Instance.nChannels;

      // restore prev state of samples
      RestoreState();

      TempoSet(bpmCurr);
   }

   // save current state
   private void SaveState() {
      Dictionary<string, object> localState = new Dictionary<string, object>();
      localState.Add("tempo", bpmCurr);
      PlayerPrefs.SetString(stateKey, MiniJSON.Json.Serialize(localState));
   }

   // restore prev state of tiles
   public void RestoreState() {
      if (String.IsNullOrEmpty(PlayerPrefs.GetString(stateKey)))
         return;

      var stateInfo = MiniJSON.Json.Deserialize(PlayerPrefs.GetString(stateKey)) as Dictionary<string, object>;

      object tempo_o = null;
      int tempo_i = 120;
      if (stateInfo.TryGetValue("tempo", out tempo_o)) {
         tempo_i = Convert.ToInt32(tempo_o);
         TempoSet(tempo_i);
      }
   }

   public void Play() {
      playing = true;
      nextBeatTime = AudioSettings.dspTime;
   }
   
   public void Stop() {
      playing = false;
   }
   
   public void Rewind() {
      playing = false;

      currentBeat = 3;
      totalBeats = -1;
   }

   public void TempoSet(int bpmNew) {
      bpmCurr = Math.Max(Math.Min(bpmNew, bpmMax), bpmMin);

      // a beat is really, er, an 8th note in this case, but much better timing
      beatDur = 30.0 / (double)bpmCurr;

      DisplayManager.Instance.SetTempo(bpmCurr);
      SaveState();
   }

   public void TempoUp() {
      TempoSet(bpmCurr + 5);
   }

   public void TempoDown() {
      TempoSet(bpmCurr - 5);
   }

   // Update is called once per frame
   public void Update() {
      if (!playing)
         return;

      // check if it's time to handle the next beat
      double currentTime = AudioSettings.dspTime;
      if (currentTime + 0.05 < nextBeatTime)
         return;
      
      // advance to next beat
      currentBeat = (currentBeat + 1) % 4;
      totalBeats++;

      // compute the upcoming slot
      int computedSlot = totalBeats % numSlots;
      
      // position the location bar based on the beat
      LocationBar.Instance.UpdatePosition(computedSlot);
      
      // now figure out which notes to play
      for (int i = 0; i < numChannels; i++) {
         if (!TileManager.Instance.Tiles[i, computedSlot].PlayState)
            continue;

         // play the note and show the visual
         SampleManager.Instance.StopSample(i);
         SampleManager.Instance.PlaySample(i, nextBeatTime);
         TileManager.Instance.Tiles[i, computedSlot].OnPlayHit();
      }

      // increment the next time to play
      nextBeatTime += beatDur;
   }
}
