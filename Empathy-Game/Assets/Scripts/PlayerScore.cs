using Unity.Netcode;

public struct PlayerScore : INetworkSerializable
{
    public int PersonalPoints;
    public int TeamPoints;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref PersonalPoints);
        serializer.SerializeValue(ref TeamPoints);
    }
}
