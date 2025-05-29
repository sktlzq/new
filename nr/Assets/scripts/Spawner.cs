using UnityEngine;
using UnityEngine.UI;

public class ObjectSpawnerUI : MonoBehaviour
{

    [SerializeField] private GameObject uiPanel; // Панель с кнопками
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject bomba; // Префаб куба
    [SerializeField] private GameObject gun; // Префаб сферы
    [SerializeField] private GameObject bita; // Префаб сферы
    [SerializeField] private Transform spawnPoint; // Точка создания объектов

    void Start()
    {
        // Скрываем панель при старте
        uiPanel.SetActive(false);
        panel.SetActive(true);

        // Находим кнопки и назначаем методы
        Button[] buttons = uiPanel.GetComponentsInChildren<Button>();
        buttons[0].onClick.AddListener(SpawnBomba);    // Первая кнопка
        buttons[1].onClick.AddListener(SpawnGun);   // Вторая кнопка
        buttons[2].onClick.AddListener(SpawnBita);   // Вторая кнопка
    }

    void Update()
    {
        // Включаем/выключаем панель по нажатию Tab
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            uiPanel.SetActive(!uiPanel.activeInHierarchy);
        }
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            panel.SetActive(!panel.activeInHierarchy);
        }
    }

    void SpawnBomba()
    {
        Instantiate(bomba, spawnPoint.position, Quaternion.identity);
        uiPanel.SetActive(false); // Закрываем панель после выбора
    }

    void SpawnGun()
    {
        Instantiate(gun, spawnPoint.position, Quaternion.identity);
        uiPanel.SetActive(false);
    }
    void SpawnBita()
    {
        Instantiate(bita, spawnPoint.position, Quaternion.identity);
        uiPanel.SetActive(false);
    }
}