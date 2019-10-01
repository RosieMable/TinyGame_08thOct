using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelGenerator : MonoBehaviour
{
    //reference to the texture that is going to be read to create the level
    [SerializeField] Texture2D levelTexture;
    [SerializeField] List<ColorToPrefab> colorMapping = new List<ColorToPrefab>();

    private void Awake()
    {
        GenerateLevel();
    }

    void GenerateLevel()
    {
        //loops through width and height of the texture
        for (int x = 0; x < levelTexture.width; x++)
        {
            for (int y = 0; y < levelTexture.height; y++)
            {
                ReadPixelData(x, y);
            }
        }
    }

    void ReadPixelData(int x, int y)
    {
      Color pixelColor =  levelTexture.GetPixel(x, y);

        if (levelTexture == null)
            return;


        if (colorMapping == null)
            return;
        //if the pixel is transparent, then ignore
        if (pixelColor.a == 0)
            return;

        foreach (var color in colorMapping)
        {
            if (color.pixelCol.Equals(pixelColor))
            {
                Vector2 position = new Vector2(x, y);
                GameObject go = color.selector.ChooseRandom();
                Instantiate(go, position, Quaternion.identity, transform);
            }
        }
    }


}

[Serializable]
public class ColorToPrefab
{
    public string TileName;
    public Color pixelCol;
    public PrefabSelector selector;

    public ColorToPrefab(string name, Color pixel, PrefabSelector prefab)
    {
        TileName = name;
        pixelCol = pixel;
        selector = prefab;
    }
}