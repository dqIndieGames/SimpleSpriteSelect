



using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.U2D.Sprites;
using UnityEngine;
using Object = UnityEngine.Object;

public abstract class SpriteEditorSelectModuleBase : SpriteEditorModuleBase
{
    
    
    
    
    private float m_Zoom = 1.0f;
    protected List<SpriteRect> spriteRectList = new();
    protected List<Sprite> baseAllSpriteList = new();
    
    

    private Vector2 startDragPosition;
    protected bool isDragging { get; private set; }
    private Rect drawBoxRect;


    
    protected List<SpriteRect> selectionSpriteRectList = new();


    protected bool isSingleSpriteDragAndDrop { get; private set; }


    protected float postGUIY = 0;
    
    public override bool CanBeActivated()
    {
        return true;
    }

    public override void DoMainGUI()
    {
        
        
        m_Zoom = Handles.matrix.GetColumn(0).magnitude;



        if (!isSingleSpriteDragAndDrop)
        {
            if (isDragging || IsNeedDragBox())
            {
                DragBoxGUI();

                DrawSelectedSpriteGUI();
                
            }
        }
        else
        {
            DrawDragAndDropGUI();
        }

        DrawActiveObjDependSpriteRectGUI();
    }

    

    protected virtual bool IsNeedDragBox()
    {
        return true;
    }

    private void DrawDragAndDropGUI()
    {

        foreach (var spriteRect in spriteRectList)
        {
            // draw color.white , aplha 0.5f
            var oldColor = Handles.color;
            Handles.color = new Color(1, 1, 1, 0.5f);
            var spriteRectRect = spriteRect.rect;
            Handles.DrawSolidRectangleWithOutline(spriteRectRect, new Color(1, 1, 1, 0.1f), new Color(1, 1, 1, 1));
            Handles.color = oldColor;
        }
        
        Event currentEvent = Event.current;
        switch (currentEvent.type)
        {
            case EventType.MouseDown:

                if (currentEvent.button == 0)
                {
                    
                    var startPosition = currentEvent.mousePosition;
                    startPosition = Handles.inverseMatrix.MultiplyPoint(startPosition);
                    
                    // check startPosition is in spriteRect
                    foreach (var spriteRect in spriteRectList)
                    {
                        var spriteRectRect = spriteRect.rect;
                        if (spriteRectRect.Contains(startPosition))
                        {
                            var sprite = GetSprite(spriteRect);
                            
                            // start drag and drop
                            DragAndDrop.PrepareStartDrag();
                            DragAndDrop.objectReferences = new Object[] {sprite};
                            DragAndDrop.StartDrag("Drag Sprite");
                            
                            
                            
                            break;
                            
                        }
                    }
                    
                    
                    
                }
                
                
                break;
                
        }
        
        
    }
    
    
    
    

    private void DragBoxGUI()
    {
        
        // 保存当前的 GUI color, 以便在之后恢复
        Color oldColor = GUI.color;
        // 设置绘制颜色为绿色
        GUI.color = Color.green;

        // 开始绘制 GUI
        Handles.BeginGUI();

        // 获取当前事件
        Event currentEvent = Event.current;
      

        // 检查当前事件的类型
        switch (currentEvent.type)
        {
            case EventType.MouseDown:
                // 如果鼠标左键按下
                if (currentEvent.button == 0)
                {
                    // 存储起始拖动位置
                    startDragPosition = currentEvent.mousePosition;
                    startDragPosition = Handles.inverseMatrix.MultiplyPoint(startDragPosition);
                    
                    
                    // 标记开始拖动
                    isDragging = true;
                }
                break;

            case EventType.MouseDrag:
                // 如果开始拖动
                if (isDragging)
                {
                    var currentMousePosition = currentEvent.mousePosition;
                    currentMousePosition = Handles.inverseMatrix.MultiplyPoint(currentMousePosition);
                    
                    // 更新矩形的位置和大小
                    drawBoxRect.xMin = Mathf.Min(startDragPosition.x, currentMousePosition.x);
                    drawBoxRect.xMax = Mathf.Max(startDragPosition.x, currentMousePosition.x);
                    drawBoxRect.yMin = Mathf.Min(startDragPosition.y, currentMousePosition.y);
                    drawBoxRect.yMax = Mathf.Max(startDragPosition.y, currentMousePosition.y);

                    // 重绘场景视图，这将导致DoMainGUI重新执行
                    HandleUtility.Repaint();
                }
                break;

            case EventType.MouseUp:
                // 如果鼠标左键释放
                if (currentEvent.button == 0 && isDragging)
                {
                    if (drawBoxRect.size.magnitude > 0.1f)
                    {
                        OnMouseBtnUp();
                    }
                    else
                    {
                        var startPosition = Handles.inverseMatrix.MultiplyPoint(currentEvent.mousePosition);
                        foreach (var spriteRect in spriteRectList)
                        {
                            var spriteRectRect = spriteRect.rect;
                            if (spriteRectRect.Contains(startPosition))
                            {
                                var sprite = GetSprite(spriteRect);
                                OnSelectSprite(new List<Sprite> { sprite });
                                break;
                            }
                        }
                    }
                    drawBoxRect = new Rect();
                    // 完成拖动
                    isDragging = false;
                    // 可以在这里处理框选结束的逻辑
                    HandleUtility.Repaint();
                }
                break;
        }

        // 如果正在拖动，则绘制矩形
        if (isDragging)
        {
            // 使用DrawRect绘制矩形
            Handles.DrawSolidRectangleWithOutline(drawBoxRect, new Color(0, 1, 0, 0.25f), new Color(0, 1, 0, 1));
        }

        // 结束 GUI 绘制
        Handles.EndGUI();

        // 恢复原先的 GUI color
        GUI.color = oldColor;
        
        
    }


    private void DrawSelectedSpriteGUI()
    {
        if (!isDragging)
        {
            return;
        }


        foreach (var spriteRect in spriteRectList)
        {
            var spriteRectRect = spriteRect.rect;

            var curRect = drawBoxRect;
            
            // check curRect is overlap with spriteRectRect
            if (curRect.xMin > spriteRectRect.xMax || curRect.xMax < spriteRectRect.xMin ||
                curRect.yMin > spriteRectRect.yMax || curRect.yMax < spriteRectRect.yMin)
            {
                continue;
            }
            
            // draw red for spriteRectRect
            var oldColor = Handles.color;
            Handles.color = Color.red;
            
            Handles.DrawSolidRectangleWithOutline(spriteRectRect, new Color(1, 0, 0, 0.25f), new Color(1, 0, 0, 1));
            
            Handles.color = oldColor;



        }
        
        
    }


    protected virtual void OnMouseBtnUp()
    {

        var overlapRectList = new List<SpriteRect>();
        var curRect = drawBoxRect;
        foreach (var spriteRect in spriteRectList)
        {
            // check curRect is overlap with spriteRectRect
            if (curRect.xMin > spriteRect.rect.xMax || curRect.xMax < spriteRect.rect.xMin ||
                curRect.yMin > spriteRect.rect.yMax || curRect.yMax < spriteRect.rect.yMin)
            {
                continue;
            }
            
            overlapRectList.Add(spriteRect);
            
        }
        
        
        


        var spriteList = new List<Sprite>();


        foreach (var spriteRect in overlapRectList)
        {
            var sprite = GetSprite(spriteRect);
            if (sprite != null)
            {
                spriteList.Add(sprite);
            }
        }
        
        
        OnSelectSprite(spriteList);
        
        
    }

    protected Sprite GetSprite(SpriteRect spriteRect)
    {
        var findIndex = spriteRectList.FindIndex(x => x == spriteRect);
        return baseAllSpriteList[findIndex];
    }
    
    protected SpriteRect GetSpriteRect(Sprite sprite)
    {
        var findIndex = baseAllSpriteList.FindIndex(x => x == sprite);
        return spriteRectList[findIndex];
    }
    
    
    private List<Sprite> GetAllSprite()
    {
        var dataProvider = spriteEditor.GetDataProvider<ISpriteEditorDataProvider>();
        var assetPath = AssetDatabase.GetAssetPath(dataProvider.targetObject);

        var allAssetsArray = AssetDatabase.LoadAllAssetsAtPath(assetPath);

        var spriteList = new List<Sprite>();
        
        foreach (var obj in allAssetsArray)
        {
            var sprite = obj as Sprite;
            if (sprite != null)
            {
                spriteList.Add(sprite);
            }
        }
        


        return spriteList;
    }
    
    
    
    protected abstract void OnSelectSprite(List<Sprite> selectionSprite);


    protected virtual void DrawActiveObjDependSpriteRectGUI()
    {

        foreach (var spriteRect in selectionSpriteRectList)
        {
            // draw yellow wire cube
            var oldColor = Handles.color;
            Handles.color = Color.yellow;
            var spriteRectRect = spriteRect.rect;
            var size = spriteRectRect.size;
            size += new Vector2(2, 2);
            Handles.DrawWireCube(spriteRectRect.center, size);
            Handles.color = oldColor;
            
        }
        
    }
    
    
    public override void DoToolbarGUI(Rect drawArea)
    {

        DoToolbarGUIInternal(in drawArea);

    }

    protected Rect GetFromDrawRect(in Rect drawArea, ref float startXPos, float width)
    {
        var newRect = new Rect(drawArea.min + new Vector2(startXPos, 0), new Vector2(width, drawArea.height));
        startXPos += width;
        return newRect;
    }
    
    /// <summary>
    /// Use GetFromDrawRect control startXPos
    /// </summary>
    /// <param name="drawArea"></param>
    /// <returns></returns>
    protected virtual float DoToolbarGUIInternal(in Rect drawArea)
    {
        
        var startXPos = 0f;

        if (true)
        {
            var rect = GetFromDrawRect(in drawArea, ref startXPos, 100);
            
            
            var oldColor = GUI.color;
            GUI.color = isSingleSpriteDragAndDrop ? Color.green : Color.white;
            
            if (GUI.Button(rect, "SingleDragMode"))
            {
                isSingleSpriteDragAndDrop = !isSingleSpriteDragAndDrop;
            }
            
            GUI.color = oldColor;
            
          
            
            
        }

        
        return startXPos;
    }
    

    

    public override void OnModuleActivate()
    {
        
        
        selectionSpriteRectList.Clear();
        isDragging = false;
        
        var dataProvider = spriteEditor.GetDataProvider<ISpriteEditorDataProvider>();
        var spriteRects = dataProvider.GetSpriteRects();
        spriteRectList.Clear();
        spriteRectList.AddRange(spriteRects);

        var allSprite = GetAllSprite();
        baseAllSpriteList.Clear();
        foreach (var spriteRect in spriteRectList)
        {
            var sprite = allSprite.Find(x => x.name == spriteRect.name);
            baseAllSpriteList.Add(sprite);
        }
        

        Selection.selectionChanged -= OnSelectionChanged;
        Selection.selectionChanged += OnSelectionChanged;
    }

    private static List<Sprite> tempSpriteListForSelectionChanged = new List<Sprite>();
    protected virtual void OnSelectionChanged()
    {
        selectionSpriteRectList.Clear();
        
        var objects = Selection.objects;

        var tempSpriteList = tempSpriteListForSelectionChanged;
        tempSpriteList.Clear();

        foreach (var obj in objects)
        {
            if (obj is Texture)
            {
                continue;
            }

            if (obj is Sprite sprite)
            {
                AddToSelectionSpriteRect(sprite);
                continue;
            }
            
            
            tempSpriteList.Clear();
            // get obj all depend assets
            SerializedObjectHelperUtils.GetAllDependAssets(obj, tempSpriteList);

            
            
            foreach (var tempSprite in tempSpriteList)
            {
                AddToSelectionSpriteRect(tempSprite);
            }
            
            
        }
        
        

    }





    protected SpriteRect FindSpriteRect(Sprite sprite)
    {

        return spriteRectList.Find(x => x.name == sprite.name);

    }


    protected void AddToSelectionSpriteRect(Sprite sprite)
    {
        var findSpriteRect = FindSpriteRect(sprite);
        if (findSpriteRect != null)
        {
            // check contain
            if (!selectionSpriteRectList.Contains(findSpriteRect))
            {
                selectionSpriteRectList.Add(findSpriteRect);
            }
        }
    }
    
    public override void OnModuleDeactivate()
    {
        isDragging = false;
        Selection.selectionChanged -= OnSelectionChanged;
        selectionSpriteRectList.Clear();
    }

    public override void DoPostGUI()
    {
        if (GUILayout.Button("SelectSelectionSprite", GUILayout.MaxWidth(200)))
        {
            OnSelectSelectionSpriteBtnClick();
        }

        postGUIY = 30;
    }
    
    

    protected abstract void OnSelectSelectionSpriteBtnClick();

    public override bool ApplyRevert(bool apply)
    {
        return false;
    }
}
