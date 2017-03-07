using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;

public class Tile : MonoBehaviour, IPointerDownHandler {

   #region Events
   public event EventHandler OnTileChanged;
   #endregion

   private float visualDelay = 0.1f;

   private bool _playState;
   private bool _visualState;
   public bool PlayState {
      get { return _playState; }
   }
   public bool VisualState {
      get { return _visualState; }
   }
   
   public int channel;
   public int slot;

   // Use this for initialization
   void Start() {
      SetVisualState(false);
      SetPlayState(false);

      // TODO: add OnSequenceHit to sequence hit callback
   }
   
   public void SetPlayState(bool state) {
      _playState = state;
      if (_playState)
         gameObject.GetComponentInChildren<CanvasGroup>().alpha = 0.7f;
      else
         gameObject.GetComponentInChildren<CanvasGroup>().alpha = 0f;

      // Debug.Log("playState: " + channel + ", " + slot + ": " + _playState);
   }
   public void SetVisualState(bool state) {
      _visualState = state;
      // Debug.Log("visualState: " + channel + ", " + slot + ": " + _visualState);
   }

   public void OnPlayHit() {
      StartCoroutine(PlayHit());
   }
   
   private IEnumerator PlayHit() {
      // TODO: change visual state here
      _visualState = true;
      gameObject.GetComponentInChildren<CanvasGroup>().alpha = 1.0f;
      yield return new WaitForSeconds(visualDelay);
      _visualState = false;

      // handle case where tapped during hilite
      if (_playState)
         gameObject.GetComponentInChildren<CanvasGroup>().alpha = 0.7f;
      else
         gameObject.GetComponentInChildren<CanvasGroup>().alpha = 0f;
   }

   public void OnPointerDown(PointerEventData data)
   {
      SetPlayState(!_playState);
      if (OnTileChanged != null) {
         TileChangedEventArgs args = new TileChangedEventArgs();
         args.channel = channel;
         args.slot = slot;
         args.state = _playState;
         OnTileChanged(this, args);
      }
   }

   #region Public Support Classes
   public class TileChangedEventArgs : EventArgs {
      public int channel { get; set; }
      
      public int slot { get; set; }
      
      public bool state { get; set; }
   }
   #endregion
}
