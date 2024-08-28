using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMain : MonoCharacter, IDamageable
{
	private Dictionary<Type, IPlayerComponent> _components;

	public event Action OnSetUpPlayer;

	private void Awake()
	{
		_components = new Dictionary<Type, IPlayerComponent>();

		IPlayerComponent[] comArr = GetComponentsInChildren<IPlayerComponent>();
		foreach (var component in comArr)
		{
			_components.Add(component.GetType(), component);
		}

		foreach (IPlayerComponent compo in _components.Values)
		{
			compo.Initialize(this);
		}

		MapManager.OnCompleteLoadingMap += SetUpPlayerMain;
	}

	public T GetCompo<T>() where T : class
	{
		if (_components.TryGetValue(typeof(T), out IPlayerComponent compo))
			return compo as T;
		return default(T);
	}
	
	public void SetUpPlayerMain()
	{
		CurrentHP = MaxHP;

		CurrentTileNumber = new Vector2Int(3, 3);

		OnSetUpPlayer?.Invoke();
	}

	#region Damageable Methods

	public void TakeDamage(float damage, HighSpellTypeEnum AttackedType)
	{
		CurrentHP = CurrentHP - damage;
		if (CurrentHP <= 0) Die();
	}

	public void TakeHeal(float heal)
	{
		CurrentHP = Mathf.Clamp(CurrentHP + heal,0, MaxHP);
	}

	public void Die()
	{
	}

	#endregion
}
