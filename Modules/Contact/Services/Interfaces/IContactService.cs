using Contact.Models;
using Microsoft.AspNetCore.Mvc;

namespace Contact.Services.Interfaces
{
    public interface IContactService
    {
        Task<List<ContactModel>> GetAllAsync();

        Task<ContactModel?> GetByIdAsync(
            Guid id
        );

        Task CreateAsync(
            ContactRequest request
        );

        Task ReplyAsync(
            ContactReplyRequest request
        );

        Task<bool> DeleteAsync(
            Guid id
        );

        Task SendAsync(Guid id);
    }
}