using Active_Blog_Service_API.Dto;
using Active_Blog_Service_API.Services.Contracts;
using System.Security.Claims;

namespace Active_Blog_Service.Services.Contracts
{
    public interface IContactService : IScopedServiceMarker
    {
        Task<object> SendEmailServiceAsync(ClaimsPrincipal user, ContactDto sendMailViewModel);
    }
}