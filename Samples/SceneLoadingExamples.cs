// ONI, Copyright (c) Nathan MacAdam, All rights reserved. 
// MIT License (See LICENSE file)

using UnityEngine;

namespace Oni.SceneManagement.Demos
{
	public class SceneLoadingExamples : MonoBehaviour
	{
		[SerializeField] private SceneReference _next = new SceneReference();
		[SerializeField] private SceneReference _additiveContent = new SceneReference();
		[SerializeField] private SceneDirector _director = default;
		[SerializeField] private SceneTransition _customTransitionOut = default;
		[SerializeField] private SceneTransition _customTransitionIn = default;

		public void SetCustomTransitions()
		{
			_director.SetInTransition(_customTransitionIn);
			_director.SetOutTransition(_customTransitionOut);
		}

		public void Load()
		{
			_director.LoadSceneImmediate(_next);
		}

		public void LoadAsync()
		{
			_director.LoadSceneAsync(_next);
		}

		public void LoadWithScreen()
		{
			_director.LoadSceneWithLoadingScreen(_next);
		}

		// public void LoadAsyncWithScreen()
		// {
		// 	_director.LoadSceneAsync(_next);
		// }

		public void LoadAdditive()
		{
			_director.LoadSceneImmediate(_additiveContent, false, UnityEngine.SceneManagement.LoadSceneMode.Additive);
		}

		public void LoadAdditiveAsync()
		{
			_director.LoadSceneAsync(_additiveContent, true, UnityEngine.SceneManagement.LoadSceneMode.Additive);
		}
	}
}