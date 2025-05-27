//using UnityEngine;
//using Unity.Sentis;
//using UnityEngine.UI;
//using System.Threading.Tasks;

//public class NeuroDrawingGame : MonoBehaviour
//{
//    [Header("Settings")]
//    public int drawingResolution = 256;
//    public float spawnDistance = 3f;
//    public float minConfidence = 0.7f;

//    [Header("References")]
//    public ModelAsset classificationModel;
//    public ModelAsset generationModel;
//    public DrawinCanvas drawingCanvas;
//    public Camera mainCamera;
//    public Transform spawnArea;
//    public Text debugText;

//    private Worker classifierWorker;
//    private Worker generatorWorker;
//    private string[] objectLabels;
//    private Texture2D drawingTexture;

//    private async void Start()
//    {
//        // Загрузка модели классификации
//        Model classificationRuntimeModel = ModelLoader.Load(classificationModel);
//        classifierWorker = new Worker(classificationRuntimeModel, BackendType.GPUCompute);

//        // Загрузка модели генерации (если используется)
//        if (generationModel != null)
//        {
//            Model generationRuntimeModel = ModelLoader.Load(generationModel);
//            generatorWorker = new Worker(generationRuntimeModel, BackendType.GPUCompute);
//        }

//        // Загрузка меток объектов
//        objectLabels = Resources.Load<TextAsset>("Labels/object_labels").text.Split('\n');

//        // Инициализация текстуры для рисования
//        drawingTexture = new Texture2D(drawingResolution, drawingResolution, TextureFormat.RGBA32, false);
//    }

//    public async void ProcessDrawing()
//    {
//        // 1. Получаем рисунок с канваса
//        RenderTexture drawingRT = drawingCanvas.GetDrawing();
//        RenderTexture.active = drawingRT;
//        drawingTexture.ReadPixels(new Rect(0, 0, drawingResolution, drawingResolution), 0, 0);
//        drawingTexture.Apply();

//        // 2. Классифицируем рисунок
//        string recognizedObject = await ClassifyDrawing(drawingTexture);
//        debugText.text = $"Распознано: {recognizedObject}";

//        // 3. Генерируем 3D объект
//        await Generate3DObject(recognizedObject);
//    }

//    private async Task<string> ClassifyDrawing(Texture2D drawing)
//    {
//        // Подготовка входного тензора
//        using TensorFloat inputTensor = PrepareClassificationInput(drawing);

//        // Выполнение модели
//        classifierWorker.SetInput("input", inputTensor);
//        classifierWorker.Schedule();
//        await classifierWorker.WaitForCompletionAsync();

//        // Получение результатов
//        using TensorFloat outputTensor = new TensorFloat(new TensorShape(1, objectLabels.Length));
//        classifierWorker.CopyOutput("output", outputTensor);

//        // Анализ результатов
//        float[] probabilities = outputTensor.ToReadOnlyArray();
//        int bestClass = 0;
//        float maxProb = 0;

//        for (int i = 0; i < probabilities.Length; i++)
//        {
//            if (probabilities[i] > maxProb)
//            {
//                maxProb = probabilities[i];
//                bestClass = i;
//            }
//        }

//        return maxProb > minConfidence ? objectLabels[bestClass] : "Неизвестный объект";
//    }

//    private TensorFloat PrepareClassificationInput(Texture2D image)
//    {
//        // Масштабирование и нормализация изображения
//        Texture2D processedImage = ScaleAndNormalizeImage(image, 224, 224);

//        // Создание тензора
//        TensorFloat tensor = new TensorFloat(new TensorShape(1, 3, 224, 224));

//        // Заполнение данными
//        float[] tensorData = new float[3 * 224 * 224];
//        for (int y = 0; y < 224; y++)
//        {
//            for (int x = 0; x < 224; x++)
//            {
//                Color pixel = processedImage.GetPixel(x, y);
//                tensorData[y * 224 * 3 + x * 3 + 0] = (pixel.r - 0.485f) / 0.229f;
//                tensorData[y * 224 * 3 + x * 3 + 1] = (pixel.g - 0.456f) / 0.224f;
//                tensorData[y * 224 * 3 + x * 3 + 2] = (pixel.b - 0.406f) / 0.225f;
//            }
//        }

//        tensor.Upload(tensorData, new TensorShape(1, 3, 224, 224));
//        return tensor;
//    }

//    private async Task Generate3DObject(string objectType)
//    {
//        // Позиция для спавна перед камерой
//        Vector3 spawnPosition = mainCamera.transform.position + mainCamera.transform.forward * spawnDistance;
//        spawnPosition.y = spawnArea.position.y;

//        // Временный куб (замените на генерацию нейросетью)
//        GameObject tempObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
//        tempObject.transform.position = spawnPosition;
//        tempObject.transform.localScale = Vector3.one * 0.5f;

//        // Здесь будет код генерации через нейросеть
//        // await GenerateWithAI(objectType, spawnPosition);

//        debugText.text += $"\nСоздан объект: {objectType}";
//    }

//    private Texture2D ScaleAndNormalizeImage(Texture2D source, int width, int height)
//    {
//        RenderTexture rt = RenderTexture.GetTemporary(width, height);
//        RenderTexture.active = rt;
//        Graphics.Blit(source, rt);
//        Texture2D result = new Texture2D(width, height);
//        result.ReadPixels(new Rect(0, 0, width, height), 0, 0);
//        result.Apply();
//        RenderTexture.ReleaseTemporary(rt);
//        return result;
//    }

//    private void OnDestroy()
//    {
//        classifierWorker?.Dispose();
//        generatorWorker?.Dispose();
//    }
//}