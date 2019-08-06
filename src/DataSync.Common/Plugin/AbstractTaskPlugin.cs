namespace DataSync.Common.Plugin
{
    public abstract class AbstractTaskPlugin : AbstractPlugin
    {
        public int Group { get; set; }

        public int Id { get; set; }
    }
}