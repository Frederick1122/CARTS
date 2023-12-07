using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MCR_ChangeButtonNavigation {

    public void MCR_NewButtonNavigation(Button btn_01, Button btn_02_NewNavigation, string whichDirection)
    {
        if (whichDirection == "Left")
        {
            Navigation navigation = btn_01.navigation;
            navigation.selectOnLeft = btn_02_NewNavigation;
            btn_01.navigation = navigation;
        }
        if (whichDirection == "Right")
        {
            Navigation navigation = btn_01.navigation;
            navigation.selectOnRight = btn_02_NewNavigation;
            btn_01.navigation = navigation;
        }
        if (whichDirection == "Up")
        {
            Navigation navigation = btn_01.navigation;
            navigation.selectOnUp = btn_02_NewNavigation;
            btn_01.navigation = navigation;
        }
        if (whichDirection == "Down")
        {
            Navigation navigation = btn_01.navigation;
            navigation.selectOnDown = btn_02_NewNavigation;
            btn_01.navigation = navigation;
        }

    }
}
