using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Set_Skin_Buy : MonoBehaviour
{   
    public int index;
    public Part part = new Part();

    
    public ShipCosmaticData shipCosmatic;    
    public CanonSkins CannonCosmatic;    
    public SailCosmaticData sailCosmatic;    
    public HelmCosmaticData helmCosmatic;    
    public FlagCosmaticData flagCosmatic;    
    public AnchorCosmaticData anchorCosmatic;

    
    public TMP_Text Coins;    
    public TMP_Text Diammond;
    
    public GameObject BuyButton;

    public GameObject CostObject;
    public GameObject AdObject;

    public Image ShipImage;
    public Image SailImage;
    public Image FlagImage;
    public Image HelmImage;
    public Image AnchorImage;
    public Image CannonImage;

    private void Start()
    {        
        if(part == Part.ship)
        {
            ShipCosmatic shipSkin = shipCosmatic.Get_Skin(index);
            if (Shop.Instance.skins.Ships_Skins.Contains(index))
            {
                BuyButton.GetComponent<Button>().onClick.RemoveAllListeners();
                BuyButton.GetComponent<Button>().onClick.AddListener(() => SelectSkinShip(index));
                BuyButton.transform.GetChild(0).gameObject.SetActive(true);
                if (GameManager.Instance.player_1._selectedShip == index)
                {
                    BuyButton.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = "Selected";
                    BuyButton.GetComponent<Button>().interactable = false;
                }
                else
                {
                    BuyButton.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = "Select";
                    BuyButton.GetComponent<Button>().interactable = true;
                }
                BuyButton.transform.GetChild(1).gameObject.SetActive(false);
            }
            else
            {
                if (shipSkin.Ad_Skin)
                {
                    AdObject.SetActive(true);
                    CostObject.SetActive(false);                    
                }
                else
                {
                    BuyButton.GetComponent<Button>().onClick.AddListener(() => BuySkin());
                    Coins.text = shipSkin.cost.Coins.ToString();
                    Diammond.text = shipSkin.cost.Diamond.ToString();
                }
            }
            ShipImage.gameObject.SetActive(true);
            ShipImage.sprite = shipSkin.Cover;
            UI_Animator _animator = ShipImage.gameObject.GetComponent<UI_Animator>();
            _animator.sprites = shipSkin.spriteSheet;            
        }
        else if(part == Part.sail)
        {
            Cosmatic sailSkin = sailCosmatic.Get_Skin(index);
            if (Shop.Instance.skins.Sail_Skins.Contains(index))
            {
                BuyButton.GetComponent<Button>().onClick.RemoveAllListeners();
                BuyButton.GetComponent<Button>().onClick.AddListener(() => SelectSkinSail(index));
                BuyButton.transform.GetChild(0).gameObject.SetActive(true);
                if (GameManager.Instance.player_1._selectedSail == index)
                {
                    BuyButton.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = "Selected";
                    BuyButton.GetComponent<Button>().interactable = false;
                }
                else
                {
                    BuyButton.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = "Select";
                    BuyButton.GetComponent<Button>().interactable = true;
                }
                BuyButton.transform.GetChild(1).gameObject.SetActive(false);
            }
            else
            {
                if (sailSkin.Ad_Skin)
                {
                    AdObject.SetActive(true);
                    CostObject.SetActive(false);                    
                }
                else
                {
                    BuyButton.GetComponent<Button>().onClick.AddListener(() => BuySkin());
                    Coins.text = sailSkin.cost.Coins.ToString();
                    Diammond.text = sailSkin.cost.Diamond.ToString();
                }
            }
            SailImage.gameObject.SetActive(true);
            SailImage.sprite = sailSkin.Cover;
            UI_Animator _animator = SailImage.gameObject.GetComponent<UI_Animator>();
            _animator.sprites = sailSkin.spriteSheet;            
        }
        else if (part == Part.flag)
        {
            Cosmatic flagSkin = flagCosmatic.Get_Skin(index);
            if (Shop.Instance.skins.Flag_Skins.Contains(index))
            {
                BuyButton.GetComponent<Button>().onClick.RemoveAllListeners();
                BuyButton.GetComponent<Button>().onClick.AddListener(() => SelectSkinFlag(index));
                BuyButton.transform.GetChild(0).gameObject.SetActive(true);
                if (GameManager.Instance.player_1._selectedFlag == index)
                {
                    BuyButton.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = "Selected";
                    BuyButton.GetComponent<Button>().interactable = false;
                }
                else
                {
                    BuyButton.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = "Select";
                    BuyButton.GetComponent<Button>().interactable = true;
                }
                BuyButton.transform.GetChild(1).gameObject.SetActive(false);
            }
            else
            {
                if (flagSkin.Ad_Skin)
                {
                    AdObject.SetActive(true);
                    CostObject.SetActive(false);                    
                }
                else
                {
                    BuyButton.GetComponent<Button>().onClick.AddListener(() => BuySkin());
                    Coins.text = flagSkin.cost.Coins.ToString();
                    Diammond.text = flagSkin.cost.Diamond.ToString();
                }
            }
            FlagImage.transform.parent.gameObject.SetActive(true);
            FlagImage.sprite = flagSkin.Cover;
            UI_Animator _animator = FlagImage.gameObject.GetComponent<UI_Animator>();
            _animator.sprites = flagSkin.spriteSheet;            
        }
        else if (part == Part.helm)
        {
            Cosmatic helmSkin = helmCosmatic.Get_Skin(index);
            if (Shop.Instance.skins.Helm_Skins.Contains(index))
            {
                BuyButton.GetComponent<Button>().onClick.RemoveAllListeners();
                BuyButton.GetComponent<Button>().onClick.AddListener(() => SelectSkinHelm(index));
                BuyButton.transform.GetChild(0).gameObject.SetActive(true);
                if (GameManager.Instance.player_1._selectedHelm == index)
                {
                    BuyButton.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = "Selected";
                    BuyButton.GetComponent<Button>().interactable = false;
                }
                else
                {
                    BuyButton.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = "Select";
                    BuyButton.GetComponent<Button>().interactable = true;
                }
                BuyButton.transform.GetChild(1).gameObject.SetActive(false);
            }
            else
            {
                if (helmSkin.Ad_Skin)
                {
                    AdObject.SetActive(true);
                    CostObject.SetActive(false);                    
                }
                else
                {
                    BuyButton.GetComponent<Button>().onClick.AddListener(() => BuySkin());
                    Coins.text = helmSkin.cost.Coins.ToString();
                    Diammond.text = helmSkin.cost.Diamond.ToString();
                }
            }
            HelmImage.gameObject.SetActive(true);
            HelmImage.sprite = helmSkin.Cover;            
        }
        else if (part == Part.cannon)
        {
            CanonCosmaticData cannonSkin = CannonCosmatic.Get_Skin(index);
            if (Shop.Instance.skins.Cannon_Skins.Contains(index))
            {
                BuyButton.GetComponent<Button>().onClick.RemoveAllListeners();
                BuyButton.GetComponent<Button>().onClick.AddListener(() => SelectSkinCannon(index));
                BuyButton.transform.GetChild(0).gameObject.SetActive(true);
                if (GameManager.Instance.player_1._selectedCannon == index)
                {
                    BuyButton.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = "Selected";
                    BuyButton.GetComponent<Button>().interactable = false;
                }
                else
                {
                    BuyButton.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = "Select";
                    BuyButton.GetComponent<Button>().interactable = true;
                }
                BuyButton.transform.GetChild(1).gameObject.SetActive(false);
            }
            else
            {
                if (cannonSkin.Ad_Skin)
                {
                    AdObject.SetActive(true);
                    CostObject.SetActive(false);                    
                }
                else
                {
                    BuyButton.GetComponent<Button>().onClick.AddListener(() => BuySkin());
                    Coins.text = cannonSkin.cost.Coins.ToString();
                    Diammond.text = cannonSkin.cost.Diamond.ToString();
                }
            }
            CannonImage.gameObject.SetActive(true);
            CannonImage.sprite = cannonSkin.Cover;            
        }
        else if(part == Part.anchor)
        {
            AnchorCosmatic anchorSkin = anchorCosmatic.Get_Skin(index);            

            if (Shop.Instance.skins.Anchors_Skins.Contains(index))
            {
                BuyButton.GetComponent<Button>().onClick.RemoveAllListeners();
                BuyButton.GetComponent<Button>().onClick.AddListener(() => SelectSkinAnchor(index));
                BuyButton.transform.GetChild(0).gameObject.SetActive(true);
                if(GameManager.Instance.player_1._selectedAnchor == index)
                {
                    BuyButton.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = "Selected";
                    BuyButton.GetComponent<Button>().interactable = false;
                }
                else
                {
                    BuyButton.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = "Select";
                    BuyButton.GetComponent<Button>().interactable = true;
                }                
                BuyButton.transform.GetChild(1).gameObject.SetActive(false);
            }
            else
            {
                if (anchorSkin.Ad_Skin)
                {
                    AdObject.SetActive(true);
                    CostObject.SetActive(false);                    
                }
                else
                {
                    BuyButton.GetComponent<Button>().onClick.AddListener(() => BuySkin());
                    Coins.text = anchorSkin.cost.Coins.ToString();
                    Diammond.text = anchorSkin.cost.Diamond.ToString();
                }                
            }
            AnchorImage.gameObject.SetActive(true);
            AnchorImage.sprite = anchorSkin.Bottom;            
        }
    }

    private void BuySkin()
    {
        if (part == Part.ship)
        {
            bool bought = Shop.Instance.BuyShip_Skin(index);
            if (bought)
            {
                BuyButton.transform.GetChild(0).gameObject.SetActive(true);
                SelectSkinShip(index);
                BuyButton.transform.GetChild(1).gameObject.SetActive(false);
                BuyButton.GetComponent<Button>().onClick.RemoveAllListeners();
                BuyButton.GetComponent<Button>().onClick.AddListener(() => SelectSkinShip(index));
            }            
        }
        else if (part == Part.sail)
        {
            bool bought = Shop.Instance.BuySail_Skin(index);
            if (bought)
            {
                BuyButton.transform.GetChild(0).gameObject.SetActive(true);
                SelectSkinSail(index);
                BuyButton.transform.GetChild(1).gameObject.SetActive(false);
                BuyButton.GetComponent<Button>().onClick.RemoveAllListeners();
                BuyButton.GetComponent<Button>().onClick.AddListener(() => SelectSkinSail(index));
            }            
        }
        else if (part == Part.flag)
        {
            bool bought = Shop.Instance.BuyFlag_Skin(index);
            if (bought)
            {
                BuyButton.transform.GetChild(0).gameObject.SetActive(true);
                SelectSkinFlag(index);
                BuyButton.transform.GetChild(1).gameObject.SetActive(false);
                BuyButton.GetComponent<Button>().onClick.RemoveAllListeners();
                BuyButton.GetComponent<Button>().onClick.AddListener(() => SelectSkinFlag(index));
            }            
        }
        else if (part == Part.helm)
        {
            bool bought = Shop.Instance.BuyHelm_Skin(index);
            if (bought)
            {
                BuyButton.transform.GetChild(0).gameObject.SetActive(true);
                SelectSkinHelm(index);
                BuyButton.transform.GetChild(1).gameObject.SetActive(false);
                BuyButton.GetComponent<Button>().onClick.RemoveAllListeners();
                BuyButton.GetComponent<Button>().onClick.AddListener(() => SelectSkinHelm(index));
            }            
        }
        else if (part == Part.cannon)
        {
            bool bought = Shop.Instance.BuyCannon_Skin(index);
            if (bought)
            {
                BuyButton.transform.GetChild(0).gameObject.SetActive(true);
                SelectSkinCannon(index);
                BuyButton.transform.GetChild(1).gameObject.SetActive(false);
                BuyButton.GetComponent<Button>().onClick.RemoveAllListeners();
                BuyButton.GetComponent<Button>().onClick.AddListener(() => SelectSkinCannon(index));
            }            
        }
        else if (part == Part.anchor)
        {            
            bool bougth = Shop.Instance.BuyAnchor_Skin(index);
            if (bougth)
            {
                BuyButton.transform.GetChild(0).gameObject.SetActive(true);
                SelectSkinAnchor(index);
                BuyButton.transform.GetChild(1).gameObject.SetActive(false);
                BuyButton.GetComponent<Button>().onClick.RemoveAllListeners();
                BuyButton.GetComponent<Button>().onClick.AddListener(() => SelectSkinAnchor(index));
            }            
        }
    }
    
    private void GetSkinFromAd(List<int> skinList, string key)
    {
        Shop.Instance.skins.Add_Skin(skinList, index);
        GameManager.Instance.SaveData(key, skinList);
        BuyButton.transform.GetChild(0).gameObject.SetActive(true);        
        BuyButton.transform.GetChild(1).gameObject.SetActive(false);
        BuyButton.GetComponent<Button>().onClick.RemoveAllListeners();        
        AdObject.SetActive(false);
    }
    
    private void Update()
    {
        if (UI_Controller.instance.Buy_Skins.gameObject.activeInHierarchy)
        {
            if (part == Part.ship)
            {
                CheckForSelected("select_skin_ship");                
            }
            else if (part == Part.sail)
            {
                CheckForSelected("select_skin_sail");                
            }
            else if (part == Part.flag)
            {
                CheckForSelected("select_skin_flag");                
            }
            else if (part == Part.helm)
            {
                CheckForSelected("select_skin_helm");
            }
            else if (part == Part.cannon)
            {
                CheckForSelected("select_skin_cannon");                
            }
            else if (part == Part.anchor)
            {
                CheckForSelected("select_skin_anchor");
            }            
        }        
    }


    private void CheckForSelected(string key)
    {
        if (index == PlayerPrefs.GetInt(key))
        {
            BuyButton.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = "Selected";
            BuyButton.GetComponent<Button>().interactable = false;
        }
        else
        {
            BuyButton.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = "Select";
            BuyButton.GetComponent<Button>().interactable = true;
        }
    }

    private void SelectSkinAnchor(int index)
    {
        GameManager.Instance.player_1._selectedAnchor = index;
        GameManager.Instance.SaveData("select_skin_anchor", index);
        BuyButton.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = "Selected";
        BuyButton.GetComponent<Button>().interactable = false;
        GameManager.Instance.GetCoverSkin();
    }
    private void SelectSkinSail(int index)
    {
        GameManager.Instance.player_1._selectedSail = index;
        GameManager.Instance.SaveData("select_skin_sail", index);
        BuyButton.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = "Selected";
        BuyButton.GetComponent<Button>().interactable = false;
        GameManager.Instance.GetCoverSkin();
    }
    private void SelectSkinFlag(int index)
    {
        GameManager.Instance.player_1._selectedFlag = index;
        GameManager.Instance.SaveData("select_skin_flag", index);
        BuyButton.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = "Selected";
        BuyButton.GetComponent<Button>().interactable = false;
        GameManager.Instance.GetCoverSkin();
    }
    private void SelectSkinCannon(int index)
    {
        GameManager.Instance.player_1._selectedCannon = index;
        GameManager.Instance.SaveData("select_skin_cannon", index);
        BuyButton.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = "Selected";
        BuyButton.GetComponent<Button>().interactable = false;
        GameManager.Instance.GetCoverSkin();
    }
    private void SelectSkinHelm(int index)
    {
        GameManager.Instance.player_1._selectedHelm = index;
        GameManager.Instance.SaveData("select_skin_helm", index);
        BuyButton.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = "Selected";
        BuyButton.GetComponent<Button>().interactable = false;
        GameManager.Instance.GetCoverSkin();
    }
    private void SelectSkinShip(int index)
    {
        GameManager.Instance.player_1._selectedShip = index;
        GameManager.Instance.SaveData("select_skin_ship", index);
        BuyButton.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = "Selected";
        BuyButton.GetComponent<Button>().interactable = false;
        GameManager.Instance.GetCoverSkin();
    }

    public void Show_Skin()
    {
        if (part == Part.ship)
        {
            ShipCosmatic shipSkin = shipCosmatic.Get_Skin(index);
            UI_Controller.instance.Show_Skin_Controller(1);
            GameObject show = UI_Controller.instance.Show_Skin.gameObject;

            Image ship = show.transform.Find("BG/ship").GetComponent<Image>();

            ship.sprite = shipSkin.Cover;
            UI_Animator _animator = ship.gameObject.GetComponent<UI_Animator>();
            _animator.sprites = shipSkin.spriteSheet;
            _animator.Func_PlayUIAnim();
        }
        else if (part == Part.sail)
        {
            Cosmatic sailSkin = sailCosmatic.Get_Skin(index);            
            UI_Controller.instance.Show_Skin_Controller(1);
            GameObject show = UI_Controller.instance.Show_Skin.gameObject;

            Image sail = show.transform.Find("BG/sail").GetComponent<Image>();

            sail.sprite = sailSkin.Cover;
            UI_Animator _animator = sail.gameObject.GetComponent<UI_Animator>();
            _animator.sprites = sailSkin.spriteSheet;
            _animator.Func_PlayUIAnim();
        }
        else if (part == Part.flag)
        {
            Cosmatic flagSkin = flagCosmatic.Get_Skin(index);
            UI_Controller.instance.Show_Skin_Controller(1);

            GameObject show = UI_Controller.instance.Show_Skin.gameObject;

            Image flag = show.transform.Find("BG/flag").GetComponent<Image>();

            flag.sprite = flagSkin.Cover;
            UI_Animator _animator = flag.gameObject.GetComponent<UI_Animator>();
            _animator.sprites = flagSkin.spriteSheet;
            _animator.Func_PlayUIAnim();
        }
        else if (part == Part.helm)
        {
            Cosmatic helmSkin = helmCosmatic.Get_Skin(index);
            UI_Controller.instance.Show_Skin_Controller(1);
            GameObject show = UI_Controller.instance.Show_Skin.gameObject;

            Image helm = show.transform.Find("BG/helm").GetComponent<Image>();

            helm.sprite = helmSkin.Cover;
            
        }
        else if (part == Part.cannon)
        {
            CanonCosmaticData cannonSkin = CannonCosmatic.Get_Skin(index);
            UI_Controller.instance.Show_Skin_Controller(1);
            GameObject show = UI_Controller.instance.Show_Skin.gameObject;

            Image cannon_Top = show.transform.Find("BG/cannon/top").GetComponent<Image>();
            Image cannon_stand = show.transform.Find("BG/cannon/stand").GetComponent<Image>();

            cannon_Top.sprite = cannonSkin.Cover;
            cannon_stand.sprite = cannonSkin.Stand;
        }
        else if (part == Part.anchor)
        {
            AnchorCosmatic anchorSkin = anchorCosmatic.Get_Skin(index);
            UI_Controller.instance.Show_Skin_Controller(1);
            GameObject show = UI_Controller.instance.Show_Skin.gameObject;

            Image anchor_Top = show.transform.Find("BG/anchor/1").GetComponent<Image>();
            Image anchor_Bottom = show.transform.Find("BG/anchor/2").GetComponent<Image>();

            anchor_Top.sprite = anchorSkin.Top;
            anchor_Bottom.sprite = anchorSkin.Bottom;
        }
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
