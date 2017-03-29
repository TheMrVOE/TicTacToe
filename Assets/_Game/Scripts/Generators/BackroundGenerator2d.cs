using UnityEngine;
using System.Collections;
using Tools.Pool;

public struct WorldSreenPosition
{
    public float up;
    public float down;
    public float left;
    public float right;
}

public class BackroundGenerator2d : MonoBehaviour {

    [SerializeField] private float generationTimeMax = 8.0f;
    [SerializeField] private float generationTimeMin = 4.0f;
    [SerializeField] private int preloadedCount = 5;
    [SerializeField] private GameObject cloneableObject;
    [SerializeField] private float Z = 15.0f;

    private WorldSreenPosition screenWorldBorders;
    private Pool<SpriteRenderer> _poolElements;

    private void Start()
    {
        _poolElements = new Pool<SpriteRenderer>("BackgroundFigures", cloneableObject);
        InitViewBorders();
        PreloadedGeneration();
        StartCoroutine(StartGenerator());
    }
    private void Update()
    {
        foreach (var item in _poolElements.GetActiveElements())
        {
            if (!item.enabled) item.gameObject.SetActive(false);
        }
    }
    private void InitViewBorders()
    {
        var upperRight = Camera.main.ViewportToWorldPoint(Vector2.one);
        var downLeft = Camera.main.ViewportToWorldPoint(Vector2.zero);
        screenWorldBorders.right = upperRight.x;
        screenWorldBorders.up = upperRight.y;
        screenWorldBorders.down = downLeft.y;
        screenWorldBorders.left = downLeft.x;
    }
    private IEnumerator StartGenerator()
    {
        while(true)
        {
            yield return new WaitForSeconds(Random.Range(generationTimeMin, generationTimeMax));
            RandomizeElement(_poolElements.GetObjectFromPool().transform);
        }
    }

    private void PreloadedGeneration()
    {
        for (int i = 0; i < preloadedCount; i++)
        {
            RandomizeElement(_poolElements.GetObjectFromPool().transform);
        }
    }
    private void RandomizeElement(Transform obj)
    {
        float X = Random.Range(screenWorldBorders.left, screenWorldBorders.right);
        float Y = Random.Range(screenWorldBorders.down, screenWorldBorders.up);

        obj.transform.position = new Vector3(X, Y, Random.Range(Z - 2, Z + 2));

        if (Random.Range(0, 2) == 0)
        {
            obj.GetComponent<Animator>().SetTrigger("PlayCross");
        }
        else
        {
            obj.GetComponent<Animator>().SetTrigger("PlayCircle");
        }
    }
}
