using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum CurrentTurn
{
    PlayerTurn, EnemyTurn, GameOver
}

public class CombatManager : GenericSingleton<CombatManager>
{
    //Crear delegado para cambio de turno
    public delegate void OnEndRound();
    public OnEndRound RoundFinished;
    //Crear delegado para los ataques (acepta int de daño base, float de critical y float de miss)
    public delegate void OnDoDamage(int damage, float critical, float miss);
    public OnDoDamage GetTotalDamage;
    //Iniciar delegado para cuando reciba daño (delegado acepta float (porcentaje de vida) y bool (jugador o enemigo))
    public delegate void OnSetHealthBar(float newBarPercentege);
    public OnSetHealthBar SetPlayerHealth;
    public OnSetHealthBar SetEnemyHealth;
    //Iniciar delegado para cambiar los datos de lo que suceda en el combate
    public delegate void OnSetData(string dataString);
    public OnSetData SetData;

    CurrentTurn _thisTurn = CurrentTurn.PlayerTurn;
    Vector3 _originalMoverPosition;
    int _turnDamage;
    bool _wasCritical;
    bool _wasMissed;
    [SerializeField] CharacterBase _player;
    [SerializeField] CharacterBase _enemy;
    [SerializeField] FloatNumber _floatNumberRef;
    [Header("Audio")]
    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip _hitClip;
    [SerializeField] AudioClip _criticalClip;
    [SerializeField] AudioClip _missClip;

    private void Start()
    {
        //Unir delegado de cambio de turno a EndTurn
        RoundFinished += EndTurn;
        //Unir las funciones de Basic, Critical y Miss al delegado de Ataques
        GetTotalDamage = BasicDamage;
        GetTotalDamage += CriticalDamage;
        GetTotalDamage += MissChange;
        //Unir funciones EndCombat en caso de que mueran todos los enemigos o muera el jugador
        EnemyManager.Instance.CombatEnd += EndCombat;
        _player.Death += EndCombat;
    }

    public void PressAttackButton()
    {
        //Desactivar botón
        CanvasManager.Instance.EnableAttackButton(false);
        //Iniciar ataque del jugador
        Attack(_player, _enemy);
    }

    public void ShowUsableItems()
    {
        CanvasManager.Instance.EnableUseItemButton(false);
        ShowItems();
    }

    void ShowItems()
    {
        for (int i = 0; i < _player.consumableItems.Count; i++)
        {
            CanvasManager.Instance.CreateConsumableButton(_player.consumableItems[i]);
        }
    }

    void BasicDamage(int baseDamage, float criticalProbability, float missProbability)
    {
        //Set de damage
        float damage = baseDamage * Random.Range(0.8f, 1.2f);
        _turnDamage = Mathf.RoundToInt(damage); //Se ajusta de primeras al daño base
    }

    void CriticalDamage(int baseDamage, float criticalProbability, float missProbability)
    {
        //Modificar damage
        if (Random.value < criticalProbability)
        {
            print("Did Critical");
            _wasCritical = true;
            float damage = baseDamage * Random.Range(1.75f, 2.3f);
            _turnDamage = Mathf.RoundToInt(damage); //Se ajusta el daño si es critico
        }
    }

    void MissChange(int baseDamage, float criticalProbability, float missProbability)
    {
        //Set damage a 0
        if (Random.value < missProbability)
        {
            print("Miss change");
            _wasMissed = true;
            _turnDamage = 0;
        }
    }

    void Attack(CharacterBase attacker, CharacterBase defender)
    {
        _originalMoverPosition = attacker.transform.position;
        //Movimiento por DoTween al rival
        Vector3 directionToDefender = (defender.transform.position - attacker.transform.position).normalized;
        var sequence = DOTween.Sequence();
        sequence.Insert(0, attacker.transform.DOMove((defender.transform.position - directionToDefender * 1.2f), 1.0f));

        //Iniciar coroutine al acabar el movimiento
        sequence.OnComplete(() => StartCoroutine(AttackCoroutine(attacker, defender)));
    }

    IEnumerator AttackCoroutine(CharacterBase attacker, CharacterBase defender)
    {
        //Calcular daño (llamar al delegado de ataques)
        GetTotalDamage?.Invoke(attacker._damage, attacker._criticalProbability, attacker._missProbability);
        if (_turnDamage - defender._protection <= 0 && !_wasMissed) _turnDamage = defender._protection + 5; //Si la protección anulara por completo el ataque sin ser miss se hará un mínimo de 5pts de daño
        //Hacer daño
        _floatNumberRef.SetFloatNumber(_turnDamage - defender._protection, attacker._damage - defender._protection, _thisTurn);
        SetAttackData(attacker._thisName, defender._thisName, defender._protection);
        defender.TakeDamage(_turnDamage - defender._protection);
        SetDefenderHealthBar(defender._maxHealth, defender.health);
        //Movimiento por DoTween al punto de origen
        attacker.transform.DOMove(_originalMoverPosition, 1.25f);
        //Iniciar WaitForSeconds
        yield return new WaitForSeconds(4.5f);
        //Lanzar delegado que avise al cambio de turno
        RoundFinished?.Invoke();
    }

    void SetDefenderHealthBar(int defenderMaxHealth, int defenderHealth)
    {
        if (_thisTurn == CurrentTurn.EnemyTurn)
        {
            SetPlayerHealth?.Invoke((float)defenderHealth / defenderMaxHealth);
        }
        else if (_thisTurn == CurrentTurn.PlayerTurn)
        {
            SetEnemyHealth?.Invoke((float)defenderHealth / defenderMaxHealth);
        }
    }

    void SetAttackData(string attackerName, string defenderName, int defenderProtection)
    {
        if (_wasMissed)
        {
            SetData?.Invoke(attackerName + " missed a chance to hit " + defenderName);
            _audioSource.PlayOneShot(_missClip);
        }
        else if (_wasCritical)
        {
            SetData?.Invoke(attackerName + " did a critical hit of " + (_turnDamage - defenderProtection) + "pts to " + defenderName);
            _audioSource.PlayOneShot(_criticalClip);
        }
        else
        {
            SetData?.Invoke(attackerName + " did a hit of " + (_turnDamage - defenderProtection) + "pts to " + defenderName);
            _audioSource.PlayOneShot(_hitClip);
        }

        _wasCritical = false;
        _wasMissed = false;
    }

    void EndTurn()
    {
        if (_thisTurn == CurrentTurn.PlayerTurn)//Si es turno de jugador
        {
            //-----Check si el enemigo a muerto

            //--------Si ha muerto se avisa al EnemyManager para ver si quedan mas enemigos o si ya gana el Player (corta el resto de script con return)

            //--------Si no ha muerto sigue normal

            //-----Pasa a turno enemigo
            _thisTurn = CurrentTurn.EnemyTurn;
            //-----Inicia un nuevo Attack pero atacando enemigo
            Attack(_enemy, _player);
            //-----Resetear las variables cambiadas por objetos
            ResetPlayerStats();
            SetData?.Invoke(_enemy._thisName + " is going to attack");
        }
        else if (_thisTurn == CurrentTurn.EnemyTurn)//Si es turno del enemigo
        {
            //-----Comprobar que el player no ha muerto

            //--------Si ha muerto pierde y sale interfaz que puede reiniciar interfaz (corta el resto de script con return (no se activa el boton de atacar))

            //--------Si no ha muerto sigue normal

            //-----Pasa a turno de jugador
            _thisTurn = CurrentTurn.PlayerTurn;
            //-----Activa boton attack para que pueda atacar de nuevo
            CanvasManager.Instance.EnableAttackButton(true);
            CanvasManager.Instance.EnableUseItemButton(true); 
            SetData?.Invoke(_player._thisName + "'s turn");
            ResetPlayerDefense();
        }
        else if (_thisTurn == CurrentTurn.GameOver)//Si se acabó el combate
        {
            CanvasManager.Instance.ShowEndPanel(_player.health > 0); //Activo panel final. Si la vida del player es > 0 es porque habrá ganado
        }
    }

    void ResetPlayerStats()
    {
        _player._damage = _player._initialDamage;
        _player._criticalProbability = _player._initialCriticalProbability;
    }

    void ResetPlayerDefense()
    {
        _player._protection = _player._initialProtection;
    }

    void EndCombat(string name)
    {
        _thisTurn = CurrentTurn.GameOver;
    }
}
