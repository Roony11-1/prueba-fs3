# Librerias de .NET<br><br>

Microsoft.EntityFrameworkCore<br>
Microsoft.EntityFrameworkCore.Design<br>
Npgsql.EntityFrameworkCore.PostgreSQL<br>
Confluent.Kafka<br>
Microsoft.Extensions.Http.Resilience<br>

# Instalar dependencias:<br> dotnet add package<br><br>

dotnet tool install --global dotnet-ef<br><br>
# Migraciones<br>
- dotnet ef migrations add <Nombre><br>
- dotnet ef database update<br>


<br>git config --global user.email "Correo"<br>
git config --global user.name "Usuario"<br><br>

# Configurar el keycloak<br><br>
El primer paso es crear un "reino/realm"<br>
<b>El que usaremos de ejemplo es: "reino-neytan"</b><br><br>
El segundo paso es crear un cliente<br>
El que usaremos de ejemplo es: ricardito<br>
Solo dejaremos el Standard Flow activado<br><br>
En la tercera pagina dejaremos<br>
    - Root Url: http://localhost<br>
    - Home Url: http://localhost<br>
    - Valid Redirect Urls: http://localhost/*<br>
    - Valid Post Logout Redirect Urls: http://localhost/*<br>
    - Web Origins: *