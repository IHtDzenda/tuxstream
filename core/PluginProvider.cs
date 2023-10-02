using TuxStream.Core.Obj;
namespace TuxStream.Core
{
    public interface IProvider
    {
        Links Main(string _query, int _tmdbid);
    }
}
