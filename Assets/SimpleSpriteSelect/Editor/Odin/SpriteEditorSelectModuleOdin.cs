

// used for odin

/*
using System;
using System.Collections.Generic;
using Odin.Custom;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public class SpriteEditorSelectModuleOdin : SpriteEditorSelectModuleBase
{

    #region 类定义

    [Serializable]
    public class SelectionData
    {

        
        public bool isSelect = false;
        
        
        public List<Sprite> spriteList = new();

        [Button]
        public void CopyToSpriteList()
        {
            Clipboard.Copy(spriteList);
        }

        [Button]
        public void CopyToSpriteArray()
        {
            Clipboard.Copy(spriteList.ToArray());
        }
        

    }
    
    
    
    [Serializable]
    public class ShowData
    {

        [FoldoutGroup("ShowData")]
        [ListDrawerSettings(ShowIndexLabels = true)]
        public List<SelectionData> selectionDataList = new();
    }
    

    #endregion
    
    public override string moduleName => "SpriteSelectOdin";

    private GUIDrawObjectNew<ShowData> showDataGUI = new();
    private ShowData showData = new();

    protected override void OnSelectSprite(List<Sprite> selectionSprite)
    {


        
        if (showData.selectionDataList.Count == 0)
        {
            // new
            var selectionData = new SelectionData();
            selectionData.isSelect = true;
            // add to showdata.selectionDataList
            showData.selectionDataList.Add(selectionData);
        }


        foreach (var selectionData in showData.selectionDataList)
        {
            if (selectionData.isSelect)
            {
                
                // if not contain then add
                foreach (var sprite in selectionSprite)
                {
                    if (!selectionData.spriteList.Contains(sprite))
                    {
                        selectionData.spriteList.Add(sprite);
                    }
                }
                
                
            }
        }
        
        
    }


    public override void DoMainGUI()
    {
        base.DoMainGUI();
        
        
        OnDrawSelectionSpriteGUI();

        OnDrawDragAndDropGUI();
    }


    private void OnDrawDragAndDropGUI()
    {

        if (isDragging)
        {
            return;
        }
        
        
        // mouse left button down
        if (Event.current.type != EventType.MouseDown || Event.current.button != 0)
        {
            return;
        }

        if (!IsMouseInSelectingRect())
        {
            return;
        }
        
        // start drag and drop
        DragAndDrop.PrepareStartDrag();
        DragAndDrop.objectReferences = GetAllSelectingSpriteList().ToArray();
        DragAndDrop.StartDrag("SpriteEditorSelectModuleOdin");
        Event.current.Use();
        
        
        
    }


    private List<Sprite> GetAllSelectingSpriteList()
    {
        var spriteList = new List<Sprite>();
        foreach (var selectionData in showData.selectionDataList)
        {
            if (!selectionData.isSelect)
            {
                continue;
            }
            spriteList.AddRange(selectionData.spriteList);
        }
        return spriteList;
    }
    

    private bool IsMouseInSelectingRect()
    {
        // get mouse pos
        var mousePos = Event.current.mousePosition;
        mousePos = Handles.inverseMatrix.MultiplyPoint(mousePos);
        
        // check mouse Pos in spriteRect
        foreach (var selectionData in showData.selectionDataList)
        {
            if (!selectionData.isSelect)
            {
                continue;
            }
            foreach (var sprite in selectionData.spriteList)
            {
                
                // sprite find spriterect
                var spriteRect = GetSpriteRect(sprite);
                
                // check mouse pos in spriteRect
                if (spriteRect.rect.Contains(mousePos))
                {
                    return true;
                }
            }
        }

        return false;
    }

    protected override bool IsNeedDragBox()
    {
        
        return !IsMouseInSelectingRect();

    }


    private void OnDrawSelectionSpriteGUI()
    {

        foreach (var selectionData in showData.selectionDataList)
        {
            if (!selectionData.isSelect)
            {
                continue;
            }
            
            
            
            foreach (var sprite in selectionData.spriteList)
            {
                
                // sprite find spriterect
                var spriteRect = spriteRectList.Find(x => x.name == sprite.name);
                if (spriteRect == null)
                {
                    continue;
                }
                
                Handles.color = Color.red;
                
                var size = spriteRect.rect.size;
                
                // draw red wire
                Handles.DrawWireCube(spriteRect.rect.center, size);
                
                
            }
            
            
            
            
        }
        
        
    }
    

    public override void DoPostGUI()
    {
        base.DoPostGUI();

        SirenixEditorGUI.BeginBox(GUILayout.MaxWidth(300), GUILayout.Height(500));
        
        EditorGUI.BeginChangeCheck();
        showDataGUI.Draw(showData);
        if (EditorGUI.EndChangeCheck())
        {
            GUIHelper.RequestRepaint();
        }
        
        
        SirenixEditorGUI.EndBox();
    }

    public override void OnModuleActivate()
    {
        base.OnModuleActivate();
        showData.selectionDataList.Clear();
    }

    public override void OnModuleDeactivate()
    {
        base.OnModuleDeactivate();
        showDataGUI.Dispose();
    }
}*/