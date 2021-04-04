using UnityEngine;

public class UpdateManger : MonoBehaviour
{
    // Resets the scene when the figure is updated
    // called by pressing Update Figure Button
    // Destroys all objects created for the current figure (children of the figure containers specified in Destruction)
    // Inactivates UI elements of the current figure (in ToInactivate)
    public Transform[] Destruction;
    public GameObject[] ToInactivate;

    public void UpdateButtonPressed()
    {
        for (int i = 0; i < Destruction.Length; i++)
        {
            DestroyAllChildren(Destruction[i]); 
        }

        for (int i = 0; i < ToInactivate.Length; i++)
        {
            ToInactivate[i].SetActive(false); 
        }
    }
    public void DestroyAllChildren(Transform doomed)
    {
        foreach (Transform child in doomed)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
}
