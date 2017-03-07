using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;

public class EventsRecordButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
   
   void Start() {
   }
   
   public void OnPointerDown(PointerEventData data)
   {
      string last = this.gameObject.name.Substring(this.gameObject.name.Length - 1, 1);
      int lastIx = Convert.ToInt32(last);
      SampleManager.Instance.PointerDown(lastIx);
   }
   public void OnPointerUp(PointerEventData data)
   {
      string last = this.gameObject.name.Substring(this.gameObject.name.Length - 1, 1);
      int lastIx = Convert.ToInt32(last);
      SampleManager.Instance.PointerUp(lastIx);
   }
}
