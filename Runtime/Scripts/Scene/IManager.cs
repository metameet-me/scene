
using System.Threading.Tasks;

public interface IManager
{
    bool Initialized { get; set; }
    bool Ready { get; set; }

    Task<bool> Initialize();
}
