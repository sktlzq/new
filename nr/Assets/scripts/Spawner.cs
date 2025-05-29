using UnityEngine;
using UnityEngine.UI;

public class ObjectSpawnerUI : MonoBehaviour
{

    [SerializeField] private GameObject uiPanel; // ������ � ��������
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject bomba; // ������ ����
    [SerializeField] private GameObject gun; // ������ �����
    [SerializeField] private GameObject bita; // ������ �����
    [SerializeField] private Transform spawnPoint; // ����� �������� ��������

    void Start()
    {
        // �������� ������ ��� ������
        uiPanel.SetActive(false);
        panel.SetActive(true);

        // ������� ������ � ��������� ������
        Button[] buttons = uiPanel.GetComponentsInChildren<Button>();
        buttons[0].onClick.AddListener(SpawnBomba);    // ������ ������
        buttons[1].onClick.AddListener(SpawnGun);   // ������ ������
        buttons[2].onClick.AddListener(SpawnBita);   // ������ ������
    }

    void Update()
    {
        // ��������/��������� ������ �� ������� Tab
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
        uiPanel.SetActive(false); // ��������� ������ ����� ������
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