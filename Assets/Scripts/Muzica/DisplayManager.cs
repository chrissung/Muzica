using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class DisplayManager : MonoBehaviour {

   #region Static Properties
   public static DisplayManager Instance { get; set; }
   #endregion

   public enum PanelType {
      Sample,
      Mix,             
      Share,
      Settings
   }
   
   private PanelType _panelType = PanelType.Sample;
   private GameObject _samplePanel = null;
   private GameObject _mixPanel = null;
   private GameObject _samplePlayPanel = null;
   private GameObject _sampleRecordPanel = null;
   private GameObject _mixTempoField = null;
   private InputField _mixTempoInput = null;
   private GameObject _imageMixPlay = null;
   private GameObject _imageMixPause = null;
   private GameObject _ftueSampleField = null;
   private InputField _ftueSampleInput = null;
   private GameObject _ftueMixField = null;
   private InputField _ftueMixInput = null;

   private string _ftueSampleRecordText = "In Record Mode: tap a square to start and stop recording from mic";
   private string _ftueSamplePlayText = "In Play Mode: tap squares to play your sounds";
   private string _ftueSampleStartedRecordingText = "Recording: tap the square to stop recording this sound";
   private string _ftueSampleStoppedRecordingText = "Done: switch to Play Mode to hear your new sound";

   private string _ftueMixText = "Tap squares where you want sounds to play";

   private string _ftueResetText = "All sounds have been reset";
   
   void Awake() {
      if (DisplayManager.Instance == null)
         DisplayManager.Instance = this;
   }

   // Use this for initialization
   void Start() {
      // main panels
      _samplePanel = GameObject.Find("SamplesPanel");
      _mixPanel = GameObject.Find("MixPanel");

      // bg tabs in sample panel
      _samplePlayPanel = GameObject.Find("SamplePlayBg");
      _sampleRecordPanel = GameObject.Find("SampleRecordBg");

      // text fields to manipulate
      _mixTempoField = GameObject.Find("MixTempoField");
      _mixTempoInput = _mixTempoField.GetComponent<InputField>();

      _ftueSampleField = GameObject.Find("FtueSampleField");
      _ftueSampleInput = _ftueSampleField.GetComponent<InputField>();

      _ftueMixField = GameObject.Find("FtueMixField");
      _ftueMixInput = _ftueMixField.GetComponent<InputField>();

      _imageMixPlay = GameObject.Find("ImageMixPlay");
      _imageMixPause = GameObject.Find("ImageMixPause");

      // now set the current
      SetPanels(_panelType);

      // TODO: bind any event listeners here
   }
   
   // Update is called once per frame
   void Update() {
   
   }

   public void SetTempo(int tempo) {
      string tempoString = tempo.ToString();
      _mixTempoInput.text = tempoString;
   }

   public void TogglePanel(GameObject panel, bool state) {
      if (state == true) {
         panel.GetComponent<CanvasGroup>().alpha = 1f;
         panel.GetComponent<CanvasGroup>().interactable = true;
         panel.GetComponent<CanvasGroup>().blocksRaycasts = true;
      }
      else {
         panel.GetComponent<CanvasGroup>().alpha = 0f;
         panel.GetComponent<CanvasGroup>().interactable = false;
         panel.GetComponent<CanvasGroup>().blocksRaycasts = false;
      }
   }

   public void ToggleRecordingState(string stateOn) {
      if (stateOn == "started") {
         _ftueSampleInput.text = _ftueSampleStartedRecordingText;
      }
      else {
         _ftueSampleInput.text = _ftueSampleStoppedRecordingText;
      }
   }

   public void ToggleRecordMode(string stateOn) {
      if (stateOn == "record") {
         _ftueSampleInput.text = _ftueSampleRecordText;
      }
      else {
         _ftueSampleInput.text = _ftueSamplePlayText;
      }
   }

   public void ToggleMixMode(string stateOn) {
      if (stateOn == "mix") {
         _ftueMixInput.text = _ftueMixText;
      }
   }

   public void ConveyReset() {
      _ftueSampleInput.text = _ftueResetText;
      _ftueMixInput.text = _ftueResetText;
   }
   
   public void TogglePlayPause(string stateOn) {
      if (stateOn == "play") {
         _imageMixPlay.GetComponent<CanvasGroup>().alpha = 1f;
         _imageMixPlay.GetComponent<CanvasGroup>().interactable = true;
         _imageMixPlay.GetComponent<CanvasGroup>().blocksRaycasts = true;
         _imageMixPause.GetComponent<CanvasGroup>().alpha = 0f;
         _imageMixPause.GetComponent<CanvasGroup>().interactable = false;
         _imageMixPause.GetComponent<CanvasGroup>().blocksRaycasts = false;
      }
      else {
         _imageMixPause.GetComponent<CanvasGroup>().alpha = 1f;
         _imageMixPause.GetComponent<CanvasGroup>().interactable = true;
         _imageMixPause.GetComponent<CanvasGroup>().blocksRaycasts = true;
         _imageMixPlay.GetComponent<CanvasGroup>().alpha = 0f;
         _imageMixPlay.GetComponent<CanvasGroup>().interactable = false;
         _imageMixPlay.GetComponent<CanvasGroup>().blocksRaycasts = false;
      }
   }
   
   public void SetPanels(PanelType panelType) {
      
      // blank all panels
      TogglePanel(_mixPanel, false);
      TogglePanel(_samplePanel, false);
      
      // turn on the current one
      switch (panelType) {
      case PanelType.Mix:
         TogglePanel(_mixPanel, true);
         ToggleMixMode("mix");
         // HACK: flaky if done from TileManager Start()
         TileManager.Instance.RestoreState();
         break;
      case PanelType.Sample:
         TogglePanel(_samplePanel, true);

         // set sub panels
         if (SampleManager.Instance.editMode) {
            TogglePanel(_samplePlayPanel, false);
            TogglePanel(_sampleRecordPanel, true);
            ToggleRecordMode("record");
         }
         else {
            TogglePanel(_samplePlayPanel, true);
            TogglePanel(_sampleRecordPanel, false);
            ToggleRecordMode("play");
         }
         break;
      default:
         Debug.Log("Found unknown panel type");
         break;
      }
      
      // record current
      _panelType = panelType; 
   }
}
