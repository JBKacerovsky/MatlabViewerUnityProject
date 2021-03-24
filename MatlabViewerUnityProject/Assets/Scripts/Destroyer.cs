using UnityEngine;

public class Destroyer : MonoBehaviour
{
    public void DestroyAllChildren()
    {
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
}
