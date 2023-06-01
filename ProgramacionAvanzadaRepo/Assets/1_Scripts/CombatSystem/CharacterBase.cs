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
    int _attack;
    public int attack
    {
        get { return _attack; }
        set
        {
            _attack = Mathf.Clamp(value, 0, 9999);
        }
    }
    int _defence;
    public int defence
    {
        get { return _defence; }
        set
        {
            _defence = Mathf.Clamp(value, 0, 9999);
        }
    }

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

    public void SetConsumableItems()
    {
        for (int i = 0; i < consumableItems.Count; i++)
        {
            consumableItems[i]._char = this;
        }
    }
}
