FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
COPY blazor-frontend .
RUN dotnet publish -c Release -o /app/publish

FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=build /app/publish/wwwroot .
EXPOSE 80
ENTRYPOINT ["nginx", "-g", "daemon off;"]
