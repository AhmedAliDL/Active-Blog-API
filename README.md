# Active Blog Service – ASP.NET Core Web API

Active Blog Service is a full-featured blogging platform implemented as a **RESTful API** using **ASP.NET Core Web API**, **Entity Framework Core**, and **JWT authentication**. It provides endpoints for managing blog posts, comments, and user accounts inside a secure, role-based environment. The system is engineered with clean architecture principles to support scalable, maintainable, and testable development.

---

## Features

### Authentication & User Management

* Token-based authentication using **JWT**
* Role-based authorization (Admin / User)
* API endpoints for user registration, login, and profile management

### Blogging System

* Create, update, delete, and retrieve blog posts
* Rich content support: title, categories, images, and formatted text
* Metadata tracking (author, created date, updated date)
* Public endpoints for browsing posts with author attribution

### Comment System

* Authenticated users can comment on posts
* Comments linked to both posts and users
* Endpoints for creating and retrieving comments

### Admin Capabilities

* Manage users and roles through admin-only endpoints
* Assign or revoke user roles
* Administrative control over platform content

---

## Technology Stack

* **ASP.NET Core Web API**
* **Entity Framework Core (Code First)**
* **SQL Server**
* **JWT Authentication**
* **Swagger / OpenAPI** for endpoint documentation

---

## Database Diagram (ERD)

<img height="700" width="1200" src="Active Database Schema.png">

