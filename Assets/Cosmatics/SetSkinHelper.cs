//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using TMPro;

//#if UNITY_EDITOR
//using UnityEditor;

//[CustomEditor(typeof(Set_Skin_Buy))]
//public class SetSkinHelper : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        Set_Skin_Buy script = (Set_Skin_Buy)target;

//        script.part = (Set_Skin_Buy.Part)EditorGUILayout.EnumPopup("Part", script.part);
//        script.index = EditorGUILayout.IntField("index", script.index);
//        script.Coins = (TMP_Text)EditorGUILayout.ObjectField("Coins", script.Coins, typeof(TMP_Text), true);
//        script.Diammond = (TMP_Text)EditorGUILayout.ObjectField("Diammond", script.Diammond, typeof(TMP_Text), true);

//        script.image = (Image)EditorGUILayout.ObjectField("image", script.image, typeof(Image), true);
//        script.BuyButton = (GameObject)EditorGUILayout.ObjectField("Buy Button", script.BuyButton, typeof(GameObject), true);

//        if (script.part == Set_Skin_Buy.Part.ship)
//        {
//            script.shipCosmatic = (ShipCosmaticData)EditorGUILayout.ObjectField("ship", script.shipCosmatic, typeof(ShipCosmaticData), true);
//        }
//        else if (script.part == Set_Skin_Buy.Part.sail)
//        {
//            script.sailCosmatic = (SailCosmaticData)EditorGUILayout.ObjectField("sail", script.sailCosmatic, typeof(SailCosmaticData), true);
//        }
//        else if (script.part == Set_Skin_Buy.Part.helm)
//        {
//            script.helmCosmatic = (HelmCosmaticData)EditorGUILayout.ObjectField("helm", script.helmCosmatic, typeof(HelmCosmaticData), true);
//        }
//        else if (script.part == Set_Skin_Buy.Part.flag)
//        {
//            script.flagCosmatic = (FlagCosmaticData)EditorGUILayout.ObjectField("flag", script.flagCosmatic, typeof(FlagCosmaticData), true);
//        }
//        else if (script.part == Set_Skin_Buy.Part.cannon)
//        {
//            script.CannonCosmatic = (CanonSkins)EditorGUILayout.ObjectField("cannon", script.CannonCosmatic, typeof(CanonSkins), true);
//        }
//        else if (script.part == Set_Skin_Buy.Part.anchor)
//        {
//            script.anchorCosmatic = (AnchorCosmaticData)EditorGUILayout.ObjectField("anchor", script.anchorCosmatic, typeof(AnchorCosmaticData), true);
//        }
//    }
//}

//#endif
