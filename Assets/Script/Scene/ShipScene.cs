using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipScene : BaseScene
{
    protected override UI_Scene createUIScene()
    {
        return Utils.GetOrAddComponent<UI_ShipScene>(this.gameObject);
    }

}
