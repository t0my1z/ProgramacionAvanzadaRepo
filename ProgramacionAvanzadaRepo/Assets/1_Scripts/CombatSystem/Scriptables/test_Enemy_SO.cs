using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newEnemy", menuName = "ProgramacionAvanzadaRepo/Enemy", order = 0)]
public class test_Enemy_SO : ScriptableObject
{
    public string _enemyName;
    public Sprite _enemySprite;
    public int _totalHealth;
    public int _damage;
    public int _protection;
    public float _criticalProbability;
    public float _missProbability;
}
