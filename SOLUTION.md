# SOLUTION.md

## 📐 Architecture Overview

- **Backend API:**
  - Built with **.NET 8.0** using a layered structure: **Controllers → Services → Managers → DTOs/Models**.
  - Implements **JWT-based user authentication** (register, login, secured endpoints).
  - Uses custom **exception handling middleware** and JWT middleware.
  - Deployed on **Azure App Service** as a hosted Web API.

- **Database:**
  - **MySQL** database hosted on **Google Cloud Platform (GCP)**.
  - Structured relational tables for **recipes, ingredients, dietary tags, steps**, with bridging tables for many-to-many relations.
  - ERD diagram provided in the repository (`recipe-share-ERD.png`).

- **Frontend Web App:**
  - Developed in **React + TypeScript**.
  - Consumes the secured API for all CRUD operations.
  - Deployed using **Firebase Hosting** for fast static site delivery.
  - Includes **Firebase configuration and deploy scripts** to streamline deployment.

- **Testing & Benchmarking:**
  - Unit tests at the **Manager** layer to validate core logic.
  - Uses **BenchmarkDotNet** for measuring API performance, e.g., making 500 HTTP requests sequentially.

---

## ⚖️ Trade-offs and Design Decisions

- **Complex Ingredient Model vs Simpler Approach:**
  Typically, recipe apps just store ingredients as free-text fields (like how steps are handled). Here, ingredients are normalized into a separate table with a bridging table and quantities.
  👉 *This adds complexity intentionally, to demonstrate more realistic relational design and upserts, even though free-text would suffice in production for many simple apps.*

- **Email Verification Not Implemented:**
  User registration does not verify whether emails actually exist.
  👉 *Originally intended to integrate AWS SES for verification emails, but deferred due to time constraints.*

- **Database Hosted on GCP, API on Azure:**
  The choice to split hosting between **Google Cloud Platform** for the MySQL database and **Azure** for the API was driven by familiarity with each platform.
  👉 *In a real-world scenario, consolidating to a single cloud provider would reduce costs and complexity (e.g., lower latency, simplified networking).*

- **Secrets Management:**
  Connection strings and other sensitive config are **excluded from the public Git repository** via `.gitignore` and environment variables, to prevent accidental credential leaks.

---

## 🔒 Security & Monitoring Notes

- JWT tokens are used for authenticating and authorizing users.
- Custom middleware enforces token validation.
- Exception middleware standardizes error handling.
- No email verification yet — adding SES or an equivalent service would improve account integrity.
- Logs are basic; in production, a more robust observability stack (like Application Insights or ELK) is recommended.

---

## 💰 Cost Considerations

- **Azure App Service:** incurs standard hosting fees based on resource tier.
- **Google Cloud SQL:** charged for compute, storage, and network egress between GCP and Azure.
- **Firebase Hosting** (Web App) → free for light personal or demo use (Spark plan).
- In a production scenario, cheaper alternatives like **Hetzner Cloud VMs** or **Oracle Free Tier VMs** could dramatically lower costs for small-scale deployments.
- Ideally, unify API and DB hosting on a single provider to avoid cross-cloud charges and simplify ops.

---

## ✅ Summary

This solution shows a typical full-stack architecture:
- **.NET API** with JWT auth, CRUD operations, and clean separation of concerns.
- **React TypeScript client** with secure API consumption.
- Intentional complexity in relational data models to demonstrate real-world design.
- Cost and simplicity trade-offs acknowledged for future improvements.

