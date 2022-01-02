using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ItemEffect
{
    public string itemName; // 아이템 이름 (키값)
    [Tooltip("HP, SP, DP, HUNGRY, THIRSTY, SATISFY만 가능합니다.")]
    public string[] part; // 효과가 적용되는 스탯
    public int[] num; // 수치

}



public class ItemEffectDatabase : MonoBehaviour
{
    [SerializeField]
    private ItemEffect[] itemEffects;   // 아이템 목록

    // 필요한 컴포넌트
    [SerializeField]
    private StatusController thePlayerStatus;
    [SerializeField]
    private WeaponManager theWeaponManager;
    [SerializeField]
    private SlotToolTip theSlotToolTip;


    private const string HP = "HP", SP = "SP", DP = "DP", HUNGRY = "HUNGRY", THIRSTY = "THIRSTY", SATISFY = "SATISFY";


    // 툴팁을 보여줌
    public void ShowToolTip(Item _item, Vector3 _pos)
    {
        theSlotToolTip.ShowToolTip(_item, _pos);
    }

    
    // 툴팁을 가림
    public void HideToolTip()
    {
        theSlotToolTip.HideToolTip();
    }


    // 아이템 사용
    public void UseItem(Item _item)
    {

        if (_item.itemType == Item.ItemType.Equipment)
        {
            // 장착
            StartCoroutine(theWeaponManager.ChangeWeaponCoroutine(_item.weaponType, _item.itemName));
        }
        // 사용 아이템인지 확인
        else if (_item.itemType == Item.ItemType.Used)
        {
            // 포션 종류 찾기
            for (int x = 0; x < itemEffects.Length; x++)
            {
                if (itemEffects[x].itemName == _item.itemName)
                {
                    // 효과들 읽기
                    for (int y = 0; y < itemEffects[x].part.Length; y++)
                    {
                        switch (itemEffects[x].part[y])
                        {
                            case HP:
                                thePlayerStatus.IncreaseHP(itemEffects[x].num[y]);
                                break;
                            case SP:
                                thePlayerStatus.IncreaseSP(itemEffects[x].num[y]);
                                break;
                            case DP:
                                thePlayerStatus.IncreaseDP(itemEffects[x].num[y]);
                                break;
                            case HUNGRY:
                                thePlayerStatus.IncreaseHungry(itemEffects[x].num[y]);
                                break;
                            case THIRSTY:
                                thePlayerStatus.IncreaseThirsty(itemEffects[x].num[y]);
                                break;
                            case SATISFY:
                                break;
                            default:
                                Debug.Log("잘못된 Status.\n" +
                                    "HP, SP, DP, HUNGRY, THIRSTY, SATISFY만 가능합니다.");
                                break;
                        }

                        // 1 개 소모
                        Debug.Log(_item.itemName + "을 사용했습니다");
                    }

                    return;
                }
            }

            Debug.Log(_item.name + "이 ItemEffectDatabase에 없습니다.");
        }
    }
}
