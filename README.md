# School Management ERP (Legacy)

> **Archived repository. This project represents an earlier generation of my work and continues to serve thousands of users in production. A modern successor is currently being developed using contemporary .NET enterprise architecture and distributed systems principles.**

---

## Overview

School Management ERP is a comprehensive school administration platform built to support the day-to-day operations of educational institutions.

Over the years, the platform evolved into a production system used daily by thousands of students, teachers, parents, and administrators. It has supported real-world school operations across multiple functional areas and remains in active use today.

Although this repository is no longer under active development, it remains publicly available as part of my engineering portfolio. It represents an important stage in my growth as a software engineer and documents the architectural approach that served the platform throughout its lifecycle.

---

## Why This Repository Is Archived

Software engineering is an evolving discipline, and so is the way we design systems.

As both the product and my understanding of enterprise software architecture matured, I reached the point where continuing to evolve the existing architecture no longer aligned with how I build software today.

Instead of continually extending the original application, I decided to redesign the platform around modern .NET architectural practices, including modularity, clearer domain boundaries, distributed services, and improved maintainability.

This repository therefore represents a successful production system that informed the design decisions behind its modern successor.

---

## Current Status

- ✅ Production system
- ✅ Continues to serve thousands of users
- ✅ Archived for historical and reference purposes
- 🚧 Modern successor currently under development

---

## Technology Stack

The following technologies and libraries are used within this project.

### Platform

- ASP.NET Web Forms
- .NET Framework 4.8
- VB.NET

### Integrations

- Google Classroom API
- Google Calendar API
- Paystack Payment Gateway

### Libraries

- Newtonsoft.Json
- log4net
- BouncyCastle.Cryptography

### Deployment

- Microsoft IIS
- Windows Server

---

## Functional Areas

The platform includes functionality covering major school administration workflows, including:

- Student Management
- Staff Management
- Admissions
- Academic Sessions
- Classes & Subjects
- Timetable Management
- Attendance
- Examination Management
- Result Processing
- School Fees & Payments
- Parent Portal
- Teacher Portal
- User Administration
- Reporting


---

## Architecture

This application follows a traditional enterprise architecture commonly used for large ASP.NET Framework applications.

The solution is organized around:

- Presentation Layer
- Business Logic
- Data Access
- Shared Utilities
- Third-party Integrations

While this architecture proved reliable in production, it also highlighted many of the challenges that come with maintaining large monolithic applications over time.

Those experiences directly influenced how I now approach enterprise software architecture.

---

## Lessons Learned

Building and maintaining a production system over several years taught me lessons that continue to influence every project I work on today.

Some of the most significant include:

- Design systems around business capabilities rather than technical folders.
- Keep responsibilities clearly separated.
- Reduce coupling between modules whenever possible.
- Build with long-term maintainability in mind.
- Consider operational concerns alongside feature development.

These lessons became the foundation for the architecture of the platform's successor.

---

## Why Keep This Repository Public?

I believe a portfolio should demonstrate progression rather than only polished end results.

This repository represents an earlier stage of my engineering journey, but it also represents a system that successfully reached production, supported real users, and solved real business problems.

Rather than removing it, I've chosen to archive it as a record of that progression and the experience gained from building and maintaining a long-lived enterprise application.

---

## Looking Forward

The next generation of this platform is currently being developed using a modern .NET ecosystem with a stronger emphasis on:

- ASP.NET Core
- ABP Framework
- Clean Architecture
- Domain-Driven Design (DDD)
- Microservices
- Event-Driven Communication
- RabbitMQ
- Redis
- Docker
- PostgreSQL
- API Gateway
- Multi-tenancy

The objective is not simply to modernize the technology stack, but to build a platform that is easier to scale, maintain, deploy, and evolve as business requirements grow.

---

## License

This project is licensed under the MIT License.

---

## Author

**Hope Odomene**

Backend Engineer | .NET Developer | Enterprise Software Engineer

GitHub: https://github.com/simplexhopee