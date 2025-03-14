# URLShorteiningService

Welcome everyone! This is a RESTful API project built to shorten long URLs into more readable and easy to share ones. It also stores the times a URL is requested allowing to retreive this data as stats.

## Installation

1. Clone repository 
    ```
    git clone https://github.com/migmaram/URLShorteningService.git
    ```
2. Navigate to project’s directory
3. Using your preferred text editor set your database connection string in the `appsettings.json` file
    ```
    "ConnectionStrings": {
        "DbConnectionString" : "{your-database-connection-string}"
    }
    ```
4. Run migrations (make sure you have Entity Framework installed)
    1. VS console: `Update-database`
    2. VS Code terminal: `dotnet ef database update`
5. Run the application

## Usage

### **Create Short URL**

```json
POST /shorten
{
  "url": "https://www.example.com/some/long/url"
}
```

Response:

```json
201 Created

{
  "id": "1",
	"key": "{shortUrlKey}",
  "url": "{originalUrl}",
  "shortUrl": "{shortUrl}",
  "createdAt": "2021-09-01T12:00:00Z",
  "updatedAt": "2021-09-01T12:00:00Z"
}
```

### **Retrieve Original URL**

Retrieve the original URL from a short URL using the **`GET`** method

```json
GET /shorten/{shortUrlKey}
```

**Response**:

```json
200 OK

{
  "id": "1",
	"key": "{shortUrlKey}",
  "url": "{originalUrl}",
  "shortUrl": "{shortUrl}",
  "createdAt": "2021-09-01T12:00:00Z",
  "updatedAt": "2021-09-01T12:00:00Z"
}
```
Each time the original URL is retrieved the API adds a visit to the URL’s stats.

### **Update Short URL**

```json
PUT /shorten/abc123
{
  "url": "https://www.example.com/some/updated/url"
}
```

**Response**:

```json
200 OK

{
  "id": "1",
	"key": "{shortUrlKey}",
  "url": "{originalUrl}",
  "shortUrl": "{shortUrl}",
  "createdAt": "2021-09-01T12:00:00Z",
  "updatedAt": "2021-09-01T12:00:00Z"
}
```

### **Delete Short URL**

Delete an existing short URL using the **`DELETE`** method

```json
DELETE /shorten/{shortUrlKey}
```

**Response**: `204 No Content`

### **Get URL Statistics**

Get statistics for a short URL using the **`GET`** method

```json
GET /shorten/{ShortUrlKey}/stats
```

**Response**:

```json
200 OK

{
  "id": "1",
	"key": "{shortUrlKey}",
  "url": "{originalUrl}",
  "shortUrl": "{shortUrl}",
  "createdAt": "2021-09-01T12:00:00Z",
  "updatedAt": "2021-09-01T12:00:00Z".
  "accessCount": {timesAccessed}
}
```

### Statement

This project was made with practice in mind. 
Project's idea was grabbed from: [Developer Roadmaps - URL Shortening Service](https://roadmap.sh/projects/url-shortening-service) ✨

