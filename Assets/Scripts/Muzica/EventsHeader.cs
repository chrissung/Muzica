using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.IO;
using System.Collections;

public class EventsHeader : MonoBehaviour, IPointerDownHandler {

   private static Canvas _canvas = null; 

   void Start() {
      if ( !_canvas)
         _canvas = FindObjectOfType( typeof( Canvas ) ) as Canvas;
   }

   public void OnPointerDown(PointerEventData data)
   {
      // stop any current recording
      SampleManager.Instance.FinishRecording();

      // stop any playback
      PlaybackManager.Instance.Stop();
      PlaybackManager.Instance.Rewind();
      SampleManager.Instance.StopAll();
      LocationBar.Instance.Reset();

      DisplayManager.Instance.TogglePlayPause("play");

      // set the currently displayed panel
      switch (this.gameObject.name) {
      case "ButtonSample":
         DisplayManager.Instance.SetPanels(DisplayManager.PanelType.Sample);
         break;
      case "ButtonMix":
         DisplayManager.Instance.SetPanels(DisplayManager.PanelType.Mix);
         break;
      case "ButtonReset":
         SampleManager.Instance.ResetSamples();
         DisplayManager.Instance.ConveyReset();
         break;
      default:
         Debug.Log("Found unknown header button name");
         break;
      }
   }
}
