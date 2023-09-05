using Unity.Netcode;

public struct RoundStatistics : INetworkSerializable
{
    public int PersonalPoints;
    public int TeamPoints;
    public int UnPlayedCardsCount;
    public int unusedSlots;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref PersonalPoints);
        serializer.SerializeValue(ref TeamPoints);
        serializer.SerializeValue(ref UnPlayedCardsCount);
        serializer.SerializeValue(ref unusedSlots);
    }
}