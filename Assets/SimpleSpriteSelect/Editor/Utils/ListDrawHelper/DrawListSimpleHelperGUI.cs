


using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;


public interface IDrawItem<T_Data>
{
    T_Data DrawItemGUI(T_Data data);
}

public abstract class DrawListSimpleHelperGUI<T_Data, U_DrawItemHelper>
    where U_DrawItemHelper : IDrawItem<T_Data>, new()
{

    public List<T_Data> list { get; private set; }
    public string title { get; private set; }

    private bool isShowAll = true;
    private Vector2 scrollPos = Vector2.zero;
    private U_DrawItemHelper drawItemHelper = new();
    

    public void Init(List<T_Data> list, string title)
    {
        
        this.list = list;
        
        this.title = title;
        
    }





    public void OnGUI()
    {
        
        GUILayout.BeginHorizontal();
        
        GUILayout.Label(title);
        
        // draw list count
        GUILayout.Label($"   Count: {list.Count}");
        
        GUILayout.EndHorizontal();


        DrawBaseControlBtn();


        if (isShowAll)
        {
            
            // draw scroll view and all list
            scrollPos = GUILayout.BeginScrollView(scrollPos, GUILayout.MaxHeight(500));
            GUILayout.BeginVertical();
            for (var i = 0; i < list.Count; i++)
            {
                GUILayout.BeginHorizontal();
                DrawItem(i);
                GUILayout.EndHorizontal();
            }

            GUILayout.EndVertical();
            GUILayout.EndScrollView();
            
            
            
        }
        
        
        
    }


    private void DrawItem(int index)
    {
        GUILayout.BeginHorizontal();
        
        
        
        
        list[index] = drawItemHelper.DrawItemGUI(list[index]);
        
        
        
        // add button
        if (GUILayout.Button("+"))
        {
            list.Insert(index + 1, default);
        }
        
        // remove button
        if (GUILayout.Button("-"))
        {
            list.RemoveAt(index);
        }
        
        
        
        
        
        
        GUILayout.EndHorizontal();
        
        
    }


    private void DrawBaseControlBtn()
    {

        GUILayout.BeginHorizontal();
        
        if (isShowAll)
        {
            if (GUILayout.Button("Hide"))
            {
                isShowAll = false;
            }
        }
        else
        {
            if (GUILayout.Button("Show"))
            {
                isShowAll = true;
            }
        }
        
        // add
        if (GUILayout.Button("+"))
        {
            list.Add(default);
        }
        
        // remove all
        if (GUILayout.Button("Clear"))
        {
            list.Clear();
        }
        
        GUILayout.EndHorizontal();
        
    }
    
    
    
    
    
}