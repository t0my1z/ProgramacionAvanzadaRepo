using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBase : MonoBehaviour, Interfaces.IDamageable
{
    //Delegado para avisar a EnemyManager de que ha muerto el enemigo o al CombatManager de que ha muerto el jugador
    public delegate void OnCharacterDeath(string name);
    public OnCharacterDeath Death;

    public string _thisName;
    public SpriteRenderer _sprite;
    public int _maxHealth;
    private int _health;

    public List<ConsumableItem> consumableItems = new List<ConsumableItem>(3);

    public int health
    {
        get { return _health; }
        set
        {
            _health = Mathf.Clamp(value, 0, _maxHealth);
            if (_health == 0)
            {
                Die();
            }
        }
    }
    public int _damage;
    public int _initialDamage; 
    public int _protection;
    public int _initialProtection; 
    public float _criticalProbability;
    public float _initialCriticalProbability; 
    public float _missProbability;

    private void Awake()
    {
        health = _maxHealth;
        _initialCriticalProbability = _criticalProbability;
        _initialDamage = _damage;
        _initialProtection = _protection;
        consumableItems.Add(new PrecisionPotion("Precision Potion", this));
        consumableItems.Add(new RagePotion("Rage Potion", this)); 
        consumableItems.Add(new RagePotion("Rage Potion", this)); 
    }

    private void Start()
    {
        Death += CanvasManager.Instance.SetDataInfo;
    }

    public void TakeDamage(int damageReceived)
    {
        health -= damageReceived;
    }

    public void Die()
    {
        Death?.Invoke(_thisName + " was killed");
    }
}
