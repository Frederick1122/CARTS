using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Tools
{
    public static class ButtonExtension 
    {
        public static void MakeChosenVisual(this Button button)
        {
            var colorBlock = button.colors;
            colorBlock.normalColor = Color.white;
            colorBlock.selectedColor = Color.white;
            colorBlock.pressedColor = Color.white;
            colorBlock.disabledColor = Color.grey;
            button.colors = colorBlock;
        }

        public static void MakeChosenVisual(this Button button, 
            Color32 normalColor, Color32 selectedColor,
            Color32 pressedColor, Color32 disabledColor)
        {
            var colorBlock = button.colors;
            colorBlock.normalColor = normalColor;
            colorBlock.selectedColor = selectedColor;
            colorBlock.pressedColor = pressedColor;
            colorBlock.disabledColor = disabledColor;
            button.colors = colorBlock;
        }

        public static void MakeUnChosenVisual(this Button button)
        {
            var colorBlock = button.colors;
            colorBlock.normalColor = Color.grey;
            colorBlock.selectedColor = Color.grey;
            colorBlock.pressedColor = Color.grey;
            colorBlock.disabledColor = Color.grey;
            button.colors = colorBlock;
        }

        public static void MakeUnChosenVisual(this Button button,
            Color32 normalColor, Color32 selectedColor,
            Color32 pressedColor, Color32 disabledColor)
        {
            var colorBlock = button.colors;
            colorBlock.normalColor = normalColor;
            colorBlock.selectedColor = selectedColor;
            colorBlock.pressedColor = pressedColor;
            colorBlock.disabledColor = disabledColor;
            button.colors = colorBlock;
        }
    }
}
