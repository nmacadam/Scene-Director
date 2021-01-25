// ONI, Copyright (c) Nathan MacAdam, All rights reserved. 
// MIT License (See LICENSE file)

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Oni.SceneManagement.Transitions
{
	public class MaterialTransition : CanvasBasedTransition
	{
		[SerializeField] private Image _image = default;

		private int _propertyID = 0;

		private void Awake() 
		{
			_propertyID = Shader.PropertyToID("_Interpolator");
		}

        public override void TransitionIn(Action onVisible)
        {
            StartCoroutine(TransitionRoutine(1, 0, Duration, onVisible));
        }

        public override void TransitionOut(Action onObscured)
        {
            StartCoroutine(TransitionRoutine(0, 1, Duration, onObscured));
        }

		private IEnumerator TransitionRoutine(float initial, float fadeTo, float duration, System.Action onFinished)
        {
            if (duration == 0) 
            {
                onFinished.Invoke();
                yield break;
            }
			
            CanvasGroup.alpha = 1;

            float t = Time.time;
            while (Time.time - t <= duration)
            {
				_image.material.SetFloat(_propertyID, Mathf.Lerp(initial, fadeTo, (Time.time - t) / duration));
                yield return null;
            }
            
            onFinished.Invoke();
        }
	}
}