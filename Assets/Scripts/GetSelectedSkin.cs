using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetSelectedSkin : MonoBehaviour
{
    UI_Animator anim;
    Image img;
    public Part part = new Part();
    public ShipCosmaticData shipCosmatic;
    public CanonSkins CannonCosmatic;
    public SailCosmaticData sailCosmatic;
    public HelmCosmaticData helmCosmatic;
    public FlagCosmaticData flagCosmatic;
    public AnchorCosmaticData anchorCosmatic;

    public Image Stand;

    void Start()
    {
        
        Get_Skin();
    }

    
    public void Get_Skin()
    {
        if (part == Part.ship)
        {
            ShipCosmatic shipSkin = shipCosmatic.Get_Skin(GameManager.Instance.player_1._selectedShip);
            anim = GetComponent<UI_Animator>();
            anim.sprites = shipSkin.spriteSheet;
        }
        else if (part == Part.sail)
        {
            Cosmatic sailSkin = sailCosmatic.Get_Skin(GameManager.Instance.player_1._selectedSail);
            anim = GetComponent<UI_Animator>();
            anim.sprites = sailSkin.spriteSheet;
        }
        else if (part == Part.flag)
        {
            Cosmatic flagSkin = flagCosmatic.Get_Skin(GameManager.Instance.player_1._selectedFlag);
            anim = GetComponent<UI_Animator>();            
            anim.sprites = flagSkin.spriteSheet;
        }
        else if (part == Part.helm)
        {
            Cosmatic helmSkin = helmCosmatic.Get_Skin(GameManager.Instance.player_1._selectedHelm);
            img = GetComponent<Image>();
            img.sprite = helmSkin.Cover;
        }
        else if (part == Part.cannon)
        {
            CanonCosmaticData cannonSkin = CannonCosmatic.Get_Skin(GameManager.Instance.player_1._selectedCannon);
            img = GetComponent<Image>();
            img.sprite = cannonSkin.Cover;
            Stand.sprite = cannonSkin.Stand;
        }
        else if (part == Part.anchorBottom)
        {
            AnchorCosmatic anchorSkin = anchorCosmatic.Get_Skin(GameManager.Instance.player_1._selectedAnchor);
            img = GetComponent<Image>();
            img.sprite = anchorSkin.Bottom;
        }
        else if (part == Part.anchorTop)
        {
            AnchorCosmatic anchorSkin = anchorCosmatic.Get_Skin(GameManager.Instance.player_1._selectedAnchor);
            img = GetComponent<Image>();
            img.sprite = anchorSkin.Top;
        }
    }


    public enum Part
    {
        ship,
        sail,
        flag,
        helm,
        cannon,
        anchorTop,
        anchorBottom
    }
}
