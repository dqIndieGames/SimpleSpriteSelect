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

        if (isSingleSpriteDragAndDrop)
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

    protected override void OnSelectSelectionSpriteBtnClick()
    {
        var targetList = selectionSpriteRectList;
        var baseList = selectingSpriteList;
        baseList.Clear();


        foreach (var spriteRect in targetList)
        {
            // get sprite
            var sprite = GetSprite(spriteRect);
            if (sprite == null)
            {
                continue;
            }
            // add to baselist
            baseList.Add(sprite);
        }
        
    }

    private void DrawSelectionSpriteRect()
    {
        foreach (var sprite in selectingSpriteList)
        {
            if (sprite == null)
            {
                continue;
            }
            
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

    protected override float DoToolbarGUIInternal(in Rect drawArea)
    {
        var startX = base.DoToolbarGUIInternal(in drawArea);

        if (true)
        {
            var rect = GetFromDrawRect(in drawArea, ref startX, 100);

            if (GUI.Button(rect, "SetToSelection"))
            {
                Selection.objects = selectingSpriteList.ToArray();
            }
            
        }
        
        
        return startX;
    }


    private void DrawSelectingSpriteListGUI()
    {
        
        GUILayout.BeginArea(new Rect(0, postGUIY, 200, 500));
        
        
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
            if (sprite == null)
            {
                continue;
            }
            
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
        selectingSpriteList.Clear();
        drawListSimpleHelperGUI.Init(selectingSpriteList, "Selecting Sprite List");
    }
}
