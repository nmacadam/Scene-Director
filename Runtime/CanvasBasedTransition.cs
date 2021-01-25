// ONI, Copyright (c) Nathan MacAdam, All rights reserved. 
// MIT License (See LICENSE file)

using UnityEngine;

namespace Oni.SceneManagement
{
    public abstract class CanvasBasedTransition : SceneTransition
	{
		[SerializeField] private CanvasGroup _canvasGroup = default;
		
		public CanvasGroup CanvasGroup => _canvasGroup;
	}
}