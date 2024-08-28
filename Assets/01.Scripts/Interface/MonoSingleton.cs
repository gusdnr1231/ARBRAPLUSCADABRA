using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T _instance = null;
	private static bool _isQuitting = false;

	public static T GetInstacne() { return Instance; }

	public static T Instance
	{
		get
		{
			if (_isQuitting)
			{
				_instance = null;
			}

			if (_instance == null)
			{
				_instance = FindObjectOfType<T>();

				if (_instance == null)
				{
					Debug.Assert(_instance != null, $"{typeof(T).Name} is not exist");
				}
				else
				{
					_isQuitting = false;
				}
			}
			return _instance;
		}
	}
	public virtual void Awake()
	{
		if (Instance == this)
		{
			if (transform.root.gameObject.scene.name != "DontDestroyOnLoad")
			{
				DontDestroyOnLoad(transform.root.gameObject);
			}
		}
		else
		{
			Debug.LogErrorFormat($"{typeof(T).Name} is already running!");
			Destroy(gameObject);
		}
	}

	protected virtual void OnDisable()
	{
		_isQuitting = true;
		_instance = null;
	}
}
