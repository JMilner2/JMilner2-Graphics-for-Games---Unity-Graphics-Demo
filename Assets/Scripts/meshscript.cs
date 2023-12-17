using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meshscript : MonoBehaviour
{
    public float colorChangeSpeed = 0.2f;
    public int numberOfStars = 7000;
    public float sphereRadius = 50f;
    public float rotationSpeed = 10f; // Adjust the rotation speed as needed

    private Material starMaterial;
    private Color currentColor;
    private Color targetColor;

    void Start()
    {
        GenerateStarrySky();
        starMaterial = GetComponent<MeshRenderer>().material;
        currentColor = starMaterial.GetColor("_StarColor");
        targetColor = GenerateRandomColor();
    }

    Color GenerateRandomColor()
    {
        return new Color(Random.value, Random.value, Random.value, 1f);
    }

    void GenerateStarrySky()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();

        Mesh mesh = new Mesh();
        List<Vector3> vertices = new List<Vector3>();
        List<int> indices = new List<int>();
        List<Color> colors = new List<Color>();

        for (int i = 0; i < numberOfStars; i++)
        {
            float longitude = Random.Range(0f, 2f * Mathf.PI);
            float latitude = Random.Range(0f, Mathf.PI);

            float x = sphereRadius * Mathf.Sin(latitude) * Mathf.Cos(longitude);
            float y = sphereRadius * Mathf.Sin(latitude) * Mathf.Sin(longitude);
            float z = sphereRadius * Mathf.Cos(latitude);

            vertices.Add(new Vector3(x, y, z));
            indices.Add(i);

            // Calculate brightness based on the y-coordinate
            float brightness = Mathf.Clamp01(1.0f - Mathf.Abs(y / sphereRadius));

            // Generate a color with adjusted brightness
            Color starColor = currentColor;

            colors.Add(starColor);
        }

        mesh.SetVertices(vertices);
        mesh.SetIndices(indices.ToArray(), MeshTopology.Points, 0);
        mesh.SetColors(colors);

        meshFilter.mesh = mesh;

        // Ensure there is a MeshRenderer component attached to the GameObject
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer == null)
        {
            meshRenderer = gameObject.AddComponent<MeshRenderer>();
        }
    }


    void Update()
    {
        currentColor = Color.Lerp(currentColor, targetColor, colorChangeSpeed * Time.deltaTime);
        starMaterial.SetColor("_StarColor", currentColor);

        // Check if we reached the target color, generate a new target color
        if (Vector4.Distance(currentColor, targetColor) < 0.01f)
        {
            targetColor = GenerateRandomColor();
        }
        // Rotate the sphere on its x-axis
        transform.Rotate(Vector3.right, rotationSpeed * Time.deltaTime);
    }
}
