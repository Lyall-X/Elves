using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SonicBloom.Koreo;

public class SoundNoteController : LaneController
{
    public override void CheckSpawnNext()
    {
        int samplesToTarget = (int)(level.SampleRate() * moveTime);
        int currentTime = level.CurrentSampleTime();
        while (pendingEventIdx<laneEvents.Count&&laneEvents[pendingEventIdx].StartSample<currentTime+samplesToTarget)
        {
            KoreographyEvent evt = laneEvents[pendingEventIdx];
            int noteNum = evt.GetIntValue();
            SoundNote newObj = GameManager.Instance.GetObj(StringManager.soundObj).GetComponent<SoundNote>();
            newObj.Initialize(evt,noteNum,this,level);
            newObj.setEventIndex(pendingEventIdx);
            trackedNotes.Enqueue(newObj);
            pendingEventIdx++;
        }
    }
}
