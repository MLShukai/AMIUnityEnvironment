using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.SideChannels;
using System.Text;
using System;
using System.Collections.Generic;

public class TransformLogSideChannel : SideChannel
{
    public TransformLogSideChannel(string uuid)
    {
        ChannelId = new Guid(uuid);
    }

    protected override void OnMessageReceived(IncomingMessage msg)
    {

    }
    
    public void SendTransformToPython(Transform transform)
    {
        var valueList = new List<float>();
        valueList.Add((float)Time.frameCount);
        valueList.Add(Time.time);
        valueList.Add(transform.position.x);
        valueList.Add(transform.position.y);
        valueList.Add(transform.position.z);
        valueList.Add(transform.rotation.eulerAngles.x);
        valueList.Add(transform.rotation.eulerAngles.y);
        valueList.Add(transform.rotation.eulerAngles.z);

        using (var msgOut = new OutgoingMessage())
        {
            msgOut.WriteFloatList(valueList);
            QueueMessageToSend(msgOut);
        }
    }
}
