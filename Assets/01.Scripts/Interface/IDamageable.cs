public interface IDamageable
{
    public abstract void TakeDamage(float damage, HighSpellTypeEnum AttackedType);

    public void TakeHeal(float heal);

    public void Die();
}
