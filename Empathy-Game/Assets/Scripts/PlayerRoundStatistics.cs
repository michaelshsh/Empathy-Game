using Unity.Netcode;

public class PlayerRoundStatistics : INetworkSerializable
{
    public int PersonalPoints = 0;
    public int TeamPoints = 0;
    public int UnPlayedCardsCount = 0;
    public int unusedSlots = 0;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref PersonalPoints);
        serializer.SerializeValue(ref TeamPoints);
        serializer.SerializeValue(ref UnPlayedCardsCount);
        serializer.SerializeValue(ref unusedSlots);
    }
}