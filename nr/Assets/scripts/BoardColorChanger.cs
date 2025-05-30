using UnityEngine;

[RequireComponent(typeof(MeshRenderer))] // Автоматически добавит MeshRenderer, если его нет
public class BoardPlaneColorChanger : MonoBehaviour
{
    [Header("Настройки")]
    public Color touchColor = Color.red;       // Цвет при касании
    public float brushSize = 0.001f;            // Размер "кисти" (в мировых единицах)      

    private Texture2D _texture;               // Динамическая текстура
    private Color[] _originalPixels;          // Исходные пиксели для сброса
    MeshRenderer _meshRenderer;

    void Start()
    {
        // Получаем компонент рендерера
        _meshRenderer = GetComponent<MeshRenderer>();

        // Создаем копию текстуры для изменения
        InitDynamicTexture();
    }

    void Update()
    {

    }

    // Изменение цвета при столкновении
    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Pen"))
        {
            foreach (ContactPoint contact in collision.contacts)
            {
                PaintAtPosition(contact.point);
            }
        }
    }

    // Инициализация динамической текстуры
    private void InitDynamicTexture()
    {
        // Получаем исходную текстуру из материала
        Texture2D originalTexture = _meshRenderer.material.mainTexture as Texture2D;

        // Создаем новую текстуру с теми же параметрами
        _texture = new Texture2D(
            originalTexture.width,
            originalTexture.height,
            originalTexture.format,
            false  // Отключаем мипмапы для производительности
        );

        // Копируем пиксели
        _originalPixels = originalTexture.GetPixels();
        _texture.SetPixels(_originalPixels);
        _texture.Apply();

        // Применяем новую текстуру к материалу
        _meshRenderer.material.mainTexture = _texture;
    }

    // Рисуем в указанной позиции (мировые координаты)
    private void PaintAtPosition(Vector3 worldPosition)
    {
        // Преобразуем мировые координаты в UV-координаты текстуры
        Vector3 localPos = transform.InverseTransformPoint(worldPosition);
        Vector2 uv = new Vector2(
            Mathf.Clamp01(localPos.x + 0.5f),  // Plane имеет размер 1x1, потому центрируем
            Mathf.Clamp01(localPos.z + 0.5f)   // Для Plane используется XZ-плоскость
        );

        // Рассчитываем размер кисти в пикселях
        int pixelRadius = Mathf.RoundToInt(brushSize * _texture.width);
        int centerX = Mathf.RoundToInt(uv.x * _texture.width);
        int centerY = Mathf.RoundToInt(uv.y * _texture.height);

        // Изменяем пиксели в области кисти
        for (int x = centerX - pixelRadius; x <= centerX + pixelRadius; x++)
        {
            for (int y = centerY - pixelRadius; y <= centerY + pixelRadius; y++)
            {
                if (x >= 0 && x < _texture.width && y >= 0 && y < _texture.height)
                {
                    float distance = Vector2.Distance(
                        new Vector2(x, y),
                        new Vector2(centerX, centerY)
                    ) / pixelRadius;

                    if (distance <= 1f)
                    {
                        // Плавное смешение цветов (опционально)
                        Color currentColor = _texture.GetPixel(x, y);
                        _texture.SetPixel(x, y, Color.Lerp(currentColor, touchColor, 1f - distance));
                    }
                }
            }
        }

        _texture.Apply();
    }

    // Сброс текстуры к исходному состоянию
    public void ResetTexture()
    {
        if (_texture != null && _originalPixels != null)
        {
            _texture.SetPixels(_originalPixels);
            _texture.Apply();
            Debug.Log("Текстура сброшена!");
        }
    }
    void Activate()
    {
        ResetTexture();
    }
}