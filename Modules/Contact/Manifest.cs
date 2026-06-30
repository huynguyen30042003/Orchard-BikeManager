using OrchardCore.Modules.Manifest;

[assembly: Module(
    Name = "Contact",

    Dependencies = new[]
    {
        "OrchardCore.Email",
        "OrchardCore.Email.Smtp"
    }
)]