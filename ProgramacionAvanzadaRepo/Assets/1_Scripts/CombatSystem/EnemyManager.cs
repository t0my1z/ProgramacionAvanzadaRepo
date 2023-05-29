using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyWaveArgs : EventArgs
{
    public int _numberOfEnemies;

    public EnemyWaveArgs(int numberOfEnemies)
    {
        _numberOfEnemies = numberOfEnemies;
    }
}

public class EnemyManager : GenericSingleton<EnemyManager>
{
    //Delegado para avisar al CombatManager de que se acabó
    public delegate void OnCombatEnded(string name);
    public OnCombatEnded CombatEnd;

    GenericPile<test_Enemy_SO> _roundEnemies;
    [SerializeField] Enemy _enemyObj;

    private void Start()
    {
        //Unirse al delegado de CanvasManager para iniciar ronda
        CanvasManager.Instance.OnStartCombatEvent += StartCombat;
        //Unirse al delegado para cuando muera un enemigo
        _enemyObj.Death += SendNextEnemy;
    }

    void StartCombat(object sender, EnemyWaveArgs e)
    {
        //Crear pila genérica de enemigos
        _roundEnemies = new GenericPile<test_Enemy_SO>();
        test_Enemy_SO[] availableEnemies = Resources.LoadAll<test_Enemy_SO>("TestEnemies");
        for (int i = 0; i < e._numberOfEnemies; i++)
        {
            _roundEnemies.AddItem(availableEnemies[i]);
        }
        //Segun el enemigo actual sacarlo de la pila
        _enemyObj.SetObjData(_roundEnemies.GetHead());
        //Activar el boton de AttackButton
        CanvasManager.Instance.EnableAttackButton(true);
    }

    void SendNextEnemy(string enemyName)
    {
        test_Enemy_SO nextEnemy = _roundEnemies.GetHead();
        if (nextEnemy == default)
        {
            CombatEnd?.Invoke(enemyName);
        }
        else
        {
            _enemyObj.SetObjData(nextEnemy);
        }
    }
}
