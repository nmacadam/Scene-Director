// ONI, Copyright (c) Nathan MacAdam, All rights reserved. 
// MIT License (See LICENSE file)

using System.Collections;
using UnityEngine;

namespace Oni.SceneManagement.Transitions
{
	/// <summary>
    /// Fade in/out transition
    /// </summary>
	public class FadeTransition : CanvasBasedTransition
	{
        public override void TransitionIn(System.Action onVisible)
        {
            CanvasGroup.alpha = 1;
            StartCoroutine(FadeRoutine(1, 0, Duration, onVisible));
        }

        public override void TransitionOut(System.Action onObscured)
        {
            CanvasGroup.alpha = 0;
            StartCoroutine(FadeRoutine(0, 1, Duration, onObscured));
        }

        private IEnumerator FadeRoutine(float initial, float fadeTo, float duration, System.Action onFinished)
        {
            if (duration == 0) 
            {
                onFinished.Invoke();
                yield break;
            }

            float t = Time.time;
            while (Time.time - t <= duration && CanvasGroup.alpha != fadeTo)
            {
                CanvasGroup.alpha = Mathf.Lerp(initial, fadeTo, (Time.time - t) / duration);
                yield return null;
            }
            
            CanvasGroup.alpha = fadeTo;
            onFinished.Invoke();
        }
	}
}