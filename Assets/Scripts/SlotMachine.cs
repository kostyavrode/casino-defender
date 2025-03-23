using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class SlotRow
{
    public Image[] slots; // Один ряд (3 слота)
}

public class SlotMachine : MonoBehaviour
{
    [SerializeField] private List<SlotRow> slotRows = new List<SlotRow>(); // 3 ряда по 3 слота
    [SerializeField] private Sprite[] slotItems; // Набор предметов для выпадения
    [SerializeField] private Button spinButton; // Кнопка запуска
    [SerializeField] private Button increaseBetButton; // Кнопка увеличения ставки
    [SerializeField] private Button decreaseBetButton; // Кнопка уменьшения с тавки
    [SerializeField] private TMP_Text betText; // Текст текущей ставки

    [SerializeField] private TMP_Text coinText;
    
    [SerializeField] private GameObject winPanel;
    [SerializeField] private TMP_Text winText;
    //[SerializeField] private TMP_Text moneyText; // Текст денег
    [SerializeField] private int minBet = 1;
    [SerializeField] private int maxBet = 10;

    private int currentBet = 1;
    private bool isSpinning = false;
    private int money;

    private void Start()
    {
        money = PlayerPrefs.GetInt("Money", 100); // Начальные деньги (если их нет)
        UpdateUI();
        FillSlotsWithRandomItems();

        spinButton.onClick.AddListener(StartSpin);
        increaseBetButton.onClick.AddListener(() => ChangeBet(1));
        decreaseBetButton.onClick.AddListener(() => ChangeBet(-1));
    }

    private void ChangeBet(int change)
    {
        currentBet = Mathf.Clamp(currentBet + change, minBet, maxBet);
        UpdateUI();
    }
    
    private void FillSlotsWithRandomItems()
    {
        foreach (var row in slotRows)
        {
            for (int i = 0; i < row.slots.Length; i++)
            {
                row.slots[i].sprite = slotItems[Random.Range(0, slotItems.Length)];
            }
        }
    }

    private void UpdateUI()
    {
        betText.text = currentBet.ToString();
        coinText.text = money.ToString();
        //moneyText.text = "Баланс: " + money;
        
        spinButton.interactable = (money >= currentBet); // Отключаем кнопку если денег не хватает
    }

    public void StopGame()
    {
        Time.timeScale = 0;
    }

    public void UnPauseGame()
    {
        Time.timeScale = 1;
    }

    private void StartSpin()
    { 
        if (isSpinning || money < currentBet) return;

        money -= currentBet; // Списываем ставку
        winPanel.SetActive(false);
        UpdateUI();
        isSpinning = true;
        StartCoroutine(SpinSlots());
    }

    private IEnumerator SpinSlots()
    {
        float spinTime = 2f;
        float interval = 0.1f;

        float elapsedTime = 0f;

        while (elapsedTime < spinTime)
        {
            foreach (var row in slotRows)
            {
                for (int i = 0; i < row.slots.Length; i++)
                {
                    row.slots[i].sprite = slotItems[Random.Range(0, slotItems.Length)];
                }
            }

            yield return new WaitForSecondsRealtime(interval); // <- теперь не зависит от timeScale
            elapsedTime += interval;
        }

        isSpinning = false;
        CheckWinCondition();
    }

    private void CheckWinCondition()
    {
        int winnings = 0;

        // Проверяем совпадения по строкам
        foreach (var row in slotRows)
        {
            if (row.slots[0].sprite == row.slots[1].sprite && row.slots[1].sprite == row.slots[2].sprite)
            {
                winnings += currentBet * 5; // Полное совпадение = 5x ставка
            }
            else if (row.slots[0].sprite == row.slots[1].sprite || row.slots[1].sprite == row.slots[2].sprite)
            {
                winnings += currentBet * 2; // 2 одинаковых = 2x ставка
            }
        }

        // Если есть выигрыш, добавляем деньги
        if (winnings > 0)
        {
            money += winnings;
            winPanel.SetActive(true);
            winText.text = winnings.ToString();
        }
        Debug.Log(winnings+"||Money:"+money);

        PlayerPrefs.SetInt("Money", money);
        PlayerPrefs.Save();
        UpdateUI();
    }
}
