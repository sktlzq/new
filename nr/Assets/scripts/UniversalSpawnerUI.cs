using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[RequireComponent(typeof(ScrollRect))]
public class UniversalSpawnerUI : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("Main UI panel containing the scroll view")]
    public GameObject uiPanel;
    [Tooltip("Button template for spawnable objects")]
    public GameObject buttonPrefab;
    [Tooltip("Where to spawn objects in world space")]
    public Transform spawnPoint;

    [Header("Layout Settings")]
    [Range(1, 8)] public int buttonsPerRow = 4;
    public Vector2 buttonSize = new Vector2(160, 40);
    public float spacing = 10f;
    public RectOffset padding = new RectOffset(5, 5, 5, 5);

    [Header("Control Settings")]
    public KeyCode toggleKey = KeyCode.Tab;
    public bool unlockCursorOnOpen = true;

    [Header("Spawnable Objects")]
    public List<GameObject> spawnablePrefabs;

    [SerializeField] ScrollRect scrollRect;
    private RectTransform content;
    private bool isUIOpen;

    private void Start()
    {
        scrollRect = GetComponent<ScrollRect>();
        content = scrollRect.content;

        InitializeUI();
        CloseUI();
    }

    private void InitializeUI()
    {
        // Ensure we have all required components
        if (content.GetComponent<GridLayoutGroup>() == null)
        {
            var grid = content.gameObject.AddComponent<GridLayoutGroup>();
            grid.cellSize = buttonSize;
            grid.spacing = new Vector2(spacing, spacing);
            grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            grid.constraintCount = buttonsPerRow;
            grid.padding = padding;
        }

        if (content.GetComponent<ContentSizeFitter>() == null)
        {
            var fitter = content.gameObject.AddComponent<ContentSizeFitter>();
            fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        }

        // Create all buttons
        foreach (var prefab in spawnablePrefabs)
        {
            CreateSpawnButton(prefab);
        }
    }

    private void CreateSpawnButton(GameObject prefab)
    {
        var buttonObj = Instantiate(buttonPrefab, content);
        var button = buttonObj.GetComponent<Button>();

        // Set button text
        if (buttonObj.TryGetComponent<Text>(out var text))
        {
            text.text = string.IsNullOrEmpty(prefab.name)
                ? "Unnamed Object"
                : prefab.name;
        }

        // Set button action
        button.onClick.AddListener(() => SpawnObject(prefab));
    }

    private void SpawnObject(GameObject prefab)
    {
        Instantiate(prefab, spawnPoint.position, Quaternion.identity);
        CloseUI();
    }

    private void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            if (isUIOpen) CloseUI();
            else OpenUI();
        }
    }

    private void OpenUI()
    {
        isUIOpen = true;
        uiPanel.SetActive(true);

        if (unlockCursorOnOpen)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        // Scroll to top
        scrollRect.verticalNormalizedPosition = 1;
    }

    private void CloseUI()
    {
        isUIOpen = false;
        uiPanel.SetActive(false);

        if (unlockCursorOnOpen)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}