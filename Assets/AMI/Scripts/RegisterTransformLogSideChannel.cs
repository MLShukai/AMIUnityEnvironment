using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.SideChannels;

public class RegisterTransformLogSideChannel : MonoBehaviour
{
    [SerializeField]
    private Transform _target;
    private TransformLogSideChannel _sideChannel;

    private void Awake()
    {
        _sideChannel = new("621f0a70-4f87-11ea-a6bf-784f4387d1f7");
        SideChannelManager.RegisterSideChannel(_sideChannel);
    }

    void Update()
    {
        _sideChannel.SendTransformToPython(_target);
    }
}
