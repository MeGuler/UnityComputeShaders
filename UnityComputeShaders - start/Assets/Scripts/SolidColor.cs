using UnityEngine;
using System.Collections;

public class SolidColor : MonoBehaviour
{
    public ComputeShader shader;
    public int texResolution = 256;
    public string kernelName = "SolidRed";

    private Renderer _rend;
    private RenderTexture _outputTexture;

    private int _kernelHandle;

    // Use this for initialization
    void Start()
    {
        _outputTexture = new RenderTexture(texResolution, texResolution, 0);
        _outputTexture.enableRandomWrite = true;
        _outputTexture.Create();

        _rend = GetComponent<Renderer>();
        _rend.enabled = true;

        InitShader();
    }

    private void InitShader()
    {
        _kernelHandle = shader.FindKernel(kernelName);

        shader.SetInt("texResolution", texResolution);
        shader.SetTexture(_kernelHandle, "Result", _outputTexture);

        _rend.material.SetTexture("_MainTex", _outputTexture);

        DispatchShader(texResolution / 8, texResolution / 8);
    }

    private void DispatchShader(int x, int y)
    {
        shader.Dispatch(_kernelHandle, x, y, 1);
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.U))
        {
            DispatchShader(texResolution / 8, texResolution / 8);
        }
    }
}