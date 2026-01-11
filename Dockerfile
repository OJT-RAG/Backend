FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

ENV NUGET_PACKAGES=/root/.nuget/packages
ENV DOTNET_NOLOGO=true
ENV DOTNET_SKIP_FIRST_TIME_EXPERIENCE=true

COPY NuGet.config .
COPY OJT_RAG.sln ./

COPY OJT_RAG.API/OJT_RAG.API.csproj OJT_RAG.API/
COPY OJT_RAG.Core/OJT_RAG.Core.csproj OJT_RAG.Core/
COPY OJT_RAG.Engine/OJT_RAG.Engine.csproj OJT_RAG.Engine/
COPY OJT_RAG.ModelViews/OJT_RAG.ModelViews.csproj OJT_RAG.ModelViews/
COPY OJT_RAG.Repositories/OJT_RAG.Repositories.csproj OJT_RAG.Repositories/
COPY OJT_RAG.Services/OJT_RAG.Services.csproj OJT_RAG.Services/
COPY OJT_RAG.WorkerService/OJT_RAG.WorkerService.csproj OJT_RAG.WorkerService/


RUN dotnet restore OJT_RAG.sln

COPY . .


RUN dotnet publish OJT_RAG.API/OJT_RAG.API.csproj \
    -c Release \
    -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "OJT_RAG.API.dll"]
