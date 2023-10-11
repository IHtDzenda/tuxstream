using TuxStream.Core.Obj;
namespace TuxStream.Core
{
    public interface IProvider
    {
        string[] Languages();
         Task<Links>  Main(string _query, int _tmdbid);
    }


}
