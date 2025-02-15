FROM mcr.microsoft.com/dotnet/sdk:8.0 
ENV ASPNETCORE_URLS=http://+:5000
WORKDIR /src
COPY ["Web/Web.csproj", "Web/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Infrastucture/Infrastucture.csproj", "Infrastucture/"]
RUN dotnet restore "./Web/Web.csproj"
COPY . .
WORKDIR "/src/Web"
EXPOSE 5000
RUN dotnet build "./Web.csproj" -c Release -o /app/build
