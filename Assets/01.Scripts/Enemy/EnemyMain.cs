using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyMain : MonoCharacter, IDamageable
{
	private Animator EnemyAnimator;
	private SpriteRenderer EnemyRenderer;

	[Header("Enemy Datas")]
	[SerializeField] private List<HighSpellTypeEnum> WeaknessTypes;
	[SerializeField] private List<HighSpellTypeEnum> StrengthTypes;
	[SerializeField][Range(1, 10)] private int MaxPatience;

	[Header("Enemy Children")]
	[SerializeField] private Transform EnemyVisual;
	[SerializeField] private TMP_Text PatienceTxt;

	private int CurrentPatience = 0;

	private void Start()
	{
		EnemyVisual.TryGetComponent(out EnemyAnimator);
		EnemyVisual.TryGetComponent(out EnemyRenderer);
	}

	public void SetUpEnemy()
	{
		CurrentHP = MaxHP;
		CurrentPatience = MaxPatience;
	}

	public void NoticeAttack()
	{

	}

	public void ActiveAttack()
	{

	}

	public void TakeDamage(float damage, HighSpellTypeEnum AttackedType)
	{
		if (WeaknessTypes.Contains(AttackedType)) CurrentHP = CurrentHP - (damage * 1.5f);
		else if(StrengthTypes.Contains(AttackedType)) CurrentHP = CurrentHP - (damage * 0.5f);
		else CurrentHP = CurrentHP - damage;

		if (CurrentHP <= 0) Die();
	}

	public void TakeHeal(float heal)
	{
		CurrentHP = Mathf.Clamp(CurrentHP + heal, 0, MaxHP);
	}

	public void Die()
	{
	}

}
