using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class EventsHeaderSample : MonoBehaviour, IPointerDownHandler {

   private GameObject _samplePlayPanel = null;
   private GameObject _sampleRecordPanel = null;

	// Use this for initialization
	void Start () {
      _samplePlayPanel = GameObject.Find("SamplePlayBg");
      _sampleRecordPanel = GameObject.Find("SampleRecordBg");
	}
	
   // TODO: abstract some of the manager logic
   public void OnPointerDown(PointerEventData data)
   {
      // handle any rogue recording taps
      SampleManager.Instance.FinishRecording();

      // handle tap events for this segment of ux
      switch (this.gameObject.name) {
      case "ButtonSamplePlay":
         Debug.Log("ButtonSamplePlay");
         SampleManager.Instance.editMode = false;
         DisplayManager.Instance.ToggleRecordMode("play");
         if (SampleManager.Instance.editMode) {
            DisplayManager.Instance.TogglePanel(_samplePlayPanel, false);
            DisplayManager.Instance.TogglePanel(_sampleRecordPanel, true);
         }
         else {
            DisplayManager.Instance.TogglePanel(_samplePlayPanel, true);
            DisplayManager.Instance.TogglePanel(_sampleRecordPanel, false);
         }
         break;
      case "ButtonSampleRecord":
         Debug.Log("ButtonSampleRecord");
         SampleManager.Instance.editMode = true;
         DisplayManager.Instance.ToggleRecordMode("record");
         if (SampleManager.Instance.editMode) {
            DisplayManager.Instance.TogglePanel(_samplePlayPanel, false);
            DisplayManager.Instance.TogglePanel(_sampleRecordPanel, true);
         }
         else {
            DisplayManager.Instance.TogglePanel(_samplePlayPanel, true);
            DisplayManager.Instance.TogglePanel(_sampleRecordPanel, false);
         }
         break;
      default:
         Debug.Log("Found unknown sample header button name");
         break;
      }
   }
}
