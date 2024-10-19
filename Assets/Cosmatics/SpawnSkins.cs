using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSkins : MonoBehaviour
{
    public Part part = new Part();


    public ShipCosmaticData shipCosmatic;
    public CanonSkins CannonCosmatic;
    public SailCosmaticData sailCosmatic;
    public HelmCosmaticData helmCosmatic;
    public FlagCosmaticData flagCosmatic;
    public AnchorCosmaticData anchorCosmatic;

    public GameObject SkinPrefab;
    
    void Start()
    {
        if (part == Part.ship)
        {
            for (int i = 0; i < shipCosmatic.Get_Lenght; i++)
            {
                SpawnStuff(Set_Skin_Buy.Part.ship, i);
            }
        }
        else if (part == Part.sail)
        {
            for (int i = 0; i < sailCosmatic.Get_Lenght; i++)
            {
                SpawnStuff(Set_Skin_Buy.Part.sail, i);
            }
        }
        else if (part == Part.flag)
        {
            for (int i = 0; i < flagCosmatic.Get_Lenght; i++)
            {
                SpawnStuff(Set_Skin_Buy.Part.flag, i);
            }
        }
        else if (part == Part.helm)
        {            
            for (int i = 0; i < helmCosmatic.Get_Lenght; i++)
            {
                SpawnStuff(Set_Skin_Buy.Part.helm, i);
            }
        }
        else if (part == Part.cannon)
        {
            for (int i = 0; i < CannonCosmatic.GetLength; i++)
            {
                SpawnStuff(Set_Skin_Buy.Part.cannon, i);
            }                
        }
        else if (part == Part.anchor)
        {
            for (int i = 0; i < anchorCosmatic.Get_Lenght; i++)
            {
                SpawnStuff(Set_Skin_Buy.Part.anchor, i);
            }
            
        }
    }
    
    private void SpawnStuff(Set_Skin_Buy.Part part, int i)
    {        
        GameObject ob = Instantiate(SkinPrefab, transform);
        Set_Skin_Buy skin = ob.GetComponent<Set_Skin_Buy>();
        skin.part = part;
        skin.index = i;
    }

    public enum Part
    {
        ship,
        sail,
        flag,
        helm,
        cannon,
        anchor
    }
}
