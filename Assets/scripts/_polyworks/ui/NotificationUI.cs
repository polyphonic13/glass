namespace Polyworks 
{
	using UnityEngine;
	using UnityEngine.UI;
	using System.Collections;

	public class NotificationUI : MonoBehaviour 
	{
		private float _fadeTime;
		private float _fadeSpeed;

		private Text _textField;
		private CanvasGroup _canvasGroup;
		private float _startAlpha; 

		private bool _isDisplayingNote = false; 

		public bool isDisplayingNote {
			get {
				return _isDisplayingNote;
			}
		}

		public void RemoveNote() 
		{
			Hide ();
		}

		public void AddNote(string message) 
		{
			_isDisplayingNote = true;
			_canvasGroup.alpha = _startAlpha;
			_textField.text = message;
			StartCoroutine ("_fade");
		}

		public void Init(float fadeTime, float fadeSpeed) 
		{
			Debug.Log("NotificationUI/Init");
			_fadeTime = fadeTime;
			_fadeSpeed = fadeSpeed; 
			
			_textField = GetComponentInChildren<Text>();
			_canvasGroup = gameObject.GetComponent<CanvasGroup>();
			
			_startAlpha = _canvasGroup.alpha;
			_canvasGroup.alpha = 0;
		}

		public void Hide() 
		{
			_isDisplayingNote = false;
			_textField.text = "";
			_canvasGroup.alpha = 0;
		}

		private IEnumerator _fade() 
		{
			for (float f = _fadeTime; f >= 0; f -= _fadeSpeed) 
			{
				if(f <= 1f) 
				{
					if(f < 0.1f) 
					{
						f = 0;
					}
					_canvasGroup.alpha = f;
					if(f == 0) 
					{
						RemoveNote();
					}
				}
				yield return new WaitForSeconds(0.2f);
			}
		}
	}
}

