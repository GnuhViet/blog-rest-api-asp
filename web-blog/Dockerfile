FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal AS base
WORKDIR /app
EXPOSE 7185

ENV ASPNETCORE_URLS=http://+:7185

RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
WORKDIR /src
COPY ./blog-rest-api-asp/web-blog/web-blog.csproj ./
RUN dotnet restore "web-blog.csproj"

COPY ./blog-rest-api-asp/web-blog/ ./
WORKDIR "/src/."
RUN dotnet build "web-blog.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "web-blog.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

RUN mkdir /app/uploads

ENTRYPOINT ["dotnet","web-blog.dll","--server.urls","https://+:7185"]
