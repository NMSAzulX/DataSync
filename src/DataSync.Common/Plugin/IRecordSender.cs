namespace DataSync.Common.Plugin
{
    public interface IRecordSender
    {
        IRecord CreateRecord();

        void SendToWriter(IRecord record);

        void Flush();

        void Terminate();

        void Shutdown();
    }
}