using Microsoft.Extensions.Configuration;

namespace DataSync.Common.Plugin
{
    public abstract class AbstractPlugin : IPlugin
    {
        public string Name
        {
            get
            {
                Validate.NotNull(Configuration, nameof(Configuration));
                return Configuration["name"];
            }
        }

        public string Developer
        {
            get
            {
                Validate.NotNull(Configuration, nameof(Configuration));
                return Configuration["developer"];
            }
        }

        public string Description
        {
            get
            {
                Validate.NotNull(Configuration, nameof(Configuration));
                return Configuration["description"];
            }
        }

        public abstract void Init();


        public abstract void Destroy();

        public virtual void PreCheck()
        {
        }

        public virtual void Prepare()
        {
        }

        public virtual void Post()
        {
        }

        public virtual void PreHandler(IConfiguration configuration)
        {
        }

        public virtual void PostHandler(IConfiguration configuration)
        {
        }

        public IConfiguration Configuration { set; get; }

        public IConfiguration JobConfiguration { set; get; }
    }
}