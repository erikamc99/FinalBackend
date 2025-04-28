# 🐔 Muuki - Backend

Este proyecto es el **backend** de la aplicación **Muuki**, desarrollada en **C#** y conectada a una base de datos **MongoDB**. El objetivo es mejorar las condiciones y calidad de vida de los animales, en esta primera versión enfocado en gallinas. Se encarga de gestionar la información recibida de la app frontend y lanzar alertas si se detectan condiciones no óptimas.

El frontend correspondiente está disponible en el repositorio de la app móvil Muuki.

## 🚀 Instalación

### 1. Clonar el repositorio

```bash
git clone <url-del-repo-backend>
cd <FinalBackend>
```

### 2. Instalar dependencias

Usa el siguiente comando para instalar todas las dependencias necesarias del proyecto:

```bash
dotnet restore
```

### 3. Configurar las variables de entorno
Hay que crear el archivo .env para JWT y la base de datos.

### 4. Levantar la Base de Datos (MongoDB)

Si no tienes MongoDB instalado localmente, puedes levantarlo fácilmente usando Docker:

```bash
docker run -d -p 27017:27017 --name mongodb -v mongo_data:/data/db mongo
```

Esto hará que la base de datos esté disponible en:

```
mongodb://localhost:27017
```

### 4. Ejecutar el backend

Para compilar el proyecto:

```bash
dotnet build
```

Para correrlo normalmente en local:

```bash
dotnet run
```

Si necesitas que el backend sea accesible desde fuera de tu red local (por ejemplo, para conexiones móviles o uso de Expo Tunnel en el frontend), correlo así:

```bash
dotnet run --urls "http://0.0.0.0:5098"
```

> 💡 Asegúrate de que el puerto configurado coincida con el que uses en la app frontend (Muuki) en el archivo `api.js`.

## 📱 Tecnologías Utilizadas

- C# (.NET)
- MongoDB
- Docker
- Postman (pruebas de conexión)
- Swagger

## 📋 Funcionalidades principales

- Recepción y almacenamiento de datos ambientales
- Análisis de condiciones y generación de alertas
- API RESTful para comunicación con la app frontend

## ✨ Estado del proyecto

- Primera versión enfocada en gallinas 🐔
- Planeadas mejoras para soportar más especies y nuevas lógicas de análisis

## 📄 Licencia

Este proyecto es de uso personal para fines educativos.