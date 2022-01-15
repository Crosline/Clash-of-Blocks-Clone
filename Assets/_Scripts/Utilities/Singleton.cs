using UnityEngine;

namespace _Utilities {
	
	// static instance, similar to a singleton, instead of destroying instances it overrides, reseting state etc.
	// Overrides Instances
	
	public abstract class StaticInstance<T> : MonoBehaviour where T : MonoBehaviour {
		public static T Instance { get; private set; }
		protected virtual void Awake() => Instance = this as T;
		
		protected virtual void OnApplicationQuit() {
			Instance = null;
			Destroy(gameObject);
		}
	}
	
	
	// Destroys on scene change
	// Singleton
	
	public abstract class Singleton<T> : StaticInstance<T> where T : MonoBehaviour {
		protected override void Awake() {
			if (Instance != null) Destroy(gameObject);
			base.Awake();
		}
		
	}
	
	
	// DontDestroyOnLoad
	// Singleton that is Persistent
	
	public abstract class PersistentSingleton<T> : Singleton<T> where T : MonoBehaviour {
		
		protected override void Awake() {
			base.Awake();
			DontDestroyOnLoad(gameObject);
		}
	}
	
}