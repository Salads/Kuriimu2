using System;
using Kontract.Interfaces.Managers;
using Kontract.Interfaces.Plugins.Identifier;
using Kontract.Interfaces.Plugins.State;
using Kontract.Models;

namespace plugin_level5._3DS.Archives
{
    public class GuildPlugin : IFilePlugin
    {
        public Guid PluginId => Guid.Parse("e9d0e538-cec3-407d-8793-704e3dae24db");
        public PluginType PluginType => PluginType.Archive;
        public string[] FileExtensions => new[] {"*.fa"};
        public PluginMetadata Metadata { get; }

        public GuildPlugin()
        {
            Metadata=new PluginMetadata("GuildFA", "zavaro", "The main archive resource in Guild01");
        }

        public IPluginState CreatePluginState(IBaseFileManager fileManager)
        {
            return new GuildState();
        }
    }
}
