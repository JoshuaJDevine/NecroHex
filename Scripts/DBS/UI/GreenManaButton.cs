using System.Collections;
using System.Collections.Generic;
using DBS;
using DBS.Cauldron;
using DBS.Units;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Types = DBS.utils.Types;

public class GreenManaButton : MonoBehaviour
{
    public Image greenManaImage;

    private Necromancer necromancer;
    private Cauldron cauldron;
    public void OnClickEvent()
    {
        if (necromancer == null) necromancer = Game.Instance.necromancer;
        if (cauldron == null) cauldron = Game.Instance.cauldron;
        
                            
        //Use Mana
        if (necromancer.Mana <= 0) return;
        
        necromancer.Mana -= 1;
        int manaPercentage = necromancer.Mana * 100 / necromancer.MaxMana;
        
        Debug.Log(necromancer.Mana);
        Debug.Log(necromancer.MaxMana);
        Debug.Log(((float)manaPercentage / 100));
        greenManaImage.DOFillAmount(((float)manaPercentage / 100), 1f);
        cauldron.SetCauldronColor(Types.ManaColors.Green);
    }
}
