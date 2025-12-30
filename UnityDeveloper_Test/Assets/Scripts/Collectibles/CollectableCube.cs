using UnityEngine;

public class CollectableCube : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        Collected();
    }
    void Collected()
    {
        GameManager.instance.OnCubeCollected();
        //Vfx or something
        Destroy(gameObject);
    }
}
