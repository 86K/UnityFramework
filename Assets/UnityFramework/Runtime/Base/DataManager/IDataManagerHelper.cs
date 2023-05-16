namespace UnityFramework.Runtime
{
    /// <summary>
    /// NOTICE
    /// 相对于UGF，我们希望数据管理器的辅助器实现者，能够管理数据的增删改查和数据内存释放
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDataManagerHelper<in T>
    {
        bool ReadData(T owner, object data);

        bool ReadData(T owner, byte[] data, int startIndex, int length);
        
        // NOTICE
        // 是否要增加增删改的接口，无论数据源是什么，都可以进行接口实现，且具有实际的意义
        // bool AddData(T owner, )

        void Release(T owner, object data);
    }
}