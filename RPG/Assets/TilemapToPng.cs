using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using System.IO;

#if UNITY_EDITOR //This will run this code in the unity editor: https://docs.unity3d.com/Manual/PlatformDependentCompilation.html
using UnityEditor;

[CustomEditor(typeof(TilemapToPng))]
public class TilemapToPngEditor : Editor
{
    string fileName = "";

    public override void OnInspectorGUI ()
    {
        TilemapToPng GTM = (TilemapToPng)target;


        //DrawDefaultInspector();

        if (GTM.ReadyImage == null)
        {
            if (GUILayout.Button("Pack as PNG"))
            {
                GTM.Pack();
            }
        }
        else
        {
            GUILayout.Label("File name");
            fileName = GUILayout.TextField(fileName);
            if(fileName.Length > 0)
            {
                if (GUILayout.Button("Export file"))
                {
                    GTM.ExportPNG(fileName);
                }
            }
            
        }
            
        
    }

}
#endif



    public class TilemapToPng : MonoBehaviour
{

    Tilemap tm;

    int minX = 0;
    int maxX = 0;
    int minY = 0;
    int maxY = 0;
    
    public Texture2D ReadyImage;

    public void Pack()
    {
        tm = GetComponent<Tilemap>();
        Sprite sprite = null;


        for (int x = 0; x < tm.size.x; x++) //we find the smallest and largest point
        {
            for (int y = 0; y < tm.size.y; y++)
            {
                Vector3Int pos = new Vector3Int(-x, -y, 0);
                if (tm.GetSprite(pos) == null)
                {
                    print("there is no sprite in this position");
                }
                else
                {
                    sprite = tm.GetSprite(pos); //We select any sprite to later know the dimensions of the sprites
                    if (minX > pos.x)
                    {
                        minX = pos.x;
                    }
                    if (minY > pos.y)
                    {
                        minY = pos.y;
                    }
                }

                pos = new Vector3Int(x, y, 0);
                if (tm.GetSprite(pos) == null)
                {
                    print("there is no sprite in this position");
                }
                else
                {
                    if (maxX < pos.x)
                    {
                        maxX = pos.x;
                    }
                    if (maxY < pos.y)
                    {
                        maxY = pos.y;
                    }
                }
            }
        }


        //We find the size of the sprite in pixels
        float width = sprite.rect.width;
        float height = sprite.rect.height;


        //we create a texture with the size multiplied by the number of cells
        Texture2D Image = new Texture2D((int)width * tm.size.x, (int)height * tm.size.y);

        //We assign the entire invisible image
        Color[] invisible = new Color[Image.width * Image.height];
        for (int i = 0; i < invisible.Length; i++)
        {
            invisible[i] = new Color(0f, 0f, 0f, 0f);
        }
        Image.SetPixels(0,0,Image.width, Image.height, invisible);
        

        //Now we assign each block its respective pixels
        for (int x = minX; x <= maxX; x++)
        {
            for(int y = minY; y <= maxY; y++)
            {
                if (tm.GetSprite(new Vector3Int(x, y, 0)) == null)
                {
                    print("Bloque vacio");
                }
                else
                {
                    //We map the pixels so that the minX = 0 and minY = 0
                    Image.SetPixels((x - minX) * (int)width, (y - minY) * (int)height, (int)width, (int)height, GetCurrentSprite(tm.GetSprite(new Vector3Int(x, y, 0))).GetPixels()   );
                }
            }
        }
        Image.Apply();

        ReadyImage = Image; //We store the texture of the ready image
    }

    Texture2D GetCurrentSprite(Sprite sprite) //method to obtain the trimmed sprite as we put it
    {
        var pixels = sprite.texture.GetPixels((int)sprite.textureRect.x,
                                         (int)sprite.textureRect.y,
                                         (int)sprite.textureRect.width,
                                         (int)sprite.textureRect.height);

        Texture2D textura = new Texture2D((int)sprite.textureRect.width,
                                         (int)sprite.textureRect.height);

        textura.SetPixels(pixels);
        textura.Apply();
        return textura;
    }

     public void ExportPNG (string fileName) //method that exports as png
     {
         byte[] bytes = ReadyImage.EncodeToPNG();
         var dirPath = Application.dataPath + "/Exported Tilemaps/";
         if (!Directory.Exists(dirPath))
         {
             Directory.CreateDirectory(dirPath);
         }
         File.WriteAllBytes(dirPath + fileName + ".png", bytes);
        ReadyImage = null;
     }

}
