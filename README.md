# VuokratoimistotApp

**VuokratoimistotApp** is a WPF desktop application developed for a fictitious office rental company as a school project. The app allows users to manage office rental locations, services, and customers. Users can make reservations for office spaces for customers, generate invoices, view reports, and send invoices via email or save them as PDF files.

## Features

- **CRUD Operations**: Add, edit, or delete office locations, services, and customers.
- **Reservation System**: Make reservations for office spaces for customers and automatically generate an invoice.
- **Invoice Generation**: Create detailed invoices for reservations, viewable in-app or savable as a PDF.
- **Email Invoices**: Send invoices via email as PNG attachments.
- **Reporting**: View detailed reports on rentals and invoices.

## Installation

**Clone the repository:** `git clone https://github.com/lauri-sa/VuokratoimistotApp.git`

**Open the solution:** Use Visual Studio to open the `.sln` file in the project directory.

### Database Setup

The repository contains the necessary SQL scripts to create the required database. Before running the application, you'll need to:

- Create the database using the provided SQL scripts.

- Add an `App.config` file to the project and configure the connection string to point to your database.

Copy this into your `App.config` file:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
    <connectionStrings>
        <add name="Ohtu1" connectionString="Host=your-database-host;Port=3306;Database=your-database-name;Uid=your-username;pwd=your-password;" />
    </connectionStrings>
</configuration>
```
Replace the placeholders with your actual database information but keep the name as `Ohtu1`.

### Email Configuration

To enable email functionality, configure the email credentials in the code. Gmail users must use an **app-specific password** for SMTP.

#### Configuring Gmail SMTP

1. **Enable Two-Factor Authentication (2FA)** for your Gmail account if not already enabled. This can be done by visiting your [Google Account Security page](https://myaccount.google.com/security).

2. **Generate an app-specific password** from your [Google Account](https://myaccount.google.com/apppasswords).

3. **Replace the following placeholders** in the `SendEmail()` method in the `ViewInvoiceWindowViewModel.cs` file:
   - `AddYourEmailAddressHere`: Your Gmail address.
   - `AddYourCredentialsHere`: The app-specific password you generated.

After these steps, the application will be able to use Gmail's SMTP server.

#### Using Another SMTP Server (Optional)

You can use any email provider by modifying the SMTP settings in the `SendEmail()` method:

```csharp
var SmtpServer = new SmtpClient("your-smtp-server-address"); // Replace with your SMTP server
SmtpServer.Port = your-smtp-port; // Replace with the correct port (e.g., 465 for SSL, 587 for TLS)
SmtpServer.Credentials = new System.Net.NetworkCredential("your-email", "your-password"); // Your credentials
SmtpServer.EnableSsl = true; // Adjust SSL settings based on your server requirements
```

Consult your email provider's documentation for the correct server address, port, and SSL settings.

Once you have configured the SMTP settings correctly, the application will be able to use your chosen email provider.

### Running the Application

**Build the project:** Build the project using Visual Studio.

**Run the application:** Start the application directly from Visual Studio.

## Screenshots

![image](https://github.com/user-attachments/assets/3ff28ea2-82df-4a89-bcca-cf9eb7c201f3)
