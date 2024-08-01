//using UnityEditor.XR;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public PressurePlate otherPlate;
    public GameObject key;
    public int numOnPlates = 0;
    public int numNeededOnPlate = 1;
    public bool gotKey = false;
    bool activated;
    public Material onMaterial;
    public Material offMaterial;
    MeshRenderer mr;

    private void Start()
    {
        
        mr = GetComponent<MeshRenderer>();
        mr.material = offMaterial;
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("rat"))
        {
            numOnPlates += 1;
            CheckPlates();
            mr.material = onMaterial;
        }
        
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("rat"))
        {
            numOnPlates -= 1;
            CheckPlates();
            mr.material = offMaterial;
        }
    }

    void CheckPlates()
    {
        if (otherPlate.numOnPlates == numNeededOnPlate &
            numOnPlates == numNeededOnPlate & !gotKey)
        {
            key.SetActive(true);
            gotKey = true;
        }
    }
}
