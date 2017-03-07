using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class EventsHeaderMix : MonoBehaviour, IPointerDownHandler {
   
   private static Canvas _canvas = null;
   
   void Start() {
      if (!_canvas)
         _canvas = FindObjectOfType(typeof(Canvas)) as Canvas;
   }
   
   public void OnPointerDown(PointerEventData data) {
      // handle tap events for this segment of ux
      switch (this.gameObject.name) {
      case "ButtonPlay":
         LocationBar.Instance.ToggleState(true);
         if (PlaybackManager.Instance.playing) {
            SampleManager.Instance.StopAll();
            PlaybackManager.Instance.Stop();
            DisplayManager.Instance.TogglePlayPause("play");
         }
         else {
            PlaybackManager.Instance.Play();
            DisplayManager.Instance.TogglePlayPause("pause");
         }
         break;
      case "ButtonStop":
      case "ButtonRewind":
         SampleManager.Instance.StopAll();
         PlaybackManager.Instance.Rewind();
         DisplayManager.Instance.TogglePlayPause("play");
         LocationBar.Instance.Reset();
         break;
      case "ButtonTempoUp":
         PlaybackManager.Instance.TempoUp();
         break;
      case "ButtonTempoDown":
         PlaybackManager.Instance.TempoDown();
         break;
      default:
         Debug.Log("Found unknown mix header button name");
         break;
      }
   }
}
