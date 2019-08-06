namespace DataSync.Common.Plugin
{
    public interface IRecordReceiver
    {
        IRecord GetFromReader();

        void Shutdown();
    }
}