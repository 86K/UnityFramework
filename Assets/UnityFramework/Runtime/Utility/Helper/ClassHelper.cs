using System;

public static class ClassHelper
{
    /// <summary>
    /// ��������⾲̬����Ĵ���ʹ��
    /// 1.ֵ���ͣ���������
    /// 2.�������ͣ������ֶ�ֵ������ֵ
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="t"></param>
    /// <returns></returns>
    public static T DeepCopy<T>(T t)
    {
        if (t == null)
            return default(T);

        var type = t.GetType();
        // ֵ���͵������������
        if (t is string || type.IsValueType)
            return t;

        // �����������Ͷ���
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
    /// �õ�����������ֶ���
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
    /// �õ�����������ֶ�ֵ
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
    /// �õ����������������
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
    /// �õ��������������ֵ
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
