# Recipe Share Application

## Architecture Diagram

![Architecture Diagram](./recipe-share-ERD.png)

---

## Setup Instructions

### Prerequisites

- [.NET Core SDK 8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [MySQL Server](https://dev.mysql.com/downloads/mysql/) installed locally
- [Node.js and npm](https://nodejs.org/en/download/) installed for the React app

---

### Backend (.NET Core 8.0 API)

1. Ensure MySQL Server is running locally.
2. Create and seed the database by running the following SQL scripts in order:
   - `SQLScripts/1 - user.sql`
   - `SQLScripts/2 - recipe.sql`
   - `SQLScripts/3 - seed.sql`
3. Update the connection string in `appsettings.Development.json` if needed to match your MySQL setup.
4. Run the backend API with:
   ```bash
   dotnet run --project ./RecipeShareWebApi/RecipeShareWebApi.csproj
   ```
5. The API will start on the configured port, `https://localhost:5015` and the Swagger is accessible at `http://localhost:5015/swagger/index.html`

---

### Frontend (React TypeScript Web App)

1. Navigate to the web app directory:
   ```bash
   cd recipe-share-web
   ```
2. Install dependencies:
   ```bash
   npm install
   ```
3. Run the development server:
   ```bash
   npm start
   ```
4. The app will open in your browser, `http://localhost:8080`.

---

## Notes

- Make sure both backend API and MySQL are running before starting the frontend to ensure data fetching works.
- You can customize ports and connection strings in the respective config files if needed.