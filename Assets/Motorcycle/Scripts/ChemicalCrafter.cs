using UnityEngine;
using System.Collections;

public class ChemicalCrafter : MonoBehaviour
{
    int chemicalsSize = 0;
    int maxChemicals = 3;
    string[] chemicals;
    bool isFull;

    void Start()
    {
        chemicals = new string[maxChemicals];
    }

    void Update()
    {
        
    }

    public void addChemical(string chemicalValue)
    {
        if(chemicalsSize < maxChemicals)
        {
            chemicals[chemicalsSize++] = chemicalValue;
            for(int i = 0; i < chemicalsSize; i++)
            {
                Debug.Log("Chemical:" + chemicals[i]);
            }
            isFull = chemicalsSize == maxChemicals;
        }
    }

    public bool IsFull() { return isFull; }
}
