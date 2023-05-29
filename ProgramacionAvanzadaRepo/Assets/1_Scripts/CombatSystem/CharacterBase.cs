using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    //Delegado para avisar a EnemyManager de que ha muerto el enemigo o al CombatManager de que ha muerto el jugador
    public delegate void OnCharacterDeath(string name);
    public OnCharacterDeath Death;

    public string _thisName;
    public SpriteRenderer _sprite;
    public int _maxHealth;
    private int _health;
    public int health
    {
        get { return _health; }
        set
        {
            _health = Mathf.Clamp(value, 0, _maxHealth);
            if (_health == 0)
            {
                Death?.Invoke(_thisName + " was killed");
            }
        }
    }
    public int _damage;
    public int _protection;
    public float _criticalProbability;
    public float _missProbability;

    private void Awake()
    {
        health = _maxHealth;
    }

    private void Start()
    {
        Death += CanvasManager.Instance.SetDataInfo;
    }

    public void TakeDamage(int damageReceived)
    {
        health -= damageReceived;
    }
}
