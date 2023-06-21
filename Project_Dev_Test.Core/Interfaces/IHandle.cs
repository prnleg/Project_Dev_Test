using Project_Dev_Test.Core.SharedKernel;

namespace Project_Dev_Test.Core.Interfaces
{
    public interface IHandle<T> where T : BaseDomainEvent
    {
        void Handle(T domainEvent);
    }
}