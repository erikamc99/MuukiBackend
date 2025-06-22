# 🐔 Muuki - Backend

Este proyecto es el **backend** de la aplicación **Muuki**, desarrollada en **C#** y conectada a una base de datos **MongoDB**. 
Permite gestionar espacios, animales y condiciones ambientales, generando alertas y reportes en tiempo real para el bienestar animal.  
Está pensada para integrarse con sensores y servir datos a la [app móvil](https://github.com/erikamc99/MuukiFrontend) desarrollada en React Native.

---

## 🚀 Instalación y despliegue rápido

### 1. Clonar el repositorio

```bash
git clone https://github.com/erikamc99/MuukiBackend
cd MuukiBackend
````

### 2. Instalar dependencias

```bash
dotnet restore
```

### 3. Configurar variables de entorno

Crea un archivo `.env` con:

```
JWT_SECRET=...
MONGO_URI=mongodb://localhost:27017
```

> El archivo `.env` no se sube por seguridad.

### 4. Levantar MongoDB con Docker

Si no tienes MongoDB local:

```bash
docker run -d -p 27017:27017 --name mongodb -v mongo_data:/data/db mongo
```

### 5. Ejecutar el backend

```bash
dotnet build
dotnet run
```

O para exponer en tu red local (por ejemplo, para usar desde Expo Go):

```bash
dotnet run --urls "http://0.0.0.0:5098"
```

---

## 📱 Tecnologías principales

* **.NET 8 (C#)**
* **MongoDB**
* **Docker**
* **Swagger** (documentación interactiva)
* **Postman** (pruebas de API)
* **JWT** (autenticación)

---

## 📚 Documentación de la API

* Consulta todos los endpoints, parámetros y ejemplos de uso aquí:
  👉 [Wiki Lista y explicación de endpoints](https://github.com/erikamc99/MuukiBackend/wiki/Lista-y-explicaci%C3%B3n-de-endpoints)

* Colección de Postman lista para importar:
  👉 [Wiki Colección de Postman](https://github.com/erikamc99/MuukiBackend/wiki/Colecci%C3%B3n-de-Postman)

* Swagger UI (corriendo el backend):

  * [http://localhost:5098/swagger](http://localhost:5098/swagger)

---

## 🧪 Testing y pruebas

1. Importa la colección de Postman incluida para probar todos los endpoints con ejemplos ya cargados.
2. Usa Swagger UI para explorar y probar la API desde el navegador.

---

## 📦 Estructura del proyecto

```
FinalBackend/
├── Controllers/      # Lógica de endpoints
├── Models/           # Modelos y entidades de negocio
├── DTOs/             # Objetos de transferencia de datos
├── Services/         # Lógica de negocio y acceso a datos
├── Data/             # Conexión a MongoDB
├── Properties/       # Configuración
├── Utils/            # Utilidades reutilizables: helpers para JWT, manejo de tokens y excepciones personalizadas usadas en todo el proyecto
├── Program.cs        # Entry point
├── appsettings.json  # Configuración de la app
└── ...otros archivos
```

---

## ⚙️ Variables de entorno

* `JWT_SECRET`: Secreto para generación y validación de tokens JWT
* `MONGO_URI`: Cadena de conexión a MongoDB

---

## 🤝 Contribuciones

1. Haz fork de este repo
2. Crea una branch (`feature/nueva-funcionalidad`)
3. Abre un Pull Request

---

## 📝 Licencia

Este proyecto es de uso personal y educativo.
