using UnityEngine;

public class PushBuckets : MonoBehaviour
{

    public HangingBucket[] Buckets;

    public void PushAll()
    {
        foreach (var bucket in Buckets)
        {
            bucket.PushRandomly();
        }
    }
}
