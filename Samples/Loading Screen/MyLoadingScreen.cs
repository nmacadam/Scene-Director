// ONI, Copyright (c) Nathan MacAdam, All rights reserved. 
// MIT License (See LICENSE file)

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Oni.SceneManagement.Demos
{
    public class MyLoadingScreen : LoadingScreen
    {
		[SerializeField] private Slider _slider = default;
		[SerializeField] private float _minLoadTime = 3f;

		private float _t;

		private void Update() 
		{
			_t += Time.deltaTime;
		}

        protected override void OnUpdateProgress(float progress)
        {
			float modifiedProgress;

			if (_minLoadTime == 0)
			{
				modifiedProgress = progress;
			}
			else
			{
            	modifiedProgress = Mathf.Min(progress, _t / _minLoadTime);
			}

			_slider.value = modifiedProgress;

			if (modifiedProgress >= 1f)
			{
				IsComplete = true;
			}
        }
    }
}