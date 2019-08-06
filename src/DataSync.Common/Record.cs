namespace DataSync.Common
{
    public interface IRecord
    {
        int Count { get; }

        void AddColumn(Column column);

        void AddColumn(int i, Column column);
    }
}