FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY GUI.csproj .
RUN dotnet restore GUI.csproj
COPY . .
RUN dotnet build GUI.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish GUI.csproj -c Release -o /app/publish

FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/publish/wwwroot .
COPY nginx.conf /etc/nginx/nginx.conf

# # Build stage
# FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
# WORKDIR /src

# # Copy and restore dependencies
# COPY . ./
# RUN dotnet restore

# # Build the project
# RUN dotnet publish -c Release -o /app/publish

# # Final stage
# FROM nginx:alpine AS final
# WORKDIR /usr/share/nginx/html
# COPY --from=build /app/publish/wwwroot .

# # Remove default nginx configuration
# RUN rm /etc/nginx/conf.d/default.conf
# COPY nginx.conf /etc/nginx/conf.d

# EXPOSE 8080
# ENTRYPOINT ["nginx", "-g", "daemon off;"]
