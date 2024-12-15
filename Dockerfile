FROM mcr.microsoft.com/dotnet/sdk:8.0@sha256:35792ea4ad1db051981f62b313f1be3b46b1f45cadbaa3c288cd0d3056eefb83
WORKDIR /app
COPY blazor_frontend.csproj blazor_frontend.csproj
RUN dotnet restore blazor_frontend.csproj
COPY . .
RUN dotnet publish -c Release -o /output --no-restore --nologo