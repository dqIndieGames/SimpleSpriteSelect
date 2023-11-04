using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpriteEditorSelectModuleNormal : SpriteEditorSelectModuleBase
{
    
    
    
    
    public override string moduleName => "SpriteSelectNormal";
    
    
    private List<Sprite> selectingSpriteList = new List<Sprite>();
    private DrawListSimpleHelperGUI_Sprite drawListSimpleHelperGUI = new DrawListSimpleHelperGUI_Sprite();


    public override void DoMainGUI()
    {
        base.DoMainGUI();


        DrawSelectionSpriteRect();

        DragAndDropAllSelectionSpriteList();
        
    }


    private void DragAndDropAllSelectionSpriteList()
    {

        if (isDragging)
        {
            return;
        }
        
        // check left mouse down
        if (!Event.current.isMouse || Event.current.button != 0 || Event.current.type != EventType.MouseDown)
        {
            return;
        }
        

        if (!IsMouseInSelectingRect())
        {
            return;
        }
        
        // start drag and drop
        DragAndDrop.PrepareStartDrag();
        DragAndDrop.objectReferences = selectingSpriteList.ToArray();
        DragAndDrop.StartDrag("SpriteEditorSelectModuleOdin");
        Event.current.Use();
        
        
        
        
    }

    private void DrawSelectionSpriteRect()
    {
        foreach (var sprite in selectingSpriteList)
        {
            var spriteRect = GetSpriteRect(sprite);
            
            Handles.color = Color.red;
            Handles.DrawWireCube(spriteRect.rect.center, spriteRect.rect.size);
            
        }
    }

    public override void DoPostGUI()
    {
        base.DoPostGUI();
        
        DrawSelectingSpriteListGUI();
    }
    
    


    private void DrawSelectingSpriteListGUI()
    {
        
        GUILayout.BeginArea(new Rect(0, 0, 200, 500));
        
        
        drawListSimpleHelperGUI.OnGUI();
        
        
        GUILayout.EndArea();
    }
    
    
    private bool IsMouseInSelectingRect()
    {
        // get mouse pos
        var mousePos = Event.current.mousePosition;
        mousePos = Handles.inverseMatrix.MultiplyPoint(mousePos);
        
        // check mouse Pos in spriteRect
        foreach (var sprite in selectingSpriteList)
        {
          
            var spriteRect = GetSpriteRect(sprite);
                
            // check mouse pos in spriteRect
            if (spriteRect.rect.Contains(mousePos))
            {
                return true;
            }
        }

        return false;
    }

    protected override bool IsNeedDragBox()
    {
        return !IsMouseInSelectingRect();
    }

    protected override void OnSelectSprite(List<Sprite> selectionSprite)
    {
     
        // check not contain and add
        foreach (var sprite in selectionSprite)
        {
            if (!selectingSpriteList.Contains(sprite))
            {
                selectingSpriteList.Add(sprite);
            }
        }
                
        
    }


    public override void OnModuleActivate()
    {
        base.OnModuleActivate();
        
        drawListSimpleHelperGUI.Init(selectingSpriteList, "Selecting Sprite List");
    }
}
