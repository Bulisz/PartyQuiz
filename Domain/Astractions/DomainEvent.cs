using MediatR;

namespace Domain.Astractions;
public abstract record DomainEvent : INotification;