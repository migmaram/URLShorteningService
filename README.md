### URLShorteiningService

This is a RESTful API built to shorten long URLs into more readable and easy to share URLs. 

### Installation

1. Clone repository
2. Navigate to project’s directory
3. Using your preferred text editor set your database connection string in the `appsettings.json` file
4. Run migrations (make sure you have Entity Framework installed)
    1. VS console: `Update-database`
    2. VS Code terminal: `dotnet ef database update`
5. Run te application

### Usage

### **Create Short URL**

```json
POST /shorten
{
  "url": "https://www.example.com/some/long/url"
}
```

## **Retrieve Original URL**

Retrieve the original URL from a short URL using the **`GET`** method

```json
GET /shorten/{shortUrlKey}
```

Each time the original URL is retrieved the API adds a visit to the URL’s stats.

## **Update Short URL**

```json
PUT /shorten/abc123
{
  "url": "https://www.example.com/some/updated/url"
}
```

### **Delete Short URL**

Delete an existing short URL using the **`DELETE`** method

```json
DELETE /shorten/{shortUrlKey}
```

### **Get URL Statistics**

Get statistics for a short URL using the **`GET`** method

```json
GET /shorten/{ShortUrlKey}/stats
```

**Statement**

The main goal of building this project was to practice:

- C# coding
- SOLID principles
- Entity Framework, code first.
- Dependency injecton, repository and unitOfWork desing patterns

✨I took the idea from: [Developer Roadmaps - URL Shortening Service](https://roadmap.sh/projects/url-shortening-service)
