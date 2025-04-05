using UnityEngine;

public class ShotgunShell : MonoBehaviour
{
    public Rigidbody OwnRigidBody;

    public void Init()
    {
        transform.SetParent(null);
        transform.localScale = Vector3.one;
        OwnRigidBody.isKinematic = false;
        OwnRigidBody.AddForce(transform.up * 200f, ForceMode.Impulse);
        Destroy(gameObject, 5);
    }
}
