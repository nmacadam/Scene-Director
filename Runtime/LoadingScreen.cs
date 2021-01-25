// ONI, Copyright (c) Nathan MacAdam, All rights reserved. 
// MIT License (See LICENSE file)

using System.Collections;
using UnityEngine;

namespace Oni.SceneManagement
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class LoadingScreen : MonoBehaviour, IAsyncOperationMonitor
    {
		private bool _isComplete = false;
		
		private AsyncOperation _operation;
        public bool IsComplete { get => _isComplete; protected set => _isComplete = value; }

        public void Monitor(AsyncOperation operation)
        {
			_operation = operation;
            StartCoroutine(UpdateScreen(operation));
        }

        protected virtual IEnumerator UpdateScreen(AsyncOperation operation)
        {
            while (!operation.isDone)
            {
                float progress = Mathf.Clamp01(operation.progress / .9f);
				OnUpdateProgress(progress);

                yield return null;
            }

			_operation = null;
        }

		protected abstract void OnUpdateProgress(float progress);
    }
}