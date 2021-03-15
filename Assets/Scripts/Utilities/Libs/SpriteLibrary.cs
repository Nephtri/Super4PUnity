using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class SpriteLibrary : MonoBehaviour
{
    public List<Sprite> SpriteList;

    public static SpriteLibrary SL;

    void Awake()
    {
        //Singleton pattern
        if (SL == null) {
            SL = this;
        }
        else if (SL != this) {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this);
    }

    public void AddExternalSprite(string filePath, string spriteName)
    {
        if(File.Exists(filePath) && !SpriteList.Any(x => x.name == spriteName))
        {
            var fileData = File.ReadAllBytes(filePath);
            var spriteTexture = new Texture2D(1, 1);

            if(spriteTexture.LoadImage(fileData))
            {
                var sprite = Sprite.Create(spriteTexture, new Rect(0, 0, spriteTexture.width, spriteTexture.height), Vector2.zero, 100);
                sprite.name = spriteName;
                SpriteList.Add(sprite);
            }
        }
    }

    public void RemoveSpritesByName(IEnumerable<string> names)
    {
        SpriteList.RemoveAll(x => names.Contains(x.name));
    }

    public Sprite GetSpriteByName(string name)
    {
        return SpriteList.Find(x => x.name == name);
    }
}