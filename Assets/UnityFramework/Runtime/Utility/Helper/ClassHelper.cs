using System;

public static class ClassHelper
{
    /// <summary>
    /// 深拷贝，避免静态对象的错误使用
    /// 1.值类型：返回自身
    /// 2.引用类型：拷贝字段值和属性值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="t"></param>
    /// <returns></returns>
    public static T DeepCopy<T>(T t)
    {
        if (t == null)
            return default(T);

        var type = t.GetType();
        // 值类型的深拷贝返回自身
        if (t is string || type.IsValueType)
            return t;

        // 创建引用类型对象
        var instance = Activator.CreateInstance(type);
        var fields = type.GetFields();
        foreach(var field in fields)
        {
            field.SetValue(instance, field.GetValue(t));
        }

        var properties = type.GetProperties();
        foreach (var property in properties)
        {
            property.SetValue(instance, property.GetValue(t));
        }

        return (T)instance;
    }

    /// <summary>
    /// 得到类对象所有字段名
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="t"></param>
    /// <returns></returns>
    public static string[] GetFieldNames<T>(T t)
    {
        var fields = typeof(T).GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        string[] fieldNames = new string[fields.Length];
        for(int i = 0; i < fields.Length; i++)
        {
            fieldNames[i] = fields[i].Name;
        }

        return fieldNames;
    }

    /// <summary>
    /// 得到类对象所有字段值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="t"></param>
    /// <returns></returns>
    public static string[] GetFieldValues<T>(T t)
    {
        var fields = typeof(T).GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        string[] fieldValues = new string[fields.Length];
        for(int i = 0; i < fields.Length; i++)
        {
            object obj = fields[i].GetValue(t);
            fieldValues[i] = (obj == null) ? string.Empty : obj.ToString();
        }

        return fieldValues;
    }

    /// <summary>
    /// 得到类对象所有属性名
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="t"></param>
    /// <returns></returns>
    public static string[] GetPropertyNames<T>(T t)
    {
        var properties = typeof(T).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        string[] propertyNames = new string[properties.Length];
        for(int i = 0; i < properties.Length; i++)
        {
            propertyNames[i] = properties[i].Name;
        }

        return propertyNames;
    }

    /// <summary>
    /// 得到类对象所有属性值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="t"></param>
    /// <returns></returns>
    public static string[] GetPropertyValues<T>(T t)
    {
        var properties = typeof(T).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        string[] propertyValues = new string[properties.Length];
        for(int i = 0; i < properties.Length; i++)
        {
            object obj = properties[i].GetValue(t);
            propertyValues[i] = (obj == null) ? string.Empty : obj.ToString();
        }

        return propertyValues;
    }
}
