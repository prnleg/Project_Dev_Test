using Project_Dev_Test.Core.SharedKernel;

namespace Project_Dev_Test.Core.Interfaces
{
    public interface IDomainEventDispatcher
    {
        void Dispatch(BaseDomainEvent domainEvent);
    }
}