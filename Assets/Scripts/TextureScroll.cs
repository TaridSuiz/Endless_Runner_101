using UnityEngine;

public class TextureScroll : MonoBehaviour
{
    public float speedScroll;
    public bool isScrolling = true;
    Material bgMaterial;

    private void Awake()
    {
        bgMaterial = GetComponent<Renderer>().material;
    }

    private void Update()
    {
        if (isScrolling)
        {
            Vector2 offset = new Vector2(Time.time * speedScroll, 0);
            bgMaterial.mainTextureOffset = offset;
        }
    }
}
