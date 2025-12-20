# =========================
# BUILD STAGE
# =========================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution
COPY OJT_RAG.sln ./

# Copy tất cả csproj (để cache restore)
COPY OJT_RAG.API/OJT_RAG.API.csproj OJT_RAG.API/
COPY OJT_RAG.Core/OJT_RAG.Core.csproj OJT_RAG.Core/
COPY OJT_RAG.Engine/OJT_RAG.Engine.csproj OJT_RAG.Engine/
COPY OJT_RAG.ModelViews/OJT_RAG.ModelViews.csproj OJT_RAG.ModelViews/
COPY OJT_RAG.Repositories/OJT_RAG.Repositories.csproj OJT_RAG.Repositories/
COPY OJT_RAG.Services/OJT_RAG.Services.csproj OJT_RAG.Services/
COPY OJT_RAG.WorkerService/OJT_RAG.WorkerService.csproj OJT_RAG.WorkerService/

# Restore cho toàn solution
RUN dotnet restore OJT_RAG.sln

# Copy toàn bộ source (giữ nguyên structure)
COPY . .

# Publish API
RUN dotnet publish OJT_RAG.API/OJT_RAG.API.csproj \
    -c Release \
    -o /app/publish \
    --no-restore

# =========================
# RUNTIME STAGE
# =========================
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "OJT_RAG.API.dll"]
