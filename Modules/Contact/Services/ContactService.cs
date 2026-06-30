using BikeManagerV3.Contact.Data;
using Contact.Models;
using Contact.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrchardCore.Email;

namespace Contact.Services
{
    public class ContactService :
        IContactService
    {
        private readonly IEmailService _emailService;
        private readonly ContactDbContext _db;
        public ContactService(
            ContactDbContext db, IEmailService emailService
        )
        {
            _emailService = emailService;
            _db = db;
        }

        public async Task<List<ContactModel>>
            GetAllAsync()
        {
            return await _db.Contacts
                .OrderByDescending(
                    x => x.CreatedAt
                )
                .ToListAsync();
        }

        public async Task<ContactModel?>
            GetByIdAsync(
                Guid id
            )
        {
            return await _db.Contacts
                .FirstOrDefaultAsync(
                    x => x.Id == id
                );
        }

        public async Task CreateAsync(
            ContactRequest request
        )
        {
            var entity =
                new ContactModel
                {
                    Id = Guid.NewGuid(),

                    FullName =
                        request.FullName,

                    PhoneNumber =
                        request.PhoneNumber,

                    Email =
                        request.Email,

                    Address =
                        request.Address,

                    Title =
                        request.Title,

                    Content =
                        request.Content,

                    IsPublic =
                        request.IsPublic,

                    Status = 0,

                    CreatedAt =
                        DateTime.UtcNow
                };

            _db.Contacts.Add(
                entity
            );

            await _db.SaveChangesAsync();
        }

        public async Task ReplyAsync(
            ContactReplyRequest request
        )
        {
            var entity =
                await _db.Contacts
                .FirstOrDefaultAsync(
                    x => x.Id == request.Id
                );

            if (entity == null)
                return;

            entity.ReplyContent =
                request.ReplyContent;

            entity.Status =
                request.Status;

            entity.ReplyDate =
                DateTime.UtcNow;

            entity.UpdatedAt =
                DateTime.UtcNow;

            await _db.SaveChangesAsync();
        }
        public async Task<bool> DeleteAsync(
            Guid id
        )
        {
            var contact = await _db.Contacts.FindAsync(id);

            if (contact == null)
                return false;

            _db.Contacts.Remove(contact);

            await _db.SaveChangesAsync();

            return true;

        }

        public async Task SendAsync(
        Guid id)
        {

            var contact = await _db.Contacts.FindAsync(id);
            if (contact == null)
            {
                return;
            }
            var body = $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='UTF-8'>
</head>
<body style='font-family: Arial, Helvetica, sans-serif; line-height:1.6; color:#333;'>

    <div style='max-width:700px; margin:auto; border:1px solid #e5e7eb; border-radius:10px; overflow:hidden;'>

        <div style='background:#2563eb; color:white; padding:20px;'>
            <h2 style='margin:0;'>Phản hồi liên hệ</h2>
        </div>

        <div style='padding:24px;'>

            <p>Xin chào <strong>{contact.FullName}</strong>,</p>

            <p>Chúng tôi đã nhận được thông tin liên hệ của bạn.</p>

            <h3>Thông tin đã gửi</h3>

            <table style='width:100%; border-collapse:collapse;'>

                <tr>
                    <td style='padding:10px; border:1px solid #ddd; width:180px;'><strong>Họ tên</strong></td>
                    <td style='padding:10px; border:1px solid #ddd;'>{contact.FullName}</td>
                </tr>

                <tr>
                    <td style='padding:10px; border:1px solid #ddd;'><strong>Email</strong></td>
                    <td style='padding:10px; border:1px solid #ddd;'>{contact.Email}</td>
                </tr>

                <tr>
                    <td style='padding:10px; border:1px solid #ddd;'><strong>Số điện thoại</strong></td>
                    <td style='padding:10px; border:1px solid #ddd;'>{contact.PhoneNumber}</td>
                </tr>

                <tr>
                    <td style='padding:10px; border:1px solid #ddd;'><strong>Nội dung liên hệ</strong></td>
                    <td style='padding:10px; border:1px solid #ddd;'>{contact.Content}</td>
                </tr>

            </table>

            <h3 style='margin-top:30px;'>Nội dung phản hồi</h3>

            <div style='background:#f3f4f6; padding:16px; border-left:4px solid #2563eb; border-radius:6px;'>
                {contact.ReplyContent}
            </div>

            <p style='margin-top:30px;'>
                Trân trọng,<br/>
                <strong>Đội ngũ hỗ trợ khách hàng</strong>
            </p>

        </div>

    </div>

</body>
</html>
";
            await _emailService.SendAsync(
                contact.Email,
                contact.Title,
                body
            );
        }
    }
}