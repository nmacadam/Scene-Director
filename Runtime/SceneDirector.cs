// ONI, Copyright (c) Nathan MacAdam, All rights reserved. 
// MIT License (See LICENSE file)

#define ONI_SCENE_DIRECTOR

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Oni.SceneManagement
{
    /// <summary>
    /// Scriptable Object wrapper for the Unity SceneManager class to provide stricter access, more features, and customizability
    /// without MonoBehaviours or Singletons
    /// </summary>
    [CreateAssetMenu(fileName = "Scene Director", menuName = "Oni/Scene Director")]
    public class SceneDirector : ScriptableObject
    {
		[Header("Loading Screen")]
        [SerializeField] private SceneReference _loadingScreenScene = default;
        [SerializeField] private LoadingScreen _loadingScreenUI = default;

		[Header("Transitions")]
		[SerializeField] private bool _transitionsEnabled = true;
        [SerializeField] private SceneTransition _defaultTransition = default;
		[SerializeField] private bool _resetTransitionsOnSceneLoaded = true;

		private SceneTransition _inTransition = null;
		private SceneTransition _outTransition = null;

        public SceneReference LoadingScreenScene => _loadingScreenScene;
        public LoadingScreen LoadingScreenUI { get => _loadingScreenUI; set => _loadingScreenUI = value; }

        public SceneTransition DefaultTransition { get => _defaultTransition; set => _defaultTransition = value; }
        public bool ResetTransitionsOnSceneLoaded { get => _resetTransitionsOnSceneLoaded; set => _resetTransitionsOnSceneLoaded = value; }

        private SceneTransition _activeTransition = null;
		private bool _usingTransitions = false;

        private System.Action<Scene, LoadSceneMode> _onBeforeSceneLoad = delegate {};
        private System.Action<Scene, LoadSceneMode> _onSceneLoaded = delegate {};
        private System.Action<Scene, Scene> _onSceneChanged = delegate {};

        public System.Action<Scene, LoadSceneMode> OnBeforeSceneLoad { get => _onBeforeSceneLoad; set => _onBeforeSceneLoad = value; }
        public System.Action<Scene, LoadSceneMode> OnSceneLoaded { get => _onSceneLoaded; set => _onSceneLoaded = value; }
        public System.Action<Scene, Scene> OnSceneChanged { get => _onSceneChanged; set => _onSceneChanged = value; }


#region Unity Scene Manager Wrappers

		/// <summary> The total number of currently loaded Scenes. </summary>
        public int SceneCount => SceneManager.sceneCount;
		/// <summary> Number of Scenes in Build Settings. </summary>
        public int SceneCountInBuildSettings => SceneManager.sceneCountInBuildSettings;

		/// <summary>
		/// Create an empty new Scene at runtime with the given name.
		/// </summary>
		/// <param name="sceneName">The name of the new Scene. It cannot be empty or null, or same as the name of the existing Scenes.</param>
		/// <returns>A reference to the new Scene that was created, or an invalid Scene if creation failed. </returns>
        public Scene CreateScene(string sceneName)
        {
            return SceneManager.CreateScene(sceneName);
        }
        
		/// <summary>
		/// Create an empty new Scene at runtime with the given name.
		/// </summary>
		/// <param name="sceneName">The name of the new Scene. It cannot be empty or null, or same as the name of the existing Scenes.</param>
		/// <param name="parameters">Various parameters used to create the Scene.</param>
		/// <returns>A reference to the new Scene that was created, or an invalid Scene if creation failed. </returns>
        public Scene CreateScene(string sceneName, CreateSceneParameters parameters)
        {
            return SceneManager.CreateScene(sceneName, parameters);
        }

		/// <summary>
		/// Gets the currently active Scene.
		/// </summary>
		/// <returns>The active Scene. </returns>
        public Scene GetActiveScene()
        {
            return SceneManager.GetActiveScene();
        }

		/// <summary>
		/// Get the Scene at index in the SceneManager's list of loaded Scenes.
		/// </summary>
		/// <param name="index">Index of the Scene to get. Index must be greater than or equal to 0 and less than SceneManager.sceneCount.</param>
		/// <returns>A reference to the Scene at the index specified. </returns>
        public Scene GetSceneAt(int index)
        {
            return SceneManager.GetSceneAt(index);
        }

		/// <summary>
		/// Get a Scene struct from a build index.
		/// </summary>
		/// <param name="buildIndex">Build index as shown in the Build Settings window.</param>
		/// <returns>A reference to the Scene, if valid. If not, an invalid Scene is returned. </returns>
        public Scene GetSceneByBuildIndex(int buildIndex)
        {
            return SceneManager.GetSceneByBuildIndex(buildIndex);
        }

		/// <summary>
		/// Searches through the Scenes loaded for a Scene with the given name.
		/// </summary>
		/// <param name="name">Name of Scene to find.</param>
		/// <returns>A reference to the Scene, if valid. If not, an invalid Scene is returned. </returns>
        public Scene GetSceneByName(string name)
        {
            return SceneManager.GetSceneByName(name);
        }

		/// <summary>
		/// Searches all Scenes loaded for a Scene that has the given asset path.
		/// </summary>
		/// <param name="scenePath">Path of the Scene. Should be relative to the project folder. Like: "Assets/MyScenes/MyScene.unity".</param>
		/// <returns>A reference to the Scene, if valid. If not, an invalid Scene is returned. </returns>
        public Scene GetSceneByPath(string scenePath)
        {
            return SceneManager.GetSceneByPath(scenePath);
        }

		/// <summary>
		/// This will merge the source Scene into the destinationScene.
		/// </summary>
		/// <param name="sourceScene">The Scene that will be merged into the destination Scene.</param>
		/// <param name="destinationScene">Existing Scene to merge the source Scene into.</param>
		public void MergeScenes(Scene sourceScene, Scene destinationScene)
		{
			SceneManager.MergeScenes(sourceScene, destinationScene);
		}

		/// <summary>
		/// Move a GameObject from its current Scene to a new Scene.
		/// </summary>
		/// <param name="go">GameObject to move.</param>
		/// <param name="scene">Scene to move into.</param>
		public void MoveGameObjectToScene(GameObject go, Scene scene)
		{
			SceneManager.MoveGameObjectToScene(go, scene);
		}

		/// <summary>
		/// Set the Scene to be active.
		/// </summary>
		/// <param name="scene">The Scene to be set.</param>
		/// <returns>Returns false if the Scene is not loaded yet. </returns>
		public bool SetActiveScene(Scene scene)
		{
			return SceneManager.SetActiveScene(scene);
		}

		/// <summary>
		/// Destroys all GameObjects associated with the given Scene and removes the Scene from the SceneManager.
		/// </summary>
		/// <param name="scene">Scene to unload.</param>
		/// <returns> Use the AsyncOperation to determine if the operation has completed. </returns>
		public AsyncOperation UnloadSceneAsync(Scene scene)
		{
			return SceneManager.UnloadSceneAsync(scene);
		}

		/// <summary>
		/// Destroys all GameObjects associated with the given Scene and removes the Scene from the SceneManager.
		/// </summary>
		/// <param name="scene">Scene to unload.</param>
		/// <param name="options">Scene unloading options.</param>
		/// <returns> Use the AsyncOperation to determine if the operation has completed. </returns>
		public AsyncOperation UnloadSceneAsync(Scene scene, UnloadSceneOptions options)
		{
			return SceneManager.UnloadSceneAsync(scene, options);
		}

		/// <summary>
		/// Destroys all GameObjects associated with the given Scene and removes the Scene from the SceneManager.
		/// </summary>
		/// <param name="sceneReference">SceneReference to the Scene to unload</param>
		/// <returns> Use the AsyncOperation to determine if the operation has completed. </returns>
		public AsyncOperation UnloadSceneAsync(SceneReference sceneReference)
		{
			return SceneManager.UnloadSceneAsync(sceneReference.BuildIndex);
		}

		/// <summary>
		/// Destroys all GameObjects associated with the given Scene and removes the Scene from the SceneManager.
		/// </summary>
		/// <param name="sceneReference">SceneReference to the Scene to unload</param>
		/// <param name="options">Scene unloading options.</param>
		/// <returns> Use the AsyncOperation to determine if the operation has completed. </returns>
		public AsyncOperation UnloadSceneAsync(SceneReference sceneReference, UnloadSceneOptions options)
		{
			return SceneManager.UnloadSceneAsync(sceneReference.BuildIndex, options);
		}

#endregion

#region Scene Loading

        /// <summary>
        /// Load the given scene immediately
        /// </summary>
        public void LoadSceneImmediate(SceneReference scene, bool useTransitions = true, LoadSceneMode mode = LoadSceneMode.Single, System.Action onTransitioned = null)
        {
			_usingTransitions = useTransitions;
            if (useTransitions && _outTransition != null)
            {
				_activeTransition = GameObject.Instantiate(_outTransition);
                _activeTransition.name = "Transition";
                _activeTransition.TransitionOut(() => 
                { 
                    onTransitioned?.Invoke(); 
                    LoadSceneOperation(scene.BuildIndex, mode); 
                });
            }
            else
            {
                onTransitioned?.Invoke();
                LoadSceneOperation(scene.BuildIndex, mode);
            }
        }

        private void LoadSceneOperation(int buildIndex, LoadSceneMode mode)
        {
            _onBeforeSceneLoad.Invoke(GetSceneByBuildIndex(buildIndex), mode);
            SceneManager.LoadScene(buildIndex, mode);
        }

        /// <summary>
        /// Load the given scene asynchronously
        /// </summary>
        public void LoadSceneAsync(SceneReference scene, bool useTransitions = true, LoadSceneMode mode = LoadSceneMode.Single)
        {
			_usingTransitions = useTransitions;
            AsyncLoadHandler asyncHandler = new GameObject().AddComponent<AsyncLoadHandler>();
            asyncHandler.StartCoroutine(LoadAsyncRoutine(scene, useTransitions, asyncHandler.gameObject, mode));
        }

        /// <summary>
        /// Load the given scene with a loading screen
        /// </summary>
        public void LoadSceneWithLoadingScreen(SceneReference scene, bool useTransitions = true)
        {
			_usingTransitions = useTransitions;
            AsyncLoadHandler asyncHandler = new GameObject().AddComponent<AsyncLoadHandler>();
            asyncHandler.StartCoroutine(LoadAsyncWithLoadingScreenRoutine(scene, useTransitions, asyncHandler.gameObject));
        }

        private IEnumerator LoadAsyncRoutine(SceneReference scene, bool useTransitions, GameObject handler, LoadSceneMode mode = LoadSceneMode.Single)
        {
            _onBeforeSceneLoad.Invoke(GetSceneByBuildIndex(scene.BuildIndex), mode);
    
            // Transition out of current screen
            var screenOccluded = false;
			if (_transitionsEnabled && useTransitions && _outTransition != null)
            {
                _activeTransition = GameObject.Instantiate(_outTransition);
                _activeTransition.name = "Transition";
                _activeTransition.TransitionOut(() => screenOccluded = true);

				// wait for transition to complete
                yield return new WaitUntil(() => screenOccluded);
            }

            // Without a frame-break the allowSceneActivation flag doesn't work! Not sure why!
            yield return null;

            // Start async scene load to target scene
            var operation = SceneManager.LoadSceneAsync(scene.BuildIndex, mode);
            operation.allowSceneActivation = false;

            // Wait until scene is loaded
            yield return new WaitUntil(() => operation.progress >= 0.9f);
            
            // Activate the scene
            operation.allowSceneActivation = true;

            // Destroy the coroutine managing object
            Destroy(handler);
        }

        private IEnumerator LoadAsyncWithLoadingScreenRoutine(SceneReference scene, bool useTransitions, GameObject handler)
        {
            _onBeforeSceneLoad.Invoke(GetSceneByBuildIndex(scene.BuildIndex), LoadSceneMode.Single);

            // Go to loading screen
            var screenOccluded = false;
            LoadSceneImmediate(_loadingScreenScene, useTransitions, LoadSceneMode.Single, () => screenOccluded = true);

            // Wait for transition to complete
            yield return new WaitUntil(() => screenOccluded);

            // Without a frame-break the allowSceneActivation flag doesn't work! Not sure why!
            yield return null;

            // Instantiate async operation dependent elements
            LoadingScreen ui = null;
            if (_loadingScreenUI != null)
            {
                ui = GameObject.Instantiate(_loadingScreenUI);
            }

            // Start async scene load to target scene
            var operation = SceneManager.LoadSceneAsync(scene.BuildIndex);
            operation.allowSceneActivation = false;

            // Monitor async operation
            if (_loadingScreenUI != null && ui != null)
            {
                ui.Monitor(operation);
            }

            // Wait until scene is loaded
            yield return new WaitUntil(() => operation.progress >= 0.9f);

			if (ui != null)
            {
                yield return new WaitUntil(() => ui.IsComplete);
            }

            yield return new WaitForSeconds(1f);

            // Transition out of loading screen
            screenOccluded = false;
			if (_transitionsEnabled && useTransitions && _outTransition != null)
            {
                _activeTransition = GameObject.Instantiate(_outTransition);
                _activeTransition.name = "Transition";
                _activeTransition.TransitionOut(() => screenOccluded = true);

				// wait for transition to complete
                yield return new WaitUntil(() => screenOccluded);
            }
            
            // Activate the scene
            operation.allowSceneActivation = true;

            // Destroy the coroutine managing object
            Destroy(handler);
        }

#endregion

#region Transitions

		public void SetInTransition(SceneTransition prefab)
		{
			_inTransition = prefab;
		}

		public void SetOutTransition(SceneTransition prefab)
		{
			_outTransition = prefab;
		}

		public void ResetTransitions()
		{
			_inTransition = _defaultTransition;
			_outTransition = _defaultTransition;
		}

#endregion

#region Unity Events

        private void OnEnable() 
        {
            SceneManager.sceneLoaded += OnSceneLoadedInvoke;
			SceneManager.activeSceneChanged += OnSceneChangedInvoke;
        }

        private void OnDisable() 
        {
            SceneManager.sceneLoaded -= OnSceneLoadedInvoke;
			SceneManager.activeSceneChanged += OnSceneChangedInvoke;
        }

		private void OnSceneChangedInvoke(Scene a, Scene b)
		{
			_onSceneChanged.Invoke(a, b);

            if (_transitionsEnabled && _usingTransitions && _inTransition != null)
            {
                _activeTransition = GameObject.Instantiate(_inTransition);
                _activeTransition.name = "Transition";
                _activeTransition.TransitionIn(delegate {});
            }

			if (_resetTransitionsOnSceneLoaded)
			{
				ResetTransitions();
			}

			_usingTransitions = _transitionsEnabled;
		}

        private void OnSceneLoadedInvoke(Scene scene, LoadSceneMode mode)
        {
            _onSceneLoaded.Invoke(scene, mode);

			if (_transitionsEnabled && _usingTransitions && mode == LoadSceneMode.Additive && _inTransition != null)
            {
				if (_activeTransition != null) Destroy(_activeTransition.gameObject);

                _activeTransition = GameObject.Instantiate(_inTransition);
                _activeTransition.name = "Transition";
                _activeTransition.TransitionIn(delegate {});

				if (_resetTransitionsOnSceneLoaded)
				{
					ResetTransitions();
				}
            }

			_usingTransitions = _transitionsEnabled;
        }
        
#endregion
    }
}