namespace Rceptor.Core.ServiceProxy.Provider
{

    public interface IRoute
    {
        string Name { get; set; }
        int Order { get; set; }
        bool IsVariable { get; set; }
        bool InBody { get; set; }
        bool IsQuerySegment { get; set; }
    }

}