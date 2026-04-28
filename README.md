# neoDesk

**neoDesk** is a full-stack Helpdesk and Ticketing Management System designed to streamline IT support and issue tracking. Built with an Angular 17 frontend and .NET 8 Web API backend, it offers a seamless experience for managing software and hardware support requests.

One of the standout features of neoDesk is its **Automated Email-to-Ticket integration**, which actively polls an inbox directory for `.eml` files and automatically converts user emails into trackable support tickets.

## Features

* **Ticketing Lifecycle:** Create, manage, and track tickets through various statuses (`New`, `Assigned`, `Suspended`, `Solved`).
* **Categorization:** Classify issues efficiently (e.g., `Software`, `Hardware`).
* **Email-to-Ticket Polling:** A built-in background service (`EmailPollingService`) that parses raw `.eml` files and automatically generates tickets for the system.
* **Secure Authentication:** JWT-based user authentication and authorization with BCrypt password hashing.
* **Rich Text Editing:** Integrated with Quill (`ngx-quill`) for rich, formatted ticket descriptions and comments.
* **Interactive Dashboards:** Visualized statistics and metrics using Chart.js.
* **Modern UI:** Built using Tailwind CSS and DaisyUI components for a clean, responsive, and accessible user interface.

## Tech Stack

**Frontend (Client)**
* [Angular 17](https://angular.io/) (Standalone/Modern Architecture)
* [Tailwind CSS](https://tailwindcss.com/) & [DaisyUI](https://daisyui.com/) for styling
* [Chart.js](https://www.chartjs.org/) for data visualization
* [Quill](https://quilljs.com/) for rich text input
* [Lucide Icons](https://lucide.dev/) for iconography

**Backend (Server)**
* [.NET 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) / ASP.NET Core Web API
* [Entity Framework Core 8](https://learn.microsoft.com/en-us/ef/core/) (SQL Server Provider)
* [MimeKit](https://github.com/jstedfast/MimeKit) for robust email parsing
* JWT (JSON Web Tokens) for security
* Swagger / OpenAPI for API documentation

---

## Getting Started

### Prerequisites
Before you begin, ensure you have the following installed on your machine:
* [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
* [Node.js](https://nodejs.org/) (v18 or higher recommended)
* [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (Express, Developer, or Docker container)

### Installation & Setup

**1. Clone the repository**
```bash
git clone [https://github.com/your-username/neodesk.git](https://github.com/your-username/neodesk.git)
cd neodesk
```

**2. Setup the Database**
Ensure your SQL Server instance is running. Update the `DefaultConnection` string in the `neoDesk.Server/appsettings.json` file to point to your local SQL Server instance.

Navigate to the server directory and apply the Entity Framework migrations:
```bash
cd neoDesk.Server
dotnet ef database update
```

**3. Install Frontend Dependencies**
The backend is configured to use SPA Proxy, meaning it will attempt to launch the Angular app automatically, but you need to install the Node modules first.
```bash
cd ../neodesk.client
npm install
```

**4. Run the Application**
Navigate back to the server directory and run the .NET project. The SPA proxy setup in the `.csproj` file will automatically boot up the Angular frontend (`npm start`) alongside the .NET Web API.
```bash
cd ../neoDesk.Server
dotnet run
```
* **Frontend Application:** `https://localhost:4200`
* **Swagger API Documentation:** `https://localhost:<port>/swagger`

## Project Structure

* **`neoDesk.Server/`**: Contains the .NET 8 Web API project.
  * `Controllers/`: API endpoints for handling tickets, auth, and lookups.
  * `Data/`: Entity Framework `DbContext` and configurations.
  * `Models/`: Domain entities like `Ticket`, `User`, `Comment`, etc.
  * `Services/`: Business logic, including the `EmailPollingService` and `AuthService`.
  * `Emails/`: Contains `Inbox/` and `Processed/` directories for the local email-to-ticket polling system.
* **`neodesk.client/`**: Contains the Angular 17 frontend application.
  * `src/app/`: Angular components, services, models, and guards.
  * `tailwind.config.js`: Tailwind configuration utilizing the DaisyUI plugin.

## How the Email Polling Works
If you want to test the automated email-to-ticket creation locally:
1. Drop any valid `.eml` file into the `neoDesk.Server/Emails/Inbox/` directory.
2. The `EmailPollingService` runs every 5 seconds in the background.
3. It parses the sender, subject, and HTML/Text body.
4. A new Ticket is created in the database and the `.eml` file is moved to `neoDesk.Server/Emails/Processed/`.
