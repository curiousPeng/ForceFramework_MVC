namespace RedisTools.Serialization
{
    public interface ISerializer
    {
        byte[] Serialize(object item);

        object Deserialize(byte[] serializedObject);

        T Deserialize<T>(byte[] serializedObject);
    }
}
