using UnityEngine;

public class Parallax : MonoBehaviour
{
    [System.Serializable]
    public class ParallaxLayer
    {
        public GameObject layerObject; // The GameObject for this layer
    }

    [SerializeField] private ParallaxLayer[] layers; // Array of layers
    [SerializeField] private float baseDuration = 10f; // Duration for the first layer to cross the screen
    [SerializeField] private float width = 10f; // Width of the screen (or the distance to move)
    private Vector3[] startPositions; // Store the starting positions of each layer

    void Start()
    {
        // Initialize the starting positions for each layer
        startPositions = new Vector3[layers.Length];
        for (int i = 0; i < layers.Length; i++)
        {
            startPositions[i] = layers[i].layerObject.transform.position;
        }
    }

    void Update()
    {
        for (int i = 0; i < layers.Length; i++)
        {
            if (layers[i].layerObject == null)
            {
                Debug.LogWarning($"Layer {i} is missing its GameObject!");
                continue;
            }

            // Calculate the duration for this layer
            float duration = baseDuration / Mathf.Pow((9f - i) / 8f, 1.5f);

            // Calculate the speed for this layer
            float speed = width / duration;

            // Move the layer from left to right
            layers[i].layerObject.transform.Translate(Vector3.right * speed * Time.deltaTime);

            // Loop the layer when it goes off-screen
            if (layers[i].layerObject.transform.position.x >= startPositions[i].x + width)
            {
                layers[i].layerObject.transform.position = startPositions[i];
            }
        }
    }
}
