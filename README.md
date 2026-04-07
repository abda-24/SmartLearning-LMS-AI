<div align="center">
  
  # 🎓 SmartLearning LMS AI
  **An Intelligent, Scalable, and Secure E-Learning Management System**

  [![.NET](https://img.shields.io/badge/.NET_8.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)]()
  [![SQL Server](https://img.shields.io/badge/SQL_Server-CC292B?style=for-the-badge&logo=microsoft-sql-server&logoColor=white)]()
  [![Redis](https://img.shields.io/badge/Redis-DC382D?style=for-the-badge&logo=redis&logoColor=white)]()
  [![Swagger](https://img.shields.io/badge/Swagger-85EA2D?style=for-the-badge&logo=swagger&logoColor=black)]()
  [![Gemini AI](https://img.shields.io/badge/Google_Gemini-4285F4?style=for-the-badge&logo=google&logoColor=white)]()

  *Empowering education through Clean Architecture, High Performance, and Artificial Intelligence.*

  [Live Demo 🚀](http://elbanna-edu.runasp.net/) • [Report Bug 🐛](#) • [Request Feature 💡](#)

</div>

---

## 📖 About The Project

**SmartLearning LMS** is a comprehensive backend API designed to power modern E-Learning platforms. It bridges the gap between instructors and students by providing a seamless, highly performant, and secure environment for course management, enrollment, and AI-assisted learning.

Built from the ground up using **ASP.NET Core 8 Web API**, this project adheres to enterprise-level standards, utilizing **Clean Architecture (Onion)** to ensure the system is maintainable, testable, and strictly follows the Separation of Concerns principle.

### 📸 System Overview (Swagger UI)
> 💡 **Note:** You can test the live endpoints directly!
<div align="center">
  
  ![Swagger Screenshot](./swagger.png)
  
</div>

---

## ✨ Key Features & Business Value

* 🤖 **AI-Powered Learning (Google Gemini):** Integrated Generative AI to provide smart summaries, answer student queries, and assist in content generation.
* ⚡ **Ultra-Fast Performance (Redis):** Implemented Distributed Caching using Redis to drastically reduce database hits for frequently accessed data like course catalogs.
* 🛡️ **Ironclad Security (JWT & Identity):** Robust Role-Based Access Control (RBAC) separating features for **Admins, Instructors, and Students**.
* 💳 **Secure Payment Integration:** Seamless checkout process using the **Fawaterak** Payment Gateway.
* 📧 **Automated Background Services:** Integrated **MailKit** for asynchronous email notifications (Welcome emails, OTPs, Password Resets).
* 📊 **Smart Pagination & Filtering:** Optimized data retrieval ensuring the API remains blazing fast even with thousands of records.

---

## 🏗️ Architecture Design (Clean Architecture)

The solution is divided into loosely coupled layers to ensure maximum scalability:

1. **`Domain` Layer:** The core of the system. Contains Entities, Enums, and custom Exceptions. (No dependencies).
2. **`Application` Layer:** Contains Business Logic, Interfaces, DTOs, and **AutoMapper** profiles.
3. **`Infrastructure` Layer:** Handles external concerns. Contains the EF Core `DbContext`, Repositories, Unit of Work, and 3rd-party integrations (Email, Redis, AI).
4. **`Presentation` (WebAPI):** The entry point. Contains Controllers, Global Exception Handling Middleware, and DI configurations.

---

## 💻 Technical Stack

### Backend & Database
* **Framework:** ASP.NET Core 8 Web API
* **Language:** C# 12
* **ORM:** Entity Framework Core (Code-First)
* **Database:** Microsoft SQL Server
* **Caching:** Redis

### Security & Integrations
* **Authentication:** ASP.NET Core Identity + JWT Bearer Tokens
* **AI Provider:** Google Gemini 1.5 Flash API
* **Email Service:** MailKit / SMTP
* **Payment Gateway:** Fawaterak API

### Architecture Patterns Used
* Clean Architecture / Onion Architecture
* Repository & Unit of Work Patterns
* Dependency Injection (DI)
* Global Exception Handling Middleware

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
   git clone [https://github.com/your-username/SmartLearning-LMS-AI.git](https://github.com/your-username/SmartLearning-LMS-AI.git)
