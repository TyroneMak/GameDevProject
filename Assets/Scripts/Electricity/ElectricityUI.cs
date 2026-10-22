using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Builds the energy HUD at runtime: a horizontal bar labelled "ENERGY" above the hotbar.
/// No Canvas setup needed. Adjust bottomOffset so it sits just above your hotbar.
/// </summary>
public class ElectricityUI : MonoBehaviour
{
    public ElectricitySystem electricity;

    [Header("Bar")]
    public float barWidth = 360f;
    public float barHeight = 14f;
    [Tooltip("Distance from the bottom of the screen (at 1080p). Raise this to clear your hotbar.")]
    public float bottomOffset = 130f;
    public Color fillColor = new Color(1f, 0.85f, 0.2f);
    public Color chargingColor = new Color(0.5f, 1f, 0.5f);
    public Color lowColor = new Color(1f, 0.25f, 0.2f);
    public Color frameColor = new Color(1f, 1f, 1f, 0.85f);
    public Color backgroundColor = new Color(0f, 0f, 0f, 0.45f);

    RectTransform _fill;
    Image _fillImage;
    Text _label;

    void Awake()
    {
        if (electricity == null) electricity = GetComponent<ElectricitySystem>();
        Build();
    }

    void Build()
    {
        var font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

        var canvasGo = new GameObject("ElectricityCanvas", typeof(Canvas), typeof(CanvasScaler));
        var canvas = canvasGo.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 109;
        var scaler = canvasGo.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        scaler.matchWidthOrHeight = 1f;

        var bar = NewRect("EnergyBar", canvasGo.transform);
        bar.anchorMin = bar.anchorMax = new Vector2(0.5f, 0f);
        bar.pivot = new Vector2(0.5f, 0f);
        bar.anchoredPosition = new Vector2(0f, bottomOffset);
        bar.sizeDelta = new Vector2(barWidth + 6f, barHeight + 6f);

        AddImage(bar, frameColor);
        var bg = NewRect("Background", bar);
        Stretch(bg, 2f);
        AddImage(bg, backgroundColor);

        _fill = NewRect("Fill", bg);
        _fill.anchorMin = Vector2.zero;
        _fill.anchorMax = Vector2.one;
        _fill.offsetMin = new Vector2(1f, 1f);
        _fill.offsetMax = new Vector2(-1f, -1f);
        _fillImage = AddImage(_fill, fillColor);

        _label = NewText("Label", bar, font, 18, TextAnchor.LowerCenter);
        var lr = _label.rectTransform;
        lr.anchorMin = lr.anchorMax = new Vector2(0.5f, 1f);
        lr.pivot = new Vector2(0.5f, 0f);
        lr.anchoredPosition = new Vector2(0f, 4f);
        lr.sizeDelta = new Vector2(200f, 26f);
        _label.text = "ENERGY";
    }

    void Update()
    {
        if (electricity == null) return;

        _fill.anchorMax = new Vector2(electricity.Normalized, 1f);

        Color c = electricity.IsCharging ? chargingColor : fillColor;
        if (electricity.IsLow && !electricity.IsCharging)
            c = Color.Lerp(lowColor, fillColor, 0.5f + 0.5f * Mathf.Sin(Time.time * 8f) * 0.5f);
        _fillImage.color = c;

        _label.text = electricity.IsCharging ? "ENERGY - CHARGING"
                    : !electricity.HasPower ? "ENERGY - EMPTY"
                    : "ENERGY";
        _label.color = electricity.IsLow && !electricity.IsCharging ? lowColor : Color.white;
    }

    static RectTransform NewRect(string name, Transform parent)
    {
        var go = new GameObject(name, typeof(RectTransform));
        go.transform.SetParent(parent, false);
        return (RectTransform)go.transform;
    }

    static void Stretch(RectTransform rt, float inset)
    {
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = new Vector2(inset, inset);
        rt.offsetMax = new Vector2(-inset, -inset);
    }

    static Image AddImage(RectTransform rt, Color color)
    {
        var img = rt.gameObject.AddComponent<Image>();
        img.color = color;
        img.raycastTarget = false;
        return img;
    }

    static Text NewText(string name, Transform parent, Font font, int size, TextAnchor anchor)
    {
        var rt = NewRect(name, parent);
        var t = rt.gameObject.AddComponent<Text>();
        t.font = font;
        t.fontSize = size;
        t.alignment = anchor;
        t.color = Color.white;
        t.raycastTarget = false;
        t.horizontalOverflow = HorizontalWrapMode.Overflow;
        t.verticalOverflow = VerticalWrapMode.Overflow;
        var shadow = rt.gameObject.AddComponent<Shadow>();
        shadow.effectColor = new Color(0f, 0f, 0f, 0.6f);
        return t;
    }
}
