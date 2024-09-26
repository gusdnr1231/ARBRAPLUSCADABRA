using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyMain : MonoCharacter, IDamageable
{
	private Animator EnemyAnimator;
	private SpriteRenderer EnemyRenderer;
	private EnemyAttack _enemyAttack;

	[Header("Enemy Datas")]
	[SerializeField] private List<HighSpellTypeEnum> WeaknessTypes;
	[SerializeField] private List<HighSpellTypeEnum> StrengthTypes;
	[SerializeField][Range(1, 10)] private int MaxPatience;

	[Header("Enemy Children")]
	[SerializeField] private Transform EnemyVisual;
	[SerializeField] private TMP_Text PatienceTxt;

	private int CurrentPatience = 0;

	private Managers _mngs;
	private GameManager _gameMng;
	private SpellManager _spellMng;

	private void Start()
	{
		EnemyVisual.TryGetComponent(out EnemyAnimator);
		EnemyVisual.TryGetComponent(out EnemyRenderer);

		_enemyAttack = GetComponent<EnemyAttack>();
		_enemyAttack.Initialize(this);

		_mngs = Managers.GetInstacne();
		_gameMng = _mngs.GetManager<GameManager>();
		_spellMng = _mngs.GetManager<SpellManager>();

		MapManager.OnCompleteLoadingMap += SetUpEnemy;
		_gameMng.OnStartTurn += UpdateTurn;

	}

	public void MoveEnemy()
	{
		CurrentTileNumber.x = Random.Range(0, 7);
		CurrentTileNumber.y = Random.Range(0, 7);

		transform.position = MapManager.Instance.SettedTiles[CurrentTileNumber].ReturnTilePosition();
		MapManager.Instance.SettedTiles[CurrentTileNumber].SetOnCharacter(this);
	}

	public void SetUpEnemy()
	{
		CurrentHP = MaxHP;
		CurrentPatience = MaxPatience;

		UpdatePatience(true);

		MoveEnemy();
	}

	public void UpdateTurn()
	{

		UpdatePatience(false);
	}

	public void UpdatePatience(bool isFirst)
	{
		if (isFirst == false) CurrentPatience = CurrentPatience - 1;

		PatienceTxt.text = CurrentPatience.ToString();
		if (CurrentPatience <= 1) PatienceTxt.color = Color.red;
		else if (CurrentPatience > 1) PatienceTxt.color = Color.white;
		
		if (CurrentPatience == 1) NoticeAttack();
		if (CurrentPatience == 0) ActiveAttack();
	}

	public void NoticeAttack()
	{
		Debug.Log("Notice Enemy AttackZone");
		_spellMng.AddEnemyAttackZone(_enemyAttack.CalculateAttackRange());
	}

	public void ActiveAttack()
	{
		CurrentPatience = MaxPatience;
		Debug.Log("Active Enemy AttackZone");
		//_spellMng.ActiveAttack(_enemyAttack.CalculateAttackRange(), TileState.EnemyAttack);
	}

	public void TakeDamage(float damage, HighSpellTypeEnum AttackedType)
	{
		if (WeaknessTypes.Contains(AttackedType)) CurrentHP = CurrentHP - (damage * 1.5f);
		else if(StrengthTypes.Contains(AttackedType)) CurrentHP = CurrentHP - (damage * 0.5f);
		else CurrentHP = CurrentHP - damage;

		if (CurrentHP <= 0) Die();
		Debug.Log("Active Enemy Take Damage: " + CurrentHP);

	}

	public void TakeHeal(float heal)
	{
		CurrentHP = Mathf.Clamp(CurrentHP + heal, 0, MaxHP);
	}

	public void Die()
	{
		Destroy(gameObject);
	}

}
