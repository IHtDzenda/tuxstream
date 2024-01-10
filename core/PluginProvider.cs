using TuxStream.Core.Obj;

namespace TuxStream.Core
{
    public interface IProvider
    {
        ProviderConfig Config { get; set; }
        Task<Links> Main(string _query, int _tmdbid);
    }
}
