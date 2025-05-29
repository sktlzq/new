using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ObjectSpawnerUI : MonoBehaviour
{
    [Header("Настройки UI")]
    [SerializeField] private GameObject uiPanel;       // Основная панель
    [SerializeField] private GameObject buttonPrefab; // Префаб кнопки
    [SerializeField] private Transform buttonsParent; // Контейнер для кнопок (VerticalLayoutGroup)
    [SerializeField] private int maxButtonsPerRow = 4; // Макс. кнопок в строке

    [Header("Объекты для спавна")]
    [SerializeField] private List<GameObject> spawnableObjects; // Префабы
    [SerializeField] private Transform spawnPoint; // Точка создания объектов

    private List<GameObject> spawnedButtons = new List<GameObject>();
    private bool isUIOpen = false;

    private void Start()
    {
        uiPanel.SetActive(false);
        CreateButtons();
        LockCursor(); // Блокируем курсор при старте
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleUI();
        }
    }

    // Включение/выключение UI с управлением курсором
    private void ToggleUI()
    {
        isUIOpen = !isUIOpen;
        uiPanel.SetActive(isUIOpen);

        if (isUIOpen)
        {
            UnlockCursor(); // Разблокируем курсор
        }
        else
        {
            LockCursor(); // Блокируем курсор
        }
    }

    // Создание кнопок с автопереносом
    private void CreateButtons()
    {
        ClearOldButtons();

        GameObject currentRow = null;
        HorizontalLayoutGroup currentRowHLG = null;
        int buttonsInCurrentRow = 0;

        for (int i = 0; i < spawnableObjects.Count; i++)
        {
            if (buttonsInCurrentRow == 0 || buttonsInCurrentRow >= maxButtonsPerRow)
            {
                currentRow = CreateNewRow();
                currentRowHLG = currentRow.GetComponent<HorizontalLayoutGroup>();
                buttonsInCurrentRow = 0;
            }

            CreateButton(currentRow.transform, i);
            buttonsInCurrentRow++;
        }
    }

    private GameObject CreateNewRow()
    {
        GameObject row = new GameObject($"Row_{spawnedButtons.Count / maxButtonsPerRow}");
        row.transform.SetParent(buttonsParent, false);

        HorizontalLayoutGroup hlg = row.AddComponent<HorizontalLayoutGroup>();
        hlg.childAlignment = TextAnchor.MiddleCenter;
        hlg.spacing = 10;

        return row;
    }

    private void CreateButton(Transform parent, int index)
    {
        GameObject buttonObj = Instantiate(buttonPrefab, parent);
        Button button = buttonObj.GetComponent<Button>();

        if (buttonObj.TryGetComponent(out Text buttonText))
        {
            buttonText.text = spawnableObjects[index].name;
        }

        button.onClick.AddListener(() => OnButtonClick(index));
        spawnedButtons.Add(buttonObj);
    }

    private void OnButtonClick(int index)
    {
        if (index < 0 || index >= spawnableObjects.Count) return;

        Instantiate(spawnableObjects[index], spawnPoint.position, Quaternion.identity);
        ToggleUI(); // Закрываем UI после выбора
    }

    private void ClearOldButtons()
    {
        foreach (var button in spawnedButtons)
        {
            if (button != null) Destroy(button);
        }
        spawnedButtons.Clear();
    }

    // Блокировка/разблокировка курсора
    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}