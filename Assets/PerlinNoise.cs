using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoise : MonoBehaviour {
    public int width = 1024;
    public int height = 1024;

    public float scale = 1000f;

    public float range = 10f;

    void Update() {
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.mainTexture = GenerateTexture();
    }

    Texture2D GenerateTexture() {
        Texture2D texture = new Texture2D(width, height);

        //Generate perlin noise map for texture
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                Color color = CalculateColor(i, j);
                texture.SetPixel(i, j, color);
            }
        }

        texture.Apply();

        return texture;
    }

    Color CalculateColor(int x, int y) {
        float xCoord = (float)(x) / width * scale;
        float yCoord = (float)(y) / height * scale;

        float sample = (Mathf.PerlinNoise(xCoord, yCoord) / range) + 0.5f - (1f / range / 2f);

        return new Color(0, sample, 0);
    }
}
