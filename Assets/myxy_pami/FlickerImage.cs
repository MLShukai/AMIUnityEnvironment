using UnityEngine;

public class FlickerImage : MonoBehaviour
{
    [SerializeField]
    private Texture[] _textureList;
    [SerializeField]
    private MeshRenderer _renderer;
    [SerializeField, Range(0.001f, 1f)]
    private float _flickerIntervalSecond = 0.1f;
    private float _flickerIntervalTimer = 0;

    void Update()
    {
        if (_flickerIntervalTimer < 0)
        {
            var index = Random.Range(0, _textureList.Length);
            _renderer.material.SetTexture("_MainTex", _textureList[index]);
            _flickerIntervalTimer = _flickerIntervalSecond;
        }
        _flickerIntervalTimer -= Time.deltaTime;
    }
}
