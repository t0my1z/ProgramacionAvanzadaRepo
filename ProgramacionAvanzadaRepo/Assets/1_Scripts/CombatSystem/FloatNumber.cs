using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FloatNumber : MonoBehaviour
{
    [SerializeField] RectTransform _thisRect;
    [SerializeField] TextMeshProUGUI _txt;
    [SerializeField] AnimationCurve _horizontalMovement;
    const float _maxTimer = 2.5f;
    float _timer;
    float _originalXPos = 200;

    private void Update()
    {
        _timer -= Time.deltaTime;
        if (_timer < 0)
        {
            _timer = _maxTimer;
            gameObject.SetActive(false);
            _thisRect.anchoredPosition = new Vector2(_thisRect.anchoredPosition.x, 0.0f);
        }
        else
        {
            _thisRect.anchoredPosition = new Vector2(_thisRect.anchoredPosition.x + _horizontalMovement.Evaluate(_timer / _maxTimer) * Time.deltaTime, Mathf.Lerp(90, 0, _timer / _maxTimer));
            _txt.color = new Vector4(_txt.color.r, _txt.color.g, _txt.color.b, _timer / _maxTimer);
        }
    }

    public void SetFloatNumber(int damage, int attackerBaseDamage, CurrentTurn turn)
    {
        damage = Mathf.Clamp(damage, 0, 999);
        Color textColor;

        if (damage > (float)attackerBaseDamage * 1.5f) //Ha hecho más daño de lo normal
        {
            textColor = Color.red;
        }
        else if (damage == 0) //Ha fallado el golpe
        {
            textColor = Color.white;
        }
        else //Ha hecho daño promedio
        {
            textColor = Color.yellow;
        }

        if (turn == CurrentTurn.EnemyTurn)
        {
            _thisRect.anchoredPosition = new Vector2(-_originalXPos, 0.0f);
        }
        else
        {
            _thisRect.anchoredPosition = new Vector2(_originalXPos, 0.0f);
        }

        _txt.text = damage.ToString();
        _txt.color = textColor;
        gameObject.SetActive(true);
        _timer = _maxTimer;
    }
}
