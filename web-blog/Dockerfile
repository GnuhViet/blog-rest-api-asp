FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
#EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["web-blog.csproj", "web-blog/"]
RUN dotnet restore "web-blog/web-blog.csproj"

WORKDIR "/src/web-blog"
COPY . .

#RUN dotnet dev-certs https
RUN dotnet build "web-blog.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "web-blog.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
#COPY --from=build /root/.dotnet/corefx/cryptography/x509stores/my/* /root/.dotnet/corefx/cryptography/x509stores/my/
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "web-blog.dll"]
