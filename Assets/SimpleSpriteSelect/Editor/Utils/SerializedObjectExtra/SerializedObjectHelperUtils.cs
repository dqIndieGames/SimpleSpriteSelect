


using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class SerializedObjectHelperUtils
{

    private static HashSet<Object> findedHashSet = new HashSet<Object>();
    public static void GetAllDependAssets<T>(Object obj, List<T> resultList)
        where T : Object
    {

        findedHashSet.Clear();
        
        switch (obj)
        {
            case GameObject gameObject:
                GetAllDependAssetsInternal(gameObject, resultList);
                break;
            
            
            default:
                
                GetAllDependAssetsInternal(obj, resultList);
                
                break;
            
        }
        
    }



    private static void GetAllDependAssetsInternal<T>(GameObject gameObject, List<T> resultList)
        where T : Object
    {
        
        // check finded
        if (!findedHashSet.Add(gameObject))
        {
            return;
        }
        
        
        var componentArray = gameObject.GetComponentsInChildren<Component>(true);

        foreach (var component in componentArray)
        {
            if (component is T t)
            {
                 AddToResultList(t, resultList);
            }


            GetAllDependAssets(component, resultList);

        }
        
    }
    
    
    private static void GetAllDependAssetsInternal<T>(Object obj, List<T> resultList)
        where T : Object
    {
       
        var serializedObject = new SerializedObject(obj);
        
        
        
        // loop serializedObject
        var property = serializedObject.GetIterator();
        while (property.Next(true))
        {
            if (property.propertyType == SerializedPropertyType.ObjectReference)
            {
                if (property.objectReferenceValue == null)
                {
                    continue;
                }

                if (!findedHashSet.Add(property.objectReferenceValue))
                {
                    continue;
                }
                
                
                if (property.objectReferenceValue is T t)
                {
                    AddToResultList(t, resultList);
                }
                else
                {
                    GetAllDependAssets(property.objectReferenceValue, resultList);
                }
            }
        }
        
        
        
        
        serializedObject.Dispose();
        
    }



    private static void AddToResultList<T>(T t, List<T> resultList)
        where T : Object
    {

        // check contain and add
        if (!resultList.Contains(t))
        {
            resultList.Add(t);
        }
        
    }
    
    
    
    
    
    
        
}