using UnityEngine;
using UnityEngine.UI;

public class colorPicker : MonoBehaviour
{
    public SpriteRenderer cube;
    public Slider rSlider;
    public Slider gSlider;
    public Slider bSlider;
    private float rv;
    private float gv;
    private float bv;
    public void init(float r, float g, float b)
    {
        rv = r;
        gv = g;
        bv = b;
        cube.color = new Color(rv,gv,bv);
    }
    public void updateR()
    {
        rv = rSlider.value;
        cube.color = new Color(rv,gv,bv);
    }
    public void updateG()
    {
        gv = gSlider.value;
        cube.color = new Color(rv,gv,bv);
    }
    public void updateB()
    {
        bv = bSlider.value;
        cube.color = new Color(rv,gv,bv);
    }
}
