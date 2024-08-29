using DG.Tweening;
using System;
using System.Collections.Generic;

public class Managers : MonoSingleton<Managers>
{
	private Dictionary<Type, IManagerComponent> _components;

	public override void Awake()
	{
		base.Awake();

		DOTween.SetTweensCapacity(1000, 50);

		_components = new Dictionary<Type, IManagerComponent>();

		IManagerComponent[] comArr = GetComponentsInChildren<IManagerComponent>();
		foreach (var component in comArr)
		{
			_components.Add(component.GetType(), component);
		}

		foreach (IManagerComponent compo in _components.Values)
		{
			compo.Initialize(this);
		}
	}

	public T GetManager<T>() where T : class
	{
		if (_components.TryGetValue(typeof(T), out IManagerComponent compo))
			return compo as T;
		return default(T);
	}
}
