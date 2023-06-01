using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CanvasManager : GenericSingleton<CanvasManager>
{
    //Crear EventHandler con GenericPile
    public event EventHandler<EnemyWaveArgs> OnStartCombatEvent;

    [SerializeField] GameObject _startCombatPanel;
    [SerializeField] GameObject _combatPanel;
    [SerializeField] GameObject _winPanel;
    [SerializeField] GameObject _LosePanel;
    [SerializeField] GameObject _alertTxtObj;
    [SerializeField] TMP_InputField _numberInputField;
    [SerializeField] Button _attackButton;
    [SerializeField] Button _useItemButton;
    [SerializeField] Image _playerHealthBar;
    [SerializeField] Image _enemyHealthBar;
    [SerializeField] TextMeshProUGUI _enemyTmPro;
    [SerializeField] TextMeshProUGUI _dataTxtPro;

    [SerializeField] GameObject _consumableItemsButtonPrefabs;
    [SerializeField] GameObject _usableItemsPanel;

    private void Start()
    {
        CombatManager.Instance.SetPlayerHealth += SetPlayerHealthBar;
        CombatManager.Instance.SetEnemyHealth += SetEnemyHealthBar;
        CombatManager.Instance.SetData += SetDataInfo;

    }

    private class ConsumableItemUI
    {
        Button _consumableButton;
        ConsumableItem _item;

        public ConsumableItemUI(Button button, ConsumableItem item)
        {
            _consumableButton = button;
            _item = item;
        }

        public void SetButtonEventToItem()
        {
            _consumableButton.onClick.AddListener(_item.Interact);
            _consumableButton.onClick.AddListener(DestroyGameObject);
        }

        public void SetImageToItemSprite()
        {
            _consumableButton.GetComponent<Image>().sprite = _item.icon;
        }


        public void DestroyGameObject()
        {
            _item._char.consumableItems.Remove(_item);
            Destroy(_consumableButton.gameObject);
        }
    }

    public void DisableConsumableButtons()
    {
        Button[] consumableButtons = _usableItemsPanel.GetComponentsInChildren<Button>();

        for (int i = 0; i < consumableButtons.Length; i++)
        {
            Destroy(consumableButtons[i]);
        }
    }

    public void EnableUseItemButton(bool enable)
    {
        _useItemButton.interactable = enable;
    }

    public void CreateConsumableButton(ConsumableItem item)
    {
        GameObject instantiatedButton = Instantiate(_consumableItemsButtonPrefabs, _usableItemsPanel.transform);
        ConsumableItemUI consumItem = new ConsumableItemUI(instantiatedButton.GetComponent<Button>(), item);
        consumItem.SetButtonEventToItem();
        consumItem.SetImageToItemSprite();
        instantiatedButton.GetComponentInChildren<TextMeshProUGUI>().text = item.Name;
    }

    public void DestroyConsumableButtons()
    {
        foreach (Button item in _usableItemsPanel.GetComponentsInChildren<Button>())
        {
            Destroy(item.gameObject);
        }
    }

    public void StartCombatButton()
    {
        if (int.TryParse(_numberInputField.text, out int number))
        {
            print(number);
            if (number <= 0)
            {
                _alertTxtObj.SetActive(true);
                return;
            }

            //Quitar interfaz de empezar combate
            _startCombatPanel.SetActive(false);
            //Poner interfaz de combate
            _combatPanel.SetActive(true);
            //Lanzar EventHandler que lo escuche EnemyManager
            OnStartCombatEvent?.Invoke(this, new EnemyWaveArgs(number));
        }
        else
        {
            _alertTxtObj.SetActive(true);
        }
    }

    public void EnableAttackButton(bool enable)
    {
        _attackButton.interactable = enable;
    }

    public void SetEnemyData(string name)
    {
        _enemyTmPro.text = name;
        _enemyHealthBar.fillAmount = 1.0f;
    }

    public void SetPlayerHealthBar(float percentege)
    {
        _playerHealthBar.fillAmount = percentege;
    }

    public void SetEnemyHealthBar(float percentege)
    {
        _enemyHealthBar.fillAmount = percentege;
    }

    public void ShowEndPanel(bool playerWon)
    {
        _combatPanel.SetActive(false);

        _winPanel.SetActive(playerWon);
        _LosePanel.SetActive(!playerWon);
    }

    public void RestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SetDataInfo(string newData)
    {
        _dataTxtPro.text = newData;
    }
}
