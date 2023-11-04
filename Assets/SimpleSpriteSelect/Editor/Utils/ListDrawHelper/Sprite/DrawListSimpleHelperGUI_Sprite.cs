


using UnityEditor;
using UnityEngine;

public class DrawItem_Sprite : IDrawItem<Sprite>
{
    public Sprite DrawItemGUI(Sprite data)
    {
        return EditorGUILayout.ObjectField(data, typeof(Sprite), false) as Sprite;
    }
}



public class DrawListSimpleHelperGUI_Sprite : DrawListSimpleHelperGUI<Sprite, DrawItem_Sprite>
{
        
}