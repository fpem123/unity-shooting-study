using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 생성 메뉴에 아이템 메뉴 추가
[CreateAssetMenu(fileName="New Item", menuName="New Item/item")]
public class Item : ScriptableObject    // 게임 오브젝트에 붙힐 필요가 없는 스크립트
{
    public string itemName;     // 아이템 이름
    [TextArea]
    public string itemDesc;     // 아이템의 설명
    
    public ItemType itemType;   // 아이템 타입
    public Sprite itemImage;    // 인벤토리 내 아이템 이미지
    public GameObject itemPrefab;   // 아이템의 프리팹, 내보낼 오브젝트

    public string weaponType;   // 무기 유형

    public enum ItemType 
    {
        Equipment,      // 장비
        Used,           // 소모품
        Ingredient,     // 재료
        ETC             // 기타
    }

}
