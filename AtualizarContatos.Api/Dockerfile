FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["AtualizarContatos.Api/AtualizarContatos.Api.csproj", "AtualizarContatos.Api/"]
COPY ["AtualizarContatos.Domain/AtualizarContatos.Domain.csproj", "AtualizarContatos.Domain/"]
COPY ["AtualizarContatos.Infrastructure/AtualizarContatos.Infrastructure.csproj", "AtualizarContatos.Infrastructure/"]
COPY ["AtualizarContatos.Service/AtualizarContatos.Service.csproj", "AtualizarContatos.Service/"]
RUN dotnet restore "AtualizarContatos.Api/AtualizarContatos.Api.csproj"
COPY . .
WORKDIR "/src/AtualizarContatos.Api"
RUN dotnet build -c Release -o /app/build
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:80
ENTRYPOINT ["dotnet", "AtualizarContatos.Api.dll"]
