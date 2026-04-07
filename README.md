<div align="center">
  
  # 🎓 SmartLearning LMS AI
  **An Intelligent, Scalable, and Secure E-Learning Management System**

  [![.NET](https://img.shields.io/badge/.NET_8.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)]()
  [![SQL Server](https://img.shields.io/badge/SQL_Server-CC292B?style=for-the-badge&logo=microsoft-sql-server&logoColor=white)]()
  [![Redis](https://img.shields.io/badge/Redis-DC382D?style=for-the-badge&logo=redis&logoColor=white)]()
  [![Swagger](https://img.shields.io/badge/Swagger-85EA2D?style=for-the-badge&logo=swagger&logoColor=black)]()
  [![Gemini AI](https://img.shields.io/badge/Google_Gemini-4285F4?style=for-the-badge&logo=google&logoColor=white)]()

  *Empowering education through Clean Architecture, High Performance, and Artificial Intelligence.*

</div>

---

## 🔗 Quick Links
* **Live API (Swagger):** http://elbanna-edu.runasp.net/index.html
* **API Documentation:** https://ds1veqw7gd.apidog.io/
* **GitHub Repository:** [https://lnkd.in/dG7iDd6M](https://lnkd.in/dG7iDd6M)

---

## 📖 About The Project

**SmartLearning LMS** is a comprehensive, production-ready backend API designed to power modern E-Learning platforms. It is not just a standard Web API; it represents a complete journey from **System Design to Production Deployment**. 

Built to solve real-world challenges in performance, security, and data management, this project adheres strictly to enterprise software engineering principles. It bridges the gap between instructors and students by providing a seamless, highly performant, and secure environment for course management, enrollment, and AI-assisted learning.

### 📸 System Overview (Swagger UI)
> 💡 **Note:** You can test the live endpoints directly using the Bearer Token!
<div align="center">
  
  ![Swagger Screenshot](./swagger.png)
  
</div>

---

## 🏗️ Architecture & System Design

The solution is engineered using **Clean Architecture (Onion Architecture)**, dividing the system into loosely coupled layers (`Domain`, `Application`, `Infrastructure`, `Presentation`). This ensures maximum scalability, maintainability, and Separation of Concerns.

* **Repository & Unit of Work Patterns:** Implemented to manage complex database transactions safely and maintain data integrity.
* **Dependency Injection (DI):** Heavily utilized to reduce tight coupling between services and improve testability.
* **DTOs & AutoMapper:** Completely isolated the Database Schema from the client by mapping Domain Entities to Data Transfer Objects securely.

---

## ✨ Core Features & Technical Highlights

### ⚡ Performance & Scalability (Database Optimization)
* **Distributed Caching (Redis):** Drastically reduced database hits and improved response times by caching frequently accessed, rarely changed data (e.g., Course Catalogs, Categories).
* **Smart Query Optimization:** Carefully utilized **Eager vs. Lazy Loading** depending on the specific endpoint's scenario to eliminate the N+1 query problem.
* **Advanced Pagination & Filtering:** Built a robust data retrieval system that returns standard `Metadata` (Current Page, Total Pages, etc.) to ensure the frontend renders massive datasets smoothly.

### 🛡️ Ironclad Security & Authentication
* **JWT & ASP.NET Core Identity:** Fully secured API with JSON Web Tokens.
* **Role-Based Access Control (RBAC):** Distinct privileges for `Admin`, `Instructor`, and `Student`.
* **Claim-Based Access:** Extracted sensitive user data (like User IDs) directly and securely from the token claims, entirely eliminating reliance on client-side inputs.

### 🤖 AI & Third-Party Integrations
* **Generative AI (Google Gemini 1.5 Flash):** Embedded smart capabilities into the platform to provide an interactive, next-level learning experience.
* **Secure Payment Gateway:** Seamless checkout and subscription process integrated via **Fawaterak**.
* **Automated Background Services:** Implemented **MailKit + SMTP** to handle asynchronous email notifications without blocking the main execution thread.

### 🛠️ API Quality & Error Handling
* **Global Exception Middleware:** Replaced scattered `try/catch` blocks with a centralized middleware that catches all unhandled exceptions and returns a standardized, sanitized JSON response.
* **Comprehensive Documentation:** Fully documented via **Swagger**, customized to support direct JWT authorization for seamless testing.

### 🌐 Production Deployment
* **Live on IIS:** Successfully deployed to a real production environment.
* **Infrastructure Challenges Resolved:** Handled advanced configurations including Server Routing, SSL & HTTPS enforcement, and IIS hosting models.
* **Secret Management:** Strictly protected sensitive configurations (JWT Secrets, AI API Keys, Database Strings) using secure Environment Variables.

---

## 💻 Technical Stack

* **Backend:** ASP.NET Core 8 Web API, C# 12
* **Database & ORM:** Microsoft SQL Server, Entity Framework Core (Code-First)
* **Caching:** Redis
* **Security:** JWT Bearer Authentication
* **Mapping:** AutoMapper
* **AI Provider:** Google Gemini API
* **Email Service:** MailKit / SMTP
* **Payment Integration:** Fawaterak API

---

## 🚀 Getting Started Locally

To get a local copy up and running, follow these simple steps.

### Prerequisites
* [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
* SQL Server (LocalDB or SSMS)
* Visual Studio 2022 or VS Code

### Installation

1. **Clone the repo:**
   ```sh
   git clone [https://lnkd.in/dG7iDd6M](https://lnkd.in/dG7iDd6M)
