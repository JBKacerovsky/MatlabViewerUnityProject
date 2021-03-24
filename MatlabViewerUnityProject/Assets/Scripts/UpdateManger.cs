using UnityEngine;

public class UpdateManger : MonoBehaviour
{
    public Transform[] Destruction;

    public GameObject ToUpdate;

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

        ToUpdate.GetComponent<MatlabReaderScript>().UpdateMatlabFigure();
    }

    public void DestroyAllChildren(Transform doomed)
    {
        foreach (Transform child in doomed)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
}
