using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : CharacterBase
{
    public void SetObjData(test_Enemy_SO newData)
    {
        if (newData == null) return;

        _thisName = newData._enemyName;
        _sprite.sprite = newData._enemySprite;
        _maxHealth = newData._totalHealth;
        health = _maxHealth;
        attack = newData._damage;
        _protection = newData._protection;
        _criticalProbability = newData._criticalProbability;
        _missProbability = newData._missProbability;

        //Actualizar interfaz (nombre y sprite)
        CanvasManager.Instance.SetEnemyData(_thisName);
    }
}
