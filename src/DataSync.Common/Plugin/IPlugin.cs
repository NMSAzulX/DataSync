using Microsoft.Extensions.Configuration;

namespace DataSync.Common.Plugin
{
    public interface IPlugin
    {
        string Name { get; }

        string Developer { get; }

        string Description { get; }

        IConfiguration Configuration { set; get; }

        IConfiguration JobConfiguration { set; get; }

        void Init();

        void Destroy();
    }
}