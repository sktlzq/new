using UnityEngine;

[RequireComponent(typeof(MeshRenderer))] // ������������� ������� MeshRenderer, ���� ��� ���
public class BoardPlaneColorChanger : MonoBehaviour
{
    [Header("���������")]
    public Color touchColor = Color.red;       // ���� ��� �������
    public float brushSize = 0.001f;            // ������ "�����" (� ������� ��������)      

    private Texture2D _texture;               // ������������ ��������
    private Color[] _originalPixels;          // �������� ������� ��� ������
    MeshRenderer _meshRenderer;

    void Start()
    {
        // �������� ��������� ���������
        _meshRenderer = GetComponent<MeshRenderer>();

        // ������� ����� �������� ��� ���������
        InitDynamicTexture();
    }

    void Update()
    {

    }

    // ��������� ����� ��� ������������
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

    // ������������� ������������ ��������
    private void InitDynamicTexture()
    {
        // �������� �������� �������� �� ���������
        Texture2D originalTexture = _meshRenderer.material.mainTexture as Texture2D;

        // ������� ����� �������� � ���� �� �����������
        _texture = new Texture2D(
            originalTexture.width,
            originalTexture.height,
            originalTexture.format,
            false  // ��������� ������� ��� ������������������
        );

        // �������� �������
        _originalPixels = originalTexture.GetPixels();
        _texture.SetPixels(_originalPixels);
        _texture.Apply();

        // ��������� ����� �������� � ���������
        _meshRenderer.material.mainTexture = _texture;
    }

    // ������ � ��������� ������� (������� ����������)
    private void PaintAtPosition(Vector3 worldPosition)
    {
        // ����������� ������� ���������� � UV-���������� ��������
        Vector3 localPos = transform.InverseTransformPoint(worldPosition);
        Vector2 uv = new Vector2(
            Mathf.Clamp01(localPos.x + 0.5f),  // Plane ����� ������ 1x1, ������ ����������
            Mathf.Clamp01(localPos.z + 0.5f)   // ��� Plane ������������ XZ-���������
        );

        // ������������ ������ ����� � ��������
        int pixelRadius = Mathf.RoundToInt(brushSize * _texture.width);
        int centerX = Mathf.RoundToInt(uv.x * _texture.width);
        int centerY = Mathf.RoundToInt(uv.y * _texture.height);

        // �������� ������� � ������� �����
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
                        // ������� �������� ������ (�����������)
                        Color currentColor = _texture.GetPixel(x, y);
                        _texture.SetPixel(x, y, Color.Lerp(currentColor, touchColor, 1f - distance));
                    }
                }
            }
        }

        _texture.Apply();
    }

    // ����� �������� � ��������� ���������
    public void ResetTexture()
    {
        if (_texture != null && _originalPixels != null)
        {
            _texture.SetPixels(_originalPixels);
            _texture.Apply();
            Debug.Log("�������� ��������!");
        }
    }
    void Activate()
    {
        ResetTexture();
    }
}