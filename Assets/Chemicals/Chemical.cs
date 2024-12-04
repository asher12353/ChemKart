using UnityEngine;

public class Chemical : MonoBehaviour
{
    public string chemicalValue = "";
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("You picked up " + chemicalValue + " type chemical!");
        ChemicalCrafter crafter = other.gameObject.transform.parent.GetComponent<ChemicalCrafter>();
        if(crafter != null && !crafter.IsFull())
        {
            crafter.addChemical(chemicalValue);
            Destroy(gameObject);
        }
    }
}
