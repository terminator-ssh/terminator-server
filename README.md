# Basic Usage

Make sure Docker is installed.
```
docker run deeplerg/terminator --name terminator -v terminator-data:/app/data -p 8080:8080 -d
```

It is recommended to provide a `SecretKey` (see configuration section below) so that existing authentication tokens remain valid across restarts. 
If no key is provided, the app generates a new key (see console logs).

# Configuration

The app can be configured via environment variables or via /app/appsettings.json.

Note that environment variables use double underscore (`__`) as section delimeter.

### `AuthSettings`

Authentication options.

- `AuthSettings__SecretKey`: Base64 string for signing JWTs
- `AuthSettings__Issuer`: JWT Issuer claim. Default: `terminator-web`
- `AuthSettings__Audience`: The JWT Audience claim. Default: `terminator-client`
- `AuthSettings__ExpirationDays`: How long the JWT is valid. Default: `7`

### `ConnectionStrings`

Database connection options.

- `ConnectionStrings__Database`: SQLite connection string. Default: `Data Source=data/database.db`

### `DefaultAdmin`

If no admin users exist and this section is not empty, create an admin user with these options.

- `DefaultAdmin__Username`: Creates this admin on startup if no admins exist. Default: `admin`
- `DefaultAdmin__Password`: Password for the default admin. Default: `admin`
