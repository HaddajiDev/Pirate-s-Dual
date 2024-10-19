using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyedShip : MonoBehaviour
{
    [Header("Player")]
    public SpriteRenderer ShipPart_1;
    public SpriteRenderer ShipPart_2;
    public SpriteRenderer cannon;
    public SpriteRenderer stand;
    public Animator flag;
    public Animator sail;
    public SpriteRenderer anchor_top;
    public SpriteRenderer anchor_bottom;
    public SpriteRenderer helm;

    public ShipCosmaticData shipCosmatic;
    public CanonSkins CannonCosmatic;
    public SailCosmaticData sailCosmatic;
    public HelmCosmaticData helmCosmatic;
    public FlagCosmaticData flagCosmatic;
    public AnchorCosmaticData anchorCosmatic;

    Destroy_Effect fo;

    [Header("Enemy")]
    public Level_Data lvl;
 



    void Start()
    {
        fo = GetComponent<Destroy_Effect>();
        if (fo.Ship)
        {
            ShipCosmatic shipCos = shipCosmatic.Get_Skin(GameManager.Instance.player_1._selectedShip);
            ShipPart_1.sprite = shipCos.half_1;
            ShipPart_2.sprite = shipCos.half_2;

            Cosmatic sailSkin = sailCosmatic.Get_Skin(GameManager.Instance.player_1._selectedSail);
            sail.runtimeAnimatorController = sailSkin.anim;

            Cosmatic flagSkin = flagCosmatic.Get_Skin(GameManager.Instance.player_1._selectedFlag);
            flag.runtimeAnimatorController = flagSkin.anim;

            Cosmatic helmSkin = helmCosmatic.Get_Skin(GameManager.Instance.player_1._selectedHelm);
            helm.sprite = helmSkin.spriteSheet[0];

            CanonCosmaticData cannonSkin = CannonCosmatic.Get_Skin(GameManager.Instance.player_1._selectedCannon);
            cannon.sprite = cannonSkin.spriteSheet[0];
            stand.sprite = cannonSkin.Stand;

            AnchorCosmatic anchorSkin = anchorCosmatic.Get_Skin(GameManager.Instance.player_1._selectedAnchor);
            anchor_top.sprite = anchorSkin.Top;
            anchor_bottom.sprite = anchorSkin.Bottom;
        }
        else
        {
            Level level = lvl.Get_Level(GameManager.Instance.Current_Level);
            ShipCosmatic shipCos = shipCosmatic.Get_Skin(level.ship);
            ShipPart_1.sprite = shipCos.half_1;
            ShipPart_2.sprite = shipCos.half_2;

            Cosmatic sailSkin = sailCosmatic.Get_Skin(level.sail);
            sail.runtimeAnimatorController = sailSkin.anim;

            Cosmatic flagSkin = flagCosmatic.Get_Skin(level.flag);
            flag.runtimeAnimatorController = flagSkin.anim;

            Cosmatic helmSkin = helmCosmatic.Get_Skin(level.helm);
            helm.sprite = helmSkin.spriteSheet[0];

            CanonCosmaticData cannonSkin = CannonCosmatic.Get_Skin(level.cannon);
            cannon.sprite = cannonSkin.spriteSheet[0];
            stand.sprite = cannonSkin.Stand;

            AnchorCosmatic anchorSkin = anchorCosmatic.Get_Skin(level.anchor);
            anchor_top.sprite = anchorSkin.Top;
            anchor_bottom.sprite = anchorSkin.Bottom;
        }
        
    }

}
