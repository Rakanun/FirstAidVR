using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class HighlightController : MonoBehaviour
{
    [Header("Highlight Settings")]
    public Color highlightColor = new Color(1f, 1f, 0.5f, 1f);
    public float highlightRadius = 1.0f;
    public float intensity = 2.0f;
    public float smoothness = 0.2f;

    [Header("Deformation Settings")]
    [Range(0f, 0.3f)] public float deformationDepth = 0.1f;
    [Range(0.1f, 5f)] public float deformationHardness = 2f;

    [Header("State")]
    public Transform highlightCenter;

    private Material _material;
    private bool _isHighlighting;

    private Color _initialColor;
    private float _initialRadius;
    private float _initialIntensity;
    private float _initialSmoothness;
    private float _initialDeformationDepth;
    private float _initialDeformationHardness;

    void Start()
    {
        _material = GetComponent<Renderer>().materials[0];
        SaveInitialStates();
        UpdateMaterialProperties();
    }

    void Update()
    {
        SetDeformation(deformationDepth, deformationHardness);
        if (_isHighlighting)
        {
            highlightColor.a = Mathf.PingPong(Time.time, 1f);
            UpdateMaterialProperties();
        }
    }

    public void StartHighlight()
    {
        _isHighlighting = true;
        highlightColor = _initialColor;
        intensity = _initialIntensity;
    }

    public void StopHighlight()
    {
        _isHighlighting = false;
        ResetAllParameters();
        UpdateMaterialProperties();
    }

    public void SetDeformation(float depth, float hardness)
    {
        deformationDepth = Mathf.Clamp(depth, 0f, 0.3f);
        deformationHardness = Mathf.Clamp(hardness, 0.1f, 5f);
        UpdateMaterialProperties();
    }

    private void SaveInitialStates()
    {
        _initialColor = highlightColor;
        _initialRadius = highlightRadius;
        _initialIntensity = intensity;
        _initialSmoothness = smoothness;
        _initialDeformationDepth = deformationDepth;
        _initialDeformationHardness = deformationHardness;
    }

    private void ResetAllParameters()
    {
        highlightColor = _initialColor;
        highlightRadius = _initialRadius;
        intensity = _initialIntensity;
        smoothness = _initialSmoothness;
        deformationDepth = _initialDeformationDepth;
        deformationHardness = _initialDeformationHardness;
    }

    private void UpdateMaterialProperties()
    {
        _material.SetVector("_HighlightCenter", highlightCenter.position);
        _material.SetColor("_HighlightColor", highlightColor);
        _material.SetFloat("_HighlightRadius", highlightRadius);
        _material.SetFloat("_ColorIntensity", intensity);   // matches shader property _ColorIntensity
        _material.SetFloat("_EdgeSmoothness", smoothness);
        _material.SetFloat("_DeformationDepth", deformationDepth);
        _material.SetFloat("_DeformationHardness", deformationHardness);
    }

    void OnDestroy()
    {
        if (_material != null)
            Destroy(_material);
    }
}
